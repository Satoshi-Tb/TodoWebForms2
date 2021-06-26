Public Class DBManager

    ' 簡易DB。排他は考慮しない
    Private Const TODO_TABLE_KEY As String = "TodoWebForms_TodoTable"
    Private Shared todoTable As Dictionary(Of Integer, TodoItem)
    Private Shared seqNo As Integer = 0


    Public Shared Sub InitDB()
        todoTable = New Dictionary(Of Integer, TodoItem)

    End Sub

    Public Shared Function GetAllTodo() As List(Of TodoItem)
        Return todoTable.AsEnumerable().Select(Of TodoItem)(Function(x) x.Value).ToList
    End Function

    Public Shared Function GetCompleteTodoList() As List(Of TodoItem)
        Return todoTable.AsEnumerable().
            Where(Function(x) x.Value.IsCompleted).
            Select(Of TodoItem)(Function(x) x.Value).
            ToList
    End Function

    Public Shared Function GetInCompleteTodoList() As List(Of TodoItem)
        Return todoTable.AsEnumerable().
            Where(Function(x) Not x.Value.IsCompleted).
            Select(Of TodoItem)(Function(x) x.Value).
            ToList
    End Function

    Public Shared Function GetTodoById(id As Integer) As TodoItem
        Return todoTable(id)
    End Function

    Public Shared Sub InsertTodo(todo As TodoItem)
        '重複チェック割愛
        seqNo += 1
        todo.ID = seqNo
        todoTable(todo.ID) = todo
    End Sub

    Public Shared Sub UpdateTodo(todo As TodoItem)
        'データなしチェック割愛
        todoTable(todo.ID) = todo
    End Sub

    Public Shared Sub DeleteTodo(todo As TodoItem)
        todoTable.Remove(todo.ID)
    End Sub

End Class
