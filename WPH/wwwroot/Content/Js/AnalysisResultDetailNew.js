

var patientAuto;
var invoiceNumAuto;
let priority = 1;
var printed = false;
var receptionAnalysis;
var analysisResult;



function patientReceptionAnalysisAjax() {

    let receptionId = $("#PatientReceptionId").val();

    return $.ajax({

        type: "Post",
        url: "/AnalysisResultMaster/GetAllPatientReceptionAnalysis",
        data: {
            ReceptionId: receptionId
        },

        success: function (response) {
            receptionAnalysis = response;

        }
    });

}

function AnalysisResultAjax() {

    let masterId = $("#Guid").val();

    return $.ajax({

        type: "Post",
        url: "/AnalysisResult/GetAllAnalysisResult",
        data: {
            AnalysisResultMasterId: masterId
        },

        success: function (response) {
            analysisResult = response;
        }
    });

}



$(document).ready(function () {

    $.when(AnalysisResultAjax(), patientReceptionAnalysisAjax()).done(function (a1, a2) {
        $("#AnalysisTab").empty();
        CreateDynamicTab(receptionAnalysis);
        let all = $("[data-analysisItem]");

        let date = $("#PatientDateOfBirth").data("kendoDatePicker");
        if ($("#PatientDateOfBirth").val() !== "")
            getAge(kendo.toString(date.value(), 'd'));
        keyPressConfigurations();
        if ($(all[0]).hasClass('text') || $(all[0]).hasClass('number')) {
            $(all[0]).focus();
        }
        if ($(all[0]).hasClass('dropdown')) {
            let AmountCurrencyId = $(all[0]).data("kendoDropDownList");
            AmountCurrencyId.focus();
            AmountCurrencyId.open();
        }

        if (analysisResult.length !== 0) {
            for (let i = 1; i < priority; i++) {
                let editor = $("#dropdown_" + i).data("kendoDropDownList");
                let analysisId = editor.element.context.dataset.analysis;
                if (analysisId === undefined) {
                    analysisId = null;
                }
                let analysisItemId = editor.element.context.dataset.analysisitem;
                let groupAnalysisId = editor.element.context.dataset.group;
                if (groupAnalysisId === undefined) {
                    groupAnalysisId = null;
                }
                let val = analysisResult.find(x => x.AnalysisId == analysisId && x.AnalysisItemId == analysisItemId && x.GroupAnalysisId == groupAnalysisId);
                //if (val !== null && val !== undefined)
                editor.value(val.Value);
            }
        }

    });

    let tem = "";
    $.ajax({

        type: "Post",
        url: "/ClinicSectionSetting/GetSpecificSettingByName",
        data: {

            SettingName: "UseTemplate"
        },

        success: function (response) {
            tem = response.toLowerCase();
            if (tem.toLowerCase() === "true") {


                $('#UseTemplateCheckbox').prop('checked', true);
                $('#TempelateSection').removeClass("hidden");
                $('#TempelateSection2').removeClass("hidden");
                $('#EditorTab').removeClass("hidden");
                $('#ExplanationTab').addClass("hidden");
                $("#useTemp").text(tem.toLowerCase());

            }
            else {
                $("#useTemp").text(tem.toLowerCase());
            }


        }
    });


    $("select.k-fontSize").children().first().remove();

    let font = $("input.k-fontSize").attr("aria-owns");
    $("#" + font).children().first().remove();
    let ul = $("#" + font).children().eq(4).click();

});


