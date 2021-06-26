Imports System.Diagnostics

Public Class TodoForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'リピーターの初期化は、初期表示の1回だけにしないとNG
            UpdateList()
        End If

    End Sub

    Protected Sub btnAddTodo_Click(sender As Object, e As EventArgs) Handles btnAddTodo.Click
        Session("MODE") = "NEW"
        Server.Transfer("TodoDetail.aspx")
    End Sub

    Protected Sub btnComplete_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        todo.IsCompleted = True
        DBManager.UpdateTodo(todo)
        UpdateList()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        DBManager.DeleteTodo(todo)
        UpdateList()
    End Sub


    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        todo.IsCompleted = False
        DBManager.UpdateTodo(todo)
        UpdateList()
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        'HttpContext
        Session("MODE") = "EDIT"
        Session("TARGET") = todo
        Server.Transfer("TodoDetail.aspx")
    End Sub

    Private Sub UpdateList()
        rptCompleteList.DataSource = DBManager.GetInCompleteTodoList
        rptCompleteList.DataBind()
        rptInCompleteList.DataSource = DBManager.GetCompleteTodoList
        rptInCompleteList.DataBind()
    End Sub


End Class