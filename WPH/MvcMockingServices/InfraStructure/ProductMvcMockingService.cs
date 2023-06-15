using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.ExpireList;
using WPH.Models.MaterialStoreroom;
using WPH.Models.Product;
using WPH.Models.PurchaseInvoiceDetail;
using WPH.Models.TransferDetail;
using WPH.MvcMockingServices.Interface;
using static Common.Enums;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ProductMvcMockingService : IProductMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public ProductMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }


        public OperationStatus RemoveProduct(Guid Productid)
        {
            try
            {
                Product Hos = _unitOfWork.Products.Get(Productid);
                _unitOfWork.Products.Remove(Hos);
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

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Product/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewProductWithOutBarcode(ProductViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ProductTypeName) || string.IsNullOrWhiteSpace(viewModel.ProducerName) || string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProductTypeName, "ProductType", viewModel.ClinicSectionId.Value);
            viewModel.ProductTypeId = typeId;

            var producerId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProducerName, "Producer", viewModel.ClinicSectionId.Value);
            viewModel.ProducerId = producerId;

            if (typeId != null && typeId != Guid.Empty && producerId != null && producerId != Guid.Empty &&
                CheckRepeatedProductName(viewModel.ClinicSectionId.Value, viewModel.ProducerId.Value, viewModel.ProductTypeId.Value, viewModel.Name, true, viewModel.MaterialTypeId))
                return "ValueIsRepeated";

            var now = DateTime.Now;
            viewModel.CreatedDate = now;
            Product product1 = Common.ConvertModels<Product, ProductViewModel>.convertModels(viewModel);

            product1.ProductMasterId = Guid.NewGuid();

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Guid = Guid.NewGuid(),
                    Name = viewModel.ProductTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("ProductType"),
                    ClinicSectionId = viewModel.ClinicSectionId
                };
                _unitOfWork.BaseInfos.Add(baseInfo);
                product1.ProductTypeId = baseInfo.Guid;
            }

            if (producerId == null || producerId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Guid = Guid.NewGuid(),
                    Name = viewModel.ProducerName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("Producer"),
                    ClinicSectionId = viewModel.ClinicSectionId
                };
                _unitOfWork.BaseInfos.Add(baseInfo);
                product1.ProducerId = baseInfo.Guid;
            }

            List<Product> allProduct = new List<Product>();

            string[] ids = viewModel.ClinicSections?.Split(',');

            if (ids?.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    Product p = Common.ConvertModels<Product, Product>.convertModels(product1);

                    p.ClinicSectionId = Guid.Parse(ids[i]);

                    allProduct.Add(p);
                }
            }

            allProduct.Add(product1);

            _unitOfWork.Products.AddRange(allProduct);
            _unitOfWork.Complete();
            return product1.Guid.ToString();
        }

        public string AddNewProduct(ProductViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ProductTypeName) || string.IsNullOrWhiteSpace(viewModel.ProducerName)
                || string.IsNullOrWhiteSpace(viewModel.Name) || string.IsNullOrWhiteSpace(viewModel.ScientificName))
                return "DataNotValid";

            if (!string.IsNullOrWhiteSpace(viewModel.Barcode) && _unitOfWork.ProductBarcodes.CheckBarcodeExist(null, viewModel.Barcode, viewModel.ClinicSectionId.Value))
                return "RepeatedBarcode";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProductTypeName, "ProductType", viewModel.ClinicSectionId.Value);
            viewModel.ProductTypeId = typeId;

            var producerId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProducerName, "Producer", viewModel.ClinicSectionId.Value);
            viewModel.ProducerId = producerId;

            if (typeId != null && typeId != Guid.Empty && producerId != null && producerId != Guid.Empty &&
                CheckRepeatedProductName(viewModel.ClinicSectionId.Value, viewModel.ProducerId.Value, viewModel.ProductTypeId.Value, viewModel.Name, true, viewModel.MaterialTypeId))
                return "ValueIsRepeated";

            var now = DateTime.Now;
            viewModel.CreatedDate = now;
            Product product1 = Common.ConvertModels<Product, ProductViewModel>.convertModels(viewModel);

            product1.ProductMasterId = Guid.NewGuid();

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Guid = Guid.NewGuid(),
                    Name = viewModel.ProductTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("ProductType"),
                    ClinicSectionId = viewModel.ClinicSectionId
                };
                _unitOfWork.BaseInfos.Add(baseInfo);
                product1.ProductTypeId = baseInfo.Guid;
            }

            if (producerId == null || producerId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Guid = Guid.NewGuid(),
                    Name = viewModel.ProducerName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("Producer"),
                    ClinicSectionId = viewModel.ClinicSectionId
                };
                _unitOfWork.BaseInfos.Add(baseInfo);
                product1.ProducerId = baseInfo.Guid;
            }

            ProductBarcode barcode = null;
            if (!string.IsNullOrWhiteSpace(viewModel.Barcode))
            {
                barcode = new ProductBarcode
                {
                    Barcode = viewModel.Barcode,
                    CreateDate = now,
                    CreateUserId = viewModel.CreateUserId
                };

                product1.ProductBarcodes = new List<ProductBarcode>
                {
                    barcode
                };
            }

            List<Product> allProduct = new List<Product>();

            string[] ids = viewModel.ClinicSections?.Split(',');

            if (ids?.Length > 0)
            {
                for (int i = 0; i < ids.Length; i++)
                {
                    Product p = Common.ConvertModels<Product, Product>.convertModels(product1);

                    p.ClinicSectionId = Guid.Parse(ids[i]);

                    if (_unitOfWork.ProductBarcodes.CheckBarcodeExist(null, viewModel.Barcode, p.ClinicSectionId.Value))
                        return "RepeatedBarcode";

                    if (barcode != null)
                        p.ProductBarcodes = new List<ProductBarcode>
                        {
                            barcode
                        };

                    allProduct.Add(p);
                }
            }

            allProduct.Add(product1);

            _unitOfWork.Products.AddRange(allProduct);
            _unitOfWork.Complete();
            return product1.Guid.ToString();
        }

        public string UpdateProduct(ProductViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ProductTypeName) || string.IsNullOrWhiteSpace(viewModel.ProducerName) || string.IsNullOrWhiteSpace(viewModel.Name))
                return "DataNotValid";

            //if (!string.IsNullOrEmpty(viewModel.Barcode) && _unitOfWork.Products.CheckRepeatedProductBarcode(viewModel.Name, viewModel.Barcode))
            //    return "RepeatedBarcode";

            var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProductTypeName, "ProductType", viewModel.ClinicSectionId.Value);
            viewModel.ProductTypeId = typeId;

            var producerId = _unitOfWork.BaseInfos.GetIdByNameAndType(viewModel.ProducerName, "Producer", viewModel.ClinicSectionId.Value);
            viewModel.ProducerId = producerId;

            if (typeId != null && typeId != Guid.Empty && producerId != null && producerId != Guid.Empty &&
                CheckRepeatedProductName(viewModel.ClinicSectionId.Value, viewModel.ProducerId.Value, viewModel.ProductTypeId.Value, viewModel.Name, false, viewModel.MaterialTypeId, viewModel.NameHolder))
                return "ValueIsRepeated";

            if (typeId == null || typeId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = viewModel.ProductTypeName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("ProductType"),
                    ClinicSectionId = viewModel.ClinicSectionId,
                    Guid = Guid.NewGuid()
                };

                _unitOfWork.BaseInfos.Add(baseInfo);
                typeId = baseInfo.Guid;
            }

            if (producerId == null || producerId == Guid.Empty)
            {
                var baseInfo = new BaseInfo
                {
                    Name = viewModel.ProducerName,
                    TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("Producer"),
                    ClinicSectionId = viewModel.ClinicSectionId,
                    Guid = Guid.NewGuid()
                };

                _unitOfWork.BaseInfos.Add(baseInfo);
                producerId = baseInfo.Guid;
            }



            if (viewModel.ProductMasterId == null || viewModel.ProductMasterId == Guid.Empty)
            {
                Guid MasterId = Guid.NewGuid();

                var product = _unitOfWork.Products.Get(viewModel.Guid);

                product.Name = viewModel.Name;
                product.ScientificName = viewModel.ScientificName;
                product.OrderPoint = viewModel.OrderPoint;
                product.ModifiedUserId = viewModel.ModifiedUserId;
                product.ModifiedDate = DateTime.Now;
                product.ProductLocation = viewModel.ProductLocation;
                product.Description = viewModel.Description;
                product.ProductTypeId = typeId;
                product.ProducerId = producerId;
                product.ProductMasterId = MasterId;
                _unitOfWork.Products.UpdateState(product);

                List<Product> allProduct = new List<Product>();

                string[] ids = viewModel.ClinicSections?.Split(',');

                if (ids?.Length > 0)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {

                        Product oldClinicSection = _unitOfWork.Products.GetSingle(a => a.ClinicSectionId == Guid.Parse(ids[i]) && a.Name == viewModel.Name && a.ProducerId == producerId && a.ProductTypeId == typeId);

                        if (oldClinicSection == null)
                        {
                            Product newProduct = new Product()
                            {
                                Name = viewModel.Name,
                                ClinicSectionId = Guid.Parse(ids[i]),
                                ScientificName = viewModel.ScientificName,
                                OrderPoint = viewModel.OrderPoint,
                                ProducerId = producerId,
                                ProductTypeId = typeId,
                                ModifiedUserId = viewModel.ModifiedUserId,
                                ModifiedDate = DateTime.Now,
                                ProductLocation = viewModel.ProductLocation,
                                Description = viewModel.Description,
                                ProductMasterId = MasterId
                            };
                            allProduct.Add(newProduct);
                        }
                        else
                        {
                            oldClinicSection.ProductMasterId = viewModel.ProductMasterId;
                            oldClinicSection.ScientificName = viewModel.ScientificName;
                            oldClinicSection.OrderPoint = viewModel.OrderPoint;
                            oldClinicSection.ModifiedUserId = viewModel.ModifiedUserId;
                            oldClinicSection.ModifiedDate = DateTime.Now;
                            oldClinicSection.ProductLocation = viewModel.ProductLocation;
                            oldClinicSection.Description = viewModel.Description;
                            oldClinicSection.ProductTypeId = typeId;
                            oldClinicSection.ProducerId = producerId;
                            oldClinicSection.ProductMasterId = MasterId;
                            _unitOfWork.Products.UpdateState(oldClinicSection);
                        }

                    }

                }

            }
            else
            {
                var products = _unitOfWork.Products.Find(a => a.ProductMasterId == viewModel.ProductMasterId.GetValueOrDefault());

                List<Product> allProduct = new List<Product>();

                string[] ids = viewModel.ClinicSections.Split(',');

                if (ids.Length > 0)
                {
                    for (int i = 0; i < ids.Length; i++)
                    {

                        Product p = products.FirstOrDefault(a => a.ClinicSectionId == Guid.Parse(ids[i]));

                        if (p == null)
                        {
                            Product oldClinicSection = _unitOfWork.Products.GetSingle(a => a.ClinicSectionId == Guid.Parse(ids[i]) && a.Name == viewModel.Name && a.ProducerId == producerId && a.ProductTypeId == typeId);

                            if (oldClinicSection == null)
                            {
                                Product newProduct = new Product()
                                {
                                    Name = viewModel.Name,
                                    ClinicSectionId = Guid.Parse(ids[i]),
                                    ScientificName = viewModel.ScientificName,
                                    OrderPoint = viewModel.OrderPoint,
                                    ProducerId = producerId,
                                    ProductTypeId = typeId,
                                    CreatedDate = DateTime.Now,
                                    CreateUserId = viewModel.ModifiedUserId ?? Guid.Empty,
                                    ModifiedUserId = viewModel.ModifiedUserId,
                                    ModifiedDate = DateTime.Now,
                                    ProductLocation = viewModel.ProductLocation,
                                    Description = viewModel.Description,
                                    ProductMasterId = viewModel.ProductMasterId,
                                    MaterialTypeId = viewModel.MaterialTypeId
                                };
                                allProduct.Add(newProduct);

                            }
                            else
                            {
                                oldClinicSection.ProductMasterId = viewModel.ProductMasterId;
                            }

                        }

                        //p.ClinicSectionId = Guid.Parse(ids[i]);



                    }

                }

                _unitOfWork.Products.AddRange(allProduct);

                foreach (var pro in products)
                {
                    pro.Name = viewModel.Name;
                    pro.ScientificName = viewModel.ScientificName;
                    pro.OrderPoint = viewModel.OrderPoint;
                    pro.ModifiedUserId = viewModel.ModifiedUserId;
                    pro.ModifiedDate = DateTime.Now;
                    pro.ProductLocation = viewModel.ProductLocation;
                    pro.Description = viewModel.Description;
                    pro.ProductTypeId = typeId;
                    pro.ProducerId = producerId;
                    pro.ProductMasterId = viewModel.ProductMasterId;
                    _unitOfWork.Products.UpdateState(pro);
                }
            }

            _unitOfWork.Complete();
            return null;

        }

        public bool CheckRepeatedProductName(Guid clinicSectionId, Guid producerId, Guid productTypeId, string name, bool NewOrUpdate, int? materialTypeId, string oldName = "")
        {
            Product product = null;
            if (NewOrUpdate)
            {
                product = _unitOfWork.Products.GetSingle(x => x.ClinicSectionId == clinicSectionId && x.Name.Trim() == name.Trim() && x.ProducerId == producerId && x.ProductTypeId == productTypeId && x.MaterialTypeId == materialTypeId);
            }
            else
            {
                product = _unitOfWork.Products.GetSingle(x => x.ClinicSectionId == clinicSectionId && x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ProducerId == producerId && x.ProductTypeId == productTypeId && x.MaterialTypeId == materialTypeId);
            }
            if (product != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<ProductViewModel> GetLimitedTotalProductsName(Guid clinicSectionId, Guid? productId)
        {
            List<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 20).ToList();

            if (productId != null)
            {
                var product = _unitOfWork.Products.GetProductName(productId.Value);
                hosp = hosp.Where(p => p.Guid != productId).ToList();

                hosp.Add(product);
            }

            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public IEnumerable<ProductViewModel> GetLimitedMaterialProductsName(Guid clinicSectionId, Guid? productId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Material", "MaterialType");
            List<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 20, p => p.MaterialTypeId == typeId).ToList();

            if (productId != null)
            {
                var product = _unitOfWork.Products.GetProductName(productId.Value);
                hosp = hosp.Where(p => p.Guid != productId).ToList();

                hosp.Add(product);
            }

            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public IEnumerable<ProductWithBarcodeViewModel> GetLimitedProductsWithBarcode(Guid clinicSectionId)
        {
            List<Product> hosp = _unitOfWork.Products.GetProductsWithBarcode(clinicSectionId, 20).ToList();

            IEnumerable<ProductWithBarcodeViewModel> hospconvert = hosp.Select(p => new ProductWithBarcodeViewModel
            {
                ProductId = p.Guid,
                ProductName = $"{p.Name} | {p.ProductType.Name} | {p.Producer.Name}",
                Barcode = string.Join(",", p.ProductBarcodes.Select(x => x.Barcode))
            });

            return hospconvert;
        }

        public IEnumerable<ProductWithBarcodeViewModel> GetAllTotalProductsWithBarcode(Guid clinicSectionId)
        {
            List<Product> hosp = _unitOfWork.Products.GetProductsWithBarcode(clinicSectionId, 0).ToList();

            IEnumerable<ProductWithBarcodeViewModel> hospconvert = hosp.Select(p => new ProductWithBarcodeViewModel
            {
                ProductId = p.Guid,
                ProductName = $"{p.Name} | {p.ProductType.Name} | {p.Producer.Name}",
                Barcode = string.Join(",", p.ProductBarcodes.Select(x => x.Barcode))
            });

            return hospconvert;
        }


        public IEnumerable<ProductViewModel> GetLimitedProductsName(Guid clinicSectionId, Guid? productId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Medicine", "MaterialType");
            List<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 20, p => p.MaterialTypeId == typeId).ToList();

            if (productId != null)
            {
                var product = _unitOfWork.Products.GetProductName(productId.Value);
                hosp = hosp.Where(p => p.Guid != productId).ToList();

                hosp.Add(product);
            }

            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public IEnumerable<ProductViewModel> GetAllTotalProductsForFilter(Guid clinicSectionId)
        {
            IEnumerable<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 0);
            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public IEnumerable<ProductViewModel> GetAllMaterialProductsName(Guid clinicSectionId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Material", "MaterialType");
            IEnumerable<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 0, p => p.MaterialTypeId == typeId);
            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public IEnumerable<ProductViewModel> GetAllProductsName(Guid clinicSectionId)
        {
            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Medicine", "MaterialType");
            IEnumerable<Product> hosp = _unitOfWork.Products.GetAllProductsName(clinicSectionId, 0, p => p.MaterialTypeId == typeId);
            List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
            return hospconvert;
        }

        public ProductViewModel GetProduct(Guid MasterProductId, Guid ProductId)
        {
            try
            {
                Product Productgu = _unitOfWork.Products.GetWithProducerAndProduct(MasterProductId, ProductId);
                return ConvertModel(Productgu);
            }
            catch { return null; }
        }

        public string GetProductName(Guid ProductId)
        {
            try
            {
                Product result = _unitOfWork.Products.GetProductName(ProductId);
                return $"{result.Name} | {result.ProductType.Name} | {result.Producer.Name}";
            }
            catch { return ""; }
        }



        public IEnumerable<ProductWithBarcodeViewModel> GetAllProductsWithBarcode(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<ProductWithBarcodeModel> Productgu = _unitOfWork.Products.GetAllProductsWithBarcode(clinicSectionId);
                return Common.ConvertModels<ProductWithBarcodeViewModel, ProductWithBarcodeModel>.convertModelsLists(Productgu);
            }
            catch (Exception e) { return null; }
        }


        public PurchaseInvoiceDetailViewModel GetProductDetails(Guid productId, bool latestPrice, string SaleType, int? SellCurrencyId)
        {
            try
            {
                PurchaseOrTransferProductDetail Productgu = _unitOfWork.PurchaseInvoiceDetails.GetProductDetails(productId, latestPrice, SaleType, SellCurrencyId);
                return ConvertModelForProductDetail(Productgu);
            }
            catch (Exception e) { return null; }
        }




        public IEnumerable<MaterialStoreroomViewModel> GetAllMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId)
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAllMaterialProducts(originalClinicSectionId, clinicSectionId);

            //var total = products.Count();
            //int skip = (page - 1) * pageSize;

            List<MaterialStoreroomViewModel> result = new();
            //if (skip > 0)
            //{
            //    MaterialStoreroomViewModel[] array = new MaterialStoreroomViewModel[skip];
            //result.AddRange(array.Select(p => new MaterialStoreroomViewModel()).ToList());
            //}

            //var hosp = products/*.Skip(skip).Take(pageSize)*/.ToList();
            //var hosp = products.ToList();
            result = ConvertMaterialModelsLists(products);

            //int rem = total - (skip + pageSize);
            //if (rem > 0)
            //{
            //    MaterialStoreroomViewModel[] array = new MaterialStoreroomViewModel[rem];
            //    result.AddRange(array.Select(p => new MaterialStoreroomViewModel()).ToList());
            //}

            Indexing<MaterialStoreroomViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductStoreroomViewModel> GetAllStoreroomProducts(ProductFilterViewModel filterViewModel)
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAllStoreroomProducts(filterViewModel.OriginalClinicSectionId, filterViewModel.ClinicSectionId, filterViewModel.ProductBarcode, filterViewModel.FromOrderPoint, filterViewModel.ToOrderPoint, filterViewModel.SupplierId);

            if (filterViewModel.SupplierId != null)
            {
                products = products.Where(p => p.PurchaseInvoiceDetails.Any() || p.TransferDetailDestinationProducts.Any());
            }

            List<ProductStoreroomViewModel> result = ConvertProductModelsLists(products);

            Indexing<ProductStoreroomViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductStoreroomViewModel> GetAllClinicSectionsStoreroomProducts(ProductFilterViewModel filterViewModel)
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAllClinicSectionsStoreroomProducts(filterViewModel.OriginalClinicSectionId, filterViewModel.ClinicSectionId, filterViewModel.ProductBarcode, filterViewModel.FromOrderPoint, filterViewModel.ToOrderPoint, filterViewModel.SupplierId);

            if (filterViewModel.SupplierId != null)
            {
                products = products.Where(p => p.PurchaseInvoiceDetails.Any() || p.TransferDetailDestinationProducts.Any());
            }

            List<ProductStoreroomViewModel> result = ConvertProductModelsLists(products).GroupBy(a => a.ProductMasterId)
                .Select(a => new ProductStoreroomViewModel
                {
                    ChildrensCount = a.Count(),
                    ChildrensGuid = string.Join(',', a.Select(a => a.Guid)),
                    Count = a.Sum(a => a.Count),
                    Description = a.FirstOrDefault().Description,
                    Guid = a.FirstOrDefault().Guid,
                    Name = a.FirstOrDefault().Name,
                    OrderPoint = a.FirstOrDefault().OrderPoint,
                    ProducerName = a.FirstOrDefault().ProducerName,
                    ProductLocation = a.FirstOrDefault().ProductLocation,
                    ProductMasterId = a.Key,
                    ProductTypeName = a.FirstOrDefault().ProductTypeName,
                    ScientificName = a.FirstOrDefault().ScientificName
                }).ToList();

            Indexing<ProductStoreroomViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<MaterialStoreroomViewModel> GetAllClinicSectionsMaterialProducts(Guid originalClinicSectionId, Guid clinicSectionId)
        {
            IEnumerable<Product> products = _unitOfWork.Products.GetAllClinicSectionsMaterialProducts(originalClinicSectionId, clinicSectionId);

            List<MaterialStoreroomViewModel> result = ConvertProductModelsLists(products).GroupBy(a => a.ProductMasterId)
                .Select(a => new MaterialStoreroomViewModel
                {
                    Count = a.Sum(a => a.Count),
                    Guid = a.FirstOrDefault().Guid,
                    Name = a.FirstOrDefault().Name,
                    ProducerName = a.FirstOrDefault().ProducerName,
                    ProductTypeName = a.FirstOrDefault().ProductTypeName,
                }).ToList();

            Indexing<MaterialStoreroomViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<MaterialProductHistoryViewModel> GetAllMaterialProductHistory(Guid originalClinicSectionId, Guid clinicSectionId, Guid productId)
        {
            var product = _unitOfWork.Products.GetProductHistory(productId, clinicSectionId, originalClinicSectionId);

            var get = product.TransferDetailDestinationProducts.Select(s => new MaterialProductHistoryViewModel
            {
                Guid = s.Guid,
                Num = s.Num.GetValueOrDefault(0),
                ExpireDate = s.ExpireDate,
                SellingPrice = s.SellingPrice,
                PurchasePrice = s.PurchasePrice,
                SourceClinicSectionName = s.Master.SourceClinicSection.Name,
                DestinationClinicSectionName = s.Master.DestinationClinicSection.Name,
                Type = "Transfer - Get",
                CreateDate = s.CreatedDate
            }).ToList();

            var send = product.TransferDetailProducts.Select(s => new MaterialProductHistoryViewModel
            {
                Guid = s.Guid,
                Num = s.Num.GetValueOrDefault(0),
                ExpireDate = s.ExpireDate,
                SellingPrice = s.SellingPrice,
                PurchasePrice = s.PurchasePrice,
                SourceClinicSectionName = s.Master.SourceClinicSection.Name,
                DestinationClinicSectionName = s.Master.DestinationClinicSection.Name,
                Type = "Transfer - Send",
                CreateDate = s.CreatedDate
            }).ToList();

            var buy = product.PurchaseInvoiceDetails.Select(s => new MaterialProductHistoryViewModel
            {
                Guid = s.Guid,
                Num = s.Num.GetValueOrDefault(0) + s.FreeNum.GetValueOrDefault(0),
                ExpireDate = s.ExpireDate,
                SellingPrice = s.SellingPrice,
                PurchasePrice = s.PurchasePrice,
                SourceClinicSectionName = "",
                DestinationClinicSectionName = "",
                Type = "Purchase",
                CreateDate = s.CreateDate
            }).ToList();

            List<MaterialProductHistoryViewModel> result = new();

            result.AddRange(get);
            result.AddRange(send);
            result.AddRange(buy);

            result = result.OrderByDescending(p => p.CreateDate).ToList();

            Indexing<MaterialProductHistoryViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }


        public IEnumerable<ProductViewModel> GetAllProductByMaterialTypeJustName(Guid clinicSectionId, string materialType)
        {
            try
            {
                IEnumerable<Product> hosp = _unitOfWork.Products.GetAllProductByMaterialTypeJustName(clinicSectionId, materialType);
                List<ProductViewModel> hospconvert = ConvertModelsListsJustName(hosp);
                return hospconvert;
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<UsableProductModel> GetAllUsableProductList(Guid ProductId, Guid ClinicSectionId)
        {
            var product = _unitOfWork.Products.GetProductCount(ProductId, ClinicSectionId);

            List<UsableProductModel> usableList = new();

            usableList.AddRange(product.PurchaseInvoiceDetails.Select(purchase => new UsableProductModel
            {
                ExpireDate = purchase.ExpireDate,
                PurchasePrice = purchase.PurchasePrice,
                SellingPrice = purchase.SellingPrice,
                RemainingNum = purchase.RemainingNum,
                PurchaseInvoiceDetailId = purchase.Guid,
                SourcePurchaseInvoiceDetailId = purchase.Guid
            }));

            usableList.AddRange(product.TransferDetailDestinationProducts.Select(purchase => new UsableProductModel
            {
                ExpireDate = purchase.ExpireDate,
                PurchasePrice = purchase.PurchasePrice,
                SellingPrice = purchase.SellingPrice,
                RemainingNum = purchase.RemainingNum,
                TransferDetailId = purchase.Guid,
                SourcePurchaseInvoiceDetailId = purchase.SourcePurchaseInvoiceDetailId
            }));


            return usableList.OrderBy(a => a.ExpireDate);

        }


        public IEnumerable<ProductWithExpireViewModel> GetAllProductExpireList(Guid id)
        {
            IEnumerable<PurchaseOrTransferProductDetail> product = _unitOfWork.PurchaseInvoiceDetails.GetAllProductExpireList(id);
            List<ProductWithExpireViewModel> result = ConvertModelListForExpire(product);
            Indexing<ProductWithExpireViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public PurchaseInvoiceDetailViewModel GetProductDetailsFromExpireList(Guid invoiceId, string invoiceType, string saleType, int? SellCurrencyId, bool latestPrice, Guid productId)
        {
            PurchaseOrTransferProductDetail product = _unitOfWork.PurchaseInvoiceDetails.GetProductDetailsFromExpireList(invoiceId, invoiceType, saleType, SellCurrencyId, latestPrice, productId);
            return ConvertModelForProductDetail(product);
        }

        public PieChartViewModel GetMostSaledProducts(Guid clinicSectionId)
        {
            try
            {
                var result = _unitOfWork.SaleInvoiceDetails.GetMostSaledProducts(clinicSectionId);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = result.Select(a => a.Label).ToArray(),
                    Value = result.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;
            }
            catch (Exception e)
            {
                throw e;
            }

        }


        public PieChartViewModel GetProductStocks(Guid clinicSectionId)
        {
            try
            {
                var result = _unitOfWork.PurchaseInvoiceDetails.GetProductStocks(clinicSectionId);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = result.Select(a => a.Label).ToArray(),
                    Value = result.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<int> GetExpiredProducts(Guid clinicSectionId)
        {
            try
            {
                var result = _unitOfWork.Products.GetExpiredProducts(clinicSectionId);
                var now = DateTime.Now;
                List<int> expired = new List<int>
                {
                    result.Where(a => a.ExpireDate.GetValueOrDefault() < now).Count(),
                    result.Where(a => a.ExpireDate.GetValueOrDefault() < now.AddMonths(3) && a.ExpireDate.GetValueOrDefault() > now).Count(),
                    result.Where(a => a.ExpireDate.GetValueOrDefault() < now.AddMonths(6) && a.ExpireDate.GetValueOrDefault() > now).Count(),
                    result.Where(a => a.ExpireDate.GetValueOrDefault() > now).Count()
                };
                return expired;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public ProductDetailResultViewModel GetProductDetailById(Guid productId, Guid clinicSectionId)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            var product = _unitOfWork.Products.GetProductDetailById(productId, clinicSectionId);

            var total = product.PurchaseInvoiceDetails.Select(p => new
            {
                ExpireDate = p.ExpireDate.Value,
                Buj = p.BujNumber,
                Num = p.RemainingNum.GetValueOrDefault(0),
                PurchasePrice = p.PurchasePrice.GetValueOrDefault(0),
                SellingPrice = p.SellingPrice.GetValueOrDefault(0),
                MiddleSellPrice = p.MiddleSellPrice.GetValueOrDefault(0),
                WholeSellPrice = p.WholeSellPrice.GetValueOrDefault(0),
                p.CurrencyName,
                p.Consideration
            }).ToList();

            total.AddRange(product.TransferDetailDestinationProducts.Select(p => new
            {
                ExpireDate = p.ExpireDate.Value,
                Buj = p.SourcePurchaseInvoiceDetail.BujNumber,
                Num = p.RemainingNum.GetValueOrDefault(0),
                PurchasePrice = p.PurchasePrice.GetValueOrDefault(0),
                SellingPrice = p.SellingPrice.GetValueOrDefault(0),
                MiddleSellPrice = p.MiddleSellPrice.GetValueOrDefault(0),
                WholeSellPrice = p.WholeSellPrice.GetValueOrDefault(0),
                p.CurrencyName,
                p.Consideration
            }));

            total = total.OrderBy(p => p.ExpireDate).ToList();

            var last = total.LastOrDefault();

            var access = _idunit.subSystem.GetUserSubSystemAccess("CanUseWholeSellPrice", "CanUseMiddleSellPrice");
            var canUseWholeSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseWholeSellPrice");
            var canUseMiddleSellPrice = access.Any(p => p.AccessName == "Edit" && p.SubSystemName == "CanUseMiddleSellPrice");

            var result = new ProductDetailResultViewModel
            {
                ProductName = product.Name,
                Consideration = last?.Consideration ?? "",
                PurchasePrice = $"{last?.PurchasePrice.ToString("#,#.##", cultures) ?? ""} {last?.CurrencyName}",
                SellingPrice = $"{last?.SellingPrice.ToString("#,#.##", cultures) ?? ""} {last?.CurrencyName}",
                MiddleSellPrice = canUseMiddleSellPrice ? $"{last?.MiddleSellPrice.ToString("#,#.##", cultures) ?? ""} {last?.CurrencyName}" : "",
                WholeSellPrice = canUseWholeSellPrice ? $"{last?.WholeSellPrice.ToString("#,#.##", cultures) ?? ""} {last?.CurrencyName}" : "",
                Profit = $"% {(((last?.SellingPrice - last?.PurchasePrice) / last?.PurchasePrice) * 100).GetValueOrDefault(0).ToString("#,#.##", cultures)}"
            };

            result.ProductExp = total.Where(p => p.Num > 0).GroupBy(p => p.ExpireDate).Select(p => new ProductDetailViewModel
            {
                Result = p.Key.ToString("dd/MM/yyyy", cultures),
                Value = p.Sum(x => x.Num).ToString("#,#.##", cultures)
            });

            result.ProductBuj = total.Where(p => p.Num > 0).OrderBy(p => p.Buj).GroupBy(p => p.Buj).Select(p => new ProductDetailViewModel
            {
                Result = p.Key ?? "",
                Value = p.Sum(x => x.Num).ToString("#,#.##", cultures)
            });

            var supp = product.PurchaseInvoiceDetails.Select(p => p.Master.Supplier.User.Name).ToList();
            supp.AddRange(product.TransferDetailDestinationProducts.Select(p => p.SourcePurchaseInvoiceDetail.Master.Supplier.User.Name));

            result.Suppliers = supp.Distinct().Select(p => new ProductDetailViewModel
            {
                Value = p
            });

            return result;
        }

        public IEnumerable<ProductPricesViewModel> GetProductLastPricesByProducId(Guid productId, Guid transferId)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var prices = _unitOfWork.PurchaseInvoiceDetails.GetProductLastPricesByProducId(productId, transferId);
            var result = prices.Select(p => new ProductPricesViewModel
            {
                Guid = p.Guid,
                InvoiceType = p.InvoiceType,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                BujNumber = p.BujNumber,
                Consideration = p.Consideration,
                RemainingNum = p.RemainingNum.ToString("#,0.##", cultures),
                PurchasePrice = $"{p.PurchasePrice.ToString("#,0.##", cultures)} {p.CurrencyName}",
                SellingPrice = $"{p.SellingPrice.ToString("#,0.##", cultures)} {p.CurrencyName}",
                MiddleSellPrice = $"{p.MiddleSellPrice.ToString("#,0.##", cultures)} {p.CurrencyName}",
                WholeSellPrice = $"{p.WholeSellPrice.ToString("#,0.##", cultures)} {p.CurrencyName}",
            }).ToList();

            Indexing<ProductPricesViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductCardexReportResultViewModel> GetProductCardexReport(ProductReportFilterViewModel viewModel, IStringLocalizer<SharedResource> _localizer)
        {
            var result = new List<ProductCardexReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var cardex = _unitOfWork.Products.GetProductCardexReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            decimal rem = 0;
            result = cardex.OrderBy(p => p.CreateDate).Select(p => new ProductCardexReportResultViewModel
            {
                Type = p.Type,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Description = $"{_localizer[p.Type]} {p.Name} {p.InvoiceNum}",
                InNum = p.InNum.GetValueOrDefault(0),
                InFreeNum = p.InFree.GetValueOrDefault(0),
                PurchasePrice = p.PurchasePrice.GetValueOrDefault(0),
                OutNum = p.OutNum.GetValueOrDefault(0),
                OutFreeNum = p.OutFreeNum.GetValueOrDefault(0),
                SalePrice = p.SalePrice.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                BujNumber = p.BujNumber,
                RemainingNum = rem = CalculateRem(rem, p.InNum, p.InFree, p.OutNum, p.OutFreeNum)
            }).ToList();

            Indexing<ProductCardexReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }


        private static decimal CalculateRem(decimal last, decimal? InNum, decimal? InFreeNum, decimal? OutNum, decimal? OutFreeNum)
        {
            return last + InNum.GetValueOrDefault(0) + InFreeNum.GetValueOrDefault(0) - OutNum.GetValueOrDefault(0) - OutFreeNum.GetValueOrDefault(0);
        }


        public IEnumerable<ProductReportResultViewModel> GetProductPurchaseReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductPurchaseReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                MainInvoiceNum = p.MainInvoiceNum,
                Supplier_Customer = p.Name,
                BujNumber = p.BujNumber,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                FreeNum = p.FreeNum.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                Discount = p.Discount.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                Description = p.Consideration,
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductReportResultViewModel> GetProductSaleReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductSaleReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                MainInvoiceNum = p.MainInvoiceNum,
                Supplier_Customer = p.Name,
                BujNumber = p.BujNumber,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                FreeNum = p.FreeNum.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                Discount = p.Discount.GetValueOrDefault(0),
                CurrencyName = p.PurchaseCurrencyName,
                Description = p.Consideration,
                SalePrice = p.SalePrice,
                SaleCurrencyName = p.SaleCurrencyName,
                MoneyConvert = p.MoneyConvert == null ? "" : (p.MoneyConvert.BaseAmount.Value + p.MoneyConvert.BaseCurrency.Name + " = " + p.MoneyConvert.DestAmount.Value + p.MoneyConvert.DestCurrency.Name)
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductReportResultViewModel> GetProductReturnPurchaseReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductReturnPurchaseReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Supplier_Customer = p.Name,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                FreeNum = p.FreeNum.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                Discount = p.Discount.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                Reason = p.Reason
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductReportResultViewModel> GetProductReturnSaleReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductReturnSaleReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum,
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Supplier_Customer = p.Name,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                FreeNum = p.FreeNum.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                Discount = p.Discount.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                Reason = p.Reason
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductReportResultViewModel> GetProductTransferReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductTransferReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum.ToString(),
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Supplier_Customer = p.Name,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                Description = p.Consideration
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ProductReportResultViewModel> GetProductReceiveReport(ProductReportFilterViewModel viewModel)
        {
            var result = new List<ProductReportResultViewModel>();

            if (viewModel.ProductId == Guid.Empty)
                return result;

            if (viewModel.Year == 0)
            {
                viewModel.DateFrom = viewModel.DateTo = DateTime.Now;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateFrom))
                    viewModel.DateFrom = DateFrom;

                if (DateTime.TryParseExact(viewModel.DateFromTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime DateTo))
                    viewModel.DateTo = DateTo;
            }
            else
            {
                viewModel.DateFrom = new DateTime(viewModel.Year, 1, 1, 0, 0, 0);
                viewModel.DateTo = new DateTime(viewModel.Year, 12, 31, 23, 59, 59);
            }

            CultureInfo cultures = new CultureInfo("en-US");
            var purchases = _unitOfWork.Products.GetProductReceiveReport(viewModel.ProductId, viewModel.ClinicSectionId, viewModel.DateFrom, viewModel.DateTo, viewModel.CurrencyId);
            result = purchases.Select(p => new ProductReportResultViewModel
            {
                Guid = p.Guid,
                InvoiceNum = p.InvoiceNum.ToString(),
                InvoiceDate = p.InvoiceDate.ToString("dd/MM/yyyy", cultures),
                Supplier_Customer = p.Name,
                ExpireDate = p.ExpireDate.ToString("dd/MM/yyyy", cultures),
                Num = p.Num.GetValueOrDefault(0),
                Price = p.PurchasePrice.GetValueOrDefault(0),
                CurrencyName = p.CurrencyName,
                Description = p.Consideration
            }).ToList();

            Indexing<ProductReportResultViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public IEnumerable<ExpireListViewModel> GetExpiredList(Guid clinicSectionId, string type)
        {
            IEnumerable<ExpireListModel> product = _unitOfWork.PurchaseInvoiceDetails.GetExpiredList(clinicSectionId, type);
            List<ExpireListViewModel> result = Common.ConvertModels<ExpireListViewModel, ExpireListModel>.convertModelsLists(product);
            Indexing<ExpireListViewModel> indexing = new();
            return indexing.AddIndexing(result);
        }

        public PurchaseInvoiceDetailViewModel ConvertModelForProductDetail(PurchaseOrTransferProductDetail product)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseOrTransferProductDetail, PurchaseInvoiceDetailViewModel>()
                .ForMember(a => a.PurchasePriceTxt, b => b.MapFrom(c => c.PurchasePrice.GetValueOrDefault().ToString("#,#.##", cultures) + " " + c.PurchaseCurrencyName))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.SellingCurrencyName))
                .ForMember(a => a.OldSalePrice, b => b.MapFrom(c => c.SellingPrice))
                .ForMember(a => a.Stock, b => b.MapFrom(c => c.Stock.ToString("#,#.##", cultures)))
                .ForMember(a => a.TotalStock, b => b.MapFrom(c => c.TotalStock.ToString("#,#.##", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<PurchaseOrTransferProductDetail, PurchaseInvoiceDetailViewModel>(product);
        }

        public List<ProductWithExpireViewModel> ConvertModelListForExpire(IEnumerable<PurchaseOrTransferProductDetail> product)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PurchaseOrTransferProductDetail, ProductWithExpireViewModel>()
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<PurchaseOrTransferProductDetail>, List<ProductWithExpireViewModel>>(product);
        }


        // Begin Convert 
        public ProductViewModel ConvertModel(Product product)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductViewModel>()
                .ForMember(a => a.ProducerName, b => b.MapFrom(c => c.Producer.Name))
                .ForMember(a => a.ProductTypeName, b => b.MapFrom(c => c.ProductType.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Product, ProductViewModel>(product);
        }

        public List<MaterialStoreroomViewModel> ConvertMaterialModelsLists(IEnumerable<Product> products)
        {
            List<MaterialStoreroomViewModel> productDtoList = new List<MaterialStoreroomViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, MaterialStoreroomViewModel>()
                .ForMember(a => a.ProducerName, b => b.MapFrom(c => c.Producer.Name))
                .ForMember(a => a.ProductTypeName, b => b.MapFrom(c => c.ProductType.Name))
                //.ForMember(a => a.Count, b => b.MapFrom(c => c.PurchaseInvoiceDetails.Sum(s => s.Num.GetValueOrDefault(0) + s.FreeNum.GetValueOrDefault(0)) - c.TransferDetailProducts.Sum(s => s.Num.GetValueOrDefault(0)) + c.TransferDetailDestinationProducts.Sum(s => s.Num.GetValueOrDefault(0))))
                .ForMember(a => a.Count, b => b.MapFrom(c => c.PurchaseInvoiceDetails.Sum(s => s.RemainingNum.GetValueOrDefault(0)) + c.TransferDetailDestinationProducts.Sum(s => s.RemainingNum.GetValueOrDefault(0))))
                ;
            });

            IMapper mapper = config.CreateMapper();
            productDtoList = mapper.Map<IEnumerable<Product>, List<MaterialStoreroomViewModel>>(products);
            return productDtoList;
        }

        public List<ProductStoreroomViewModel> ConvertProductModelsLists(IEnumerable<Product> products)
        {
            List<ProductStoreroomViewModel> productDtoList = new List<ProductStoreroomViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductStoreroomViewModel>()
                .ForMember(a => a.ProducerName, b => b.MapFrom(c => c.Producer.Name))
                .ForMember(a => a.Warning, b => b.MapFrom(c => c.PurchaseInvoiceDetails.Any() || c.TransferDetailDestinationProducts.Any()))
                .ForMember(a => a.ProductTypeName, b => b.MapFrom(c => c.ProductType.Name))
                .ForMember(a => a.Count, b => b.MapFrom(c => c.PurchaseInvoiceDetails.Sum(s => s.RemainingNum.GetValueOrDefault(0)) + c.TransferDetailDestinationProducts.Sum(s => s.RemainingNum.GetValueOrDefault(0))))
                ;
            });

            IMapper mapper = config.CreateMapper();
            productDtoList = mapper.Map<IEnumerable<Product>, List<ProductStoreroomViewModel>>(products);
            return productDtoList;
        }

        public List<ProductViewModel> ConvertModelsListsJustName(IEnumerable<Product> products)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductViewModel>()
                .ForMember(a => a.ProducerName, b => b.MapFrom(c => c.Producer.Name))
                .ForMember(a => a.ProductTypeName, b => b.MapFrom(c => c.ProductType.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductViewModel>>(products);
        }






        // End Convert
    }
}
