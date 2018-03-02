using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerretAzureDemo.MerretDataAccess
{
    public class MerretDataException : Exception
    {
        public MerretDataException() : base("Merret Data Exception") { }

        public MerretDataException(string msg) : base(msg) { }

        public MerretDataException(string msg, Exception innerException) : base(msg, innerException) { }
    }
}
