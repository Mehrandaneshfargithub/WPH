using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.PurchaseInvoiceDetailSalePrice;
using WPH.Models.TransferDetail;

namespace WPH.MvcMockingServices.Interface
{
    public interface ITransferDetailMvcMockingService
    {
        string RemoveTransferDetail(Guid TransferDetailid);
        string AddNewTransferDetail(TransferDetailViewModel TransferDetail/*, Guid originalClinicSectionId*/);
        string UpdateTransferDetail(TransferDetailViewModel TransferDetail/*, Guid originalClinicSectionId*/);
        IEnumerable<TransferDetailGridViewModel> GetAllTransferDetails(Guid clinicSectionId);
        TransferDetailViewModel GetTransferDetailForUpdate(Guid TransferDetailId);
        IEnumerable<TransferDetailGridViewModel> GetUnreceivedTransferDetail(Guid transferId);
        void GetModalsViewBags(dynamic viewBag);
        
        void ResetProductReceive(Guid transferId);
        string ConfirmAllProductRecive(Guid transferId, Guid userId);
        TransferDetailViewModel GetSourceProducName(Guid transferDateilId);
        string AddProductReceive(TransferDetailViewModel viewModel, Guid clinicSectionId);
        string AddNewTransferDetailList(IEnumerable<TransferDetailViewModel> viewModels, Guid userId);
        ParentDetailSalePriceViewModel GetForNewSalePrice(Guid transferDetailId);
    }
}
