#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e59d4c45ad7dc3b586e206c1de2f5116d05d1f2c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Service.Views_Shared_PartialViews_AppWebForms_Service_wpUseService), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Service/wpUseService.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Service
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
#line 1 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
using WPH.Models.BaseInfo;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e59d4c45ad7dc3b586e206c1de2f5116d05d1f2c", @"/Views/Shared/PartialViews/AppWebForms/Service/wpUseService.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Service_wpUseService : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 6 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
  
    string font = "", pull = "", dir = "";
    if (HttpContextAccessor.HttpContext.Session.GetString("culture") == "en")
    {
        font = " MyFont-Roboto-grid ";
        pull = " pull-right ";
        dir = " ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newServiceText\" class=\"hidden\">");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                          Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                                            Write(Localizer["Service"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 14 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
    }
    else
    {
        font = " MyFont-Sarchia-grid ";
        pull = " pull-left ";
        dir = " direction:rtl; ";

#line default
#line hidden
#nullable disable
            WriteLiteral("        <h3 id=\"newServiceText\" class=\"hidden\">");
#nullable restore
#line 20 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                          Write(Localizer["Service"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(" ");
#nullable restore
#line 20 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                                                Write(Localizer["New"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n");
#nullable restore
#line 21 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 24 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n<div class=\"col-sm-12 shadow-border\" style=\"margin:0\">\r\n\r\n    <h4 class=\"MyFont-Roboto\" style=\"border-bottom: dotted 1px #808080\"> ");
#nullable restore
#line 29 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                                                    Write(Localizer["Services"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n\r\n    <div class=\"col-sm-12 \" style=\"display:flex;align-items: flex-end;\">\r\n\r\n        <div class=\"col-sm-3\">\r\n            <label class=\"block clearfix\">\r\n                <label for=\"form-field-8\">");
#nullable restore
#line 35 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                     Write(Localizer["Date"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                <span class=""block input-icon input-icon-right"">
                    <input id=""ServiceDate"" class=""boldText"" title=""datepicker"" style=""width: 100%; margin: 0"" />
                </span>
            </label>
        </div>

        <div class=""col-sm-5"">
            <label class=""block clearfix"">

                <label for=""form-field-8"">");
#nullable restore
#line 45 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                     Write(Localizer["Other"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n                <span class=\"block input-icon input-icon-right\">\r\n\r\n");
#nullable restore
#line 48 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                      
                        string width = "100%";
                    

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 52 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                     if ((bool)ViewBag.AccessNewService)
                    {
                        width = "80%";

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <a href=\"\\\\#\"");
            BeginWriteAttribute("class", " class=\"", 1854, "\"", 1884, 3);
#nullable restore
#line 55 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
WriteAttributeValue("  ", 1862, pull, 1864, 5, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 1869, "btn", 1870, 4, true);
            WriteAttributeValue(" ", 1873, "btn-danger", 1874, 11, true);
            EndWriteAttribute();
            WriteLiteral(" data-value=\"Other\" onclick=\"NewService(this)\" style=\" padding:0 0.2rem;\"><span class=\"fa fa-plus\" style=\"margin:0.1rem\"></span></a>\r\n");
#nullable restore
#line 56 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"

                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    <input id=\"ServiceName\" class=\"boldText\"");
            BeginWriteAttribute("style", " style=\"", 2106, "\"", 2138, 5);
            WriteAttributeValue("", 2114, "width:", 2114, 6, true);
#nullable restore
#line 59 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
WriteAttributeValue(" ", 2120, width, 2121, 6, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 2127, ";", 2127, 1, true);
            WriteAttributeValue(" ", 2128, "margin:", 2129, 8, true);
            WriteAttributeValue(" ", 2136, "0", 2137, 2, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                </span>\r\n            </label>\r\n        </div>\r\n\r\n        <div class=\"col-sm-3\">\r\n            <label class=\"block clearfix\">\r\n                <label for=\"form-field-8\">");
#nullable restore
#line 66 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                                     Write(Localizer["Num"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</label>
                <span class=""block input-icon input-icon-right"">
                    <input id=""ServiceValue"" class=""boldText"" style=""width: 100%; margin: 0"" />
                </span>
            </label>
        </div>

        <div class=""col-sm-1"">

            ");
#nullable restore
#line 75 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
        Write(Html.Kendo().Button()
                        .Name("btn-add-service")
                        .HtmlAttributes(new { style = "font-size:1.5rem;padding:0.7rem;margin-bottom:5px;", type = "button", @class = "k-primary", onclick = "addService()" })
                        .Content(Localizer["Add"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n        </div>\r\n\r\n    </div>\r\n\r\n    <div class=\"col-sm-12 \">\r\n\r\n        ");
#nullable restore
#line 86 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
   Write(await Html.PartialAsync("~/Views/Shared/PartialViews/AppWebForms/ReceptionService/dgReceptionSpeceficServiceGrid.cshtml", new SectionViewModel { Id = ViewBag.ReceptionId, Name = "Other" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n    </div>\r\n\r\n</div>\r\n\r\n\r\n<div id=\"CreateNewServiceModal\"");
            BeginWriteAttribute("class", " class=\"", 3256, "\"", 3280, 3);
            WriteAttributeValue("", 3264, "modal", 3264, 5, true);
            WriteAttributeValue(" ", 3269, "fade", 3270, 5, true);
#nullable restore
#line 93 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
WriteAttributeValue(" ", 3274, font, 3275, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-backdrop=\"static\" data-keyboard=\"false\"");
            BeginWriteAttribute("style", " style=\"", 3326, "\"", 3338, 1);
#nullable restore
#line 93 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
WriteAttributeValue("", 3334, dir, 3334, 4, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""modal-dialog"">
        <div class=""modal-content"">
            <div class=""modal-header"">
                <button type=""button"" class=""close"" onclick=""closeCreateNewServiceModal()"" aria-hidden=""true"">&times;</button>
                <h3 id=""CreateNewServiceModal-header""");
            BeginWriteAttribute("class", " class=\"", 3632, "\"", 3676, 5);
            WriteAttributeValue("", 3640, "smaller", 3640, 7, true);
            WriteAttributeValue(" ", 3647, "lighter", 3648, 8, true);
            WriteAttributeValue(" ", 3655, "blue", 3656, 5, true);
            WriteAttributeValue(" ", 3660, "no-margin", 3661, 10, true);
#nullable restore
#line 98 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
WriteAttributeValue(" ", 3670, font, 3671, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
                </h3>
            </div>

            <div id=""CreateNewServiceModal-body"" class=""modal-body"">
            </div>

            <div id=""ERROR_Data"" class=""red label-white middle hidden"" style=""height:40px;display:block"">
                <i class=""ace-icon fa fa-stop bigger-120""></i>
                ");
#nullable restore
#line 107 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
           Write(Localizer["ERROR_DataNotValid"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div id=\"ERROR_SomeThingWentWrong\" class=\"red label-white middle hidden\" style=\"height:40px;display:block\">\r\n                <i class=\"ace-icon fa fa-stop bigger-120\"></i>\r\n                ");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
           Write(Localizer["ERROR_InsertWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n\r\n            <div class=\"modal-footer\">\r\n                ");
#nullable restore
#line 116 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-CreateNewServiceModal-accept")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "k-primary pull-right" })
                .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                ");
#nullable restore
#line 120 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
            Write(Html.Kendo().Button()
                .Name("btn-CreateNewServiceModal-close")
                .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = "pull-right", onclick = "closeCreateNewServiceModal()" })
                .Content(Localizer["Exit"]));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
            </div>
        </div>
    </div>
</div>

<script>

    $(""#ServiceDate"").kendoDateTimePicker({
        value: new Date(),
        max: new Date(),
        format: ""dd/MM/yyyy HH:mm"",
        dateInput: true
    });

    $(""#ServiceName"").kendoDropDownList({
        dataTextField: ""Name"",
        dataValueField: ""Guid"",
        dataSource: {
            serverFiltering: false,
            transport: {
                read: {
                    url: ""/Service/GetAllSpeceficServices?ServiceName=Other"",
                }
            }
        }
    });

    $(""#ServiceValue"").kendoNumericTextBox({
    });


    function addService() {

        var TempDate = $(""#ServiceDate"").data(""kendoDateTimePicker"");
        var Date = TempDate.value();
        let cMonth = Date.getMonth() + 1;
        let cDay = Date.getDate();

        if (cMonth < 10)
            cMonth = ""0"" + cMonth;

        if (cDay < 10)
            cDay = ""0"" + cDay;

        let CostDateDay ");
            WriteLiteral(@"= cDay;
        let CostDateMonth = cMonth;
        let CostDateYear = Date.getFullYear();
        let CostDateHour = Date.getHours();
        let CostDateMin = Date.getMinutes();

        let SerNum = $(""#ServiceValue"").data(""kendoNumericTextBox"").value();
        if (SerNum === null || SerNum < 1) {
            SerNum = 1;
        }
        let receptionId = $(""#Reception-Id"").text();
        let ServiceId = $(""#ServiceName"").val();
        let Service = $(""#ServiceName"").data(""kendoDropDownList"");
        var dataItem = Service.dataItem();

        $.ajax({
            type: ""Post"",
            data:
            {
                ReceptionId: receptionId,
                ServiceId: ServiceId,
                Number: SerNum,
                Price: dataItem.Price,
                ServiceDateDay: CostDateDay,
                ServiceDateMonth: CostDateMonth,
                ServiceDateYear: CostDateYear,
                ServiceDateHour: CostDateHour,
                ServiceDateMin: C");
            WriteLiteral("ostDateMin\r\n            },\r\n            url: \"/ReceptionService/AddReceptionService\",\r\n            success: function (response) {\r\n\r\n                if (response === 0) {\r\n                    bootbox.alert({\r\n                        message: \'");
#nullable restore
#line 202 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                             Write(Localizer["ERROR_InsertWrong"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\',\r\n                        className: \'bootbox-class\'\r\n                    });\r\n\r\n                } else if (response === \"NotEnoughProductCount\") {\r\n                    bootbox.alert({\r\n                        message: \'");
#nullable restore
#line 208 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Service\wpUseService.cshtml"
                             Write(Localizer["NotEnoughProductCount"]);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"',
                        className: 'bootbox-class'
                    });

                } else {

                    $(""#Other .k-i-reload"").click();
                }


            }
        });

    }


    function NewService(element) {

        txt = $(element).attr('data-value');

        if (txt != 'Transfusion' && txt != 'Other' && txt != 'Stitch')
            return;

        $(""#CreateNewServiceModal #ERROR_Data"").addClass(""hidden"");
        $(""#CreateNewServiceModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        var link = `/Service/SingleNewModal?typeName=${txt}`;
        $("".loader"").removeClass(""hidden"");

        let header = $(""#newServiceText"").text();
        $('#CreateNewServiceModal-header').text(header);

        $('#CreateNewServiceModal').modal('toggle');
        $('#CreateNewServiceModal-body').load(link + '', function () {
            $("".loader"").fadeIn(""slow"");
            $("".loader"").addClass(""hidden"");
        });

    }

 ");
            WriteLiteral(@"   $('#btn-CreateNewServiceModal-accept').on(""click"", function () {
        $(this).attr(""disabled"", true);

        $(""#CreateNewServiceModal #ERROR_Data"").addClass(""hidden"");
        $(""#CreateNewServiceModal #ERROR_SomeThingWentWrong"").addClass(""hidden"");

        $('#CreateNewServiceModal .emptybox').addClass('hidden');
        var isEmmpty = true;
        $('#CreateNewServiceModal .emptybox').each(function () {
            if ($(this).attr('data-isEssential') === 'true') {
                var empty = $(this).attr('id');
                if ($('[data-checkEmpty=""' + empty + '""]').val() === """") {
                    $(this).removeClass('hidden');
                    $('#btn-CreateNewServiceModal-accept').removeAttr(""disabled"");
                    isEmmpty = false;
                    return;
                }
            }
        });

        if (isEmmpty === false) {
            return;
        }

        var link = ""/Service/AddOrUpdate"";
        var data = $(""#addNewServiceForm");
            WriteLiteral(@""").serialize();

        $("".loader"").removeClass(""hidden"");
        $.ajax({
            type: ""Post"",
            url: link,
            data: data,
            success: function (response) {
                $('#btn-CreateNewServiceModal-accept').removeAttr(""disabled"");

                if (response !== 0) {
                    if (response === ""ValueIsRepeated"") {

                        $('#CreateNewServiceModal #Name-Exist').removeClass('hidden');
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");
                    } else if (response === ""DataNotValid"") {

                        $(""#CreateNewServiceModal #ERROR_Data"").removeClass(""hidden"");
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                    } else {

                        $('#CreateNewServiceModal').modal('hide');
                        $("".modal-backdrop:last"").remove();
            ");
            WriteLiteral(@"            $('#CreateNewServiceModal-body').empty();
                        $("".loader"").fadeIn(""slow"");
                        $("".loader"").addClass(""hidden"");

                        if (txt == 'Transfusion') {
                            $(""#TransfusionName"").data(""kendoDropDownList"").dataSource.read();
                            let pr = $(""#TransfusionName"").data(""kendoDropDownList"");
                            pr.bind(""dataBound"", function () {
                                pr.value(response);
                            });
                            txt = '';
                        } else if (txt == 'Other') {
                            $(""#ServiceName"").data(""kendoDropDownList"").dataSource.read();
                            let pr = $(""#ServiceName"").data(""kendoDropDownList"");
                            pr.bind(""dataBound"", function () {
                                pr.value(response);
                            });
                            txt = '';
            ");
            WriteLiteral(@"            } else if (txt == 'Stitch') {
                            $(""#StitchName"").data(""kendoDropDownList"").dataSource.read();
                            let pr = $(""#StitchName"").data(""kendoDropDownList"");
                            pr.bind(""dataBound"", function () {
                                pr.value(response);
                            });
                            txt = '';
                        }
                    }
                } else {

                    $(""#CreateNewServiceModal #ERROR_SomeThingWentWrong"").removeClass(""hidden"");
                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            }
        });
    });

    function closeCreateNewServiceModal() {

        $('#CreateNewServiceModal').modal('hide');
        $('#CreateNewServiceModal-body').empty();
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
