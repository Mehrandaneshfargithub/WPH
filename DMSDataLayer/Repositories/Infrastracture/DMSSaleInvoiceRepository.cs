using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSDataLayer.Repositories.Infrastracture
{
    public class DMSSaleInvoiceRepository : Repository<TblSaleInvoice>, IDMSSaleInvoiceRepository
    {
        protected readonly DMSContext _Context;

        public DMSSaleInvoiceRepository(DMSContext Context) : base(Context)
        {
            _Context = Context;
        }



        public IEnumerable<FN_MedicineNumModel> GetAllSaleProductByCustomerId(int customerId)
        {
            try
            {

                var wholequry = "SELECT Medicine.JoineryName, Medicine.Id, ISNULL(Producer.Name, '') AS ProducerName, ISNULL(MedicineForm.Name, '') AS MedicineFormName," +
                    " ISNULL(MidiBarcode.Barcode,'')Barcode, tblSaleInvoiceDetails.Num as Num , 0.0 as Price FROM " +
                    "dbo.tblSaleInvoice inner JOIN dbo.tblSaleInvoiceDetails ON dbo.tblSaleInvoice.Id = dbo.tblSaleInvoiceDetails.MasterId inner JOIN" +
                    " dbo.tblMedicine Medicine ON dbo.tblSaleInvoiceDetails.MedicineId = Medicine.Id left outer JOIN" +
                    " dbo.tblBaseInfo AS Producer ON Medicine.ProducerId = Producer.Id left outer JOIN" +
                    " dbo.tblBaseInfo AS MedicineForm ON Medicine.FormId = MedicineForm.Id left outer JOIN" +
                    $" dbo.tblMedicineBarcode AS MidiBarcode ON Medicine.Id = MidiBarcode.MedicineId WHERE dbo.tblSaleInvoice.CustomerId = {customerId}";

                List<FN_MedicineNumModel> store = _Context.Set<FN_MedicineNumModel>().FromSqlRaw(wholequry).ToList();


                var wholeReturnqury = "SELECT Medicine.JoineryName, Medicine.Id, '' as Barcode, '' as MedicineFormName, '' as ProducerName " +
                    " , tblReturnSaleInvoiceDetails.Num as Num , 0.0 as Price FROM " +
                    "dbo.tblReturnSaleInvoice inner JOIN dbo.tblReturnSaleInvoiceDetails ON dbo.tblReturnSaleInvoice.Id = dbo.tblReturnSaleInvoiceDetails.MasterId inner JOIN" +
                    " dbo.tblMedicine Medicine ON dbo.tblReturnSaleInvoiceDetails.MedicineId = Medicine.Id"+
                    $" WHERE dbo.tblReturnSaleInvoice.CustomerId = {customerId}";

                IEnumerable<FN_MedicineNumModel> returnedmedicines = _Context.Set<FN_MedicineNumModel>().FromSqlRaw(wholeReturnqury);

                try
                {
                    foreach (var re in returnedmedicines)
                    {
                        var returnStore = store.FirstOrDefault(x => x.Id == re.Id);
                        returnStore.Num = returnStore.Num - re.Num;
                    }
                }
                catch { }
                

                return store;
            }
            catch(Exception e)
            {
                return null;
            }
                
        }

}
}
