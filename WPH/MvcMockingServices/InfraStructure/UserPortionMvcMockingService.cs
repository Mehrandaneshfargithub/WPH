using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models;
using WPH.Models.CustomDataModels.UserManagment;
using WPH.Models.Reception;
using WPH.Models.UserPortion;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class UserPortionMvcMockingService : IUserPortionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserPortionMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }
        

        public IEnumerable<UserPortionViewModel> GetAllUserPortionsByClinicSection(Guid clinicSectionId)
        {
            IEnumerable<UserPortion> UserPortionDb = _unitOfWork.UserPortions.GetAllUserPortions(clinicSectionId);
            List<UserPortionViewModel> UserPortionvi = Common.ConvertModels<UserPortionViewModel, UserPortion>.convertModelsLists(UserPortionDb);
            Indexing<UserPortionViewModel> indexing = new Indexing<UserPortionViewModel>();
            return indexing.AddIndexing(UserPortionvi);
        }

        

        public OperationStatus RemoveUserPortion(Guid UserPortionid)
        {
            try
            {
                UserPortion Rom = _unitOfWork.UserPortions.Get(UserPortionid);
                _unitOfWork.UserPortions.Remove(Rom);
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

        public string AddNewUserPortion(UserPortionViewModel UserPortion)
        {

            UserPortion UserPortionDb = Common.ConvertModels<UserPortion, UserPortionViewModel>.convertModels(UserPortion);

            var UP = _unitOfWork.UserPortions.GetSingle(a => a.Guid == UserPortion.Guid);

            if(UP != null)
            {
                UP.PortionPercent = UserPortion.PortionPercent;
                UP.Specification = UserPortion.Specification;
                _unitOfWork.UserPortions.UpdateState(UP);
            }
            else
            {
                _unitOfWork.UserPortions.Add(UserPortionDb);
            }

            
            _unitOfWork.Complete();
            return UserPortionDb.Guid.ToString();
        }

        public string UpdateUserPortion(UserPortionViewModel UserPortion)
        {

            UserPortion UserPortiondb = Common.ConvertModels<UserPortion, UserPortionViewModel>.convertModels(UserPortion);
            
            _unitOfWork.UserPortions.UpdateState(UserPortiondb);
            _unitOfWork.Complete();
            return UserPortiondb.Guid.ToString();

        }



        public UserPortionViewModel GetUserPortion(Guid UserPortionId)
        {
            UserPortion UserPortionDb = _unitOfWork.UserPortions.Get(UserPortionId);
            return Common.ConvertModels<UserPortionViewModel, UserPortion>.convertModels(UserPortionDb); ;

        }

        public IEnumerable<UserPortionViewModel> GetAllUserPortionsBySpecification(Guid clinicSectionId, bool specification, Guid ReceptionId)
        {
            var result = _unitOfWork.UserPortions.GetAllUserPortionsBySpecification(clinicSectionId, specification, ReceptionId);
            return Common.ConvertModels<UserPortionViewModel, UserPortion>.convertModelsLists(result);

        }

        public IEnumerable<ReceptionDetailPayViewModel> GetAllReceptionDetailPayBySpecification(Guid receptionId, bool specification)
        {
            var result = _unitOfWork.ReceptionDetailPaies.GetAllReceptionDetailPayBySpecification(receptionId, specification);
            return Common.ConvertModels<ReceptionDetailPayViewModel, ReceptionDetailPay>.convertModelsLists(result);
        }

        public void AddReceptionDetailPay(ReceptionDetailPayViewModel portion)
        {
            try
            {
                var por = Common.ConvertModels<ReceptionDetailPay, ReceptionDetailPayViewModel>.convertModels(portion);
                _unitOfWork.ReceptionDetailPaies.Add(por);
                _unitOfWork.Complete();
            }
            catch(Exception e) { throw e; }


        }

        public OperationStatus RemoveReceptionDetailPay(Guid id)
        {
            try
            {
                ReceptionDetailPay Rom = _unitOfWork.ReceptionDetailPaies.Get(id);
                _unitOfWork.ReceptionDetailPaies.Remove(Rom);
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

        public IEnumerable<UserPortionReportViewModel> GetAllUserPortionForReport(Guid userId, DateTime fromDate, DateTime toDate, bool detail)
        {
            try
            {

                var Rom = _unitOfWork.UserPortions.GetAllUserPortionForReport(userId, fromDate, toDate, detail);

                var por = Common.ConvertModels<UserPortionReportViewModel, UserPortionReport>.convertModelsLists(Rom);

                return por;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<UserPortionReportViewModel> GetPortionReport(Guid clinicSectionId, DateTime dateFrom, DateTime dateTo, string status, Guid doctorId)
        {
            try
            {

                IEnumerable<UserPortionReport> Rom = _unitOfWork.UserPortions.GetPortionReport(clinicSectionId, dateFrom, dateTo, status, doctorId);

                var por = Common.ConvertModels<UserPortionReportViewModel, UserPortionReport>.convertModelsLists(Rom);

                return por;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
