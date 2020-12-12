<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Web3.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Start Location: <asp:TextBox ID="StartPoint" runat="server" />   
            <br />
            End Location: <asp:TextBox ID="EndPoint" runat="server" />
            <br />
            <asp:Button ID="Submit" runat="server" Text="OK" OnClick="Submit_Click1" style="width: 43px"/>
            <br />
            <asp:Label ID="NameLabel" runat="server" Text=""/>
        </div>
    </form>
</body>
</html>