function CreateDynamicTab(allAnalysis) {

    let allSources = [];
    let dataSource = [];
    let analysisItemTabName = "Other Analyses";
    let analysisItemTabContent = "<div class ='tabs col-sm-12' style = ''>";
    let minVal = "";
    let maxVal = "";
    let analysisItemExist = false;
    if (ageInterval === "baby") {
        minVal = "B_minValue";
        maxVal = "B_maxValue";
    }
    else if (ageInterval === "child") {
        minVal = "C_minValue";
        maxVal = "C_maxValue";
    }
    else if (ageInterval === "adult") {
        if (gender === "Man") {
            minVal = "M_minValue";
            maxVal = "M_maxValue";
        }
        else {
            minVal = "F_minValue";
            maxVal = "F_maxValue";
        }
    }

    for (let i = 0; i < allAnalysis.length; i++) {
        if (allAnalysis[i].GroupAnalysis !== null && allAnalysis[i].GroupAnalysis !== undefined) {
            let tabName = allAnalysis[i].GroupAnalysis.Name;
            let tabContent = "<div class ='tabs col-sm-12' style=''>";

            if (allAnalysis[i].GroupAnalysis.GroupAnalysisAnalyses !== null && allAnalysis[i].GroupAnalysis.GroupAnalysisAnalyses !== undefined) {
                let analysis = allAnalysis[i].GroupAnalysis.GroupAnalysisAnalyses;

                for (let j = 0; j < analysis.length; j++) {
                    let subContent = "<div class='radius-border row tabs' style = 'padding:1rem;margin:1rem 0;direction:ltr'><h3 class='MyFont-Roboto-grid' style='border-bottom: dotted 1px rgba(0,0,0,0.3);margin-bottom:1rem'>" + analysis[j].Analysis.Name + "</h3>";
                    if (analysis[j].Analysis.Analysis_AnalysisItem !== null && analysis[j].Analysis.Analysis_AnalysisItem !== undefined) {
                        let analysisItem = analysis[j].Analysis.Analysis_AnalysisItem;
                        for (let k = 0; k < analysisItem.length; k++) {
                            if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                                analysisItem[k].AnalysisItem.NormalValues = "";
                            if (analysisItem[k].AnalysisItem.UnitName === "null" || analysisItem[k].AnalysisItem.UnitName === null || analysisItem[k].AnalysisItem.UnitName === "NULL")
                                analysisItem[k].AnalysisItem.UnitName = "";
                            if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {

                                subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional' style='width: 100%;'  data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                let source = [];
                                if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                                    for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                        source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                                    }
                                }
                                allSources.push(source);
                                priority++;

                            }
                            else {

                                var set_value = '';
                                var show_chart = '';
                                var set_numerical = '';
                                var set_class = 'text';
                                
                                if (analysisResult.length !== 0) {
                                    if (analysisItem[k].AnalysisId === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);
                                    if (val === null || val === undefined) {
                                        set_value = "";
                                        
                                        if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                            show_chart = ' checked ';
                                    }
                                    else {
                                        set_value = "  value = " + val.Value;
                                        
                                        if (val.ShowChart != null && val.ShowChart.toString().toLowerCase() == 'true')
                                            show_chart = ' checked ';
                                    }
                                } else {
                                    if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                        show_chart = ' checked ';
                                }

                                if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                                    set_numerical = "  type='number'  ";
                                    set_class = "number";
                                }

                                subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'  style='display:flex; align-items:center;'>  <div class='col-sm-3'> <input id='" + analysisItem[k].AnalysisItem.Guid + "' name='gri" + k + "' class='k-checkbox kendoCheckbox'  type='checkbox' " + show_chart + "> <label class='fa fa-chart-column' for='gri" + k + "'></label></div>  <div class='col-sm-9'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div> </div><div class = 'col-sm-2 pull-left' ><input class='" + set_class + "' style = 'width:100%' " + set_numerical + "  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  " + set_value + " /></div > <div class='col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div > ";
                            }
                        }
                    }
                    subContent = subContent + "</div>";
                    tabContent = tabContent + subContent;
                }
            }

            if (allAnalysis[i].GroupAnalysis.GroupAnalysisItems !== null && allAnalysis[i].GroupAnalysis.GroupAnalysisItems !== undefined) {
                let analysisItem = allAnalysis[i].GroupAnalysis.GroupAnalysisItems;
                let content = "";
                for (let k = 0; k < analysisItem.length; k++) {
                    if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                        analysisItem[k].AnalysisItem.NormalValues = "";
                    if (analysisItem[k].AnalysisItem.UnitName === "null" || analysisItem[k].AnalysisItem.UnitName === null || analysisItem[k].AnalysisItem.UnitName === "NULL")
                        analysisItem[k].AnalysisItem.UnitName = "";
                    if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown' style = 'width:100%' title='optional'   data-group=" + allAnalysis[i].GroupAnalysis.Guid + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        let source = [];
                        if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                            for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                            }
                        }
                        allSources.push(source);
                        priority++;
                    }
                    else {

                        var set_value = '';
                        var show_chart = '';
                        var set_numerical = '';
                        var set_class = 'text';
                        
                        if (analysisResult.length !== 0) {
                            if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                analysisItem[k].AnalysisId = null;
                            let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);
                            if (val === null || val === undefined) {
                                set_value = " ";

                                if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                    show_chart = ' checked ';
                            }
                            else {
                                set_value = "  value = " + val.Value;

                                if (val.ShowChart != null && val.ShowChart.toString().toLowerCase() == 'true')
                                    show_chart = ' checked ';
                            }
                        } else {
                            if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                show_chart = ' checked ';
                        }

                        if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                            set_numerical = "  type='number'  ";
                            set_class = "number";
                        }

                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'  style='display:flex; align-items:center;'>  <div class='col-sm-3'> <input id='" + analysisItem[k].AnalysisItem.Guid + "' name='gra" + k + "' class='k-checkbox kendoCheckbox'  type='checkbox' " + show_chart + "> <label class='fa fa-chart-column' for='gra" + k + "'></label></div>  <div class='col-sm-9'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div> </div><div class = 'col-sm-2 pull-left' ><input class='" + set_class + "' style='width:100%' " + set_numerical + "  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  " + set_value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                    }
                }
                tabContent = tabContent + content;
            }

            tabContent = tabContent + "</div>";

            dataSource.push({ text: tabName, content: tabContent, CssClass: "analysis-name" });

        }

        if (allAnalysis[i].Analysis !== null && allAnalysis[i].Analysis !== undefined) {
            let tabName = allAnalysis[i].Analysis.Name;
            let tabContent = "<div class ='tabs col-sm-12' style=''>";
            let content = "";
            if (allAnalysis[i].Analysis.Analysis_AnalysisItem !== null && allAnalysis[i].Analysis.Analysis_AnalysisItem !== undefined) {
                let analysisItem = allAnalysis[i].Analysis.Analysis_AnalysisItem;

                for (let k = 0; k < analysisItem.length; k++) {
                    if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                        analysisItem[k].AnalysisItem.NormalValues = "";
                    if (analysisItem[k].AnalysisItem.UnitName === "null" || analysisItem[k].AnalysisItem.UnitName === null || analysisItem[k].AnalysisItem.UnitName === "NULL")
                        analysisItem[k].AnalysisItem.UnitName = "";
                    if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional' style='width: 100%;' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        let source = [];
                        if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                            for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                            }
                        }
                        allSources.push(source);
                        priority++;
                    }
                    else {

                        var set_value = '';
                        var show_chart = '';
                        var set_numerical = '';
                        var set_class = 'text';
                        
                        if (analysisResult.length !== 0) {
                            if (allAnalysis[i].AnalysisId === undefined)
                                allAnalysis[i].AnalysisId = null;

                            let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == null);
                            if (val === null || val === undefined) {
                                set_value = " ";

                                if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                    show_chart = ' checked ';
                            }
                            else {
                                set_value = "  value = " + val.Value;

                                if (val.ShowChart != null && val.ShowChart.toString().toLowerCase() == 'true')
                                    show_chart = ' checked ';
                            }
                        } else {
                            if (analysisItem[k].AnalysisItem.ShowChart != null && analysisItem[k].AnalysisItem.ShowChart.toString().toLowerCase() == 'true')
                                show_chart = ' checked ';
                        }

                        if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                            set_numerical = "  type='number'  ";
                            set_class = "number";
                        }

                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'  style='display:flex; align-items:center;'>  <div class='col-sm-3'> <input id='" + analysisItem[k].AnalysisItem.Guid + "' name='alla" + k + "' class='k-checkbox kendoCheckbox'  type='checkbox' " + show_chart + "> <label class='fa fa-chart-column' for='alla" + k + "'></label></div>  <div class='col-sm-9'><span class='pull-right analysis-name' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.UnitName + ")" + "</span></div> </div><div class = 'col-sm-2 pull-left' ><input class ='" + set_class + "' style='width:100%' " + set_numerical + " title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  " + set_value + " /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                    }
                }
            }
            tabContent = tabContent + content + "</div>";

            dataSource.push({ text: tabName, content: tabContent, CssClass: "analysis-name" });
        }
        if (allAnalysis[i].AnalysisItem !== null && allAnalysis[i].AnalysisItem !== undefined) {
            analysisItemExist = true;

            let analysisItem = allAnalysis[i].AnalysisItem;
            if (analysisItem.NormalValues === "null" || analysisItem.NormalValues === null || analysisItem.NormalValues === "NULL")
                analysisItem.NormalValues = "";
            if (analysisItem.UnitName === "null" || analysisItem.UnitName === null || analysisItem.UnitName === "NULL")
                analysisItem.UnitName = "";
            let content = "";
            if (analysisItem.ValueTypeName === "Optional") {
                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' >" + analysisItem.Name + " (" + analysisItem.UnitName + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional'   data-analysisItem=" + analysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";

                let source = [];
                if (analysisItem.AnalysisItemValuesRanges !== null && analysisItem.AnalysisItemValuesRanges !== undefined) {

                    for (let range = 0; range < analysisItem.AnalysisItemValuesRanges.length; range++) {
                        source.push(analysisItem.AnalysisItemValuesRanges[range]);
                    }
                }
                allSources.push(source);
                priority++;
            }
            else {

                var set_value = '';
                var show_chart = '';
                var set_numerical = '';
                var set_class = 'text';
                
                if (analysisResult.length !== 0) {

                    let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem.Guid && x.GroupAnalysisId == null);
                    if (val === null || val === undefined) {
                        set_value = " ";

                        if (analysisItem.ShowChart != null && analysisItem.ShowChart.toString().toLowerCase() == 'true')
                            show_chart = ' checked ';
                    }
                    else {
                        set_value = "  value = " + val.Value;

                        if (val.ShowChart != null && val.ShowChart.toString().toLowerCase() == 'true')
                            show_chart = ' checked ';
                    }
                } else {
                    if (analysisItem.ShowChart != null && analysisItem.ShowChart.toString().toLowerCase() == 'true')
                        show_chart = ' checked ';
                }

                if (analysisItem.ValueTypeName === "Numerical") {
                    set_numerical = "  type='number'  ";
                    set_class = "number";
                }

                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0;display:flex;align-items:center;'><div class = 'col-sm-4 pull-left'  style='display:flex; align-items:center;'>  <div class='col-sm-3'> <input id='" + analysisItem.Guid + "' name='alli" + i + "' class='k-checkbox kendoCheckbox'  type='checkbox' " + show_chart + "> <label class='fa fa-chart-column' for='alli" + i + "'></label></div>  <div class='col-sm-9'><span class='pull-right analysis-name' >" + analysisItem.Name + " (" + analysisItem.UnitName + ")" + "</span></div> </div><div class = 'col-sm-2 pull-left' ><input class='" + set_class + "' style = 'width:100%' " + set_numerical + " title='numeric'    data-analysisItem=" + analysisItem.Guid + "  " + set_value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";

            }

            analysisItemTabContent = analysisItemTabContent + content;
        }
    }

    if (analysisItemExist) {
        analysisItemTabContent = analysisItemTabContent + "</div>";
        dataSource.push({ text: analysisItemTabName, content: analysisItemTabContent, CssClass: "analysis-name" });
    }


    $("#AnalysisTab").kendoTabStrip({
        dataTextField: "text",
        dataContentField: "content",
        dataSpriteCssClass: "CssClass",
        dataSource: dataSource

    }).data("kendoTabStrip").select(0);

    $(".number").kendoNumericTextBox();
    $(".text").kendoTextBox();

    for (let i = 1; i < priority; i++) {
        $("#dropdown_" + i).kendoDropDownList({
            //optionLabel: " ",
            dataTextField: "Value",
            dataValueField: "Value",
            dataSource: allSources[i - 1],
            attributes: {
                class: "drop"

            }
        });
    }


}


