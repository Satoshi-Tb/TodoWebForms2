<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TodoForm.aspx.vb" Inherits="TodoWebForms2.TodoForm" %>

<!DOCTYPE html>
<html>
<head>
  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
  <title>ToDoList</title>
  <link rel="stylesheet" href="style.css">
  <script type="text/javascript">
  </script>
</head>
<body>
  <form id="form1" runat="server">
    <div class="new-button-area">
      <asp:Button runat="server" ID="btnAddTodo" Text="TODOを追加する"  CssClass="button"/>
      </div>
    <div class="incomplete-area">
      <p class="title">未完了のTODO</p>
      <!-- リピータ配置 -->
      <ul id="incomplete-list">
        <asp:Repeater runat="server" ID="rptCompleteList">
          <ItemTemplate>
            <li>
              <div class="list-row">
                <p><%# DataBinder.Eval(Container.DataItem, "Title")  %></p>
                <asp:Button runat="server" ID="btnComplete" Text="完了" CssClass="button" OnClick="btnComplete_Click" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "ID")  %>'/>
                <asp:Button runat="server" ID="btnUpdate" Text="修正" CssClass="button" OnClick="btnEdit_Click" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "ID")  %>'/>
                <asp:Button runat="server" ID="btnDelete" Text="削除" CssClass="button" OnClick="btnDelete_Click" CommandArgument ='<%# DataBinder.Eval(Container.DataItem, "ID")  %>'/>
                </div>
            </li>
          </ItemTemplate>
        </asp:Repeater>
      </ul>
    </div>
    <div class="complete-area">
      <p class="title">完了したTODO</p>
      <ul id="complete-list">
        <!-- リピータ配置 -->
        <asp:Repeater runat="server" ID="rptInCompleteList" >
          <ItemTemplate>
            <li>
              <div class="list-row">
                <p><%# DataBinder.Eval(Container.DataItem, "Title")  %></p>
                <asp:Button runat="server" ID="btnBack" Text="戻す" CssClass="button" OnClick="btnBack_Click" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ID")  %>'/>
                </div>
            </li>
          </ItemTemplate>
        </asp:Repeater>
      </ul>
    </div>
  </form>
</body>
</html>