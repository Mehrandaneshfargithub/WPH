using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories.Infrastracture;
using DMSDataLayer.Repositories.Interfaces;

namespace DMSDataLayer.Repositories
{
    public class DMSUnitOfWork : IDMSUnitOfWork
    {
        private readonly DMSContext _context;
        public DMSUnitOfWork()
            : this(new DMSContext())
        {
        }
        public DMSUnitOfWork(DMSContext dmscontext)
        {

            _context = dmscontext;
            DMSMedicine = new DMSMedicineRepository(_context);
            DMSSaleInvoice = new DMSSaleInvoiceRepository(_context);
            DMSSaleInvoiceDetail = new DMSSaleInvoiceDetailRepository(_context);
            DMSCustomer = new DMSCustomerRepository(_context);
            
        }

        public IDMSMedicineRepository DMSMedicine { get; private set; }
        public IDMSSaleInvoiceRepository DMSSaleInvoice { get; private set; }
        public IDMSSaleInvoiceDetailRepository DMSSaleInvoiceDetail { get; private set; }
        public IDMSCustomerRepository DMSCustomer { get; private set; }
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