function OkAndExit() {

    var allValues = [];


    AggregateData(allValues);


    let editor = $("#Explanation").data("kendoEditor");
    let editorValue = "";

    let useTemp = $("#useTemp").text();

    if (useTemp === "true") {

        editorValue = editor.value();

    } else {

        editorValue = $("#Explanation2").val();

    }


    let master = { Guid: $("#Guid").val(), Description: editorValue, PrintedNum: $("#PrintedNum").val(), AnalysisResults: allValues }

    $(".loader").removeClass("hidden");
    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $.ajax({

        type: "Post",
        url: "/AnalysisResultMaster/AddOrUpdate",
        data: {
            __RequestVerificationToken: token.attr('value'),
            AnalysisResultMaster: master
        },

        success: function (response) {
            if (response !== 0) {

                $(".page-content").load("/AnalysisResultMaster/Form", function (responce) {

                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");
                })
            }

        }
    });

}


function Exit() {


    $(".loader").removeClass("hidden");
    $(".page-content").load("/AnalysisResultMaster/Form", function (responce) {

        $(".loader").fadeIn("slow");
        $(".loader").addClass("hidden");
    })

}



function AnalysisResultReport() {

    //let ThisPatientIsDebtorText = ThisPatientIsDebtor + " \n (" + remainAmount + " " + currencyType + ")";

    //if (parseFloat(remainAmount) !== 0) {


    //    bootbox.dialog({
    //        //title: 'A custom dialog with buttons and callbacks',
    //        message: "<h5 class='MyFont-Sarchia-grid'>" + ThisPatientIsDebtorText + "</h5>",
    //        size: 'large',
    //        buttons: {
    //            cancel: {
    //                label: lblcancel,
    //                className: 'btn-danger MyFont-Sarchia-grid',
    //                callback: function () {

    //                }
    //            },
    //            noclose: {
    //                label: lblPrint,
    //                className: 'btn-warning MyFont-Sarchia-grid',
    //                callback: function () {
    //                    PreparingForPrint();

    //                }
    //            },
    //            ok: {
    //                label: lblRecieve,
    //                className: 'btn-info MyFont-Sarchia-grid',
    //                callback: function () {
    //                    var link = "/PatientReceptionReceived/AddNewModal";
    //                    $(".loader").removeClass("hidden");
    //                    $('#PatientReceptionReceivedModal').modal('toggle');
    //                    $('#PatientReceptionReceivedModal-body').load(link, function () {
    //                        $(".loader").fadeIn("slow");
    //                        $(".loader").addClass("hidden");
    //                        $("#PatientReceptionReceivedAmount").val(remainAmount);
    //                    });
    //                }
    //            }
    //        }
    //    });

    //}
    //else {

    //    PreparingForPrint();
    //}


    PreparingForPrint();
}


