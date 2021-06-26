<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TodoDetail.aspx.vb" Inherits="TodoWebForms2.TodoDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <link rel="stylesheet" href="style.css" />
</head>
<body>
  <form id="form1" runat="server">
    <table class="edit-area">
      <tr>
        <td>
          <asp:Label runat="server" ID="lblTodoTitle" Text="TODOタイトル"/>
        </td>
        <td>
          <asp:TextBox runat="server" ID="txtTitle"  CssClass="input-text" Width="250px"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label runat="server" ID="lblDeDate" Text="期限" />
        </td>
        <td>
          <asp:TextBox runat="server" ID="txtDueDate" CssClass="input-text" Width="75px" placeholder="2021/03/05"></asp:TextBox>
        </td>
      </tr>
      <tr>
        <td colspan="2">
          <asp:Button runat="server" ID="btnAction" Text=""  CssClass="button" OnClick="btnAction_Click"/>
          <input type="reset" value="リセット"  class="button"/>
          <asp:Button runat="server" ID="btnBack" Text="戻る"  CssClass="button" OnClick="btnBack_Click"/>
        </td>
      </tr>
    </table>
    <asp:HiddenField runat="server" ID="hdnTodoId" />
    <asp:HiddenField runat="server" ID="hdnEditMode" />
  </form>
</body>
</html>
