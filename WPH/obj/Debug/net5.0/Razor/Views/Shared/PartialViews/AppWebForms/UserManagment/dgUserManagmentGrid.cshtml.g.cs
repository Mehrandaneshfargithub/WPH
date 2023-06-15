#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "92d39682e17e4425ed01b5d96652bf51c58dfc81"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.UserManagment.Views_Shared_PartialViews_AppWebForms_UserManagment_dgUserManagmentGrid), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/UserManagment/dgUserManagmentGrid.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.UserManagment
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
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"92d39682e17e4425ed01b5d96652bf51c58dfc81", @"/Views/Shared/PartialViews/AppWebForms/UserManagment/dgUserManagmentGrid.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_UserManagment_dgUserManagmentGrid : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<bool>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 6 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml"
  

    string edit_title = Localizer["EditUser"];
    string remove_title = Localizer["RemoveUser"];
    string active_title = Localizer["ActiveUser"];
    string access_title = Localizer["UserAccess"];
    string human_title = Localizer["ShowHumanResource"];

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n    function FilterUserManagementGrid() {\r\n        return {\r\n            humanId: $(\"#filter_user_grid\").attr(\"data-value\")\r\n        };\r\n    }\r\n</script>\r\n\r\n<div>\r\n    ");
#nullable restore
#line 24 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml"
Write(Html.Kendo().Grid<WPH.Models.CustomDataModels.UserManagment.UserInformationViewModel>()
    .Name("kendoGrid")
    .Columns(columns =>
    {
        if (HttpContextAccessor.HttpContext.Session.GetString("culture") != "en")
        {
            if ((bool)ViewBag.AccessDeleteUser)
            {
                columns.Bound("").Title(" ").Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-error grid-btn' data-rel='tooltip' onClick='GridDeleteFunction(this);' data-id='#=Guid#' title='{remove_title}' data-original-title='Delete' data-toggle='modal'>
                        <span class='red'>
                            <i class='ace-icon fa fa-trash-can bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if ((bool)ViewBag.AccessEditUserAccess)
            {
                columns.Bound("").Title(" ").Width(30)
                .ClientTemplate(
                   $@"<a class='bs-tooltip-bottom grid-btn' data-rel='tooltip' onClick='GridUserAccessFunction(this);' data-name='#=UserName#'  data-id='#=Guid#' title='{access_title}' data-original-title='Access'>
                        <span class='blue'>
                            <i class='ace-icon fa fa-lock bigger-120'></i>
                        </span>
                    </a>
                ");

                columns.Bound("").Title(" ").Width(10)
                .ClientTemplate(
                   $"<a class='bs-tooltip-bottom grid-btn' data-rel='tooltip' onClick='ActiveDeactiveUserFunction(this);' data-id='#=Guid#' title='{active_title}' data-original-title='Active'> "+
                        @"<span class='#if(Active){#blue#}else{#red#}#'>
                            <i class='ace-icon fa fa-eye bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if ((bool)ViewBag.AccessEditUser)
            {
                columns.Bound("").Title(" ").Width(10)
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn edit' data-rel='tooltip' onClick='EditUser(this);' data-id='#=Guid#' title='{edit_title}' data-original-title='Edit' >
                        <span class='green'>
                            <i class='ace-icon fa fa-pencil bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if (Model){
                columns.Bound("").Title("").Width(10).HtmlAttributes(new { @style = "text-align:center;" })
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn' onClick='SelectHumanInUserManager(this);' data-id='#=Guid#' data-rel='tooltip' title='{human_title}' data-original-title='ShowHumanResource'>
                        <span class='blue'>
                            <i class='ace-icon fa fa-chevron-circle-up bigger-120' aria-hidden='true'></i>
                        </span>
                    </a>
                ");
            }

            columns.Bound(x => x.UserTypeName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["UserType"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.PhoneNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Mobile"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Id).Filterable(false).Title(Localizer["UserCode"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.UserName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Username"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Name"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.Index).Filterable(false).Width(10).Title("#").HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" }).Width(40);
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Active).Hidden();
        }
        else
        {
            columns.Bound(x => x.Guid).Hidden();
            columns.Bound(x => x.Active).Hidden();
            columns.Bound(x => x.Index).Filterable(false).Width(20).Title("#").HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" }).Width(40);
            columns.Bound(x => x.Name).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Name"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.UserName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Username"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.Id).Filterable(false).Title(Localizer["UserCode"]).HtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Roboto-grid" }).HeaderHtmlAttributes(new { @style = "text-align:center;", @class = "MyFont-Sarchia-grid" });
            columns.Bound(x => x.PhoneNumber).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["Mobile"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            columns.Bound(x => x.UserTypeName).Filterable(ftb => ftb.Cell(cell => cell.Operator("contains").SuggestionOperator(FilterType.Contains))).Title(Localizer["UserType"]).HtmlAttributes(new { @style = "text-align:center;" }).HeaderHtmlAttributes(new { @style = "text-align:center;" });
            if(Model){
                columns.Bound("").Title("").Width(10).HtmlAttributes(new { @style = "text-align:center;" })
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn' onClick='SelectHumanInUserManager(this);' data-id='#=Guid#' data-rel='tooltip' title='{human_title}' data-original-title='ShowHumanResource'>
                        <span class='blue'>
                            <i class='ace-icon fa fa-chevron-circle-up bigger-120' aria-hidden='true'></i>
                        </span>
                    </a>
                ");
            }

            if((bool)ViewBag.AccessEditUser)
            {
                columns.Bound("").Title(" ").Width(10)
                .ClientTemplate(
                   $@"<a class='tooltip-success grid-btn edit' data-rel='tooltip' onClick='EditUser(this);' data-id='#=Guid#' title='{edit_title}' data-original-title='Edit' >
                        <span class='green'>
                            <i class='ace-icon fa fa-pencil bigger-120'></i>
                        </span>
                    </a>
                ");
            }

            if ((bool)ViewBag.AccessEditUserAccess)
            {
                columns.Bound("").Title(" ").Width(10)
                .ClientTemplate(
                   $"<a class='bs-tooltip-bottom grid-btn' data-rel='tooltip' onClick='ActiveDeactiveUserFunction(this);' data-id='#=Guid#' title='{active_title}' data-original-title='Active'> "+
                        @"<span class='#if(Active){#blue#}else{#red#}#'>
                            <i class='ace-icon fa fa-eye bigger-120'></i>
                        </span>
                    </a>
                ");

                columns.Bound("").Title(" ").Width(10)
                .ClientTemplate(
                   $@"<a class='bs-tooltip-bottom grid-btn' data-rel='tooltip' onClick='GridUserAccessFunction(this);' data-name='#=UserName#'   data-id='#=Guid#' title='{access_title}' data-original-title='Access'>
                        <span class='blue'>
                            <i class='ace-icon fa fa-lock bigger-120'></i>
                        </span>
                    </a>
                ");
            }


            if ((bool)ViewBag.AccessDeleteUser)
            {
                columns.Bound("").Title(" ").Width(30)
                .ClientTemplate(
                   $@"<a class='tooltip-error grid-btn' data-rel='tooltip' onClick='GridDeleteFunction(this);' data-id='#=Guid#' title='{remove_title}' data-original-title='Delete' data-toggle='modal'>
                        <span class='red'>
                            <i class='ace-icon fa fa-trash-can bigger-120'></i>
                        </span>
                    </a>
                ");
            }

        }
    })
    .DataSource(dataSource => dataSource // Configure the Grid data source.
    .Ajax()
    //.ServerOperation(false)
    .Model(model =>
    {
        model.Id(p => p.Guid);
    })// Specify that Ajax binding is used.
    .PageSize(10)
    .Read(read => read.Action("GetAll", "UserManagment").Data("FilterUserManagementGrid")) // Set the action method which will return the data in JSON format.
    )
    .Filterable(ftb => ftb.Mode(GridFilterMode.Row))
    .Selectable(selectable => selectable.Mode(GridSelectionMode.Single))
    .Pageable(pageable => pageable
    .Input(true)
    .Refresh(true)
    .Numeric(true)) // Enable paging
    .Sortable().HtmlAttributes(new { @style = "direction: ltr;margin-top:2rem;overflow:auto;height:auto" }) // Enable sorting
    );

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n\r\n<h3 id=\"editUserText\" class=\"hidden\">");
#nullable restore
#line 185 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml"
                                Write(Localizer["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 185 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\UserManagment\dgUserManagmentGrid.cshtml"
                                                   Write(Localizer["User"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h3>
<input class=""hidden"" type=""text"" id=""filter_user_grid"" data-value="""" />
<script>


    $(document).ready(function () {

        $(""#kendoGrid"").on(""dblclick"", ""tr.k-state-selected"", function (element) {

            $(""#kendoGrid"").find(""tr.k-state-selected td a.edit"").click();

        });
    })

    function EditUser(element) {
        $('#my-modal-edit').modal('toggle');

        let header = $(""#editUserText"").text();

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<bool> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
