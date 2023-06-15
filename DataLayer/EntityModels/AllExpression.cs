using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class AllExpression
    {
        public AllExpression()
        {
            LanguageExpressions = new HashSet<LanguageExpression>();
        }

        public int Id { get; set; }
        public string ExpressionText { get; set; }

        public virtual ICollection<LanguageExpression> LanguageExpressions { get; set; }
    }
}
