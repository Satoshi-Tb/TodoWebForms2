Imports System.Diagnostics

Public Class TodoForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'リピーターの初期化は、初期表示の1回だけにしないとNG
            UpdateList()
        End If

        'DbSample.DeleteAndInsertData()
        'DbSample.SelectDataUsingConnectedType()
        'DbSample.SelectDataUsingDisconnectedType()
        TodoDao.DeleteAll()
        Dim item1 As TodoItem = TodoDao.Insert(New TodoItem() With {.Title = "サンプル１", .DueDate = "2021/9/21", .IsCompleted = 1})
        Dim item2 As TodoItem = TodoDao.Insert(New TodoItem() With {.Title = "サンプル２", .IsCompleted = 1})
        Dim item3 As TodoItem = TodoDao.Insert(New TodoItem() With {.Title = "サンプル３", .DueDate = "2021/9/21", .IsCompleted = 0})

        TodoDao.GetAllTodoList().ForEach(Sub(elm) Debug.WriteLine(elm))

        item1.Title = "サンプル変更１"
        item2.Title = "サンプル変更２"
        item3.Title = "サンプル変更３"
        TodoDao.Update(item1)
        TodoDao.Update(item2)
        TodoDao.Update(item3)
        TodoDao.GetCompleteTodoList().ForEach(Sub(elm) Debug.WriteLine(elm))
        TodoDao.GetInCompleteTodoList().ForEach(Sub(elm) Debug.WriteLine(elm))

        TodoDao.Delete(item1)
        TodoDao.Delete(item2)
        TodoDao.Delete(item3)

    End Sub

    Protected Sub btnAddTodo_Click(sender As Object, e As EventArgs) Handles btnAddTodo.Click
        Session("MODE") = "NEW"
        Server.Transfer("TodoDetail.aspx")
    End Sub

    Protected Sub btnComplete_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        todo.IsCompleted = True
        DBManager.Update(todo)
        UpdateList()
    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        DBManager.Delete(todo)
        UpdateList()
    End Sub


    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim todo As TodoItem = DBManager.GetTodoById(Integer.Parse(btn.CommandArgument))
        todo.IsCompleted = False
        DBManager.Update(todo)
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