function PreparingForPrint() {

    let printNum = $("#PrintedNum").val();

    if (printNum === null || printNum === "" || printNum === undefined)
        printNum = 1;
    else
        printNum = parseInt($("#PrintedNum").val()) + 1;

    var allValues = [];

    AggregateData(allValues);

    let editor = $("#Explanation").data("kendoEditor");
    let editorValue = "";

    let useTemp = $("#useTemp").text();

    if (useTemp === "true") {

        editorValue = editor.value();

    } else {

        editorValue = $("#Explanation2").val();

    }

    //if (confirm) {
    let master = { Guid: $("#Guid").val(), Description: editorValue, PrintedNum: printNum, AnalysisResults: allValues, InvoiceDate: $("#InvoiceDate").val(), CreatedDate: $("#CreatedDate").val() }


    $(".loader").removeClass("hidden");
    var token = $(':input:hidden[name*="RequestVerificationToken"]');
    $.ajax({

        type: "Post",
        url: "/AnalysisResultMaster/AddOrUpdate",
        data: {
            __RequestVerificationToken: token.attr('value'),
            AnalysisResultMaster: master
        },

        success: function (response) {
            let id = $("#Guid").val();
            $.ajax({
                url: "/AnalysisResultMaster/PrintAnalysisResultReport",
                type: "Post",
                data: { AnalysisResultMasterId: id },
                success: function (response) {
                    Exit();
                    draw2(response);
                    $(".loader").fadeIn("slow");
                    $(".loader").addClass("hidden");

                },
                error: function (response) {

                }
            });

        }
    });


    //}
    bodyPadding();
}


