using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WPH.Areas.Admin.Models.BaseInfoType;
using WPH.Helper;

namespace WPH.MvcMockingServices.Interface
{
    public interface IBaseInfoTypeMvcMockingService
    {
        string AddNewBaseInfoType(BaseInfoTypeViewModel baseInfoType);
        string UpdateBaseInfoType(BaseInfoTypeViewModel baseInfoType);
        BaseInfoTypeViewModel GetBaseInfoType(Guid id);
        OperationStatus RemoveBaseInfoType(Guid id);
        List<BaseInfoTypeViewModel> GetAllBaseInfoTypes();
        List<BaseInfoSectioTypeViewModel> GetBaseInfoSectioType(Guid baseInfoTypeId);
        void AddBaseInfoSectioType(Guid baseInfoTypeId, List<BaseInfoSectioTypeViewModel> sectionTypes);
        List<BaseInfoSectioTypeViewModel> GetSubsystemSectioType(int subSystemId);
    }
}
