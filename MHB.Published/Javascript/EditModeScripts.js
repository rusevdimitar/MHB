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

// Override ShowDetailsTable from JQuery_function.js
function ShowDetailsTable(name, pickASupplierPopupTitle, showSupplierPopup) {   
    __doPostBack('RebindDetailsGridInEditMode', '');
}

function ShowDetailsTableInEditMode()
{
    var panelDetailsTable = $('#PanelDetailsTable');

    panelDetailsTable.dialog(
    {
        width: '950px',
        title: name
    });

    $("[id*=TextBoxDetailNameNew]").val("");
    $("[id*=TextBoxDetailDescriptionNew]").val("");
    $("[id*=TextBoxDetailValueNew]").val("");

    panelDetailsTable.parent().appendTo(jQuery("form:first"));

    ShowProductDetailsPanel();
    ShowNotesPanel();
    ShowSearchPanel();    
}

function ShowProductDetailsPanel() {
    var panelProductDetails = $('#DivProductDetails');

    panelProductDetails.dialog(
    {
        width: 'auto'
    });

    panelProductDetails.parent().appendTo(jQuery("form:first"));
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
           width: 'auto'
       });

    panelNotes.parent().appendTo(jQuery("form:first"));
}