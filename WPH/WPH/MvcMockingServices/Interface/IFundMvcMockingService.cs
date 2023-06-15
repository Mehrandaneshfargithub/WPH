using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Fund;

namespace WPH.MvcMockingServices.Interface
{
    public interface IFundMvcMockingService
    {
        //FundReportViewModel GetAllReceives(List<Guid> clinicSectionId,  DateTime DateFrom, DateTime DateTo, bool Detail);
        FundReportViewModel GetAllReceivesForHospital(List<Guid> allClinicSectionGuids, DateTime fromDate, DateTime toDate, bool detail);
    }
}
