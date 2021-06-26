
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(sender As Object, e As EventArgs)
        ' アプリケーションの起動時に呼び出されます
        DBManager.InitDB()

        ' サンプルデータ
        'DBManager.InsertTodo(New TodoItem With {.ID = 1, .Title = "課題作成"})
        'DBManager.InsertTodo(New TodoItem With {.ID = 2, .Title = "環境整備"})
        'DBManager.InsertTodo(New TodoItem With {.ID = 3, .Title = "買い物"})
    End Sub
End Class