using GBReplay.Replays;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay
{




  public class ReplayWriter : BinaryWriter, IDisposable
  {


      public StreamSHA1 Hasher { get; set; }
      public ReplayInfo ReplayFlags { get; set; }
      Stream _stream;
      public Stream RepStream
      {
          get { return _stream; }
      }
      public ReplayWriter(Stream stream, ReplayInfo flag)
          : base(stream)
      {
          _stream = stream;
          Hasher = new StreamSHA1();
          ReplayFlags = flag;
          Write((uint)flag);
      
      }

      public void Write(ReplayHeader header)
      {
          byte[] data = header.ToRaw();
          Write((ushort)data.Length); // 2 bytes
          Write(data, 0, data.Length); // n bytes
      }

      public void Write(ReplaySection sec, bool final)
      {

          Write((byte)sec.Type); // 1 byte
          Write(sec.Length); // 4 bytes
          Write(sec.Data, 0, (int)sec.Length); // n bytes

          if ((ReplayFlags & ReplayInfo.Signed) == ReplayInfo.Signed)
          {
              if (!final)
                  Hasher.Compute(sec.Data);
              else
                  Hasher.ComputeFinal(sec.Data);

          }

      }





      ~ReplayWriter()
      {
          this.Dispose();
      }


      // Flag: Has Dispose already been called? 
      bool disposed = false;

      // Public implementation of Dispose pattern callable by consumers. 
      public void Dispose()
      {
          Dispose(true);
          GC.SuppressFinalize(this);
      }

      // Protected implementation of Dispose pattern. 
      protected virtual void Dispose(bool disposing)
      {
          if (disposed)
              return;

          if (disposing)
          {
              // Free any other managed objects here. 
              //
              //this.Flush();

          }

          // Free any unmanaged objects here. 
          //
          disposed = true;
      }
  }
}
