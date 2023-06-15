using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Chart;
using WPH.Models.CustomDataModels.BaseInfo;
using WPH.Models.CustomDataModels.Medicine;
using WPH.Models.MedicineReport;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class MedicineMvcMockingService : IMedicineMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MedicineMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public IEnumerable<MedicineViewModel> GetAllMedicines(Guid clinicSectionId)
        {
            IEnumerable<Medicine> medDtos = _unitOfWork.Medicines.GetAllMedicines(clinicSectionId);
            List<MedicineViewModel> meds = ConvertModelsLists(medDtos);
            Indexing<MedicineViewModel> indexing = new Indexing<MedicineViewModel>();
            return indexing.AddIndexing(meds);

        }

        public IEnumerable<MedicineViewModel> GetAllMedicineForListBox(Guid clinicSectionId, Guid patientId)
        {
            IEnumerable<Medicine> medDtos = _unitOfWork.Medicines.GetAllMedicineForListBox(clinicSectionId, patientId);
            List<MedicineViewModel> meds = ConvertModelsLists(medDtos);
            Indexing<MedicineViewModel> indexing = new Indexing<MedicineViewModel>();
            return indexing.AddIndexing(meds);

        }

        public IEnumerable<MedicineForVisitViewModel> GetAllMedicinesForVisitPrescription(Guid clinicSectionId)
        {
            IEnumerable<MedicineForVisit> medDtos = _unitOfWork.Medicines.GetAllMedicinesForVisitPrescription(clinicSectionId);
            return Common.ConvertModels<MedicineForVisitViewModel, MedicineForVisit>.convertModelsLists(medDtos);

        }
        public IEnumerable<MedicineForVisitViewModel> GetAllMedicinesForDisease(Guid clinicSectionId, Guid diseaseId, bool all)
        {
            IEnumerable<MedicineForVisit> medDtos = _unitOfWork.Medicines.GetAllMedicinesForDisease(clinicSectionId, diseaseId, all);
            //return convertModelsLists(medDtos).ToList();
            return Common.ConvertModels<MedicineForVisitViewModel, MedicineForVisit>.convertModelsLists(medDtos);
        }
        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/Medicine/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public Guid AddNewMedicine(MedicineViewModel newMedicine, Guid clinicSectionId)
        {
            try
            {
                newMedicine.ClinicSectionid = clinicSectionId;
                if (string.IsNullOrWhiteSpace(newMedicine.MedicineFormName) || string.IsNullOrWhiteSpace(newMedicine.ProducerName) || string.IsNullOrWhiteSpace(newMedicine.JoineryName))
                    return Guid.Empty;

                //if (!string.IsNullOrWhiteSpace(viewModel.Barcode) && _unitOfWork.Products.CheckRepeatedProductBarcode(viewModel.Name, viewModel.Barcode))
                //    return "RepeatedBarcode"; 

                var typeId = _unitOfWork.BaseInfos.GetIdByNameAndType(newMedicine.MedicineFormName, "MedicineForm", clinicSectionId);
                newMedicine.MedicineFormId = typeId;

                var producerId = _unitOfWork.BaseInfos.GetIdByNameAndType(newMedicine.ProducerName, "Producer", clinicSectionId);
                newMedicine.ProducerId = producerId;

                Medicine medicineDto = Common.ConvertModels<Medicine, MedicineViewModel>.convertModels(newMedicine);

                if (typeId == null || typeId == Guid.Empty)
                {
                    var baseInfo = new BaseInfo
                    {
                        Name = newMedicine.MedicineFormName,
                        TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("MedicineForm"),
                        ClinicSectionId = clinicSectionId
                    };

                    medicineDto.MedicineForm = baseInfo;
                }

                if (producerId == null || producerId == Guid.Empty)
                {
                    var baseInfo = new BaseInfo
                    {
                        Name = newMedicine.ProducerName,
                        TypeId = _unitOfWork.BaseInfos.GetBaseInfoTypeIdByName("Producer"),
                        ClinicSectionId = clinicSectionId
                    };

                    medicineDto.Producer = baseInfo;
                }
                
                IEnumerable<Medicine> medDtos = _unitOfWork.Medicines.GetAllMedicines(clinicSectionId);
                if (medDtos.Any())
                {
                    var maxp = medDtos.Max(n => n.Priority);
                    medicineDto.Priority = maxp + 1;
                }
                else
                {
                    medicineDto.Priority = 1;
                }

                _unitOfWork.Medicines.Add(medicineDto);
                _unitOfWork.Complete();
                return medicineDto.Guid;
            }
            catch (Exception ex) { throw ex; }
        }



        public Guid UpdateMedicine(MedicineViewModel med)
        {
            try
            {
                Medicine medicineDto = Common.ConvertModels<Medicine, MedicineViewModel>.convertModels(med);
                _unitOfWork.Medicines.UpdateState(medicineDto);
                _unitOfWork.Complete();
                return medicineDto.Guid;
            }
            catch (Exception ex) { throw ex; }

        }

        public void SwapPriority(Guid medicineId, Guid clinicSectionId, string type)
        {
            try
            {
                Medicine currentMedicine = _unitOfWork.Medicines.GetSingle(x => x.Guid == medicineId);
                int? currentMedicinePriority = currentMedicine.Priority;
                Medicine swapMedicine = new Medicine();
                if (type == "Up")
                {
                    swapMedicine = _unitOfWork.Medicines.Find(x => x.ClinicSectionId == clinicSectionId && x.Priority < currentMedicinePriority).OrderByDescending(x => x.Priority).FirstOrDefault();
                }
                else if (type == "Down")
                {
                    swapMedicine = _unitOfWork.Medicines.Find(x => x.ClinicSectionId == clinicSectionId && x.Priority > currentMedicinePriority).OrderBy(x => x.Priority).FirstOrDefault();
                }
                currentMedicine.Priority = swapMedicine.Priority;
                swapMedicine.Priority = currentMedicinePriority;
                _unitOfWork.Medicines.UpdatePriority(currentMedicine);
                _unitOfWork.Medicines.UpdatePriority(swapMedicine);
                _unitOfWork.Complete();
            }
            catch { }
        }

        public OperationStatus RemoveMedicine(Guid medicineId)
        {
            try
            {
                Medicine medicine = _unitOfWork.Medicines.Get(medicineId);
                IEnumerable<MedicineDisease> medicineDiseases = _unitOfWork.MedicineDiseases.Find(x => x.MedicineId == medicineId);
                if (medicineDiseases.Any())
                    _unitOfWork.MedicineDiseases.RemoveRange(medicineDiseases);
                _unitOfWork.Medicines.Remove(medicine);
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

        public MedicineViewModel GetMedicine(Guid medicineId)
        {
            try
            {
                Medicine medicineDto = _unitOfWork.Medicines.Get(medicineId);
                return ConvertModel(medicineDto);
            }
            catch { return null; }
        }

        public bool CheckRepeatedMedicineName(string name, Guid clinicSectionId, bool NewOrUpdate, string oldName = "")
        {
            try
            {
                Medicine medicine = null;
                if (NewOrUpdate)
                {
                    medicine = _unitOfWork.Medicines.GetSingle(x => x.JoineryName.Trim() == name.Trim() && x.ClinicSectionId == clinicSectionId);
                }
                else
                {
                    medicine = _unitOfWork.Medicines.GetSingle(x => x.JoineryName.Trim() == name.Trim() && x.JoineryName.Trim() != oldName && x.ClinicSectionId == clinicSectionId);
                }
                if (medicine != null)
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



        public List<MedicineReportViewModel> GetMedicineReport(Guid clinicSectionId, DateTime fromDate, DateTime toDate, Guid medicineId, Guid producerId)
        {
            try
            {
                List<PrescriptionDetail> AllMedicine = _unitOfWork.Medicines.GetMedicineReport(clinicSectionId, fromDate, toDate, medicineId, producerId);

                IEnumerable<Guid> AllMedicineId = AllMedicine.Select(x => x.Medicine.Guid).Distinct();

                List<MedicineReportViewModel> allMedicineReports = new List<MedicineReportViewModel>();

                foreach (Guid id in AllMedicineId)
                {
                    MedicineReportViewModel medicineReport = new ();
                    medicineReport.MedicineName = AllMedicine.FirstOrDefault(x => x.Medicine.Guid == id).Medicine.JoineryName;
                    medicineReport.ProducerName = AllMedicine.FirstOrDefault(x => x.Medicine.Guid == id).Medicine.Producer.Name;
                    //medicineReport.Count = AllMedicine.Count(x=>x.Guid == id);
                    medicineReport.Count = AllMedicine.Where(x => x.Medicine.Guid == id).Sum(x => Convert.ToInt32(x.Num));
                    allMedicineReports.Add(medicineReport);
                }



                return allMedicineReports;
            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdateMedicineNum(Guid PreId, Guid MedicineId, string num, string status)
        {
            try
            {
                _unitOfWork.Medicines.UpdateMedicineNum(PreId, MedicineId, num, status);
                _unitOfWork.Complete();
            }
            catch (Exception e) { throw e; }
        }

        public IEnumerable<MedicineViewModel> GetAllExpiredMedicines(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<Medicine> allMed = _unitOfWork.Medicines.GetAllExpiredMedicines(clinicSectionId);
                return Common.ConvertModels<MedicineViewModel, Medicine>.convertModelsLists(allMed);

            }
            catch (Exception e) { throw e; }
        }

        public PieChartViewModel GetMostUsedMedicine(Guid clinicSectionId)
        {
            try
            {
                IEnumerable<PieChartModel> allMed = _unitOfWork.Medicines.GetMostUsedMedicine(clinicSectionId);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = allMed.Select(a => a.Label).ToArray(),
                    Value = allMed.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;

            }
            catch (Exception e) { throw e; }
        }

        public static List<MedicineViewModel> ConvertModelsLists(IEnumerable<Medicine> meds)
        {

            List<MedicineViewModel> MedicineDtoList = new List<MedicineViewModel>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Medicine, MedicineViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.Medicine_Disease, b => b.Ignore())
                .ForMember(x => x.PrescriptionDetails, b => b.Ignore())
                .ForMember(x => x.PatientMedicineRecords, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            MedicineDtoList = mapper.Map<IEnumerable<Medicine>, List<MedicineViewModel>>(meds);

            return MedicineDtoList;
        }

        public static MedicineViewModel ConvertModel(Medicine meds)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Medicine, MedicineViewModel>()
                .ForMember(x => x.ClinicSection, b => b.Ignore())
                .ForMember(x => x.Medicine_Disease, b => b.Ignore())
                .ForMember(x => x.PrescriptionDetails, b => b.Ignore())
                .ForMember(x => x.PatientMedicineRecords, b => b.Ignore())
                ;
                cfg.CreateMap<BaseInfo, BaseInfoViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<Medicine, MedicineViewModel>(meds);

        }

        
    }

}
