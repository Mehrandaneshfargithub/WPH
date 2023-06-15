using DataLayer.EntityModels;
using DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.Reserve;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{
    public class ClinicCalendarMvcMockingService : IClinicCalendarMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClinicCalendarMvcMockingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
        }

        public List<ReserveViewModel> GetClinicCalendarDaysByClinicSection(Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate)
        {

            List<Reserve> calendarDays = new List<Reserve>();
            List<Reserve> newCalendarDays = new List<Reserve>();


            fromDate = fromDate.AddDays(-7);

            if (periodId == (int)Periods.FromDateToDate)
            {
                calendarDays = _unitOfWork.Reserves.GetClinicCalendarDaysByClinicSection(clinicSectionId, fromDate, toDate).ToList();
            }
            else
            {
                DateTime DateFrom = DateTime.Now.AddDays(-7);
                DateTime DateTo = DateTime.Now;
                GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                calendarDays = _unitOfWork.Reserves.GetClinicCalendarDaysByClinicSection(clinicSectionId, DateFrom, DateTo).ToList();
            }

            int count = 0;
            
            if(periodId == (int)Periods.Day)
            {
                count = 1;
            }
            else if (periodId == (int)Periods.Week)
            {
                count = 7;
            }
            else if (periodId == (int)Periods.Month)
            {
                count = 30;
            }
            else if (periodId == (int)Periods.FromDateToDate)
            {
                count = (toDate - fromDate).Days;
            }

            for (int i = 0; i < count; i++)
            {

                Reserve today = calendarDays.SingleOrDefault(x=>x.Date == DateTime.Now.AddDays(i).Date);


                if(today == null)
                {
                    Reserve prevoday = calendarDays.SingleOrDefault(x => x.Date == DateTime.Now.AddDays(i - 7).Date);

                    if(prevoday == null)
                    {
                        today = new Reserve();
                        today.ClinicSectionId = clinicSectionId;
                        today.Date = DateTime.Now.AddDays(i).Date;
                        today.StartTime = new TimeSpan(14, 0, 0);
                        today.EndTime = new TimeSpan(17, 0, 0);
                        today.RoundTime = 15;
                    }
                    else
                    {
                        today = new Reserve();
                        today.Date = DateTime.Now.AddDays(i).Date;
                        today.StartTime = prevoday.StartTime;
                        today.EndTime = prevoday.EndTime;
                        today.RoundTime = prevoday.RoundTime;

                    }

                    
                }

                newCalendarDays.Add(today);

            }

            

            return Common.ConvertModels<ReserveViewModel, Reserve>.convertModelsLists(newCalendarDays);
        }

        public void GetPeriodDateTimes(ref DateTime dateFrom, ref DateTime dateTo, int periodId)
        {
            switch (periodId)
            {
                case (int)Periods.Day:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
                    break;
                case (int)Periods.Week:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59).AddDays(7);
                    break;
                case (int)Periods.Month:
                    dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
                    dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59).AddDays(30);
                    break;
                
                
            }
        }

        public async Task SaveClinicCalendar(List<ReserveViewModel> reserves, Guid clinicSectionId, int periodId, DateTime fromDate, DateTime toDate)
        {

            List<Reserve> allReserve = Common.ConvertModels<Reserve, ReserveViewModel>.convertModelsLists(reserves);

            if (periodId == (int)Periods.FromDateToDate)
            {
               await _unitOfWork.Reserves.SaveClinicCalendar(allReserve, clinicSectionId, fromDate, toDate);
            }
            else
            {
                DateTime DateFrom = DateTime.Now;
                DateTime DateTo = DateTime.Now;
                GetPeriodDateTimes(ref DateFrom, ref DateTo, periodId);
                await _unitOfWork.Reserves.SaveClinicCalendar(allReserve, clinicSectionId, DateFrom, DateTo);
            }
        }
    }
}
