using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Models
{
    public class SyncProgress
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public string Message { get; set; }
    }
}