function keyPressConfigurations() {

    let tab = $("#AnalysisTab").data("kendoTabStrip");
    let allTabs = $(".tabs");

    if (allTabs.length > 0) {
        for (let i = 0; i < allTabs.length; i++) {
            let tabItems = $(allTabs[i]).find("[data-analysisItem]");

            if (tabItems.length > 0) {
                for (let j = 0; j < tabItems.length; j++) {
                    if (j === tabItems.length - 1 && i === allTabs.length - 1) {
                        if ($(tabItems[j]).hasClass('text') || $(tabItems[j]).hasClass('number')) {
                            $(tabItems[j]).keypress(function (e) {
                                if (e.which === 13) {

                                    $('#Explanation').focus();
                                }
                            });
                        }

                        else {
                            if ($(tabItems[j]).hasClass('dropdown')) {
                                $(tabItems[j]).parent().keypress(function (e) {
                                    if (e.which === 13) {
                                        $('#Explanation').focus();
                                    }
                                });
                            }
                        }

                    }
                    else if (j === tabItems.length - 1) {
                        if ($(tabItems[j]).hasClass('text') || $(tabItems[j]).hasClass('number')) {
                            $(tabItems[j]).keypress(function (e) {
                                if (e.which === 13) {
                                    tab.select(i + 1);

                                    let Item = $(allTabs[i + 1]).find("[data-analysisItem]")[0];
                                    if ($(Item).hasClass('text') || $(Item).hasClass('number')) {
                                        $(Item).focus();
                                    }

                                    else {
                                        if ($(Item).hasClass('dropdown')) {
                                            $(Item).data("kendoDropDownList").focus();
                                            $(Item).data("kendoDropDownList").open();

                                        }
                                    }

                                }
                            });
                        }
                        if ($(tabItems[j]).hasClass('dropdown')) {
                            $(tabItems[j]).parent().keypress(function (e) {
                                if (e.which === 13) {
                                    tab.select(i + 1);
                                    let Item = $(allTabs[i + 1]).find("[data-analysisItem]")[0];
                                    if ($(Item).hasClass('text') || $(Item).hasClass('number')) {
                                        $(Item).keypress(function (e) {
                                            if (e.which === 13) {
                                                $(Item).focus();
                                            }
                                        });
                                    }

                                    else {
                                        if ($(Item).hasClass('dropdown')) {
                                            $(Item).data("kendoDropDownList").focus();
                                            $(Item).data("kendoDropDownList").open();

                                        }
                                    }

                                }
                            });
                        }

                    }
                    else {
                        if ($(tabItems[j]).hasClass('text') || $(tabItems[i]).hasClass('number')) {
                            $(tabItems[j]).keypress(function (e) {
                                if (e.which === 13) {
                                    if ($(tabItems[j + 1]).hasClass('text') || $(tabItems[j + 1]).hasClass('number')) {
                                        $(tabItems[j + 1]).focus();
                                    }
                                    if ($(tabItems[j + 1]).hasClass('dropdown')) {
                                        $(tabItems[j + 1]).data("kendoDropDownList").focus();
                                        $(tabItems[j + 1]).data("kendoDropDownList").open();

                                    }

                                }
                            });
                        }

                        else if ($(tabItems[j]).hasClass('dropdown')) {

                            $(tabItems[j]).parent().keypress(function (e) {
                                if (e.which === 13) {
                                    if ($(tabItems[j + 1]).hasClass('text') || $(tabItems[j + 1]).hasClass('number')) {
                                        $(tabItems[j + 1]).focus();
                                    }
                                    if ($(tabItems[j + 1]).hasClass('dropdown')) {
                                        $(tabItems[j + 1]).data("kendoDropDownList").focus();
                                        $(tabItems[j + 1]).data("kendoDropDownList").open();

                                    }
                                }
                            });

                        }
                    }
                }
            }
        }
    }



}


