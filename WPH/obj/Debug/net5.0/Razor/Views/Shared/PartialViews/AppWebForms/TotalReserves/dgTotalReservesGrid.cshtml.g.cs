#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\TotalReserves\dgTotalReservesGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "776c013bfe7c681ae66c34464cc9d83ffc2f819c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.TotalReserves.Views_Shared_PartialViews_AppWebForms_TotalReserves_dgTotalReservesGrid), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/TotalReserves/dgTotalReservesGrid.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.TotalReserves
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
#line 1 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\TotalReserves\dgTotalReservesGrid.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"776c013bfe7c681ae66c34464cc9d83ffc2f819c", @"/Views/Shared/PartialViews/AppWebForms/TotalReserves/dgTotalReservesGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_TotalReserves_dgTotalReservesGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\TotalReserves\dgTotalReservesGrid.cshtml"
  

    string remove_title = Localizer["RemoveReserve"];
    string service_title = Localizer["VisitServices"];
    string status_title = Localizer["ChangeToVisited"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div>\r\n    ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\TotalReserves\dgTotalReservesGrid.cshtml"
Write(Html.Kendo().Grid<WPH.Models.CustomDataModels.ReserveDetail.ReserveDetailViewModel>()
    .Name("kendoTotalReserveGrid")
    .Columns(columns =>
    {

        if (HttpContextAccessor.HttpContext.Session.GetString("culture") != "en")
        {
            if ((bool)ViewBag.AccessDeleteTotalReserves)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                    $@"<a class='tooltip-error grid-btn' onClick='DeleteReserve(this);' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete'>
                        <span class='red'>
                            <i class='ace-icon fa fa-trash-can bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
                $@"<a class='tooltip-success grid-btn' onClick='PayVisitModal(this);' data-id='#=Guid#' data-rel='tooltip' title='{service_title}' data-original-title='Services'>
                    <span class='orange'>
                        <i class='ace-icon fa fa-dollar bigger-120'></i>
                    </span>
                </a>
            ");

            columns.Bound("").Filterable(false).Title("").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).ClientTemplate("#= ConvertToVisit_Databound(StatusName,Guid)#");
            columns.Bound(x => x.StatusName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Status"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.ReserveStartTime).Title(Localizer["ReserveTime"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).ClientTemplate("  #= kendo.toString(kendo.parseDate(ReserveStartTime, 'yyyy-MM-ddTHH:mm:ss'), 'dd/MM/yyyy HH:mm') # ");
            columns.Bound(x => x.Patient.PhoneNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["Mobile"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });

            if (ViewBag.useform ?? false)
            {
                columns.Bound(x => x.Patient.FormNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["FormNumber"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });

            }
            else
            {
                columns.Bound(x => x.Patient.FileNum).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["FormNumber"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });

            }
            columns.Bound(x => x.Patient.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["PatientName"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.StatusId).Hidden();
            columns.Bound(x => x.Index).Filterable(false).Width(20).Title("#").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).Width(40);
            columns.Bound(x => x.Guid).Hidden();
        }
        else
        {
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Index).Filterable(false).Title("#").Width(20).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" }).Width(40);
            columns.Bound(x => x.StatusId).Hidden();
            columns.Bound(x => x.Patient.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["PatientName"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            if (ViewBag.useform ?? false)
            {
                columns.Bound(x => x.Patient.FormNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["FormNumber"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });

            }
            else
            {
                columns.Bound(x => x.Patient.FileNum).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["FormNumber"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });

            }
            columns.Bound(x => x.Patient.PhoneNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("startswith").SuggestionOperator(FilterType.Contains))).Title(Localizer["Mobile"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.ReserveStartTime).Title(Localizer["ReserveTime"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" }).ClientTemplate("  #= kendo.toString(kendo.parseDate(ReserveStartTime, 'yyyy-MM-ddTHH:mm:ss'), 'dd/MM/yyyy HH:mm') # ");
            columns.Bound(x => x.StatusName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Status"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound("").Filterable(false).Title("").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).ClientTemplate("#= ConvertToVisit_Databound(StatusName,Guid)#");

            columns.Bound("").Title(string.Empty).Width(30)
            .ClientTemplate(
                $@"<a class='tooltip-success grid-btn' onClick='PayVisitModal(this);' data-id='#=Guid#' data-rel='tooltip' title='{service_title}' data-original-title='Services'>
                    <span class='orange'>
                        <i class='ace-icon fa fa-dollar bigger-120'></i>
                    </span>
                </a>
            ");


            if ((bool)ViewBag.AccessDeleteTotalReserves)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-error grid-btn' onClick='DeleteReserve(this);' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete'>
                        <span class='red'>
                            <i class='ace-icon fa fa-trash-can bigger-120'></i>
                        </span>
                    </a>
                ");
            }

        }

    })

        .DataSource(dataSource => dataSource
        .Ajax()
        .ServerOperation(false)
        .Sort(x => x.Add("ReserveStartTime").Descending())
        .Model(model =>
        {
            model.Id(p => p.Guid);
        })
        .PageSize(10)
        .Read(read => read.Action("GetAllReserves", "TotalReserves").Data("GetPeriodForAllVisits"))
        )
        .Filterable(ftb => ftb.Mode(GridFilterMode.Row))
        .Pageable(pageable => pageable
        .Input(true)
        .Refresh(true)
        .PageSizes(true)
        .ButtonCount(5)
        .Numeric(true)) // Enable paging
        .Sortable().HtmlAttributes(new { @style = "direction: ltr;margin-top:2rem;overflow:auto" }) // Enable sorting
    );

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n");
#nullable restore
#line 130 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\TotalReserves\dgTotalReservesGrid.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
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
