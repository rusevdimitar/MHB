//function FlagSum(id) {
//    var xmlhttp;

//    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
//        xmlhttp = new XMLHttpRequest();
//    }
//    else {// code for IE6, IE5
//        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
//    }

//    xmlhttp.open("GET", "FlagExpense.aspx?id=" + id, true);
//    xmlhttp.send();
//}
function ScrollToEditedProduct(productID) {
    //$('span[name=' + productID + ']');

    //location.hash = "#" + productID;
}

function remote2(url) {
    window.opener.location = url
}

function SelectAllIsPaid(id) {
    var frm = document.forms[0];

    for (i = 0; i < frm.elements.length; i++) {
        if (frm.elements[i].type == "checkbox" && frm.elements[i].id.indexOf("IsPaid") != -1) {
            frm.elements[i].checked = document.getElementById(id).checked;
        }
    }
}
function SelectAllRows(id) {
    var frm = document.forms[0];

    for (i = 0; i < frm.elements.length; i++) {
        if (frm.elements[i].type == "checkbox" && frm.elements[i].id.indexOf("SelectRow") != -1) {
            frm.elements[i].checked = document.getElementById(id).checked;
        }
    }
}
function EnableAddToSumDiv(sumDivID) {
    //document.getElementById(sumDivID).style.display = '';

    $("[id$='" + sumDivID + "']").show("slow");
}

function AddToCurrentSum(txtSum, txtAddition, divAdditionID) {
    $("[id$='" + divAdditionID + "']").hide("slow");

    var currSum = document.getElementById(txtSum).value;
    var addition = document.getElementById(txtAddition).value;
    currSum = currSum * 1;

    addition = addition * 1;
    var sum = (currSum + addition);

    document.getElementById(txtSum).value = sum;
}

var operation = true;

function RevertToLastSum(controlID_NEW_val, controlID_OLD_val, controlID_Current_val, controlID_Operation) {
    var txtNewValue = $("[id$='" + controlID_NEW_val + "']");
    var txtOldValue = $("[id$='" + controlID_OLD_val + "']");
    var txtCurrentValue = $("[id$='" + controlID_Current_val + "']");

    if (operation == true) {
        txtNewValue.attr("value", txtOldValue.attr("value"));
        operation = false;
    } else if (operation == false) {
        txtNewValue.attr("value", txtCurrentValue.attr("value"));
        operation = true;
    }
}

function GridUndoIconHover(controlID) {
    var undoIcon = $("[id$='" + controlID + "']");

    //undoIcon.attr("src", "../Images/undo_grid_icon_hover.gif")
    undoIcon.fadeTo("slow", 1.0)
}

function GridUndoIconBlur(controlID) {
    var undoIcon = $("[id$='" + controlID + "']");

    //undoIcon.attr("src", "../Images/undo_grid_icon.gif")

    undoIcon.fadeTo("slow", 0.2)
}

function FadeOutUndoIcons() {
    var imgs, i;
    imgs = document.getElementsByTagName('img');
    for (i in imgs) {
        if (imgs[i].id != null) {
            if (imgs[i].id.indexOf("RevertToLastSavedSum") != -1) {
                $("[id$='" + imgs[i].id + "']").fadeTo("slow", 0.2);
            }
        }
    }
}

//function SelectRow(rowid, chkid, chkispaid) {
//    var checked = $("[id$='" + chkid + "']").attr("checked");

//    if (!checked) {
//        //$("[id$='" + chkid + "']").attr("checked", false);

//        $("[id$='" + rowid + "']").removeClass("MarkedRow");
//        $("[id$='" + rowid + "']").find("input").each(function () {
//            $(this).removeClass("MarkedRow");
//            $(this).addClass("GridCells");
//        });
//        $("[id$='" + rowid + "']").find("[id$='TextBoxFieldExpectedValue']").removeClass("GridCells");
//        $("[id$='" + rowid + "']").find("[id$='TextBoxFieldValue']").removeClass("GridCells");
//        $("[id$='" + rowid + "']").find("[id$='TextBoxFieldExpectedValue']").addClass("GridExpectedCostCells");
//        $("[id$='" + rowid + "']").find("[id$='TextBoxFieldValue']").addClass("GridActualCostCells");