function AggregateData(allValues) {

    let all = $("[data-analysisItem]");

    analysisResultMasterId = $("#Guid").val();
    for (let i = 0; i < all.length; i++) {
        let analysisId = all[i].dataset.analysis;
        let analysisItemId = all[i].dataset.analysisitem;
        let groupAnalysisId = all[i].dataset.group;
        let value = all[i].value;
        let checkBox = document.getElementById(analysisItemId);
        if (value !== "" && value !== null) {
            allValues.push({
                AnalysisResultMasterId: analysisResultMasterId,
                Value: value,
                AnalysisId: analysisId,
                AnalysisItemId: analysisItemId,
                GroupAnalysisId: groupAnalysisId,
                ShowChart: ((checkBox) ? checkBox.checked : false)
            })
        }


    }
}


function bodyPadding() {

    $('body').addClass('body-padding');
}


$('#Explanation').on('keydown', function (e) {

    if (e.which === 9) {
        e.preventDefault();
        $("#okAndExit").focus();

    }

});


$('#okAndExit').on('keydown', function (e) {

    if (e.which === 37) {
        $("#btnExit").focus();
    }

});


$('#btnExit').on('keydown', function (e) {

    if (e.which === 39) {

        $("#okAndExit").focus();
    }
    else if (e.which === 37) {
        $("#btnPrint").focus();
    }

});


