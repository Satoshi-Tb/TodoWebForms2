Public Class TodoItem

    Public Property ID As Integer
    Public Property Title As String
    Public Property DueDate As String
    Public Property IsCompleted As Boolean

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & $"ID={ID},Title={Title},DueDate={DueDate},IsCompleted={IsCompleted}"
    End Function

End Class