//        //var fieldValue = $("[id$='" + rowid + "']").find("[id$='TextBoxFieldValue']").val();
//        $("[id$='" + chkid + "']").removeClass("GridCells");
//        $("[id$='" + chkispaid + "']").removeClass("GridCells");
//    } else {
//        //$("[id$='" + chkid + "']").attr("checked", true);

//        $("[id$='" + rowid + "']").addClass("MarkedRow");
//        $("[id$='" + rowid + "']").find("input").each(function () {
//            $(this).removeClass("GridCells");
//            $(this).removeClass("GridActualCostCells");
//            $(this).removeClass("GridExpectedCostCells");
//            $(this).addClass("MarkedRow");
//        });

//        $("[id$='" + chkid + "']").removeClass("MarkedRow");
//        $("[id$='" + chkispaid + "']").removeClass("MarkedRow");

//        //var fieldValue = $("[id$='" + rowid + "']").find("[id$='TextBoxFieldValue']").val();
//    }
//}

function ResetGridCellSum() {
    $("[id$=HiddenSumSelectedGridCells]").val(0);
}

function AddCellValue(ctlID) {
    var sum = $("[id$=HiddenSumSelectedGridCells]");

    var sumSpan = $("[id$='spanSelectedSum']");
    var ctl = $("[id$='" + ctlID + "']");

    var ctlOriginalWidth = ctl.width();

    var isCalc = ctl.attr("isCalc");

    if (isCalc) {
        sum.val(parseFloat(sum.val()) + parseFloat(ctl.val()));
        ctl.removeAttr("isCalc");
        ctl.css({ "background-color": "#333", "color": "#FFF" });
    } else {
        sum.val(parseFloat(sum.val()) - parseFloat(ctl.val()));
        ctl.attr("isCalc", true);
        ctl.removeAttr("style");
        ctl.width(ctlOriginalWidth);
    }

    sumSpan.text(parseFloat(sum.val()).toFixed(2));
}

var _fancyboxPostBackControl;

function CloseFancyBoxAndSubmit(sender, e) {
    _fancyboxPostBackControl = sender;
    $.fancybox.close();
}

function CloseFancyBox() {
    _fancyboxPostBackControl = null;
    $.fancybox.close();
}

function ShowTodaySpentList() {
    var divTodaySpentList = $('#DivTodaySpentList');

    divTodaySpentList.dialog(
    {
        width: '500px',
        title: ''
    });

    divTodaySpentList.parent().appendTo(jQuery("form:first"));
}

function ShowPurchaseHistoryDateSelector() {
    var divPurchaseHistoryDateList = $('#DivPurchaseHistoryDateSelector');

    divPurchaseHistoryDateList.dialog(
    {
        width: '160px',
        title: ''
    });

    divPurchaseHistoryDateList.parent().appendTo(jQuery("form:first"));
}

function ShowPurchaseHistoryList() {
    var divPurchaseHistoryDateList = $('#DivPurchaseHistory');

    divPurchaseHistoryDateList.dialog(
    {
        width: '550px',
        title: ''
    });

    divPurchaseHistoryDateList.parent().appendTo(jQuery("form:first"));
}

function MoveDetailsToNewParent(name) {
    var chk = $("[id*=GridViewDetails]").find('input:checkbox:checked:first');

    if (chk.attr('id') != undefined) {
        var panelDetailsSelectParent = $('#MoveDetailsSelectParent');

        panelDetailsSelectParent.dialog(
        {
            width: '270px',
            title: name
        });

        panelDetailsSelectParent.parent().appendTo(jQuery("form:first"));

        return false;
    }
    else {
        return true;
    }
}

