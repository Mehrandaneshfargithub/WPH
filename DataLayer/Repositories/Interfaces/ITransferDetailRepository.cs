using DataLayer.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface ITransferDetailRepository : IRepository<TransferDetail>
    {
        IEnumerable<TransferDetail> GetAllWithChildByMasterId(Guid transferIdId);
        bool CheckTransferDetailInUse(Guid transferDetailId);
        TransferDetail GetWithMasterByDetailId(Guid transferDetailId);
        decimal GetTransferSourceRemCount(Guid transferDetailId);
        IEnumerable<TransferDetail> GetUnreceivedTransferDetailByMasterId(Guid transferId);
        IEnumerable<TransferDetail> GetTransferDetailReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<TransferDetail, bool>> predicate = null);
        bool CheckConfirmAllProductRecive(Guid transferId);
        TransferDetail GetWithSourceProduct(Guid transferDetailId);
        IEnumerable<TransferDetail> GetWithPurchaseInvoiceDetail(List<Guid> transfers);
        TransferDetail GetWithSourcePurchaseInvoice(Guid detailId);
        void IncreaseUpdateWithLocal(TransferDetail detail, decimal remainingNum);
        IEnumerable<TransferDetail> GetWithPricesByMultipleIds(List<Guid> transfers);
        TransferDetail GetWithPricesById(Guid transferDetailId);
        TransferDetail GetTransferDetailForUpdate(Guid transferDetailId);
        TransferDetail GetForNewSalePrice(Guid transferDetailId);
        TransferDetail GetForSalePrice(Guid transferDetailId);
        TransferDetail GetParentCurrency(Guid transferDetailId);
        TransferDetail GetPricesByDetailId(Guid detailId);
    }
}
