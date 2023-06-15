using AutoMapper;
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
    public class DMSMedicineMvcMockingService : IDMSMedicineMvcMockingService
    {
        private readonly IDMSUnitOfWork _unitOfWork;
        public DMSMedicineMvcMockingService(IDMSUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new DMSUnitOfWork();
        }

        public IEnumerable<ProductViewModel> GetAllProduct()
        {
            try
            {
                IEnumerable<FN_MedicineNumModel> Products = _unitOfWork.DMSMedicine.GetAllProduct();

                List<ProductViewModel> allProduct = convertModelsLists(Products);
                Indexing<ProductViewModel> indexing = new();
                return indexing.AddIndexing(allProduct);
            }
            catch(Exception e) { throw e; }
            
        }

        public List<ProductViewModel> convertModelsLists(IEnumerable<FN_MedicineNumModel> pro)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FN_MedicineNumModel, ProductViewModel>()
                .ForMember(p => p.Id, r => r.MapFrom(s => s.Id))
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
