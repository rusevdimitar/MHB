<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master"
    CodeBehind="Events.aspx.vb" Inherits="MHB.Web.Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center">
                <asp:Calendar ID="CalendarEvents" runat="server" Width="100%" Height="600" CssClass="EventsCalendar"
                    BorderStyle="None">
                    <TitleStyle CssClass="EventsCalendarHeader" Height="23" BackColor="#EAF4FF" />
                    <NextPrevStyle ForeColor="White" />
                    <WeekendDayStyle BackColor="#ECFFEC" ForeColor="Green" />
                    <SelectedDayStyle BackColor="#EAF4FF" ForeColor="DarkBlue" BorderColor="Blue" BorderStyle="Solid"
                        BorderWidth="1" />
                    <DayHeaderStyle Font-Size="12" Height="20" BackColor="#EAF4FF" ForeColor="DarkBlue" />
                    <DayStyle CssClass="EventsCalendarDay" VerticalAlign="Top" HorizontalAlign="Left" />
                    <OtherMonthDayStyle BackColor="Gainsboro" ForeColor="Gray" />
                </asp:Calendar>
            </td>
        </tr>
    </table>
</asp:Content>