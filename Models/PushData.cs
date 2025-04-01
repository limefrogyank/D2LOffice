using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2LOffice.Models
{
    public enum WeeklyInfoType
    {
        Checklists,
        Modules
    }

    public class PushData
    {
        public required string ErrorString { get; set; }
        public WeeklyInfoType WeeklyInfoType { get; set; }
        public required string RootModule { get; set; }
    }
}
