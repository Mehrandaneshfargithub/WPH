

var patientAuto;
var invoiceNumAuto;
let priority = 1;
var printed = false;
$(document).ready(function () {

    //CreateDynamicTab(receptionAnalysis);

    let all = $("[data-analysisItem]");

    let date = $("#PatientDateOfBirth").data("kendoDatePicker");
    if ($("#PatientDateOfBirth").val() !== "")
        getAge(kendo.toString(date.value(), 'd'));




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


    keyPressConfigurations();


    if ($(all[0]).hasClass('text') || $(all[0]).hasClass('number')) {
        $(all[0]).focus();
    }
    if ($(all[0]).hasClass('dropdown')) {
        let AmountCurrencyId = $(all[0]).data("kendoDropDownList");
        AmountCurrencyId.focus();
        AmountCurrencyId.open();
    }

    //let tab = $("#AnalysisTab").data("kendoTabStrip").items();
    //let tab1 = $(".tabs");

    //let ff = $(tab1[0]).find("[data-analysisItem]");


    $("select.k-fontSize").children().first().remove();

    let font = $("input.k-fontSize").attr("aria-owns");
    $("#" + font).children().first().remove();
    let ul = $("#" + font).children().eq(4).click();

});

$("#template").on('focus', function (e) {
    let temps = $("#template").data("kendoAutoComplete");
    let value = temps.value();
    temps.search(value);
});