function ShowDetailsTable(name, pickASupplierPopupTitle, showSupplierPopup) {
    var panelDetailsTable = $('#PanelDetailsTable');

    panelDetailsTable.dialog(
    {
        width: '1300px',
        title: name
    });

    $("[id*=TextBoxDetailNameNew]").val("");
    $("[id*=TextBoxDetailDescriptionNew]").val("");
    $("[id*=TextBoxDetailValueNew]").val("");

    panelDetailsTable.parent().appendTo(jQuery("form:first"));

    if (showSupplierPopup == true) {
        ShowNewDetailsSupplierSelectorDiv(pickASupplierPopupTitle);
    }
}

function ShowEditControlTranslationPopup(controlID, currentTranslation) {
    var panelEditControlTranslation = $('#PanelEditControlTranslation');

    panelEditControlTranslation.dialog(
    {
        width: '420px',
        title: name
    });

    $("[id$=TextBoxControlID]").val(controlID);
    $("[id$=TextBoxCurrentTranslation]").val(currentTranslation);
    $("[id$=TextBoxNewTranslation]").val(currentTranslation);

    panelEditControlTranslation.parent().appendTo(jQuery("form:first"));
}

function ShowDivWeightVolumeSelector() {
    var panelWeightVolumeSelector = $('#DivWeightVolumeSelector');

    var textBoxNewName = $("[id$=TextBoxDetailNameNew]");

    if (textBoxNewName.val().length == 0) {
        var amount = $("[id$=TextBoxDetailAmountNew]").val();

        if (amount % 1 != 0) // if decimal value
        {
            $('[id$=RadioButtonListWeightVolumeSelector] input[value=1]').prop('checked', true);
        }
        else {
            $('[id$=RadioButtonListWeightVolumeSelector] input[value=3]').prop('checked', true);
        }
    }

    panelWeightVolumeSelector.dialog(
    {
        width: 'auto',
        title: name,
        close: function (event, ui) {
            textBoxNewName.focus();
            //textBoxNewName.select();
        },
        open: function (event, ui) {
            setTimeout(function () {
                CloseDivWeightVolumeSelector(false);
            }, 5000);
        }
    });

    panelWeightVolumeSelector.parent().appendTo(jQuery("form:first"));
}

function SelectDetailsNameInput() {
    var textBoxNewName = $("[id$=TextBoxDetailNameNew]");

    var textBoxNewAmount = $("[id$=TextBoxDetailAmountNew]");

    if (textBoxNewAmount.val().length == 5) {
        textBoxNewName.focus();
        return false;
    }

    return true;
}

function CloseDivWeightVolumeSelector(focusInputs) {
    $('#DivWeightVolumeSelector').dialog('close');
    $("[id$=HiddenDetailsMeasureTypeID]").val($('[id$=RadioButtonListWeightVolumeSelector] input:checked').val());

    if (focusInputs) {
        var textBoxNewName = $("[id$=TextBoxDetailNameNew]");

        textBoxNewName.focus();
        textBoxNewName.select();
    }
}

// DETAILS ATTACH
function ShowDetailsAttach(name) {
    var chk = $("[id*=GridViewDetails]").find('input:checkbox:checked:first');

    if (chk.attr('id') != undefined) {
        $("[id*=HiddenAttachingToDetailsTable]").val(true);

        var panelDetailsAttach = $('#DivAttach');

        panelDetailsAttach.dialog(
            {
                width: 'auto',
                title: name
            });

        panelDetailsAttach.parent().appendTo(jQuery("form:first"));
    }
    else {
        $("[id*=HiddenAttachingToDetailsTable]").val(false);
        alert('Please select a row!');
    }
}

// MAIN ATTACH
function ShowMainTableAttach(name) {
    var chk = $("[id*=GridView1]").find('input[id*=CheckBoxSelectRow]:checked:first');

    if (chk.attr('id') != undefined) {
        $("[id*=HiddenAttachingToDetailsTable]").val(false);

        var panelDetailsAttach = $('#DivAttach');

        panelDetailsAttach.dialog(
            {
                width: 'auto',
                title: name
            });

        panelDetailsAttach.parent().appendTo(jQuery("form:first"));
    }
    else {
        alert('Please select a row!');
    }
}

