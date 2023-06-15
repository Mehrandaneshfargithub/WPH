using DMSDataLayer.Repositories.Interfaces;
using System;

namespace DMSDataLayer.Repositories
{
    public interface IDMSUnitOfWork : IDisposable
    {
        IDMSMedicineRepository DMSMedicine { get; }
        IDMSSaleInvoiceRepository DMSSaleInvoice { get; }
        IDMSSaleInvoiceDetailRepository DMSSaleInvoiceDetail { get; }
        IDMSCustomerRepository DMSCustomer { get; }
        int Complete();
    }
}
