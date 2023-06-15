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
    public class HumanResourceSalaryRepository : Repository<HumanResourceSalary>, IHumanResourceSalaryRepository
    {
        public WASContext _context
        {
            get { return Context as WASContext; }
        }
        public HumanResourceSalaryRepository(WASContext context)
            : base(context)
        {
        }
        public IEnumerable<HumanResourceSalary> GetAllHumanSalary()
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(x => x.HumanResource).ThenInclude(x => x.Gu)
                .Include(x => x.CreateUser)
                .Include(x => x.Currency).OrderByDescending(z => z.CreateDate);
        }

        public IEnumerable<HumanResourceSalary> GetAllHumanSalaryWithDate(List<Guid> sections, DateTime dateFrom, DateTime dateTo, Expression<Func<HumanResourceSalary, bool>> predicate = null)
        {
            IQueryable<HumanResourceSalary> result = _context.HumanResourceSalaries.AsNoTracking()
                                                    .Include(x => x.HumanResource.Gu)
                                                    .Include(x => x.PaymentStatus)
                                                    .Include(x => x.SalaryType)
                                                    .Include(x => x.Surgery.Reception)
                                                    .Include(x => x.HumanResourceSalaryPayments)
                                                    .Where(x => x.CreateDate > dateFrom && x.CreateDate < dateTo && sections.Contains(x.HumanResource.Gu.ClinicSectionId ?? Guid.Empty));
            if (predicate != null)
                result = result.Where(predicate);

            return result.OrderByDescending(z => z.CreateDate)
                .Select(p => new HumanResourceSalary
                {
                    Guid = p.Guid,
                    Id = p.Id,
                    CurrencyId = p.CurrencyId,
                    CreateDate = p.CreateDate,
                    WorkTime = p.WorkTime,
                    ExtraSalary = p.ExtraSalary,
                    Salary = p.Salary,
                    HumanResource = new HumanResource
                    {
                        MinWorkTime = p.HumanResource.MinWorkTime,
                        Gu = new User
                        {
                            Name = p.HumanResource.Gu.Name
                        }
                    },
                    PaymentStatus = new BaseInfoGeneral
                    {
                        Name = p.PaymentStatus.Name
                    },
                    SalaryType = new BaseInfoGeneral
                    {
                        Name = p.SalaryType.Name,
                        Priority = p.SalaryType.Priority
                    },
                    HumanResourceSalaryPayments = (ICollection<HumanResourceSalaryPayment>)p.HumanResourceSalaryPayments.Select(s => new HumanResourceSalaryPayment
                    {
                        Amount = s.Amount
                    }),
                    Surgery = new Surgery
                    {
                        Reception = new Reception
                        {
                            ReceptionNum = p.Surgery.Reception.ReceptionNum
                        }
                    }
                });
        }

        public IEnumerable<HumanResourceSalary> GetAllTreatmentStaffWage(List<Guid> sections, Guid surgeryId, int? cadreType)
        {
            var typeId = _context.BaseInfoGeneralTypes.AsNoTracking().FirstOrDefault(p => p.Ename == "SalaryType")?.Id;
            var wageId = _context.BaseInfoGenerals.AsNoTracking().FirstOrDefault(p => p.Name == "Wage" && p.TypeId == typeId)?.Id;

            IQueryable<HumanResourceSalary> result = _context.HumanResourceSalaries.AsNoTracking()
                                                    .Include(x => x.HumanResource.Gu)
                                                    .Include(x => x.PaymentStatus)
                                                    .Where(x => x.CadreTypeId == cadreType && x.SurgeryId == surgeryId && x.SalaryTypeId == wageId && sections.Contains(x.HumanResource.Gu.ClinicSectionId ?? Guid.Empty));

            return result.OrderByDescending(z => z.CreateDate)
                .Select(p => new HumanResourceSalary
                {
                    Guid = p.Guid,
                    Salary = p.Salary,
                    HumanResource = new HumanResource
                    {
                        Guid = p.HumanResource.Guid,
                        Gu = new User
                        {
                            Name = p.HumanResource.Gu.Name
                        }
                    },
                    PaymentStatus = new BaseInfoGeneral
                    {
                        Name = p.PaymentStatus.Name
                    },
                });
        }

        public IEnumerable<HumanResourceSalary> GetAllHumanSalaryName(List<Guid> sections)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(x => x.HumanResource.Gu)
                .Include(x => x.CreateUser)
                .Include(x => x.Currency).Where(x => sections.Contains(x.HumanResource.Gu.ClinicSectionId ?? Guid.Empty)).OrderByDescending(z => z.CreateDate);
        }

        public IEnumerable<HumanResourceSalary> GetHumanSalaryByHumanId(Guid gd)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(x => x.HumanResource)
                .Where(x => x.HumanResourceId == gd);
        }

        public HumanResourceSalary GetHumanSalaryByHumanSalaryId(Guid gd)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(x => x.HumanResource).ThenInclude(a => a.Gu)
                .Include(x => x.CreateUser)
                .Include(x => x.ModifiedUser)
                .Include(x => x.SalaryType)
                .Where(x => x.Guid == gd).FirstOrDefault();
        }

        public IEnumerable<HumanResourceSalary> GetAllHumanSalaryByForSpecificDate(DateTime dateFrom, DateTime dateTo)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Where(x => x.CreateDate > dateFrom && x.CreateDate < dateTo);
        }

        public decimal? GetHumanSalaryRem(Guid humanSalaryId)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(p => p.SalaryType)
                .Include(p => p.HumanResource)
                .Include(p => p.HumanResourceSalaryPayments)
                .Where(x => x.Guid == humanSalaryId)
                .Select(p => (p.SalaryType.Name.ToLower().Equals("salary") ?
                       ((p.WorkTime ?? 0) < (p.HumanResource.MinWorkTime ?? 0) ?
                                   (((p.Salary ?? 0) / (p.HumanResource.MinWorkTime ?? 0)) * (p.WorkTime ?? 0)) :
                                    ((p.Salary ?? 0) + ((p.ExtraSalary ?? 0) * ((p.WorkTime ?? 0) - (p.HumanResource.MinWorkTime ?? 0))))) :
                        (p.Salary ?? 0))
                     - (p.HumanResourceSalaryPayments.Sum(s => s.Amount)))
                .SingleOrDefault();

        }

        public HumanResourceSalary GetFirstOrDefault(Expression<Func<HumanResourceSalary, bool>> predicate = null)
        {
            IQueryable<HumanResourceSalary> result = _context.HumanResourceSalaries.AsNoTracking();

            if (predicate != null)
                result = result.Where(predicate);

            return result.FirstOrDefault();
        }

        //public IEnumerable<HumanResourceSalary> GetDetailSalaryReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<HumanResourceSalary, bool>> predicate = null)
        //{
        //    IQueryable<HumanResourceSalary> result = _context.HumanResourceSalaries.AsNoTracking()
        //         .Include(p => p.HumanResource.Gu.UserType)
        //         .Include(p => p.HumanResource.Gu.ClinicSection)
        //         .Include(p => p.PaymentStatus)
        //         .Include(p => p.SalaryType)
        //         .Include(p => p.HumanResourceSalaryPayments)
        //         .Where(p => clinicSectionId.Contains(p.HumanResource.Gu.ClinicSectionId ?? Guid.Empty))
        //         ;

        //    if (predicate != null)
        //        result = result.Where(predicate);

        //    return result.Select(s => new HumanResourceSalary
        //    {
        //        WorkTime = s.WorkTime,
        //        ExtraSalary = s.ExtraSalary,
        //        Salary = s.Salary,
        //        PaymentStatus = new BaseInfoGeneral
        //        {
        //            Name = s.PaymentStatus.Name
        //        },
        //        SalaryType = new BaseInfoGeneral
        //        {
        //            Name = s.SalaryType.Name
        //        },
        //        HumanResource = new HumanResource
        //        {
        //            MinWorkTime = s.HumanResource.MinWorkTime,
        //            Gu = new User
        //            {
        //                Name = s.HumanResource.Gu.Name,
        //                UserType = new BaseInfoGeneral
        //                {
        //                    Name = s.HumanResource.Gu.UserType.Name
        //                },
        //                ClinicSection = new ClinicSection
        //                {
        //                    Name = s.HumanResource.Gu.ClinicSection.Name
        //                }
        //            }
        //        },
        //        HumanResourceSalaryPayments = (ICollection<HumanResourceSalaryPayment>)s.HumanResourceSalaryPayments.Where(d => d.CreatedDate >= fromDate && d.CreatedDate <= toDate).Select(p => new HumanResourceSalaryPayment
        //        {
        //            CreatedDate = p.CreatedDate,
        //            Amount = p.Amount
        //        })
        //    });
        //}


        public IEnumerable<HumanResourceSalary> GetUnpaidHumanSalariesByHumanId(Guid humanResourceId)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(p => p.SalaryType)
                .Include(p => p.HumanResource)
                .Include(p => p.HumanResourceSalaryPayments)
                .Where(p => p.HumanResourceId == humanResourceId)
                .Select(p => new HumanResourceSalary
                {
                    Guid = p.Guid,
                    CurrencyId = p.CurrencyId,
                    Salary = ((p.SalaryType.Name.ToLower().Equals("salary") ?
                            ((p.WorkTime ?? 0) < (p.HumanResource.MinWorkTime ?? 0) ?
                                        (((p.Salary ?? 0) / (p.HumanResource.MinWorkTime ?? 0)) * (p.WorkTime ?? 0)) :
                                         ((p.Salary ?? 0) + ((p.ExtraSalary ?? 0) * ((p.WorkTime ?? 0) - (p.HumanResource.MinWorkTime ?? 0))))) :
                             (p.Salary ?? 0))
                          - (p.HumanResourceSalaryPayments.Sum(s => s.Amount)))
                })
                .Where(w => w.Salary.GetValueOrDefault(0) > 0);

        }

        public HumanResourceSalary GetWithPaymentBySurgeryAndCadreTypeAndSalaryTypeId(Guid? surgeryId, int? cadreTypeId, int? salaryTypeId)
        {
            return _context.HumanResourceSalaries.AsNoTracking()
                .Include(p => p.HumanResourceSalaryPayments)
                .Where(p => p.SurgeryId == surgeryId && p.CadreTypeId == cadreTypeId && p.SalaryTypeId == salaryTypeId)
                .FirstOrDefault();
        }

        public IEnumerable<HumanResourceSalary> GetDetailSalaryReport(List<Guid> clinicSectionId, DateTime fromDate, DateTime toDate, Expression<Func<HumanResourceSalary, bool>> predicate = null)
        {

            IQueryable<HumanResourceSalary> result = _context.HumanResourceSalaries.AsNoTracking()
                 .Include(p => p.HumanResource).ThenInclude(p => p.Gu).ThenInclude(p => p.UserType)
                 .Include(p => p.HumanResource).ThenInclude(p => p.Gu).ThenInclude(p => p.ClinicSection)
                 .Include(p => p.PaymentStatus)
                 .Include(p => p.SalaryType)
                 .Include(p => p.HumanResourceSalaryPayments)
                 .Include(p => p.Surgery).ThenInclude(a => a.Reception).ThenInclude(a => a.ReceptionServices).ThenInclude(a => a.Service)
                 .Include(p => p.Surgery).ThenInclude(a => a.Reception).ThenInclude(a => a.Patient).ThenInclude(a => a.User)
                 .Where(p => clinicSectionId.Contains(p.HumanResource.Gu.ClinicSectionId ?? Guid.Empty) && p.CreateDate >= fromDate && p.CreateDate <= toDate)
                 ;


            //IQueryable<HumanResourceSalaryPayment> result = _context.HumanResourceSalaryPayments.AsNoTracking()
            //     .Include(p => p.HumanResourceSalary).ThenInclude(p => p.HumanResource).ThenInclude(p => p.Gu).ThenInclude(p => p.UserType)
            //     .Include(p => p.HumanResourceSalary).ThenInclude(p => p.HumanResource).ThenInclude(p => p.Gu).ThenInclude(p => p.ClinicSection)
            //     .Include(p => p.HumanResourceSalary).ThenInclude(p => p.PaymentStatus)
            //     .Include(p => p.HumanResourceSalary).ThenInclude(p => p.SalaryType)
            //     .Where(p => /*clinicSectionId.Contains(p.HumanResourceSalary.HumanResource.Gu.ClinicSectionId ?? Guid.Empty) &&*/ p.CreatedDate >= fromDate && p.CreatedDate <= toDate)
            //     ;

            if (predicate != null)
                result = result.Where(predicate);

            return result.Select(s => new HumanResourceSalary
            {
                Guid = s.Guid,
                WorkTime = s.WorkTime,
                ExtraSalary = s.ExtraSalary,
                CreateDate = s.CreateDate,
                Salary = s.Salary,
                PaymentStatusId = s.PaymentStatusId,
                PaymentStatus = new BaseInfoGeneral
                {
                    Name = s.PaymentStatus.Name
                },
                SalaryType = new BaseInfoGeneral
                {
                    Name = s.SalaryType.Name
                },
                HumanResource = new HumanResource
                {
                    MinWorkTime = s.HumanResource.MinWorkTime,
                    Gu = new User
                    {
                        Name = s.HumanResource.Gu.Name,
                        UserType = new BaseInfoGeneral
                        {
                            Name = s.HumanResource.Gu.UserType.Name
                        },
                        ClinicSection = new ClinicSection
                        {
                            Name = s.HumanResource.Gu.ClinicSection.Name
                        }
                    }
                },
                Surgery = new Surgery
                {
                    Reception = new Reception
                    {
                        ReceptionDate = s.Surgery.Reception.ReceptionDate,
                        Patient = new Patient 
                        {
                            User = new User 
                            {
                                Name = s.Surgery.Reception.Patient.User.Name
                            }
                        },
                        ReceptionServices = s.Surgery.Reception.ReceptionServices.Select(a => new ReceptionService
                        {
                            Service = new Service
                            {
                                Name = a.Service.Name,
                                DoctorWage = a.Service.DoctorWage
                            }
                        }).ToList()
                    }
                },
                HumanResourceSalaryPayments = s.HumanResourceSalaryPayments.Select(a => new HumanResourceSalaryPayment
                {
                    CreatedDate = a.CreatedDate,
                    Amount = a.Amount
                }).ToList()


            });

            //return result.Select(s => new HumanResourceSalaryPayment
            //{
            //    CreatedDate = s.CreatedDate,
            //    Amount = s.Amount,
            //    HumanResourceSalary = new HumanResourceSalary
            //    {
            //        Guid = s.HumanResourceSalary.Guid,
            //        WorkTime = s.HumanResourceSalary.WorkTime,
            //        ExtraSalary = s.HumanResourceSalary.ExtraSalary,
            //        Salary = s.HumanResourceSalary.Salary,
            //        PaymentStatusId = s.HumanResourceSalary.PaymentStatusId,
            //        PaymentStatus = new BaseInfoGeneral
            //        {
            //            Name = s.HumanResourceSalary.PaymentStatus.Name
            //        },
            //        SalaryType = new BaseInfoGeneral
            //        {
            //            Name = s.HumanResourceSalary.SalaryType.Name
            //        },
            //        HumanResource = new HumanResource
            //        {
            //            MinWorkTime = s.HumanResourceSalary.HumanResource.MinWorkTime,
            //            Gu = new User
            //            {
            //                Name = s.HumanResourceSalary.HumanResource.Gu.Name,
            //                UserType = new BaseInfoGeneral
            //                {
            //                    Name = s.HumanResourceSalary.HumanResource.Gu.UserType.Name
            //                },
            //                ClinicSection = new ClinicSection
            //                {
            //                    Name = s.HumanResourceSalary.HumanResource.Gu.ClinicSection.Name
            //                }
            //            }
            //        },
            //    }

            //});

        }
    }
}
