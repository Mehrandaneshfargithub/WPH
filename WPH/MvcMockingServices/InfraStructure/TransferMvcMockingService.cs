using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Transfer;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class TransferMvcMockingService : ITransferMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;
        public TransferMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
        }

        public string RemoveTransfer(Guid transferid)
        {
            try
            {
                var transfer = _unitOfWork.Transfers.Get(transferid);

                if (transfer.ReceiverUserId != null)
                    return "TransferAccepted";

                _unitOfWork.Transfers.RemoveTransfer(transfer);
                return OperationStatus.SUCCESSFUL.ToString();
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
            string controllerName = "/Transfer/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }

        public string AddNewTransfer(TransferViewModel viewModel/*, Guid originalClinicSectionId*/)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ReceiverName) || viewModel.SourceClinicSectionId == null ||
                viewModel.DestinationClinicSectionId == null)
                return "DataNotValid";

            Transfer transfer = ConvertModel(viewModel);

            var now = DateTime.Now;
            transfer.CreatedDate = now;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseTransferDate");
            if (!access)
            {
                transfer.InvoiceDate = now;
            }
            else
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > now.Date)
                //    return "DateNotValid";

                transfer.InvoiceDate = invoiceDate;
            }

            var is_parent = _unitOfWork.ClinicSections.CheckClinicSectionIsParent(transfer.SourceClinicSectionId.Value, transfer.DestinationClinicSectionId.Value);

            transfer.ReceiverDate = (is_parent ? null : now);

            _unitOfWork.Transfers.Add(transfer);
            _unitOfWork.Complete();
            return transfer.Guid.ToString();

        }

        public string UpdateTransfer(TransferViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ReceiverName) || viewModel.SourceClinicSectionId == null ||
                viewModel.DestinationClinicSectionId == null)
                return "DataNotValid";

            Transfer transfer = _unitOfWork.Transfers.Get(viewModel.Guid);

            if (transfer.ReceiverUserId != null)
                return "TransferAccepted";

            transfer.ModifiedDate = DateTime.Now;
            transfer.ModifiedUserId = viewModel.ModifiedUserId;
            transfer.ReceiverName = viewModel.ReceiverName;
            transfer.SourceClinicSectionId = viewModel.SourceClinicSectionId;
            transfer.DestinationClinicSectionId = viewModel.DestinationClinicSectionId;
            transfer.Description = viewModel.Description;

            var access = _idunit.subSystem.CheckUserAccess("Edit", "CanUseTransferDate");
            if (access)
            {
                if (!DateTime.TryParseExact(viewModel.InvoiceDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime invoiceDate))
                    return "DateNotValid";

                //if (invoiceDate.Date > DateTime.Now.Date)
                //    return "DateNotValid";

                transfer.InvoiceDate = invoiceDate;
            }

            _unitOfWork.Transfers.UpdateState(transfer);
            _unitOfWork.Complete();
            return transfer.Guid.ToString();
        }

        public IEnumerable<TransferViewModel> GetAllTransfers(List<Guid> clinicSections, TransferFilterViewModel filterViewModel)
        {
            if (filterViewModel.PeriodId != (int)Periods.FromDateToDate)
            {
                var DateFrom = DateTime.Now;
                var DateTo = DateTime.Now;
                CommonWas.GetPeriodDateTimes(ref DateFrom, ref DateTo, filterViewModel.PeriodId);

                filterViewModel.DateFrom = DateFrom;
                filterViewModel.DateTo = DateTo;
            }

            IEnumerable<Transfer> hosp;
            if (filterViewModel.TypeId.GetValueOrDefault(0) == 1)
            {
                hosp = _unitOfWork.Transfers.GetAllTransfer(filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                       clinicSections.Contains(p.SourceClinicSectionId.Value));
            }
            else if (filterViewModel.TypeId.GetValueOrDefault(0) == 2)
            {
                hosp = _unitOfWork.Transfers.GetAllTransfer(filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                        clinicSections.Contains(p.DestinationClinicSectionId.Value));
            }
            else
            {
                hosp = _unitOfWork.Transfers.GetAllTransfer(filterViewModel.DateFrom, filterViewModel.DateTo, p =>
                       clinicSections.Contains(p.SourceClinicSectionId.Value) || clinicSections.Contains(p.DestinationClinicSectionId.Value));
            }

            List<TransferViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<TransferViewModel> indexing = new Indexing<TransferViewModel>();
            return indexing.AddIndexing(hospconvert);

        }

        public TransferViewModel GetTransfer(Guid TransferId)
        {
            try
            {
                Transfer Transfergu = _unitOfWork.Transfers.GetWithType(TransferId);
                return ConvertModel(Transfergu);
            }
            catch { return null; }
        }

        public IEnumerable<string> GetReceiversName(List<Guid> clinicSections)
        {
            return _unitOfWork.Transfers.GetReceiversName(clinicSections).ToList();
        }

        public IEnumerable<TransferReportResultViewModel> GetTransferReport(TransferReportViewModel reportViewModel)
        {
            var result = _unitOfWork.TransferDetails.GetTransferDetailReport(reportViewModel.AllClinicSectionGuids, reportViewModel.FromDate, reportViewModel.ToDate, p =>
             (string.IsNullOrWhiteSpace(reportViewModel.ReceiverName) || (p.Master.ReceiverName == reportViewModel.ReceiverName)) &&
             (reportViewModel.ProductId == null || (p.ProductId == reportViewModel.ProductId || p.DestinationProductId == reportViewModel.ProductId)) &&
             (reportViewModel.SourceClinicSectionId == null || (p.Master.SourceClinicSectionId == reportViewModel.SourceClinicSectionId)) &&
             (reportViewModel.DestinationClinicSectionId == null || (p.Master.DestinationClinicSectionId == reportViewModel.DestinationClinicSectionId))
             );

            CultureInfo cultures = new CultureInfo("en-US");
            return result.Select(p => new TransferReportResultViewModel
            {
                Date = p.Master.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures),
                CreatedUserName = p.CreatedUser.Name,
                Num = p.Num.Value.ToString("N0"),
                SourceClinicSectionName = p.Master.SourceClinicSection.Name,
                DestinationClinicSectionName = p.Master.DestinationClinicSection.Name,
                ReceiverName = p.Master.ReceiverName,
                ProductName = p.Product.Name,
                DestinationProduct = p.DestinationProduct.Name
            }).ToList();
        }

        public IEnumerable<TransferViewModel> GetAllProductRecive(Guid clinicSectionId)
        {
            var hosp = _unitOfWork.Transfers.GetAllProductRecive(clinicSectionId);

            List<TransferViewModel> hospconvert = ConvertModelsLists(hosp);
            Indexing<TransferViewModel> indexing = new Indexing<TransferViewModel>();
            return indexing.AddIndexing(hospconvert);
        }



        // Begin Convert 
        public Transfer ConvertModel(TransferViewModel Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TransferViewModel, Transfer>().ForMember(a => a.TransferDetails, b => b.Ignore())
                //.ForMember(a => a.TransferTypeName, b => b.MapFrom(c => c.TransferType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<TransferViewModel, Transfer>(Users);
        }

        public TransferViewModel ConvertModel(Transfer Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transfer, TransferViewModel>()
                //.ForMember(a => a.TransferTypeName, b => b.MapFrom(c => c.TransferType.Name))
                //.ForMember(a => a.SectionName, b => b.MapFrom(c => c.Section.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Transfer, TransferViewModel>(Users);
        }
        public List<TransferViewModel> ConvertModelsLists(IEnumerable<Transfer> transfers)
        {
            List<TransferViewModel> transferDtoList = new List<TransferViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Transfer, TransferViewModel>()
                .ForMember(a => a.SourceClinicSectionName, b => b.MapFrom(c => c.SourceClinicSection.Name))
                .ForMember(a => a.DestinationClinicSectionName, b => b.MapFrom(c => c.DestinationClinicSection.Name))
                .ForMember(a => a.CreatedUserName, b => b.MapFrom(c => c.CreatedUser.Name))
                .ForMember(a => a.ReceiverUserName, b => b.MapFrom(c => c.ReceiverUser.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            transferDtoList = mapper.Map<IEnumerable<Transfer>, List<TransferViewModel>>(transfers);
            return transferDtoList;
        }
        // End Convert
    }
}
