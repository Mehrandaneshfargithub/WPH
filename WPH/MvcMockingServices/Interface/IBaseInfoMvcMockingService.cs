using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Helper;
using WPH.Models.CustomDataModels.BaseInfo;

namespace WPH.MvcMockingServices.Interface
{
    public interface IBaseInfoMvcMockingService
    {
        IEnumerable<BaseInfoGeneralViewModel> GetAllBaseInfoGenerals(string baseinfoType);
        IEnumerable<BaseInfoGeneralViewModel> GetAllBaseInfoGeneralsExcept(string baseinfoType, string exceptName);
        IEnumerable<BaseInfoViewModel> GetAllBaseInfos(string baseinfoType, Guid clinicSectionId);
        int GetBaseInfoGeneralByName(string baseinfoGeneralName);
        BaseInfoGeneralViewModel GetBaseInfoGeneral(int baseInfoId);
        IEnumerable<BaseInfoViewModel> GetAllBaseInfos(Guid baseinfoTypeId, Guid clinicSectionId);
        IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoTypesForSpecificSection(int sectionTypeId, IStringLocalizer<SharedResource> _localizer);
        IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoTypes();
        Guid GetBaseInfoTypeIdByName(string BaseInfoTypeName);
        IEnumerable<BaseInfoTypeViewModel> GetAllBaseInfoType();
        void AddBaseInfoTypeOfBaseInfo(Guid baseInfoId, Guid baseInfoTypeId);
        void GetModalsViewBags(dynamic viewBag);
        Guid AddNewBaseInfo(BaseInfoViewModel newBaseInfo, Guid clinicSectionId);
        Guid UpdateBaseInfo(BaseInfoViewModel baseInfo);
        OperationStatus RemoveBaseInfo(Guid baseInfoId);
        BaseInfoViewModel GetBaseInfo(Guid baseInfoId);
        bool CheckRepeatedInfoBaseName(string name, Guid clinicSectionId, bool NewOrUpdate, Guid baseInfoTypeId, string oldName = "");
        IEnumerable<PeriodsViewModel> GetAllPeriods(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllMonthPeriods(IStringLocalizer<SharedResource> _localizer);
        BaseInfoViewModel GetBaseInfoByName(string baseInfo, Guid ClinicSectionId);
        IEnumerable<BaseInfoViewModel> GetAllBaseInfos(string v);
        IEnumerable<PeriodsViewModel> GetAllClearanceType(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllPaymentStatus(IStringLocalizer<SharedResource> _localizer);
        Guid GetBaseInfoTypeGuidByName(string baseinfoName);
        IEnumerable<BaseInfoGeneralViewModel> GetCustomInvoiceDetailTypes(string baseinfoType);
        int? GetIdByNameAndType(string name, string type);
        IEnumerable<PeriodsViewModel> GetAllPurchaseFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllSaleFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllTransferFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllReturnPurchaseFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllSupplierFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllPayReportFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllTransferYearFilter(IStringLocalizer<SharedResource> _localizer, string year);
        IEnumerable<PeriodsViewModel> GetAllReturnSaleFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllReceiveReportFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetMedicineProductFilter(IStringLocalizer<SharedResource> _localizer);
        IEnumerable<PeriodsViewModel> GetAllProductReportFilter(IStringLocalizer<SharedResource> _localizer);
    }
}
