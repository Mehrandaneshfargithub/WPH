using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public class FN_GetPastAnalysisResult_Result
    {
        public string Name { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Value { get; set; }
    }
}
