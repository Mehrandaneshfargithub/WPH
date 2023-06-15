using AutoMapper;
using DataLayer.EntityModels;
using DataLayer.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.Fund;
using WPH.Models.PurchaseInvoice;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class FundMvcMockingService : IFundMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _idunit;
        private readonly IMemoryCache _memoryCache;

        public FundMvcMockingService(IUnitOfWork unitOfWork, IDIUnit idunit, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _idunit = idunit;
            _memoryCache = memoryCache;
        }

        public FundReportViewModel GetAllReceivesForHospital(List<Guid> clinicSectionIds, DateTime DateFrom, DateTime DateTo, bool detail)
        {
            var justDateFrom = DateFrom;
            var justDateTo = DateTo;
            CommonWas.GetPeriodDateTimes(ref justDateFrom, ref justDateTo, 1);

            IEnumerable<ReceptionService> fundDtos;

            fundDtos = _unitOfWork.ReceptionServices.GetAllReceptionServices(clinicSectionIds, DateFrom, DateTo);

            var salary = _unitOfWork.HumanResourceSalaryPayments.GetSalaryForReport(clinicSectionIds, DateFrom, DateTo);

            var costDtos = _unitOfWork.Costs.GetAllCostsForReport(clinicSectionIds, DateFrom, DateTo);

            var purchaseDtos = _unitOfWork.PurchaseInvoices.GetAllPurchaseInvoiceFroReport(clinicSectionIds, justDateFrom, justDateTo, detail).ToList();

            var temp_funds = new List<FundViewModel>();


            temp_funds.AddRange(fundDtos.Where(item => item.ReceptionServiceReceiveds != null).SelectMany(p => p.ReceptionServiceReceiveds, (p, recive) => new FundViewModel
            {
                Amount = recive.AmountStatus.GetValueOrDefault(false) ? $"-{recive.Amount.Value:N0}" : recive.Amount.Value.ToString("N0"),
                Temp_Amount = recive.AmountStatus.GetValueOrDefault(false) ? -recive.Amount.Value : recive.Amount.Value,
                Date = recive.CreatedDate.GetValueOrDefault().Date,
                ClinicSectionName = p.Reception.ClinicSection.Name,
                ReceptionNum = p.Reception.ReceptionNum,
                RadiologyDoctor = p.Reception?.ReceptionDoctors?.FirstOrDefault()?.Doctor?.User?.Name ?? ""
            }));


            temp_funds.AddRange(salary.Select(item => new FundViewModel
            {
                Amount = $"-{item.Amount.Value:N0}",
                Temp_Amount = -item.Amount,
                Date = item.CreatedDate.GetValueOrDefault().Date,
                ClinicSectionName = $"{item.HumanResourceSalary.HumanResource.Gu.ClinicSection.Name} - {item.HumanResourceSalary.SalaryType.Name}"
            }));


            temp_funds.AddRange(costDtos.Select(item => new FundViewModel
            {
                Amount = $"-{item.Price.Value:N0}",
                Temp_Amount = -item.Price,
                Date = item.CostDate.GetValueOrDefault().Date,
                ClinicSectionName = $"{item.ClinicSection.Name} - {item.CostType.Name}"
            }));


            FundReportViewModel funds = new();
            funds.AllFund = new List<FundViewModel>();
            funds.AllSectionsTotal = new List<FundViewModel>();
            funds.TotalCurrency = new List<PurchaseInvoiceReportViewModel>();

            CultureInfo cultures = new CultureInfo("en-US");
            if (detail)
            {
                funds.AllFund = temp_funds.Where(p => !string.IsNullOrWhiteSpace(p.ReceptionNum)).GroupBy(p => new { p.ClinicSectionName, p.Date, p.ReceptionNum, p.RadiologyDoctor }).Select(p => new FundViewModel
                {
                    Date = p.Key.Date,
                    ClinicSectionName = p.Key.ClinicSectionName,
                    Amount = p.Sum(s => s.Temp_Amount.Value).ToString("N0"),
                    ReceptionNum = p.Key.ReceptionNum,
                    RadiologyDoctor = p.Key.RadiologyDoctor
                }).ToList();

                funds.AllFund.AddRange(temp_funds.Where(p => string.IsNullOrWhiteSpace(p.ReceptionNum)).Select(p => new FundViewModel
                {
                    Date = p.Date,
                    ClinicSectionName = p.ClinicSectionName,
                    Amount = p.Amount,
                    ReceptionNum = p.ReceptionNum
                }).ToList());


                funds.PurchaseFundDetail = new List<PurchaseInvoiceReportViewModel>();
                if (purchaseDtos.Any())
                    funds.PurchaseFundDetail.AddRange(purchaseDtos.SelectMany(p => p.PurchaseInvoiceDetails, (p, s) => new PurchaseInvoiceReportViewModel
                    {
                        InvoiceNum = p?.InvoiceNum ?? "",
                        InvoiceDate = p?.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures) ?? "",
                        Supplier = p?.Supplier?.User?.Name ?? "",
                        ///Currency = p?.Currency?.Name ?? "",
                        Product = s?.Product?.Name ?? "",
                        TempNum = s?.Num.GetValueOrDefault(0) ?? 0,
                        TempPurchasePrice = s?.PurchasePrice.GetValueOrDefault(0) ?? 0,
                        TempDiscount = s?.Discount.GetValueOrDefault(0) ?? 0,
                        //TempWholeDiscount = p?.Discount.GetValueOrDefault(0) ?? 0,
                        //TempWholePurchasePrice = p?.TotalPrice.GetValueOrDefault(0) ?? 0
                    }).ToList());

            }
            else
            {
                funds.AllFund = temp_funds.GroupBy(p => new { p.ClinicSectionName, p.Date }).Select(x => new FundViewModel
                {
                    ClinicSectionName = x.Key.ClinicSectionName,
                    Date = x.Key.Date,
                    Amount = x.Sum(p => p.Temp_Amount.Value).ToString("N0")

                }).ToList();


                funds.PurchaseFund = new List<PurchaseInvoiceReportViewModel>();
                if (purchaseDtos.Any())
                    funds.PurchaseFund.AddRange(purchaseDtos.Select(p => new PurchaseInvoiceReportViewModel
                    {
                        InvoiceDate = p?.InvoiceDate.Value.ToString("dd/MM/yyyy", cultures) ?? "",
                        Supplier = p?.Supplier?.User?.Name ?? "",
                        //Currency = p?.Currency?.Name ?? "",
                        //TempWholePurchasePrice = p?.TotalPrice.GetValueOrDefault(0) ?? 0,
                        //TempWholeDiscount = p?.Discount.GetValueOrDefault(0) ?? 0,
                        TempNum = 0,
                        TempDiscount = 0,
                        TempPurchasePrice = 0
                    }).ToList());
            }

            funds.AllSectionsTotal = temp_funds.GroupBy(p => p.ClinicSectionName).Select(x => new FundViewModel
            {
                ClinicSectionName = x.Key,
                Amount = x.Sum(p => p.Temp_Amount.Value).ToString("N0"),
                AnalysisNumber = x.Count().ToString("N0")

            }).ToList();

            funds.Total = temp_funds.Sum(p => p.Temp_Amount.Value).ToString("N0");

            //if (purchaseDtos.Any())
            //    funds.TotalCurrency.AddRange(purchaseDtos.GroupBy(p => p.Currency.Name).Select(p => new PurchaseInvoiceReportViewModel
            //    {
            //        Currency = p.Key,
            //        TempWholeDiscount = p.Sum(s => s.Discount.GetValueOrDefault(0)),
            //        TempWholePurchasePrice = p.Sum(s => s.TotalPrice.GetValueOrDefault(0)),
            //        TempNum = 0,
            //        TempDiscount = 0,
            //        TempPurchasePrice = 0
            //    }).ToList());

            return funds;
        }

        //public FundReportViewModel GetAllReceives(List<Guid> clinicSectionIds, DateTime DateFrom, DateTime DateTo, bool Detail)
        //{
        //    List<PatientReceptionReceived> costDtos = new List<PatientReceptionReceived>();

        //    costDtos = _unitOfWork.PatientReceptionReceived.GetAllReceives(clinicSectionIds, DateFrom, DateTo).ToList();

        //    IEnumerable<Guid> allReceptionIds = costDtos.Select(x => x.ReceptionId);

        //    FundReportViewModel costs = new();
        //    costs.AllFund = new List<FundViewModel>();

        //    if (Detail)
        //    {

        //        List<PatientReceptionAnalysis> allPRA = _unitOfWork.PatientReceptionAnalysis.GetAllPatientReceptionAnalysisByClinicSectionIds(allReceptionIds, DateFrom, DateTo).ToList();

        //        var analysisItems = _memoryCache.Get<List<AnalysisItem>>("analysisItems");
        //        var analysis = _memoryCache.Get<List<Analysis>>("analysis");
        //        var groupAnalysis = _memoryCache.Get<List<GroupAnalysis>>("groupAnalysis");


        //        foreach (var reception in allPRA)
        //        {
        //            if (reception.AnalysisId != null)
        //            {
        //                reception.Analysis = new Analysis();
        //                reception.Analysis.Name = analysis.FirstOrDefault(x => x.Guid == reception.AnalysisId).Name;
        //            }
        //            else if (reception.AnalysisItemId != null)
        //            {
        //                reception.AnalysisItem = new AnalysisItem();
        //                reception.AnalysisItem.Name = analysisItems.FirstOrDefault(x => x.Guid == reception.AnalysisItemId).Name;
        //            }
        //            else if (reception.GroupAnalysisId != null)
        //            {
        //                reception.GroupAnalysis = new GroupAnalysis();
        //                reception.GroupAnalysis.Name = groupAnalysis.FirstOrDefault(x => x.Guid == reception.GroupAnalysisId).Name;
        //            }
        //        }

        //        foreach (var section in costDtos)
        //        {
        //            var amount = section.Amount;
        //            costs.AllFund.Add(
        //                new FundViewModel
        //                {
        //                    Amount = amount.GetValueOrDefault(0).ToString("N0"),
        //                    Date = section.Date.GetValueOrDefault(),
        //                    ClinicSectionName = section.Reception.ClinicSection.Name,
        //                    ReceptionNum = section.Reception.ReceptionNum,
        //                    RadiologyDoctor = section.Reception?.ReceptionDoctors?.FirstOrDefault()?.Doctor?.User?.Name ?? ""
        //                });

        //        }

        //        costs.AllSectionsTotal = new List<FundViewModel>();
        //        List<string> allSections = costDtos.Select(x => x.Reception.ClinicSection.Name).Distinct().ToList();
        //        costs.AllAnalysis = new List<FundViewModel>();

        //        IEnumerable<Guid?> allAnalysis = allPRA.Select(x => x.AnalysisId).Distinct();
        //        IEnumerable<Guid?> allAnalysisItem = allPRA.Select(x => x.AnalysisItemId).Distinct();
        //        IEnumerable<Guid?> allGroupAnalysis = allPRA.Select(x => x.GroupAnalysisId).Distinct();
        //        foreach (var section in allSections)
        //        {
        //            costs.AllSectionsTotal.Add(new FundViewModel
        //            {
        //                Amount = costDtos.Where(x => x.Reception.ClinicSection.Name == section).Sum(a => a.Amount).GetValueOrDefault().ToString("N0"),
        //                ClinicSectionName = section,
        //                AnalysisNumber = costDtos.Where(x => x.Reception.ClinicSection.Name == section).Count().ToString()
        //            });

        //            foreach (var analyses in allAnalysis)
        //            {
        //                try
        //                {
        //                    var ss = allPRA.FirstOrDefault(x => x.AnalysisId == analyses);
        //                    costs.AllSectionsTotal.Add(new FundViewModel
        //                    {
        //                        ClinicSectionName = allPRA.FirstOrDefault(x => x.AnalysisId == analyses).Analysis.Name,
        //                        AnalysisNumber = allPRA.Where(x => x.AnalysisId == analyses).Count().ToString()
        //                    });
        //                }
        //                catch (Exception e) { }

        //            }

        //            foreach (var analyses in allAnalysisItem)
        //            {
        //                try
        //                {
        //                    costs.AllSectionsTotal.Add(new FundViewModel
        //                    {
        //                        ClinicSectionName = allPRA.FirstOrDefault(x => x.AnalysisItemId == analyses).AnalysisItem.Name,
        //                        AnalysisNumber = allPRA.Where(x => x.AnalysisItemId == analyses).Count().ToString()
        //                    });
        //                }
        //                catch { }
        //            }

        //            foreach (var analyses in allGroupAnalysis)
        //            {
        //                try
        //                {
        //                    costs.AllSectionsTotal.Add(new FundViewModel
        //                    {
        //                        ClinicSectionName = allPRA.FirstOrDefault(x => x.GroupAnalysisId == analyses).GroupAnalysis.Name,
        //                        AnalysisNumber = allPRA.Where(x => x.GroupAnalysisId == analyses).Count().ToString()
        //                    });
        //                }
        //                catch { }

        //            }

        //        }

        //    }
        //    else
        //    {
        //        IEnumerable<DateTime> allTime = costDtos.Select(x => x.Date.GetValueOrDefault().Date).Distinct();
        //        List<string> allSections = costDtos.Select(x => x.Reception.ClinicSection.Name).Distinct().ToList();

        //        foreach (var section in allSections)
        //        {
        //            foreach (var time in allTime)
        //            {
        //                var f = time.Date;
        //                var amount = costDtos.Where(x => x.Date.GetValueOrDefault().Date == time.Date && x.Reception.ClinicSection.Name == section).Sum(a => a.Amount);

        //                costs.AllFund.Add(
        //                    new FundViewModel
        //                    {
        //                        Amount = amount.GetValueOrDefault(0).ToString("N0"),
        //                        Date = time.Date,
        //                        ClinicSectionName = section,
        //                    });

        //            }
        //        }

        //        costs.AllSectionsTotal = new List<FundViewModel>();

        //        foreach (var section in allSections)
        //        {
        //            costs.AllSectionsTotal.Add(new FundViewModel { Amount = costDtos.Where(x => x.Reception.ClinicSection.Name == section).Sum(a => a.Amount).GetValueOrDefault().ToString("N0"), ClinicSectionName = section, AnalysisNumber = costDtos.Where(x => x.Reception.ClinicSection.Name == section).Count().ToString() });
        //        }
        //    }

        //    costs.DoctorFund = costDtos.Select(s => new FundViewModel
        //    {
        //        Temp_Amount = 1,
        //        Date = s.Date.GetValueOrDefault().Date,
        //        RadiologyDoctor = s.Reception?.ReceptionDoctors?.FirstOrDefault()?.Doctor?.User?.Name ?? ""
        //    }).GroupBy(g => new { g.Date, g.RadiologyDoctor }).Select(p => new FundViewModel
        //    {
        //        Amount = p.Count().ToString("N0"),
        //        Date = p.Key.Date,
        //        RadiologyDoctor = p.Key.RadiologyDoctor
        //    }).ToList();


        //    costs.Total = costDtos.Sum(x => x.Amount).GetValueOrDefault().ToString("N0");

        //    return costs;
        //}

    }
}
