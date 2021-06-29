Imports System.Data.SqlClient

''' <summary>
''' 簡易DBテーブル操作クラス
''' 性能・トランザクションの使いやすさについては、考慮しない。
''' </summary>
Public Class TodoDao

    Private Enum SelectType
        ALL
        COMPLETE
        INCOMPLETE
    End Enum

    ''' <summary>
    ''' DBコネクション取得
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(ConfigurationManager.ConnectionStrings("Connection1").ConnectionString)
    End Function

    ''' <summary>
    ''' 最大ID番号取得
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function GetMaxId() As Integer
        Dim conn As SqlConnection = GetConnection()

        Dim query As String = "select count(*), max(Id) from Todo"

        Dim cmd As New SqlCommand(query, conn)
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        da.SelectCommand = cmd

        Try
            da.Fill(ds)

            If Integer.Parse(ds.Tables(0).Rows(0)(0)) = 0 Then Return 0
            Return Integer.Parse(ds.Tables(0).Rows(0)(1))
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' DataRowから、TodoItemを生成
    ''' </summary>
    ''' <param name="row"></param>
    ''' <returns></returns>
    Private Shared Function CreateTodoItemFrom(row As DataRow) As TodoItem
        Return New TodoItem() With {
                               .ID = Integer.Parse(row("Id")),
                               .Title = row("Title").ToString,
                               .DueDate = If(row("DueDate") Is DBNull.Value, "", DirectCast(row("DueDate"), Date).ToShortDateString),
                               .IsCompleted = If(row("IsCompleted") = 0, False, True)
                           }
    End Function

    Private Shared Function GetTodoList(condition As SelectType) As List(Of TodoItem)
        Dim conn As SqlConnection = GetConnection()

        Dim where As String = ""
        If condition = SelectType.COMPLETE Then
            where = "where IsCompleted = 1"
        ElseIf condition = SelectType.INCOMPLETE Then
            where = "where IsCompleted = 0"
        End If

        Dim query As String = $"select * from Todo {where} order by Id"

        Dim cmd As New SqlCommand(query, conn)
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        da.SelectCommand = cmd

        Try
            da.Fill(ds)
            Dim result As New List(Of TodoItem)
            For Each r As DataRow In ds.Tables(0).Rows
                result.Add(CreateTodoItemFrom(r))
            Next
            Return result
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetAllTodoList() As List(Of TodoItem)
        Return GetTodoList(SelectType.ALL)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetCompleteTodoList() As List(Of TodoItem)
        Return GetTodoList(SelectType.COMPLETE)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetInCompleteTodoList() As List(Of TodoItem)
        Return GetTodoList(SelectType.INCOMPLETE)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Shared Function GetTodoById(id As Integer) As TodoItem
        Dim conn As SqlConnection = GetConnection()

        Dim query As String = "select * from Todo where Id = @Id"

        Dim cmd As New SqlCommand(query, conn)
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id

        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        da.SelectCommand = cmd

        Try
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then Return Nothing
            Return CreateTodoItemFrom(ds.Tables(0).Rows(0))
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="todo"></param>
    Public Shared Function Insert(todo As TodoItem) As TodoItem
        Dim id As Integer = GetMaxId() + 1
        Dim conn As SqlConnection = GetConnection()
        conn.Open()
        Try
            Dim tran As SqlTransaction = conn.BeginTransaction()
            Try
                Dim insQuery As String = "insert into Todo (Id, Title, DueDate, IsCompleted) values (@Id, @Title, @DueDate, @IsCompleted)"
                Dim insCmd As New SqlCommand(insQuery, conn, tran)

                insCmd.Parameters.Add("@Id", SqlDbType.Int).Value = id
                insCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = todo.Title
                insCmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = If(todo.DueDate = "", DBNull.Value, DateTime.Parse(todo.DueDate))
                insCmd.Parameters.Add("@IsCompleted", SqlDbType.TinyInt).Value = If(todo.IsCompleted, 1, 0)
                insCmd.ExecuteNonQuery()
                tran.Commit()
                todo.ID = id
                Return todo
            Catch ex As Exception
                tran.Rollback()
                Debug.WriteLine(ex.ToString)
                Throw
            End Try

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        Finally
            conn.Close()
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="todo"></param>
    Public Shared Sub Update(todo As TodoItem)
        Dim conn As SqlConnection = GetConnection()
        conn.Open()
        Try
            Dim tran As SqlTransaction = conn.BeginTransaction()
            Try
                Dim updQuery As String = "update Todo set Title = @Title, DueDate = @DueDate, IsCompleted = @IsCompleted where Id = @Id"
                Dim updCmd As New SqlCommand(updQuery, conn, tran)

                updCmd.Parameters.Add("@Id", SqlDbType.Int).Value = todo.ID
                updCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = todo.Title
                updCmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = If(todo.DueDate = "", DBNull.Value, DateTime.Parse(todo.DueDate))
                updCmd.Parameters.Add("@IsCompleted", SqlDbType.TinyInt).Value = If(todo.IsCompleted, 1, 0)
                updCmd.ExecuteNonQuery()
                tran.Commit()

            Catch ex As Exception
                tran.Rollback()
                Debug.WriteLine(ex.ToString)
                Throw
            End Try

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        Finally
            conn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="todo"></param>
    Public Shared Sub Delete(todo As TodoItem)
        Dim conn As SqlConnection = GetConnection()
        conn.Open()
        Try
            Dim tran As SqlTransaction = conn.BeginTransaction()
            Try
                Dim delQuery As String = "delete from Todo where Id = @Id"
                Dim delCmd As New SqlCommand(delQuery, conn, tran)

                delCmd.Parameters.Add("@Id", SqlDbType.Int).Value = todo.ID
                delCmd.ExecuteNonQuery()
                tran.Commit()

            Catch ex As Exception
                tran.Rollback()
                Debug.WriteLine(ex.ToString)
                Throw
            End Try

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        Finally
            conn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Shared Sub DeleteAll()
        Dim conn As SqlConnection = GetConnection()
        conn.Open()
        Try
            Dim tran As SqlTransaction = conn.BeginTransaction()
            Try
                Dim delQuery As String = "delete from Todo"
                Dim delCmd As New SqlCommand(delQuery, conn, tran)

                delCmd.ExecuteNonQuery()
                tran.Commit()

            Catch ex As Exception
                tran.Rollback()
                Debug.WriteLine(ex.ToString)
                Throw
            End Try

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
            Throw
        Finally
            conn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Public Shared Sub InitDB()
        DeleteAll()
    End Sub
End Class
