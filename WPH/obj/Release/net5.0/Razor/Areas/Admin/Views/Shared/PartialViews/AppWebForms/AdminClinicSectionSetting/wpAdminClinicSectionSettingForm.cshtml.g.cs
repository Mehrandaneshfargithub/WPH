#pragma checksum "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b4288f49cefd6680e38bc9f22ffeb487169e1091"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.AdminClinicSectionSetting.Areas_Admin_Views_Shared_PartialViews_AppWebForms_AdminClinicSectionSetting_wpAdminClinicSectionSettingForm), @"mvc.1.0.view", @"/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/wpAdminClinicSectionSettingForm.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.AdminClinicSectionSetting
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
#line 1 "H:\Projects\WAS\WPH\Areas\Admin\Views\_ViewImports.cshtml"
using WPH;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "H:\Projects\WAS\WPH\Areas\Admin\Views\_ViewImports.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b4288f49cefd6680e38bc9f22ffeb487169e1091", @"/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/wpAdminClinicSectionSettingForm.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Areas_Admin_Views_Shared_PartialViews_AppWebForms_AdminClinicSectionSetting_wpAdminClinicSectionSettingForm : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 6 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
  
    string font = "", pull = "", direction = "";
    if (HttpContextAccessor.HttpContext.Session.GetString("culture") == "en")
    {
        font = " MyFont-Roboto-grid ";
        pull = " pull-left ";
        direction = " ";
    }
    else
    {
        font = " MyFont-Sarchia-grid ";
        pull = " pull-right ";
        direction = " direction:rtl; ";
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 22 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"row page-header \">\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 596, "\"", 610, 2);
#nullable restore
#line 25 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue("", 604, pull, 604, 5, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 609, "", 610, 1, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n        <h1");
            BeginWriteAttribute("class", " class=\"", 625, "\"", 638, 1);
#nullable restore
#line 26 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue("", 633, font, 633, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"font-size: 2.3rem\">\r\n            ");
#nullable restore
#line 27 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
       Write(Localizer["ClinicSectionSetting"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </h1>\r\n\r\n    </div>\r\n\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 755, "\"", 774, 2);
#nullable restore
#line 32 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue("", 763, font, 763, 5, false);

#line default
#line hidden
#nullable disable
#nullable restore
#line 32 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue(" ", 768, pull, 769, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"margin:0 2rem;transform:translateY(-10%)\">\r\n        <a class=\"btn btn-danger\" onClick=\"AddAdminClinicSectionSetting(this)\" href=\"#\" style=\"padding:0.2rem\">\r\n            ");
#nullable restore
#line 34 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
       Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            <i class=\"ace-icon glyphicon  glyphicon-plus align-top bigger-125\"></i>\r\n        </a>\r\n    </div>\r\n\r\n</div><!-- /.page-header -->\r\n\r\n\r\n<div");
            BeginWriteAttribute("class", " class=\"", 1122, "\"", 1152, 3);
            WriteAttributeValue("", 1130, "row", 1130, 3, true);
            WriteAttributeValue(" ", 1133, "Grid-Content", 1134, 13, true);
