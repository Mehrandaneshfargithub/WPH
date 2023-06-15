using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.BaseInfo;
using WPH.Models.Clinic;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Clinic;
using WPH.Models.CustomDataModels.ClinicSection;
using WPH.Models.CustomDataModels.Dashboard;
using WPH.Models.Hospital;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class ClinicSectionMvcMockingService : IClinicSectionMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicSectionMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ClinicSection/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";
        }


        public List<ClinicSectionSettingValueViewModel> GetAllClinicSectionSettingValues(Guid ClinicSectionId)
        {
            IEnumerable<ClinicSectionSettingValue> clinicSectionSettingValues = _unitOfWork.ClinicSectionSettingValues.GetAllClinicSectionSettingValueWithSetting(ClinicSectionId);
            return ConvertModelClinicSectionSettingsValue(clinicSectionSettingValues);

        }

        public List<ClinicSectionSettingViewModel> GetAllClinicSectionSettingsBasedOnSectionType(Guid clinicSectionId, int? sectionTypeId)
        {
            IEnumerable<ClinicSectionSetting> res = _unitOfWork.ClinicSectionSettingValues.GetAllClinicSectionSettingsBasedOnSectionType(clinicSectionId, sectionTypeId);
            List<ClinicSectionSetting> allSetting = new();
            allSetting.AddRange(res.Where(p => p.InputType != "bool").ToList());
            allSetting.AddRange(res.Where(p => p.InputType == "bool").ToList());
            return ConvertModelClinicSectionSettings(allSetting);
        }


        public OperationStatus RemoveClinicSection(Guid id)
        {
            try
            {
                ClinicSection ClinicSection = _unitOfWork.ClinicSections.Get(id);
                _unitOfWork.ClinicSections.Remove(ClinicSection);
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

        public Guid GetClinicSectionIdByName(string Name)
        {
            return _unitOfWork.ClinicSections.GetSingle(x => x.Name == Name).Guid;

        }

        public IEnumerable<SectionViewModel> GetAllUserClinicSectionsJustNameAndGuid(Guid userId, string type)
        {
            IEnumerable<ClinicSection> ClinicSectionUser = _unitOfWork.ClinicSection_Users.GetClinicSectionsForUser(userId, type);

            return ClinicSectionUser.Select(csu => new SectionViewModel { Id = csu.Guid, Name = csu.Name });
        }

        public IEnumerable<ClinicSectionViewModel> GetAllUserClinicSections(Guid userId, Guid clinicSectionId)
        {
            IEnumerable<ClinicSection> ClinicSectionUser = _unitOfWork.ClinicSection_Users.GetAllUserClinicSections(userId, clinicSectionId);

            return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(ClinicSectionUser);
        }

        public bool ClinicSectionHasChild(Guid cliniSectionId, Guid userId)
        {
            return _unitOfWork.ClinicSections.ClinicSectionHasChild(cliniSectionId, userId);
        }

        public IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsChild(Guid cliniSectionId, Guid userId)
        {
            IEnumerable<ClinicSection> clinicSectionDtos = _unitOfWork.ClinicSections.GetAllClinicSectionsChild(cliniSectionId, userId).OrderBy(a => a.Priority);
            return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(clinicSectionDtos);
        }

        public IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsChildForTransferSource(Guid clinicSectionId, Guid userId)
        {
            IEnumerable<ClinicSection> clinicSectionDtos = _unitOfWork.ClinicSections.GetAllClinicSectionsChildForTransferSource(clinicSectionId, userId).OrderBy(a => a.Priority);
            return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(clinicSectionDtos);
        }

        public IEnumerable<ClinicSectionViewModel> GetAllMainClinicSectionsExceptOne(Guid cliniSectionId)
        {
            IEnumerable<ClinicSection> clinicSectionDtos = _unitOfWork.ClinicSections.GetAllMainClinicSectionsExceptOne(cliniSectionId);
            return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(clinicSectionDtos);
        }

        public ClinicSectionViewModel GetClinicSectionById(Guid ClinicSectionId)
        {
            try
            {
                ClinicSection clinicSectionDtos = _unitOfWork.ClinicSections.GetWithSectionName(ClinicSectionId);
                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModels(clinicSectionDtos);
            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ClinicSectionViewModel> GetAllClinicSectionsBasedOnClinicId(Guid clinicId)
        {
            IEnumerable<ClinicSection> clinicSection = _unitOfWork.ClinicSections.Find(x => x.ClinicId == clinicId).OrderBy(a => a.Priority);
            List<ClinicSectionViewModel> clinicSections = Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(clinicSection).ToList();
            Indexing<ClinicSectionViewModel> indexing = new Indexing<ClinicSectionViewModel>();
            return indexing.AddIndexing(clinicSections);
        }

        public List<ClinicSectionViewModel> GetAllClinicSectionWithType(Guid clinicId)
        {
            IEnumerable<ClinicSection> clinicSection = _unitOfWork.ClinicSections.GetAllClinicSectionWithType(clinicId).OrderBy(a => a.Priority);
            List<ClinicSectionViewModel> clinicSections = ConvertModelclinicSectionwithType(clinicSection).ToList();
            Indexing<ClinicSectionViewModel> indexing = new Indexing<ClinicSectionViewModel>();
            return indexing.AddIndexing(clinicSections);
        }


        public bool CheckRepeatedClinicNameAndCode(string name, bool NewOrUpdate, string systemCode, string nameHolder = "", string systemCodeHolder = "")
        {
            try
            {
                Clinic clinic = null;
                if (systemCode == null)
                {
                    systemCode = "";
                }
                if (NewOrUpdate)
                {
                    clinic = _unitOfWork.Clinics.GetSingle(x => x.Name.Trim() == name.Trim() && x.SystemCode.Trim() == systemCode.Trim());
                }
                else
                {
                    clinic = _unitOfWork.Clinics.GetSingle(x => x.Name.Trim() == name.Trim() && x.Name.Trim() != nameHolder && x.SystemCode.Trim() == systemCode.Trim() && x.SystemCode.Trim() != systemCodeHolder);
                }
                if (clinic != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public List<ClinicSectionViewModel> GetClinicSectionsForUser(Guid userId, string type, Guid? parentId = null)
        {
            try
            {

                IEnumerable<ClinicSection> ClinicSectionUser = _unitOfWork.ClinicSection_Users.GetClinicSectionsForUser(userId, type, parentId).OrderBy(a => a.Priority);


                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(ClinicSectionUser).ToList();
            }
            catch (Exception e) { return null; }
        }

        public List<ClinicSectionViewModel> GetAllClinicSectionParentsForUser(Guid userId, Guid clinicId)
        {
            try
            {

                IEnumerable<ClinicSection> ClinicSectionUser = _unitOfWork.ClinicSection_Users.GetAllClinicSectionParentsForUser(userId, clinicId);

                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(ClinicSectionUser);

            }
            catch (Exception e) { return null; }
        }

        public IEnumerable<ClinicSectionViewModel> GetAllParentClinicSections()
        {
            try
            {
                IEnumerable<ClinicSection> ClinicSectionUser = _unitOfWork.ClinicSections.GetAllParentClinicSections().OrderBy(a => a.Priority);
                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(ClinicSectionUser).ToList();
            }
            catch (Exception ex) { return null; }
        }


        public Guid UpdateClinicSection(ClinicSectionViewModel clinicSection)
        {
            ClinicSection clinicSectionDto = Common.ConvertModels<ClinicSection, ClinicSectionViewModel>.convertModels(clinicSection);
            ClinicSection oldClinicSection = _unitOfWork.ClinicSections.Get(clinicSectionDto.Guid);
            _unitOfWork.ClinicSections.Detach(oldClinicSection);
            _unitOfWork.ClinicSections.UpdateState(clinicSectionDto);
            _unitOfWork.Complete();
            return clinicSectionDto.Guid;
        }

        public IEnumerable<DashboardViewModel> GetAllDashboardDatas(Guid clinicSectionId)
        {
            IEnumerable<DashboardPoco> clinicSectionDtos = _unitOfWork.ClinicSections.GetAllDashboardDatas(clinicSectionId);
            return Common.ConvertModels<DashboardViewModel, DashboardPoco>.convertModelsLists(clinicSectionDtos);
        }

        public Guid AddNewClinicSection(ClinicSectionViewModel clinicSection)
        {
            try
            {
                ClinicSection ClinicSection = Common.ConvertModels<ClinicSection, ClinicSectionViewModel>.convertModels(clinicSection);

                _unitOfWork.ClinicSections.Add(ClinicSection);
                _unitOfWork.Complete();
                return ClinicSection.Guid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClinicSectionViewModel> GetAllClinicSectionsBasedOnClinicSectionId(Guid clinicSectionId)
        {
            Guid clinicId = _unitOfWork.ClinicSections.Find(x => x.Guid == clinicSectionId).SingleOrDefault().ClinicId;
            IEnumerable<ClinicSection> clinicSection = _unitOfWork.ClinicSections.Find(x => x.ClinicId == clinicId);
            return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(clinicSection).ToList();
        }

        public void SaveClinicSectionSettings(IEnumerable<ClinicSectionSettingValueViewModel> cssvmList, string rootPath, List<ClinicSectionSettingBannerViewModel> banners, Guid clinicSectionId)
        {
            IEnumerable<ClinicSectionSettingValue> clinicSectionDtos = Common.ConvertModels<ClinicSectionSettingValue, ClinicSectionSettingValueViewModel>.convertModelsLists(cssvmList);
            IEnumerable<ClinicSectionSettingValue> oldClinicSectionSettingValues = _unitOfWork.ClinicSectionSettingValues.Find(x => x.ClinicSectionId == clinicSectionId);

            _unitOfWork.ClinicSectionSettingValues.RemoveRange(oldClinicSectionSettingValues);
            _unitOfWork.ClinicSectionSettingValues.AddRange(clinicSectionDtos);

            _unitOfWork.Complete();

            foreach (var item in banners)
            {
                FileAttachments fileAttachments = new();
                var oldBanners = oldClinicSectionSettingValues.Where(p => p.SettingId == item.SettingId).Select(s => s.Svalue);
                fileAttachments.DeleteAllBanners(oldBanners, rootPath);

                if (item.Banner != null)
                    SaveBanner(clinicSectionId, rootPath, item.Banner, item.SettingId);
            }
        }

        public void SaveClinicSectionSettingValue(Guid clinicSectionId, string value, string name)
        {
            ClinicSectionSettingValue old = _unitOfWork.ClinicSectionSettingValues.GetSingle(x => x.ClinicSectionId == clinicSectionId && x.Setting.Sname == name);
            old.Svalue = value;

            _unitOfWork.ClinicSectionSettingValues.UpdateState(old);
            _unitOfWork.Complete();
        }

        public string GetClinicSectionNameById(Guid clinicSectionId)
        {
            try
            {
                ClinicSection clinicSection = _unitOfWork.ClinicSections.Get(clinicSectionId);
                return clinicSection.Name;
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<ClinicSectionSettingValueViewModel> GetClinicSectionSettingValueBySettingName(Guid clinicSectionId, params string[] settingsName)
        {
            try
            {
                var cd = _unitOfWork.ClinicSectionSettingValues.GetSettingIdBySettingName(clinicSectionId, settingsName);
                return ConvertModelClinicSectionSettingsValue(cd);
            }
            catch (Exception ex) { return new List<ClinicSectionSettingValueViewModel>(); }
        }


        public List<ClinicSectionViewModel> GetAllClinicSectionsWithChilds(Guid clinicId, bool showChild)
        {
            try
            {
                var cd = _unitOfWork.ClinicSections.GetAllClinicSectionsWithChilds(clinicId).ToList();

                //IEnumerable<ClinicSectionViewModel> child;

                if (!showChild)
                 cd.RemoveAll(a => a.ClinicSectionTypeId != null);

                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(cd);
            }
            catch (Exception ex) { return null; }
        }

        public List<ClinicSectionViewModel> GetAllAccessedUserClinicSectionWithChilds(Guid userId, bool showChild)
        {
            try
            {
                var cd = _unitOfWork.ClinicSections.GetAllAccessedUserClinicSectionWithChilds(userId).ToList();

                if (!showChild)
                    cd.RemoveAll(a => a.ClinicSectionTypeId != null);

                return Common.ConvertModels<ClinicSectionViewModel, ClinicSection>.convertModelsLists(cd);
            }
            catch (Exception ex) { return null; }
        }


        public void UpdateClinicSectionName(Guid clinicSectionId, string name)
        {
            try
            {
                _unitOfWork.ClinicSections.UpdateClinicSectionName(clinicSectionId, name);
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<Guid> GetClinicSectionChilds(List<Guid> clinicSections, Guid? UserId = null)
        {
            try
            {
                return _unitOfWork.ClinicSections.GetClinicSectionChilds(clinicSections, UserId);
            }
            catch (Exception ex) { throw ex; }
        }

        public string SaveBanner(Guid clinicSectionId, string rootPath, IFormFile banner, int SettingId)
        {
            string path = "";
            FileAttachments fileAttachments = new();
            try
            {
                var newValue = new ClinicSectionSettingValue
                {
                    Guid = Guid.NewGuid(),
                    ClinicSectionId = clinicSectionId,
                    SettingId = SettingId
                };
                path = fileAttachments.UploadBanner(banner, rootPath, "", newValue.Guid.ToString(), $"\\Uploads\\Banner\\");

                if (string.IsNullOrWhiteSpace(path))
                    return "SomeThingWentWrong";

                newValue.Svalue = path;
                _unitOfWork.ClinicSectionSettingValues.Add(newValue);

                _unitOfWork.Complete();
                return "";
            }
            catch (Exception)
            {
                if (!string.IsNullOrWhiteSpace(path))
                    fileAttachments.DeleteBanner(Path.Combine(rootPath + path));

                return "SomeThingWentWrong";
            }

        }

        public string GetBanner(Guid clinicSectionId, string settingName)
        {
            try
            {
                var setting = _unitOfWork.ClinicSectionSettingValues.GetBySettingName(clinicSectionId, settingName);
                if (setting == null)
                    return "";

                return setting.Svalue;
            }
            catch (Exception)
            {
                return "";
            }

        }

        public IEnumerable<ClinicSectionViewModel> ConvertModelclinicSectionwithType(IEnumerable<ClinicSection> settings)
        {
            List<ClinicSectionViewModel> clinicSectionSetting = new List<ClinicSectionViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClinicSection, ClinicSectionViewModel>();
                cfg.CreateMap<BaseInfoGeneral, BaseInfoGeneralViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            clinicSectionSetting = mapper.Map<IEnumerable<ClinicSection>, List<ClinicSectionViewModel>>(settings);
            return clinicSectionSetting;
        }

        public List<ClinicSectionSettingValueViewModel> ConvertModelClinicSectionSettingsValue(IEnumerable<ClinicSectionSettingValue> settings)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClinicSectionSettingValue, ClinicSectionSettingValueViewModel>()
                .ForMember(a => a.ClinicSectionSettingSName, b => b.MapFrom(c => c.Setting.Sname))
                .ForMember(a => a.ShowSName, b => b.MapFrom(c => c.Setting.Sname))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<ClinicSectionSettingValue>, List<ClinicSectionSettingValueViewModel>>(settings);
        }


        public List<ClinicSectionSettingViewModel> ConvertModelClinicSectionSettings(IEnumerable<ClinicSectionSetting> settings)
        {
            List<ClinicSectionSettingViewModel> clinicSectionSetting = new List<ClinicSectionSettingViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ClinicSectionSetting, ClinicSectionSettingViewModel>();
                cfg.CreateMap<ClinicSectionSettingValue, ClinicSectionSettingValueViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            clinicSectionSetting = mapper.Map<IEnumerable<ClinicSectionSetting>, List<ClinicSectionSettingViewModel>>(settings);
            return clinicSectionSetting;
        }

        
    }
}
