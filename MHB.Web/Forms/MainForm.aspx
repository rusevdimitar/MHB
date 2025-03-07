<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="MainForm.aspx.vb" Inherits="MHB.Web.TestGrid" EnableViewStateMac="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../CustomControls/ToolTip.ascx" TagName="ToolTip" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/Budgets.ascx" TagName="Budgets" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/Notes.ascx" TagName="Note" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/SearchControl.ascx" TagName="Search" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/SupplierSelector.ascx" TagName="SupplierSelector" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/TodaySpentListControl.ascx" TagName="TodaySpentListControl" TagPrefix="uc1" %>

<%@ Register Src="../CustomControls/PurchaseHistoryControl.ascx" TagName="PurchaseHistoryControl" TagPrefix="uc1" %>
<%@ Register Src="../CustomControls/PurchaseHistoryDateSelectorControl.ascx" TagName="PurchaseHistoryDateSelectorControl" TagPrefix="uc1" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Import Namespace="MHB.BL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input id="HiddenRowID" runat="server" value="" type="hidden" />
    <input id="HiddenDetailsRowID" runat="server" value="" type="hidden" />
    <input id="HiddenDetailsMeasureTypeID" runat="server" value="3" type="hidden" />
    <input id="HiddenSelectedSupplier" runat="server" value="1" type="hidden" />
    <input id="HiddenProductID" runat="server" value="1" type="hidden" />
    <input id="HiddenAttachingToDetailsTable" runat="server" value="1" type="hidden" />
    <input id="HiddenPrintShoppingListDetailsIncludeAllProductEntries" runat="server" value="1" type="hidden" />
    <input id="HiddenSumSelectedGridCells" runat="server" value="0" type="hidden" />

    <script type="text/javascript" language="javascript">

        EventUtil.DOMReady(function () {
            var myContextMenu = new ContextMenu('<%= GridView1.ClientID %>', 'contextMenu', '<%= HiddenRowID.ClientID %>', function (e) { });
            loadDiv();
            SetAutoComplete();
        });

        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(load_lazyload);
            load_lazyload();
        });

        function load_lazyload()
        {
            var selected_tab = 1;
            $(function () {

                var tabs = $("#tabs").tabs({
                    select: function (e, i) {
                        selected_tab = i.index;
                    }
                });
                selected_tab = $("[id$=HiddenSelectedLastEditedParentExpenditureTabIndex]").val() != "" ? parseInt($("[id$=HiddenSelectedLastEditedParentExpenditureTabIndex]").val()) : 0;
                //tabs.tabs('select', selected_tab);
                tabs.tabs({ active: selected_tab });
                //alert(selected_tab);
                $("[id$=HiddenSelectedLastEditedParentExpenditureTabIndex]").val(selected_tab);
                //$("form").submit(function () {
                //    $("[id$=HiddenSelectedLastEditedParentExpenditureTabIndex]").val(selected_tab);
                //});
                $('#<%= TextBoxDetailsGridSearch.ClientID%>').keyup(function(event){
                    if(event.keyCode == 13){
                        __doPostBack('<%= TextBoxDetailsGridSearch.ClientID%>', 0);
                    }
                });
            });
        }
        function DrawDetailsPieChart(dataPoints) {
            $("#detailsGridChartContainer").CanvasJSChart({
                width: 700,
                height: 400,
                backgroundColor: "#F2F5F7",
                axisY: {
                    title: "Categories"
                },
                legend: {
                    verticalAlign: "center",
                    horizontalAlign: "right"
                },
                data: [
                {
                    type: "pie",
                    showInLegend: true,
                    toolTipContent: '<%= String.Format("{{label}} <br /> {{y}} {0}", Me.Currency)%>',
                    indexLabel: '<%= String.Format("{{y}} {0}", Me.Currency) %>',
                    dataPoints: dataPoints
                }
                ]
            });
        }

        function SetAutoComplete() {

            var textBoxDetailNameNew = $("[id$=TextBoxDetailNameNew]");

            var textBoxDetailsGridSearch = $("[id$=TextBoxDetailsGridSearch]");

            //textBoxDetailNameNew.autocomplete({ source: ["c++", "java", "php", "coldfusion", "javascript", "asp", "ruby"] });
            // VERY IMPORTANT COMMENT!! I SPENT THE ENTIRE AFTERNOON WITH THIS ****
            // IT ALWAYS CAME UP WITH INTERNAL SERVER ERROR WHEN I PASSED textBoxDetailNameNew.val() TO data: PARAMETER IN THE AJAX REQUEST
            // AND IF YOU PUT IN A PLAIN STRING LIKE SO: '{prefixText:"kame"}' IT WOULD WORK !?!?!
            // THE REASON FOR ALL THIS **** WAS THAT IF YOU USE THE textBoxDetailNameNew.val() AND TYPE JUST A SINGLE LETTER
            // IT WOULD RETURN HUNDREDS OF RECORDS CAUSING JavaScriptSerializer().Serialize(products) TO SHOW LENGTH EXCEEDED OR SOMETHING
            // I WAS SO DESPERATE THAT I DID NOT CHANGE THE MOST IMPORTANT ERROR MESSAGE IN THE ERROR HANDLER LIKE SO: alert(JSON.stringify(XMLHttpRequest));
            // AND THE BEST PART WAS THAT I WAS ABLE TO DEBUG GetProductsJSON AND IT WOULD RETURN NORMALLY AND THROW THE ERROR UPON EXIT CLIENT SIDE :(
            // ALWAYS, ALWAYS, ALWAYS GET THE MOST ERROR INFO AS YOU CAN GET !!!
            textBoxDetailNameNew.autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        type: "POST",
                        url: "/Forms/ProductAutoComplete.asmx/GetProductsJSON",
                        data: '{prefixText:"' + textBoxDetailNameNew.val().replace(/\\/g, '') + '"}',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var jsonData = jQuery.parseJSON(data.d)

                            var s = $.map(jsonData, function (item) {
                                return [{ label: item.Name, value: item.ID }];
                            })

                            response(s);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(JSON.stringify(errorThrown));
                            alert(textStatus);
                            alert(JSON.stringify(XMLHttpRequest));
                        }
                    });
                },
                select: function (event, ui) {
                    $("[id*=HiddenProductID]").val(ui.item.value);
                    textBoxDetailNameNew.val(ui.item.label);
                    __doPostBack('<%= ButtonAddExpenditureDetails.ClientID%>', ui.item.value);
                },
                open: function (event, ui) {
                    if ($('#DivWeightVolumeSelector').is(':visible')) {
                        $('#DivWeightVolumeSelector').dialog('close');
                    }
                },
                minLength: 1
            });

            textBoxDetailsGridSearch.autocomplete(
            {
                source: function (request, response) {
                    $.ajax(
                    {
                        type: "POST",
                        url: "/Forms/DetailsGridSearch.asmx/SearchDetails",
                        data: '{input:"' + textBoxDetailsGridSearch.val().replace(/\\/g, '') + '"}',
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var jsonData = jQuery.parseJSON(data.d)

                            var s = $.map(jsonData, function (item) {
                                return [{ label: item.Name, value: item.ID }];
                            })

                            response(s);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(JSON.stringify(errorThrown));
                            alert(textStatus);
                            alert(JSON.stringify(XMLHttpRequest));
                        }
                    });
                },
                select: function (event, ui) {
                    //textBoxDetailsGridSearch.val(ui.item.label);
                    __doPostBack('<%= TextBoxDetailsGridSearch.ClientID%>', ui.item.value);
                },
                open: function (event, ui) {
                },
                minLength: 1
            });
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();

        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    SetAutoComplete();
                }
            });
        };

      <%--  function ProductAutoCompleteDetailsItemSelected(source, eventArgs) {

            <%= String.Format("$('#{0}').val(eventArgs.get_value());", HiddenProductID.ClientID)%>

            __doPostBack('<%= ButtonAddExpenditureDetails.ClientID%>', eventArgs.get_value());
        }--%>

        function ProductAutoCompleteItemSelected(source, eventArgs) {
            $("[id*=HiddenProductID]").val(eventArgs.get_value());
        }

        function confirmSelectDetailsPrintShoppingListMode() {

            if ($("[id*=CheckBoxDetailsTableSelect]:checked").length > 0) {
                $('#dialog-modal').dialog({
                    autoOpen: false,
                    width: 700,
                    modal: true,
                    resizable: false,

                    buttons: {
                        '<%= Me.GetTranslatedValue("ShoppingListDetailsButtonAllProductOccurrences", Me.CurrentLanguage) %>': function () {
                            <%= String.Format("$('#{0}').val(1);", HiddenPrintShoppingListDetailsIncludeAllProductEntries.ClientID)%>
                            __doPostBack('<%= ButtonDetailsPrintShoppingList.ClientID%>', '');
                            return true;
                        },
                        '<%= Me.GetTranslatedValue("ShoppingListDetailsButtonIncludeProductOnce", Me.CurrentLanguage) %>': function () {
                            <%= String.Format("$('#{0}').val(0);", HiddenPrintShoppingListDetailsIncludeAllProductEntries.ClientID)%>
                            __doPostBack('<%= ButtonDetailsPrintShoppingList.ClientID%>', '');
                            return true;
                        },
                        "Cancel": function () {
                            $(this).dialog("close");
                            return false;
                        }
                    }

                });
                $('#dialog-modal').dialog('open');
            }
            else {
                __doPostBack('<%= ButtonDetailsPrintShoppingList.ClientID%>', '');
            }
        }

        function MoveSelectedDetails() {
            return MoveDetailsToNewParent('<%= Me.GetTranslatedValue("MoveSelectedDetailsToAnotherParentTooltip", Me.CurrentLanguage) %>');
        }
    </script>

    <ul id="contextMenu">
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/page_white_copy.png" alt="Context menu copy" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuCopy" Text="Copy & paste"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/context_delete.png" alt="Context menu delete" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuDelete" Text="Delete"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/flag_blue.png" alt="Context menu flag" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuFlag" Text="Flag/unflag"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/tick_circle.png" alt="Context menu paid" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuMarkedPaid" Text="Mark paid/unpaid"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/attach.png" alt="Context menu view attachment" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuDownloadAttachment" Text="Download attachment"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/context_delete_attach.gif" alt="Context menu delete attachment" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuDeleteAttachment" Text="Delete attachment"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/resultset_next.png" alt="Context menu next month" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuNextMonth" Text="Go to next month"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/resultset_previous.png" alt="Context menu prev. month" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuPreviousMonth" Text="Go to previous month"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/copy_all.png" alt="Context menu copy all" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuCopyToNextMonth" Text="Copy ALL to next month"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
        <li>
            <table width="100%">
                <tr>
                    <td width="20px">
                        <img src="../Images/hide_paid.png" alt="Hide all records marked as paid" />
                    </td>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="LinkButtonContextMenuHidePaid" Text="Hide/show paid"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </li>
    </ul>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="divContentBody" id="divUpdateProgress">
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <div id="DivLoading" class="LoadingDiv">
                    <div style="border: solid 1px #C1DAFF">
                        <table style="width: 100%;">
                            <tr>
                                <td width="33px">
                                    <img src="../Images/loading_dollar.gif" alt="Loading..." />
                                </td>
                                <td align="center">Loading...
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <uc1:ToolTip ID="ToolTip1" runat="server" ControlToToolTip="ButtonShowAddNewFieldsDiv"
            ToolTipMessage="Click to add a new bill / expense for this month." />
        <div align="center">
            <table class="MainFormCenterTable" cellspacing="0" width="100%">
                <tr>
                    <td align="left">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="DivError" runat="server" align="center" class="ErrorDiv">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="display: none">
                            <div id="data" style="width: 450px; height: 170px;">
                                <table>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblDuplicateDestinationMonth" runat="server" Text="Destination Month:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpDestinationMonth" runat="server" CssClass="PlainText" Width="160px">
                                                <asp:ListItem Text="January" Value="1" />
                                                <asp:ListItem Text="February" Value="2" />
                                                <asp:ListItem Text="March" Value="3" />
                                                <asp:ListItem Text="April" Value="4" />
                                                <asp:ListItem Text="May" Value="5" />
                                                <asp:ListItem Text="June" Value="6" />
                                                <asp:ListItem Text="July" Value="7" />
                                                <asp:ListItem Text="August" Value="8" />
                                                <asp:ListItem Text="September" Value="9" />
                                                <asp:ListItem Text="October" Value="10" />
                                                <asp:ListItem Text="November" Value="11" />
                                                <asp:ListItem Text="December" Value="12" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblDuplicateDestinationYear" runat="server" Text="Destination Year:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpYear" runat="server" CssClass="PlainText" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblDUplicateFLaggedFilesOnly" runat="server" Text="Copy flagged records only?"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkCopyFlaggedRecordsOnly" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblDuplicateDeleteAllPriorToCopy" runat="server" Text="Delete all existing files prior to copying?"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkDeleteExistingData" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblCopyAllAsUnpaid" runat="server" Text="Mark unpaid?"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkMarkUnpaid" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">
                                            <asp:Label ID="lblZeroActualSums" runat="server" Text="Zero actual sums?"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkZeroActualSums" runat="server" Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="ButtonStartDuplicate" runat="server" Text="Start Duplicate Routine"
                                                CssClass="ButtonAdd" OnClientClick="CloseFancyBoxAndSubmit(this, event); return false;" />
                                            <input type="button" value="Cancel" class="ButtonAddSmall" onclick="javascript: $.fancybox.close();" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanelTabButtons" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="PanelTabMenu" runat="server" CssClass="TabButtonsPanel" HorizontalAlign="Center">
                                    <asp:Button ID="Button1" runat="server" Text="January" />
                                    <asp:Button ID="Button2" runat="server" Text="February" />
                                    <asp:Button ID="Button3" runat="server" Text="March" />
                                    <asp:Button ID="Button4" runat="server" Text="April" />
                                    <asp:Button ID="Button5" runat="server" Text="May" />
                                    <asp:Button ID="Button6" runat="server" Text="June" />
                                    <asp:Button ID="Button7" runat="server" Text="July" />
                                    <asp:Button ID="Button8" runat="server" Text="August" />
                                    <asp:Button ID="Button9" runat="server" Text="September" />
                                    <asp:Button ID="Button10" runat="server" Text="October" />
                                    <asp:Button ID="Button11" runat="server" Text="November" />
                                    <asp:Button ID="Button12" runat="server" Text="December" />
                                    <asp:DropDownList ID="DropDownListYear" runat="server" AutoPostBack="True" CssClass="DropDownYear">
                                        <asp:ListItem>2006</asp:ListItem>
                                        <asp:ListItem>2007</asp:ListItem>
                                        <asp:ListItem>2008</asp:ListItem>
                                        <asp:ListItem>2009</asp:ListItem>
                                        <asp:ListItem>2010</asp:ListItem>
                                        <asp:ListItem>2011</asp:ListItem>
                                        <asp:ListItem>2012</asp:ListItem>
                                        <asp:ListItem>2013</asp:ListItem>
                                        <asp:ListItem>2014</asp:ListItem>
                                        <asp:ListItem>2015</asp:ListItem>
                                        <asp:ListItem>2016</asp:ListItem>
                                        <asp:ListItem>2017</asp:ListItem>
                                        <asp:ListItem>2018</asp:ListItem>
                                        <asp:ListItem>2019</asp:ListItem>
										<asp:ListItem>2020</asp:ListItem>
										<asp:ListItem>2021</asp:ListItem>
										<asp:ListItem>2022</asp:ListItem>
                                        <asp:ListItem>2023</asp:ListItem>
                                        <asp:ListItem>2024</asp:ListItem>
                                        <asp:ListItem>2025</asp:ListItem>
                                        <asp:ListItem>2026</asp:ListItem>
                                        <asp:ListItem>2027</asp:ListItem>
                                        <asp:ListItem>2028</asp:ListItem>
                                        <asp:ListItem>2029</asp:ListItem>
                                        <asp:ListItem>2030</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:UpdatePanel ID="UpdatePanelCurrentMonth" runat="server">
                            <ContentTemplate>
                                &nbsp;&nbsp;<asp:Label ID="LabelCurrentMonth" runat="server" CssClass="LabelCurrentMonth"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div id="DivAttach" style="display: none;">
                                    <table>
                                        <tr>
                                            <td class="PlainTextBold">Choose a file ot attach:
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                                <br />
                                                <asp:Label ID="LabelFileMaxSize" CssClass="PlainTextError" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Button ID="ButtonFileAttachConfirm" runat="server" CssClass="ButtonAddSmall" Text="OK" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="ButtonFileAttachConfirm" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td class="ActionButtonsMainFormBar">
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ButtonShowAddNewFieldsDiv" runat="server" Text="Add new" OnClientClick="javascript:return false;"
                                        CssClass="ButtonAddInsert" />
                                    <asp:Button ID="ButtonUpdate" runat="server" Text="Update" CssClass="ButtonAddSave" />
                                    <asp:Button ID="ButtonDeleteSelectedRow" runat="server" Text="Delete selected" CssClass="ButtonAddRemove" />
                                    <asp:Button ID="ButtonAddDetails" runat="server" CssClass="ButtonAddMedium" Text="Add " />
                                    <asp:Button ID="ButtonAttach" runat="server" CssClass="ButtonAddMedium" OnClientClick="javascript:ShowMainTableAttach(''); return false;" Text="Attach" />
                                    <asp:Button ID="ButtonSearch" runat="server" CssClass="ButtonAddMedium" Text="Search" OnClientClick="javascript:ShowSearchPanel(); return false;" />
                                    <asp:Button ID="ButtonUndo" runat="server" CssClass="ButtonAddMedium" Text="Undo" />
                                    <asp:Button ID="ButtonDuplicate" runat="server" CssClass="ButtonAddMedium" Text="Copy"
                                        OnClientClick="javascript:return false;" />
                                    <asp:UpdatePanel ID="UpdatePanelButtonHidePaidExpenses" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="ButtonHidePaidExpenses" runat="server" CssClass="ButtonAddMedium"
                                                Text="Hide/show paid" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table style="width: 100%;" cellspacing="0" class="MainGridTable">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="LinkButtonSelectAll" runat="server">Select all</asp:LinkButton>
                                    &nbsp;|
                                    <asp:LinkButton ID="LinkButtonDeselectAll" runat="server">Deselect all</asp:LinkButton>
                                    &nbsp;|
                                    <asp:LinkButton ID="LinkButtonFlagAll" runat="server">Flag all</asp:LinkButton>
                                    &nbsp;|
                                    <asp:LinkButton ID="LinkButtonUnflagAll" runat="server">Unflag all</asp:LinkButton>
                                </td>
                                <td align="right" valign="bottom">
                                    <div style="width: 200px;">
                                        <asp:Label ID="LabelLastUpdated" runat="server" CssClass="smallTipText"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <div id="DivAddNewRecord" class="CenteredDiv">
                                                <div id="PanelAddNewFields" class="AddNewRecordDiv">
                                                    <asp:Panel runat="server" DefaultButton="ButtonAddField">
                                                        <table style="width: 300px;">
                                                            <tr>
                                                                <td align="left" colspan="3" class="AddNewDivTopBar">
                                                                    <asp:Label ID="LabelAddNewField" runat="server" Text="Add new field for the month"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:Label ID="LabelName" runat="server" Text="Name:"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3">
                                                                    <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="PlainTextBox" Width="200px"></asp:TextBox>

                                                                    <asp:AutoCompleteExtender
                                                                        runat="server"
                                                                        BehaviorID="AutoCompleteEx"
                                                                        ID="AutoCompleteProducts"
                                                                        TargetControlID="TextBoxFieldName"
                                                                        ServicePath="ProductAutoComplete.asmx"
                                                                        ServiceMethod="GetProducts"
                                                                        MinimumPrefixLength="2"
                                                                        CompletionInterval="10"
                                                                        EnableCaching="true"
                                                                        CompletionSetCount="20"
                                                                        DelimiterCharacters=";, :"
                                                                        OnClientItemSelected="ProductAutoCompleteItemSelected"
                                                                        ShowOnlyCurrentWordInCompletionListItem="true"
                                                                        CompletionListCssClass="autocomplete_completionListElement"
                                                                        CompletionListItemCssClass="autocomplete_listItem"
                                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem">
                                                                        <Animations>
                                                                        <OnShow>
                                                                            <Sequence>
                                                                                <%-- Make the completion list transparent and then show it --%>
                                                                                <OpacityAction Opacity="0" />
                                                                                <HideAction Visible="true" />

                                                                                <%--Cache the original size of the completion list the first time
                                                                                    the animation is played and then set it to zero --%>
                                                                                <ScriptAction Script="
                                                                                    // Cache the size and setup the initial size
                                                                                    var behavior = $find('AutoCompleteEx');
                                                                                    if (!behavior._height) {
                                                                                        var target = behavior.get_completionList();
                                                                                        behavior._height = target.offsetHeight - 2;
                                                                                        target.style.height = '0px';
                                                                                    }" />

                                                                                <%-- Expand from 0px to the appropriate size while fading in --%>
                                                                                <Parallel Duration=".4">
                                                                                    <FadeIn />
                                                                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoCompleteEx')._height" />
                                                                                </Parallel>
                                                                            </Sequence>
                                                                        </OnShow>
                                                                        <OnHide>
                                                                            <%-- Collapse down to 0px and fade out --%>
                                                                            <Parallel Duration=".4">
                                                                                <FadeOut />
                                                                                <Length PropertyKey="height" StartValueScript="$find('AutoCompleteEx')._height" EndValue="0" />
                                                                            </Parallel>
                                                                        </OnHide>
                                                                        </Animations>
                                                                    </asp:AutoCompleteExtender>

                                                                    <input type="button" id="btnPickCategory" class="ButtonAddTiny" value="..." />
                                                                    <div id="Categories">
                                                                        <%-- <select size="4" id="SelectPickCategory" class="ListBoxPickCategory">
                                                                        <option value="1">Гориво</option>
                                                                        <option value="2">Електричество</option>
                                                                        <option value="3">Телефон</option>
                                                                        <option value="4">Наем</option>
                                                                        <option value="5">Спестявания</option>
                                                                        <option value="6">Интернет</option>
                                                                        <option value="7">Храна</option>
                                                                        <option value="8">Кола</option>
                                                                        <option value="9">Заеми</option>
                                                                        <option value="10">Медицински</option>
                                                                    </select>--%>
                                                                        <asp:ListBox ID="ListBoxPickCategory" runat="server" CssClass="ListBoxPickCategory"></asp:ListBox>
                                                                        <input type="button" id="btnOkCat" class="PlainButton" value="OK" />
                                                                        <input type="button" id="btnCancelCat" class="PlainButton" value="Cancel" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:Label ID="LabelDescription" runat="server" Text="Description:"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3">
                                                                    <asp:TextBox ID="TextBoxFieldDescription" runat="server" CssClass="PlainTextBox"
                                                                        Height="80px" TextMode="MultiLine" Width="200px"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:Label ID="LabelExpectedCost" runat="server" Text="Expected cost:"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3">
                                                                    <asp:TextBox ID="TextBoxExpectedValue" runat="server" CssClass="PlainTextBox" Width="200px"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="TextBoxExpectedValue"
                                                                        CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" colspan="3" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:Label ID="LabelRecurrentBill" runat="server" Text="recurrent bill?"></asp:Label>
                                                                    <asp:CheckBox ID="CheckBoxRecurrentExpenditure" runat="server" AutoPostBack="false" />
                                                                    <br />
                                                                    <asp:Label ID="LabelRecurrentForFollowingMonthsOnly" runat="server" Style="visibility: hidden;"
                                                                        Text="just for the comming months?"></asp:Label>
                                                                    <asp:CheckBox ID="CheckBoxEnterForEveryMonth" runat="server" Style="visibility: hidden;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td align="left" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:Label ID="LabelDueDate" runat="server" Text="due date:"></asp:Label><asp:TextBox
                                                                        ID="TextBoxAddNewReccurentFieldDueDate" runat="server" Width="100px"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="CalendarExtender2" TargetControlID="TextBoxAddNewReccurentFieldDueDate"
                                                                        runat="server" CssClass="recurrentCalendar">
                                                                    </asp:CalendarExtender>
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <img src="../Images/info_icon.gif" alt="info icon" />
                                                                </td>
                                                                <td colspan="2" align="left">
                                                                    <asp:Label ID="LabelEditTableInfo" runat="server" CssClass="PlainTextBold" Text="Later on you can add the actually paid sum in the 'sum paid' column. You can enter figures directly in the cells! Remember to click Save!"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td align="left" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold">
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="TextBoxAddNewReccurentFieldDueDate"
                                                                        ErrorMessage="Please enter a valid date" ValidationExpression="(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp;
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Button ID="ButtonAddField" runat="server" CssClass="ButtonAddSmall" Text="Add field" OnClientClick="CloseFancyBoxAndSubmit(this, event); return false;" />
                                                                    <asp:Button ID="ButtonCloseAddNewDiv" runat="server" CssClass="ButtonAddSmall" Text="Cancel" OnClientClick="CloseFancyBox(); return false;" />
                                                                </td>
                                                                <td>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ButtonAddField" EventName="Click" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="GridViewFirefoxBorders" onkeypress="javascript:return WebForm_FireDefaultButton(event, '<%= ButtonUpdate.ClientID%>')">
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="100%"
                                                    GridLines="Horizontal" BackColor="White" BorderWidth="0" AllowSorting="true" ShowFooter="true">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="CheckBoxSelectAllRows" runat="server" Text="" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBoxSelectRow" rowid='<%# CType(Container.DataItem, Expenditure).ID %>'
                                                                    runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%--  <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../Images/flag_active.gif"
                                                                    OnClientClick='<%#  "javascript:FlagSum(" & Eval("ID") & ");" %>' />--%>
                                                                <asp:ImageButton ID="ImageButtonActive" runat="server" ImageUrl="../Images/flag_active.gif"
                                                                    OnClick="ImageButtonFlag_Click" />
                                                                <asp:ImageButton ID="ImageButtonInActive" runat="server" ImageUrl="../Images/flag_inactive.gif"
                                                                    OnClick="ImageButtonFlag_Click" />
                                                            </ItemTemplate>
                                                            <ItemStyle Wrap="false" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxID" runat="server" Text='<%# CType(Container.DataItem, Expenditure).ID %>'
                                                                    Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxCategoryID" runat="server" CssClass="GridCells" Text='<%# CType(Container.DataItem, Expenditure).CategoryID%>'
                                                                    Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageButtonPreviewAttachment" ToolTip="Preview attachment" runat="server" ImageUrl="~/Images/ico16_leaf1.gif" Visible="False"
                                                                    OnClientClick='<%# String.Format("javascript:ShowPreviewAttach(""DivPreviewAttachment{0}"", ""ImgPreviewAttachment{0}"", {0}, ""a""); return false;", CType(Container.DataItem, Expenditure).ID)  %>' />

                                                                <div id='<%# String.Format("DivPreviewAttachment{0}", CType(Container.DataItem, Expenditure).ID)%>' style="display: none;">
                                                                    <img id='<%# String.Format("ImgPreviewAttachment{0}", CType(Container.DataItem, Expenditure).ID)%>' alt='Preview attachment' />
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageHasAttachment" runat="server" ToolTip="Download attachment"
                                                                    CommandName="Edit" ImageUrl="../Images/attach.png" Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageDeleteAttachment" runat="server" ToolTip="Delete attachment"
                                                                    CommandName="Delete" ImageUrl="../Images/delete_attachment.gif" Visible="false"
                                                                    OnClientClick="javascript:return confirm('really delete?')" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxOrderID" runat="server" CssClass="GridCells" Text='<%# CType(Container.DataItem, Expenditure).OrderID %>'
                                                                    Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxUserID" runat="server" CssClass="GridCells" Text='<%# CType(Container.DataItem, Expenditure).UserID %>'
                                                                    Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxMonth" runat="server" CssClass="GridCells" Text='<%# CType(Container.DataItem, Expenditure).Month %>'
                                                                    Visible="false"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expenses" SortExpression="FieldName" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxFieldName" runat="server" CssClass="GridCells" ToolTip='<%# "Sum was last edited on: " & CType(Container.DataItem, Expenditure).DateRecordUpdated %>'
                                                                    Text='<%# CType(Container.DataItem, Expenditure).FieldName %>' RowID='<%# CType(Container.DataItem, Expenditure).ID %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImageCategory" runat="server" Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expenditure description" SortExpression="FieldDescription" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxFieldDescription" runat="server" CssClass="GridCells" Text='<%# CType(Container.DataItem, Expenditure).FieldDescription %>'
                                                                    Width="170px" RowID='<%# CType(Container.DataItem, Expenditure).ID %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>

                                                        <%--===================================================================================================--%>
                                                        <asp:TemplateField HeaderText="Expected cost" SortExpression="FieldExpectedValue" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" class="NoBordersForTablesInsideGridViews"
                                                                    style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxFieldExpectedValue" runat="server" CssClass="GridExpectedCostCells"
                                                                                Width="100px" Text='<%# IIf(IsDBNull(CType(Container.DataItem, Expenditure).FieldExpectedValue), "0", String.Format("{0:f}", CType(Container.DataItem, Expenditure).FieldExpectedValue))%>'></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator0" runat="server" ControlToValidate="TextBoxFieldExpectedValue"
                                                                                CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="LabelSumPlanned" runat="server" CssClass="PlainTextExtraLargePaleBlue"></asp:Label>
                                                            </FooterTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <%--===================================================================================================--%>
                                                        <asp:TemplateField HeaderText="Sum" SortExpression="FieldValue" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <div class="SumPopupDiv" id="divAddToSum" runat="server" style="display: none;">
                                                                    <asp:TextBox ID="TextBoxFieldValueAddition" runat="server" CssClass="SumPopupTextBox"></asp:TextBox>
                                                                    <img id="ImageButtonAddToCurrVal" alt="Add an exact ammount to the current sum" runat="server"
                                                                        src='../Images/button_ok.png' style="cursor: pointer;" />
                                                                </div>
                                                                <table cellpadding="0" cellspacing="0" class="NoBordersForTablesInsideGridViews"
                                                                    style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <table cellpadding="0" cellspacing="0" border="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img id="ImmageButtonShowAddToSumDiv" alt="Add some ammount to the current sum" runat="server"
                                                                                            src='../Images/edit_add.png' style="cursor: pointer;" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="TextBoxFieldValue" runat="server" CssClass="GridActualCostCells"
                                                                                            Width="100px" Text='<%# String.Format("{0:f}", CType(Container.DataItem, Expenditure).FieldValue) %>'></asp:TextBox>
                                                                                    </td>
                                                                                    <td>
                                                                                        <img id="ImmageButtonRevertToLastSavedSum" alt="Revert to last saved sum" runat="server"
                                                                                            src='../Images/undo_grid_icon_hover.gif' style="cursor: pointer; opacity: 0.3;" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <asp:TextBox ID="TextBoxHiddenFieldValueOld" runat="server" CssClass="HiddenField"
                                                                                Text='<%# String.Format("{0:f}", CType(Container.DataItem, Expenditure).FieldOldValue) %>'></asp:TextBox>
                                                                            <asp:TextBox ID="TextBoxHiddenStoredCurrentValue" runat="server" CssClass="HiddenField"
                                                                                Text='<%# String.Format("{0:f}", CType(Container.DataItem, Expenditure).FieldValue) %>'></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TextBoxFieldValue"
                                                                                CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$">
                                                                            </asp:RegularExpressionValidator>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="TextBoxFieldValueAddition"
                                                                                CssClass="Validator" ErrorMessage="!!!" ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$">
                                                                            </asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>

                                                            <FooterTemplate>
                                                                <img src="../Images/edit_add_placeholder.gif" alt="" />
                                                                <asp:Label ID="LabelSumPaid" runat="server" CssClass="PlainTextExtraLargePaleBlue"></asp:Label>
                                                            </FooterTemplate>

                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <%-- <HeaderTemplate>
                                                                <div style="float:left;margin-right:5px;">
                                                                    <img id="imgSwitchExpectedAndActualSums"
                                                                        src="../Images/switch_icon.png"
                                                                        onmouseover="this.src='../Images/switch_icon_hover.png'"
                                                                        onmouseout="this.src='../Images/switch_icon.png';"
                                                                        alt="Alternate Text"
                                                                        style="cursor: pointer;" />
                                                                </div>
                                                                <div style="float:left;">
                                                                    <%= Me.GetTranslatedValue("Sum", Me.CurrentLanguage) %>
                                                                </div>
                                                            </HeaderTemplate>--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Difference" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxFieldValueDifference" Width="70px" runat="server" CssClass="GridCells"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="LabelSumDifference" runat="server" CssClass="PlainTextExtraLargeSkyBlue"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Image ID="ImageDueDateWarning" runat="server" ImageUrl="../Images/warning_icon_small.gif"
                                                                    Visible="false" />
                                                                <asp:Image ID="ImageBillIsPaid" runat="server" ImageUrl="../Images/small_green_check.gif"
                                                                    Visible="false" />
                                                                <asp:Image ID="ImagePendingIcon" runat="server" ImageUrl="../Images/pending_icon.gif"
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Due date" SortExpression="DueDate" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <table cellpadding="0" cellspacing="0" class="NoBordersForTablesInsideGridViews"
                                                                    style="width: 100%;">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="TextBoxDueDate" runat="server" Width="80px" CssClass="GridCells"
                                                                                Text='<%# CType(Container.DataItem, Expenditure).DueDate %>'></asp:TextBox>
                                                                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="TextBoxDueDate" runat="server">
                                                                            </asp:CalendarExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorDueDateGrid" runat="server"
                                                                                ControlToValidate="TextBoxDueDate" CssClass="Validator" ErrorMessage="!!!" ValidationExpression="(?=\d)^(?:(?!(?:10\D(?:0?[5-9]|1[0-4])\D(?:1582))|(?:0?9\D(?:0?[3-9]|1[0-3])\D(?:1752)))((?:0?[13578]|1[02])|(?:0?[469]|11)(?!\/31)(?!-31)(?!\.31)|(?:0?2(?=.?(?:(?:29.(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:(?:\d\d)(?:[02468][048]|[13579][26])(?!\x20BC))|(?:00(?:42|3[0369]|2[147]|1[258]|09)\x20BC))))))|(?:0?2(?=.(?:(?:\d\D)|(?:[01]\d)|(?:2[0-8])))))([-.\/])(0?[1-9]|[12]\d|3[01])\2(?!0000)((?=(?:00(?:4[0-5]|[0-3]?\d)\x20BC)|(?:\d{4}(?!\x20BC)))\d{4}(?:\x20BC)?)(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Paid?">
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkSelectAllPaid" runat="server" Text="" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBoxIsPaid" runat="server" Checked='<%# IIf(IsDBNull(CType(Container.DataItem, Expenditure).IsPaid),"False",CType(Container.DataItem, Expenditure).IsPaid) %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButtonBillDate" runat="server"
                                                                    Visible='<%# Me.IsInSearchMode%>'
                                                                    CommandName="NavigateToBillDate"
                                                                    CommandArgument='<%# String.Format("{0},{1}", CType(Container.DataItem, Expenditure).Year,CType(Container.DataItem, Expenditure).Month) %>'
                                                                    Text='<%# String.Format("{0}-{1}", CType(Container.DataItem, Expenditure).Year, CType(Container.DataItem, Expenditure).Month) %>'>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBoxHasDetails" runat="server" Checked='<%# IIf(IsDBNull(CType(Container.DataItem, Expenditure).HasDetails),"False", CType(Container.DataItem, Expenditure).HasDetails) %>'
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="CheckBoxHasAttachment" runat="server" Checked='<%# IIf(IsDBNull(CType(Container.DataItem, Expenditure).HasAttachment),"False",CType(Container.DataItem, Expenditure).HasAttachment) %>'
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Bill schedule" SortExpression="IsPaid" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="TextBoxDaysLeft" runat="server" CssClass="GridCells" ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButtonMainTableDetails" runat="server" CommandName="Select"
                                                                    OnClientClick='<%# String.Format("javascript:ShowDetailsTable(""{0}"", ""{1}"", {2});", CType(Container.DataItem, Expenditure).FieldName.Replace("""", string.Empty), Me.GetTranslatedValue("PickASupplierTitle", Me.CurrentLanguage), IIf(Me.Suppliers.Count > 0, "true", "false"))%>'>Details</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <%--  <asp:ImageButton ID="ImageButtonEpay" runat="server" Visible="false" ImageUrl="../Images/epay.png"
                                                                    OnClientClick="javascript:window.open('https://www.epay.bg/')" />--%>
                                                                <asp:CheckBox ID="CheckBoxIsShared" runat="server" Checked='<%# CType(Container.DataItem, Expenditure).IsShared%>' Visible="false" />
                                                                <asp:ImageButton
                                                                    ID="ImageButtonShareAttachment"
                                                                    runat="server"
                                                                    ToolTip="Share attachment"
                                                                    onmouseover="this.src='../Images/sharethis-24.png'"
                                                                    onmouseout='<%# IIf(CType(Container.DataItem, Expenditure).IsShared, "this.src=""../Images/sharethis-24.png""", "this.src=""../Images/sharethis-24_faded.png""")%>'
                                                                    ImageUrl='<%# IIf(CType(Container.DataItem, Expenditure).IsShared, "../Images/sharethis-24.png", "../Images/sharethis-24_faded.png")%>'
                                                                    Visible='<%# CType(Container.DataItem, Expenditure).HasAttachment%>'
                                                                    CommandName="ShareMainAttachment"
                                                                    CommandArgument='<%# CType(Container.DataItem, Expenditure).ID%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButtonTransactions" runat="server" Text="tran"
                                                                    onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                    onmouseout="this.style='opacity: 0.3; filter: alpha(opacity=30);'"
                                                                    Style="opacity: 0.3; filter: alpha(opacity=30);">
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <%--<SelectedRowStyle CssClass="MarkedRow" />--%>
                                                    <HeaderStyle CssClass="GridViewHeaderStyle" />
                                                </asp:GridView>

                                                <asp:Panel ID="PanelProgressBarContainer" runat="server">
                                                </asp:Panel>
                                                <%--  <div class="SumDiv">
                                                    <asp:Label ID="LabelSum" runat="server"></asp:Label>
                                                    <span id="spanSelectedSum"></span>
                                                </div>--%>
                                                <span id="spanSelectedSum"></span>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="ImageButtonUp" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ImageButtonDown" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ButtonUpdate" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ButtonDeleteSelectedRow" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ButtonHidePaidExpenses" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="ButtonHidePaidExpenses" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonSelectAll" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonDeselectAll" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonFlagAll" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonUnflagAll" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuCopy" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuDelete" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuFlag" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuMarkedPaid" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuDownloadAttachment" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuDeleteAttachment" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuCopyToNextMonth" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonContextMenuHidePaid" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:ImageButton ID="ImageButtonUp" runat="server" ImageUrl="~/Images/up.jpg" />
                                    <asp:ImageButton ID="ImageButtonDown" runat="server" ImageUrl="~/Images/down.jpg" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:Label ID="LabelMoveRows" runat="server" CssClass="smallTipText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">&nbsp;
                                    <asp:Button ID="ButtonCalculator" runat="server" CssClass="ButtonAddSmall" Text="Calculator" OnClientClick="javascript:ShowCalculatorPanel(); return false;" />
                                    <asp:Button ID="ButtonAnnualSummary" runat="server" CssClass="ButtonAdd" Text="Annual report (charts)" />
                                    <asp:Button ID="ButtonStatistics" runat="server" CssClass="ButtonAdd" Text="General report (charts)" />
                                    <asp:Button ID="ButtonAnnualReportForExport" runat="server" CssClass="ButtonAdd"
                                        Text="Annual report (PDF/Excel)" />
                                    <asp:Button ID="ButtonAnnualSummaryPerCategory" runat="server" CssClass="ButtonAdd"
                                        Text="Annual report by category" />
                                    <asp:Button ID="ButtonNotes" runat="server" CssClass="ButtonAddSmall" Text="Notes" OnClientClick="javascript:ShowNotesPanel(); return false;" />
                                    <asp:Button ID="ButtonEventsCalendar" runat="server" CssClass="ButtonAddSmall" Text="Calendar" />
                                    <%--   <asp:Button ID="ButtonCategoriesManagement" runat="server" CssClass="ButtonAddNeutral"
                                        Text="Categories Management" />--%>
                                    <asp:Button ID="ButtonProductsManagement" runat="server" CssClass="ButtonAddNeutral"
                                        Text="Products Management" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2"></td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">

                                    <div id="DivTodaySpentList" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanelTodaySpentList" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:TodaySpentListControl ID="TodaySpentListControl1" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonSpentToday" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="DivPurchaseHistoryDateSelector" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanelPurchaseHistoryDateSelector" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <uc1:PurchaseHistoryDateSelectorControl ID="PurchaseHistoryDateSelectorControl1" runat="server" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="LinkButtonPurchaseHistory" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="DivPurchaseHistory" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanelPurchaseHistory" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <uc1:PurchaseHistoryControl ID="PurchaseHistoryControl1" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <asp:UpdatePanel ID="UpdatePanelExpectedSum" runat="server" UpdateMode="Always">
                                        <ContentTemplate>

                                            <div id="DivExpectedCostSum">
                                                <div style="float: left;">
                                                    <fieldset>
                                                        <legend>
                                                            <asp:Label ID="LabelCalculationsHeader" runat="server" CssClass="PlainTextExtraLarge"
                                                                Text="Калкулации"></asp:Label></legend>
                                                        <table width="400px">
                                                            <%-- <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostSumText" runat="server" CssClass="PlainTextBoldExtraLarge"
                                                                        Text="Expected cost sum:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostSum" runat="server" CssClass="PlainTextExtraLarge"></asp:Label>
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostSumDifferenceText" runat="server" CssClass="PlainTextBoldExtraLarge"
                                                                        Text="Expected cost difference:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostSumDifference" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostTotalSumDifferenceText" runat="server" CssClass="PlainTextBoldExtraLarge"
                                                                        Text="Expected cost difference (total):"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedCostTotalSumDifference" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelSpentTodayText" runat="server" Text="Today I spent:" CssClass="PlainTextExtraLarge"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButtonSpentToday" runat="server" Text="" CssClass="PlainTextExtraLargePink" OnClientClick="javascript:ShowTodaySpentList();"></asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButtonPurchaseHistory" runat="server" Text="History" CssClass="PlainTextPale" OnClick="LinkButtonPurchaseHistory_Click" OnClientClick="javascript:ShowPurchaseHistoryDateSelector();"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelSumPerDayText" runat="server" Text="Budget per day:" CssClass="PlainTextExtraLarge"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelSumPerDay" runat="server" Text="" CssClass="PlainTextExtraLargeSkyBlue"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedSavingsText" runat="server" CssClass="PlainTextBoldLarge"
                                                                        Text="Savings this month:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelExpectedSavings" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="LabelAllSavingsText" runat="server" CssClass="PlainTextBoldLarge"
                                                                        Text="Yearly savings:"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="LabelAllSavings" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="LinkButtonSumFlaggedBillsText" runat="server" CssClass="LinkButtonLarge"
                                                                        Text="Sum flagged bills:"></asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblSumFlaggedBillsValue" runat="server" CssClass="PlainTextBoldLarge"
                                                                        Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                    <fieldset>
                                                        <legend>
                                                            <asp:Label ID="LabelBudgetsHeader" runat="server" CssClass="PlainTextExtraLarge"
                                                                Text="Бюджети"></asp:Label></legend>
                                                        <table width="400px">
                                                            <tr>
                                                                <td>
                                                                    <uc1:Budgets ID="Budget1" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </div>
                                                <fieldset>
                                                    <legend>
                                                        <asp:Label ID="LabelPieChartHeader" runat="server" CssClass="PlainTextExtraLarge"
                                                            Text="Разпределение разходи"></asp:Label>
                                                    </legend>
                                                    <asp:TabContainer ID="TabContainer1" runat="server" Width="683px">
                                                        <asp:TabPanel ID="TabPanelCategoriesPieChart" runat="server" HeaderText="Categories" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="ImageButtonOpenPieChartCategoriesInNewWindow" runat="server" Style="float: right; padding: 5px 5px 5px 5px;" ImageUrl="~/Images/open_new_window.png"></asp:ImageButton>
                                                                <br />

                                                                <asp:Chart ID="ChartCategories" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                    BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                    ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="690px" Height="370px"
                                                                    BackColor="White">
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                            ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <Legends>
                                                                        <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                            IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Series>
                                                                        <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                            Name="Default">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                            BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                            <Area3DStyle Rotation="0" />
                                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>

                                                                <div id="PanelPieChartCategories" style="display: none;">
                                                                    <asp:Chart ID="ChartCategoriesPopup" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                        BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                        ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="1024px"
                                                                        BackColor="#f2f5f7">
                                                                        <Titles>
                                                                            <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                                ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                            </asp:Title>
                                                                        </Titles>
                                                                        <Legends>
                                                                            <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                                IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                            </asp:Legend>
                                                                        </Legends>
                                                                        <Series>
                                                                            <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                                Name="Default">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                                BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                                <Area3DStyle Rotation="0" />
                                                                                <AxisY LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisY>
                                                                                <AxisX LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisX>
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>
                                                        <asp:TabPanel ID="TabPanelProductsPieChart" runat="server" HeaderText="Products" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="ImageButtonOpenPieChartProductsInNewWindow" runat="server" Style="float: right; padding: 5px 5px 5px 5px;" ImageUrl="~/Images/open_new_window.png"></asp:ImageButton>
                                                                <br />

                                                                <asp:Chart ID="ChartProducts" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                    BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                    ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="690px" Height="370px"
                                                                    BackColor="White">
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                            ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <Legends>
                                                                        <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                            IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Series>
                                                                        <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                            Name="Default">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                            BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                            <Area3DStyle Rotation="0" />
                                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>

                                                                <div id="PanelPieChartProducts" style="display: none;">
                                                                    <asp:Chart ID="ChartProductsPopup" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                        BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                        ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="1024px"
                                                                        BackColor="#f2f5f7">
                                                                        <Titles>
                                                                            <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                                ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                            </asp:Title>
                                                                        </Titles>
                                                                        <Legends>
                                                                            <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                                IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                            </asp:Legend>
                                                                        </Legends>
                                                                        <Series>
                                                                            <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                                Name="Default">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                                BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                                <Area3DStyle Rotation="0" />
                                                                                <AxisY LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisY>
                                                                                <AxisX LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisX>
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>
                                                        <asp:TabPanel ID="TabPanelSuppliersPieChart" runat="server" HeaderText="Suppliers" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:ImageButton ID="ImageButtonOpenPieChartSuppliersInNewWindow" runat="server" Style="float: right; padding: 5px 5px 5px 5px;" ImageUrl="~/Images/open_new_window.png"></asp:ImageButton>
                                                                <br />

                                                                <asp:Chart ID="ChartSuppliers" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                    BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                    ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="690px" Height="370px"
                                                                    BackColor="White">
                                                                    <Titles>
                                                                        <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                            ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                        </asp:Title>
                                                                    </Titles>
                                                                    <Legends>
                                                                        <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                            IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                        </asp:Legend>
                                                                    </Legends>
                                                                    <Series>
                                                                        <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                            Name="Default">
                                                                        </asp:Series>
                                                                    </Series>
                                                                    <ChartAreas>
                                                                        <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                            BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                            <Area3DStyle Rotation="0" />
                                                                            <AxisY LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisY>
                                                                            <AxisX LineColor="64, 64, 64, 64">
                                                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                                            </AxisX>
                                                                        </asp:ChartArea>
                                                                    </ChartAreas>
                                                                </asp:Chart>

                                                                <div id="PanelPieChartSuppliers" style="display: none;">
                                                                    <asp:Chart ID="ChartSuppliersPopup" runat="server" BackImageAlignment="Top" BackSecondaryColor="White"
                                                                        BorderColor="26, 59, 105" BorderDashStyle="Solid" BorderWidth="2"
                                                                        ImageLocation="../WebCharts/ChartPic_#SEQ(300,3)" Palette="Excel" Width="1024px"
                                                                        BackColor="#f2f5f7">
                                                                        <Titles>
                                                                            <asp:Title Font="Trebuchet MS, 14.25pt, style=Bold" ForeColor="26, 59, 105" Name="Title1"
                                                                                ShadowColor="32, 0, 0, 0" ShadowOffset="3">
                                                                            </asp:Title>
                                                                        </Titles>
                                                                        <Legends>
                                                                            <asp:Legend Alignment="Center" BackColor="Transparent" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold"
                                                                                IsTextAutoFit="False" LegendStyle="Column" Name="Default">
                                                                            </asp:Legend>
                                                                        </Legends>
                                                                        <Series>
                                                                            <asp:Series BorderColor="180, 26, 59, 105" ChartType="Pie" Color="220, 65, 140, 240"
                                                                                Name="Default">
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea BackColor="Transparent" BackSecondaryColor="Transparent" BorderColor="64, 64, 64, 64"
                                                                                BorderWidth="0" Name="ChartArea1" ShadowColor="Transparent">
                                                                                <Area3DStyle Rotation="0" />
                                                                                <AxisY LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisY>
                                                                                <AxisX LineColor="64, 64, 64, 64">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisX>
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </div>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>
                                                        <asp:TabPanel Height="400px" ID="TabPanelMostFrequentlyPurchasedProducts" runat="server" HeaderText="Frequently purchased products" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:Panel runat="server" ID="Panel1" Height="400px" ScrollBars="Vertical">
                                                                    <asp:GridView ID="GridViewMostFrequentlyPurchasedProducts" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                                                        ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    &nbsp;
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataFormatString="{0:0.00}" DataField="Amount" HeaderText="Total amount" ItemStyle-CssClass="PlainText" />
                                                                            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="PlainText" />
                                                                            <asp:BoundField DataField="SupplierName" HeaderText="Vendor" ItemStyle-CssClass="PlainText" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <%--  <asp:ImageButton ID="ImageButtonFreqGridEditDetailProduct" runat="server"
                                                                                        ToolTip='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                        AlternateText='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                        ImageUrl="../Images/pencil_faded.png"
                                                                                        onmouseover="this.src='../Images/pencil.png'"
                                                                                        onmouseout="this.src='../Images/pencil_faded.png'"
                                                                                        OnClick='<%#
