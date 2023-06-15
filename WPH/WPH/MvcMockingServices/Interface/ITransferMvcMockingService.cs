using System;
using System.Collections.Generic;
using WPH.Models.Transfer;

namespace WPH.MvcMockingServices.Interface
{
    public interface ITransferMvcMockingService
    {
        string RemoveTransfer(Guid Transferid);
        string AddNewTransfer(TransferViewModel Transfer/*, Guid originalClinicSectionId*/);
        string UpdateTransfer(TransferViewModel Transfer);
        IEnumerable<TransferViewModel> GetAllTransfers(List<Guid> clinicSections, TransferFilterViewModel filterViewModel);
        TransferViewModel GetTransfer(Guid TransferId);
        void GetModalsViewBags(dynamic viewBag);
        IEnumerable<string> GetReceiversName(List<Guid> clinicSections);
        IEnumerable<TransferReportResultViewModel> GetTransferReport(TransferReportViewModel reportViewModel);

        IEnumerable<TransferViewModel> GetAllProductRecive(Guid clinicSectionId);
    }
}
