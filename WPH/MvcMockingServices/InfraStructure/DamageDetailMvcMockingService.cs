using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.DamageDetail;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class DamageDetailMvcMockingService : IDamageDetailMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;

        public DamageDetailMvcMockingService(IUnitOfWork unitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = Idunit;
        }

        public string RemoveDamageDetail(Guid damageDetailid)
        {
            try
            {
                DamageDetail Hos = _unitOfWork.DamageDetails.Get(damageDetailid);

                var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(Hos.MasterId.Value);
                if (currencyExist == null)
                    return "DataNotValid";

                var total_purchase = currencyExist.DamageDetails.Where(p => p.CurrencyId == Hos.CurrencyId && p.Guid != damageDetailid).Sum(p => p.Num.GetValueOrDefault(0) * p.Price.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0));
                var total_discount = currencyExist.DamageDiscounts.Where(x => x.CurrencyId == Hos.CurrencyId).Sum(p => p.Amount.GetValueOrDefault(0));
                var after_discount = total_purchase - total_discount;
                if (after_discount < 0)
                    return "DiscountIsGreaterThanTheAmount";

                _unitOfWork.DamageDetails.Remove(Hos);

                if (Hos.TransferDetailId != null)
                {
                    var transfer = _unitOfWork.TransferDetails.Get(Hos.TransferDetailId.Value);

                    transfer.RemainingNum += Hos.Num.GetValueOrDefault(0) + Hos.FreeNum.GetValueOrDefault(0);
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(Hos.PurchaseInvoiceDetailId.Value);

                    purchase.RemainingNum += Hos.Num.GetValueOrDefault(0) + Hos.FreeNum.GetValueOrDefault(0);
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                //_unitOfWork.Complete();
                //_unitOfWork.DamageDetails.Detach(Hos);
                currencyExist.DamageDetails = currencyExist.DamageDetails.Where(p => p.Guid != damageDetailid).ToList();

                return _idunit.damage.UpdateTotalPrice(currencyExist);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    return OperationStatus.ERROR_ThisRecordHasDependencyOnItInAnotherEntity.ToString();
                }
                else
                {
                    return OperationStatus.ERROR_SomeThingWentWrong.ToString();
                }
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/DamageDetail/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewDamageDetail(DamageDetailViewModel viewModel, Guid clinicSectionId)
        {
            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.PurchaseInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                viewModel.ProductId == null || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            DamageDetail damageDetail = ConvertModelWithOutGuid(viewModel);
            damageDetail.CreateDate = viewModel.CreateDate ?? DateTime.Now;
            damageDetail.Price = viewModel.TotalPrice / viewModel.Num;

            if ((damageDetail.Num * damageDetail.Price) - damageDetail.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            var total_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            if (viewModel.TransferDetailId != null)
            {
                var transfer = _unitOfWork.TransferDetails.GetWithSourcePurchaseInvoice(viewModel.TransferDetailId.Value);
                if (total_num > transfer.RemainingNum)
                    return "NotEnoughProductCount";

                transfer.RemainingNum -= total_num;
                damageDetail.CurrencyId = transfer.SourcePurchaseInvoiceDetail.CurrencyId;

                _unitOfWork.TransferDetails.UpdateState(transfer);
            }
            else
            {
                var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.PurchaseInvoiceDetailId.Value);
                if (total_num > purchase.RemainingNum)
                    return "NotEnoughProductCount";

                purchase.RemainingNum -= total_num;
                damageDetail.CurrencyId = purchase.CurrencyId;

                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
            }

            _unitOfWork.DamageDetails.Add(damageDetail);
            //_unitOfWork.Complete();

            //_unitOfWork.DamageDetails.Detach(damageDetail);

            var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(damageDetail.MasterId.Value);
            damageDetail.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(damageDetail.CurrencyId.Value);
            currencyExist.DamageDetails.Add(damageDetail);

            return _idunit.damage.UpdateTotalPrice(currencyExist);
        }


        public string UpdateDamageDetail(DamageDetailViewModel viewModel, Guid clinicSectionId)
        {
            DamageDetail damageDetail = _unitOfWork.DamageDetails.Get(viewModel.Guid);

            if (damageDetail.PurchaseInvoiceDetailId != viewModel.PurchaseInvoiceDetailId || damageDetail.TransferDetailId != viewModel.TransferDetailId)
            {
                var old_total = damageDetail.Num.GetValueOrDefault(0) + damageDetail.FreeNum.GetValueOrDefault(0);

                if (damageDetail.TransferDetailId != null)
                {
                    var transfer = _unitOfWork.TransferDetails.Get(damageDetail.TransferDetailId.Value);
                    transfer.RemainingNum += old_total;
                    _unitOfWork.TransferDetails.UpdateState(transfer);
                }
                else
                {
                    var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(damageDetail.PurchaseInvoiceDetailId.Value);
                    purchase.RemainingNum += old_total;
                    _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
                }

                _unitOfWork.DamageDetails.Remove(damageDetail);

                viewModel.CreateDate = damageDetail.CreateDate;
                viewModel.CreatedUserId = damageDetail.CreatedUserId;
                viewModel.ModifiedDate = DateTime.Now;
                return AddNewDamageDetail(viewModel, clinicSectionId);
            }

            CultureInfo cultures = new CultureInfo("en-US");

            viewModel.Num = decimal.Parse(viewModel.NumTxt ?? "0", cultures);
            viewModel.FreeNum = decimal.Parse(viewModel.FreeNumTxt ?? "0", cultures);
            viewModel.Discount = decimal.Parse(viewModel.DiscountTxt ?? "0", cultures);
            viewModel.TotalPrice = decimal.Parse(viewModel.TotalPriceTxt ?? "0", cultures);

            if (viewModel.MasterId == null || viewModel.PurchaseInvoiceDetailId == null || viewModel.Num.GetValueOrDefault(0) <= 0 ||
                viewModel.ProductId == null || viewModel.TotalPrice.GetValueOrDefault(0) <= 0)
                return "DataNotValid";

            var new_num = viewModel.Num.GetValueOrDefault(0) + viewModel.FreeNum.GetValueOrDefault(0);
            var current_num = damageDetail.Num.GetValueOrDefault(0) + damageDetail.FreeNum.GetValueOrDefault(0);
            var diff = new_num - current_num;

            if (viewModel.TransferDetailId != null)
            {
                var transfer = _unitOfWork.TransferDetails.GetWithSourcePurchaseInvoice(viewModel.TransferDetailId.Value);
                if (diff > transfer.RemainingNum)
                    return "NotEnoughProductCount";

                transfer.RemainingNum -= diff;
                damageDetail.CurrencyId = transfer.SourcePurchaseInvoiceDetail.CurrencyId;

                _unitOfWork.TransferDetails.UpdateState(transfer);
            }
            else
            {
                var purchase = _unitOfWork.PurchaseInvoiceDetails.Get(viewModel.PurchaseInvoiceDetailId.Value);
                if (diff > purchase.RemainingNum)
                    return "NotEnoughProductCount";

                purchase.RemainingNum -= diff;
                damageDetail.CurrencyId = purchase.CurrencyId;

                _unitOfWork.PurchaseInvoiceDetails.UpdateState(purchase);
            }

            damageDetail.ModifiedUserId = viewModel.ModifiedUserId;
            damageDetail.ModifiedDate = DateTime.Now;
            damageDetail.Discount = viewModel.Discount;
            damageDetail.FreeNum = viewModel.FreeNum;
            damageDetail.Num = viewModel.Num;
            damageDetail.Price = viewModel.TotalPrice / viewModel.Num;

            if ((damageDetail.Num * damageDetail.Price) - damageDetail.Discount.GetValueOrDefault(0) < 0)
                return "DiscountIsGreaterThanTheAmount";

            _unitOfWork.DamageDetails.UpdateState(damageDetail);
            //_unitOfWork.Complete();

            //_unitOfWork.DamageDetails.Detach(damageDetail);

            var currencyExist = _unitOfWork.Damages.GetForUpdateTotalPrice(damageDetail.MasterId.Value);
            currencyExist.DamageDetails = currencyExist.DamageDetails.Where(p => p.Guid != damageDetail.Guid).ToList();
            damageDetail.CurrencyName = _unitOfWork.BaseInfoGenerals.GetNameById(damageDetail.CurrencyId.Value);
            currencyExist.DamageDetails.Add(damageDetail);

            return _idunit.damage.UpdateTotalPrice(currencyExist);
        }


        public IEnumerable<DamageDetailViewModel> GetAllDamageDetails(Guid damageId)
        {
            IEnumerable<DamageDetail> hosp = _unitOfWork.DamageDetails.GetAllDamageDetailByMasterId(damageId);
            List<DamageDetailViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<DamageDetailViewModel> indexing = new Indexing<DamageDetailViewModel>();
            return indexing.AddIndexing(hospconvert);
        }


        public DamageDetailViewModel GetDamageDetailForEdit(Guid damageDetailId)
        {
            try
            {
                DamageDetail DamageDetailgu = _unitOfWork.DamageDetails.GetDamageDetailForEdit(damageDetailId);
                return ConvertModel(DamageDetailgu);
            }
            catch (Exception) { return null; }
        }


        // Begin Convert 
        public DamageDetail ConvertModelWithOutGuid(DamageDetailViewModel dto)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageDetailViewModel, DamageDetail>()
                .ForMember(a => a.Guid, b => b.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<DamageDetailViewModel, DamageDetail>(dto);
        }

        public DamageDetailViewModel ConvertModel(DamageDetail dto)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageDetail, DamageDetailViewModel>()
                .ForMember(a => a.DamageDetailTxt, b => b.MapFrom(c => $"currency:{c.Currency.Name} | {(c.TransferDetailId == null ? "Purchase" : "Transfer")}:{c.PurchaseInvoiceDetail.Master.InvoiceNum} | {c.PurchaseInvoiceDetail.Master.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures)}"))
                .ForMember(a => a.NumTxt, b => b.MapFrom(c => c.Num.GetValueOrDefault(1).ToString("0.##", cultures)))
                .ForMember(a => a.FreeNumTxt, b => b.MapFrom(c => c.FreeNum.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.PriceTxt, b => b.MapFrom(c => c.Price.GetValueOrDefault(0).ToString("0.##", cultures)))
                .ForMember(a => a.DiscountTxt, b => b.MapFrom(c => c.Discount.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<DamageDetail, DamageDetailViewModel>(dto);
        }

        public List<DamageDetailViewModel> ConvertModelsLists(IEnumerable<DamageDetail> damageDetails)
        {
            CultureInfo cultures = new CultureInfo("en-US");
            List<DamageDetailViewModel> damageDetailDtoList = new List<DamageDetailViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DamageDetail, DamageDetailViewModel>()
                .ForMember(a => a.ProductName, b => b.MapFrom(c => $"{c.Product.Name} | {c.Product.ProductType.Name} | {c.Product.Producer.Name}"))
                .ForMember(a => a.CurrencyName, b => b.MapFrom(c => c.Currency.Name))
                .ForMember(a => a.PriceTxt, b => b.MapFrom(c => c.Price.GetValueOrDefault(0).ToString("0.##", cultures)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            damageDetailDtoList = mapper.Map<IEnumerable<DamageDetail>, List<DamageDetailViewModel>>(damageDetails);
            return damageDetailDtoList;
        }

        // End Convert
    }
}
