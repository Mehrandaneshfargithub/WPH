using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.ProductBarcode;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ProductBarcodeMvcMockingService : IProductBarcodeMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ProductBarcodeMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public string AddNewProductBarcode(ProductBarcodeViewModel viewModel)
        {
            if (viewModel.ProductId == null || string.IsNullOrWhiteSpace(viewModel.Barcode))
                return "DataNotValid";

            if (_unitOfWork.ProductBarcodes.CheckBarcodeExist(null, viewModel.Barcode, viewModel.ClinicSectionId))
                return "ValueIsRepeated";

            ProductBarcode productBarcode = Common.ConvertModels<ProductBarcode, ProductBarcodeViewModel>.convertModels(viewModel);
            productBarcode.CreateDate = DateTime.Now;

            _unitOfWork.ProductBarcodes.Add(productBarcode);
            _unitOfWork.Complete();
            return productBarcode.Guid.ToString();
        }

        public string UpdateProductBarcode(ProductBarcodeViewModel viewModel)
        {
            if (viewModel.ProductId == null || string.IsNullOrWhiteSpace(viewModel.Barcode))
                return "DataNotValid";

            if (_unitOfWork.ProductBarcodes.CheckBarcodeExist(viewModel.Guid, viewModel.Barcode, viewModel.ClinicSectionId))
                return "ValueIsRepeated";

            var productBarcode = _unitOfWork.ProductBarcodes.Get(viewModel.Guid);
            productBarcode.Barcode = viewModel.Barcode;
            productBarcode.ModifiedUserId = viewModel.ModifiedUserId;
            productBarcode.ModifiedDate = DateTime.Now;

            _unitOfWork.ProductBarcodes.UpdateState(productBarcode);
            _unitOfWork.Complete();
            return productBarcode.Guid.ToString();
        }

        public IEnumerable<ProductBarcodeViewModel> GetAllProductBarcodeByProductId(Guid productId)
        {
            IEnumerable<ProductBarcode> productBarcodes = _unitOfWork.ProductBarcodes.GetAllProductBarcodeByProductId(productId);
            List<ProductBarcodeViewModel> dtoProductBarcodes = ConvertModelsLists(productBarcodes);
            Indexing<ProductBarcodeViewModel> indexing = new();
            return indexing.AddIndexing(dtoProductBarcodes);
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ProductBarcode/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }


        public OperationStatus RemoveProductBarcode(Guid productBarcodeid)
        {
            try
            {
                ProductBarcode Rom = _unitOfWork.ProductBarcodes.Get(productBarcodeid);
                _unitOfWork.ProductBarcodes.Remove(Rom);
                _unitOfWork.Complete();
                return OperationStatus.SUCCESSFUL;
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity;
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong;
                }
            }
        }

        public ProductBarcodeViewModel GetProductBarcode(Guid productBarcodeId)
        {
            ProductBarcode ProductBarcodeDb = _unitOfWork.ProductBarcodes.Get(productBarcodeId);
            return Common.ConvertModels<ProductBarcodeViewModel, ProductBarcode>.convertModels(ProductBarcodeDb);

        }

        public List<ProductBarcodeViewModel> ConvertModelsLists(IEnumerable<ProductBarcode> productBarcodes)
        {
            List<ProductBarcodeViewModel> productBarcodeDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductBarcode, ProductBarcodeViewModel>()
                //.ForMember(p => p.TypeName, r => r.MapFrom(s => s.Type.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            productBarcodeDtoList = mapper.Map<IEnumerable<ProductBarcode>, List<ProductBarcodeViewModel>>(productBarcodes);
            return productBarcodeDtoList;
        }
        public ProductBarcodeViewModel ConvertModel(ProductBarcode productBarcode)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductBarcode, ProductBarcodeViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ProductBarcode, ProductBarcodeViewModel>(productBarcode);
        }

    }
}
