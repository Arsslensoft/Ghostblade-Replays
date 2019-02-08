/*
    Icmp classes for C#
		Version: 1.0		Date: 2002/04/15
	updated by Olivier Griffet 2006/02/23

    Copyright © 2002, The KPD-Team
    All rights reserved.
    http://www.mentalis.org/

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

    - Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer. 

    - Neither the name of the KPD-Team, nor the names of its contributors
       may be used to endorse or promote products derived from this
       software without specific prior written permission. 
  

 UPDATED BY OLIVIER GRIFFET :
 - To make the Tracert command available and to defined the time to live (TTL)
 parameter into the ping 
 - add the option weight packet

 
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

// <summary>The System.Net namespace provides a simple programming interface for many of the protocols used on networks today.</summary>
namespace GhostLib.Network
{


    /// <summary>
    /// Summary description for icmpRequest.
    /// </summary>
    public class IcmpRequest
    {
        public StringBuilder Cns = new StringBuilder();
        public IcmpRequest(ToolsCommandRequest myRequest)
        {

            //The request will be a ping
            if (myRequest.myCommandType == CommandType.ping)
            {
                Icmp myIcmp = new Icmp(Dns.Resolve(myRequest.host).AddressList[0]);
                Cns.AppendLine("Pinging " + myRequest.host + " [" + Dns.Resolve(myRequest.host).AddressList[0].ToString() + "] with " + myRequest.weightPacket.ToString() + " bytes of data:");
                Cns.AppendLine();

                while (myRequest.nbrEcho > 0)
                {
                    try
                    {
                        PingRequestInformation myResult = myIcmp.Ping(myRequest.timeout, myRequest.timeToLiveMax, myRequest.weightPacket);
                        if (myResult.duration.Equals(TimeSpan.MaxValue))  Cns.AppendLine("Request timed out.");
                        else
                        {
                            Cns.AppendLine("Reply from " + Dns.Resolve(myRequest.host).AddressList[0].ToString() + ": bytes=" + myRequest.weightPacket.ToString() + " time=" + Math.Round(myResult.duration.TotalMilliseconds).ToString() + "ms " + "TTL " + myResult.TTL.ToString());
                        }

                        if (1000 - myResult.duration.TotalMilliseconds > 0)
                        {
                            Thread.Sleep(myRequest.periodUpdate - (int)myResult.duration.TotalMilliseconds);
                        }
                    }
                    catch
                    {
                        Cns.AppendLine("Network error.");
                    }
                    myRequest.nbrEcho--;
                }
            }//end of ping


            //The request will be a tracert
            else if (myRequest.myCommandType == CommandType.tracert)
            {
                Icmp myIcmp = new Icmp(Dns.Resolve(myRequest.host).AddressList[0]);
                Cns.AppendLine("Tracing route to " + myRequest.host + " [" + Dns.Resolve(myRequest.host).AddressList[0].ToString() + "]\r\nover a maximum of " + myRequest.timeToLiveMax.ToString() + " hops:");
                Cns.AppendLine();
                bool find = false;
                bool timeOut = false;
                int nbrTTL = 1;
                int i = 1;
                int nbrError = 0;
                string IPToFind = Dns.Resolve(myRequest.host).AddressList[0].ToString();
                while (!find & !timeOut)
                {
                    try
                    {
                        nbrTTL++;

                        PingRequestInformation myResult = myIcmp.Ping(myRequest.timeout, nbrTTL);

                        //Do it again for the first stop to get a correct value
                        if (nbrTTL == 2) myResult = myIcmp.Ping(myRequest.timeout, nbrTTL);


                        if (myResult.duration.Equals(TimeSpan.MaxValue))
                        {
                            throw new Exception("Time out");
                        }
                        else
                        {
                            string hostName = "";
                            try
                            {
                                hostName = Dns.GetHostByAddress(myResult.IPEndPoint).HostName.ToString() + " ";
                            }
                            catch { }

                             Cns.AppendLine("   ".Substring(0, 3 - i.ToString().Length) + i.ToString() + "\t" + Convert.ToInt32(Math.Max(1, myResult.duration.TotalMilliseconds)).ToString() + " ms\t" + hostName + "[" + Dns.Resolve(myResult.IPEndPoint).AddressList[0].ToString() + "]");
                            nbrError = 0;
                            i++;
                        }
                        if (myResult.IPEndPoint == IPToFind)
                        {
                            find = true;
                        }
                        if (nbrTTL >= myRequest.timeToLiveMax)
                        {
                            timeOut = true;
                        }
                    }
                    catch
                    {
                        nbrError++;
                        if (nbrError > 10) timeOut = true;
                    }


                }
                 Cns.AppendLine("\r\nTrace complete.");

            }

        }


    }




    public class ToolsCommandRequest
    {


        //Default value
        public int periodUpdate = 1000;
        public CommandType myCommandType = CommandType.tracert;
        public string host = "127.0.0.1";
        public int weightPacket = 32;
        public string sessionID;
        public int nbrEcho = 4;
        public int timeout = 1000;
        public int timeToLiveMax = 128;

        public ToolsCommandRequest(string host, string sessionID, CommandType myCommandType, int nbrEcho, int weightPacket, int periodUpdate, int timeout, int timeToLiveMax)
        {
            if (weightPacket < 1) weightPacket = 1;
            this.periodUpdate = periodUpdate;
            this.myCommandType = myCommandType;
            this.host = host;
            this.weightPacket = weightPacket;
            this.sessionID = sessionID;
            this.nbrEcho = nbrEcho;
            this.timeout = timeout;
            this.timeToLiveMax = timeToLiveMax;
        }

    }


    public enum CommandType
    {
        ping,
        tracert
    }
	/// <summary>
	/// Implements the ICMP messaging service.
	/// </summary>
	/// <remarks>Currently, the implementation only supports the echo message (better known as 'ping').</remarks>
	public class Icmp 
	{
		/// <summary>
		/// Initializes an instance of the Icmp class.
		/// </summary>
		/// <param name="host">The host that will be used to communicate with.</param>
		public Icmp(IPAddress host) 
		{
			Host = host;
		}
		/// <summary>
		/// Generates the Echo message to send.
		/// </summary>
		/// <returns>An array of bytes that represents the ICMP echo message to send.</returns>
		protected byte[] GetEchoMessageBuffer(int weight) 
		{
			if (weight <1) weight = 1;
			EchoMessage message = new EchoMessage();
			message.Type = 8;	// ICMP echo
			message.Data = new Byte[1 * weight];
			for (int i = 0; i < weight; i++) 
			{
			message.Data[i] = 32;	// Send spaces
			}
			message.CheckSum = message.GetChecksum();
			return message.GetObjectBytes();
		}
		/// <summary>
		/// Initiates an ICMP ping with a timeout of 1000 milliseconds.
		/// </summary>
		/// <exception cref="SocketException">There was an error while communicating with the remote server.</exception>
		/// <returns>A TimeSpan object that holds the time it takes for a packet to travel to the remote server and back. A value of TimeSpan.MaxValue indicates a timeout.</returns>
		/// <example>
		/// The following example will ping the server www.mentalis.org ten times and print the results in the Console.
		/// <c>
		/// <pre>
		/// Icmp icmp = new Icmp(Dns.Resolve("www.mentalis.org").AddressList[0]);
		/// for (int i = 0; i &lt; 10; i++) {
		/// 	 Cns.AppendLine(icmp.Ping().TotalMilliseconds);
		/// }
		/// </pre>
		/// </c>
		/// </example>
		public PingRequestInformation Ping() 
		{
			return Ping(1000);
		}



		public PingRequestInformation Ping(int timeout)
		{
			//By default TTL = 128
			return Ping(timeout,128);

		}

		public PingRequestInformation Ping(int timeout, int TTL)
		{
			return Ping(timeout, TTL, 32);
		}

		/// <summary>
		/// Initiates an ICMP ping.
		/// </summary>
		/// <param name="timeout">Specifies the timeout in milliseconds. If this value is set to Timeout.Infinite, the method will never time out.</param>
		/// <returns>A TimeSpan object that holds the time it takes for a packet to travel to the remote server and back. A value of TimeSpan.MaxValue indicates a timeout.</returns>
		public PingRequestInformation Ping(int timeout, int TTL, int weight) 
		{
			TimeSpan ret;
			EndPoint remoteEP = new IPEndPoint(Host, 0);
			int returnTTL = 128;
			ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
			// Get the ICMP message
			byte [] buffer = GetEchoMessageBuffer(weight);

			/****** Display the ping packet
			foreach (byte b in buffer)
			{
				Console.Write(b.ToString() + "-");
				
			}
			 Cns.AppendLine();
			********************************/

			// Set the timeout timer
			HasTimedOut = false;
			PingTimeOut = new Timer(new TimerCallback(PingTimedOut), null, timeout, Timeout.Infinite);
			// Get the current time
			StartTime = DateTime.Now;
			// Send the ICMP message and receive the reply
			try 
			{
				//Set the Time to live option (usefull for tracert)
				ClientSocket.SetSocketOption(SocketOptionLevel.IP,SocketOptionName.IpTimeToLive,TTL);


				if (ClientSocket.SendTo(buffer, remoteEP) <= 0)
					throw new SocketException();

				buffer = new byte[buffer.Length + 20];
				
				if (ClientSocket.ReceiveFrom(buffer, ref remoteEP) <= 0)
					throw new SocketException();

				//Get the TTL from the return packet
				returnTTL = Convert.ToInt32(buffer[8].ToString());
			} 
			catch (SocketException e) 
			{
				if (HasTimedOut)
					return new PingRequestInformation(returnTTL,Host.ToString(),remoteEP.ToString().Substring(0,remoteEP.ToString().Length -2),TimeSpan.MaxValue);
				else
					throw e;
			} 
			finally 
			{
				// Close the socket
				ClientSocket.Close();
				ClientSocket = null;
				// destroy the timer
				PingTimeOut.Change(Timeout.Infinite, Timeout.Infinite);
				PingTimeOut.Dispose();
				PingTimeOut = null;
			}
			// Get the time
			ret = DateTime.Now.Subtract(StartTime);
			return new PingRequestInformation(returnTTL,Host.ToString(),remoteEP.ToString().Substring(0,remoteEP.ToString().Length -2),ret);;
		}
		/// <summary>
		/// Called when the ping method times out.
		/// </summary>
		/// <param name="state">The source of the event. This is an object containing application-specific information relevant to the methods invoked by this delegate, or a null reference (Nothing in Visual Basic).</param>
		protected void PingTimedOut(object state) 
		{
			HasTimedOut = true;
			// Close the socket (this will result in a throw of a SocketException in the Ping method)
			if (ClientSocket != null)
				ClientSocket.Close();
		}		
		

									  
		/// <summary>
		/// Gets or sets the address of the remote host to communicate with.
		/// </summary>
		/// <value>An IPAddress instance that specifies the address of the remote host to communicate with.</value>
		/// <exception cref="ArgumentNullException">The specified value is null (Nothing in VB.NET).</exception>
		protected IPAddress Host 
		{
			get 
			{
				return m_Host;
			}
			set 
			{
				if (value == null)
					throw new ArgumentNullException();
				m_Host = value;
			}
		}
		/// <summary>
		/// Gets or sets the Socket that is used to communicate with the remote server.
		/// </summary>
		/// <value>A Socket object that is used to communicate with the remote server.</value>
		protected Socket ClientSocket 
		{
			get 
			{
				return m_ClientSocket;
			}
			set 
			{
				m_ClientSocket = value;
			}
		}
		/// <summary>
		/// Gets or sets the time when the ping began/begins.
		/// </summary>
		/// <value>A DateTime object that specifies when the ping began.</value>
		protected DateTime StartTime 
		{
			get 
			{
				return m_StartTime;
			}
			set 
			{
				m_StartTime = value;
			}
		}
		/// <summary>
		/// Gets or sets the timer that triggers the PingTimedOut method.
		/// </summary>
		/// <value>A Timer object that triggers the PingTimedOut method.</value>
		protected Timer PingTimeOut 
		{
			get 
			{
				return m_PingTimeOut;
			}
			set 
			{
				m_PingTimeOut = value;
			}
		}
		/// <summary>
		/// Gets or sets a value that indicates whether the ping has timed out or not.
		/// </summary>
		/// <value>A boolean value that indicates whether the ping has timed out or not.</value>
		protected bool HasTimedOut 
		{
			get 
			{
				return m_HasTimedOut;
			}
			set 
			{
				m_HasTimedOut = value;
			}
		}
		/// <summary>
		/// Releases all the resources.
		/// </summary>
		~Icmp() 
		{
			if (ClientSocket != null)
				ClientSocket.Close();
		}
		// Private variables
		/// <summary>Stores the value of the Host property.</summary>
		private IPAddress m_Host;
		/// <summary>Stores the value of the ClientSocket property.</summary>
		private Socket m_ClientSocket;
		/// <summary>Stores the value of the StartTime property.</summary>
		private DateTime m_StartTime;
		/// <summary>Stores the value of the PingTimeOut property.</summary>
		private Timer m_PingTimeOut;
		/// <summary>Stores the value of the HasTimedOut property.</summary>
		private bool m_HasTimedOut;
	}
	
	/// <summary>
	/// Defines a base ICMP message
	/// </summary>
	public abstract class IcmpMessage 
	{
		/// <summary>
		/// Initializes a new IcmpMessage instance.
		/// </summary>
		public IcmpMessage() {}
		/// <summary>
		/// Gets or sets the type of the message.
		/// </summary>
		/// <value>A byte that specifies the type of the message.</value>
		public byte Type 
		{
			get 
			{
				return m_Type;
			}
			set 
			{
				m_Type = value;
			}
		}
		/// <summary>
		/// Gets or sets the message code.
		/// </summary>
		/// <value>A byte that specifies the message code.</value>
		public byte Code 
		{
			get 
			{
				return m_Code;
			}
			set 
			{
				m_Code = value;
			}
		}
		/// <summary>
		/// Gets or sets the chacksum for this message.
		/// </summary>
		/// <value>An unsigned short that holds the checksum of this message.</value>
		public ushort CheckSum 
		{
			get 
			{
				return m_CheckSum;
			}
			set 
			{
				m_CheckSum = value;
			}
		}
		/// <summary>
		/// Serializes the object into an array of bytes.
		/// </summary>
		/// <returns>An array of bytes that represents the ICMP message.</returns>
		public virtual byte [] GetObjectBytes() 
		{
			byte [] ret = new byte[4];
			Array.Copy(BitConverter.GetBytes(Type), 0, ret, 0, 1);
			Array.Copy(BitConverter.GetBytes(Code), 0, ret, 1, 1);
			Array.Copy(BitConverter.GetBytes(CheckSum), 0, ret, 2, 2);
			return ret;
		}
		/// <summary>
		/// Calculates the checksum of this message.
		/// </summary>
		/// <returns>An unsigned short that holds the checksum of this ICMP message.</returns>
		public ushort GetChecksum() 
		{
			ulong sum = 0;
			byte [] bytes = GetObjectBytes();
			// Sum all the words together, adding the final byte if size is odd
			int i;
			for (i = 0; i < bytes.Length - 1; i += 2) 
			{
				sum += BitConverter.ToUInt16(bytes, i);
			}
			if (i != bytes.Length)
				sum += bytes[i];
			// Do a little shuffling
			sum = (sum >> 16) + (sum & 0xFFFF);
			sum += (sum >> 16);
			return (ushort)(~sum);
		}
		// Private variables
		/// <summary>Holds the value of the Type property.</summary>
		private byte m_Type = 0;
		/// <summary>Holds the value of the Code property.</summary>
		private byte m_Code = 0;
		/// <summary>Holds the value of the CheckSum property.</summary>
		private ushort m_CheckSum = 0;
	}
	
	/// <summary>
	/// Defines an ICMP message with an ID and a sequence number.
	/// </summary>
	public class InformationMessage : IcmpMessage 
	{
		/// <summary>
		/// Initializes a new InformationMessage instance.
		/// </summary>
		public InformationMessage() {}
		/// <summary>
		/// Gets or sets the identification number.
		/// </summary>
		/// <value>An unsigned short that holds the identification number of this message.</value>
		public ushort Identifier 
		{
			get 
			{
				return m_Identifier;
			}
			set 
			{
				m_Identifier = value;
			}
		}
		/// <summary>
		/// Gets or sets the sequence number.
		/// </summary>
		/// <value>An unsigned short that holds the sequence number of this message.</value>
		public ushort SequenceNumber 
		{
			get 
			{
				return m_SequenceNumber;
			}
			set 
			{
				m_SequenceNumber = value;
			}
		}
		/// <summary>
		/// Serializes the object into an array of bytes.
		/// </summary>
		/// <returns>An array of bytes that represents the ICMP message.</returns>
		public override byte [] GetObjectBytes() 
		{
			byte [] ret = new byte[8];
			Array.Copy(base.GetObjectBytes(), 0, ret, 0, 4);
			Array.Copy(BitConverter.GetBytes(Identifier), 0, ret, 4, 2);
			Array.Copy(BitConverter.GetBytes(SequenceNumber), 0, ret, 6, 2);
			return ret;
		}
		// Private variables
		/// <summary>Holds the value of the Identifier property.</summary>
		private ushort m_Identifier = 0;
		/// <summary>Holds the value of the SequenceNumber property.</summary>
		private ushort m_SequenceNumber = 0;
	}
	
	/// <summary>
	/// Defines an echo ICMP message.
	/// </summary>
	public class EchoMessage : InformationMessage 
	{
		/// <summary>
		/// Initializes a new EchoMessage instance.
		/// </summary>
		public EchoMessage() {}
		/// <summary>
		/// Gets or sets the data of this message.
		/// </summary>
		/// <value>An array of bytes that represents the data of this message.</value>
		public byte[] Data 
		{
			get 
			{
				return m_Data;
			}
			set 
			{
				m_Data = value;
			}
		}
		/// <summary>
		/// Serializes the object into an array of bytes.
		/// </summary>
		/// <returns>An array of bytes that represents the ICMP message.</returns>
		public override byte [] GetObjectBytes() 
		{
			int length = 8;
			if (Data != null)
				length += Data.Length;
			byte [] ret = new byte[length];
			Array.Copy(base.GetObjectBytes(), 0, ret, 0, 8);
			if (Data != null)
				Array.Copy(Data, 0, ret, 8, Data.Length);
			return ret;
		}
		// Private variables
		/// <summary>Holds the value of the Data property.</summary>
		private byte[] m_Data;
	}

	public class PingRequestInformation
	{
		public  int TTL;
		public  string IPFrom;
		public  string IPEndPoint;
		public TimeSpan duration;
		public PingRequestInformation(int TTL, string IPFrom, string IPEndPoint, TimeSpan duration)
		{
			this.TTL = TTL;
			this.IPFrom = IPFrom;
			this.IPEndPoint = IPEndPoint;
			this.duration = duration;
		}
	}

}
