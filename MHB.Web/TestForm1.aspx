<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestForm1.aspx.vb" Inherits="MHB.Web.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Src="/CustomControls/CustomPage.ascx" TagName="Note" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="5" cellspacing="5" width="100%">
        <tr class="LoginPageIntroTexts">
            <td align="center">
                <img alt="Welcome to MyHomeBills" class="style1" src="Images/logo_main_new_design.gif" />
            </td>
            <td align="center">
                &nbsp;
            </td>
        </tr>
        <tr class="LoginPageIntroTexts">
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <div>
        <asp:Panel ID="PanelArticles" runat="server">
            <table width="100%">
                <tr>
                    <td>
                        <table class="ArticleTableRight">
                            <tr>
                                <td class="LinksBar" colspan="2">
                                    &nbsp;Лични финанси&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img alt="" src="ArticleImages/money1.jpg" style="width: 100px; height: 66px" />
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    ТИ МОЖЕШ ДА УПРАВЛЯВАШ ПАРИТЕ СИ
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderTextBoldLarge">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    15.07.09 г.
                                    <br />
                                    автор: Росен Иванов
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderTextBoldLarge">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainTextBold" valign="top" colspan="2">
                                    Парите не са сложни. Понякога изглежда, че е така, но всъщност не е. Както всяка
                                    сфера на човешкото познание, така и финансовата, се основава на няколко най-важни
                                    закона и базови допускания. Те не са необикновено сложни и трудни. За съжаление
                                    в България поради това, че икономиката и финансите като предмет почти не се изучават
                                    в задължителното средно образование, е налице почти масово невежество относно тези
                                    основни принципи. Нещо повече, още по-малко е знанието в областта на управлението
                                    на личните финанси.
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <a href="http://semeistvo.bg/index.php?option=com_content&task=view&id=279&Itemid=29">
                                        Продължава...</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="PlainText">
                                    източник: <a href="http://www.semeistvo.bg">Semeistvo.bg</a>
                                </td>
                            </tr>
                        </table>
                        <table class="ArticleTableLeft">
                            <tr>
                                <td class="LinksBar" colspan="2">
                                    &nbsp;Лични финанси&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img alt="" src="ArticleImages/money.jpg" style="width: 100px; height: 66px" />
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    Изработване на семеен бюджет
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="HeaderTextBoldLarge">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    14.11.2006 г.
                                    <br />
                                    автор: Стоян Георгиев
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainTextBold" valign="top" colspan="2">
                                    &quot;Ти не можеш да постигнеш просперитет, ако не спестяваш, не можеш да установиш
                                    солидна сигурност, ако взимаш пари назаем; не можеш да стоиш далеч от нещастието,
                                    ако харчиш повече, отколкото печелиш.&quot; Ейбрахам Линкълн
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <a href="http://semeistvo.bg/index.php?option=com_content&task=view&id=78&Itemid=32">
                                        Продължава...</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="PlainText">
                                    източник: <a href="http://www.semeistvo.bg">Semeistvo.bg</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table class="ArticleTableRight">
                            <tr>
                                <td class="LinksBar" colspan="2">
                                    Часът на парите&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img alt="На &quot;ти&quot; с парите" src="ArticleImages/natisparite.gif" style="width: 142px;
                                        height: 38px" />
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    9 начина да опростим финансите си
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainText">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    автор: Деница Стоева
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainText">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainTextBold" valign="top" colspan="2">
                                    Колкото повече неща се трупат за отреагиране, толкова по-голям е шанса да се правят
                                    грешки, да се създават излишни проблеми и съответно да ни спохождат излишни такси,
                                    глоби и наказателни лихви. Добрата организация спестява време и пари, които може
                                    да използвате за други неща. Опростените финанси ни позволяват по-добър контрол
                                    над паричните ни потоци. И често означават повече пари на разположение и по-малко
                                    пропуснати ползи. Закрийте неизползваните сметки. Излишните сметки, карти и прочее
                                    само разсейват ненужно, някои струват и пари. Колкото повече сметки – толкова по-трудно
                                    се следи общата картина. Толкова по-трудно става да се грижим добре за парите ни.<br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <a href="http://natisparite.com/?p=1236">Продължава...</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="PlainText">
                                    източник: <a href="http://natisparite.com/">natisparite.com</a>
                                </td>
                            </tr>
                        </table>
                        <table sclass="ArticleTableLeft" class="ArticleTableLeft">
                            <tr>
                                <td class="LinksBar" colspan="2">
                                    Часът на парите&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img alt="На &quot;ти&quot; с парите" src="ArticleImages/natisparite.gif" style="width: 142px;
                                        height: 38px" />
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    Изработване на семеен бюджет
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainText">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    автор: Деница Стоева
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainText">
                                    &nbsp;
                                </td>
                                <td class="HeaderTextBoldLarge" valign="top">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="PlainTextBold" valign="top" colspan="2">
                                    Часът на парите е поредица от статии с идеи за малки схватки на финансовия фронт,
                                    които могат да се осъществят бързо и без много предварителна подготовка. Кратки
                                    проекти, които могат да положат началото на добри практики и да ни помогнат да направим
                                    още една крачка към желания от нас начин на живот. Всяка статия ще предлага нещо,
                                    което може да бъде организирано в рамките на час и ако бъде превърнато в навик,
                                    ще доведе до положителни промени и улеснения в ежедневието ни. Някои от тях, признавам,
                                    ще са наистина много базови и очевидни, но въпреки това ще бъдат част от поредицата,
                                    защото познавам хора, които въпреки простотата на упражнението и очевидните ползи
                                    от него – все пак не го правят. А за други не вярваме, че не могат да имат голям
                                    ефект, ако правим сметките грубо и на ум, но пък ни шокират с резултатите си, ако
                                    седнем да ги повторим с точни данни и черно на бяло. Приятно четене и успешно действане:)
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <a href="http://natisparite.com/?page_id=505">Продължава...</a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="PlainText">
                                    източник: <a href="http://natisparite.com/">natisparite.com</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <uc1:Note ID="Note1" runat="server" />
    </div>
    </form>
</body>
</html>