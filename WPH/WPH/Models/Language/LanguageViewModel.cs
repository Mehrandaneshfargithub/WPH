using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WPH.Models.CustomDataModels.Language
{
    public class LanguageViewModel
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int ExpressionId { get; set; }
        public string Expression { get; set; }                 
        public string ExpressionEquivalent { get; set; }
    }
}