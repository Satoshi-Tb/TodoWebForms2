Public Class TodoDetail
    Inherits System.Web.UI.Page

    Private editMode As String

    Private Shared ReadOnly DATE_MATCHER As Regex = New Regex("[0-9]{4}/[01][0-9]/[0-3][0-9]", RegexOptions.Compiled)

    Private Structure EnumEditMode
        Public Const NEW_MODE = "NEW"
        Public Const EDIT_MODE = "EDIT"
    End Structure


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            '初期表示の場合
            If Session("MODE") = "EDIT" Then
                Dim todo As TodoItem = DirectCast(Session("TARGET"), TodoItem)
                txtDueDate.Text = todo.DueDate
                txtTitle.Text = todo.Title
                hdnTodoId.Value = todo.ID
                btnAction.Text = "更新"
                editMode = EnumEditMode.EDIT_MODE
            Else
                btnAction.Text = "追加"
                editMode = EnumEditMode.NEW_MODE
            End If
            hdnEditMode.Value = editMode
            Session.Remove("MODE")
            Session.Remove("TARGET")
        Else
            'PostBackの場合（＝自画面再表示の場合）
            editMode = hdnEditMode.Value
        End If

    End Sub

    Protected Sub btnBack_Click(sender As Object, e As EventArgs)
        Response.Redirect("TodoForm.aspx")
    End Sub

    Protected Sub btnAction_Click(sender As Object, e As EventArgs)

        If Not InputChcek() Then Return

        Dim todo As TodoItem = New TodoItem With {.Title = txtTitle.Text, .DueDate = txtDueDate.Text}

        If editMode = EnumEditMode.NEW_MODE Then
            DBManager.InsertTodo(todo)
        Else
            todo.ID = Integer.Parse(hdnTodoId.Value)
            DBManager.UpdateTodo(todo)
        End If
        Response.Redirect("TodoForm.aspx")
    End Sub

    Private Function InputChcek() As Boolean
        If txtTitle.Text = "" Then
            'TODO メッセージポップアップ見直し（alertに変更する）
            MsgBox("タイトルを入力して下さい")
            Return False
        End If

        If txtDueDate.Text <> "" Then
            Dim dummyDate As Date
            If Not DATE_MATCHER.IsMatch(txtDueDate.Text) OrElse
                Not Date.TryParse(txtDueDate.Text, dummyDate) Then
                MsgBox("期限はYYYY/MM/DDの形式で、正しい日付を入力して下さい")
                Return False
            End If
        End If
        Return True
    End Function
End Class