#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Analysis\dgAnalysisAnalysisItemGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9af7c83de60ee52a5d0884c9f5112b96e10262ae"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Analysis.Views_Shared_PartialViews_AppWebForms_Analysis_dgAnalysisAnalysisItemGrid), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Analysis/dgAnalysisAnalysisItemGrid.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Analysis
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
#line 1 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Analysis\dgAnalysisAnalysisItemGrid.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9af7c83de60ee52a5d0884c9f5112b96e10262ae", @"/Views/Shared/PartialViews/AppWebForms/Analysis/dgAnalysisAnalysisItemGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Analysis_dgAnalysisAnalysisItemGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 6 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Analysis\dgAnalysisAnalysisItemGrid.cshtml"
  
    string remove_title = Localizer["RemoveAnalysisItem"];
    string up_title = Localizer["IncreasePriority"];
    string down_title = Localizer["DecreasePriority"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div style=\"overflow:auto\">\r\n    ");
#nullable restore
#line 14 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Analysis\dgAnalysisAnalysisItemGrid.cshtml"
Write(Html.Kendo().Grid<WPH.Models.CustomDataModels.Analysis_AnalysisItem.Analysis_AnalysisItemViewModel>
    ()
    .Name("kendoAnalysisAnalysisItemGrid")
    .Columns(columns =>
    {

        if (HttpContextAccessor.HttpContext.Session.GetString("culture") != "en")
        {
            columns.Bound("").Title(string.Empty).Width(10)
            .ClientTemplate(
               $@"<a class='tooltip-error grid-btn' onClick='DeleteAnalysisAnalysisItem(this)' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete' data-toggle='modal'>
                    <span class='red'>
                        <i class='ace-icon fa fa-trash-can bigger-120'></i>
                    </span>
                </a>
            ");

            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
               $@"<a class='tooltip-success grid-btn' onClick='GridAnalysisAnalysisItemUpPriorityFunction(this);' data-id='#=Guid#' data-Priority='#=Priority#' data-rel='tooltip' title='{up_title}' data-original-title='Up'>
                    <span class='blue'>
                        <i class='ace-icon fa fa-arrow-up bigger-120'></i>
                    </span>
                </a>
            ");

            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
               $@"<a class='tooltip-success grid-btn' onClick='GridAnalysisAnalysisItemDownPriorityFunction(this);' data-id='#=Guid#' data-Priority='#=Priority#' data-rel='tooltip' title='{down_title}' data-original-title='Down'>
                    <span class='red'>
                        <i class='ace-icon fa fa-arrow-down bigger-120'></i>
                    </span>
                </a>
            ");

            columns.Bound(x => x.AnalysisItem.ValueTypeName).Filterable(false).Width(100).Title(Localizer["Type"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.UnitName).Filterable(false).Width(50).Title(Localizer["Unit"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.AmountCurrencyName).Width(20).Filterable(false).Title(Localizer["Type"] + ' ' + Localizer["Currency"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Amount).Width(100).Filterable(false).Format("{0:n0}").Title(Localizer["Amount"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.NormalValues).Filterable(false).Title(Localizer["NormalRange"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Note).Filterable(false).Width(100).Title(Localizer["AnalysisNote"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Abbreviation).Filterable(false).Width(50).Title(Localizer["Abbreviation"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Code).Filterable(false).Width(50).Title(Localizer["Code"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["AnalysisItemName"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.AnalysisItem.Index).Filterable(false).Width(10).Title("#").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Priority).Hidden();
        }
        else
        {
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Priority).Hidden();
            columns.Bound(x => x.AnalysisItem.Index).Filterable(false).Title("#").Width(10).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Width(300).Title(Localizer["AnalysisItemName"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.Code).Filterable(false).Width(50).Title(Localizer["Code"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.Abbreviation).Filterable(false).Width(50).Title(Localizer["Abbreviation"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.Note).Filterable(false).Width(100).Title(Localizer["AnalysisNote"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.NormalValues).Filterable(false).Title(Localizer["NormalRange"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.Amount).Width(100).Filterable(false).Format("{0:n0}").Title(Localizer["Amount"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.AmountCurrencyName).Width(20).Filterable(false).Title(Localizer["Type"] + ' ' + Localizer["Currency"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.UnitName).Filterable(false).Width(50).Title(Localizer["Unit"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.AnalysisItem.ValueTypeName).Filterable(false).Width(50).Title(Localizer["Type"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });

            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
               $@"<a class='tooltip-success grid-btn' onClick='GridAnalysisAnalysisItemDownPriorityFunction(this);' data-id='#=Guid#' data-Priority='#=Priority#' data-rel='tooltip' title='{down_title}' data-original-title='Down'>
                    <span class='red'>
                        <i class='ace-icon fa fa-arrow-down bigger-120'></i>
                    </span>
                </a>
            ");
            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
               $@"<a class='tooltip-success grid-btn' onClick='GridAnalysisAnalysisItemUpPriorityFunction(this);' data-id='#=Guid#' data-Priority='#=Priority#' data-rel='tooltip' title='{up_title}' data-original-title='Up'>
                    <span class='blue'>
                        <i class='ace-icon fa fa-arrow-up bigger-120'></i>
                    </span>
                </a>
            ");

            columns.Bound("").Title(string.Empty).Width(10)
            .ClientTemplate(
               $@"<a class='tooltip-error grid-btn' onClick='DeleteAnalysisAnalysisItem(this)' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete' data-toggle='modal'>
                    <span class='red'>
                        <i class='ace-icon fa fa-trash-can bigger-120'></i>
                    </span>
                </a>
            ");
        }

    })

        .DataSource(dataSource => dataSource
        .Ajax()
        .ServerOperation(false)
        .Model(model =>
        {
            model.Id(p => p.Guid);
        })
        .PageSize(100)
        .Read(read => read.Action("GetAllAnalysisAnalysisItem", "Analysis").Data("GetAnalysisId"))
        )
        .Filterable(ftb => ftb.Mode(GridFilterMode.Row))
        .Pageable(pageable => pageable
        .Input(true)
        .Refresh(true)
        .PageSizes(true)
        .ButtonCount(5)
        .Numeric(true)) // Enable paging
        .Sortable().HtmlAttributes(new { @style = "direction: ltr;margin-top:2rem;font-size:1.3rem;overflow:auto", @class = "MyFont-Roboto-grid" }) // Enable sorting
    );

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
