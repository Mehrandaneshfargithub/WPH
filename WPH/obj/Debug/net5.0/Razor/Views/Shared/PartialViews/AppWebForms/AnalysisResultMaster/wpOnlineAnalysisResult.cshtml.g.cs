#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d42005a9f387721b7b8e0e71bbca861ef9b2f798"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.AnalysisResultMaster.Views_Shared_PartialViews_AppWebForms_AnalysisResultMaster_wpOnlineAnalysisResult), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/AnalysisResultMaster/wpOnlineAnalysisResult.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.AnalysisResultMaster
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
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d42005a9f387721b7b8e0e71bbca861ef9b2f798", @"/Views/Shared/PartialViews/AppWebForms/AnalysisResultMaster/wpOnlineAnalysisResult.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_AnalysisResultMaster_wpOnlineAnalysisResult : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("addNewForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n\r\n\r\n");
#nullable restore
#line 8 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
  
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
            WriteLiteral("\r\n\r\n\r\n<div class=\"row page-header\">\r\n    <div");
            BeginWriteAttribute("class", " class=\"", 609, "\"", 622, 1);
#nullable restore
#line 28 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
WriteAttributeValue("", 617, pull, 617, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <h1");
            BeginWriteAttribute("class", " class=\"", 637, "\"", 650, 1);
#nullable restore
#line 29 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
WriteAttributeValue("", 645, font, 645, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"font-size: 2.3rem\">\r\n            ");
#nullable restore
#line 30 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
       Write(Localizer["AnalysisResult"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </h1>\r\n\r\n    </div>\r\n</div><!-- /.page-header -->\r\n\r\n\r\n<div id=\"signup-box\"");
            BeginWriteAttribute("class", " class=\"", 805, "\"", 839, 3);
            WriteAttributeValue("", 813, "signup-box", 813, 10, true);
            WriteAttributeValue(" ", 823, "no-border", 824, 10, true);
#nullable restore
#line 37 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
WriteAttributeValue(" ", 833, font, 834, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n    <div class=\"widget-body\">\r\n        <div class=\"widget-main\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d42005a9f387721b7b8e0e71bbca861ef9b2f7986618", async() => {
                WriteLiteral("\r\n                <fieldset>\r\n\r\n                    <div class=\"col-xs-12\"");
                BeginWriteAttribute("style", " style=\"", 1017, "\"", 1035, 1);
#nullable restore
#line 43 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
WriteAttributeValue("", 1025, direction, 1025, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@">

                        <div class=""col-xs-6"" style=""padding:0"">

                            <div class=""col-sm-6"" style=""padding:0 1rem"">

                                <label class=""block clearfix"">
                                    <label for=""form-field-8"">");
#nullable restore
#line 50 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                                         Write(Localizer["Code"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@" : </label>
                                    <span class=""block input-icon input-icon-right"">
                                        <input id=""FromDate"" class=""hidden"" title=""datepicker"" style=""width: 100%;font-size:1.5rem"" type=""date"" />
                                        ");
#nullable restore
#line 53 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                    Write(Html.Kendo().TextBox()
                                            .Name("Code")
                                            .HtmlAttributes(new { style = "width: 100%" })
                                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </span>\r\n                                    <span id=\"Code-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                                        ");
#nullable restore
#line 59 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                   Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                    </span>

                                </label>

                            </div>

                            <div class=""col-sm-6"" style=""padding:0 1rem"">

                                <label class=""block clearfix"">
                                    <label for=""form-field-8"">");
#nullable restore
#line 69 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                                         Write(Localizer["Mobile"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@" : </label>
                                    <span class=""block input-icon input-icon-right"">
                                        <input id=""ToDate"" class=""hidden"" title=""datepicker"" style=""width: 100%;font-size:1.5rem"" type=""date"" />
                                        ");
#nullable restore
#line 72 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                    Write(Html.Kendo().TextBox()
                                            .Name("Number")
                                            .HtmlAttributes(new {  style = "width: 100%" })
                                            );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                    </span>\r\n                                    <span id=\"PhoneNumber-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                                        ");
#nullable restore
#line 78 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
                                   Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                    </span>

                                </label>
                            </div>

                        </div>

                    </div>
                    <div class=""col-sm-1"">
                        <h3 id=""notExist"" class=""red hidden"" >Not Exist</h3>
                    </div>
                    

                    <div class=""space-24""></div>
                </fieldset>
            ");
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
            WriteLiteral("\r\n        </div>\r\n    </div><!-- /.widget-body -->\r\n</div><!-- /.signup-box -->\r\n<div class=\"col-sm-12\">\r\n\r\n    \r\n        ");
#nullable restore
#line 101 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\AnalysisResultMaster\wpOnlineAnalysisResult.cshtml"
    Write(Html.Kendo().Button()
        .Name("btn-GetReport-accept")
        .HtmlAttributes(new { style = "font-size:15px;height:35px;margin:2px;", type = "button", @class = $"k-primary {pull}", onclick = "GetResultFile()" })
        .Content(Localizer["Ok"]));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    

</div>


    <script>

        function GetResultFile() {

            if ($(""#Code"").val() === """") {
                $(""#Code-box"").removeClass('hidden');
                return;
            }

            if ($(""#Number"").val() === """") {
                $(""#PhoneNumber-box"").removeClass('hidden');
                return;
            }
            else {
                $(""#Code-box"").addClass('hidden');
                $(""#PhoneNumber-box"").addClass('hidden');
            }



            let Code = $(""#Code"").val();
            let PhoneNumber = $(""#Number"").val();
            //fetch(""/Home/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber).then((response) => {console.log(response)})
            //let mypage = window.open(""/Home/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber, '_blank').;

            //console.log(mypage);
            //console.log(mypage.document.body);
            //console.log(mypage.document.body.innerText);
            //console");
            WriteLiteral(@".log(mypage.document.body.firstChild);
            //window.location = (""/Home/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber, _blank);
            //fetch(""/Home/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber).then((res) => {console.log(res)});
            $.ajax({

                url: ""/AnalysisResult/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber,
                success: function (data) {
                    if (data === ""NotExist"")
                        $(""#notExist"").removeClass('hidden');
                    else {
                $(""#notExist"").css(""visibility"", ""hidden"");

                        let ss = window.open(""/AnalysisResult/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber, '_blank');
                        //let ss = window.location = ""/Home/GetResult?Code="" + Code + ""&PhoneNumber="" + PhoneNumber;
                        //console.log(ss.)
                    }

                },

            });

    }

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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
