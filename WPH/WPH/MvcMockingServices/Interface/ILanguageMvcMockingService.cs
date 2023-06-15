using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPH.MvcMockingServices.Interface
{
    public interface ILanguageMvcMockingService
    {
        string GetLanguageExpression(string expression);
        int GetLanguageId();
        string LoadLanguageCookie();
        string GetLanguageBasedOnCulture();

    }
}