// PREVIEW ATTACH
function ShowPreviewAttach(previewDivID, previewImgID, expenditureID, expenseName) {
    var panelPreviewAttach = $('#' + previewDivID);

    panelPreviewAttach.dialog(
        {
            width: 'auto',
            title: expenseName,
        });

    var imgPreviewAttachment = $('#' + previewImgID);

    imgPreviewAttachment.attr('src', 'PreviewAttachment.aspx?id=' + expenditureID + '&attachingToDetails=0');

    panelPreviewAttach.parent().appendTo(jQuery("form:first"));
}

function ShowNewSupplierTable(name) {
    var panelNewSupplier = $('[id$=PanelAddNewSupplier]');

    panelNewSupplier.dialog(
        {
            width: 'auto',
            title: name,
            show: "fade",
            hide: "explode"
        });

    panelNewSupplier.parent().appendTo(jQuery("form:first"));
}

function ShowPieChartProductsPopup(name) {
    var pieChartProducts = $('#PanelPieChartProducts');

    pieChartProducts.dialog(
        {
            width: 'auto',
            title: name
        });

    pieChartProducts.parent().appendTo(jQuery("form:first"));
}

function ShowPieChartSuppliersPopup(name) {
    var pieChartSuppliers = $('#PanelPieChartSuppliers');

    pieChartSuppliers.dialog(
        {
            width: 'auto',
            title: name
        });

    pieChartSuppliers.parent().appendTo(jQuery("form:first"));
}

function ShowPieChartCategoriesPopup(name) {
    var pieChartCategories = $('#PanelPieChartCategories');

    pieChartCategories.dialog(
        {
            width: 'auto',
            title: name
        });

    pieChartCategories.parent().appendTo(jQuery("form:first"));
}

function ShowNewProductsDiv(name, detailID) {
    $("[id*=HiddenDetailsRowID]").val(detailID);

    var panelNewProductDiv = $('#DivEnterNewProductDetails');

    panelNewProductDiv.dialog(
        {
            width: 'auto',
            title: name
        });

    panelNewProductDiv.parent().appendTo(jQuery("form:first"));

    // Set drop down suppliers selected supplier
    //$("[id$=DropDownListNewProductSupplier]").val($("[id$=HiddenSelectedSupplier]").val());
}

function SetSupplierIdHiddenField(supplierId, supplierName, buttonId) {
    var buttonSupplier = $("[id$=" + buttonId + "]");

    if (buttonSupplier.attr("class") == "ButtonAddInsert") {
        buttonSupplier.removeClass("ButtonAddInsert").addClass("ButtonAddMedium");
        $("[id*=HiddenSelectedSupplier]").val(1);
        $('#LinkButtonSelectedDetailsSupplier').text("Click to pick supplier");
        $('#PanelDetailsTable').dialog('option', 'title', "Please select a supplier");
    }
    else {
        $("[id*=ButtonSelectedDetailsSupplier]").removeClass("ButtonAddInsert").addClass("ButtonAddMedium");
        buttonSupplier.removeClass("ButtonAddMedium").addClass("ButtonAddInsert");
        $("[id*=HiddenSelectedSupplier]").val(supplierId);
        $('#PanelDetailsTable').dialog('option', 'title', supplierName);
        $('#LinkButtonSelectedDetailsSupplier').text(supplierName);
    }

    CloseSupplierPopup();

    var selected = new Array();
    $("[id$=CheckBoxDetailsTableSelect]").each(function () {
        if (this.checked == true) {
            selected.push($(this).closest('span').attr('detailid'));
        }
    });

    if (selected.length > 0) {
        __doPostBack('BatchSetDetailsSupplier', selected.join(","));
    }
}

