using DataLayer.EntityModels;
using DataLayer.Repositories;
using System.Collections.Generic;
using System.Linq;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ClinicMvcMockingService : IClinicMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public string GetClinicName()
        {
            IEnumerable<Clinic> clinics = _unitOfWork.Clinics.GetAll();
            return clinics.FirstOrDefault().Name;

        }

    }
}