$('#btnPrint').on('keydown', function (e) {

    if (e.which === 39) {

        $("#btnExit").focus();
    }


});


function addNewPatientReceptionReceived() {

    let amount = parseFloat($("#PatientReceptionReceivedAmount").val());


    if (amount === 0 || amount === null || amount === undefined || Number.isNaN(amount)) {
        $("#PatientReceptionReceivedAmount-box").removeClass('hidden');
        return;
    }

    let receptionId = $("#PatientReceptionId").val();
    var data = { PatientReceptionId: receptionId, Amount: amount, AmountCurrencyId: $("#PatientReceptionReceivedAmountCorrencyId").val() };
    $(".loader").removeClass("hidden");

    $.ajax({
        type: "Post",
        url: "/PatientReceptionReceived/AddOrUpdate",
        data: data,
        success: function (response) {
            if (response !== 0) {

                $(".loader").fadeIn("slow");
                $(".loader").addClass("hidden");
                remainAmount = remainAmount - amount;
                //AnalysisResultReport();
            }
        }
    });




    $('#PatientReceptionReceivedModal').modal('toggle');
    $('#PatientReceptionReceivedModal-body').empty();




}


function closePatientReceptionReceivedModal() {
    $('#PatientReceptionReceivedModal').modal('toggle');
    $('#PatientReceptionReceivedModal-body').empty();
}