function ShowNewDetailsSupplierSelectorDiv(name) {
    var divPickNewDetailsSupplier = $('#DivPickNewDetailsSupplier');

    divPickNewDetailsSupplier.dialog(
        {
            width: 'auto',
            title: name,
            open: function (event, ui) {
                setTimeout(function () {
                    divPickNewDetailsSupplier.dialog('close');
                }, 10000);
            }
        });

    divPickNewDetailsSupplier.parent().appendTo(jQuery("form:first"));
}

function CloseSupplierPopup() {
    $('#DivPickNewDetailsSupplier').dialog('close');
}

function CloseCreateNewSupplierDialog() {
    $('[id$=PanelAddNewSupplier]').dialog('close');

    $('[id$=TextBoxNewSupplierName]').val('')

    $('[id$=TextBoxNewSupplierDescription]').val('')
    $('[id$=TextBoxNewSupplierAddress]').val('')
    $('[id$=CheckBoxNewSupplierPreffered]').attr('checked', false)
    $('[id$=CheckBoxNewSupplierActive]').attr('checked', false)
    $('[id$=TextBoxNewSupplierWebsite]').val('')
}

function ShowCategoriesCommentsTable(name) {
    var panelCommentsTable = $('#PanelCategoriesCommentsTable');

    panelCommentsTable.dialog(
        {
            width: 'auto',
            title: name
        });

    panelCommentsTable.parent().appendTo(jQuery("form:first"));
}

function ShowSearchPanel() {
    var panelSearch = $('#PanelSearch');

    panelSearch.dialog(
       {
           width: 'auto'
       });

    panelSearch.parent().appendTo(jQuery("form:first"));
}

function ShowNotesPanel() {
    var panelNotes = $('#PanelNotes');

    panelNotes.dialog(
    {
        width: 'auto',
        open: function () {
            $('note').readmore({ maxHeight: 50 });
            $('#NotesPreview').accordion({
                collapsible: true,
                active: false,
                beforeActivate: function (event, ui) {
                    __doPostBack($("[id$=DataGridNotesPreviews]").attr("id"), '');
                }
            });
        }
    });

    panelNotes.parent().appendTo(jQuery("form:first"));
}

function ShowProductDetailsPanel() {
    var panelProductDetails = $('#DivProductDetails');

    panelProductDetails.dialog(
    {
        width: 'auto'
    });

    panelProductDetails.parent().appendTo(jQuery("form:first"));
}

function ShowCalculatorPanel() {
    var panelCalculator = $('#PanelCalculator');

    panelCalculator.dialog(
       {
           width: 'auto'
       });

    panelCalculator.parent().appendTo(jQuery("form:first"));

    //$(".ui-dialog-titlebar").hide();
}

function ApplySelect2Style(names) {
    var namesList = names.replace(" ", "").split(",");

    for (var i = 0; i < namesList.length; i++) {
        $("[id*='" + namesList[i] + "']").select2();
    }
}

function DisplayShoppingListDialog(productName) {
    document.getElementById('LabelShoppingListProductName').innerHTML = productName;

    $('<a href="#ShoppingListAmount">Enter amount</a>').fancybox({
        modal: true,
        fitToView: true,
        width: 300,
        height: 100,
        autoSize: false,
        closeClick: true,
        openEffect: 'fade',
        closeEffect: 'elastic',
        afterClose: function () {
            __doPostBack($(_fancyboxPostBackControl).attr('name'), '');
            //parent.location.reload(true);
        }
    }).click();

    $("[id$=TextBoxAddToShoppingListAmount]").val("1");
    $("[id$=TextBoxAddToShoppingListAmount]").focus();
}

