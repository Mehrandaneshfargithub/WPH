using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WPH.Helper;
using WPH.Models.Service;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ServiceMvcMockingService : IServiceMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ServiceMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<ServiceViewModel> GetAllService(Guid clinicSectionId)
        {
            IEnumerable<Service> services = _unitOfWork.Services.GetAllService(p => p.ClinicSectionId == clinicSectionId);
            List<ServiceViewModel> dtoServices = ConvertModelsLists(services);
            Indexing<ServiceViewModel> indexing = new();
            return indexing.AddIndexing(dtoServices);
        }

        public bool CheckRepeatedServiceName(Guid clinicSectionId, string name, bool NewOrUpdate, string oldName = "")
        {
            Service room = null;
            if (NewOrUpdate)
            {
                room = _unitOfWork.Services.GetSingle(x => x.Name.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
            }
            else
            {
                room = _unitOfWork.Services.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
            }
            if (room != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Service/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }


        public OperationStatus RemoveService(Guid Serviceid)
        {
            try
            {
                Service Rom = _unitOfWork.Services.Get(Serviceid);
                _unitOfWork.Services.Remove(Rom);
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

        public string AddNewService(ServiceViewModel viewModel)
        {
            if (viewModel.TypeId == null || string.IsNullOrWhiteSpace(viewModel.Name) || viewModel.Price.GetValueOrDefault(0) < 0)
                return "ERROR_Data";

            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Operation", "ServiceType");
            if (viewModel.TypeId == typeId && ((viewModel.DoctorWage.GetValueOrDefault(0) <= 0) || string.IsNullOrWhiteSpace(viewModel.OperationTypeName)))
                return "ERROR_Data";

            var exist = _unitOfWork.Services.CheckServiceExist(p => p.Name == viewModel.Name && p.ClinicSectionId == viewModel.ClinicSectionId);
            if (exist)
                return "ValueIsRepeated";

            Service ServiceDb = Common.ConvertModels<Service, ServiceViewModel>.convertModels(viewModel);

            if (!string.IsNullOrWhiteSpace(viewModel.OperationTypeName))
            {
                var operationType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.OperationTypeName, "OperationType", viewModel.ClinicSectionId);
                ServiceDb.OperationTypeId = operationType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (ServiceDb.OperationTypeId == null)
                {
                    ServiceDb.OperationType = new BaseInfo
                    {
                        Name = viewModel.OperationTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = operationType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(ServiceDb.OperationType);
                }
            }

            _unitOfWork.Services.Add(ServiceDb);
            _unitOfWork.Complete();
            return ServiceDb.Guid.ToString();
        }

        public string UpdateService(ServiceViewModel viewModel)
        {
            if (viewModel.TypeId == null || string.IsNullOrWhiteSpace(viewModel.Name) || viewModel.Price.GetValueOrDefault(0) < 0)
                return "ERROR_Data";

            var typeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Operation", "ServiceType");
            if (viewModel.TypeId == typeId && ((viewModel.DoctorWage.GetValueOrDefault(0) <= 0) || string.IsNullOrWhiteSpace(viewModel.OperationTypeName)))
                return "ERROR_Data";

            var exist = _unitOfWork.Services.CheckServiceExist(p => p.Guid != viewModel.Guid && p.Name == viewModel.Name && p.ClinicSectionId == viewModel.ClinicSectionId);
            if (exist)
                return "ValueIsRepeated";

            Service Servicedb = Common.ConvertModels<Service, ServiceViewModel>.convertModels(viewModel);

            if (!string.IsNullOrWhiteSpace(viewModel.OperationTypeName))
            {
                var operationType = _unitOfWork.BaseInfos.GetTypeWithBaseInfoByNameAndType(viewModel.OperationTypeName, "OperationType", viewModel.ClinicSectionId);
                Servicedb.OperationTypeId = operationType?.BaseInfos?.FirstOrDefault()?.Guid;
                if (Servicedb.OperationTypeId == null)
                {
                    Servicedb.OperationType = new BaseInfo
                    {
                        Name = viewModel.OperationTypeName,
                        ClinicSectionId = viewModel.ClinicSectionId,
                        TypeId = operationType.Guid
                    };

                    _unitOfWork.BaseInfos.Add(Servicedb.OperationType);
                }
            }

            _unitOfWork.Services.UpdateState(Servicedb);
            _unitOfWork.Complete();
            return Servicedb.Guid.ToString();
        }

        public IEnumerable<ServiceViewModel> GetAllSpeceficServices(string serviceType, Guid clinicSectionId)
        {
            IEnumerable<Service> allService = _unitOfWork.Services.GetAllSpeceficServices(serviceType, clinicSectionId);
            return Common.ConvertModels<ServiceViewModel, Service>.convertModelsLists(allService);
        }

        public IEnumerable<ServiceViewModel> GetAllServicesExceptOperation(Guid clinicSectionId)
        {
            IEnumerable<Service> allService = _unitOfWork.Services.GetAllServicesExceptOperation(clinicSectionId);
            return Common.ConvertModels<ServiceViewModel, Service>.convertModelsLists(allService);

        }

        public ServiceViewModel GetService(Guid ServiceId)
        {
            Service ServiceDb = _unitOfWork.Services.GetService(ServiceId);
            return ConvertModel(ServiceDb);

        }

        public ServiceViewModel GetServiceType(string typeName)
        {
            return new ServiceViewModel
            {
                TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType(typeName, "ServiceType"),
                TypeName = typeName
            };
        }

        public List<ServiceReportViewModel> GetAllOperationsForReport(Guid clinicSectionId)
        {
            IEnumerable<Service> allService = _unitOfWork.Services.GetAllOperationsForReport(clinicSectionId);
            return allService.Select(p => new ServiceReportViewModel
            {
                ServiceName = p.Name,
                Temp_Price = p.Price,
                TypeName = p.Type.Name
            }).ToList();
        }

        public List<ServiceViewModel> ConvertModelsLists(IEnumerable<Service> services)
        {
            List<ServiceViewModel> serviceDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceViewModel>()
                .ForMember(p => p.TypeName, r => r.MapFrom(s => s.OperationTypeId == null ? s.Type.Name : $"{s.Type.Name} - {s.OperationType.Name}"))
                //.ForMember(p => p.OperationTypeName, r => r.MapFrom(s => s.OperationType.Name))
                ;
            });

            IMapper mapper = config.CreateMapper();
            serviceDtoList = mapper.Map<IEnumerable<Service>, List<ServiceViewModel>>(services);
            return serviceDtoList;
        }

        public ServiceViewModel ConvertModel(Service service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Service, ServiceViewModel>()
                .ForMember(p => p.OperationTypeName, r => r.MapFrom(s => s.OperationType.Name));
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Service, ServiceViewModel>(service);
        }

    }
}
