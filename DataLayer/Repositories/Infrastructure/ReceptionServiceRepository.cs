using DataLayer.EntityModels;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Infrastructure
{
    public class ReceptionServiceRepository : Repository<ReceptionService>, IReceptionServiceRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public ReceptionServiceRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<ReceptionService> GetReceptionServicesByReceptionId(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Service)
                .Include(p => p.Product)
                .Include(p => p.Status)
                .Include(p => p.ReceptionServiceReceiveds).ThenInclude(a => a.ReceptionInsurance)
                .Where(p => p.ReceptionId == receptionId)
                .Select(p => new ReceptionService
                {
                    Guid = p.Guid,
                    Id = p.Id,
                    Number = p.Number,
                    Price = p.Price,
                    ProductIdDMS = p.ProductIdDMS,
                    Discount = p.Discount,
                    Service = new Service
                    {
                        Name = p.Service.Name
                    },
                    Status = new BaseInfoGeneral
                    {
                        Name = p.Status.Name
                    },
                    ReceptionServiceReceiveds = (ICollection<ReceptionServiceReceived>)p.ReceptionServiceReceiveds.Select(p => new ReceptionServiceReceived
                    {
                        AmountStatus = p.AmountStatus,
                        Amount = p.Amount,
                        ReceptionInsuranceId = p.ReceptionInsuranceId
                    }),
                    Product = new Product
                    {
                        Name = p.Product.Name
                    }
                });
        }

        public IEnumerable<ReceptionService> GetReceptionSpecificServicesByReceptionId(Guid receptionId, string serviceType)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Service).ThenInclude(x => x.Type)
                .Where(p => p.ReceptionId == receptionId && p.Service.Type.Name.ToLower() == serviceType)
                .Select(a => new ReceptionService
                {
                    Guid = a.Guid,
                    Number = a.Number,
                    ReceptionId = a.ReceptionId,
                    ServiceDate = a.ServiceDate,
                    Service = new Service
                    {
                        Name = a.Service.Name
                    }
                });

        }

        public IEnumerable<ReceptionService> GetAllReceptionProducts(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Product)
                .Where(p => p.ReceptionId == receptionId && p.ProductId != null)
                .Select(a => new ReceptionService
                {
                    Guid = a.Guid,
                    Number = a.Number,
                    ReceptionId = a.ReceptionId,
                    ServiceDate = a.ServiceDate,
                    Product = new Product
                    {
                        Name = a.Product.Name
                    }
                });
        }

        public IEnumerable<ReceptionService> GetAllReceptionDMSProducts(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Where(p => p.ReceptionId == receptionId && p.ProductIdDMS != null)
                .Select(a => new ReceptionService
                {
                    Guid = a.Guid,
                    Number = a.Number,
                    ReceptionId = a.ReceptionId,
                    ProductIdDMS = a.ProductIdDMS,
                    ServiceDate = a.ServiceDate,
                });
        }

        public ReceptionService GetReceptionServiceWithRecives(Guid id)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.ReceptionServiceReceiveds)
                .SingleOrDefault(p => p.Guid == id);
        }

        public IEnumerable<ReceptionService> GetAllReceptionServices(List<Guid> clinicSectionIds, DateTime dateFrom, DateTime dateTo)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Reception).ThenInclude(a => a.ClinicSection)
                .Include(p => p.Reception).ThenInclude(a => a.ReceptionDoctors).ThenInclude(a => a.Doctor).ThenInclude(a => a.User)
                .Include(p => p.Reception).ThenInclude(a => a.ReceptionDoctors).ThenInclude(a => a.DoctorRole)
                .Include(p => p.ReceptionServiceReceiveds)
                .Where(x => clinicSectionIds.Contains(x.Reception.ClinicSectionId ?? Guid.Empty))
                .Select(x => new ReceptionService
                {
                    Guid = x.Guid,
                    Number = x.Number,
                    ReceptionServiceReceiveds = (ICollection<ReceptionServiceReceived>)x.ReceptionServiceReceiveds.Where(d => d.CreatedDate >= dateFrom && d.CreatedDate <= dateTo),
                    ReceptionId = x.ReceptionId,
                    ServiceDate = x.ServiceDate,
                    Reception = new Reception
                    {
                        ReceptionDate = x.Reception.ReceptionDate,
                        ReceptionNum = x.Reception.ReceptionNum,
                        Guid = x.Reception.Guid,
                        ClinicSectionId = x.Reception.ClinicSectionId,
                        ClinicSection = new ClinicSection
                        {
                            Guid = x.Reception.ClinicSection.Guid,
                            Name = x.Reception.ClinicSection.Name
                        },
                        ReceptionDoctors = x.Reception.ReceptionDoctors == null ? null : (ICollection<ReceptionDoctor>)x.Reception.ReceptionDoctors.Where(a => a.DoctorRole.Name == "DispatcherDoctor").Select(b => new ReceptionDoctor
                        {
                            DoctorRole = new BaseInfoGeneral
                            {
                                Name = b.DoctorRole.Name
                            },
                            Doctor = new Doctor
                            {
                                User = new User
                                {
                                    Name = b.Doctor.User.Name
                                }
                            }
                        })
                    }
                });

        }

        public ReceptionService GetReceptionExceptOperationService(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Service).ThenInclude(a => a.Type).OrderBy(a => a.Id)
                .FirstOrDefault(p => p.ReceptionId == receptionId && p.Service.Type.Name != "Operation");
        }

        public IEnumerable<ReceptionService> GetUnpaidReceptionServicesByReceptionId(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.ReceptionServiceReceiveds)
                .Where(p => p.ReceptionId == receptionId)
                .Select(p => new ReceptionService
                {
                    Guid = p.Guid,
                    DiscountCurrencyId = p.DiscountCurrencyId,
                    Price = (p.Price.GetValueOrDefault(0) * p.Number.GetValueOrDefault(0) - p.Discount.GetValueOrDefault(0)) - (p.ReceptionServiceReceiveds.Sum(s => (s.AmountStatus == null && s.AmountStatus.Value) ? -s.Amount.GetValueOrDefault(0) : s.Amount.GetValueOrDefault(0)))

                }).Where(r => r.Price.GetValueOrDefault(0) > 0);
        }

        public ReceptionService GetReceptionOperation(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Service).ThenInclude(a => a.Type).OrderBy(a => a.Id)
                .FirstOrDefault(p => p.ReceptionId == receptionId && p.Service.Type.Name == "Operation");
        }


        public ReceptionService GetReceptionOperationAndDoctor(Guid receptionId)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Reception.Surgeries).ThenInclude(p => p.SurgeryDoctors).ThenInclude(p => p.DoctorRole.SurgeryDoctors).ThenInclude(x => x.Doctor.User)

                .Include(p => p.Service).ThenInclude(a => a.Type).OrderBy(a => a.Id)
                .Where(p => p.ReceptionId == receptionId && p.Service.Type.Name == "Operation")
                .Select(p => new ReceptionService
                {
                    Guid = p.Guid,
                    Price = p.Price,
                    ReceptionId = p.ReceptionId,
                    Reception = new Reception
                    {
                        Surgeries = (ICollection<Surgery>)p.Reception.Surgeries.Select(s => new Surgery
                        {
                            Guid = s.Guid,
                            SurgeryDoctors = (ICollection<SurgeryDoctor>)s.SurgeryDoctors.Select(w => new SurgeryDoctor
                            {
                                Guid = w.Guid,
                                DoctorId = w.DoctorId,
                                DoctorRole = new BaseInfoGeneral
                                {
                                    Name = w.DoctorRole.Name
                                },
                                Doctor = new Doctor
                                {
                                    Guid = w.Doctor.Guid,
                                    User = new User
                                    {
                                        Name = w.Doctor.User.Name
                                    }
                                }
                            })
                        })
                    },
                    Service = new Service
                    {
                        DoctorWage = p.Service.DoctorWage,
                    }
                }).FirstOrDefault();
        }


        public ReceptionService GetReceptionServiceWithReception(Guid id)
        {
            return Context.ReceptionServices.AsNoTracking()
                .Include(p => p.Reception)
                .SingleOrDefault(p => p.Guid == id);
        }

        public ReceptionService GetFirstOrDefault(Expression<Func<ReceptionService, bool>> predicate = null)
        {
            IQueryable<ReceptionService> result = _context.ReceptionServices.AsNoTracking();

            if (predicate != null)
                result = result.Where(predicate);

            return result.FirstOrDefault();
        }

        public decimal? GetReceptionServiceRem(Guid receptionServiceId)
        {
            var result = _context.ReceptionServices.AsNoTracking()
                 .Include(p => p.ReceptionServiceReceiveds)
                 .Where(p => p.Guid == receptionServiceId)
                 .Select(p => ((p.Number * p.Price) - (p.Discount.GetValueOrDefault(0))) -
                 (p.ReceptionServiceReceiveds.Where(s => !s.AmountStatus.Value).Sum(x => x.Amount.Value) -
                  p.ReceptionServiceReceiveds.Where(s => s.AmountStatus.Value).Sum(x => x.Amount.Value)))
                 .SingleOrDefault();

            return result;
        }

        public ReceptionService GetReceptionServiceWithPatient(Guid receptionServiceId)
        {
            return _context.ReceptionServices.AsNoTracking()
                .Include(p => p.Reception).ThenInclude(p => p.Patient).ThenInclude(p => p.User)
                .Include(p => p.Status)
                .Where(p => p.Guid == receptionServiceId)
                .Select(p => new ReceptionService
                {
                    Price = p.Price,
                    Number = p.Number,
                    Status = new BaseInfoGeneral
                    {
                        Name = p.Status.Name
                    },
                    Reception = new Reception
                    {
                        ReceptionNum = p.Reception.ReceptionNum,
                        Patient = new Patient
                        {
                            User = new User
                            {
                                Name = p.Reception.Patient.User.Name
                            }
                        }
                    }
                }).SingleOrDefault();
        }

        public bool HasDoctorVisit(Guid receptionId)
        {
            return _context.ReceptionServices.AsNoTracking()
                .Include(p => p.Service)
                .Where(p => p.ReceptionId == receptionId && p.Service.Name == "DoctorVisit")
                .Any();
        }

        public decimal? GetReceptionRemByReceptionId(Guid receptionId)
        {
            var result = _context.ReceptionServices.AsNoTracking()
                 .Include(p => p.ReceptionServiceReceiveds)
                 .Where(p => p.ReceptionId == receptionId)
                 .Select(p => (p.ReceptionServiceReceiveds.Where(s => !s.AmountStatus.Value).Sum(x => x.Amount.Value) - p.ReceptionServiceReceiveds.Where(s => s.AmountStatus.Value).Sum(x => x.Amount.Value))
                 - ((p.Number * p.Price) - (p.Discount.GetValueOrDefault(0)))
                 ).ToList();

            return result.Sum();
        }

        public IEnumerable<ReceptionService> GetVisitPriceByReserveDetailId(Guid reserveDetailId)
        {
            return _context.ReceptionServices.AsNoTracking()
                .Include(p => p.Reception)
                .Include(p => p.Service)
                .Include(p => p.ReceptionServiceReceiveds)
                .Where(p => p.Reception.ReserveDetailId == reserveDetailId && p.Service.Name == "DoctorVisit")
                .ToList();
        }

        public IEnumerable<PieChartModel> GetMostOperations(Guid userId)
        {
            return Context.ReceptionServices
                .Include(a => a.Service)
                .Include(a => a.Reception)
                .Where(a => !string.IsNullOrWhiteSpace(a.Service.Name) && a.Service.Name != "DoctorWage" && Context.ClinicSectionUsers.Where(a => a.UserId == userId).Select(a => a.ClinicSectionId).Contains(a.Reception.ClinicSectionId.Value))
                .GroupBy(a => a.Service.Name).Select(a => new PieChartModel
                {
                    Label = a.Key,
                    Value = a.Count()
                });
        }
    }
}
