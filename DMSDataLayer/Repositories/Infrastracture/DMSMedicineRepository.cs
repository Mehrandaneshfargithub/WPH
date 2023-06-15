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
    public class DMSMedicineRepository : Repository<TblMedicine>, IDMSMedicineRepository
    {
        protected readonly DMSContext _Context;

        public DMSMedicineRepository(DMSContext Context):base(Context)
        {
            _Context = Context;
        }

        public IEnumerable<FN_MedicineNumModel> GetAllProduct()
        {

            string WholeQury = "SELECT tblMedicine.JoineryName, tblMedicine.Id, ISNULL(Producer.Name, '') AS ProducerName, ISNULL(MedicineForm.Name, '') AS MedicineFormName," +
                " ISNULL(MidiBarcode.Barcode,'')Barcode, dbo.FN_MedicineNum(tblMedicine.Id) as Num, dbo.FN_LatestSellingPrice(tblMedicine.Id,1) as Price FROM dbo.tblMedicine left outer JOIN" +
                " dbo.tblBaseInfo AS Producer ON tblMedicine.ProducerId = Producer.Id left outer JOIN" +
                " dbo.tblBaseInfo AS MedicineForm ON tblMedicine.FormId = MedicineForm.Id left outer JOIN" +
                " dbo.tblMedicineBarcode AS MidiBarcode ON tblMedicine.Id = MidiBarcode.MedicineId ";
            var re = _Context.Set<FN_MedicineNumModel>().FromSqlRaw(WholeQury);

            return re;
            
        }

        public IEnumerable<FN_MedicineNumModel> GetAllProductReceptionsByIds(IEnumerable<int?> allDMSProductId)
        {
            string ids = "";

            foreach(var id in allDMSProductId)
            {
                ids = ids + id + " OR tblMedicine.Id = ";
            }

            string allIds = ids.Remove(ids.Length - 21);

            string WholeQury = "SELECT tblMedicine.JoineryName, tblMedicine.Id, ISNULL(Producer.Name, '') AS ProducerName, ISNULL(MedicineForm.Name, '') AS MedicineFormName," +
                " ISNULL(MidiBarcode.Barcode,'')Barcode, dbo.FN_MedicineNum(tblMedicine.Id) as Num, 0.0 as Price FROM  dbo.tblMedicine left outer JOIN" +
                " dbo.tblBaseInfo AS Producer ON tblMedicine.ProducerId = Producer.Id left outer JOIN" +
                " dbo.tblBaseInfo AS MedicineForm ON tblMedicine.FormId = MedicineForm.Id left outer JOIN" +
                " dbo.tblMedicineBarcode AS MidiBarcode ON tblMedicine.Id = MidiBarcode.MedicineId WHERE tblMedicine.Id = " + allIds;
            var re = _Context.Set<FN_MedicineNumModel>().FromSqlRaw(WholeQury);
            return re;
        }

        public IEnumerable<FN_MedicineNumModel> GetAllProductDMSNamesByIds(IEnumerable<int?> allDMSProductId)
        {
            string ids = "";

            foreach (var id in allDMSProductId)
            {
                if(id != null)
                ids = ids + id + " OR tblMedicine.Id = ";
            }

            string allIds = ids.Remove(ids.Length - 21);

            string WholeQury = "SELECT tblMedicine.JoineryName, tblMedicine.Id, ISNULL(Producer.Name, '') AS ProducerName, ISNULL(MedicineForm.Name, '') AS MedicineFormName," +
                " ISNULL(MidiBarcode.Barcode,'')Barcode, 0.0 as Num, 0.0 as Price FROM  dbo.tblMedicine left outer JOIN" +
                " dbo.tblBaseInfo AS Producer ON tblMedicine.ProducerId = Producer.Id left outer JOIN" +
                " dbo.tblBaseInfo AS MedicineForm ON tblMedicine.FormId = MedicineForm.Id left outer JOIN" +
                " dbo.tblMedicineBarcode AS MidiBarcode ON tblMedicine.Id = MidiBarcode.MedicineId WHERE tblMedicine.Id = " + allIds;
            var re = _Context.Set<FN_MedicineNumModel>().FromSqlRaw(WholeQury);
            return re;
        }
    }
}
