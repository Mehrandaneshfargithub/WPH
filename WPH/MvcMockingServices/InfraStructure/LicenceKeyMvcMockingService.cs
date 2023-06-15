using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using WPH.Areas.Admin.Models.LicenceKeyManagement;
using WPH.Helper;
using WPH.Models.LicenceKey;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class LicenceKeyMvcMockingService : ILicenceKeyMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LicenceKeyMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public OperationStatus RemoveLicenceKey(int id)
        {
            try
            {
                var licenceKey = _unitOfWork.LicenceKeys.GetSingle(p => p.Id == id);
                _unitOfWork.LicenceKeys.Remove(licenceKey);

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

        public string AddNewLicenceKey(string licenceKey)
        {
            CheckLicence checkLicence = new CheckLicence();
            var key = new LicenceKey()
            {
                SerialKey = licenceKey,
                ComputerSerial = checkLicence.GetComputerSerial()
            };

            _unitOfWork.LicenceKeys.Add(key);
            _unitOfWork.Complete();
            return key.Id.ToString();
        }

        public string CheckLicence()
        {
            CheckLicence checkLicence = new CheckLicence();

            var licence = _unitOfWork.LicenceKeys.GetLastLicence(checkLicence.GetComputerSerial());
            if (licence == null)
                return "KeyNotExists";

            var result = checkLicence.CheckAccess(licence.SerialKey);

            return result;
        }

        public IEnumerable<LicenceKeyManagementViewModel> GetAll()
        {
            var keys = _unitOfWork.LicenceKeys.GetAll();

            var result = ConvertModelList(keys);

            Indexing<LicenceKeyManagementViewModel> indexing = new Indexing<LicenceKeyManagementViewModel>();
            return indexing.AddIndexing(result);
        }

        // Begin Convert 
        public LicenceKeyViewModel ConvertModel(LicenceKey Users)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LicenceKey, LicenceKeyViewModel>()
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<LicenceKey, LicenceKeyViewModel>(Users);
        }

        public List<LicenceKeyManagementViewModel> ConvertModelList(IEnumerable<LicenceKey> licenceKeys)
        {
            CheckLicence ch = new CheckLicence();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LicenceKey, LicenceKeyManagementViewModel>()
                .ForMember(a => a.Date, b => b.MapFrom(c => ch.GetDate(c.SerialKey)))
                ;
            });
            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<LicenceKey>, List<LicenceKeyManagementViewModel>>(licenceKeys);
        }
        // End Convert
    }
}
