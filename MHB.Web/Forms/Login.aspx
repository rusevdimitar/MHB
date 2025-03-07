<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeBehind="Login.aspx.vb" Inherits="MHB.Web.Login" EnableViewStateMac="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div align="center">
        <div class="LoginPageMainTable">
            <table class="ContentTable">
                <tr>
                    <td align="left">
                        <div id="LogoPanel" class="LogoPanel">
                            <asp:ImageButton ID="ImageButtonDemo" runat="server" ImageUrl="../Images/demo_button_normal_1.png"
                                CssClass="DemoButton" />
                            <asp:ImageButton ID="ImageButtonScreenShots" runat="server" ImageUrl="../Images/scrnsht_button_normal_1.png"
                                CssClass="ScrnshtButton" OnClientClick="javascript:window.open('ScreenShots.htm','MyHomeBills','location=0,status=0,scrollbars=0,width=830,height=630');" />
                                                  
                            <asp:ImageButton ID="ImageButtonDownloadInstaller" runat="server" ImageUrl="../Images/btn_download_standalone.png" 
                                CssClass="InstallerButton" OnClientClick="javascript:window.location.href='BuyStandalone.aspx';return false;" />

                        </div>
                    </td>
                    <td>
                        <div id="LoginPanel" class="LoginPanel">
                            <asp:Panel ID="PanelLogin" runat="server" DefaultButton="ButtonLogin">
                                <table style="width: 100%;" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="left">&nbsp;&nbsp;&nbsp;<asp:Label ID="LabelSignUp" runat="server" CssClass="PlainTextBoldLarge"
                                            Text="Sign in to MyHomeBills ..."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">&nbsp;
                                        </td>
                                        <td class="PlainTextBold" align="left">
                                            <asp:Label ID="LabelEmail" runat="server" Text="E-Mail:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxEmail" runat="server" CssClass="LoginTextBox" oncopy="return false" onpaste="return false" oncut="return false" oncontextmenu="return false"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" WatermarkCssClass="TextWaterMark"
                                                TargetControlID="TextBoxEmail" runat="server">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">&nbsp;
                                        </td>
                                        <td style="font-family: Arial, Helvetica, sans-serif; font-size: 10px" align="left">
                                            <asp:Label ID="LabelEgEmail0" runat="server" Text="(e.g. your.email@domain.com)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">&nbsp;
                                        </td>
                                        <td class="PlainTextBold" align="left">
                                            <asp:Label ID="LabelPassword" runat="server" Text="Password:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="PlainTextBold">&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxPassword" runat="server" TextMode="Password" CssClass="LoginTextBox" oncopy="return false" onpaste="return false" oncut="return false" oncontextmenu="return false"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" WatermarkCssClass="TextWaterMark"
                                                TargetControlID="TextBoxPassword" runat="server">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="ButtonLogin" runat="server" CssClass="FancyButton" Text="Login" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:LinkButton ID="LinkButtonForgottenPasswod" runat="server" Text="Forgot your password?"></asp:LinkButton>
                                            <div id="DivRecoverPass" runat="server" style="visibility: hidden;">
                                                <table>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="LabelEnterEmailToRecover" runat="server" CssClass="PlainTextBold"
                                                                Text="Type you email:"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:TextBox ID="TextBoxEnterEmailToRecover" runat="server" CssClass="LoginTextBox"
                                                                Width="285px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="ButtonRecoverPassword" runat="server" CssClass="PlainButton" Text="Send me my password" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="ButtonCloseRecoverPassDiv" runat="server" CssClass="PlainButton"
                                                                Text="Close" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorRecoverPassEmail" runat="server"
                                                                ControlToValidate="TextBoxEnterEmailToRecover" CssClass="ValidatorSmall" ErrorMessage="This is not a valid email"
                                                                ValidationExpression="^[\s]*(?:[a-zA-Z0-9_'^&amp;amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)[\s]*$"></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorLoginPage1" runat="server"
                                                ControlToValidate="TextBoxEmail" CssClass="ValidatorSmall" ErrorMessage="This is not a valid email"
                                                ValidationExpression="^[\s]*(?:[a-zA-Z0-9_'^&amp;amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)[\s]*$"></asp:RegularExpressionValidator><br />
                                            <asp:Label ID="LabelError" runat="server" CssClass="LabelError"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="PlainTextLarge" align="left" valign="top" style="padding: 10px;">
                        <%= Me.GetTranslatedValue("LoginPageText", Me.CurrentLanguage)%>
                        <table>
                            <tr>
                                <td>
                                    <a href="//semeistvo.bg/">
                                        <img alt="Приятелите на MyHomeBills - Семейство.БГ" style="border: none;" class=""
                                            src="../Banners/semeistvo.bg.gif" /></a>
                                </td>
                                <td>
                                    <a href="//twitter.com/#!/MyHomeBills">
                                        <img alt="Намерете ни в Twitter!" style="border: none;" src="../Banners/Twitter_banner.jpg" /></a>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <br />
                                    <strong>
                                        <%= Me.GetTranslatedValue("donate", Me.CurrentLanguage)%></strong>
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <a id="paypalDonate" target="_blank" href="https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=G9F2JE7SKUHUA">
                                        <img border="0" id="payPalImage" src="https://www.paypal.com/en_US/i/btn/btn_donateCC_LG.gif"
                                            alt="Donate to MyHomeBills" />
                                    </a>
                                </td>
                                <td colspan="2">
                                    <a id="epayDonate" target="_blank" href="https://www.epay.bg/en/?PAGE=paylogin&MIN=5081628351&INVOICE=&TOTAL=4.99&DESCR=Подкрепете MyHomeBills като дарите малка сума&URL_OK=https://www.epay.bg/?p=thanks&URL_CANCEL=https://www.epay.bg/?p=cancel">
                                        <img border="0" src="//online.datamax.bg/epaynow/b03.gif" name="BUTTON:EPAYNOW"
                                            alt="Подкрепете MyHomeBills" title="Подкрепете MyHomeBills" />
                                    </a>
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                    <td>
                        <div id="RegisterPanel" class="RegisterPanel">
                            <asp:Panel ID="PanelRegister" runat="server" DefaultButton="ButtonRegister">
                                <table cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="LabelRegister" runat="server" CssClass="PlainTextBoldLarge" Text="... or register in under 5 seconds!&nbsp;&nbsp;"></asp:Label>&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left" class="PlainTextBold">
                                            <asp:Label ID="LabelTypeYourEmail" runat="server" Text="Type your email:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxRegisterEmail" runat="server" CssClass="LoginTextBox" oncopy="return false" onpaste="return false" oncut="return false" oncontextmenu="return false"></asp:TextBox>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" WatermarkCssClass="TextWaterMark"
                                                TargetControlID="TextBoxRegisterEmail" runat="server">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left" style="font-family: Arial, Helvetica, sans-serif; font-size: 10px">
                                            <asp:Label ID="LabelEgEmail1" runat="server" Text="(e.g. your.email@domain.com)"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left" class="PlainTextBold">
                                            <asp:Label ID="LabelChooseAPassword" runat="server" Text="and choose a password:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxRegisterPass" runat="server" CssClass="LoginTextBox" TextMode="Password"
                                                oncopy="return false" onpaste="return false" oncut="return false" oncontextmenu="return false"></asp:TextBox>
                                            <asp:PasswordStrength ID="PasswordStrength1" TargetControlID="TextBoxRegisterPass"
                                                DisplayPosition="AboveRight" StrengthIndicatorType="Text" PreferredPasswordLength="10"
                                                MinimumNumericCharacters="0" TextStrengthDescriptionStyles="PassVeryPoor;PassWeak;PassAverage;PassStrong;PassExcellent;"
                                                MinimumSymbolCharacters="0" RequiresUpperAndLowerCaseCharacters="false" CalculationWeightings="50;15;15;20"
                                                runat="server">
                                            </asp:PasswordStrength>
                                            <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" WatermarkCssClass="TextWaterMark"
                                                TargetControlID="TextBoxRegisterPass" runat="server">
                                            </asp:TextBoxWatermarkExtender>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Image ID="ImageCaptcha" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="LabelCaptcha" runat="server" CssClass="PlainTextBold " Text="Please enter the above number into the box bellow"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="TextBoxConfirmCaptchaText" runat="server" CssClass="LoginTextBox"
                                                Width="100px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="PlainTextBold" colspan="2">
                                            <asp:Label ID="LabelThatsIt" runat="server" Text="... and that's it! You are ready to go ..."></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td align="left">
                                            <asp:Button ID="ButtonRegister" runat="server" CssClass="FancyButton" Text="Register" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td align="center">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorLoginPage2" runat="server"
                                                ControlToValidate="TextBoxRegisterEmail" CssClass="ValidatorSmall" ErrorMessage="Please enter a valid email address"
                                                ValidationExpression="^[\s]*(?:[a-zA-Z0-9_'^&amp;amp;/+-])+(?:\.(?:[a-zA-Z0-9_'^&amp;amp;/+-])+)*@(?:(?:\[?(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\.){3}(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\]?)|(?:[a-zA-Z0-9-]+\.)+(?:[a-zA-Z]){2,}\.?)[\s]*$"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="head">
</asp:Content>