function CreateDynamicTab(allAnalysis) {

    let allSources = [];
    let dataSource = [];
    let analysisItemTabName = "Other Analyses";
    let analysisItemTabContent = "<div class ='tabs col-sm-11'>";
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
            let tabContent = "<div class ='tabs col-sm-11'>";
            if (allAnalysis[i].GroupAnalysis.GroupAnalysis_Analysis !== null && allAnalysis[i].GroupAnalysis.GroupAnalysis_Analysis !== undefined) {
                let analysis = allAnalysis[i].GroupAnalysis.GroupAnalysis_Analysis;

                for (let j = 0; j < analysis.length; j++) {
                    let subContent = "<div class='radius-border row tabs' style = 'padding:1rem;margin:1rem 0;direction:ltr'><h3 class='MyFont-Roboto-grid' style='border-bottom: dotted 1px rgba(0,0,0,0.3);margin-bottom:1rem'>" + analysis[j].Analysis.Name + "</h3>";
                    if (analysis[j].Analysis.Analysis_AnalysisItem !== null && analysis[j].Analysis.Analysis_AnalysisItem !== undefined) {
                        let analysisItem = analysis[j].Analysis.Analysis_AnalysisItem;
                        for (let k = 0; k < analysisItem.length; k++) {
                            if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                                analysisItem[k].AnalysisItem.NormalValues = "";
                            if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                                if (analysisResult.length !== 0) {
                                    if (analysisItem[k].AnalysisId === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);

                                    if (val === null || val === undefined)

                                        subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%'  type='number'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*"   min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] +*/ "'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid' >" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                    else
                                        subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%'  type='number'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + /*" min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] +*/ "'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";


                                }
                                else {
                                    subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%'  type='number'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*"  min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] +*/ "'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                }

                            }
                            else if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {

                                subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional' style='width: 100%;'  data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                let source = [];
                                if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                                    for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                        source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                                    }
                                }
                                allSources.push(source);
                                priority++;
                            }

                            else if (analysisItem[k].AnalysisItem.ValueTypeName === "Text") {
                                if (analysisResult.length !== 0) {
                                    if (analysisItem[k].AnalysisId === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                        analysisItem[k].AnalysisId = null;
                                    let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);
                                    if (val === null || val === undefined)
                                        subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                    else
                                        subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                                }
                                else {
                                    subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysis=" + analysisItem[k].AnalysisId + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                                }
                                //subContent = subContent + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem[k].AnalysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style='width: 100%;' data-group="+ allAnalysis[i].GroupAnalysi.Guid +"  data-analysis="+ analysisItem[k].AnalysisId +"  data-analysisItem="+ analysisItem[k].AnalysisItem.Guid +"    /></div></div>";
                            }
                        }
                    }
                    subContent = subContent + "</div>";
                    tabContent = tabContent + subContent;
                }
            }
            //tabContent = tabContent + content;
            if (allAnalysis[i].GroupAnalysis.GroupAnalysisItems !== null && allAnalysis[i].GroupAnalysis.GroupAnalysisItems !== undefined) {
                let analysisItem = allAnalysis[i].GroupAnalysis.GroupAnalysisItems;
                let content = "";
                for (let k = 0; k < analysisItem.length; k++) {
                    if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                        analysisItem[k].AnalysisItem.NormalValues = "";
                    if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                        if (analysisResult.length !== 0) {

                            if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                analysisItem[k].AnalysisId = null;
                            let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);
                            if (val === null || val === undefined)
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number' style = 'width:100%' title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*" min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"'   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                            else
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name'style = 'transform:translateY(20%)' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number' style = 'width:100%' title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + /*"  min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"'   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        }
                        else {
                            content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number' style = 'width:100%' title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*"   min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                        }
                        //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem[k].AnalysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number'  title='numeric' data-group="+ allAnalysis[i].GroupAnalysi.Guid +"   data-analysisItem="+ analysisItem[k].AnalysisItem.Guid +"  /></div></div>";

                    }
                    else if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown' style = 'width:100%' title='optional'   data-group=" + allAnalysis[i].GroupAnalysis.Guid + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        let source = [];
                        if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                            for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                            }
                        }
                        allSources.push(source);
                        priority++;
                    }

                    else if (analysisItem[k].AnalysisItem.ValueTypeName === "Text") {
                        if (analysisResult.length !== 0) {

                            if (allAnalysis[i].GroupAnalysis.Guid === undefined)
                                analysisItem[k].AnalysisId = null;
                            let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == allAnalysis[i].GroupAnalysis.Guid);
                            if (val === null || val === undefined)
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                            else
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%' title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        }
                        else {
                            content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-group=" + allAnalysis[i].GroupAnalysis.Guid + "  data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                        }
                        //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem[k].AnalysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style='width: 100%;'  data-group="+ allAnalysis[i].GroupAnalysi.Guid +"   data-analysisItem="+ analysisItem[k].AnalysisItem.Guid +"  /></div></div>";

                    }
                }
                tabContent = tabContent + content;
            }

            tabContent = tabContent + "</div>";

            dataSource.push({ text: tabName, content: tabContent, CssClass: "analysis-name" });

        }

        if (allAnalysis[i].Analysis !== null && allAnalysis[i].Analysis !== undefined) {
            let tabName = allAnalysis[i].Analysis.Name;
            let tabContent = "<div class ='tabs col-sm-11'>";
            let content = "";
            if (allAnalysis[i].Analysis.Analysis_AnalysisItem !== null && allAnalysis[i].Analysis.Analysis_AnalysisItem !== undefined) {
                let analysisItem = allAnalysis[i].Analysis.Analysis_AnalysisItem;

                for (let k = 0; k < analysisItem.length; k++) {
                    if (analysisItem[k].AnalysisItem.NormalValues === "null" || analysisItem[k].AnalysisItem.NormalValues === null || analysisItem[k].AnalysisItem.NormalValues === "NULL")
                        analysisItem[k].AnalysisItem.NormalValues = "";
                    if (analysisItem[k].AnalysisItem.ValueTypeName === "Numerical") {
                        //content = content + "<div class='col-sm-11'><h4>" + analysisItem[k].AnalysisItem.Name + "</h4><input class = 'number' type='number'  title='numeric'  style='width: 100%;' /></div>";

                        if (analysisResult.length !== 0) {
                            if (allAnalysis[i].AnalysisId === undefined)
                                allAnalysis[i].AnalysisId = null;

                            let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == null);
                            if (val === null || val === undefined)
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%' type='number'  title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*"  min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                            else
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)' >" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%' type='number'  title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + /*"  min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        }
                        else {
                            content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number'  style = 'width:100%'   type='number'  title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + /*" min = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem[k].AnalysisItem.AnalysisItemMinMaxValues[maxVal] + */"' /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                        }

                        //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem[k].AnalysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number'  title='numeric' data-analysis=" + allAnalysis[i].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div></div>";

                    }
                    else if (analysisItem[k].AnalysisItem.ValueTypeName === "Optional") {
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional' style='width: 100%;' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        let source = [];
                        if (analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== null && analysisItem[k].AnalysisItem.AnalysisItemValuesRanges !== undefined) {

                            for (let range = 0; range < analysisItem[k].AnalysisItem.AnalysisItemValuesRanges.length; range++) {
                                source.push(analysisItem[k].AnalysisItem.AnalysisItemValuesRanges[range]);
                            }
                        }
                        allSources.push(source);
                        priority++;
                    }

                    else if (analysisItem[k].AnalysisItem.ValueTypeName === "Text") {
                        if (analysisResult.length !== 0) {
                            if (allAnalysis[i].AnalysisId === undefined)
                                allAnalysis[i].AnalysisId = null;

                            let val = analysisResult.find(x => x.AnalysisId == analysisItem[k].AnalysisId && x.AnalysisItemId == analysisItem[k].AnalysisItem.Guid && x.GroupAnalysisId == null);
                            if (val === null || val === undefined)
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text'  style = 'width:100%' title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                            else
                                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  value = " + val.Value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";

                        }
                        else {
                            content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem[k].AnalysisItem.Name + " (" + analysisItem[k].AnalysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric' data-analysis=" + analysisItem[k].AnalysisId + "   data-analysisItem=" + analysisItem[k].AnalysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem[k].AnalysisItem.NormalValues + "</h4></div></div>";
                        }
                        //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem[k].AnalysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style='width: 100%;' data-analysis="+ allAnalysis[i].AnalysisId +"   data-analysisItem="+ analysisItem[k].AnalysisItem.Guid +"    /></div></div>";

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
            let content = "";
            if (analysisItem.ValueTypeName === "Numerical") {
                if (analysisResult.length !== 0) {

                    let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem.Guid && x.GroupAnalysisId == null);

                    if (val === null || val === undefined)
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number' style = 'width:100%' title='numeric'  data-analysisItem=" + analysisItem.Guid + /*"  min = '" + analysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";
                    else
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number' style = 'width:100%' title='numeric'  data-analysisItem=" + analysisItem.Guid + "  value = " + val.Value + /*"  min = '" + analysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left '><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";

                }
                else {
                    content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' style = 'width:100%'  type='number'  title='numeric'    data-analysisItem=" + analysisItem.Guid + /*"  min = '" + analysisItem.AnalysisItemMinMaxValues[minVal] + "' max = '" + analysisItem.AnalysisItemMinMaxValues[maxVal] + */"'  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";
                }
                //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'number' type='number'  title='numeric' data-analysisItem="+ analysisItem.Guid +"  /></div></div>";

            }
            else if (analysisItem.ValueTypeName === "Optional") {
                content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input  id = 'dropdown_" + priority + "'  class = 'dropdown'  style = 'width:100%'  title='optional'   data-analysisItem=" + analysisItem.Guid + "   /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";

                let source = [];
                if (analysisItem.AnalysisItemValuesRanges !== null && analysisItem.AnalysisItemValuesRanges !== undefined) {

                    for (let range = 0; range < analysisItem.AnalysisItemValuesRanges.length; range++) {
                        source.push(analysisItem.AnalysisItemValuesRanges[range]);
                    }
                }
                allSources.push(source);
                priority++;
            }

            else if (analysisItem.ValueTypeName === "Text") {
                if (analysisResult.length !== 0) {

                    let val = analysisResult.find(x => x.AnalysisId == null && x.AnalysisItemId == analysisItem.Guid && x.GroupAnalysisId == null);
                    if (val === null || val === undefined)
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric'    data-analysisItem=" + analysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";
                    else
                        content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%' title='numeric'  data-analysisItem=" + analysisItem.Guid + "  value = " + val.Value + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";

                }
                else {
                    content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-4 pull-left'><span class='pull-right analysis-name' style = 'transform:translateY(20%)'>" + analysisItem.Name + " (" + analysisItem.Unit.Name + ")" + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style = 'width:100%'  title='numeric'    data-analysisItem=" + analysisItem.Guid + "  /></div><div class = 'col-sm-4 pull-left'><h4 class='pull-left MyFont-Roboto-grid'>" + analysisItem.NormalValues + "</h4></div></div>";
                }
                //content = content + "<div class='col-sm-12' style = 'font-size:1.5rem;margin:0.5rem 0'><div class = 'col-sm-2 pull-left'><span class='pull-right'>" + analysisItem.Name + "</span></div><div class = 'col-sm-2 pull-left' ><input class = 'text' style='width: 100%;'  data-analysisItem="+ analysisItem.Guid +"   /></div></div>";

            }
            analysisItemTabContent = analysisItemTabContent + content;
            //dataSource.push({ text: analysisItemTabName, content: analysisItemTabContent, CssClass: "analysis-name" });

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
    var rrr = $(".rrr");


    for (let i = 1; i < priority; i++) {
        $("#dropdown_" + i).kendoDropDownList({
            optionLabel: " ",
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

    let master = { Guid: $("#Guid").val(), Description: $("#Explanation").val(), PrintedNum: $("#PrintedNum").val(), AnalysisResults: allValues }

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

    let ThisPatientIsDebtorText = ThisPatientIsDebtor + " \n (" + remainAmount + " " + currencyType + ")";

    if (parseFloat(remainAmount) !== 0) {


        bootbox.dialog({
            //title: 'A custom dialog with buttons and callbacks',
            message: "<h5 class='MyFont-Sarchia-grid'>" + ThisPatientIsDebtorText + "</h5>",
            size: 'large',
            buttons: {
                cancel: {
                    label: lblcancel,
                    className: 'btn-danger MyFont-Sarchia-grid',
                    callback: function () {

                    }
                },
                noclose: {
                    label: lblPrint,
                    className: 'btn-warning MyFont-Sarchia-grid',
                    callback: function () {
                        PreparingForPrint();

                    }
                },
                ok: {
                    label: lblRecieve,
                    className: 'btn-info MyFont-Sarchia-grid',
                    callback: function () {
                        var link = "/PatientReceptionReceived/AddNewModal";
                        $(".loader").removeClass("hidden");
                        $('#PatientReceptionReceivedModal').modal('toggle');
                        $('#PatientReceptionReceivedModal-body').load(link, function () {
                            $(".loader").fadeIn("slow");
                            $(".loader").addClass("hidden");
                            $("#PatientReceptionReceivedAmount").val(remainAmount);
                        });
                    }
                }
            }
        });

    }
    else {

        PreparingForPrint();
    }



}


function PreparingForPrint() {

    let printNum = $("#PrintedNum").val();

    if (printNum === null || printNum === "" || printNum === undefined)
        printNum = 1;
    else
        printNum = parseInt($("#PrintedNum").val()) + 1;

    var allValues = [];

    let confirm = AggregateData(allValues);

    if (confirm) {
        let master = { Guid: $("#Guid").val(), Description: $("#Explanation").val(), PrintedNum: printNum, AnalysisResults: allValues, InvoiceDate: $("#InvoiceDate").val(), CreatedDate: $("#CreatedDate").val() }


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
                    url: "/AnalysisResultMaster/PrintAnalysisResultReportRadiology",
                    type: "Post",
                    data: { AnalysisResultMasterId: id },
                    success: function (response) {
                        Exit();
                        draw2(response);
                        $(".loader").fadeIn("slow");
                        $(".loader").addClass("hidden");

                    },
                    error: function (response) {
                        console.log(response);
                        console.log(response.data);
                        console.log(response.error);
                    }
                });

            }
        });


    }
    bodyPadding();
}


