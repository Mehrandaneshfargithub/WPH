#pragma checksum "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c1c62efddf8f058e271c51ac38e8154982221f73"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(WPH.Pages.Shared.PartialViews.AppWebForms.Receive.Views_Shared_PartialViews_AppWebForms_Receive_mdReceiveNewModal), @"mvc.1.0.view", @"/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveNewModal.cshtml")]
namespace WPH.Pages.Shared.PartialViews.AppWebForms.Receive
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
#line 2 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c1c62efddf8f058e271c51ac38e8154982221f73", @"/Views/Shared/PartialViews/AppWebForms/Receive/mdReceiveNewModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5221a2d8e2059d2e61789d73e7efe285a53b8943", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Shared_PartialViews_AppWebForms_Receive_mdReceiveNewModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WPH.Models.Receive.ReceiveViewModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("addNewReceiveForm"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral(@"

<script>
    function ChangeBaseCurrency(e) {
        var dataItem = this.dataItem(e.item);

        var currencyId = $(""#CurrencyId"").data(""kendoDropDownList"");
        currencyId.value(dataItem.Id);

        $(""#BaseAmount"").data(""kendoNumericTextBox"").value(1);
        $(""#DestAmount"").data(""kendoNumericTextBox"").value(1);
        $(""#lblBaseAmount"").text("" "");
        $(""#lblDestAmount"").text("" "");
        $(""#convert_container"").addClass(""hidden"");
    }

</script>

");
#nullable restore
#line 23 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
   
    var receive_amount = Model.ReceiveAmounts.FirstOrDefault();

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"signup-box\" class=\"signup-box no-border\">\r\n    <div class=\"widget-body\">\r\n        <div class=\"widget-main\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c1c62efddf8f058e271c51ac38e8154982221f734981", async() => {
                WriteLiteral("\r\n                <fieldset>\r\n                    ");
#nullable restore
#line 32 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
               Write(Html.AntiForgeryToken());

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 33 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
               Write(Html.HiddenFor(m => m.Guid));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 34 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
               Write(Html.HiddenFor(m => m.CustomerId));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    ");
#nullable restore
#line 35 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
               Write(Html.HiddenFor(m => m.ReceiveType));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n\r\n                    <div class=\"col-sm-12\" style=\"padding:0px;\">\r\n                        <div class=\"col-sm-6\">\r\n                            <label class=\"block clearfix\">\r\n                                <label for=\"form-field-8\">");
#nullable restore
#line 40 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["InvoiceNum"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 42 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().TextBox()
                                        .Name("InvoiceNum")
                                        .Enable(false)
                                        .Value(Model.InvoiceNum)
                                        .HtmlAttributes(new { style = "width: 100%" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>

                        <div class=""col-sm-6"">

                        </div>
                    </div>

                    <div class=""col-sm-12"" style=""padding:0px;"">
                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">");
#nullable restore
#line 60 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["Customer"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 62 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().TextBox()
                                        .Name("Customer")
                                        .Enable(false)
                                        .Value(Model.CustomerName)
                                        .HtmlAttributes(new { style = "width: 100%" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>

                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">");
#nullable restore
#line 74 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["BaseCurrency"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 76 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().DropDownList()
                                    .Name("BaseCurrencyId")
                                    .DataTextField("Name")
                                    .DataValueField("Id")
                                    .Events(e=>e.Change("ChangeBaseCurrency"))
                                    //.DataSource(source =>
                                    //{
                                    //    source.Read(read =>
                                    //    {
                                    //        read.Action("GetAllCurrencies", "BaseInfo");
                                    //    })
                                    //    .ServerFiltering(false);
                                    //})
                                    //.Value(Model.BaseCurrencyId?.ToString())
                                    .HtmlAttributes(new { style = "width: 100%;" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>

                            </label>
                        </div>
                    </div>

                    <div class=""col-sm-12"" style=""padding:0px;"">
                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">");
#nullable restore
#line 101 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["Date"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 103 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().DatePicker()
                                    .Name("ReceiveDateTxt")
                                    .Format("dd/MM/yyyy")
                                    .Value(Model.ReceiveDate ?? DateTime.Now)
                                    .Enable((bool)(ViewBag.AccessEditDate ?? false))
                                    .HtmlAttributes(new { style = "width: 100%;", @data_checkEmpty = "ReceiveDateTxt-box" })
                                    );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                </span>\r\n                                <span id=\"ReceiveDateTxt-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                                    ");
#nullable restore
#line 112 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                               Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                </span>\r\n                                <span id=\"Date-valid\" class=\"emptybox hidden\" style=\"color:red;\">\r\n                                    ");
#nullable restore
#line 115 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                               Write(Localizer["DateNotValid"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>

                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">");
#nullable restore
#line 122 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["Amount"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 124 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().NumericTextBox<decimal>()
                                        .Name("Amount")
                                        .Format("#,#.##")
                                        .Min(0)
                                        .Value(receive_amount?.Amount ?? 0)
                                        .HtmlAttributes(new { style = "width: 100%", @data_checkEmpty = "Amount-box" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                                </span>\r\n                                <span id=\"Amount-box\" class=\"emptybox hidden\" data-isEssential=\"true\" style=\"color:red;\">\r\n                                    ");
#nullable restore
#line 133 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                               Write(Localizer["ThisFieldIsEmptyPleaseFillIt"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>
                    </div>

                    <div class=""col-sm-12"" style=""padding:0px;"">
                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">");
#nullable restore
#line 142 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                     Write(Localizer["Currency"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 144 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().DropDownList()
                                    .Name("CurrencyId")
                                    .DataTextField("Name")
                                    .DataValueField("Id")
                                    //.DataSource(source =>
                                    //{
                                    //    source.Read(read =>
                                    //    {
                                    //        read.Action("GetAllCurrencies", "BaseInfo");
                                    //    })
                                    //    .ServerFiltering(false);
                                    //})
                                    //.Value(Model.BaseCurrencyId?.ToString())
                                    .HtmlAttributes(new { style = "width: 100%;", onchange = "changeCurrency()" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>

                        <div id=""convert_container"" class=""col-sm-6 hidden"">
                            <div class=""col-sm-6"">
                                <label class=""block clearfix "">
                                    <label id=""lblBaseAmount"" for=""form-field-8""></label>
                                    <span class=""block input-icon input-icon-right"">
                                        ");
#nullable restore
#line 168 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                    Write(Html.Kendo().NumericTextBox<decimal>()
                                        .Name("BaseAmount")
                                        .Format("#,#.##")
                                        .Decimals(0)
                                        .Min(1)
                                        .Value(receive_amount?.BaseAmount ?? 1)
                                        .HtmlAttributes(new { style = "width: 100%" })
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                    </span>
                                </label>
                            </div>

                            <div class=""col-sm-6"">
                                <label class=""block clearfix "">
                                    <label id=""lblDestAmount"" for=""form-field-8""></label>
                                    <span class=""block input-icon input-icon-right"">
                                        ");
#nullable restore
#line 184 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                    Write(Html.Kendo().NumericTextBox<decimal>()
                                        .Name("DestAmount")
                                        .Format("#,#.##")
                                        .Decimals(0)
                                        .Min(1)
                                        .Value(receive_amount?.DestAmount ?? 1)
                                        .HtmlAttributes(new { style = "width: 100%"})
                                        );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                    </span>
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class=""col-sm-12"" style=""padding:0px;"">
                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8"">% ");
#nullable restore
#line 201 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                       Write(Localizer["Discount"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 203 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().NumericTextBox<decimal>()
                                    .Name("DetailDiscount")
                                    .Culture("en-US")
                                    .Format("#.##")
                                    .Min(0)
                                    .Max(100)
                                    .SelectOnFocus(true)
                                    .Value(0)
                                    .HtmlAttributes(new { style = "width: 100%" })
                                    );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>

                        <div class=""col-sm-6"">
                            <label class=""block clearfix"">
                                <label for=""form-field-8""># ");
#nullable restore
#line 219 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                       Write(Localizer["Discount"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(": </label>\r\n                                <span class=\"block input-icon input-icon-right\">\r\n                                    ");
#nullable restore
#line 221 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(Html.Kendo().NumericTextBox<decimal>()
                                    .Name("Discount")
                                    .Culture("en-US")
                                    .Format("#.##")
                                    .Min(0)
                                    .SelectOnFocus(true)
                                    .Value(receive_amount?.Discount ?? 0)
                                    .HtmlAttributes(new { style = "width: 100%" })
                                    );

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                                </span>
                            </label>
                        </div>
                    </div>

                    <div class=""col-sm-12"">
                        <label class=""block clearfix"">
                            <label for=""form-field-8"">");
#nullable restore
#line 237 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                                 Write(Localizer["Description"]);

#line default
#line hidden
#nullable disable
                WriteLiteral(" : </label>\r\n                            <span class=\"block input-icon input-icon-right\">\r\n                                ");
#nullable restore
#line 239 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                            Write(Html.Kendo().TextArea()
                                .Name("Description")
                                .Rows(3)
                                .Value(Model.Description)
                                .HtmlAttributes(new { style = "font-size:1.5rem;width:100%" })
                                );

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </span>\r\n                        </label>\r\n                    </div>\r\n\r\n                    <div class=\"space-24\"></div>\r\n                </fieldset>\r\n            ");
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
    SetDiscount();
    $(document).ready(function () {
        $.ajax({
            type: ""Get"",
            url: ""/BaseInfo/GetAllCurrencies"",
            success: function (response) {

                var baseCurrencyId = $(""#BaseCurrencyId"").data(""kendoDropDownList"");
                baseCurrencyId.dataSource.data(response);
                baseCurrencyId.value(");
#nullable restore
#line 266 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                                Write(receive_amount?.BaseCurrencyId);

#line default
#line hidden
#nullable disable
            WriteLiteral(");\r\n\r\n                var currencyId = $(\"#CurrencyId\").data(\"kendoDropDownList\");\r\n                currencyId.dataSource.data(response);\r\n                currencyId.value(");
#nullable restore
#line 270 "H:\Projects\WAS\WPH\Views\Shared\PartialViews\AppWebForms\Receive\mdReceiveNewModal.cshtml"
                            Write(receive_amount?.CurrencyId);

#line default
#line hidden
#nullable disable
            WriteLiteral(@");

            }
        });

        setTimeout(function () {
            $(""#BaseCurrencyId"").data(""kendoDropDownList"").focus();
        },500);
    });

    $('#BaseCurrencyId').parent().keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            $('#ReceiveDateTxt').focus();
        }
    });


    $('#ReceiveDateTxt').on('keypress', function (e) {

        if (e.which === 13) {
            $('#Amount').data(""kendoNumericTextBox"").focus();
        }
    });

    $(""#Amount"").on(""focus"", function (e) {
        $('#Amount').select();
    });

    $(""#Amount"").on(""keypress"", function (e) {
        if (e.which === 13) {
            $(""#CurrencyId"").data(""kendoDropDownList"").focus();
        }
    });

    $('#CurrencyId').parent().keypress(function (e) {
        if (e.which === 13 || e.which === 9) {
            let currency = $(""#CurrencyId"").val();
            let base = $(""#BaseCurrencyId"").val();

            if (currency !== base) {
        ");
            WriteLiteral(@"        $('#BaseAmount').data(""kendoNumericTextBox"").focus();

            } else {

                $('#DetailDiscount').data(""kendoNumericTextBox"").focus();
            }
        }
    });

    $(""#BaseAmount"").on(""keypress"", function (e) {
        if (e.which === 13) {
            $(""#DestAmount"").data(""kendoNumericTextBox"").focus();
        }
    });

    $(""#DestAmount"").on(""keypress"", function (e) {
        if (e.which === 13) {

            $('#DetailDiscount').data(""kendoNumericTextBox"").focus();
        }
    });

    $(""#DetailDiscount"").on(""focus"", function (e) {
        $('#DetailDiscount').select();
    });

    $(""#DetailDiscount"").focusout(function () {

        let wholepur = $('#Amount').data(""kendoNumericTextBox"").value();
        if (wholepur > 0) {
            let dis = $('#DetailDiscount').data(""kendoNumericTextBox"").value();
            let amount = (dis * wholepur) / 100;
            $('#Discount').data(""kendoNumericTextBox"").value(amount);
        }

");
            WriteLiteral(@"    });

    $(""#DetailDiscount"").on(""keypress"", function (e) {
        if (e.which === 13) {

            $('#Discount').data(""kendoNumericTextBox"").focus();
        }
    });


    $(""#Discount"").on(""keypress"", function (e) {
        if (e.which === 13) {

            $('#Description').focus();
        }
    });

    $(""#Discount"").on(""focus"", function (e) {
        $('#Discount').select();
    });

    $(""#Discount"").focusout(function () {
        SetDiscount();
    });

    function SetDiscount() {
        let wholepur = $('#Amount').data(""kendoNumericTextBox"").value();
        if (wholepur > 0) {
            let dis = $('#Discount').data(""kendoNumericTextBox"").value();
            let amount = (100 * dis) / wholepur;
            $('#DetailDiscount').data(""kendoNumericTextBox"").value(amount);
        }
    }

    function changeCurrency() {
        let currency = $(""#CurrencyId"").val();
        let base = $(""#BaseCurrencyId"").val();

        if (currency !== base) {
");
            WriteLiteral(@"            $(""#convert_container"").removeClass('hidden');


            $("".loader"").removeClass(""hidden"");
            $.ajax({
                type: ""Get"",
                url: ""/MoneyConvert/GetMoneyConvertBaseCurrencies"",
                data: {
                    baseCurrencyId: base,
                    destCurrencyId: currency
                },
                success: function (response) {

                    if (response == null || response === 0) {

                        $(""#BaseAmount"").data(""kendoNumericTextBox"").value(1);
                        $(""#DestAmount"").data(""kendoNumericTextBox"").value(1);

                    } else {
                        let base_currency = response.BaseCurrencyId;
                        let dest_currency = response.DestCurrencyId;

                        if (base_currency == base) {

                            $(""#BaseAmount"").data(""kendoNumericTextBox"").value(response.BaseAmount);
                            $(""#DestAmount"").data");
            WriteLiteral(@"(""kendoNumericTextBox"").value(response.DestAmount);

                        } else {

                            $(""#DestAmount"").data(""kendoNumericTextBox"").value(response.BaseAmount);
                            $(""#BaseAmount"").data(""kendoNumericTextBox"").value(response.DestAmount);

                        }

                    }

                    $("".loader"").fadeIn(""slow"");
                    $("".loader"").addClass(""hidden"");
                }
            });


            $(""#lblBaseAmount"").text($(""#BaseCurrencyId"").data(""kendoDropDownList"").text());
            $(""#lblDestAmount"").text($(""#CurrencyId"").data(""kendoDropDownList"").text());

        } else {

            $(""#BaseAmount"").data(""kendoNumericTextBox"").value(1);
            $(""#DestAmount"").data(""kendoNumericTextBox"").value(1);
            $(""#lblBaseAmount"").text("" "");
            $(""#lblDestAmount"").text("" "");
            $(""#convert_container"").addClass(""hidden"");
        }
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WPH.Models.Receive.ReceiveViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
