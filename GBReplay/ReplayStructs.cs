using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays
{
    [Flags]
   public enum ReplayInfo : uint
    {
        // FORMAT
          StandardFormat = 0 ,
          CustomFormat= 1,
    // RECORDER
          Ghostblade =2,
          LolReplay =4,
          BaronReplays =8,
          UnknownRecorder=16 ,
        // Signature
         Signed =32,
        // PoV
        Spectator = 64,
        Player = 128,
        Summoner = 256,
        PBE = 512
        
         
        
    }
    public enum ReadMode : byte
    {
        HeaderOnly = 0,
        AllExceptData = 1,
        DataOnly = 2,
        All = 3
    }
    public enum SectionType : byte
    {
        EndOfGameStats =0,
        MatchDetails = 1,
        MetaData = 2,
        ChunkInfo = 3,
        Chunks = 4,
        Keys = 5,
        Informations = 6,
        Ressources = 7,
        Custom = 8,
        Signature = 9

    }

    /// <summary>
    /// Binary Raw Entry with 8 bits size
    /// </summary>
    public struct RawEntry8
    {
        byte _length;
        public byte Length
        {
            get { return _length; }
        }

        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
        }

        public RawEntry8(byte[] data)
        {
            if (data.Length <= 255)
            {
                _data = data;

                _length = (byte)data.Length;
            }
            else throw new ArgumentOutOfRangeException("RawEntry8 must not exceed 255 bytes size");
        }

        public byte[] ToRaw()
        {
            return BinaryUtil.Append(new byte[1] { _length }, _data);
        }

    }

    /// <summary>
    /// Binary Raw Entry with 16 bits size
    /// </summary>
    public struct RawEntry16
    {
        ushort _length;
        public ushort Length
        {
            get { return _length; }
        }

        byte[] _data;
        public byte[] Data
        {
            get { return _data; }
        }

        public RawEntry16(byte[] data)
        {
            if (data.Length <= ushort.MaxValue)
            {
                _data = data;

                _length = (ushort)data.Length;
            }
            else throw new ArgumentOutOfRangeException("RawEntry16 must not exceed 65535 bytes size");
        }
        public byte[] ToRaw()
        {
            return BinaryUtil.Append(BitConverter.GetBytes(_length), _data);
        }

    }

    /// <summary>
    /// Binary Raw Entry with 24 bits size
    /// </summary>
    public struct RawEntry24
    {
        public byte[] Data;
        public uint Length;
        public RawEntry24(byte[] data)
        {
            Data = data;
            Length = (uint)data.Length;
        }
        public RawEntry24(GameData g)
        {
            Data = g.Data;
            Length = (uint)g.Data.Length;
        }

        public byte[] ToData()
        {
            return BinaryUtil.Append(BitConverter.GetBytes(Data.Length), Data, 3, Data.Length);
        }
    }

    /// <summary>
    /// Binary Raw Entry with 32 bits size
    /// </summary>
    public struct RawEntry32
    {
        public byte[] Data;
        public uint Length;
        public RawEntry32(byte[] data)
        {
            Data = data;
            Length = (uint)data.Length;
        }
        public byte[] ToData()
        {
            return BinaryUtil.Append(BitConverter.GetBytes(Length), Data);
        }
    }

    /// <summary>
    /// Binary Raw Entry with 64 bits size
    /// </summary>
    public struct RawEntry64
    {
        public byte[] Data;
        public ulong Length;
        public RawEntry64(byte[] data)
        {
            Data = data;
            Length = (uint)data.Length;
        }
        public byte[] ToData()
        {
            return BinaryUtil.Append(BitConverter.GetBytes(Length), Data);
        }
    }





    public struct Version64
    {
        public ushort Major { get; set; }
        public ushort Minor { get; set; }
        public ushort Build { get; set; }
        public ushort Revision { get; set; }

        public override string ToString()
        {
            return Major + "." + Minor + "." + Build + "." + Revision;

        }
        public static Version64 Parse(long version)
        {
            Version64 v = new Version64();
            byte[] vb = BitConverter.GetBytes(version);
            v.Major = BitConverter.ToUInt16(vb, 0);
            v.Minor = BitConverter.ToUInt16(vb, 2);
            v.Build = BitConverter.ToUInt16(vb, 4);
            v.Revision = BitConverter.ToUInt16(vb, 6);
            return v;
        }
        public static Version64 Parse(byte[] vb)
        {
            Version64 v = new Version64();

            v.Major = BitConverter.ToUInt16(vb, 0);
            v.Minor = BitConverter.ToUInt16(vb, 2);
            v.Build = BitConverter.ToUInt16(vb, 4);
            v.Revision = BitConverter.ToUInt16(vb, 6);
            return v;
        }
        public static Version64 Parse(string version)
        {
            Version64 v = new Version64();
            string[] s = version.Split('.');
            v.Major = ushort.Parse(s[0]);
            v.Minor = ushort.Parse(s[1]);
            v.Build = ushort.Parse(s[2]);
            v.Revision = ushort.Parse(s[3]);
            return v;
        }

        public long ToInteger()
        {
            byte[] b = BitConverter.GetBytes(Major);
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Minor));
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Build));
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Revision));
            return BitConverter.ToInt64(b, 0);
        }
        public byte[] ToRaw()
        {
            byte[] b = BitConverter.GetBytes(Major);
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Minor));
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Build));
            b = BinaryUtil.Append(b, BitConverter.GetBytes(Revision));
            return b;
        }
    }
    public struct Version24
    {
        public byte Major { get; set; }
        public byte Minor { get; set; }
        public byte Build { get; set; }


        public override string ToString()
        {
            return Major + "." + Minor + "." + Build;

        }
        public static Version24 Parse(int version)
        {
            return Parse(BitConverter.GetBytes(version));
        }
        public static Version24 Parse(byte[] vb)
        {
            Version24 v = new Version24();

            v.Major = vb[0];
            v.Minor = vb[1];
            v.Build = vb[2];
            return v;
        }
        public static Version24 Parse(string version)
        {
            Version24 v = new Version24();
            string[] s = version.Split('.');
            v.Major = byte.Parse(s[0]);
            v.Minor = byte.Parse(s[1]);
            v.Build = byte.Parse(s[2]);
            return v;
        }

        public int ToInteger()
        {

            return BitConverter.ToInt32(new byte[4] { Major, Minor, Build, 0 }, 0);
        }
        public byte[] ToRaw()
        {

            return new byte[3] { Major, Minor, Build };
        }
    }

	


    public class ReplayHeader
    {
        ushort _length;
        public ushort Length
        {
            get { return _length; }
        }

        public Version64 ClientVersion { get; set; }
        public Version24 ServerVersion { get; set; }
        public RawEntry8 ObserverKey { get; set; }

        public RawEntry16 Signature { get; set; }
        public RawEntry16 Certificate { get; set; }

        public ReplayHeader()
        {

        }
        public ReplayHeader(byte[] data)
        {
            int pos = 0;
         
            _length = (ushort)data.Length;
            ClientVersion = Version64.Parse(BitConverter.ToInt64(data, 0)); // 8 bytes
          ServerVersion = Version24.Parse(BinaryUtil.Extract(data, 8, 3)); // 3 bytes
            byte blen = data[11]; 
            ObserverKey = new RawEntry8(BinaryUtil.Extract(data, 12, (int)blen)); // 1+blen bytes
            pos = 12 + blen;
            if (data.Length > pos)
            {
                ushort len = BitConverter.ToUInt16(data, pos);
                pos += 2;
                Signature = new RawEntry16(BinaryUtil.Extract(data, pos, (int)len));
                pos += len;
                len = BitConverter.ToUInt16(data, pos);
                pos += 2;
                Certificate = new RawEntry16(BinaryUtil.Extract(data, pos, (int)len));
                pos += len;
            }


        }

        public byte[] ToRaw()
        {
            byte[] data =BitConverter.GetBytes( ClientVersion.ToInteger());
            data = BinaryUtil.Append(data,ServerVersion.ToRaw());
            data = BinaryUtil.Append(data, ObserverKey.ToRaw());
            if (Signature.Data != null && Certificate.Data != null)
            {
                data = BinaryUtil.Append(data, Signature.ToRaw());
                data = BinaryUtil.Append(data, Certificate.ToRaw());
            }
     
         
            return data;
        }
    }
    /// <summary>
    /// Replay Section
    /// </summary>
    public struct ReplaySection
    {
        public byte[] Data;
        public uint Length;
        public SectionType Type;
        public ReplaySection(byte[] data, SectionType dt)
        {
            Data = data;
            Type = dt;
            Length = (uint)data.Length;
        }

        public ReplaySection(List<RawEntry24> rawlist, SectionType dt)
        {

            Type = dt;
            Data = null;
            foreach (RawEntry24 r in rawlist)
            {
                if (Data == null)
                    Data = r.ToData();

                else
                    Data = BinaryUtil.Append(Data, r.ToData
                        ());



            }

            Length = (uint)Data.Length;
        }

  
        public RawEntry24[] GetRawEntries()
        {
            List<RawEntry24> re = new List<RawEntry24>();
            int pos = 0;
            while (pos < Length)
            {
                byte[] byteArray = BinaryUtil.Extract(Data, pos, 3);
                uint len =(uint) (
                        byteArray[0] | (byteArray[1] << 8)  | (byteArray[2] << 16) | (0 << 24));
            
                pos += 3;
               byte[] d = BinaryUtil.Extract(Data, pos, (int)len);
               RawEntry24 r = new RawEntry24(d);
                pos += (int)r.Length;
                re.Add(r);
            }
            return re.ToArray();
        }


    }

    public class GameDataComparer : IComparer
    {
        public GameDataComparer()
        {
        }

        int System.Collections.IComparer.Compare(object a, object b)
        {
         
            try
            {

                GameData x = (GameData)a;
                GameData y = (GameData)b;

                if (x.Id < y.Id)
                    return -1;

                if (x.Id > y.Id)
                    return 1;

                else
                    return 0;
            }
            catch
            {
                return 0;
            }
       
        }
    }

    internal class BinaryUtil
    {
        public static byte[] Extract(byte[] data, int offset, int length)
        {
            byte[] d = new byte[length];
            System.Buffer.BlockCopy(data, offset, d, 0, length);
            return d;
        }
        public static byte[] Append(byte[] x, byte[] y)
        {
            byte[] rv = new byte[x.Length + y.Length];
            System.Buffer.BlockCopy(x, 0, rv, 0, x.Length);
            System.Buffer.BlockCopy(y, 0, rv, x.Length, y.Length);
            return rv;
        }
        public static byte[] Append(byte[] x, byte[] y, int xlen, int ylen)
        {
            byte[] rv = new byte[xlen + ylen];
            System.Buffer.BlockCopy(x, 0, rv, 0, xlen);
            System.Buffer.BlockCopy(y, 0, rv, xlen, y.Length);
            return rv;
        }
    }
}
