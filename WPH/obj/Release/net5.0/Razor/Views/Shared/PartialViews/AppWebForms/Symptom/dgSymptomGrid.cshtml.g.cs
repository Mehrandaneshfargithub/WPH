#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "db50cb385aa774c44c5e437d5ad72375c82f5e29"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Symptom.Views_Shared_PartialViews_AppWebForms_Symptom_dgSymptomGrid), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Symptom/dgSymptomGrid.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Symptom
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
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"db50cb385aa774c44c5e437d5ad72375c82f5e29", @"/Views/Shared/PartialViews/AppWebForms/Symptom/dgSymptomGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Symptom_dgSymptomGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 6 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml"
  

    string edit_title = Localizer["EditSymptom"];
    string remove_title = Localizer["RemoveSymptom"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div>\r\n    ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml"
Write(Html.Kendo().Grid<WPH.Models.CustomDataModels.Symptom.SymptomViewModel>()
    .Name("kendoSymptomGrid")
    .HtmlAttributes(new { ID = "idGridSymptomList", Class = "k-grid-header" })
    .Columns(columns =>
    {

        if (HttpContextAccessor.HttpContext.Session.GetString("culture") != "en")
        {
            if ((bool)ViewBag.AccessDeleteSymptom)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-error grid-btn' onClick='GridDeleteFunction(this);' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete'>
                        <span class='red'>
                            <i class='ace-icon fa fa-trash-can bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if ((bool)ViewBag.AccessEditSymptom)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn edit' onClick='EditSymptom(this);' data-id='#=Guid#' data-rel='tooltip' title='{edit_title}' data-original-title='Edit'>
                        <span class='green'>
                            <i class='ace-icon fa fa-pencil bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            columns.Bound(x => x.Name).Width(1000).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["SymptomName"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Index).Filterable(false).Width(5).Title("#").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).Width(30);
            columns.Bound(x => x.ClinicSectionId).Hidden();
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Id).Hidden();
        }
        else
        {
            columns.Bound(x => x.Id).Hidden();
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.ClinicSectionId).Hidden();
            columns.Bound(x => x.Index).Filterable(false).Width(30).Title("#").HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" }).Width(30);
            columns.Bound(x => x.Name).Width(1000).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["SymptomName"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });

            if ((bool)ViewBag.AccessEditSymptom)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn edit' onClick='EditSymptom(this);' data-id='#=Guid#' data-rel='tooltip' title='{edit_title}' data-original-title='Edit'>
                        <span class='green'>
                            <i class='ace-icon fa fa-pencil bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if ((bool)ViewBag.AccessDeleteSymptom)
            {
                columns.Bound("").Title(string.Empty).Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-error grid-btn' onClick='GridDeleteFunction(this);' data-id='#=Guid#' data-rel='tooltip' title='{remove_title}' data-original-title='Delete'>
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
        .Model(model =>
        {
            model.Id(p => p.Guid);
        })
        .PageSize(10)
        .Read(read => read.Action("GetAll", "Symptom"))
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
            WriteLiteral("\r\n</div>\r\n\r\n");
            WriteLiteral("    <h3 id=\"editSymptomText\" class=\"hidden\">");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml"
                                       Write(Localizer["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Symptom\dgSymptomGrid.cshtml"
                                                          Write(Localizer["Symptom"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
            WriteLiteral(@"

<script>

    $(document).ready(function () {

        $(""#kendoSymptomGrid"").on(""dblclick"", ""tr.k-state-selected"", function (element) {

            $(""#kendoSymptomGrid"").find(""tr.k-state-selected td a.edit"").click();

        });

    })


    function EditSymptom(element) {


        $('#my-modal-edit').modal('toggle');
        let header = $(""#editSymptomText"").text();
        $('#edit-modal-header').text(header);
        var link = $(""#GridEditLink"").attr(""data-Value"");
        var Id = $(element).attr('data-id');
        $("".loader"").removeClass(""hidden"");
        $('#edit-modal-body').load(link + Id + '', function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
