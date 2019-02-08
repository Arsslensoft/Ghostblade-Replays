using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBReplay.Replays
{
  public struct GameData
    {
      public byte[] Data;
      public byte Id;

      public GameData(byte[] dt, byte id)
      {
          Id = id;
          Data = dt;
      }
    }
}