String.Format("javascript:window.location=""{0}""", URLRewriter.GetLink("ProductsManagement", New KeyValuePair(Of String, String)("pid", CType(Container.DataItem, ProductInfo).ID)))%>'>
                                                                                    </asp:ImageButton>--%>

                                                                                    <input type="image"
                                                                                        onclick='<%#
String.Format("javascript:window.location=""{0}""", URLRewriter.GetLink("ProductsManagement", New KeyValuePair(Of String, String)("pid", CType(Container.DataItem, ProductInfo).ID)))%>'
                                                                                        alt='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                        src="../Images/pencil_faded.png"
                                                                                        onmouseout="this.src='../Images/pencil_faded.png'"
                                                                                        onmouseover="this.src='../Images/pencil.png'"
                                                                                        title='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                        id="ImageButtonFreqGridEditDetailProduct"
                                                                                        name="ImageButtonFreqGridEditDetailProduct" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>

                                                                                    <asp:ImageButton ID="ImageButtonFreqGridProductStatistics" runat="server"
                                                                                        ToolTip='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                        AlternateText='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                        ImageUrl="../Images/product_prices_chart_faded.gif"
                                                                                        onmouseover="this.src='../Images/product_prices_chart.gif'"
                                                                                        onmouseout="this.src='../Images/product_prices_chart_faded.gif'"
                                                                                        OnClick='<%#
 MHB.Web.Environment.GetOpenInCustomWindowScript(URLRewriter.GetLink("DetailsProductPriceStatistics", _
                                                                     New KeyValuePair(Of String, String)("ProductID", CType(Container.DataItem, ProductInfo).ID)), 0, 0, False, True, 1200, _
                                                                 IIf(CType(Container.DataItem, ProductInfo).MeasureType = Enums.MeasureType.Weight OrElse _
                                                                     CType(Container.DataItem, ProductInfo).MeasureType = Enums.MeasureType.Volume, 620, 673))%>'></asp:ImageButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>

                                                        <asp:TabPanel Height="400px" ID="TabPanelSurplusItems" runat="server" HeaderText="Surplus items" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:Panel runat="server" ID="PanelSurplusItems" Height="400px" ScrollBars="Vertical">
                                                                    <asp:GridView ID="GridViewSurplusItems" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                                                        ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    &nbsp;
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="DetailName" HeaderText="Name" ItemStyle-CssClass="PlainText" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="LabelValueSurplusItem" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).DetailValue.ToString("0.00") %>' CssClass="PlainText"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="LabelTotalSumSurplusItems" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataFormatString="{0:0.00}" DataField="Amount" HeaderText="Total amount" ItemStyle-CssClass="PlainText" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="LabelSupplierNameSurplusItem" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).Supplier.Name  %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>

                                                        <asp:TabPanel Height="400px" ID="TabPanelLastEditedItems" runat="server" HeaderText="Last edited items" BackColor="White">
                                                            <ContentTemplate>
                                                                <asp:Panel runat="server" ID="Panel2" Height="400px" ScrollBars="Vertical">
                                                                    <asp:GridView ID="GridViewLastEditedItems" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                                                        ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    &nbsp;
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataField="DetailDate" HeaderText="Name" ItemStyle-CssClass="PlainText" />
                                                                            <asp:BoundField DataField="DetailName" HeaderText="Name" ItemStyle-CssClass="PlainText" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="LabelValueLastEditedItem" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).DetailValue.ToString("0.00") %>' CssClass="PlainText"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="LabelTotalSumLastEditedItems" runat="server" CssClass="PlainTextBoldLarge"></asp:Label>
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>

                                                                            <asp:BoundField DataFormatString="{0:0.00}" DataField="Amount" HeaderText="Total amount" ItemStyle-CssClass="PlainText" />

                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="LabelSupplierNameLastEditedItem" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).Supplier.Name  %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </ContentTemplate>
                                                        </asp:TabPanel>
                                                    </asp:TabContainer>
                                                </fieldset>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="font-family: Arial, Helvetica, sans-serif; font-size: 12px; font-weight: bold;"
                                    colspan="2">

                                    <div id="PanelDetailsTable" style="display: none;" onkeypress="javascript:return WebForm_FireDefaultButton(event, '<%= ButtonAddExpenditureDetails.ClientID%>')">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelDetailsActionButtons" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="ButtonAddExpenditureDetails" runat="server" CssClass="ButtonAddInsert"
                                                                Text="Add" />
                                                            <asp:Button ID="ButtonUpdateDetailsTable" runat="server" CssClass="ButtonAddSave" Text="Save changes" />
                                                            <asp:Button ID="ButtonDeleteFromDetailsTable" runat="server" CssClass="ButtonAddRemove"
                                                                Text="Delete selected" />
                                                            <asp:Button ID="ButtonDetailsTableAttach" runat="server" CssClass="ButtonAddMedium" OnClientClick="javascript:ShowDetailsAttach(''); return false;" Text="Attach" />
                                                            <asp:Button ID="ButtonDetailsPrintShoppingList" runat="server" CssClass="ButtonAddShoppingList" Text="Print shoppint list" OnClientClick="javascript:confirmSelectDetailsPrintShoppingListMode();return false;" />

                                                            <asp:Button ID="ButtonAllDetailsForCategory" runat="server" CssClass="ButtonAddMedium" Text="All for this year" />
                                                            <asp:TextBox ID="TextBoxDetailsGridSearch" runat="server" CssClass="PlainTextBox"></asp:TextBox>
                                                            <asp:DropDownList ID="DropDownListDetailsPageSizeTop" runat="server" Width="50px" AutoPostBack="true">
                                                                <asp:ListItem Text="20" Value="20" Selected="True" />
                                                                <asp:ListItem Text="50" Value="50" />
                                                                <asp:ListItem Text="100" Value="100" />
                                                                <asp:ListItem Text="150" Value="150" />
                                                                <asp:ListItem Text="200" Value="200" />
                                                                <asp:ListItem Text="All" Value="-1" />
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    <div id="dialog-modal" title='<%= Me.GetTranslatedValue("PrintShoppingListDetailsModeSelectionTitle", Me.CurrentLanguage)%>' style="display: none;">
                                                        <p>
                                                            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 0 0;"></span>
                                                            <%= Me.GetTranslatedValue("PrintShoppingListDetailsModeSelectionQuestion", Me.CurrentLanguage)%>
                                                        </p>
                                                        <p>To continue editing, click cancel.</p>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span class="PlainTextBold"><%= Me.GetTranslatedValue("SelectedSupplier", Me.CurrentLanguage) %></span>
                                                    <a id="LinkButtonSelectedDetailsSupplier" href="javascript:ShowNewDetailsSupplierSelectorDiv('');"><%= Me.GetTranslatedValue("SelectSupplier", Me.CurrentLanguage) %></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="DivWeightVolumeSelector" style="display: none;">
                                                        <asp:RadioButtonList ID="RadioButtonListWeightVolumeSelector" runat="server" onchange="javascript:CloseDivWeightVolumeSelector(true);">
                                                            <asp:ListItem Text="Weight" Value="1" />
                                                            <asp:ListItem Text="Volume" Value="2" />
                                                            <asp:ListItem Text="Count" Value="3" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </div>

                                                    <%--<script>
                                                          $(function() {
                                                            $("#tabs").tabs();
                                                          });
                                                    </script>--%>

                                                    <asp:UpdatePanel ID="UpdatePanelDetails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="HiddenSelectedLastEditedParentExpenditureTabIndex" runat="server" />
                                                            <div id="tabs" style="border: none !important; background: transparent! important;">
                                                                <ul runat="server" id="DetailsGridEditedCategoriesTabPanel" style="border: none !important; background: transparent! important;">
                                                                    <%--<li><a href="#tabs-1"></a></li>--%>
                                                                </ul>
                                                                <asp:GridView ID="GridViewDetails" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                                                    ShowFooter="true" BorderWidth="0" CssClass="GridDetails" AllowPaging="true" PageSize="20" AllowSorting="true">
                                                                    <Columns>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LabelDetailsGridRowIndex" runat="server" CssClass="PlainTextPale" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                                &nbsp;
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="CheckBoxDetailsTableSelect" runat="server" ProductID='<%# CType(Container.DataItem, ExpenditureDetail).ProductID %>' DetailID='<%# CType(Container.DataItem, ExpenditureDetail).ID %>' />
                                                                                <asp:ImageButton ID="ImageButtonCopyPasteDetail" runat="server"
                                                                                    ImageUrl="~/Images/page_white_copy_faded.png"
                                                                                    onmouseover="this.src='../Images/page_white_copy.png'"
                                                                                    onmouseout="this.src='../Images/page_white_copy_faded.png'"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("MoveSelectedDetailsToAnotherParentTooltip", Me.CurrentLanguage) %>'
                                                                                    OnClientClick="javascript:return MoveSelectedDetails();"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                    CommandName="CopyPasteDetail" AlternateText="Duplicate record" />
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:LinkButton ID="LinkButtonDetailsMerge" runat="server" Text="Merge" CommandName="Merge" CommandArgument="-1"></asp:LinkButton>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="50px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailsTableID" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).ID %>'
                                                                                    Visible="false"></asp:TextBox>
                                                                                <asp:TextBox ID="TextBoxDetailsTableProductID" runat="server" Text='<%# CType(Container.DataItem, ExpenditureDetail).ProductID%>'
                                                                                    Visible="false"></asp:TextBox>
                                                                                <asp:CheckBox ID="CheckBoxDetTblHasAttachment" runat="server" Checked='<%# IIf(IsDBNull(CType(Container.DataItem, ExpenditureDetail).HasAttachment), "False", CType(Container.DataItem, ExpenditureDetail).HasAttachment) %>'
                                                                                    Visible="false" />
                                                                                <asp:ImageButton ID="ImageDetTblHasAttachment" runat="server" CommandName="Edit"
                                                                                    ImageUrl="../Images/attach.png" Visible="false" />
                                                                                <asp:ImageButton ID="ImageDetTblDeleteAttachment" runat="server" CommandName="Delete"
                                                                                    ImageUrl="../Images/delete_attachment.gif" Visible="false" OnClientClick="javascript:return confirm('really delete?')" />
                                                                                <asp:TextBox ID="TextBoxUserDetailsTableExpenditureID" runat="server"
                                                                                    Text='<%# CType(Container.DataItem, ExpenditureDetail).ExpenditureID %>' Visible="false"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDetailAmountNew" runat="server"
                                                                                    ControlToValidate="TextBoxDetailAmountNew" CssClass="Validator" ErrorMessage="!!!"
                                                                                    ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                                <asp:TextBox ID="TextBoxDetailAmountNew" runat="server" Width="23px" Text="1" onkeypress="SelectDetailsNameInput();" onblur="ShowDivWeightVolumeSelector();"></asp:TextBox>
                                                                                &nbsp;<strong>x</strong>&nbsp;
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="100px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Name" SortExpression="DetailName">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailsTableFieldName" runat="server" CssClass="GridDetailsCells"
                                                                                    Text='<%# CType(Container.DataItem, ExpenditureDetail).DetailName%>' DetailID='<%# CType(Container.DataItem, ExpenditureDetail).ID %>' TabIndex="-1" Width="98%" ToolTip='<%# CType(Container.DataItem, ExpenditureDetail).Amount %>'></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidatorDetailAmountNew" runat="server"
                                                                                ControlToValidate="TextBoxDetailAmountNew" CssClass="Validator" ErrorMessage="!!!"
                                                                                ValidationExpression="^[\s]*[1-9][0-9]?[\s]*$"></asp:RegularExpressionValidator>--%>
                                                                                <asp:TextBox ID="TextBoxDetailNameNew" runat="server" Width="95%" Text="" Style="z-index: 999;"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Description" SortExpression="DetailDescription">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailsTableFieldDescription" runat="server" CssClass="GridDetailsCells"
                                                                                    Text='<%# CType(Container.DataItem, ExpenditureDetail).DetailDescription %>' Width="98%"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailDescriptionNew" runat="server" Text="" Width="95%"></asp:TextBox>
                                                                            </FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Value" SortExpression="DetailValue">
                                                                            <ItemTemplate>
                                                                                <div style="float: left; width: 90%;">
                                                                                    <asp:TextBox ID="TextBoxDetailsTableFieldValue" runat="server" CssClass="GridDetailsCells"
                                                                                        Text='<%# String.Format("{0:f}", CType(Container.DataItem, ExpenditureDetail).DetailValue) %>'></asp:TextBox>
                                                                                </div>
                                                                                <div style="float: right; width: 10%;">
                                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                                                                        ControlToValidate="TextBoxDetailsTableFieldValue" CssClass="Validator" ErrorMessage="!!!"
                                                                                        ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailValueNew" runat="server" Text="" Width="57px"></asp:TextBox>
                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDetailNewValue" runat="server"
                                                                                    ControlToValidate="TextBoxDetailValueNew" CssClass="Validator" ErrorMessage="!!!"
                                                                                    ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Amount">
                                                                            <ItemTemplate>
                                                                                <div style="float: left; width: 90%;">
                                                                                    <asp:TextBox ID="TextBoxDetailsTableFieldAmount" runat="server" CssClass="GridDetailsCells"
                                                                                        Text='<%# String.Format("{0:0.000}", CType(Container.DataItem, ExpenditureDetail).Amount) %>'
                                                                                        Visible="false"></asp:TextBox>
                                                                                </div>
                                                                                <div style="float: right; width: 10%;">
                                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorDetailsTableAmount" runat="server"
                                                                                        ControlToValidate="TextBoxDetailsTableFieldAmount" CssClass="Validator" ErrorMessage="!!!"
                                                                                        ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="LabelDetailsTableFieldAmount" runat="server" CssClass="PlainTextBoldLarge"
                                                                                    Text='<%# String.Format("{0:f}", Me.DetailsDataSource.Sum(Function(d) d.Amount)) %>'
                                                                                    Visible='<%# Me.DetailsDataSource.All(Function(d) d.MeasureType = MHB.BL.Enums.MeasureType.Volume OrElse d.MeasureType = MHB.BL.Enums.MeasureType.Weight) %>'></asp:Label>
                                                                                <span id="detailsspanSelectedSum" class="PlainTextBoldExtraLarge"></span>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="80px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Mileage">
                                                                            <ItemTemplate>
                                                                                <div style="float: left; width: 90%;">
                                                                                    <asp:TextBox ID="TextBoxDetailsTableMileage" runat="server" CssClass="GridDetailsCells"></asp:TextBox>
                                                                                </div>
                                                                                <div style="float: right; width: 10%;">
                                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorMilage" runat="server"
                                                                                        ControlToValidate="TextBoxDetailsTableMileage" CssClass="Validator" ErrorMessage="!!!"
                                                                                        ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="LabelDetailsTableFieldMileage" runat="server" CssClass="PlainTextBoldLarge"
                                                                                    Text='<%# String.Format("{0:f}", Me.DetailsDataSource.Where(Function(d) d.HasProductParameters = True AndAlso d.ProductID <> Product.PRODUCT_DEFAULT_ID).SelectMany(Function(d) d.Product.Parameters).Where(Function(p) p.ProductParameterTypeID = ProductParameterType.Mileage).Sum(Function(p) p.Value)) %>'></asp:Label>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="70px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Avg. Consumption" SortExpression="AverageConsumption">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextBoxDetailsTableAverageConsumption" runat="server" CssClass="GridDetailsCells" Visible="false"
                                                                                    Text='<%# String.Format("{0:0.00}", CType(Container.DataItem, ExpenditureDetail).AverageConsumption) %>'
                                                                                    onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                                    onmouseout="this.style='opacity: 0.3; filter: alpha(opacity=30);'"
                                                                                    Style="opacity: 0.3; filter: alpha(opacity=30);"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                <asp:Label ID="LabelDetailsTableAverageConsumption" runat="server" CssClass="PlainTextBoldLarge">
                                                                                </asp:Label>
                                                                            </FooterTemplate>
                                                                            <ItemStyle Width="70px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField HeaderText="Date" SortExpression="DetailDate">
                                                                            <ItemTemplate>
                                                                                <asp:TextBox ID="TextBoxFieldDetailsTableDate" runat="server" CssClass="GridDetailsCells"
                                                                                    Text='<%# CType(Container.DataItem, ExpenditureDetail).DetailDate %>'
                                                                                    onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                                    onmouseout="this.style='opacity: 0.3; filter: alpha(opacity=30);'"
                                                                                    Style="opacity: 0.3; filter: alpha(opacity=30);"></asp:TextBox>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="180px" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>

                                                                                <asp:ImageButton ID="ImageButtonAddDetailsProduct" runat="server"
                                                                                    ImageUrl="~/Images/add_product_faded.png"
                                                                                    onmouseover="this.src='../Images/add_product.png'"
                                                                                    onmouseout="this.src='../Images/add_product_faded.png'"
                                                                                    Visible='<%# CType(Container.DataItem, ExpenditureDetail).ProductID = Product.PRODUCT_DEFAULT_ID%>'
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                    CommandName="Select"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("CreateProduct", Me.CurrentLanguage) %>'
                                                                                    AlternateText='<%# Me.GetTranslatedValue("CreateProduct", Me.CurrentLanguage) %>'
                                                                                    OnClientClick='<%# String.Format("javascript:ShowNewProductsDiv(""+ {0}"", {1});", CType(Container.DataItem, ExpenditureDetail).DetailName, CType(Container.DataItem, ExpenditureDetail).ID)%>' />

                                                                                <asp:ImageButton ID="ImageButtonAddDetailsProductToShoppingList" runat="server"
                                                                                    ImageUrl="~/Images/add_to_shopping_list_faded.gif"
                                                                                    onmouseover="this.src='../Images/add_to_shopping_list.gif'"
                                                                                    onmouseout="this.src='../Images/add_to_shopping_list_faded.gif'"
                                                                                    Visible='<%# CType(Container.DataItem, ExpenditureDetail).ProductID <> 1 AndAlso Not Me.ShoppingList.Any(Function(p) p.Item2.ID = CType(Container.DataItem, ExpenditureDetail).ProductID)%>'
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                    CommandName="AddToShoppingListDetails"
                                                                                    ToolTip='<%# Me.DetailsDataSource.Where(Function(d) d.ProductID <> Product.PRODUCT_DEFAULT_ID AndAlso d.ProductID = CType(Container.DataItem, ExpenditureDetail).ProductID).Select(Function(d) d.DetailValue).Sum()%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField ItemStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                &nbsp;&nbsp;
                                                                            <asp:ImageButton ID="ImageButtonMeasureType" runat="server"
                                                                                ImageUrl='<%# IIf(CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Count, "~/Images/measure_type_count_details.png", IIf(CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Volume, "~/Images/measure_type_volume_details.png", "~/Images/measure_type_weight_details.png")) %>'
                                                                                onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                                onmouseout="this.style='opacity: 0.4; filter: alpha(opacity=40);'"
                                                                                Style="opacity: 0.4; filter: alpha(opacity=40);"
                                                                                CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                CommandName="ChangeMeasureTypeDetail"
                                                                                ToolTip='<%# CType(Container.DataItem, ExpenditureDetail).MeasureType.ToString()%>'
                                                                                Enabled='<%# CType(Container.DataItem, ExpenditureDetail).Product IsNot Nothing AndAlso Not CType(Container.DataItem, ExpenditureDetail).Product.IsFixedMeasureType %>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonEditDetailProduct" runat="server"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                    AlternateText='<%# Me.GetTranslatedValue("EditDetailProduct", Me.CurrentLanguage) %>'
                                                                                    ImageUrl="../Images/pencil_faded.png"
                                                                                    onmouseover="this.src='../Images/pencil.png'"
                                                                                    onmouseout="this.src='../Images/pencil_faded.png'"
                                                                                    CommandName="EditDetailProduct"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                    Visible='<%# CType(Container.DataItem, ExpenditureDetail).ProductID <> Product.PRODUCT_DEFAULT_ID%>'></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonDetailProductStatistics" runat="server"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                    AlternateText='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                    ImageUrl="../Images/product_prices_chart_faded.gif"
                                                                                    onmouseover="this.src='../Images/product_prices_chart.gif'"
                                                                                    onmouseout="this.src='../Images/product_prices_chart_faded.gif'"
                                                                                    CommandName="DetailProductPriceStatistics"
                                                                                    Visible='<%# CType(Container.DataItem, ExpenditureDetail).ProductID <> Product.PRODUCT_DEFAULT_ID%>'
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'
                                                                                    OnClick='<%# MHB.Web.Environment.GetOpenInCustomWindowScript(URLRewriter.GetLink("DetailsProductPriceStatistics",
                                                                                                                                                         New KeyValuePair(Of String, String)("ProductID", CType(Container.DataItem, ExpenditureDetail).ProductID)), 0, 0, False, True, 1200,
                                                                                                                                                     IIf(CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Weight OrElse
                                                                                                                                                         CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Volume, 620, 673))%>'></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonDetailRejectSuggestedProduct" runat="server"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("ImageButtonDetailRejectSuggestedProductTooltip", Me.CurrentLanguage) %>'
                                                                                    AlternateText='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                    ImageUrl="../Images/general-delete-icon_faded.png"
                                                                                    onmouseover="this.src='../Images/general-delete-icon.png'"
                                                                                    onmouseout="this.src='../Images/general-delete-icon_faded.png'"
                                                                                    CommandName="DetailProductRejectSuggestedProduct"
                                                                                    Visible="false"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:DropDownList ID="DropDownListDetailsGridProductSuggest" runat="server" Visible="false" Width="130px"></asp:DropDownList>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageButtonDetailApproveSuggestedProduct" runat="server"
                                                                                    ToolTip='<%# Me.GetTranslatedValue("ImageButtonDetailApproveSuggestedProductTooltip", Me.CurrentLanguage) %>'
                                                                                    AlternateText='<%# Me.GetTranslatedValue("DetailProductStatistics", Me.CurrentLanguage) %>'
                                                                                    ImageUrl="../Images/tick_circle.png"
                                                                                    onmouseover="this.src='../Images/tick_circle.png'"
                                                                                    onmouseout="this.src='../Images/tick_circle_faded.png'"
                                                                                    CommandName="DetailProductApproveSuggestedProduct"
                                                                                    Visible="false"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>'></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton
                                                                                    ID="ImageButtonShareDetailAttachment"
                                                                                    runat="server"
                                                                                    ToolTip="Share attachment"
                                                                                    onmouseover="this.src='../Images/sharethis-24.png'"
                                                                                    onmouseout='<%# IIf(CType(Container.DataItem, ExpenditureDetail).IsShared, "this.src=""../Images/sharethis-24.png""", "this.src=""../Images/sharethis-24_faded.png""")%>'
                                                                                    ImageUrl='<%# IIf(CType(Container.DataItem, ExpenditureDetail).IsShared, "../Images/sharethis-24.png", "../Images/sharethis-24_faded.png")%>'
                                                                                    Visible='<%# CType(Container.DataItem, ExpenditureDetail).HasAttachment%>'
                                                                                    CommandName="ShareDetailAttachment"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="#" SortExpression="CategoryID">
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageCategory" runat="server" Visible="false"
                                                                                    onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                                    onmouseout="this.style='opacity: 0.4; filter: alpha(opacity=40);'"
                                                                                    Style="opacity: 0.4; filter: alpha(opacity=40);"
                                                                                    OnClick='<%# MHB.Web.Environment.GetOpenInCustomWindowScript(URLRewriter.GetLink("DetailsProductPriceStatistics",
                                                                                                                                             New KeyValuePair(Of String, String)("CategoryID", CType(Container.DataItem, ExpenditureDetail).CategoryID), New KeyValuePair(Of String, String)("IsCategoryStatistic", True)), 0, 0, False, True, 1200,
                                                                                                                                         IIf(CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Weight OrElse
                                                                                                                                             CType(Container.DataItem, ExpenditureDetail).MeasureType = Enums.MeasureType.Volume, 620, 673))%>'></asp:ImageButton>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:ImageButton ID="ImageMarkSurplus" runat="server"
                                                                                    ImageUrl="../Images/cart_delete.png"
                                                                                    onmouseover="this.style='opacity: 1.0; filter: alpha(opacity=100);'"
                                                                                    onmouseout='<%# IIf(CType(Container.DataItem, ExpenditureDetail).IsSurplus, "this.style=""opacity: 1.0; filter: alpha(opacity=100);""", "this.style=""opacity: 0.3; filter: alpha(opacity=30);""")%>'
                                                                                    Style='<%# IIf(CType(Container.DataItem, ExpenditureDetail).IsSurplus, "opacity: 1.0; filter: alpha(opacity=100);", "opacity: 0.3; filter: alpha(opacity=30);")%>'
                                                                                    CommandName="MarkSurplus"
                                                                                    CommandArgument='<%# CType(Container.DataItem, ExpenditureDetail).ID%>' />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>

                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="LabelDetailsSupplier" runat="server" Text='<%# Me.GetSupplierName(CType(Container.DataItem, ExpenditureDetail).Supplier, False)%>' ToolTip='<%# Me.GetSupplierName(CType(Container.DataItem, ExpenditureDetail).Supplier, True)%>' CssClass="PlainTextTiny"></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <PagerStyle HorizontalAlign="Right" CssClass="GridPager" />
                                                                    <EmptyDataTemplate>
                                                                        No records.
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </div>
                                                            <asp:GridView ID="GridViewDetailsProductsInfo" runat="server" AutoGenerateColumns="False" GridLines="Horizontal"
                                                                ShowFooter="true" BorderWidth="0" CssClass="GridDetails" HeaderStyle-HorizontalAlign="Left" HeaderStyle-CssClass="PlainTextBold">
                                                                <Columns>
                                                                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Amount" HeaderText="Amount" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Total" HeaderText="Total" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Average" HeaderText="Average" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Max" HeaderText="Max" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Min" HeaderText="Min" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="Consumption" HeaderText="Consumption" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="PricePerHundredKms" HeaderText="PricePer100kms" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="PricePerKm" HeaderText="PricePerkm" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="ExpectedQuantity" HeaderText="ExpectedQuantity" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="ExpectedRange" HeaderText="ExpectedRange" ItemStyle-CssClass="PlainText" />
                                                                    <asp:BoundField DataFormatString="{0:0.00}" DataField="RemainingRange" HeaderText="RemainingRange" ItemStyle-CssClass="PlainText" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonAddExpenditureDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonUpdateDetailsTable" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonDeleteFromDetailsTable" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonDetailsAddBottom" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonDetailsSaveBottom" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="BittonDetailsDeleteBottom" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="ButtonAllDetailsForCategory" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="ListBoxParentExpenditures" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListDetailsPageSizeTop" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="DropDownListDetailsPageSizeBottom" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelDetailsActionButtonsBottom" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="ButtonDetailsAddBottom" runat="server" CssClass="ButtonAddInsert" Text="Add" />
                                                            <asp:Button ID="ButtonDetailsSaveBottom" runat="server" CssClass="ButtonAddSave" Text="Save changes" />
                                                            <asp:Button ID="BittonDetailsDeleteBottom" runat="server" CssClass="ButtonAddRemove"
                                                                Text="Delete selected" />
                                                            <asp:Button ID="ButtonDetailsTableAttachBottom" runat="server" CssClass="ButtonAddMedium" OnClientClick="javascript:ShowDetailsAttach(''); return false;" Text="Attach" />
                                                            <asp:Button ID="ButtonDetailsPrintShoppingListBottom" runat="server" CssClass="ButtonAddShoppingList" Text="Print shoppint list" OnClientClick="javascript:confirmSelectDetailsPrintShoppingListMode();return false;" />
                                                            <asp:DropDownList ID="DropDownListDetailsPageSizeBottom" runat="server" Width="50px" AutoPostBack="true">
                                                                <asp:ListItem Text="20" Value="20" Selected="True" />
                                                                <asp:ListItem Text="50" Value="50" />
                                                                <asp:ListItem Text="100" Value="100" />
                                                                <asp:ListItem Text="150" Value="150" />
                                                                <asp:ListItem Text="200" Value="200" />
                                                                <asp:ListItem Text="All" Value="-1" />
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelAjaxFileUploadReceipt" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:AjaxFileUpload ID="AjaxFileUploadReceipt" runat="server" OnUploadComplete="AjaxFileUploadReceipt_UploadComplete" OnClientUploadComplete="ClientUploadComplete" />
                                                            <script type="text/javascript">

                                                                function ClientUploadComplete(sender, args) {

                                                                    if (sender._filesInQueue[sender._filesInQueue.length - 1]._isUploaded)
                                                                        __doPostBack('AjaxFileUploadReceipt', ''); // Do post back only after all files have been uploaded
                                                                }
                                                            </script>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="UpdatePanelDetailsGridChartContainer" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <div id="detailsGridChartContainer"></div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <!-- ALWAYS, ALWAYS, ALWAYS PUT THE UPDATEPANEL INSIDE THE JQUERY DIALOG DIV! ALWAYS ALWAYS ALWAYS-->

                                    <div id="MoveDetailsSelectParent" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanelMoveDetailsSelectParent" runat="server">
                                            <ContentTemplate>
                                                <asp:ListBox ID="ListBoxParentExpenditures" runat="server" CssClass="ListBoxMoveDetailsSelectParent" AutoPostBack="true"></asp:ListBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="DivEnterNewProductDetails" style="display: none;">
                                        <asp:UpdatePanel ID="UpdatePanelCreateDetailsProduct" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductName" runat="server" CssClass="PlainTextBold" Text="Name:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductName" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductDescription" runat="server" CssClass="PlainTextBold" Text="Description:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductDescription" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductKeywords" runat="server" CssClass="PlainTextBold" Text="Keywords:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductKeywords" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductListPrice" runat="server" CssClass="PlainTextBold" Text="List price:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductListPrice" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                                ControlToValidate="TextBoxNewProductListPrice" CssClass="Validator" ErrorMessage="!!!"
                                                                ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductStandardCost" runat="server" CssClass="PlainTextBold" Text="Standard cost:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductStandardCost" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                                ControlToValidate="TextBoxNewProductStandardCost" CssClass="Validator" ErrorMessage="!!!"
                                                                ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductVolume" runat="server" CssClass="PlainTextBold" Text="Volume:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductVolume" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                                                ControlToValidate="TextBoxNewProductVolume" CssClass="Validator" ErrorMessage="!!!"
                                                                ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductWeight" runat="server" CssClass="PlainTextBold" Text="Weight:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="TextBoxNewProductWeight" runat="server" Width="200px" CssClass="PlainTextBox"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                                                ControlToValidate="TextBoxNewProductWeight" CssClass="Validator" ErrorMessage="!!!"
                                                                ValidationExpression="^[\s]*\$?\d*[0-9](|.\d*[0-9]|,\d*[0-9])?[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductSupplier" runat="server" CssClass="PlainTextBold" Text="Vendor:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DropDownListNewProductSupplier" runat="server" Width="203px"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="LabelNewProductCategory" runat="server" CssClass="PlainTextBold" Text="Category:"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="DropDownListNewProductCategory" runat="server" Width="203px"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button ID="ButtonAddNewProduct" runat="server" Text="Add product" CssClass="ButtonAddInsert" OnClientClick="$('#DivEnterNewProductDetails').dialog('close');" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="GridViewDetails" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>

                                    <div id="DivPickNewDetailsSupplier" style="display: none;">
                                        <uc1:SupplierSelector ID="SupplierSelector1" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <div id="PanelCalculator" style="display: none;">
                                        <iframe id="I1" frameborder="0" height="220" name="I1" scrolling="no" src="Calculator.htm"
                                            width="230">
                                            <p>
                                                Your browser does not support iframes.
                                            </p>
                                        </iframe>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="HiddenTextBoxRefreshGrid" runat="server" Style="visibility: hidden"></asp:TextBox>
                                    <asp:TextBox ID="HiddenTextBoxSelectedRow" runat="server" Style="visibility: hidden"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <div id="PanelNotes" style="display: none;">
                                        <uc1:Note ID="Note1" runat="server" />
                                    </div>
                                    <div id="PanelSearch" style="display: none;">
                                        <uc1:Search ID="SearchControl" runat="server" />
                                    </div>
                                    <div id="PanelEditControlTranslation" style="display: none;">
                                        <table>
                                            <tr>
                                                <td class="PlainTextBold">Control ID:</td>
                                                <td>
                                                    <asp:TextBox ID="TextBoxControlID" runat="server" CssClass="PlainTextBox" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="PlainTextBold">Current translation:</td>
                                                <td>
                                                    <asp:TextBox ID="TextBoxCurrentTranslation" runat="server" CssClass="PlainTextBox" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="PlainTextBold">New translation:</td>
                                                <td>
                                                    <asp:TextBox ID="TextBoxNewTranslation" runat="server" CssClass="PlainTextBox" Width="250px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button ID="ButtonSaveNewTranslation" runat="server" CssClass="ButtonAddSmall" Text="Save" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div style="color: #FFFFFF; font-size: 1px;">
        лични финанси, управление лични финанси, финанси, лични, програма лични финанси,
        домашни разходи, домашен, разход, разходи, домашни, домакинство, планиране домакинство,
        разходи домакинство, месечен бюджет, планиране бюджет, програма бюджет, програма
        семеен бюджет, програма месечен бюджет, нещо семеен бюджет, планиране пари, планиране
        пари месец, заплата, планиране заплата, разпределяне заплата mese4en biudjet, mesechen
        budget, budget, programa mese4en biudjet, planirane bjudjet, semeen biudget, planirane
        zaplata, zaplata, planirane, bjudjet, biudget, budget, monthly budget, family budget,
        family bills, myhomebills, my home bills, home expenses, household, expenditure,
        cost
    </div>
    <div id="DivModal" class="ModalDiv">
    </div>
</asp:Content>