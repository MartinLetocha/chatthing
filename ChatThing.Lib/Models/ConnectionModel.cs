using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatThing.Lib.Models
{
    public class ConnectionModel
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public byte[] EncryptedKey { get; set; }
        public byte[] IV { get; set; }
    }
}
