using System.ComponentModel.DataAnnotations;

namespace WPH.Models
{
    public class validation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //List<ExpressionDto> langExps = httpContext.Application["AllExpressions"] as List<ExpressionDto>;
            return base.IsValid(value, validationContext);
        }
    }
}