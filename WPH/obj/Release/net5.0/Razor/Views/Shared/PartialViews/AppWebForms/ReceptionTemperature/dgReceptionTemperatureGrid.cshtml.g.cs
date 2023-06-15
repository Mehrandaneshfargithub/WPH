#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0909f4a4b408bd2c5242ea86420b7930b6467e56"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.ReceptionTemperature.Views_Shared_PartialViews_AppWebForms_ReceptionTemperature_dgReceptionTemperatureGrid), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/ReceptionTemperature/dgReceptionTemperatureGrid.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.ReceptionTemperature
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
#line 3 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0909f4a4b408bd2c5242ea86420b7930b6467e56", @"/Views/Shared/PartialViews/AppWebForms/ReceptionTemperature/dgReceptionTemperatureGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_ReceptionTemperature_dgReceptionTemperatureGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WPH.Models.BaseInfo.SectionViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<div>\r\n    ");
#nullable restore
#line 8 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
Write(Html.Kendo().Grid<WPH.Models.ReceptionTemperature.ReceptionTemperatureViewModel>()
    .Name("kendoReceptionTemperatureGrid")
    .HtmlAttributes(new { Class = "k-grid-header" })
    .Columns(columns =>
    {

        if (HttpContextAccessor.HttpContext.Session.GetString("culture") != "en")
        {
            columns.Bound("").Title(" ").Width(30)
            .ClientTemplate(
            @"<a class='tooltip-error grid-btn' onClick='DeleteReceptionTemperature(this);' data-id='#=Guid#' data-type='Temp' data-rel='tooltip' title='Delete' data-original-title='Delete'>
                <span class='red'>
                    <i class='ace-icon fa fa-trash-can bigger-120'></i>
                </span>
            </a>
            ");



            columns.Bound(x => x.RespirationRate).Filterable(false).Title("Respiration Rate").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.PulseRate).Filterable(false).Title("Pulse Rate").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.SYSBloodPressure).Filterable(false).Title("SYSBloodPressure").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.DIABloodPressure).Filterable(false).Title("DIABloodPressure").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Temperature).Filterable(false).Title(Localizer["Temperature"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.InsertedDate).Format("{0: dd/MM/yyyy HH:mm}").Title(Localizer["Date"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Index).Filterable(false).Width(5).Title("#").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).Width(30);
            columns.Bound(x => x.Guid).Hidden();
        }
        else
        {
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Index).Filterable(false).Width(5).Title("#").HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" }).Width(30);
            columns.Bound(x => x.InsertedDate).Format("{0: dd/MM/yyyy HH:mm}").Title(Localizer["Date"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.Temperature).Filterable(false).Title(Localizer["Temperature"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.DIABloodPressure).Filterable(false).Title("DIABloodPressure").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.SYSBloodPressure).Filterable(false).Title("SYSBloodPressure").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.PulseRate).Filterable(false).Title("Pulse Rate").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.RespirationRate).Filterable(false).Title("Respiration Rate").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });


            columns.Bound("").Title(" ").Width(30)
            .ClientTemplate(
            @"<a class='tooltip-error grid-btn' onClick='DeleteReceptionTemperature(this);' data-id='#=Guid#' data-type='Temp' data-rel='tooltip' title='Delete' data-original-title='Delete'>
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
        .PageSize(10)
        .Read(read => read.Action("GetAllReceptionTemperature", "Reception", new { ReceptionId = Model.Id }))
        )
        .Filterable(ftb => ftb.Mode(GridFilterMode.Row))
        .Selectable(selectable => selectable.Mode(GridSelectionMode.Single))
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
            WriteLiteral(@"
</div>

<script>



    $(document).ready(function () {

        $(""#kendoPatientGrid"").on(""dblclick"", ""tr.k-state-selected"", function (element) {

            $(""#kendoPatientGrid"").find(""tr.k-state-selected td a.edit"").click();

        });

    })




    function PatinetAge(birthDate) {

        if (birthDate === null)
            return """";
        var today = new Date();
        //var birthDate = new Date(dateString);
        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
            m = 12 + m;
        }

        //var Age =   m + '  ");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
                        Write(ViewBag.YearTranslated);

#line default
#line hidden
#nullable disable
            WriteLiteral("  \' + \'  ");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
                                                        Write(ViewBag.MonthTranslated);

#line default
#line hidden
#nullable disable
            WriteLiteral("\' + age;\r\n        //var Age =   \'  ");
#nullable restore
#line 113 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
                    Write(ViewBag.MonthTranslated);

#line default
#line hidden
#nullable disable
            WriteLiteral(" \' + m + \'  ");
#nullable restore
#line 113 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\ReceptionTemperature\dgReceptionTemperatureGrid.cshtml"
                                                        Write(ViewBag.YearTranslated);

#line default
#line hidden
#nullable disable
            WriteLiteral(" \' + age;\r\n        var Age =  age +  \'  year \' + m + \' month \';\r\n\r\n        return Age;\r\n\r\n    }\r\n\r\n\r\n\r\n</script>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WPH.Models.BaseInfo.SectionViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
