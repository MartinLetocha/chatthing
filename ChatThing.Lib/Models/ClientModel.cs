using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatThing.Lib.Models
{
    public class ClientModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PublicKey { get; set; }
        public byte[] SomeThing { get; set; }
        public byte[] OtherThing { get; set; }
    }
}
