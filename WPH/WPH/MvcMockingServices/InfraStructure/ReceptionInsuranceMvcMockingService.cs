using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Models.ReceptionInsurance;
using WPH.Models.ReceptionInsuranceReceived;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceptionInsuranceMvcMockingService : IReceptionInsuranceMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReceptionInsuranceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public ReceptionInsuranceViewModel GetReceptionInsurance(Guid ReceptionId)
        {
            try
            {
                ReceptionInsurance ReceptionInsurance = _unitOfWork.ReceptionInsurances.GetSingle(x=>x.ReceptionId == ReceptionId);
                return Common.ConvertModels<ReceptionInsuranceViewModel, ReceptionInsurance>.convertModels(ReceptionInsurance);
            }
            catch { return null; }
        }

        public ReceptionInsuranceViewModel GetReceptionInsuranceWithReceiveds(Guid receptionId)
        {
            try
            {
                ReceptionInsurance ReceptionInsurance = _unitOfWork.ReceptionInsurances.GetReceptionInsuranceWithReceiveds(receptionId);
                return convertModels(ReceptionInsurance);
            }
            catch(Exception e) { return null; }
        }


        public ReceptionInsuranceViewModel convertModels(ReceptionInsurance Analysis)
        {
            ReceptionInsuranceViewModel AnalysisView = new ReceptionInsuranceViewModel();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionInsurance, ReceptionInsuranceViewModel>()
                .ForMember(a=>a.Reception,b=>b.Ignore())
                ;
                cfg.CreateMap<ReceptionInsuranceReceived, ReceptionInsuranceReceivedViewModel>();


            });
            IMapper mapper = config.CreateMapper();
            AnalysisView = mapper.Map<ReceptionInsurance, ReceptionInsuranceViewModel>(Analysis);
            return AnalysisView;
        }

    }
}
