using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class Language
    {
        public Language()
        {
            LanguageExpressions = new HashSet<LanguageExpression>();
        }

        public int Id { get; set; }
        public string LanguageName { get; set; }

        public virtual ICollection<LanguageExpression> LanguageExpressions { get; set; }
    }
}
