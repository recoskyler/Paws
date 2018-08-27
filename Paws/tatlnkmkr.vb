Imports System
Imports System.IO

Public Class tatlnkmkr
    Dim path As String = ""
    Dim loc As String = IO.Path.GetTempPath

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text <> "" Then
            addLink(TextBox1.Text)
        End If
    End Sub

    Private Sub addLink(ByVal links As String)
        Dim strList As New List(Of String)
        Using reader As New StringReader(TextBox1.Text)
            While reader.Peek() <> -1
                strList.Add(reader.ReadLine())
            End While
        End Using

        For Each l As String In strList
            writeLink(l)
        Next

        TextBox1.Text = ""
    End Sub

    Private Sub writeLink(ByVal link As String)
        Try
            If Not File.Exists(loc) Then
                File.Create(loc).Dispose()

                Dim w As StreamWriter = New StreamWriter(loc)
                w.WriteLine("<!DOCTYPE html>")
                w.WriteLine("<html>")
                w.WriteLine("<body>")
                w.Close()
                w.Dispose()
            End If

            Dim writer As StreamWriter = File.AppendText(loc)
            writer.WriteLine("<a href='" & link & "' target='_blank'>" & link & "</a>")
            writer.WriteLine("<hr>")
            writer.Close()
            writer.Dispose()

            TextBox1.Text = ""
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToMakeLink)
        End Try
    End Sub

    Public Sub makeLinks(ByVal p As String)
        loc = IO.Path.GetTempPath & p.Substring(p.LastIndexOf("/") + 1)

        If Not loc.Contains(".html") Then
            loc = loc & ".html"
            p = p & ".html"
        End If

        path = p

        Label2.Text = "Path: " & p

        TextBox1.Text = ""

        Try
            If File.Exists(loc) Then
                File.Delete(loc)
            End If

            If File.Exists(IO.Path.GetTempPath & "prev.html") Then
                File.Delete(IO.Path.GetTempPath & "prev.html")
            End If

            Debug.Print(path)
            Debug.Print(FTP.FileExists(path))

            If FTP.FileExists(path) Then
                Debug.Print("PREV")

                FTP.DownloadFile(path, IO.Path.GetTempPath & "prev.html")
                Dim prevtxt As String = ""
                Dim reader As StreamReader = New StreamReader(IO.Path.GetTempPath & "prev.html")
                Try
                    prevtxt = reader.ReadToEnd().Replace("<html>", "").Replace("</html>", "").Replace("</body>", "").Replace("<body>", "")
                Catch
                    Debug.Print("FILE EMPTY")
                Finally
                    reader.Close()
                    reader.Dispose()
                End Try

                Try
                    File.Delete(IO.Path.GetTempPath & "prev.html")
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                File.Create(loc).Dispose()

                Dim w As StreamWriter = New StreamWriter(loc)
                w.WriteLine("<!DOCTYPE html>")
                w.WriteLine("<html>")
                w.WriteLine("<body>")
                w.WriteLine(prevtxt)
                w.Close()
                w.Dispose()
            End If
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToMakeLink)
        End Try

        Me.Show()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text <> "" Then
                addLink(TextBox1.Text)
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Dim writer As StreamWriter = File.AppendText(loc)
            writer.WriteLine("</body>")
            writer.WriteLine("</html>")
            writer.Close()
            writer.Dispose()

            If FTP.FileExists(path) Then
                FTP.DeleteFile(path)
            End If

            Debug.Print(loc)
            Debug.Print(path)
            FTP.UploadFile(path.Substring(0, path.LastIndexOf("/") + 1), loc)

            Me.Close()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToMakeLink)
        End Try
    End Sub

    Private Sub tatlnkmkr_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
    End Sub
End Class