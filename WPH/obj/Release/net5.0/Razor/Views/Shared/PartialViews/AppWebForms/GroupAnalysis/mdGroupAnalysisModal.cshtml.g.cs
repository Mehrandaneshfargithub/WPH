#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "729dc3e45c056eb4fb3b572e230eac38bf1118b6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.GroupAnalysis.Views_Shared_PartialViews_AppWebForms_GroupAnalysis_mdGroupAnalysisModal), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/mdGroupAnalysisModal.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.GroupAnalysis
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "H:\Projects\WAS\WPH\Views\_ViewImports.cshtml"
using WPH;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "H:\Projects\WAS\WPH\Views\_ViewImports.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"729dc3e45c056eb4fb3b572e230eac38bf1118b6", @"/Views/Shared/PartialViews/AppWebForms/GroupAnalysis/mdGroupAnalysisModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_GroupAnalysis_mdGroupAnalysisModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WPH.Models.CustomDataModels.GroupAnalysis.GroupAnalysisViewModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("addNewForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n\r\n<div id=\"signup-box\" class=\"signup-box no-border\">\r\n    <div class=\"widget-body\">\r\n        <div class=\"widget-main\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "729dc3e45c056eb4fb3b572e230eac38bf1118b64283", async() => {
                WriteLiteral("\r\n                <fieldset>\r\n                    ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 14 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.Guid, new { id = "Guid" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 15 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.Id, new { id = "Id" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 16 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.CreatedUserId));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 17 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.CreatedDate));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 18 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.Priority));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 19 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
               Write(Html.HiddenFor(m => m.IsButton));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <span class=\"red fa fa-asterisk smaller-60 redStar\"></span><label for=\"form-field-8\">");
#nullable restore
#line 22 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                                                                                        Write(Localizer["Name"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" ");
#nullable restore
#line 22 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                                                                                                           Write(Localizer["GroupAnalysis"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 24 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().TextBox()
                                .Name("Name")
                                .Value(Model.Name)
                                .HtmlAttributes(new { @class = "keypress", style = "width: 100%", @data_checkEmpty = "Name-box" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                        <span id=\"Name-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 31 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                       Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                        <span id=\"Name-Exist\" class=\"NameExist hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 34 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                       Write(Localizer["TheNameIsDuplicatedPleaseTryAnotherName"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 39 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                             Write(Localizer["Section"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 41 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().DropDownList()
                                  .Name("ClinicSectionId")
                                  .DataTextField("Name")
                                  .DataValueField("Guid")
                                  .DataSource(source =>
                                  {
                                      source.Read(read =>
                                      {
                                          read.Action("GetAllLabAndXRayClinicSectionsForUser", "ClinicSection");
                                      })
                                      .ServerFiltering(false);
                                  })
                                  .HtmlAttributes(new { style = "width: 100%;", @data_checkEmpty = "ClinicSection-box" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                        <span id=\"ClinicSection-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 57 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                       Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 62 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                             Write(Localizer["Code"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 64 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().TextBox()
                            .Name("Code")
                            .Value(Model.Code)
                            .HtmlAttributes(new { @class = "keypress", style = "width: 100%" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 73 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                             Write(Localizer["Abbreviation"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 75 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().TextBox()
                            .Name("Abbreviation")
                            .Value(Model.Abbreviation)
                            .HtmlAttributes(new { @class = "keypress", style = "width: 100%" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 83 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                             Write(Localizer["AnalysisNote"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 85 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().TextBox()
                                .Name("Note")
                                .Value(Model.Note)
                                .HtmlAttributes(new { @class = "keypress", style = "width: 100%" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <label class=\"hidden\" for=\"form-field-8\">");
#nullable restore
#line 93 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                                        Write(Localizer["Active"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                    ");
#nullable restore
#line 94 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                Write(Html.Kendo().CheckBoxFor(x => x.IsActive)
                            .Name("IsActive")
                            .Checked(true)
                            .HtmlAttributes(new { @class = "kendoCheckbox hidden", style = "font-size:5rem;margin-bottom: 5px;" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\"># ");
#nullable restore
#line 100 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                               Write(Localizer["Discount"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 102 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().NumericTextBox<decimal>()
                                .Name("Discount")
                                .Min(0)
                                .Max(10000000)
                                .Value(Model.Discount)
                                .HtmlAttributes(new { style = "width: 100%", title = "Discount" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n\r\n                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 114 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                             Write(Localizer["Type"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" ");
#nullable restore
#line 114 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                                                                Write(Localizer["Currency"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 116 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                        Write(Html.Kendo().DropDownList()
                                  .Name("DiscountCurrencyId")
                                  .DataTextField("Name")
                                  .DataValueField("Id")
                                  //.Filter("contains")
                                  .DataSource(source =>
                                  {
                                      source.Read(read =>
                                      {
                                          read.Action("GetAllCurrencies", "BaseInfo");
                                      })
                                      .ServerFiltering(false);
                                  })
                                  .HtmlAttributes(new { style = "width: 100%;", onchange = "CurrencyCh()" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <div class=\"space-24\"></div>\r\n                </fieldset>\r\n            ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n    </div><!-- /.widget-body -->\r\n</div><!-- /.signup-box -->\r\n\r\n\r\n<script>\r\n\r\n    var Discount;\r\n    var decimals = ");
#nullable restore
#line 145 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
              Write(Html.Raw(Json.Serialize(Model.AllDecimalAmount)));

#line default
#line hidden
#nullable disable
            WriteLiteral(@";
    var dinarDecimal;
    var dollarDecimal;
    var euroDecimal;
    var pondDecimal;
    var format;
    $(document).ready(function () {
        setTimeout(function () {
            $(""#Name"").focus();
        }, 200);

        var discountcurrency = $(""#DiscountCurrencyId"").data(""kendoDropDownList"");
        discountcurrency.value(");
#nullable restore
#line 157 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\GroupAnalysis\mdGroupAnalysisModal.cshtml"
                          Write(ViewBag.CurrencyTypeId);

#line default
#line hidden
#nullable disable
            WriteLiteral(@");
        for (let i = 0; i < decimals.length; i++) {
            if (decimals[i].ClinicSectionSettingSName === ""DinarDecimalAmount"")
                dinarDecimal = parseInt(decimals[i].SValue);
            else if(decimals[i].ClinicSectionSettingSName === ""DollarDecimalAmount"" )
                dollarDecimal = parseInt(decimals[i].SValue);
            else if(decimals[i].ClinicSectionSettingSName === ""EuroDecimalAmount"" )
                euroDecimal = parseInt(decimals[i].SValue);
            else if(decimals[i].ClinicSectionSettingSName === ""PondDecimalAmount"" )
                pondDecimal = parseInt(decimals[i].SValue);
        }

        Discount = $(""#Discount"").data(""kendoNumericTextBox"");
        CurrencyCh();
    });

    function CurrencyCh() {

        var currency = $(""#DiscountCurrencyId"").data(""kendoDropDownList"");
        //let text = currency.text();

        if (currency.text() === ""IQD"")
            format = ""n"" + dinarDecimal;
        else if (currency.text() === ""$"")");
            WriteLiteral(@"
                    format = ""n"" + dollarDecimal;
        else if (currency.text() === ""€"")
                    format = ""n"" + euroDecimal;
        else if (currency.text() === ""£"")
                    format = ""n"" + pondDecimal;


        let priceBox = $(""#Discount"").data(""kendoNumericTextBox"");
        priceBox.setOptions({ format: format });

        priceBox.focus();
    }


    $('#Name').keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            var section = $(""#ClinicSectionId"").data(""kendoDropDownList"");
            section.focus();
        }
    });

    $('#ClinicSectionId').parent().keypress(function (e) {
        if (e.which === 13 || e.which === 9) {

            $('#Code').focus();
        }
    });

    $('#Name').focus(function (e) {
        $('#Name').select();
    });

    $('#Code').keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            $('#Abbreviation').focus();
        }
    });

    $('#Code').");
            WriteLiteral(@"focus(function (e) {
        $('#Code').select();
    });

    $('#Abbreviation').keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            $('#Note').focus();
        }
    });

    $('#Abbreviation').focus(function (e) {
        $('#Abbreviation').select();
    });


    $('#Note').keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            Discount.focus();
        }
    });

    $('#Note').focus(function (e) {
        $('#Note').select();
    });

    $('#Discount').keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            let DiscountCurrencyId = $(""#DiscountCurrencyId"").data(""kendoDropDownList"");
            DiscountCurrencyId.focus();
            DiscountCurrencyId.open();
        }
    });

    $('#Discount').focus(function (e) {
        var input = $(this);
        setTimeout(function () {
            input.select();
        });
    });

    $('#DiscountCurrencyId').parent().keypress(funct");
            WriteLiteral(@"ion (e) {
        if (e.which === 13 || e.which === 9) {
            $('#btn-GroupAnalysisModal-accept').focus();
        }
    });



    $('#Name').focus(function () {
        $(""#Name-Exist"").addClass('hidden');
        $(""#Name-box"").addClass('hidden');
    });

    function CurrencyChange() {

        Discount.focus();

    }


</script>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public WPH.Resources.SharedViewLocalizer Localizer { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IHttpContextAccessor HttpContextAccessor { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WPH.Models.CustomDataModels.GroupAnalysis.GroupAnalysisViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
