#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "dd35bf119b9bbf7eb0c6aa6110c691db88db17dc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Disease.Views_Shared_PartialViews_AppWebForms_Disease_wpDisease), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Disease/wpDisease.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Disease
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
#line 1 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dd35bf119b9bbf7eb0c6aa6110c691db88db17dc", @"/Views/Shared/PartialViews/AppWebForms/Disease/wpDisease.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Disease_wpDisease : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
  
    string font = "", pull = "", direction = "";
    if (HttpContextAccessor.HttpContext.Session.GetString("culture") == "en")
    {
        font = " MyFont-Roboto-grid ";
        pull = " pull-left ";
        direction = " ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newDiseaseText\" class=\"hidden\">");
#nullable restore
#line 12 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                          Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 12 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                                            Write(Localizer["Disease"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n        <h3 id=\"editDiseaseText\" class=\"hidden\">");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                           Write(Localizer["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                                              Write(Localizer["Disease"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 14 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
    }
    else
    {
        font = " MyFont-Sarchia-grid ";
        pull = " pull-right ";
        direction = " direction:rtl; ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newDiseaseText\" class=\"hidden\">");
#nullable restore
#line 20 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                          Write(Localizer["Disease"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 20 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                                                Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n        <h3 id=\"editDiseaseText\" class=\"hidden\">");
#nullable restore
#line 21 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                           Write(Localizer["Edit"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 21 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
                                                              Write(Localizer["Disease"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 22 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
    }


#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<div class=\"row page-header \">\r\n\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 948, "\"", 961, 1);
#nullable restore
#line 29 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue("", 956, pull, 956, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <h1");
            BeginWriteAttribute("class", " class=\"", 976, "\"", 989, 1);
#nullable restore
#line 30 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue("", 984, font, 984, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"font-size: 2.3rem\">\r\n            ");
#nullable restore
#line 31 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
       Write(Localizer["Diseases"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </h1>\r\n\r\n    </div>\r\n");
#nullable restore
#line 35 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
     if ((bool)ViewBag.AccessNewDisease)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div");
            BeginWriteAttribute("class", " class=\"", 1145, "\"", 1164, 2);
#nullable restore
#line 37 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue("", 1153, font, 1153, 5, false);

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue(" ", 1158, pull, 1159, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"margin:0 2rem;transform:translateY(-10%)\">\r\n            <a class=\"btn btn-danger\" onClick=\"AddDisease(this)\" href=\"#\" style=\"padding:0.2rem\">\r\n                ");
#nullable restore
#line 39 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
           Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                <i class=\"ace-icon glyphicon  glyphicon-plus align-top bigger-125\"></i>\r\n            </a>\r\n        </div>\r\n");
#nullable restore
#line 43 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n</div><!-- /.page-header -->\r\n\r\n<br />\r\n\r\n<div");
            BeginWriteAttribute("class", " class=\"", 1531, "\"", 1561, 3);
            WriteAttributeValue("", 1539, "row", 1539, 3, true);
            WriteAttributeValue(" ", 1542, "Grid-Content", 1543, 13, true);
#nullable restore
#line 50 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue(" ", 1555, font, 1556, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    ");
#nullable restore
#line 51 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/AppWebForms/Disease/dgDiseaseGrid.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n");
#nullable restore
#line 54 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/InterfacePartials/_GridAndModalLinks.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 55 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
Write(await Html.PartialAsync("~/Views/Shared/PartialViews/InterfacePartials/_Modal.cshtml"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n<div id=\"AddDiseaseModal\"");
            BeginWriteAttribute("class", " class=\"", 1898, "\"", 1922, 3);
            WriteAttributeValue("", 1906, "modal", 1906, 5, true);
            WriteAttributeValue(" ", 1911, "fade", 1912, 5, true);
#nullable restore
#line 59 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue(" ", 1916, font, 1917, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-backdrop=\"static\" data-keyboard=\"false\"");
            BeginWriteAttribute("style", " style=\"", 1968, "\"", 1986, 1);
#nullable restore
#line 59 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
WriteAttributeValue("", 1976, direction, 1976, 10, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" onclick=""closeAddDiseaseModal()"" aria-hidden=""true"">&times;</button>
                <h3 id=""AddDiseaseModal-header"" class=""smaller lighter blue no-margin MyFont-Roboto-grid""></h3>
            </div>

            <div id=""AddDiseaseModal-body"" class=""modal-body"" style=""width:60rem"">

            </div>

            <div id=""ERROR_SomeThingWentWrong"" class=""red label-white middle hidden"" style=""height:40px;display:block"">
                <i class=""ace-icon fa fa-stop bigger-120""></i>
                ");
#nullable restore
#line 73 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
           Write(Localizer["ERROR_InsertWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div id=\"ERROR_Data\" class=\"red label-white middle hidden\" style=\"height:40px;display:block\">\r\n                <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                ");
#nullable restore
#line 78 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
           Write(Localizer["ERROR_DataNotValid"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"modal-footer\">\r\n\r\n                ");
#nullable restore
#line 83 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-addDisease-accept")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "k-primary pull-right", onclick = "addNewDisease()" })
                .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 87 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Disease\wpDisease.cshtml"
            Write(Html.Kendo().Button()
                    .Name("btn-addDisease-close")
                    .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "pull-right", onclick="closeAddDiseaseModal()" })
                    .Content(Localizer["Exit"]));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

            </div>
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

<script>

    function AddDisease(element) {

        $('#AddDiseaseModal #ERROR_SomeThingWentWrong').addClass('hidden');
        $('#AddDiseaseModal #ERROR_Data').addClass('hidden');
        $('#Name-Exist').addClass('hidden');

        var link = ""/Disease/AddNewModal"";
        $("".loader"").removeClass(""hidden"");

        let header = $(""#newDiseaseText"").text();

        $('#AddDiseaseModal-header').text(header);
        $('#AddDiseaseModal').modal('toggle');
        $('#AddDiseaseModal-body').load(link, function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });

    }

    function EditDisease(element) {

        $('#AddDiseaseModal #ERROR_SomeThingWentWrong').addClass('hidden');
        $('#AddDiseaseModal #ERROR_Data').addClass('hidden');
        $('#Name-Exist').addClass('hidden');

        $('#AddDiseaseModal').mod");
            WriteLiteral(@"al('toggle');
        let header = $(""#editDiseaseText"").text();
        $('#AddDiseaseModal-header').text(header);
        var link = $(""#GridEditLink"").attr(""data-Value"");
        var Id = $(element).attr('data-id');
        $("".loader"").removeClass(""hidden"");
        $('#AddDiseaseModal-body').load(link + Id + '', function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
            kendo.ui.progress($(""#window""), true);
        });

    }

    function addNewDisease() {

        $('#AddDiseaseModal #ERROR_SomeThingWentWrong').addClass('hidden');
        $('#AddDiseaseModal #ERROR_Data').addClass('hidden');
        $('#Name-Exist').addClass('hidden');

        $('.emptybox').addClass('hidden');
        var isEmmpty = true;
        $('.emptybox').each(function () {
            if ($(this).attr('data-isEssential') === 'true') {
                var empty = $(this).attr('id');
                if ($('[data-checkEmpty=""' + empty + '""]').val() ===");
            WriteLiteral(@" """") {
                    $(this).removeClass('hidden');
                    isEmmpty = false;
                    return;
                }
            }
        });

        if (isEmmpty === false) {
            return;
        }

        var Medicines = [];
        var medicinesItemList = $('#allMedsForDisease').data('kendoListBox').dataItems();
        for (var i = 0; i < medicinesItemList.length; i++) {
            Medicines.push(medicinesItemList[i][""Guid""]);
        }


        var Symptoms = [];
        var symptomsItemList = $('#allSymptomsForDisease').data('kendoListBox').dataItems();
        for (var i = 0; i < symptomsItemList.length; i++) {
            Symptoms.push(symptomsItemList[i][""Guid""]);
        }

        var link = ""/Disease/AddOrUpdate"";
        //var GridRefreshLink = ""/Disease/RefreshGrid""
        //var data = $(""#addNewDiseaseForm"").serialize();
        var token = $(':input:hidden[name*=""RequestVerificationToken""]');
        var data = {
            _");
            WriteLiteral(@"_RequestVerificationToken: token.attr('value'),
            Guid: $(""#Guid"").val(),
            Name: $(""#Name"").val(),
            NameHolder: $(""#NameHolder"").val(),
            Explanation: $(""#Explanation"").val(),
            DiseaseTypeId: $(""#DiseaseTypeId"").val(),
            AllMedsForDisease: Medicines,
            AllSymptomsForDisease: Symptoms
        };


        $("".loader"").removeClass(""hidden"");
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

                        $(""#AddDiseaseModal #ERROR_Data"").removeClass(""hidden"");
                        $("".loade");
            WriteLiteral(@"r"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                    } else {

                        $('#AddDiseaseModal').modal('hide');
                        $("".modal-backdrop:last"").remove();
                        $('#AddDiseaseModal-body').empty();
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");
                        $("".k-pager-refresh"")[0].click();

                    }
                } else {

                    $('#AddDiseaseModal #ERROR_SomeThingWentWrong').removeClass('hidden');

                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            }
        });
    }

    function closeAddDiseaseModal() {

        $('#AddDiseaseModal').modal('hide');
        $('#AddDiseaseModal-body').empty();
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
