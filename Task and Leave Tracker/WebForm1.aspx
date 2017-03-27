<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Task_and_Leave_Tracker.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <button id="btnSubmit" value="submit" runat="server" onserverclick="btnSub_Click">Submit</button>
          <%--  <asp:Button ID="btnSub" Text="Submit 2" runat="server" OnClick="btnSub_Click" />
            <input type="button" value="Submit" id="btnNewSubmit" runat="server" onserverclick="btnSub_Click" />--%>
        </div>
    </form>
</body>
</html>
