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

   public class ReplayReader : BinaryReader, IDisposable
    {

         Stream RepStream { get; set; }
   
        public StreamSHA1 Hasher { get; set; }
        public ReplayInfo ReplayFlags { get; set; }


        public static Stream Decompress(Stream compressedStream)
        {
            var output = new MemoryStream();
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                zipStream.CopyTo(output);
                zipStream.Close();
                output.Position = 0;
                return output;
            }
        }

        public ReplayReader(Stream stream)
             : base(stream)
      {
          RepStream = this.BaseStream;
          Hasher = new StreamSHA1();
         
          ReplayFlags = (ReplayInfo)ReadUInt32();
           
      }

        public ReplayHeader ReadHeader()
        {
            int length =(int) ReadUInt16();
            byte[] header = ReadBytes(length);
           return new ReplayHeader(header);
        }
        public ReplaySection ReadSection()
        {
            byte setype = (byte)ReadByte();
            uint length = ReadUInt32();
            byte[] data = ReadBytes((int)length);
           // COMPUTE HASH
            if ((ReplayFlags & ReplayInfo.Signed) == ReplayInfo.Signed)
            {
                if (setype != (byte)SectionType.Keys)
                    Hasher.Compute(data);
                else
                    Hasher.ComputeFinal(data);
            }
            return new ReplaySection(data, (SectionType)setype);
        }

        public bool EOF()
        {
            return (RepStream.Position >= RepStream.Length );
        }





        ~ReplayReader()
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
              this.Close();
              base.Dispose();
          }

          // Free any unmanaged objects here. 
          //
          disposed = true;
      }
    }
}