#nullable restore
#line 42 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue(" ", 1146, font, 1147, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    ");
#nullable restore
#line 43 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
Write(await Html.PartialAsync("~/Areas/Admin/Views/Shared/PartialViews/AppWebForms/AdminClinicSectionSetting/dgAdminClinicSectionSettingGrid.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n\r\n<div id=\"AdminClinicSectionSettingModal\"");
            BeginWriteAttribute("class", " class=\"", 1358, "\"", 1382, 3);
            WriteAttributeValue("", 1366, "modal", 1366, 5, true);
            WriteAttributeValue(" ", 1371, "fade", 1372, 5, true);
#nullable restore
#line 47 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue(" ", 1376, font, 1377, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-backdrop=\"static\" data-keyboard=\"false\"");
            BeginWriteAttribute("style", " style=\"", 1428, "\"", 1446, 1);
#nullable restore
#line 47 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue("", 1436, direction, 1436, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" onclick=""closeAdminClinicSectionSettingModal()"" aria-hidden=""true"">&times;</button>
                <h3 id=""AdminClinicSectionSettingModal-header""");
            BeginWriteAttribute("class", " class=\"", 1758, "\"", 1802, 5);
            WriteAttributeValue("", 1766, "smaller", 1766, 7, true);
            WriteAttributeValue(" ", 1773, "lighter", 1774, 8, true);
            WriteAttributeValue(" ", 1781, "blue", 1782, 5, true);
            WriteAttributeValue(" ", 1786, "no-margin", 1787, 10, true);
#nullable restore
#line 52 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
WriteAttributeValue(" ", 1796, font, 1797, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                    ");
#nullable restore
#line 53 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
               Write(Localizer["ClinicSectionSetting"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                </h3>
            </div>
            <div id=""AdminClinicSectionSettingModal-body"" class=""modal-body"">
            </div>

            <div id=""ERROR_Data"" class=""red label-white middle hidden"" style=""height:40px;display:block"">
                <i class=""ace-icon fa fa-stop bigger-120""></i>
                ");
#nullable restore
#line 61 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
           Write(Localizer["ERROR_DataNotValid"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div id=\"ERROR_SomeThingWentWrong\" class=\"red label-white middle hidden\" style=\"height:40px;display:block\">\r\n                <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                ");
#nullable restore
#line 66 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
           Write(Localizer["ERROR_InsertWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"modal-footer\">\r\n                ");
#nullable restore
#line 70 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
            Write(Html.Kendo().Button()
                    .Name("btn-AdminClinicSectionSettingModal-accept")
                    .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "k-primary pull-right" })
                    .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 74 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
            Write(Html.Kendo().Button()
                    .Name("btn-AdminClinicSectionSettingModal-close")
                    .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "pull-right", onclick = "closeAdminClinicSectionSettingModal()" })
                    .Content(Localizer["Exit"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>\r\n\r\n<div id=\"DeleteAdminClinicSectionSettingModal\" class=\"modal fade MyFont-Roboto-grid\" data-backdrop=\"static\" data-keyboard=\"false\"");
            BeginWriteAttribute("style", " style=\"", 3386, "\"", 3394, 0);
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""modal-dialog modal-sm"">
        <div class=""modal-content"">
            <div class=""widget-header"" style=""padding:1rem"">
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-hidden=""true"">&times;</button>
                <h3 class='smaller MyFont-Roboto-grid'>
                    <i class='ace-icon fa fa-exclamation-triangle red '></i>");
#nullable restore
#line 89 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
                                                                       Write(Localizer["DeleteRecordHeader"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </h3>\r\n            </div>\r\n\r\n            <div id=\"DeleteAdminClinicSectionSettingModal-body\" class=\"modal-body\">\r\n                ");
#nullable restore
#line 94 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
           Write(Localizer["DeleteRecordBody"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <div id=\"ERROR_ThisRecordHasDependencyOnItInAnotherEntity\" class=\"red label-white middle hidden\" style=\"height:40px;display:block;margin-top:2rem\">\r\n                    <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                    ");
#nullable restore
#line 97 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
               Write(Localizer["ERROR_ThisRecordHasDependencyOnItInAnotherEntity"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n                <div id=\"ERROR_SomeThingWentWrong\" class=\"red label-white middle hidden\" style=\"height:40px;display:block\">\r\n                    <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                    ");
#nullable restore
#line 101 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
               Write(Localizer["ERROR_SomeThingWentWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n\r\n            <div class=\"modal-footer\">\r\n\r\n                ");
#nullable restore
#line 107 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-DeleteAdminClinicSectionSettingModal-accept")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "k-primary pull-right" })
                .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 111 "H:\Projects\WAS\WPH\Areas\Admin\Views\Shared\PartialViews\AppWebForms\AdminClinicSectionSetting\wpAdminClinicSectionSettingForm.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-DeleteAdminClinicSectionSettingModal-close")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "pull-right", @data_dismiss = "modal" })
                .Content(Localizer["Exit"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>");
            WriteLiteral("    </div>");
            WriteLiteral(@"</div>

<script>

    function AddAdminClinicSectionSetting(e) {
        $(""#AdminClinicSectionSettingModal #ERROR_Data"").addClass(""hidden"");
        $(""#AdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");
        $(""#Mobile-wrong"").addClass('hidden');

        var link = ""/Admin/AdminClinicSectionSetting/AddNewModal"";

        $("".loader"").removeClass(""hidden"");
        $('#AdminClinicSectionSettingModal').modal('toggle');
        $('#AdminClinicSectionSettingModal-body').load(link, function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });
    }

    function EditAdminClinicSectionSetting(element) {
        $(""#AdminClinicSectionSettingModal #ERROR_Data"").addClass(""hidden"");
        $(""#AdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $('#AdminClinicSectionSettingModal').modal('toggle');

        var link = ""/Admin/AdminClinicSectionSetting/EditModal/"";
        var");
            WriteLiteral(@" Id = $(element).attr('data-id');

        $("".loader"").removeClass(""hidden"");
        $('#AdminClinicSectionSettingModal-body').load(link + Id + '', function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });
    }

    function AdminClinicSectionSettingDeleteFunction(element) {
        $(""#DeleteAdminClinicSectionSettingModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity"").addClass(""hidden"");
        $(""#DeleteAdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $("".loader"").removeClass(""hidden"");
        $('#DeleteAdminClinicSectionSettingModal').modal('toggle');
        var Id = $(element).attr('data-id');
        $('#btn-DeleteAdminClinicSectionSettingModal-accept').attr('data-id', Id);
        $("".loader"").fadeIn(""slow"");
        $("".loader"").addClass(""hidden"");
    }

    $('#btn-DeleteAdminClinicSectionSettingModal-accept').on(""click"", function () {
        $(this).attr(""disabled"", true);
");
            WriteLiteral(@"
        $(""#DeleteAdminClinicSectionSettingModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity"").addClass(""hidden"");
        $(""#DeleteAdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        var Id = $(this).attr('data-id');
        var link = ""/Admin/AdminClinicSectionSetting/Remove"";

        var token = $(':input:hidden[name*=""RequestVerificationToken""]');
        $("".loader"").removeClass(""hidden"");
        $.ajax({
            type: ""Post"",
            url: link,
            data: {
                __RequestVerificationToken: token.attr('value'),
                Id: Id
            },
            success: function (response) {
                $('#btn-DeleteAdminClinicSectionSettingModal-accept').removeAttr(""disabled"");

                if (response === ""SUCCESSFUL"") {
                    $('#DeleteAdminClinicSectionSettingModal').modal('hide');
                    $("".modal-backdrop:last"").remove();

                    $("".loader"").fadeIn(""slow"")");
            WriteLiteral(@";
                    $("".loader"").addClass(""hidden"");
                    $(""#adminClinicSectionSettingKendoGrid .k-pager-refresh"")[0].click();
                }
                else if (response === ""ERROR_ThisRecordHasDependencyOnItInAnotherEntity"") {
                    $(""#DeleteAdminClinicSectionSettingModal #ERROR_ThisRecordHasDependencyOnItInAnotherEntity"").removeClass(""hidden"");
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
                else if (response === ""ERROR_SomeThingWentWrong"") {
                    $(""#DeleteAdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").removeClass(""hidden"");
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
                else if (response === ""AreYouSure"") {
                    AskForDelete(Id);
                }
                else if (response === ""CanNotDelete"") {
                    CanNotDel");
            WriteLiteral(@"ete();
                } else {
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            }
        });
    });

    $('#btn-AdminClinicSectionSettingModal-accept').on(""click"", function () {
        $(this).attr(""disabled"", true);

        $(""#AdminClinicSectionSettingModal #ERROR_Data"").addClass(""hidden"");
        $(""#AdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $('.emptybox').addClass('hidden');
        var isEmmpty = true;
        $('.emptybox').each(function () {
            if ($(this).attr('data-isEssential') === 'true') {
                var empty = $(this).attr('id');
                if ($('[data-checkEmpty=""' + empty + '""]').val() !== undefined) {
                    let text = $('[data-checkEmpty=""' + empty + '""]').val().replace(/ /g, '');
                    if (text === """") {
                        $(this).removeClass('hidden');
                        $('#b");
            WriteLiteral(@"tn-AdminClinicSectionSettingModal-accept').removeAttr(""disabled"");
                        isEmmpty = false;
                        return;
                    }
                }
            }
        });

        if (isEmmpty === false) {
            return;
        }

        var data = $(""#addNewAdminClinicSectionSettingForm"").serialize();
        var link = ""/Admin/AdminClinicSectionSetting/AddOrUpdate"";

        $("".loader"").removeClass(""hidden"");
        $.ajax({
            type: ""Post"",
            url: link,
            data: data,
            success: function (response) {
                $('#btn-AdminClinicSectionSettingModal-accept').removeAttr(""disabled"");

                if (response !== 0) {
                    if (response === ""ValueIsRepeated"") {

                        $('#Name-Exist').removeClass('hidden');

                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");
                    } else if (response =");
            WriteLiteral(@"== ""DataNotValid"") {

                        $(""#AdminClinicSectionSettingModal #ERROR_Data"").removeClass(""hidden"");
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                    } else {

                        $('#AdminClinicSectionSettingModal').modal('hide');
                        $("".modal-backdrop:last"").remove();
                        $('#AdminClinicSectionSettingModal-body').empty();
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");
                        $(""#adminClinicSectionSettingKendoGrid .k-pager-refresh"")[0].click();

                    }
                } else {

                    $(""#AdminClinicSectionSettingModal #ERROR_SomeThingWentWrong"").removeClass(""hidden"");
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            }
        });
    });

    function c");
            WriteLiteral("loseAdminClinicSectionSettingModal() {\r\n        $(\'#AdminClinicSectionSettingModal\').modal(\'toggle\');\r\n        $(\'#AdminClinicSectionSettingModal-body\').empty();\r\n    }\r\n</script>");
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
