<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateTimeBox.ascx.cs" Inherits="WSIC2010.DateTimeBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:TextBox ID="TextBoxDateTime" runat="server"></asp:TextBox>
<asp:Image ID="ImageCalendar"  runat="server" ImageUrl="~/webform/images/Calendar_scheduleHS.png" />
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>

<asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="TextBoxDateTime" PopupButtonID="ImageCalendar">
</asp:CalendarExtender>
