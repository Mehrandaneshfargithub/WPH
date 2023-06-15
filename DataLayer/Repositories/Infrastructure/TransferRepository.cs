using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class TransferRepository : Repository<Transfer>, ITransferRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public TransferRepository(WASContext context)
            : base(context)
        {
        }

        public IEnumerable<Transfer> GetAllTransfer(DateTime dateFrom, DateTime dateTo, Expression<Func<Transfer, bool>> predicate = null)
        {
            IQueryable<Transfer> result = Context.Transfers.AsNoTracking()
                .Include(p => p.CreatedUser)
                .Include(p => p.ReceiverUser)
                .Include(p => p.SourceClinicSection)
                .Include(p => p.DestinationClinicSection)
                .Where(p => p.InvoiceDate >= dateFrom && p.InvoiceDate <= dateTo);

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(p => new Transfer
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                ReceiverName = p.ReceiverName,
                SourceClinicSectionId = p.SourceClinicSectionId,
                CreatedUser = new User
                {
                    Name = p.CreatedUser.Name
                },
                ReceiverUser = new User
                {
                    Name = p.ReceiverUser.Name
                },
                SourceClinicSection = new ClinicSection
                {
                    Name = p.SourceClinicSection.Name
                },
                DestinationClinicSection = new ClinicSection
                {
                    Name = p.DestinationClinicSection.Name
                }
            });
        }

        public Transfer GetWithType(Guid transferId)
        {
            return Context.Transfers.AsNoTracking()
                .SingleOrDefault(p => p.Guid == transferId);
        }

        public IEnumerable<string> GetReceiversName(List<Guid> clinicSections)
        {
            return _context.Transfers.AsNoTracking()
                .Where(p => p.ReceiverName != null && (clinicSections.Contains(p.SourceClinicSectionId.Value) || clinicSections.Contains(p.DestinationClinicSectionId.Value)))
                .Select(p => p.ReceiverName)
                .Distinct();
        }

        public IEnumerable<Transfer> GetAllProductRecive(Guid clinicSectionId)
        {
            IQueryable<Transfer> result = Context.Transfers.AsNoTracking()
                .Include(p => p.CreatedUser)
                .Include(p => p.SourceClinicSection)
                .Include(p => p.DestinationClinicSection)
                .Include(p => p.TransferDetails)
                .Where(p => p.DestinationClinicSectionId == clinicSectionId && p.ReceiverUserId == null && p.TransferDetails.Any(x => x.DestinationProductId == null));

            return result.Select(p => new Transfer
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate,
                Description = p.Description,
                ReceiverName = p.ReceiverName,
                CreatedUser = new User
                {
                    Name = p.CreatedUser.Name
                },
                SourceClinicSection = new ClinicSection
                {
                    Name = p.SourceClinicSection.Name
                },
                DestinationClinicSection = new ClinicSection
                {
                    Name = p.DestinationClinicSection.Name
                }
            });
        }

        public void RemoveTransfer(Transfer transfer)
        {
            try
            {

                IEnumerable<TransferDetail> allDetail = Context.TransferDetails.Where(a => a.MasterId == transfer.Guid);
                foreach (var detail in allDetail)
                {
                    if (detail.PurchaseInvoiceDetailId.HasValue)
                    {
                        PurchaseInvoiceDetail pu = Context.PurchaseInvoiceDetails.FirstOrDefault(a => a.Guid == detail.PurchaseInvoiceDetailId);
                        pu.RemainingNum += detail.RemainingNum;
                    }
                    if (detail.TransferDetailId.HasValue)
                    {
                        TransferDetail pu = Context.TransferDetails.FirstOrDefault(a => a.Guid == detail.TransferDetailId);
                        pu.RemainingNum += detail.RemainingNum;
                    }

                }
                Context.TransferDetails.RemoveRange(allDetail);
                Context.Transfers.Remove(transfer);
                Context.SaveChanges();

            }
            catch (Exception e) { throw e; }
        }
    }
}
