using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.Cash;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceptionInsuranceReceivedMvcMockingService : IReceptionInsuranceReceivedMvcMockingService
    { 
        private readonly IUnitOfWork _unitOfWork;

        public ReceptionInsuranceReceivedMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public void AddNewReceptionInsuranceReceived(ReceptionInsuranceReceivedViewModel rIR)
        {
            try
            {
                ReceptionInsuranceReceived re = Common.ConvertModels<ReceptionInsuranceReceived, ReceptionInsuranceReceivedViewModel>.convertModels(rIR);
                _unitOfWork.ReceptionInsuranceReceiveds.Add(re);
                _unitOfWork.Complete();
            }
            catch { }
         }

        public void ReturnInsurance(PayAllServiceViewModel viewModel)
        {
            try
            {

                var ReceptionInsuranceId = _unitOfWork.ReceptionInsurances.Find(a => a.ReceptionId == viewModel.ReceptionId).OrderBy(a => a.Id).LastOrDefault().Guid;
                ReceptionInsuranceReceived re = new ReceptionInsuranceReceived()
                {
                    Amount = viewModel.Amount,
                    AmountStatus = true,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = viewModel.UserId,
                    PayerName = viewModel.PayerName,
                    ReceptionInsuranceId = ReceptionInsuranceId
                };
                _unitOfWork.ReceptionInsuranceReceiveds.Add(re);
                _unitOfWork.Complete();
            }
            catch { }
        }
    }
}
