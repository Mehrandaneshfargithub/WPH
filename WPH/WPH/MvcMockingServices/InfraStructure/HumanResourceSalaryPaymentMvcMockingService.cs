using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.HumanResourceSalaryPayment;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class HumanResourceSalaryPaymentMvcMockingService : IHumanResourceSalaryPaymentMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public HumanResourceSalaryPaymentMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/HumanResourceSalaryPayment/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public HumanResourceSalaryPaymentViewModel GetPayment(Guid paymentId)
        {
            var payment = _unitOfWork.HumanResourceSalaryPayments.Get(paymentId);
            return ConvertModel(payment);
        }

        public IEnumerable<HumanResourceSalaryPaymentViewModel> GetAllPaymentSalary(Guid humanSalaryId)
        {
            IEnumerable<HumanResourceSalaryPayment> payments = _unitOfWork.HumanResourceSalaryPayments.GetHumanResourceSalaryPaymentsByHumanSalaryId(humanSalaryId);

            List<HumanResourceSalaryPaymentViewModel> humanResourceSalaries = ConvertModelsLists(payments);
            Indexing<HumanResourceSalaryPaymentViewModel> indexing = new Indexing<HumanResourceSalaryPaymentViewModel>();
            return indexing.AddIndexing(humanResourceSalaries);
        }

        public string PaySalary(HumanResourceSalaryPaymentViewModel viewModel)
        {
            if (viewModel.Amount == null || viewModel.Amount.Value <= 0 || viewModel.HumanResourceSalaryId == null || viewModel.HumanResourceSalaryId == Guid.Empty)
                return "ERROR_Data";

            var rem_money = _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(viewModel.HumanResourceSalaryId.Value);
            if (viewModel.Amount > rem_money)
                return "ERROR_Money";

            viewModel.CreatedDate = DateTime.Now;
            if (viewModel.CurrencyId == null)
                viewModel.CurrencyId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("IQD", "Currency");

            var entity = ConvertModelReverce(viewModel);
            _unitOfWork.HumanResourceSalaryPayments.Add(entity);

            var hr = _unitOfWork.HumanResourceSalaries.Get(viewModel.HumanResourceSalaryId.Value);


            if (rem_money - viewModel.Amount > 0)
                hr.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            else
                hr.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

            _unitOfWork.HumanResourceSalaries.UpdateState(hr);
            _unitOfWork.Complete();

            return "";
        }


        public string UpdateSalary(HumanResourceSalaryPaymentViewModel viewModel)
        {
            if (viewModel.Amount == null || viewModel.Amount.Value <= 0 || viewModel.HumanResourceSalaryId == null || viewModel.HumanResourceSalaryId == Guid.Empty)
                return "ERROR_Data";

            var payment = _unitOfWork.HumanResourceSalaryPayments.Get(viewModel.Guid);
            var change = viewModel.Amount - payment.Amount;
            var rem_money = _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(viewModel.HumanResourceSalaryId.Value);
            if (change > rem_money)
                return "ERROR_Money";

            payment.Amount = viewModel.Amount;
            payment.Description = viewModel.Description;
            payment.ModifiedUserId = viewModel.ModifiedUserId;
            payment.ModifiedDate = DateTime.Now;
            if (viewModel.CurrencyId != null)
                payment.CurrencyId = viewModel.CurrencyId;

            _unitOfWork.HumanResourceSalaryPayments.UpdateState(payment);

            var hr = _unitOfWork.HumanResourceSalaries.Get(viewModel.HumanResourceSalaryId.Value);


            if (rem_money - change > 0)
                hr.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            else
                hr.PaymentStatusId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

            _unitOfWork.HumanResourceSalaries.UpdateState(hr);
            _unitOfWork.Complete();

            return "";
        }

        public OperationStatus RemovePayment(Guid paymentId)
        {
            try
            {
                var payment = _unitOfWork.HumanResourceSalaryPayments.Get(paymentId);
                _unitOfWork.HumanResourceSalaryPayments.Remove(payment);

                var hr = _unitOfWork.HumanResourceSalaries.Get(payment.HumanResourceSalaryId.Value);

                var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
                hr.PaymentStatusId = unPaid;

                _unitOfWork.HumanResourceSalaries.UpdateState(hr);

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


        public string PayAllSalaries(Guid humanResourceId, Guid userId, string description)
        {
            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");

            var humanResourceSalaries = _unitOfWork.HumanResourceSalaries.GetUnpaidHumanSalariesByHumanId(humanResourceId);

            foreach (var item in humanResourceSalaries)
            {
                var pay = new HumanResourceSalaryPayment
                {
                    HumanResourceSalaryId = item.Guid,
                    CreatedDate = DateTime.Now,
                    CurrencyId = item.CurrencyId,
                    CreatedUserId = userId,
                    Description = description,
                    Amount = item.Salary,
                    
                };
                _unitOfWork.HumanResourceSalaryPayments.Add(pay);

                var humanResourceSalary = _unitOfWork.HumanResourceSalaries.Get(item.Guid);
                humanResourceSalary.PaymentStatusId = paid;
                _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSalary);
            }

            _unitOfWork.Complete();

            return "";
        }


        public List<HumanResourceSalaryPaymentViewModel> ConvertModelsLists(IEnumerable<HumanResourceSalaryPayment> services)
        {
            List<HumanResourceSalaryPaymentViewModel> serviceDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalaryPayment, HumanResourceSalaryPaymentViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            serviceDtoList = mapper.Map<IEnumerable<HumanResourceSalaryPayment>, List<HumanResourceSalaryPaymentViewModel>>(services);
            return serviceDtoList;
        }
        public HumanResourceSalaryPaymentViewModel ConvertModel(HumanResourceSalaryPayment service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalaryPayment, HumanResourceSalaryPaymentViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResourceSalaryPayment, HumanResourceSalaryPaymentViewModel>(service);
        }

        public HumanResourceSalaryPayment ConvertModelReverce(HumanResourceSalaryPaymentViewModel service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<HumanResourceSalaryPaymentViewModel, HumanResourceSalaryPayment>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<HumanResourceSalaryPaymentViewModel, HumanResourceSalaryPayment>(service);
        }

    }
}
