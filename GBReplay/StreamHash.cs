using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays
{
    public class StreamSHA1
    {
        public string BytesToStr(byte[] bytes)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
                str.AppendFormat("{0:X2}", bytes[i]);

            return str.ToString();
        }
        public SHA1Managed Hasher { get; set; }
        public StreamSHA1()
        {
            Hasher = new SHA1Managed();
        }
        public void HashBlock(byte[] data)
        {
            using (SHA1Managed sha = new SHA1Managed())
            {
              Console.WriteLine(data.Length+" : "+BytesToStr(  sha.ComputeHash(data)));
            }
        }
        public void Compute(byte data)
        {
            Compute(new byte[1] { data });
        }
        public void Compute(byte[] buffer)
        {
          //  HashBlock(buffer);
            Hasher.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
        }
        public void Compute(byte[] buffer, int offset, int length)
        {
            Hasher.TransformBlock(buffer, offset, length, buffer, offset);
        }
        public void ComputeFinal(byte[] buffer, int offset, int length)
        {
            Hasher.TransformFinalBlock(buffer, offset, length);
        }
        public void ComputeFinal(byte[] buffer)
        {
          //  HashBlock(buffer);
            Hasher.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        public string ComputeHashString()
        {
            return BytesToStr(Hasher.Hash);
        }
        public byte[] ComputeHash()
        {
            return Hasher.Hash;
        }
    }
}
