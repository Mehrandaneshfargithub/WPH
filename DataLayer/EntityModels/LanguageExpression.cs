using System;
using System.Collections.Generic;

#nullable disable

namespace DataLayer.EntityModels
{
    public partial class LanguageExpression
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public int ExpressionId { get; set; }
        public string ExpressionEquivalent { get; set; }

        public virtual AllExpression Expression { get; set; }
        public virtual Language Language { get; set; }
    }
}
