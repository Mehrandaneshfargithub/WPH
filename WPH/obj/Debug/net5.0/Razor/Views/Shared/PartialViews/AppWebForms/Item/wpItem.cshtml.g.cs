#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6af2c2c95455fd6eeeac3c4379f0f39776123675"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Item.Views_Shared_PartialViews_AppWebForms_Item_wpItem), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Item/wpItem.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Item
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
#line 1 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6af2c2c95455fd6eeeac3c4379f0f39776123675", @"/Views/Shared/PartialViews/AppWebForms/Item/wpItem.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Item_wpItem : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
  
    string font = "", pull = "", dir = "";
    if (HttpContextAccessor.HttpContext.Session.GetString("culture") == "en")
    {
        font = " MyFont-Roboto-grid ";
        pull = " pull-left ";
        dir = " ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newItemText\" class=\"hidden\">");
#nullable restore
#line 12 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                       Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 12 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                                         Write(Localizer["Items"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
    }
    else
    {
        font = " MyFont-Sarchia-grid ";
        pull = " pull-right ";
        dir = " direction:rtl; ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newItemText\" class=\"hidden\">");
#nullable restore
#line 19 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                       Write(Localizer["Items"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 19 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                                           Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 20 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("    <h3 id=\"editItemText\" class=\"hidden\">");
#nullable restore
#line 21 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                    Write(Localizer["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 21 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
                                                       Write(Localizer["Items"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
            WriteLiteral("\r\n<div class=\"row page-header \">\r\n\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 812, "\"", 825, 1);
#nullable restore
#line 26 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue("", 820, pull, 820, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <h1");
            BeginWriteAttribute("class", " class=\"", 840, "\"", 853, 1);
#nullable restore
#line 27 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue("", 848, font, 848, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"font-size: 2.3rem\">\r\n            ");
#nullable restore
#line 28 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
       Write(Localizer["Items"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </h1>\r\n\r\n    </div>\r\n");
#nullable restore
#line 32 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
     if ((bool)ViewBag.AccessNewItem)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div");
            BeginWriteAttribute("class", " class=\"", 1003, "\"", 1022, 2);
#nullable restore
#line 34 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue("", 1011, font, 1011, 5, false);

#line default
#line hidden
#nullable disable
#nullable restore
#line 34 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue(" ", 1016, pull, 1017, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"margin:0 2rem;transform:translateY(-10%)\">\r\n            <a class=\"btn btn-danger\" onClick=\"AddItem(this)\" href=\"#\" style=\"padding:0.2rem\">\r\n                ");
#nullable restore
#line 36 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
           Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <i class=\"ace-icon glyphicon  glyphicon-plus align-top bigger-125\"></i>\r\n            </a>\r\n        </div>\r\n");
#nullable restore
#line 40 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div><!-- /.page-header -->\r\n\r\n\r\n<div id=\"AddItemModal\"");
            BeginWriteAttribute("class", " class=\"", 1394, "\"", 1418, 3);
            WriteAttributeValue("", 1402, "modal", 1402, 5, true);
            WriteAttributeValue(" ", 1407, "fade", 1408, 5, true);
#nullable restore
#line 45 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue(" ", 1412, font, 1413, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-backdrop=\"static\" data-keyboard=\"false\"");
            BeginWriteAttribute("style", " style=\"", 1464, "\"", 1476, 1);
#nullable restore
#line 45 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue("", 1472, dir, 1472, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" onclick=""closeAddItemModal()"" aria-hidden=""true"">&times;</button>
                <h3 id=""AddItemModal-header""");
            BeginWriteAttribute("class", " class=\"", 1752, "\"", 1796, 5);
            WriteAttributeValue("", 1760, "smaller", 1760, 7, true);
            WriteAttributeValue(" ", 1767, "lighter", 1768, 8, true);
            WriteAttributeValue(" ", 1775, "blue", 1776, 5, true);
            WriteAttributeValue(" ", 1780, "no-margin", 1781, 10, true);
#nullable restore
#line 50 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue(" ", 1790, font, 1791, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                </h3>
            </div>

            <div id=""AddItemModal-body"" class=""modal-body"" style=""width:60rem"">
            </div>
            <div id=""ERROR_Data"" class=""red label-white middle hidden"" style=""height:40px;display:block"">
                <i class=""ace-icon fa fa-stop bigger-120""></i>
                ");
#nullable restore
#line 58 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
           Write(Localizer["ERROR_DataNotValid"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div id=\"ERROR_SomeThingWentWrong\" class=\"red label-white middle hidden\" style=\"height:40px;display:block\">\r\n                <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                ");
#nullable restore
#line 63 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
           Write(Localizer["ERROR_InsertWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"modal-footer\">\r\n\r\n                ");
#nullable restore
#line 68 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-addItem-accept")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "k-primary pull-right", onclick = "addNewItem()" })
                .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 72 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-addItem-close")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "pull-right", onclick="closeAddItemModal()" })
                .Content(Localizer["Exit"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>");
            WriteLiteral("    </div>");
            WriteLiteral("</div>\r\n\r\n\r\n<!-- Grid -->\r\n<div");
            BeginWriteAttribute("class", " class=\"", 3193, "\"", 3223, 3);
            WriteAttributeValue("", 3201, "row", 3201, 3, true);
            WriteAttributeValue(" ", 3204, "Grid-Content", 3205, 13, true);
#nullable restore
#line 83 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
WriteAttributeValue(" ", 3217, font, 3218, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    ");
#nullable restore
#line 84 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/AppWebForms/Item/dgItemGrid.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n");
#nullable restore
#line 87 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/InterfacePartials/_GridAndModalLinks.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 88 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\wpItem.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/InterfacePartials/_Modal.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"


<script>

    function AddItem(element) {
        $(""#AddItemModal #ERROR_Data"").addClass(""hidden"");
        $(""#AddItemModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        var link = ""/Item/AddAndNewModal"";
        $("".loader"").removeClass(""hidden"");

        let header = $(""#newItemText"").text();

        $('#AddItemModal-header').text(header);
        $('#AddItemModal').modal('toggle');
        $('#AddItemModal-body').load(link, function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });

    }

    function EditItem(element) {
        $(""#AddItemModal #ERROR_Data"").addClass(""hidden"");
        $(""#AddItemModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $('#AddItemModal').modal('toggle');
        let header = $(""#editItemText"").text();
        $('#AddItemModal-header').text(header);
        var link = ""/Item/EditModal?Id="";
        var Id = $(element).attr('data-id');
        $("".loader"").removeCla");
            WriteLiteral(@"ss(""hidden"");
        $('#AddItemModal-body').load(link + Id + '', function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");

        });

    }


    function addNewItem() {
        $(""#AddItemModal #ERROR_Data"").addClass(""hidden"");
        $(""#AddItemModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $('.emptybox').addClass('hidden');
        var isEmmpty = true;
        $('.emptybox').each(function () {
            if ($(this).attr('data-isEssential') === 'true') {
                var empty = $(this).attr('id');
                if ($('[data-checkEmpty=""' + empty + '""]').val() === """") {
                    $(this).removeClass('hidden');
                    isEmmpty = false;
                    return;
                }
            }
        });

        if (isEmmpty === false) {
            return;
        }


        var link = ""/Item/AddOrUpdate"";

        var data = $(""#addNewItemForm"").serialize();


        $("".l");
            WriteLiteral(@"oader"").removeClass(""hidden"");
        $.ajax({
            type: ""Post"",
            url: link,
            data: data,
            success: function (response) {
                if (response !== 0) {
                    if (response === ""ValueIsRepeated"") {

                        $('#Name-Exist').removeClass('hidden');

                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                    } else if (response === ""DataNotValid"") {

                        $(""#AddItemModal #ERROR_Data"").removeClass(""hidden"");
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                    } else {

                        $('#AddItemModal').modal('hide');
                        $("".modal-backdrop:last"").remove();
                        $('#AddItemModal-body').empty();
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidd");
            WriteLiteral(@"en"");
                        $("".k-pager-refresh"")[0].click();

                    }
                } else {

                    $(""#AddItemModal #ERROR_SomeThingWentWrong"").removeClass(""hidden"");
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            }
        });
    }

    function closeAddItemModal() {

        $('#AddItemModal').modal('hide');
        $('#AddItemModal-body').empty();
        $("".modal-backdrop:last"").remove();

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
