using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Product;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DMSSaleInvoiceMvcMockingService : IDMSSaleInvoiceMvcMockingService
    {
        private readonly IDMSUnitOfWork _DMSunitOfWork;
        private readonly IUnitOfWork _unitOfWork;
        public DMSSaleInvoiceMvcMockingService(IDMSUnitOfWork DMSunitOfWork, IUnitOfWork unitOfWork)
        {
            _DMSunitOfWork = DMSunitOfWork;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<ProductViewModel> GetAllClinicSectionProducts(Guid ClinicSectionId ,string clinicSectionName)
        {

            int customerId = _DMSunitOfWork.DMSCustomer.GetCustomerIdByName(clinicSectionName);

            List<FN_MedicineNumModel> GetAllClinicSectionProducts = _DMSunitOfWork.DMSSaleInvoice.GetAllSaleProductByCustomerId(customerId)
                .GroupBy(x => x.Id).Select(x => new FN_MedicineNumModel
                {
                    Id = x.Key.Value,
                    Barcode = x.FirstOrDefault().Barcode,
                    JoineryName = x.FirstOrDefault().JoineryName,
                    MedicineFormName = x.FirstOrDefault().MedicineFormName,
                    ProducerName = x.FirstOrDefault().ProducerName,
                    Num = x.Sum(a => a.Num)
                }).ToList();

            try
            {
                //IEnumerable<InvoiceDetail> GetAllInvoiceDetail = _unitOfWork.InvoiceDetails.Find(x => x.ClinicSectionId == ClinicSectionId && x.ProductIdDms != null);
                //IEnumerable<FN_MedicineNumModel> GetAllInvoiceDetailb = GetAllInvoiceDetail.GroupBy(x => x.ProductIdDms).Select(x => new FN_MedicineNumModel
                //{
                //    Id = x.Key.Value,
                //    Num = x.Sum(a => a.Number)
                //});

                //foreach (var med in GetAllInvoiceDetailb)
                //{
                //    var medid = GetAllClinicSectionProducts.SingleOrDefault(x => x.Id == med.Id);
                //    if(medid != null)
                //        medid.Num = medid.Num - med.Num;
                //}

                //foreach (var med in GetAllClinicSectionProducts)
                //{
                //    med.Num = med.Num - (GetAllInvoiceDetailb.SingleOrDefault(x => x.Id == med.Id).Num ?? 0);
                //}
            }
            catch { }

            


            List<ProductViewModel> allProduct = convertModelsLists(GetAllClinicSectionProducts);
            Indexing<ProductViewModel> indexing = new();
            return indexing.AddIndexing(allProduct);

        }

        public List<ProductViewModel> convertModelsLists(IEnumerable<FN_MedicineNumModel> pro)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FN_MedicineNumModel, ProductViewModel>()
                //.ForMember(p => p.Barcode, r => r.MapFrom(s => s.Barcode))
                .ForMember(p => p.Name, r => r.MapFrom(s => s.JoineryName))
                .ForMember(p => p.ProducerName, r => r.MapFrom(s => s.ProducerName))
                .ForMember(p => p.ProductTypeName, r => r.MapFrom(s => s.MedicineFormName))
                .ForMember(p => p.Stock, r => r.MapFrom(s => s.Num));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<FN_MedicineNumModel>, List<ProductViewModel>>(pro);

        }
    }
}