function keyPressConfigurations() {



    let tab = $("#AnalysisTab").data("kendoTabStrip");
    let allTabs = $(".tabs");

    //let ff = $(tab1[0]).find("[data-analysisItem]");


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
                        if ($(tabItems[j]).hasClass('text') || $(all[i]).hasClass('number')) {
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

    //let all = $("[data-analysisItem]");

    //for (let i = 0; i < all.length; i++) {

    //    if (i === all.length-1) {
    //        if ($(all[i]).hasClass('text') || $(all[i]).hasClass('number')) {
    //            $(all[i]).keypress(function (e) {
    //                if (e.which === 13) {
    //                    $('#Explanation').focus();
    //                }
    //            });
    //        }

    //        else {
    //            if ($(all[i]).hasClass('dropdown')) {
    //                $(all[i]).parent().keypress(function (e) {
    //                    if (e.which === 13) {
    //                        $('#Explanation').focus();
    //                    }
    //                });
    //            }
    //        }

    //    }

    //        else {

    //            if ($(all[i]).hasClass('text')) {

    //                if ($(all[i + 1]).hasClass('text') || $(all[i + 1]).hasClass('number')) {
    //                    $(all[i]).keypress(function (e) {
    //                        if (e.which === 13) {
    //                            $(all[i + 1]).focus();
    //                        }
    //                    });
    //                }
    //                if ($(all[i + 1]).hasClass('dropdown')) {
    //                    $(all[i]).keypress(function (e) {
    //                        if (e.which === 13) {

    //                            let AmountCurrencyId = $(all[i + 1]).data("kendoDropDownList");
    //                            AmountCurrencyId.focus();
    //                            AmountCurrencyId.open();
    //                        }
    //                    });
    //                }

    //            }
    //            else if ($(all[i]).hasClass('number')) {
    //                if ($(all[i + 1]).hasClass('text') || $(all[i + 1]).hasClass('number')) {
    //                    $(all[i]).keypress(function (e) {
    //                        if (e.which === 13) {
    //                            $(all[i + 1]).focus();
    //                        }
    //                    });
    //                }
    //                if ($(all[i + 1]).hasClass('dropdown')) {
    //                    $(all[i]).keypress(function (e) {
    //                        if (e.which === 13) {

    //                            let AmountCurrencyId = $(all[i + 1]).data("kendoDropDownList");
    //                            AmountCurrencyId.focus();
    //                            AmountCurrencyId.open();
    //                        }
    //                    });
    //                }
    //            }
    //            else if ($(all[i]).hasClass('dropdown')) {

    //                if ($(all[i + 1]).hasClass('text') || $(all[i + 1]).hasClass('number')) {
    //                    $(all[i]).parent().keypress(function (e) {
    //                        if (e.which === 13) {
    //                            $(all[i + 1]).focus();
    //                        }
    //                    });
    //                }
    //                if ($(all[i + 1]).hasClass('dropdown')) {
    //                    $(all[i]).parent().keypress(function (e) {
    //                        if (e.which === 13) {

    //                            let AmountCurrencyId = $(all[i + 1]).data("kendoDropDownList");
    //                            AmountCurrencyId.focus();
    //                            AmountCurrencyId.open();
    //                        }
    //                    });
    //                }
    //            }




    //        }

    //}

}


function AggregateData(allValues) {


    //var allValues = [];
    let analysisResultMasterId = $("#Guid").val();

    let allgroup = $(".groupAnalysesId");

    for (let i = 0; i < allgroup.length; i++) {
        let analysisId = null;
        let analysisItemId = null;
        let groupAnalysisId = $(allgroup[i]).text();
        let editor = $("#editor").data("kendoEditor");
        let value = editor.value();
        if (value !== "" && value !== null) {
            allValues.push({
                AnalysisResultMasterId: analysisResultMasterId,
                Value: value,
                AnalysisId: analysisId,
                AnalysisItemId: analysisItemId,
                GroupAnalysisId: groupAnalysisId
            })
        }


    }

    let allanalysis = $(".analysesId");

    for (let i = 0; i < allanalysis.length; i++) {
        let analysisId = $(allanalysis[i]).text();
        let analysisItemId = null;
        let groupAnalysisId = null;
        let editor = $("#editor").data("kendoEditor");
        let value = editor.value();
        if (value !== "" && value !== null) {
            allValues.push({
                AnalysisResultMasterId: analysisResultMasterId,
                Value: value,
                AnalysisId: analysisId,
                AnalysisItemId: analysisItemId,
                GroupAnalysisId: groupAnalysisId
            })
        }


    }

    let allanalysisItem = $(".analysesItemId");

    for (let i = 0; i < allanalysisItem.length; i++) {
        let analysisId = null;
        let analysisItemId = $(allanalysisItem[i]).text();
        let groupAnalysisId = null;
        let editor = $("#editor").data("kendoEditor");
        let value = editor.value();
        if (value !== "" && value !== null) {
            allValues.push({
                AnalysisResultMasterId: analysisResultMasterId,
                Value: value,
                AnalysisId: analysisId,
                AnalysisItemId: analysisItemId,
                GroupAnalysisId: groupAnalysisId
            })
        }


    }

    if (allValues.length < (allgroup.length + allanalysis.length + allanalysisItem.length)) {
        bootbox.dialog({
            message: "<h5 class = 'MyFont-Sarchia-grid'> Please Fill All Of The Analysis </h5>",
            className: 'bootbox-class MyFont-Sarchia-grid',
            size: 'small',

        });

        window.setTimeout(function () {
            bootbox.hideAll();
        }, 2000);

        bodyPadding();
        return false;
    }
    return true;



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
                AnalysisResultReport();
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

                    let master = { Guid: $("#Guid").val(), Description: $("#Explanation").val(), PrintedNum: $("#PrintedNum").val(), AnalysisResults: allValues }

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
