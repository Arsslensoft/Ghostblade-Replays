using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBNet
{
   public class DownloadTask
    {
      public byte Id { get; set; }
      public bool IsChunk { get; set; }
      public long GameId {get;set;}

      public DownloadTask(byte id, bool chunk, long gid)
      { GameId = gid; IsChunk = chunk; Id = id; } 


 
    }

}
