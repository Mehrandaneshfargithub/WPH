#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "881bd1804eb5cdad203ff4a2c7ba95465a18a0d6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Item.Views_Shared_PartialViews_AppWebForms_Item_mdItemNewModal), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Item/mdItemNewModal.cshtml")]
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
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"881bd1804eb5cdad203ff4a2c7ba95465a18a0d6", @"/Views/Shared/PartialViews/AppWebForms/Item/mdItemNewModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Item_mdItemNewModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WPH.Models.Item.ItemViewModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("addNewItemForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n<div id=\"signup-box\" class=\"signup-box no-border\">\r\n    <div class=\"widget-body\">\r\n        <div class=\"widget-main\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "881bd1804eb5cdad203ff4a2c7ba95465a18a0d64136", async() => {
                WriteLiteral("\r\n                <fieldset>\r\n                    ");
#nullable restore
#line 11 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 12 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
               Write(Html.HiddenFor(m => m.Guid));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 13 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
               Write(Html.HiddenFor(m => m.NameHolder));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 15 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                                             Write(Localizer["Name"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n                            ");
#nullable restore
#line 17 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                        Write(Html.Kendo().TextBox()
                                .Name("Name")
                                .Value(Model.Name)
                                .HtmlAttributes(new { style = "width: 100%", @data_checkEmpty = "Name-box" })
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                        <span id=\"Name-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 24 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                       Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                        <span id=\"Name-Exist\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 27 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                       Write(Localizer["TheNameIsDuplicatedPleaseTryAnotherName"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n");
                WriteLiteral("                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 33 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                                             Write(Localizer["ItemType"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n\r\n                            ");
#nullable restore
#line 36 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                        Write(Html.Kendo().AutoComplete()
                            .Name("ItemTypeName")
                            .DataTextField("Name")
                            //.Events(events => events.Select("OnPatientSelectInEmergencyReception"))
                            .Filter("contains")
                            .HighlightFirst(true)
                            .ClearButton(false)
                            .HtmlAttributes(new { style = "width:100%;", @data_checkEmpty = "ItemType-box" })
                            .DataSource(source =>
                            {
                                source.Read(read =>
                                {
                                    read.Action("GetAllItemTypes", "BaseInfo");
                                })
                                .ServerFiltering(false);
                            })
                            .Value(Model.ItemTypeName)
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                        </span>\r\n                        <span id=\"ItemType-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                            ");
#nullable restore
#line 57 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                       Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                        </span>\r\n                    </label>\r\n\r\n                    <label class=\"block clearfix\">\r\n                        <label for=\"form-field-8\">");
#nullable restore
#line 62 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                                             Write(Localizer["Section"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                        <span class=\"block input-icon input-icon-right\">\r\n\r\n                            ");
#nullable restore
#line 65 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Item\mdItemNewModal.cshtml"
                        Write(Html.Kendo().AutoComplete()
                            .Name("SectionName")
                            .DataTextField("Name")
                            //.Events(events => events.Select("OnPatientSelectInEmergencyReception"))
                            .Filter("contains")
                            .HighlightFirst(true)
                            .ClearButton(false)
                            .HtmlAttributes(new { style = "width:100%;" })
                            .DataSource(source =>
                            {
                                source.Read(read =>
                                {
                                    read.Action("GetAllBaseInfosByBaseInfoTypeName", "BaseInfo", new { TypeName = "Section" });
                                })
                                .ServerFiltering(false);
                            })
                            .Value(Model.SectionName)
                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                        </span>\r\n                        \r\n                    </label>\r\n\r\n                    <div class=\"space-24\"></div>\r\n                </fieldset>\r\n            ");
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
            WriteLiteral(@"
        </div>
    </div><!-- /.widget-body -->
</div><!-- /.signup-box -->

<script>

    $(document).ready(function () {


        setTimeout(function () {
            $(""#Name"").focus();
        }, 200);

    });


    $('#Name').focus(function () {
        $(""#Name-Exist"").addClass('hidden');
        $(""#Name-box"").addClass('hidden');
        $(""#Name"").select();
    });

    $('#Name').on('keypress', function (e) {

        if (e.which === 13) {
            $('#ItemTypeName').data(""kendoAutoComplete"").focus();
        }
    });

    $(""#ItemTypeName"").on('focus', function (e) {

        let inputAuto = $(""#ItemTypeName"").data(""kendoAutoComplete"");
        let value = inputAuto.value();
        inputAuto.search(value);
    });

    $(""#SectionName"").on('focus', function (e) {

        let inputAuto = $(""#SectionName"").data(""kendoAutoComplete"");
        let value = inputAuto.value();
        inputAuto.search(value);
    });

    $(""#ItemTypeName"").on(""keypress"",");
            WriteLiteral(@" function (e) {
        if (e.which === 13) {
            $('#SectionName').data(""kendoAutoComplete"").focus();
        }
    });

    $(""#SectionName"").on(""keypress"", function (e) {
        if (e.which === 13) {
            $('#btn-addItem-accept').focus();
        }
    });

   
</script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WPH.Models.Item.ItemViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
