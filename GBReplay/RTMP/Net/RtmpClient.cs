using Complete;
using Complete.Threading;
using RtmpSharp.IO;
using RtmpSharp.Messaging;
using RtmpSharp.Messaging.Events;
using RtmpSharp.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RtmpSharp.Net
{
    public delegate void CExceptionCallback(object sender, Exception ex);
    public class RtmpClient
    {
        public event EventHandler Disconnected;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event CExceptionCallback CallbackException;

        public bool IsDisconnected { get { return !hasConnected || disconnectsFired != 0; } }

        public string ClientId;

        public bool NoDelay = true;
        public bool ExclusiveAddressUse;
        public int ReceiveTimeout;
        public int SendTimeout;
        public IPEndPoint LocalEndPoint;

        // by default, accept all certificates
        readonly Uri uri;
        readonly ObjectEncoding objectEncoding;
        readonly TaskCallbackManager<int, object> callbackManager;
        readonly SerializationContext serializationContext;
        readonly RemoteCertificateValidationCallback certificateValidator = (sender, certificate, chain, errors) => true;
        RtmpPacketWriter writer;
        RtmpPacketReader reader;
        Thread writerThread;
        Thread readerThread;

        int invokeId;
        bool hasConnected;

        volatile int disconnectsFired;

        public RtmpClient(Uri uri, SerializationContext serializationContext)
        {
            if (uri == null) throw new ArgumentNullException("uri");
            if (serializationContext == null) throw new ArgumentNullException("serializationContext");

            var scheme = uri.Scheme.ToLowerInvariant();
            if (scheme != "rtmp" && scheme != "rtmps")
                throw new ArgumentException("Only rtmp:// and rtmps:// connections are supported.");

            this.uri = uri;
            this.serializationContext = serializationContext;
            callbackManager = new TaskCallbackManager<int, object>();
        }

        public RtmpClient(Uri uri, SerializationContext serializationContext, ObjectEncoding objectEncoding) : this(uri, serializationContext)
        {
            this.objectEncoding = objectEncoding;
        }

        public RtmpClient(Uri uri, ObjectEncoding objectEncoding, SerializationContext serializationContext, RemoteCertificateValidationCallback certificateValidator)
            : this(uri, serializationContext, objectEncoding)
        {
            if (certificateValidator == null) throw new ArgumentNullException("certificateValidator");

            this.certificateValidator = certificateValidator;
        }








        void OnDisconnected(ExceptionalEventArgs e)
        {
            if (Interlocked.Increment(ref disconnectsFired) > 1)
                return;

            if (writer != null) writer.Continue = false;
            if (reader != null) reader.Continue = false;

            try { writerThread.Abort(); } catch { }
            try { readerThread.Abort(); } catch { }

            WrapCallback(() =>
            {
                if (Disconnected != null)
                    Disconnected(this, e);
            });

            WrapCallback(() => callbackManager.SetExceptionForAll(
                new ClientDisconnectedException(e.Description, e.Exception)));
        }

        Task<object> QueueCommandAsTask(Command command, int streamId, int messageStreamId, bool requireConnected = true)
        {
            if (requireConnected && IsDisconnected)
                return CreateExceptedTask(new ClientDisconnectedException("disconnected"));

            var task = callbackManager.Create(command.InvokeId);
            writer.Queue(command, streamId, messageStreamId);
            return task;
        }

        public void Close()
        {
            OnDisconnected(new ExceptionalEventArgs("disconnected"));
        }

        TcpClient CreateTcpClient()
        {
            if (LocalEndPoint == null)
                return new TcpClient();
            return new TcpClient(LocalEndPoint);
        }

     
        void OnPacketProcessorDisconnected(object sender, ExceptionalEventArgs args)
        {
            OnDisconnected(args);
        }

        void EventReceivedCallback(object sender, EventReceivedEventArgs e)
        {
            switch (e.Event.MessageType)
            {
                case MessageType.UserControlMessage:
                    var m = (UserControlMessage)e.Event;
                    if (m.EventType == UserControlMessageType.PingRequest)
                        WriteProtocolControlMessage(new UserControlMessage(UserControlMessageType.PingResponse, m.Values));
                    break;

                case MessageType.DataAmf3:
#if DEBUG
                    // Have no idea what the contents of these packets are.
                    // Study these packets if we receive them.
                    System.Diagnostics.Debugger.Break();
#endif
                    break;
                case MessageType.CommandAmf3:
                case MessageType.DataAmf0:
                case MessageType.CommandAmf0:
                    var command = (Command)e.Event;
                    var call = command.MethodCall;

                    var param = call.Parameters.Length == 1 ? call.Parameters[0] : call.Parameters;
                    if (call.Name == "_result")
                    {
                        // unwrap Flex class, if present
                        var ack = param as AcknowledgeMessage;
                        callbackManager.SetResult(command.InvokeId, ack != null ? ack.Body : param);
                    }
                    else if (call.Name == "_error")
                    {
                        // unwrap Flex class, if present
                        var error = param as ErrorMessage;
                        callbackManager.SetException(command.InvokeId, error != null ? new InvocationException(error) : new InvocationException());
                    }
                    else if (call.Name == "receive")
                    {
                        var message = param as AsyncMessage;
                        if (message == null)
                            break;

                        object subtopicObject;
                        message.Headers.TryGetValue(AsyncMessageHeaders.Subtopic, out subtopicObject);

                        var dsSubtopic = subtopicObject as string;
                        var clientId = message.ClientId;
                        var body = message.Body;

                        WrapCallback(() =>
                        {
                            if (MessageReceived != null)
                                MessageReceived(this, new MessageReceivedEventArgs(clientId, dsSubtopic, body));
                        });
                    }
                    else if (call.Name == "onstatus")
                    {
                        System.Diagnostics.Debug.Print("Received status.");
                    }
                    else
                    {
#if DEBUG
                        System.Diagnostics.Debug.Print("Unknown RTMP Command: " + call.Name);
                        System.Diagnostics.Debugger.Break();
#endif
                    }
                    break;
            }
        }



        








        public void SetChunkSize(int size)
        {
            WriteProtocolControlMessage(new ChunkSize(size));
        }

    

        void WriteProtocolControlMessage(RtmpEvent @event)
        {
            writer.Queue(@event, 2, 0);
        }

        int GetNextInvokeId()
        {
            // interlocked.increment wraps overflows
            return Interlocked.Increment(ref invokeId);
        }

        void WrapCallback(Action action)
        {
            try
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    if (CallbackException != null)
                        CallbackException(this, ex);
                }
            }
            catch (Exception unhandled)
            {
#if DEBUG && BREAK_ON_UNHANDLED_CALLBACK
                System.Diagnostics.Debug.Print("UNHANDLED EXCEPTION IN CALLBACK: {0}: {1} @ {2}", unhandled.GetType(), unhandled.Message, unhandled.StackTrace);
                System.Diagnostics.Debugger.Break();
#endif
            }
        }

        static Task<object> CreateExceptedTask(Exception exception)
        {
            var source = new TaskCompletionSource<object>();
            source.SetException(exception);
            return source.Task;
        }




        #region Handshake struct

        const int HandshakeRandomSize = 1528;

        // size for c0, c1, s1, s2 packets. c0 and s0 are 1 byte each.
        const int HandshakeSize = HandshakeRandomSize + 4 + 4;

        struct Handshake
        {
            // C0/S0 only
            public byte Version;

            // C1/S1/C2/S2
            public uint Time;
            // in C1/S1, MUST be zero. in C2/S2, time at which C1/S1 was read.
            public uint Time2;
            public byte[] Random;

            public Handshake Clone()
            {
                return new Handshake()
                {
                    Version = Version,
                    Time = Time,
                    Time2 = Time2,
                    Random = Random
                };
            }


        }
        #endregion
    }
}