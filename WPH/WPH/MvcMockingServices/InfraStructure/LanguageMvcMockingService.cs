using AutoMapper;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using DataLayer.Repositories;
using DataLayer.EntityModels;
using WPH.Models.Language;
using WPH.MvcMockingServices.Interface;

namespace WPH.MvcMockingServices.InfraStructure
{

    public class LanguageMvcMockingService : ILanguageMvcMockingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDIUnit _dIUnit;

        private string culture;
        private string Language;
       

        public LanguageMvcMockingService(IUnitOfWork unitOfWork, IDIUnit dIUnit)
        {
            _unitOfWork = unitOfWork ?? new UnitOfWork();
            _dIUnit = dIUnit;
            culture = LoadLanguageCookie();
            Language = GetLanguageBasedOnCulture();
        }
        
        public string GetLanguageExpression(string expression)
        {
            int languageId = _unitOfWork.LanguageExps.GetLanguageId(Language);
            int expressionId = _unitOfWork.Expressions.GetSingle(x => x.ExpressionText == expression).Id;
            return _unitOfWork.LanguageExps.GetSingle(x => x.LanguageId == languageId && x.ExpressionId == expressionId).ExpressionEquivalent;
        }
        public int GetLanguageId()
        {
            return _unitOfWork.LanguageExps.GetLanguageId(Language);
        }
        public string LoadLanguageCookie()
        {
            string culture = "";
            try
            {
                //IRequestCookieCollection languageCulture = _request.Cookies["languageCulture"];
                //if (languageCulture == null)
                //{
                //    culture = GetDefaultCulture();
                //    _userMvcService.saveCookie("languageCulture", "culture", "ku", _response);
                //}
                //else
                //{
                //    if (!string.IsNullOrWhiteSpace(languageCulture.Values["culture"]))
                //    {
                //        culture = languageCulture.Values["culture"].ToString();
                //    }
                //    else
                //    {
                //        culture = GetDefaultCulture();
                //        _userMvcService.saveCookie("languageCulture", "culture", "ku", _response);
                //    }
                //}
            }
            catch (Exception ex) { }
            return culture;
        }
        public string GetLanguageBasedOnCulture()
        {
            if (culture == "en")
            {
                return "English";
            }
            else if (culture == "ku")
            {
                return "Kurdi";
            }
            else if (culture == "ar")
            {
                return "Arabic";
            }
            else if (culture == "fa")
            {
                return "Persian";
            }
            else
            {
                return "Kurdi";
            }
        }
        public static string GetDefaultCulture()
        {
            return "ku";
        }

        public List<ExpressionViewModel> GetAllLanguageExpression()
        {
            culture = LoadLanguageCookie();
            Language = GetLanguageBasedOnCulture();
            int languageId = _unitOfWork.LanguageExps.GetLanguageId(Language);
            //IEnumerable<LanguageExpression> expressions = _unitOfWork.LanguageExps.Find(x => x.LanguageId == languageId);
            IEnumerable<LanguageExpression> expressions = _unitOfWork.LanguageExps.GetAllExps(languageId);
            return convertModelsLists(expressions);
        }

        public List<ExpressionViewModel> convertModelsLists(IEnumerable<LanguageExpression> langExp)
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LanguageExpression, ExpressionViewModel>().ForMember(a => a.Expression, b => b.MapFrom(c => c.Expression.ExpressionText))
                .ForMember(a => a.ExpressionEquivalent, b => b.MapFrom(c => c.ExpressionEquivalent));
            });

            IMapper mapper = config.CreateMapper();

            return mapper.Map<IEnumerable<LanguageExpression>, List<ExpressionViewModel>>(langExp);
        }


    }
}
