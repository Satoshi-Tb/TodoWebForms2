Imports System.Data.SqlClient
Imports System.Diagnostics
Public Class DbSample

    Private Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(ConfigurationManager.ConnectionStrings("Connection1").ConnectionString)
    End Function


    Public Shared Sub DeleteAndInsertData()
        Debug.WriteLine("【DBアクセス・削除＆追加テスト】")
        Dim conn As SqlConnection = GetConnection()


        conn.Open()
        Try
            Dim tran As SqlTransaction = conn.BeginTransaction()
            Try
                Dim insQuery As String = "insert into Todo (Id, Title, DueDate, IsCompleted) values (@Id, @Title, @DueDate, @IsCompleted)"
                Dim insCmd As New SqlCommand(insQuery, conn, tran)
                Dim delQuery As String = "delete from Todo"
                Dim delCmd As New SqlCommand(delQuery, conn, tran)

                delCmd.ExecuteNonQuery()

                insCmd.Parameters.Add("@Id", SqlDbType.Int).Value = 1
                insCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = "レポート作成"
                insCmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = DateTime.Now.AddMonths(1)
                insCmd.Parameters.Add("@IsCompleted", SqlDbType.TinyInt).Value = 0
                insCmd.ExecuteNonQuery()

                insCmd.Parameters.Clear()
                insCmd.Parameters.Add("@Id", SqlDbType.Int).Value = 2
                insCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = "セミナーの申込"
                insCmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = DateTime.Now.AddMonths(2)
                insCmd.Parameters.Add("@IsCompleted", SqlDbType.TinyInt).Value = 0
                insCmd.ExecuteNonQuery()

                insCmd.Parameters.Clear()
                insCmd.Parameters.Add("@Id", SqlDbType.Int).Value = 3
                insCmd.Parameters.Add("@Title", SqlDbType.NVarChar).Value = "新しい鞄の購入"
                insCmd.Parameters.Add("@DueDate", SqlDbType.Date).Value = DBNull.Value
                insCmd.Parameters.Add("@IsCompleted", SqlDbType.TinyInt).Value = 1
                insCmd.ExecuteNonQuery()

                tran.Commit()

            Catch ex As Exception
                tran.Rollback()
                Debug.WriteLine(ex.ToString)
            End Try

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
        Finally
            conn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 接続型DB接続テスト
    ''' </summary>
    Public Shared Sub SelectDataUsingConnectedType()
        Debug.WriteLine("【DBアクセス・接続型テスト】")
        Dim conn As SqlConnection = GetConnection()

        conn.Open()
        Try
            Dim cmd As New SqlCommand("select * from Todo", conn)
            Dim r As SqlDataReader = cmd.ExecuteReader()

            While r.Read
                Debug.WriteLine($"Id = {r("Id")}, Title = {r("Title")}, DueDate = {r("DueDate")}, IsComplete = {r("IsCompleted")}")
            End While

        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
        Finally
            conn.Close()
        End Try

    End Sub

    ''' <summary>
    ''' 非接続型DB接続テスト
    ''' </summary>
    Public Shared Sub SelectDataUsingDisconnectedType()
        Debug.WriteLine("【DBアクセス・非接続型テスト】")
        Dim conn As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand("select * from Todo", conn)
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter

        da.SelectCommand = cmd

        Try
            da.Fill(ds) 'Connectionは自動でOpen&Close

            For Each r As DataRow In ds.Tables(0).Rows
                Debug.WriteLine($"Id = {r("Id")}, Title = {r("Title")}, DueDate = {r("DueDate")}, IsComplete = {r("IsCompleted")}")
            Next
        Catch ex As Exception
            Debug.WriteLine(ex.ToString)
        End Try


    End Sub
End Class
