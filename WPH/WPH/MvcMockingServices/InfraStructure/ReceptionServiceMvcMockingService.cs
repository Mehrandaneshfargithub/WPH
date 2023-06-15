using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using DMSDataLayer.EntityModels;
using DMSDataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Cash;
using WPH.Models.Chart;
using WPH.Models.ReceptionService;
using WPH.Models.ReceptionServiceReceived;
using WPH.Models.Service;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ReceptionServiceMvcMockingService : IReceptionServiceMvcMockingService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IDMSUnitOfWork _DMSunitOfWork;
        private readonly IDIUnit _idunit;
        public ReceptionServiceMvcMockingService(IUnitOfWork unitOfWork, IDMSUnitOfWork DMSunitOfWork, IDIUnit Idunit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _DMSunitOfWork = DMSunitOfWork ?? new DMSUnitOfWork();
            _idunit = Idunit;
        }



        public IEnumerable<ReceptionServiceViewModel> GetReceptionServicesByReceptionId(Guid receptionId, string DMS)
        {
            try
            {
                List<ReceptionService> services = _unitOfWork.ReceptionServices.GetReceptionServicesByReceptionId(receptionId).ToList();
                
                List<ReceptionServiceViewModel> dtoServices = ConvertModelsLists(services);
                Indexing<ReceptionServiceViewModel> indexing = new();
                return indexing.AddIndexing(dtoServices);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public IEnumerable<ReceptionServiceViewModel> GetReceptionSpecificServicesByReceptionId(Guid receptionId, string serviceType)
        {
            IEnumerable<ReceptionService> services = _unitOfWork.ReceptionServices.GetReceptionSpecificServicesByReceptionId(receptionId, serviceType);
            List<ReceptionServiceViewModel> dtoServices = ConvertModelsLists(services);
            Indexing<ReceptionServiceViewModel> indexing = new();
            return indexing.AddIndexing(dtoServices);
        }

        public IEnumerable<ReceptionServiceViewModel> GetAllReceptionProducts(Guid receptionId, string DMS)
        {

            try
            {
                List<ReceptionService> services;

                services = _unitOfWork.ReceptionServices.GetAllReceptionProducts(receptionId).ToList();

                List<ReceptionServiceViewModel> dtoServices = ConvertModelsListsForReceptionProduct(services);
                Indexing<ReceptionServiceViewModel> indexing = new();
                return indexing.AddIndexing(dtoServices);
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public Guid GetReceptionOperationService(Guid receptionId)
        {
            try
            {
                ReceptionService reception = _unitOfWork.ReceptionServices.GetReceptionOperation(receptionId);
                return reception.ServiceId ?? Guid.Empty;
            }
            catch (Exception e) { throw e; }
        }

        public ReceptionServiceViewModel GetReceptionExceptOperationService(Guid receptionId)
        {
            try
            {
                ReceptionService reception = _unitOfWork.ReceptionServices.GetReceptionExceptOperationService(receptionId);
                return Common.ConvertModels<ReceptionServiceViewModel, ReceptionService>.convertModels(reception);
            }
            catch (Exception e) { throw e; }
        }

        public void AddReceptionService(ReceptionServiceViewModel service)
        {
            try
            {
                var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
                ReceptionService receptionService = Common.ConvertModels<ReceptionService, ReceptionServiceViewModel>.convertModels(service);
                receptionService.StatusId = unPaid;
                if (service.DiscountPercent.GetValueOrDefault(0) > 0 && service.Discount.GetValueOrDefault(0) == 0)
                {
                    receptionService.Discount = service.Number * service.Price * service.DiscountPercent / 100;
                }

                List<ReceptionService> all = new();
                var reception = _unitOfWork.Receptions.Get(receptionService.ReceptionId.Value);

                if (service.ProductId != null && service.ProductId != Guid.Empty)
                {

                    var AllUsableProductList = _idunit.product.GetAllUsableProductList(service.ProductId.Value, reception.ClinicSectionId.Value);
                    var totalNum = service.Number;

                    if (totalNum > AllUsableProductList.Sum(a => a.RemainingNum))
                    {
                        Exception e = new Exception("NotEnoughProductCount");
                        throw e;
                    }

                    foreach (var usableProduct in AllUsableProductList)
                    {
                        if (totalNum > 0)
                        {
                            if (totalNum >= usableProduct.RemainingNum)
                            {
                                all.Add(new ReceptionService
                                {

                                    Number = service.Number,
                                    Price = usableProduct.SellingPrice,
                                    ReceptionId = service.ReceptionId,
                                    ServiceDate = service.ServiceDate,
                                    CreatedDate = DateTime.Now,
                                    CreatedUserId = service.CreatedUserId,
                                    ProductId = service.ProductId,
                                    StatusId = unPaid,
                                    PurchaseInvoiceDetailId = usableProduct.PurchaseInvoiceDetailId,
                                    TransferDetailId = usableProduct.TransferDetailId

                                });

                                if (usableProduct.TransferDetailId.HasValue)
                                {
                                    TransferDetail td = _unitOfWork.TransferDetails.Get(usableProduct.TransferDetailId.Value);
                                    td.RemainingNum = 0;
                                }
                                else
                                {
                                    PurchaseInvoiceDetail pid = _unitOfWork.PurchaseInvoiceDetails.Get(usableProduct.PurchaseInvoiceDetailId.Value);
                                    pid.RemainingNum = 0;
                                }
                                totalNum -= usableProduct.RemainingNum;

                            }
                            else
                            {
                                all.Add(new ReceptionService
                                {

                                    Number = service.Number,
                                    Price = usableProduct.SellingPrice,
                                    ReceptionId = service.ReceptionId,
                                    ServiceDate = service.ServiceDate,
                                    CreatedDate = DateTime.Now,
                                    CreatedUserId = service.CreatedUserId,
                                    ProductId = service.ProductId,
                                    StatusId = unPaid,
                                    PurchaseInvoiceDetailId = usableProduct.PurchaseInvoiceDetailId,
                                    TransferDetailId = usableProduct.TransferDetailId

                                });

                                if (usableProduct.TransferDetailId.HasValue)
                                {
                                    TransferDetail td = _unitOfWork.TransferDetails.Get(usableProduct.TransferDetailId.Value);
                                    td.RemainingNum -= totalNum;
                                }
                                else
                                {
                                    PurchaseInvoiceDetail pid = _unitOfWork.PurchaseInvoiceDetails.Get(usableProduct.PurchaseInvoiceDetailId.Value);
                                    pid.RemainingNum -= totalNum;
                                }

                                totalNum = 0;
                            }
                        }

                    }

                }


                try
                {

                    if (service.ProductId == null || service.ProductId == Guid.Empty)
                    {
                        all.Add(receptionService);
                    }

                    var recService = PayServiceByInsurance(receptionService.ReceptionId ?? Guid.Empty, all);
                    _unitOfWork.ReceptionServices.AddRange(recService);
                    reception.PaymentStatusId = recService.LastOrDefault().StatusId;
                    _unitOfWork.Complete();

                }
                catch (Exception e) { }


            }
            catch (Exception e) { throw e; }
        }


        public List<ReceptionService> PayServiceByInsurance(Guid receptionId, List<ReceptionService> receptionService)
        {
            var RI = _unitOfWork.ReceptionInsurances.GetSingle(a => a.ReceptionId == receptionService.FirstOrDefault().ReceptionId);
            if (RI != null)
            {
                decimal totalInsuranceAmount = _unitOfWork.ReceptionInsuranceReceiveds
                    .GetAllReceptionInsuranceReceived(receptionId).Sum(a => (a.AmountStatus.GetValueOrDefault(false)) ? -a.Amount : a.Amount) ?? 0;
                decimal totalServiceAmount = _unitOfWork.ReceptionServiceReceiveds
                    .Find(a => a.ReceptionInsuranceId == RI.Guid).Sum(a => a.Amount) ?? 0;
                var Paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
                decimal all = totalInsuranceAmount - totalServiceAmount;

                foreach (var service in receptionService)
                {
                    if (all > 0)
                    {
                        decimal amount = Convert.ToDecimal((service.Price * service.Number) - (service.Discount ?? 0));
                        var rem = all - amount;
                        //all -= amount;
                        decimal pay = 0;
                        if (rem >= 0)
                        {
                            pay = amount;
                            service.StatusId = Paid;
                        }
                        else
                        {
                            pay = all;
                        }

                        ReceptionServiceReceived rsr = new ReceptionServiceReceived()
                        {
                            Amount = pay,
                            AmountStatus = false,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = service.CreatedUserId,
                            ReceptionInsuranceId = RI.Guid,
                        };
                        service.ReceptionServiceReceiveds.Add(rsr);
                        all -= amount;
                    }
                }



            }
            return receptionService;
        }


        public void GetModalsViewBags(dynamic viewBag)
        {
            string controllerName = "/ReceptionService/";
            viewBag.AddNewLink = controllerName + "AddOrUpdate";
            viewBag.EditLink = controllerName + "AddOrUpdate";
            viewBag.DeleteLink = controllerName + "Remove";
            viewBag.GridDeleteLink = controllerName + "Remove?Id=";
            viewBag.GridEditLink = controllerName + "EditModal?Id=";
            viewBag.GridAddLink = controllerName + "AddNewModal";
            viewBag.GridRefreshLink = controllerName + "RefreshGrid";

        }

        public ReceptionServiceViewModel GetReceptionService(Guid id)
        {
            try
            {
                ReceptionService reception = _unitOfWork.ReceptionServices.GetReceptionServiceWithReception(id);
                return ConvertModel(reception);
            }
            catch { return null; }
        }


        public DoctorWageViewModel GetReceptionOperationAndDoctor(Guid receptionId)
        {
            try
            {
                ReceptionService reception = _unitOfWork.ReceptionServices.GetReceptionOperationAndDoctor(receptionId);

                var result = ConvertCustomModel(reception);

                var doctor_guid = reception.Reception.Surgeries.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Surgery1")?.DoctorId;
                var anesthesiologist_guid = reception.Reception.Surgeries?.FirstOrDefault().SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Anesthesiologist")?.DoctorId;
                var pediatrician_guid = reception.Reception.Surgeries?.FirstOrDefault().SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Pediatrician")?.DoctorId;
                var resident_guid = reception.Reception.Surgeries?.FirstOrDefault().SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident")?.DoctorId;

                var surgeryId = reception.Reception.Surgeries.FirstOrDefault().Guid;
                var salaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType");

                var doctorSallary = _unitOfWork.HumanResourceSalaries.Find(f => f.HumanResourceId == doctor_guid
                                                                && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId).FirstOrDefault();
                result.Amount = doctorSallary?.Salary ?? reception.Service.DoctorWage;


                var anesthesiologist_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");
                var pediatrician_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");
                var resident_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Resident", "DoctorRole");


                var anesthesiologistSallary = _unitOfWork.HumanResourceSalaries.Find(f => (anesthesiologist_guid == null ? f.CadreTypeId == anesthesiologist_role : f.HumanResourceId == anesthesiologist_guid)
                                                                && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId).FirstOrDefault();
                result.AnesthesiologistAmount = anesthesiologistSallary?.Salary ?? 0;


                var pediatricianSallary = _unitOfWork.HumanResourceSalaries.Find(f => (pediatrician_guid == null ? f.CadreTypeId == pediatrician_role : f.HumanResourceId == pediatrician_guid)
                                                                && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId).FirstOrDefault();
                result.PediatricianAmount = pediatricianSallary?.Salary ?? 0;


                var residentSallary = _unitOfWork.HumanResourceSalaries.Find(f => (resident_guid == null ? f.CadreTypeId == resident_role : f.HumanResourceId == resident_guid)
                                                                && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId).FirstOrDefault();
                result.ResidentAmount = residentSallary?.Salary ?? 0;

                return result;
            }
            catch { return null; }
        }

        public string AddDoctorWage(DoctorWageViewModel viewModel)
        {
            if (viewModel.ReceptionId == Guid.Empty)
                return "DataNotValid";
            Guid? updateReceptioStatus = null;
            int? unpaidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");
            int? paidId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
            var serviceId = _unitOfWork.Services.GetServiceByName(null, "DoctorWage")?.Guid;
            var salaryTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Wage", "SalaryType");
            var receptionService = _unitOfWork.ReceptionServices.GetReceptionOperationAndDoctor(viewModel.ReceptionId);
            var surgeryId = receptionService.Reception.Surgeries.FirstOrDefault().Guid;

            //var surgery1_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Surgery1", "DoctorRole");
            var anesthesiologist_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Anesthesiologist", "DoctorRole");
            var pediatrician_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Pediatrician", "DoctorRole");
            var resident_role = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Resident", "DoctorRole");

            if (viewModel.Amount.GetValueOrDefault(0) > 0)
            {
                HumanResourceSalary humanResourceSallary_Surgery;
                var doctor_guid = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Surgery1")?.DoctorId;

                if (doctor_guid != null && doctor_guid.Value != Guid.Empty)
                {
                    humanResourceSallary_Surgery = _unitOfWork.HumanResourceSalaries.Find(f => f.HumanResourceId == doctor_guid
                                                                    && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId).FirstOrDefault();

                    if (humanResourceSallary_Surgery == null)
                    {
                        humanResourceSallary_Surgery = new HumanResourceSalary()
                        {
                            BeginDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUserId = viewModel.UserId,
                            HumanResourceId = doctor_guid,
                            PaymentStatusId = unpaidId,
                            Salary = viewModel.Amount,
                            WorkTime = 1,
                            SalaryTypeId = salaryTypeId,
                            SurgeryId = surgeryId,
                            EndDate = DateTime.Now,
                            ExtraSalary = 0

                        };


                        HumanResource humanResource_Surgery = _unitOfWork.HumanResources.Get(doctor_guid.Value);
                        if (humanResource_Surgery == null)
                        {
                            humanResource_Surgery = new HumanResource()
                            {
                                Guid = doctor_guid.Value,
                                RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                HumanResourceSalaries = new List<HumanResourceSalary>()
                            };

                            humanResource_Surgery.HumanResourceSalaries.Add(humanResourceSallary_Surgery);

                            _unitOfWork.HumanResources.Add(humanResource_Surgery);
                        }
                        else
                        {
                            _unitOfWork.HumanResourceSalaries.Add(humanResourceSallary_Surgery);
                        }
                    }
                    else
                    {

                        decimal? payed = humanResourceSallary_Surgery.Salary - _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanResourceSallary_Surgery.Guid);
                        decimal? rem = viewModel.Amount.GetValueOrDefault(0) - payed;
                        if (rem < 0)
                            return "ERROR_Money";

                        humanResourceSallary_Surgery.WorkTime = 1;
                        humanResourceSallary_Surgery.ExtraSalary = 0;
                        humanResourceSallary_Surgery.Salary = viewModel.Amount;
                        humanResourceSallary_Surgery.ModifiedDate = DateTime.Now;
                        humanResourceSallary_Surgery.ModifiedUserId = viewModel.UserId;

                        if (rem == 0)
                            humanResourceSallary_Surgery.PaymentStatusId = paidId;
                        else
                            humanResourceSallary_Surgery.PaymentStatusId = unpaidId;


                        _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSallary_Surgery);
                    }


                    ReceptionService _receptionService;

                    _receptionService = _unitOfWork.ReceptionServices.GetFirstOrDefault(p => p.ReceptionId == viewModel.ReceptionId && p.ServiceId == serviceId);

                    if (_receptionService == null)
                    {
                        _receptionService = new()
                        {
                            ReceptionId = viewModel.ReceptionId,
                            ServiceId = serviceId,
                            Number = 1,
                            Price = viewModel.Amount,
                            StatusId = unpaidId,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = viewModel.UserId
                        };

                        _unitOfWork.ReceptionServices.Add(_receptionService);
                    }
                    else
                    {
                        decimal? payed = _receptionService.Price - _unitOfWork.ReceptionServices.GetReceptionServiceRem(_receptionService.Guid);
                        decimal? rem = viewModel.Amount.GetValueOrDefault(0) - payed;

                        _receptionService.Price = viewModel.Amount;

                        if (rem > 0)
                            _receptionService.StatusId = unpaidId;
                        else
                            _receptionService.StatusId = paidId;


                        _unitOfWork.ReceptionServices.UpdateState(_receptionService);
                    }


                    if (serviceId == null || serviceId == Guid.Empty)
                    {
                        _receptionService.Service = new Service
                        {
                            Name = "DoctorWage",
                            Price = 1,
                            TypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Other", "ServiceType")

                        };
                    }

                    updateReceptioStatus = viewModel.ReceptionId;

                }
            }

            if (viewModel.AnesthesiologistAmount.GetValueOrDefault(0) > 0)
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(surgeryId, anesthesiologist_role, salaryTypeId);
                var anesthesiologist_guid = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Anesthesiologist")?.DoctorId;

                HumanResourceSalary humanResourceSallary_Anesthesiologist;

                if (old_wage == null)
                {
                    humanResourceSallary_Anesthesiologist = new HumanResourceSalary()
                    {
                        BeginDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUserId = viewModel.UserId,
                        HumanResourceId = anesthesiologist_guid,
                        PaymentStatusId = unpaidId,
                        CadreTypeId = anesthesiologist_role,
                        Salary = viewModel.AnesthesiologistAmount,
                        WorkTime = 1,
                        SalaryTypeId = salaryTypeId,
                        SurgeryId = surgeryId,
                        EndDate = DateTime.Now,
                        ExtraSalary = 0
                    };

                    if (anesthesiologist_guid != null)
                    {
                        HumanResource humanResource_Anesthesiologist = _unitOfWork.HumanResources.Get(anesthesiologist_guid.Value);
                        if (humanResource_Anesthesiologist == null)
                        {
                            humanResource_Anesthesiologist = new HumanResource()
                            {
                                Guid = anesthesiologist_guid.Value,
                                RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                            };

                            _unitOfWork.HumanResources.Add(humanResource_Anesthesiologist);
                        }
                    }

                    _unitOfWork.HumanResourceSalaries.Add(humanResourceSallary_Anesthesiologist);
                }
                else
                {
                    if (anesthesiologist_guid == null)
                    {
                        if (old_wage.HumanResourceSalaryPayments != null && old_wage.HumanResourceSalaryPayments.Any())
                            return "ERROR_Money";

                        old_wage.WorkTime = 1;
                        old_wage.ExtraSalary = 0;
                        old_wage.Salary = viewModel.AnesthesiologistAmount;
                        old_wage.ModifiedDate = DateTime.Now;
                        old_wage.ModifiedUserId = viewModel.UserId;

                        _unitOfWork.HumanResourceSalaries.UpdateState(old_wage);
                    }
                    else
                    {
                        humanResourceSallary_Anesthesiologist = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == anesthesiologist_guid
                                                                        && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId);

                        decimal? payed = humanResourceSallary_Anesthesiologist.Salary - _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanResourceSallary_Anesthesiologist.Guid);
                        decimal? rem = viewModel.AnesthesiologistAmount.GetValueOrDefault(0) - payed;
                        if (rem < 0)
                            return "ERROR_Money";

                        humanResourceSallary_Anesthesiologist.WorkTime = 1;
                        humanResourceSallary_Anesthesiologist.ExtraSalary = 0;
                        humanResourceSallary_Anesthesiologist.Salary = viewModel.AnesthesiologistAmount;
                        humanResourceSallary_Anesthesiologist.ModifiedDate = DateTime.Now;
                        humanResourceSallary_Anesthesiologist.ModifiedUserId = viewModel.UserId;

                        if (rem == 0)
                            humanResourceSallary_Anesthesiologist.PaymentStatusId = paidId;
                        else
                            humanResourceSallary_Anesthesiologist.PaymentStatusId = unpaidId;


                        _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSallary_Anesthesiologist);
                    }
                }

            }
            else
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(surgeryId, anesthesiologist_role, salaryTypeId);

                if (old_wage != null)
                {
                    if (old_wage.HumanResourceSalaryPayments != null && old_wage.HumanResourceSalaryPayments.Any())
                        return "ERROR_HumanResourceSallaryDependency";

                    _unitOfWork.HumanResourceSalaries.Remove(old_wage);
                }
            }

            if (viewModel.PediatricianAmount.GetValueOrDefault(0) > 0)
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(surgeryId, pediatrician_role, salaryTypeId);
                var pediatrician_guid = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Pediatrician")?.DoctorId;

                HumanResourceSalary humanResourceSallary_Pediatrician;

                if (old_wage == null)
                {
                    humanResourceSallary_Pediatrician = new HumanResourceSalary()
                    {
                        BeginDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        CreateUserId = viewModel.UserId,
                        HumanResourceId = pediatrician_guid,
                        PaymentStatusId = unpaidId,
                        CadreTypeId = pediatrician_role,
                        Salary = viewModel.PediatricianAmount,
                        WorkTime = 1,
                        SalaryTypeId = salaryTypeId,
                        SurgeryId = surgeryId,
                        EndDate = DateTime.Now,
                        ExtraSalary = 0
                    };

                    if (pediatrician_guid != null)
                    {
                        HumanResource humanResource_Pediatrician = _unitOfWork.HumanResources.Get(pediatrician_guid.Value);
                        if (humanResource_Pediatrician == null)
                        {
                            humanResource_Pediatrician = new HumanResource()
                            {
                                Guid = pediatrician_guid.Value,
                                RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType")
                            };

                            _unitOfWork.HumanResources.Add(humanResource_Pediatrician);
                        }
                    }

                    _unitOfWork.HumanResourceSalaries.Add(humanResourceSallary_Pediatrician);
                }
                else
                {
                    if (pediatrician_guid == null)
                    {
                        if (old_wage.HumanResourceSalaryPayments != null && old_wage.HumanResourceSalaryPayments.Any())
                            return "ERROR_Money";

                        old_wage.WorkTime = 1;
                        old_wage.ExtraSalary = 0;
                        old_wage.Salary = viewModel.PediatricianAmount;
                        old_wage.ModifiedDate = DateTime.Now;
                        old_wage.ModifiedUserId = viewModel.UserId;

                        _unitOfWork.HumanResourceSalaries.UpdateState(old_wage);
                    }
                    else
                    {
                        humanResourceSallary_Pediatrician = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == pediatrician_guid
                                                                        && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId);

                        decimal? payed = humanResourceSallary_Pediatrician.Salary - _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanResourceSallary_Pediatrician.Guid);
                        decimal? rem = viewModel.PediatricianAmount.GetValueOrDefault(0) - payed;
                        if (rem < 0)
                            return "ERROR_Money";

                        humanResourceSallary_Pediatrician.WorkTime = 1;
                        humanResourceSallary_Pediatrician.ExtraSalary = 0;
                        humanResourceSallary_Pediatrician.Salary = viewModel.PediatricianAmount;
                        humanResourceSallary_Pediatrician.ModifiedDate = DateTime.Now;
                        humanResourceSallary_Pediatrician.ModifiedUserId = viewModel.UserId;

                        if (rem == 0)
                            humanResourceSallary_Pediatrician.PaymentStatusId = paidId;
                        else
                            humanResourceSallary_Pediatrician.PaymentStatusId = unpaidId;


                        _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSallary_Pediatrician);
                    }
                }

            }
            else
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(surgeryId, pediatrician_role, salaryTypeId);

                if (old_wage != null)
                {
                    if (old_wage.HumanResourceSalaryPayments != null && old_wage.HumanResourceSalaryPayments.Any())
                        return "ERROR_HumanResourceSallaryDependency";

                    _unitOfWork.HumanResourceSalaries.Remove(old_wage);
                }
            }




            /*if (viewModel.ResidentAmount.GetValueOrDefault(0) > 0)
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(p => p.SurgeryId == surgeryId && p.CadreTypeId == resident_role && p.SalaryTypeId == salaryTypeId);
                var old_doctor = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident");

                if (old_wage == null)
                {
                    if (viewModel.ResidentGuid == Guid.Empty)
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    if (viewModel.ResidentGuid == Guid.Empty)
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }
            else
            {
                var old_wage = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(p => p.SurgeryId == surgeryId && p.CadreTypeId == resident_role && p.SalaryTypeId == salaryTypeId);
                var old_doctor = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident");

                if (old_wage == null)
                {
                    if (viewModel.ResidentGuid == Guid.Empty)
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                }
                else
                {
                    if (viewModel.ResidentGuid == Guid.Empty)
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (old_doctor == null)
                        {

                        }
                        else
                        {

                        }
                    }
                }
            }
*/


            if (viewModel.ResidentGuid == Guid.Empty)
            {
                HumanResourceSalary humanResourceSallary_Resident;
                var resident_guid = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident")?.DoctorId;
                if (resident_guid != null && resident_guid != Guid.Empty)
                {
                    humanResourceSallary_Resident = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == resident_guid
                                                                    && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId);

                    if (humanResourceSallary_Resident != null)
                        return "ERROR_HumanResourceSallaryDependency";


                    var surgeryDoctorId = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident")?.Guid;
                    if (surgeryDoctorId != null)
                    {
                        var surgeryDoctor = _unitOfWork.SurgeryDoctors.Get(surgeryDoctorId.Value);
                        _unitOfWork.SurgeryDoctors.Remove(surgeryDoctor);
                    }
                }
            }
            else
            {

                if (viewModel.ResidentAmount.GetValueOrDefault(0) > 0)
                {
                    HumanResourceSalary humanResourceSallary_Resident;
                    var resident_guid = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident")?.DoctorId;

                    if (resident_guid != null && resident_guid != Guid.Empty)
                    {

                        if (viewModel.ResidentGuid == resident_guid.Value)
                        {
                            humanResourceSallary_Resident = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == resident_guid
                                                                        && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId);

                            decimal? payed = humanResourceSallary_Resident.Salary - _unitOfWork.HumanResourceSalaries.GetHumanSalaryRem(humanResourceSallary_Resident.Guid);
                            decimal? rem = viewModel.ResidentAmount.GetValueOrDefault(0) - payed;
                            if (rem < 0)
                                return "ERROR_Money";

                            humanResourceSallary_Resident.WorkTime = 1;
                            humanResourceSallary_Resident.ExtraSalary = 0;
                            humanResourceSallary_Resident.Salary = viewModel.ResidentAmount;
                            humanResourceSallary_Resident.ModifiedDate = DateTime.Now;
                            humanResourceSallary_Resident.ModifiedUserId = viewModel.UserId;

                            if (rem == 0)
                                humanResourceSallary_Resident.PaymentStatusId = paidId;
                            else
                                humanResourceSallary_Resident.PaymentStatusId = unpaidId;


                            _unitOfWork.HumanResourceSalaries.UpdateState(humanResourceSallary_Resident);
                        }
                        else
                        {
                            humanResourceSallary_Resident = _unitOfWork.HumanResourceSalaries.GetFirstOrDefault(f => f.HumanResourceId == resident_guid
                                                                    && f.SurgeryId == surgeryId && f.SalaryTypeId == salaryTypeId);

                            if (humanResourceSallary_Resident != null)
                                return "ERROR_HumanResourceSallaryDependency";

                            var surgeryDoctorId = receptionService.Reception?.Surgeries?.FirstOrDefault()?.SurgeryDoctors?.FirstOrDefault(d => d.DoctorRole.Name == "Resident")?.Guid;
                            if (surgeryDoctorId != null)
                            {
                                var surgeryDoctor = _unitOfWork.SurgeryDoctors.Get(surgeryDoctorId.Value);
                                _unitOfWork.SurgeryDoctors.Remove(surgeryDoctor);
                            }

                            humanResourceSallary_Resident = new HumanResourceSalary()
                            {
                                BeginDate = DateTime.Now,
                                CreateDate = DateTime.Now,
                                CreateUserId = viewModel.UserId,
                                HumanResourceId = viewModel.ResidentGuid,
                                PaymentStatusId = unpaidId,
                                Salary = viewModel.ResidentAmount,
                                WorkTime = 1,
                                SalaryTypeId = salaryTypeId,
                                SurgeryId = surgeryId,
                                EndDate = DateTime.Now,
                                ExtraSalary = 0

                            };


                            HumanResource humanResource_Resident = _unitOfWork.HumanResources.Get(viewModel.ResidentGuid);
                            if (humanResource_Resident == null)
                            {
                                humanResource_Resident = new HumanResource()
                                {
                                    Guid = viewModel.ResidentGuid,
                                    RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                    HumanResourceSalaries = new List<HumanResourceSalary>()
                                };

                                humanResource_Resident.HumanResourceSalaries.Add(humanResourceSallary_Resident);

                                _unitOfWork.HumanResources.Add(humanResource_Resident);
                            }
                            else
                            {
                                _unitOfWork.HumanResourceSalaries.Add(humanResourceSallary_Resident);
                            }

                            var addSurgeryDoctor = new SurgeryDoctor
                            {
                                DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Resident", "DoctorRole"),
                                SurgeryId = surgeryId,
                                DoctorId = humanResource_Resident.Guid
                            };

                            _unitOfWork.SurgeryDoctors.Add(addSurgeryDoctor);
                        }

                    }
                    else
                    {
                        humanResourceSallary_Resident = new HumanResourceSalary()
                        {
                            BeginDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            CreateUserId = viewModel.UserId,
                            HumanResourceId = viewModel.ResidentGuid,
                            PaymentStatusId = unpaidId,
                            Salary = viewModel.ResidentAmount,
                            WorkTime = 1,
                            SalaryTypeId = salaryTypeId,
                            SurgeryId = surgeryId,
                            EndDate = DateTime.Now,
                            ExtraSalary = 0

                        };


                        HumanResource humanResource_Resident = _unitOfWork.HumanResources.Get(viewModel.ResidentGuid);
                        if (humanResource_Resident == null)
                        {
                            humanResource_Resident = new HumanResource()
                            {
                                Guid = viewModel.ResidentGuid,
                                RoleTypeId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Doctor", "UserType"),
                                HumanResourceSalaries = new List<HumanResourceSalary>()
                            };

                            humanResource_Resident.HumanResourceSalaries.Add(humanResourceSallary_Resident);

                            _unitOfWork.HumanResources.Add(humanResource_Resident);
                        }
                        else
                        {
                            _unitOfWork.HumanResourceSalaries.Add(humanResourceSallary_Resident);
                        }

                        var addSurgeryDoctor = new SurgeryDoctor
                        {
                            DoctorRoleId = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Resident", "DoctorRole"),
                            SurgeryId = surgeryId,
                            DoctorId = humanResource_Resident.Guid
                        };

                        _unitOfWork.SurgeryDoctors.Add(addSurgeryDoctor);
                    }

                }
            }
            var hospitalWage = _unitOfWork.ReceptionServices.Get(viewModel.ReceptionServiceId);
            var hospitalWageRecived = _unitOfWork.ReceptionServiceReceiveds.Find(a=>a.ReceptionServiceId == viewModel.ReceptionServiceId);
            if(hospitalWageRecived != null)
            {
                hospitalWage.Price = viewModel.HospitalWage;
                if (hospitalWage.Price > hospitalWageRecived.Sum(a=>a.Amount))
                {
                    hospitalWage.StatusId = unpaidId;
                }
                else
                {
                    hospitalWage.StatusId = paidId;
                }
            }
            
            _unitOfWork.Complete();

            if (updateReceptioStatus != null)
                UpdateReceptionStatus(updateReceptioStatus.Value);

            return "";
        }

        public ReceptionServiceViewModel GetReceptionOperation(Guid receptionId)
        {
            try
            {
                ReceptionService reception = _unitOfWork.ReceptionServices.GetReceptionOperation(receptionId);
                return ConvertModel(reception);
            }
            catch (Exception e) { return null; }
        }


        public OperationStatus RemoveReceptionService(Guid receptionServiceid)
        {
            try
            {
                var rs = _unitOfWork.ReceptionServices.Get(receptionServiceid);

                _unitOfWork.ReceptionServiceReceiveds.RemoveRange(_unitOfWork.ReceptionServiceReceiveds.Find(x => x.ReceptionServiceId == receptionServiceid));
                _unitOfWork.ReceptionServices.Remove(rs);
                _unitOfWork.Complete();


                UpdateReceptionStatus(rs.ReceptionId.Value);

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

        public void UpdateReceptionStatus(Guid ReceptionId)
        {
            var paid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Paid", "PaymentStatus");
            var unPaid = _unitOfWork.BaseInfoGenerals.GetIdByNameAndType("Unpaid", "PaymentStatus");

            var _reception = _unitOfWork.Receptions.GetReceptionWithServices(ReceptionId);
            var pay = _reception.ReceptionServices.All(p => p.StatusId == paid);
            _reception.PaymentStatusId = pay ? paid : unPaid;

            _unitOfWork.Receptions.UpdateState(_reception);
            _unitOfWork.Complete();
        }

        public PieChartViewModel GetMostOperations(Guid userId)
        {
            try
            {
                IEnumerable<PieChartModel> allMed = _unitOfWork.ReceptionServices.GetMostOperations(userId).OrderByDescending(a => a.Value).Take(10);

                PieChartViewModel pie = new PieChartViewModel
                {
                    Labels = allMed.Select(a => a.Label).ToArray(),
                    Value = allMed.Select(a => Convert.ToInt32(a.Value ?? 0)).ToArray()
                };

                return pie;

            }
            catch (Exception e) { throw e; }
        }

        public List<ReceptionServiceViewModel> ConvertModelsLists(IEnumerable<ReceptionService> services)
        {
            List<ReceptionServiceViewModel> serviceDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>();
                cfg.CreateMap<Service, ServiceViewModel>();
                cfg.CreateMap<ReceptionService, ReceptionServiceViewModel>()
                .ForMember(p => p.ServiceName, r => r.MapFrom(s => s.Service.Name ?? s.Product.Name))
                .ForMember(p => p.ServiceStatus, r => r.MapFrom(s => s.Status.Name))
                .ForMember(p => p.Recived, r => r.MapFrom(s => s.ReceptionServiceReceiveds.Where(p => !p.AmountStatus.Value).Sum(p => p.Amount.Value)))
                .ForMember(p => p.Returned, r => r.MapFrom(s => s.ReceptionServiceReceiveds.Where(p => p.AmountStatus.Value).Sum(p => p.Amount.Value)))
                .ForMember(p => p.PayByInsurance, r => r.MapFrom(s => s.ReceptionServiceReceiveds.Where(a => a.ReceptionInsuranceId != null).Sum(a => a.Amount)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            serviceDtoList = mapper.Map<IEnumerable<ReceptionService>, List<ReceptionServiceViewModel>>(services);
            return serviceDtoList;
        }

        public List<ReceptionServiceViewModel> ConvertModelsListsForReceptionProduct(IEnumerable<ReceptionService> services)
        {
            List<ReceptionServiceViewModel> serviceDtoList = new();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionServiceReceived, ReceptionServiceReceivedViewModel>();
                cfg.CreateMap<Service, ServiceViewModel>();
                cfg.CreateMap<ReceptionService, ReceptionServiceViewModel>()
                .ForMember(p => p.ServiceName, r => r.MapFrom(s => (s.Product == null) ? "" : s.Product.Name))
                .ForMember(p => p.ServiceStatus, r => r.MapFrom(s => s.Status.Name))
                .ForMember(p => p.Recived, r => r.MapFrom(s => s.ReceptionServiceReceiveds.Where(p => !p.AmountStatus.Value).Sum(p => p.Amount.Value)))
                .ForMember(p => p.Returned, r => r.MapFrom(s => s.ReceptionServiceReceiveds.Where(p => p.AmountStatus.Value).Sum(p => p.Amount.Value)))
                ;
            });

            IMapper mapper = config.CreateMapper();
            serviceDtoList = mapper.Map<IEnumerable<ReceptionService>, List<ReceptionServiceViewModel>>(services);
            return serviceDtoList;
        }

        public ReceptionServiceViewModel ConvertModel(ReceptionService service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionService, ReceptionServiceViewModel>()
                .ForMember(p => p.ReceptionInvoiceNum, r => r.MapFrom(s => s.Reception.ReceptionInvoiceNum))
                .ForMember(p => p.Service, r => r.Ignore())
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionService, ReceptionServiceViewModel>(service);
        }

        public DoctorWageViewModel ConvertCustomModel(ReceptionService service)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ReceptionService, DoctorWageViewModel>()
                .ForMember(p => p.ReceptionServiceId, r => r.MapFrom(s => s.Guid))
                .ForMember(p => p.ReceptionId, r => r.MapFrom(s => s.ReceptionId))
                .ForMember(p => p.HospitalWage, r => r.MapFrom(s => s.Price))
                .ForMember(p => p.DoctorGuid, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Surgery1").FirstOrDefault().Doctor.Guid))
                .ForMember(p => p.DoctorName, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Surgery1").FirstOrDefault().Doctor.User.Name))
                .ForMember(p => p.AnesthesiologistGuid, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Anesthesiologist").FirstOrDefault().Doctor.Guid))
                .ForMember(p => p.Anesthesiologist, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Anesthesiologist").FirstOrDefault().Doctor.User.Name))
                .ForMember(p => p.PediatricianGuid, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Pediatrician").FirstOrDefault().Doctor.Guid))
                .ForMember(p => p.Pediatrician, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Pediatrician").FirstOrDefault().Doctor.User.Name))
                .ForMember(p => p.ResidentGuid, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Resident").FirstOrDefault().Doctor.Guid))
                .ForMember(p => p.Resident, r => r.MapFrom(s => s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Resident").FirstOrDefault().Doctor.User.Name))
                //.ForMember(p => p.Amount, r => r.MapFrom(s => (s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Surgery1").FirstOrDefault().Surgery.HumanResourceSalaries.Where(s => s.SalaryTypeId == wageId).FirstOrDefault()?.Salary) ?? s.Service.DoctorWage))
                //.ForMember(p => p.AnesthesiologistAmount, r => r.MapFrom(s => (s.Reception.Surgeries.FirstOrDefault().SurgeryDoctors.Where(d => d.DoctorRole.Name == "Anesthesiologist").FirstOrDefault().Surgery.HumanResourceSalaries.Where(s => s.SalaryTypeId == wageId).FirstOrDefault().Salary) ?? 0))
                ;
            });

            IMapper mapper = config.CreateMapper();
            return mapper.Map<ReceptionService, DoctorWageViewModel>(service);
        }

        
    }
}
