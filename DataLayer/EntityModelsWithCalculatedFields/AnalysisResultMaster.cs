using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.EntityModels
{
    public partial class AnalysisResultMaster
    {
        //public string Ready
        //{
        //    get
        //    {
        //        if(Reception != null)
        //        {
        //            int allAnalysisCount = Reception.PatientReceptionAnalyses.Sum(a => a.Analysis == null ? 0 : a.Analysis.AnalysisAnalysisItems.Count)
        //            + Reception.PatientReceptionAnalyses.Sum(a => a.AnalysisItem == null ? 0 : 1)
        //            + Reception.PatientReceptionAnalyses.Sum(a => a.GroupAnalysis == null ? 0 : a.GroupAnalysis.GroupAnalysisItems.Count)
        //            + Reception.PatientReceptionAnalyses.Sum(a => a.GroupAnalysis == null ? 0 : a.GroupAnalysis.GroupAnalysisAnalyses.Sum(b => b.Analysis == null ? 0 : b.Analysis.AnalysisAnalysisItems.Count))
        //            ;
        //            if (AnalysisResults.Count == 0)
        //            {
        //                return "not ready";
        //            }
        //            else if (AnalysisResults.Count < allAnalysisCount && Reception.ClinicSection.ClinicSectionType.Name != "Radiology")
        //                return "partial ready";
        //            else
        //                return "ready";
        //        }
        //        else
        //        {
        //            return "";
        //        }
                
        //    }
        //}


    }
}
