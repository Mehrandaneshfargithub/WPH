using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.Chart;
using WPH.Models.ReceptionInsurance;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.Models.ReceptionServiceReceived;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceptionServiceReceivedMvcMockingService : IReceptionServiceReceivedMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ReceptionServiceReceivedMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ReceptionServiceReceived/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public string PayService(ReceptionServiceReceivedViewModel viewModel, bool InvoiceNumAndPayerNameRequired)
        {
            if ( (InvoiceNumAndPayerNameRequired) && (string.IsNullOrWhiteSpace(viewModel.PayerName?.Trim()) || string.IsNullOrWhiteSpace(viewModel.ReceptionInvoiceNum?.Trim())))
                return "ERROR_Data";

            if (viewModel.Amount == null || viewModel.Amount.Value <= 0 || viewModel.AmountStatus == null )
                return "ERROR_Data";

            viewModel.Guid = Guid.NewGuid();
            viewModel.CreatedDate = DateTime.Now;
            if (viewModel.CurrencyId == null)
                viewModel.CurrencyId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("IQD", "Currency");

            var entity = ConvertModelReverce(viewModel);
            _unitOfWork.ReceptionServiceReceiveds.Add(entity);
            _unitOfWork.Complete();

            var rs = _unitOfWork.ReceptionServices.GetReceptionServiceWithRecives(viewModel.ReceptionServiceId.Value);

            var recived_money = rs.ReceptionServiceReceiveds.Where(p => !p.AmountStatus.Value).Sum(p => p.Amount.Value);
            var returned_money = rs.ReceptionServiceReceiveds.Where(p => p.AmountStatus.Value).Sum(p => p.Amount.Value);

            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
            var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

            if ((recived_money - returned_money) < (rs.Price * rs.Number) - rs.Discount.GetValueOrDefault(0))
                rs.StatusId = unPaid;
            else
                rs.StatusId = paid;

            _unitOfWork.ReceptionServices.UpdateState(rs);
            _unitOfWork.Complete();

            var reception = _unitOfWork.Receptions.GetReceptionWithServices(rs.ReceptionId.Value);
            reception.ReceptionInvoiceNum = viewModel.ReceptionInvoiceNum;
            var pay = reception.ReceptionServices.All(p => p.StatusId == paid);
            reception.PaymentStatusId = pay ? paid : unPaid;

            _unitOfWork.Receptions.UpdateState(reception);
            _unitOfWork.Complete();
            return "";
        }

        public string PayAllServices(PayAllServiceViewModel viewModel, bool InvoiceNumAndPayerNameRequired)
        {

            if ((InvoiceNumAndPayerNameRequired) && (string.IsNullOrWhiteSpace(viewModel.PayerName?.Trim()) || string.IsNullOrWhiteSpace(viewModel.ReceptionInvoiceNum?.Trim()) || viewModel.ReceptionId == Guid.Empty))
                return "DataNotValid";

            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

            var services = _unitOfWork.ReceptionServices.GetUnpaidReceptionServicesByReceptionId(viewModel.ReceptionId);

            foreach (var item in services)
            {
                var pay = new ReceptionServiceReceived
                {
                    Guid = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    CurrencyId = item.DiscountCurrencyId,
                    CreatedUserId = viewModel.UserId,
                    AmountStatus = false,
                    PayerName = viewModel.PayerName,
                    Amount = item.Price,
                    ReceptionServiceId = item.Guid
                };
                _unitOfWork.ReceptionServiceReceiveds.Add(pay);

                var service = _unitOfWork.ReceptionServices.Get(item.Guid);
                service.StatusId = paid;
                _unitOfWork.ReceptionServices.UpdateState(service);
            }

            var reception = _unitOfWork.Receptions.Get(viewModel.ReceptionId);
            reception.PaymentStatusId = paid;
            reception.ReceptionInvoiceNum = viewModel.ReceptionInvoiceNum;
            ReceptionInsuranceReceived RIR = new ReceptionInsuranceReceived();
            ReceptionInsurance RI = new ReceptionInsurance();
            if (viewModel.Insurance != 0)
            {
                var receptionInsurance = _unitOfWork.ReceptionInsurances.GetSingle(x=>x.ReceptionId == viewModel.ReceptionId);
                if (receptionInsurance != null)
                {

                    RIR.Amount = Convert.ToDecimal(viewModel.Insurance);
                    RIR.CreatedDate = DateTime.Now;
                    RIR.CreatedUserId = viewModel.UserId;
                    RIR.PayerName = viewModel.PayerName;
                    RIR.AmountStatus = false;
                    RIR.ReceptionInsuranceId = receptionInsurance.Guid;
                    _unitOfWork.ReceptionInsuranceReceiveds.Add(RIR);
                }
                else
                {
                    RIR.Amount = Convert.ToDecimal(viewModel.Insurance);
                    RIR.CreatedDate = DateTime.Now;
                    RIR.CreatedUserId = viewModel.UserId;
                    RIR.PayerName = viewModel.PayerName;
                    RIR.AmountStatus = false;
                    RI.ReceptionId = viewModel.ReceptionId;
                    RI.CreatedDate = DateTime.Now;
                    RI.CreatedUserId = viewModel.UserId;

                    RI.ReceptionInsuranceReceiveds = new List<ReceptionInsuranceReceived>();
                    RI.ReceptionInsuranceReceiveds.Add(RIR);
                    _unitOfWork.ReceptionInsurances.Add(RI);
                }

                

            }

            _unitOfWork.Receptions.UpdateState(reception);
            _unitOfWork.Complete();

            return "";
        }

        public IEnumerable<ReceptionInsuranceReceivedViewModel> GetAllReceptionInsuranceReceived(Guid receptionId)
        {
            IEnumerable<ReceptionInsuranceReceived> Re = _unitOfWork.ReceptionInsuranceReceiveds.GetAllReceptionInsuranceReceived(receptionId);
            List<ReceptionInsuranceReceivedViewModel> doc = ConvertModelsListsInsurance(Re);
            Indexing<ReceptionInsuranceReceivedViewModel> indexing = new Indexing<ReceptionInsuranceReceivedViewModel>();
            return indexing.AddIndexing(doc);
        }


        public void PayInstallment(PayAllServiceViewModel viewModel)
        {

            IEnumerable<ReceptionService> unPaidServices = _unitOfWork.ReceptionServices.GetUnpaidReceptionServicesByReceptionId(viewModel.ReceptionId);
            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

            Guid installment = Guid.NewGuid();

            if (viewModel.Amount >= unPaidServices.Sum(a => a.Price))
            {
                var reception = _unitOfWork.Receptions.Get(viewModel.ReceptionId);
                reception.PaymentStatusId = paid;
                _unitOfWork.Receptions.UpdateState(reception);
            }

            if (DateTime.TryParseExact(viewModel.RecievedDateTxt, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
                viewModel.RecievedDate = date;
            else
                viewModel.RecievedDate = DateTime.Now;
            List<ReceptionServiceReceived> allRecei = new List<ReceptionServiceReceived>();

            foreach (var rese in unPaidServices)
            {
                allRecei.Add(new ReceptionServiceReceived
                {
                    Amount = (viewModel.Amount >= rese.Price) ? (rese.Price) : (viewModel.Amount),
                    ReceptionServiceId = rese.Guid,
                    CreatedUserId = viewModel.CreatedUserId,
                    CreatedDate = viewModel.RecievedDate,
                    AmountStatus = false,
                    InstallmentId = installment
                });

                

                if (viewModel.Amount >= rese.Price)
                {
                    var service = _unitOfWork.ReceptionServices.Get(rese.Guid);
                    service.StatusId = paid;
                    _unitOfWork.ReceptionServices.UpdateState(service);
                }

                viewModel.Amount -= rese.Price;
                if (viewModel.Amount <= 0)
                    break;
            }

            _unitOfWork.ReceptionServiceReceiveds.AddRange(allRecei);
            _unitOfWork.Complete();
        }

        public IEnumerable<ReceptionServiceReceivedViewModel> GetAllReceptionServiceRecievedForInstallment(Guid receptionId)
        {
            try
            {
                var result = _unitOfWork.ReceptionServiceReceiveds.GetAllReceptionServiceRecievedForInstallment(receptionId);

                var recives = result.GroupBy(a => a.InstallmentId ).Select(a => new ReceptionServiceReceivedViewModel
                {
                    Amount = a.Sum(a => a.Amount),
                    CreatedDate = a.FirstOrDefault().CreatedDate,
                    InstallmentId = a.Key
                }).OrderBy(a=>a.CreatedDate).ToList();

                //var doc = Common.ConvertModels<ReceptionServiceReceivedViewModel, ReceptionServiceReceived>.convertModelsLists(recives);
                Indexing<ReceptionServiceReceivedViewModel> indexing = new Indexing<ReceptionServiceReceivedViewModel>();
                return indexing.AddIndexing(recives);
            }
            catch (Exception e) { throw e; }
        }

        public OperationStatus RemoveInstallment(Guid id)
        {
            var result = _unitOfWork.ReceptionServiceReceiveds.Find(a => a.InstallmentId == id);

            var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

            foreach(var install in result)
            {
                var receptionService = _unitOfWork.ReceptionServices.Get(install.ReceptionServiceId??Guid.Empty);
                receptionService.StatusId = unPaid;
                _unitOfWork.ReceptionServices.UpdateState(receptionService);
            }

            var rece =_unitOfWork.Receptions.Get(result.FirstOrDefault().ReceptionService.ReceptionId ?? Guid.Empty);
            rece.PaymentStatusId = unPaid;
            _unitOfWork.Receptions.UpdateState(rece);
            _unitOfWork.ReceptionServiceReceiveds.RemoveRange(result);
            _unitOfWork.Complete();
            return OperationStatus.SUCCESSFUL;
        }


        public PieChartViewModel GetAllClinicInCome(Guid userId, string type)
        {
            IEnumerable<ReceptionServiceReceived> all =  _unitOfWork.ReceptionServiceReceiveds.GetAllClinicInCome(userId);
            List<PieChartModel> pi = new List<PieChartModel>();
            if (string.Compare(type, "day", true) == 0)
            {
                
                for(int i = 1; i < 8; i++)
                {
                    pi.Add(new PieChartModel
                    {
                        Label = DateTime.Now.AddDays(i - 7).Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Value = all.Where(a => a.CreatedDate == DateTime.Now.AddDays(i - 7).Date).Sum(a => a.Amount)
                    });
                }

            }
            else if(string.Compare(type, "month", true) == 0)
            {

                for (int i = 1; i < 13; i++)
                {

                    DateTime thismonth = new DateTime(DateTime.Now.Year, i, 1);

                    string mo = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(thismonth.Month);

                    pi.Add(new PieChartModel
                    {
                        Label = mo,
                        Value = all.Where(a => a.CreatedDate.Value.Month == i).Sum(a => a.Amount)
                    });
                }

                
            }
            else
            {

                int firstyear = all.Min(a=>a.CreatedDate.Value.Year);
                int currentyear = DateTime.Now.Year;

                for (int i = firstyear; i <= currentyear; i++)
                {

                    

                    pi.Add(new PieChartModel
                    {
                        Label = i.ToString(),
                        Value = all.Where(a => a.CreatedDate.Value.Year == i).Sum(a => a.Amount)
                    });
                }
            }

            PieChartViewModel result = new PieChartViewModel
            {
                Labels = pi.Select(a => a.Label).ToArray(),
                Value = pi.Select(a => Convert.ToInt32(a.Value)).ToArray()
            };

            return result;

        }

        public List<ReceptionInsuranceReceivedViewModel> ConvertModelsListsInsurance(IEnumerable<ReceptionInsuranceReceived> services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionInsuranceReceived, ReceptionInsuranceReceivedViewModel>().ForMember(a => a.ReceptionInsurance, b => b.Ignore())
                .ForMember(a => a.Amount, b => b.MapFrom(c => (c.AmountStatus.GetValueOrDefault(false)) ? -c.Amount : c.Amount))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ReceptionInsuranceReceived>, List<ReceptionInsuranceReceivedViewModel>>(services);
        }

        public List<ReceptionServiceReceivedViewModel> ConvertModelsLists(IEnumerable<ReceptionServiceReceived> services)
        {
            List<ReceptionServiceReceivedViewModel> serviceDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            serviceDtoList = mapper.Map<IEnumerable<ReceptionServiceReceived>, List<ReceptionServiceReceivedViewModel>>(services);
            return serviceDtoList;
        }
        public ReceptionServiceReceivedViewModel ConvertModel(ReceptionServiceReceived service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>(service);
        }

        public ReceptionServiceReceived ConvertModelReverce(ReceptionServiceReceivedViewModel service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionServiceReceivedViewModel, ReceptionServiceReceived>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionServiceReceivedViewModel, ReceptionServiceReceived>(service);
        }

        
    }
}