$(document).ready(function () {
    //$("[id*=ButtonDetailsPrintShoppingList]").easyconfirm(
    //{
    //    locale: {
    //        title: 'Multiple records of a same product detected. ',
    //        text: 'Include all entries or include a product just once?',
    //        button: ['Include all product entries', 'Include selected products once in the list'],
    //        closeText: 'Close'
    //    }
    //});

    //$(document).keyup(function (e) {
    //    if (e.keyCode == 27) { $('#DivWeightVolumeSelector').close(); }   // esc
    //});

    //$("[id*=ButtonDetailsPrintShoppingList]").click(function () {
    //    confirmSelectDetailsPrintShoppingListMode();
    //    return false;
    //});

    //$("[id$=ButtonDetailsPrintShoppingListBottom]").click(function () {
    //    confirmSelectDetailsPrintShoppingListMode();
    //    return false;
    //});

    $(document).keydown(function (e) {
        var keyCode = e.keyCode || e.which, arrow = { left: 37, up: 38, right: 39, down: 40 };

        if ($('#DivWeightVolumeSelector').is(":visible")) {
            var currentSelectedValue = $('[id$=RadioButtonListWeightVolumeSelector] input:checked').val();

            switch (keyCode) {
                case arrow.left:
                    //..
                    break;
                case arrow.up:
                    if (currentSelectedValue == 1) {
                        currentSelectedValue = 3;
                    } else { currentSelectedValue = currentSelectedValue - 1; }

                    $('[id$=RadioButtonListWeightVolumeSelector] input[value=' + currentSelectedValue + ']').prop('checked', true);
                    break;
                case arrow.right:
                    //..
                    break;
                case arrow.down:
                    if (currentSelectedValue == 3) {
                        currentSelectedValue = 1;
                    } else { currentSelectedValue++; }

                    $('[id$=RadioButtonListWeightVolumeSelector] input[value=' + currentSelectedValue + ']').prop('checked', true);

                    break;
            }
        }

        if ($('[id$=TextBoxDetailNameNew]').is(":focus") && keyCode != arrow.up && keyCode != arrow.down) {
            CloseDivWeightVolumeSelector(false);
        }
    });

    $(document).bind('keydown', 'F1', function () {
        var testBoxDetailAmount = $('[id$=TextBoxDetailAmountNew]');

        testBoxDetailAmount.focus();
        testBoxDetailAmount.select();
    });

    $(document).bind('keydown', 'F2', function () {
        ShowDivWeightVolumeSelector();
    });

    $(document).bind('keydown', 'F3', function () {
        ShowNewDetailsSupplierSelectorDiv();
    });

    $(document).bind('keydown', 'F4', function () {
        ShowDetailsTable();
    });

    $(document).bind('keydown', 'ctrl+n', function () {
        $("[id*=ButtonAddExpenditureDetails]").click();
        return false;
    });

    $(document).bind('keydown', 'del', function () {
        $("[id*=ButtonDeleteFromDetailsTable]").click();
        return false;
    });

    $(document).bind('keydown', 'ctrl+a', function () {
        $("[id$=CheckBoxDetailsTableSelect]").each(function () {
            this.checked = true;
        });
        return false;
    });

    //$("[id*=LinkButtonMainTableDetails]").click(function () {
    //    ShowDetailsTable('', '', false);
    //});

    $(document).on("dblclick", ".GridActualCostCells", function () {
        $(".GridActualCostCells").each(function (index) {
            AddCellValue($(this).attr("id"));
        });
    });

    $(document).on("dblclick", "[id*=TextBoxDetailsTableFieldValue]", function () {
        $("[id*=TextBoxDetailsTableFieldValue]").each(function (index) {
            AddCellValue($(this).attr("id"));
        });
    });

    //$(document).on("dblclick", "[id*=TextBoxDetailsTableFieldName]", function (event) {
    //    var textBoxDetailsTableFieldName = $('#' + event.target.id);

    //    __doPostBack('ProductExpenditureDetailsPopup', textBoxDetailsTableFieldName.attr('DetailID'));
    //});

    $(document).on("dblclick", "[id*=TextBoxDetailsTableFieldAmount]", function () {
        $("[id*=TextBoxDetailsTableFieldAmount]").each(function (index) {
            AddCellValue($(this).attr("id"));
        });
    });

    $(document).on("dblclick", ".GridExpectedCostCells", function () {
        $(".GridExpectedCostCells").each(function (index) {
            AddCellValue($(this).attr("id"));
        });
    });

    // COPY
    $("[id$='ctl00_ContentPlaceHolder1_ButtonDuplicate']").click(function () {
        $('<a href="#data">Copy expenses</a>').fancybox({
            modal: true,
            fitToView: true,
            width: 500,
            height: 200,
            autoSize: false,
            closeClick: true,
            openEffect: 'fade',
            closeEffect: 'elastic',
            afterClose: function () {
                __doPostBack($(_fancyboxPostBackControl).attr('name'), '');
                //parent.location.reload(true);
            }
        }).click();
    });

    // API
    $("[id$='ctl00_ContentPlaceHolder1_ApiManagementControl1_ButtonGenerateApiKeyShowPopup']").click(function () {
        $('<a href="#generateApiKey">Generate API key</a>').fancybox({
            modal: true,
            fitToView: true,
            width: 250,
            height: 100,
            autoSize: false,
            closeClick: true,
            openEffect: 'fade',
            closeEffect: 'elastic',
            afterClose: function () {
                __doPostBack($(_fancyboxPostBackControl).attr('name'), '');
                //parent.location.reload(true);
            }
        }).click();
    });

    $(document).on("click", "[id*=ButtonDetailsTableAttach]", function () {
        var chk = $("[id*=GridViewDetails]").find('input:checkbox:checked:first');

        $("[id*=HiddenDetailsRowID]").val(chk.parent().attr('detailid'));
    });

    $(document).on("click", "[id*=ButtonAttach]", function () {
        var chk = $("[id*=GridView1]").find('input[id*=CheckBoxSelectRow]:checked:first');

        $("[id*=HiddenRowID]").val(chk.parent().attr('rowid'));
    });

    // ADD NEW - Category DropDown OPEN
    $(document).on("click", "#btnPickCategory", function () {
        var cat = $("#Categories");
        cat.css("position", "absolute");
        cat.show('slow');
    });

    // ADD NEW - Category DropDown CANCEL
    $(document).on("click", "#btnCancelCat", function () {
        $("#Categories").hide("slow");
    });

    // ADD NEW - Category DropDown OK
    $(document).on("click", "#btnOkCat", function () {
        var cat = $("[id$='ctl00_ContentPlaceHolder1_TextBoxFieldName']");
        cat.attr("value", $("[id$='ctl00_ContentPlaceHolder1_ListBoxPickCategory'] option:selected").text());
        $("#Categories").hide('slow');
    });

    // ADD NEW - SHOW
    $("[id*=ButtonShowAddNewFieldsDiv]").click(function () {
        $('<a href="#PanelAddNewFields">Add new expenditure</a>').fancybox({
            modal: true,
            fitToView: true,
            width: 300,
            height: 500,
            autoSize: false,
            closeClick: true,
            openEffect: 'fade',
            closeEffect: 'elastic',
            afterClose: function () {
                __doPostBack($(_fancyboxPostBackControl).attr('name'), '');
                //parent.location.reload(true);
            }
        }).click();
    });

    // ADD NEW - Checkbox Recurrent
    $(document).on("click", "[id*=CheckBoxRecurrentExpenditure]", function () {
        var checked = $("[id*=CheckBoxRecurrentExpenditure]").prop("checked");

        var checkBoxEveryMonth = $("[id*=CheckBoxEnterForEveryMonth]");

        var labelRecurrentForRemainingMonthOnly = $("[id*=LabelRecurrentForFollowingMonthsOnly]");

        if (checked) {
            checkBoxEveryMonth.css("visibility", "visible");
            labelRecurrentForRemainingMonthOnly.css("visibility", "visible");
        } else {
            checkBoxEveryMonth.css("visibility", "hidden");
            labelRecurrentForRemainingMonthOnly.css("visibility", "hidden");
            checkBoxEveryMonth.attr("checked", false);
        }
    });

    // Close Fancybox
    $(document).on("click", "#ctl00_ContentPlaceHolder1_ApiManagementControl1_ButtonCancel, #ButtonCancelAddField", function () {
        $.fancybox.close();
    });

    $("[id*=TextBoxAddToShoppingListAmount]").spinner();
});