function GoToCustomReception(number) {



    let currentInvoiceNum = parseInt($('#InvoiceNumber').val());
    let LastNum = parseInt($('#LastInvoiceNum').val());
    let FirstNum = parseInt($('#FirstInvoiceNum').val());
    let wantedInvoiceNum = 0;


    if (number === "Next") {

        if (currentInvoiceNum >= LastNum) {

            bootbox.dialog({
                message: "<h5 class = 'MyFont-Sarchia-grid'> This is a Last Reception </h5>",
                className: 'bootbox-class MyFont-Sarchia-grid',
                size: 'small',

            });

            window.setTimeout(function () {
                bootbox.hideAll();
            }, 2000);

            return;
        }

        wantedInvoiceNum = currentInvoiceNum + 1;

        getReception(wantedInvoiceNum);
    }
    else if (number === "Pre") {

        if (currentInvoiceNum === "1") {

            bootbox.dialog({
                message: "<h5 class = 'MyFont-Sarchia-grid'> This is a First Reception </h5>",
                className: 'bootbox-class MyFont-Sarchia-grid',
                size: 'small',

            });

            window.setTimeout(function () {
                bootbox.hideAll();
            }, 2000);


            return;
        }

        wantedInvoiceNum = currentInvoiceNum - 1;
        getReception(wantedInvoiceNum);
    }
    else if (number === "Last") {

        if (currentInvoiceNum === LastNum) {

            bootbox.dialog({
                message: "<h5 class = 'MyFont-Sarchia-grid'> This is a Last Reception </h5>",
                className: 'bootbox-class MyFont-Sarchia-grid',
                size: 'small',

            });

            window.setTimeout(function () {
                bootbox.hideAll();
            }, 2000);


            return;

        }

        wantedInvoiceNum = LastNum;
        getReception(wantedInvoiceNum);
    }
    else if (number === "First") {
        if (currentInvoiceNum === FirstNum) {

            bootbox.dialog({
                message: "<h5 class = 'MyFont-Sarchia-grid'> This is Today's First Reception </h5>",
                className: 'bootbox-class MyFont-Sarchia-grid',
                size: 'small',

            });

            window.setTimeout(function () {
                bootbox.hideAll();
            }, 2000);


            return;

        }

        wantedInvoiceNum = FirstNum;
        getReception(wantedInvoiceNum);
    }



    let num = $('#ReceptionNumber').val();
    if (num === null || num === undefined || num === "") {
        return;
    }

    wantedInvoiceNum = parseInt($('#ReceptionNumber').val());

    if (wantedInvoiceNum >= LastNum) {
        bootbox.alert({
            message: "this reception number not exist",
            className: 'bootbox-class MyFont-Sarchia-grid'

        });
        return;
    }

    if (isNaN(wantedInvoiceNum)) {
        return;
    }

    getReception(wantedInvoiceNum);

}

function draw2(imgData) {

    "use strict";
    var dataUrl = [];
    for (let index = 0; index < imgData.allb.length; index++) {

        //var uInt8Array = imgData.allb[index];
        //var i = uInt8Array.length;
        //var binaryString = [i];
        //while (i--) {
        //    binaryString[i] = String.fromCharCode(uInt8Array[i]);
        //}

        //var data = binaryString.join('');
        //var base64 = window.btoa(data);

        var img = new Image();
        img.src = "data:image/jpeg;base64," + imgData.allb[index];
        dataUrl.push(img.src)


    }
    printJS({ printable: dataUrl, type: 'image' });



}


function getReception(InvoiceNum) {

    bootbox.dialog({
        message: "<h5 class = 'MyFont-Sarchia-grid'> You Dont Save This Analysis Result Do You Want Save It? </h5>",
        className: 'bootbox-class MyFont-Sarchia-grid',
        size: 'medium',
        buttons: {
            cancel: {
                label: "No",
                className: 'MyFont-Roboto-grid',
                callback: function () {
                    $(".loader").removeClass("hidden");
                    $(".page-content").load("/AnalysisResultMaster/GetAnalysisResult?InvoiceNum=" + InvoiceNum, function (responce) {

                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");
                    });
                }
            },

            ok: {
                label: "Yes",
                className: 'MyFont-Roboto-grid',
                callback: function () {

                    let allValues = [];


                    AggregateData(allValues);

                    let editor = $("#Explanation").data("kendoEditor");
                    let editorValue = "";

                    let useTemp = $("#useTemp").text();

                    if (useTemp === "true") {

                        editorValue = editor.value();

                    } else {

                        editorValue = $("#Explanation2").val();

                    }

                    let master = { Guid: $("#Guid").val(), Description: editorValue, PrintedNum: $("#PrintedNum").val(), AnalysisResults: allValues }

                    $(".loader").removeClass("hidden");
                    var token = $(':input:hidden[name*="RequestVerificationToken"]');
                    $.ajax({

                        type: "Post",
                        url: "/AnalysisResultMaster/AddOrUpdate",
                        data: {
                            __RequestVerificationToken: token.attr('value'),
                            AnalysisResultMaster: master
                        },

                        success: function (response) {
                            if (response !== 0) {

                                $(".page-content").load("/AnalysisResultMaster/GetAnalysisResult?InvoiceNum=" + InvoiceNum, function (responce) {


                                    $(".loader").fadeIn("slow");
                                    $(".loader").addClass("hidden");
                                });

                            }

                        }
                    });







                }
            }
        }
    });


}
