Public Class FolLock
    Dim dirs As New List(Of String)
    Dim mode As Integer = 0 '0 - Unlock, 1 - Password
    Dim type As Integer = 1 '0 - file, 1 - folder
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        checkPass()
    End Sub

    Private Sub FolLock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = ""
        TextBox1.Focus()
        Label2.Text = ""
    End Sub

    Public Sub unlockFolders(ByVal dir As List(Of String))
        Me.Show()
        MainForm.Hide()
        TextBox1.Text = ""
        Label2.Text = ""
        dirs.Clear()
        dirs = dir
        mode = 0
        TextBox1.Focus()
    End Sub

    Public Sub unlockPass(ByVal dir As String, ByVal t As Integer)
        Me.Show()
        MainForm.Hide()
        TextBox1.Text = ""
        Label2.Text = ""
        dirs.Clear()
        dirs.Add(dir)
        mode = 1
        type = t
        TextBox1.Focus()
    End Sub

    Private Sub TextBox1_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyUp
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            checkPass()
        End If
    End Sub

    Private Sub checkPass()
        If TextBox1.Text.Equals(My.Settings.folpass) Then
            TextBox1.Text = ""
            Label2.Text = ""

            Hide()
            MainForm.Show()
            MainForm.Focus()
            Application.DoEvents()

            If mode = 0 Then
                For Each dir As String In dirs
                    If My.Settings.lockedPaths.Contains(dir) Then
                        My.Settings.lockedPaths.Remove(dir)
                        My.Settings.Save()
                        My.Settings.Reload()
                    End If
                Next

                dirs.Clear()

                If MainForm.cpath.Last <> "/" Then
                    MainForm.cpath = MainForm.cpath & "/"
                End If

                MainForm.populateListView(MainForm.cpath)
            Else
                If type = 0 Then
                    If MainForm.cpath.Last <> "/" Then
                        MainForm.cpath = MainForm.cpath & "/"
                    End If

                    MainForm.openFile(dirs(0))
                Else
                    MainForm.cpath = dirs(0)

                    If MainForm.cpath.Last <> "/" Then
                        MainForm.cpath = MainForm.cpath & "/"
                    End If

                    MainForm.populateListView(MainForm.cpath)
                End If
            End If
        Else
            Label2.Text = "Wrong Password"
            TextBox1.Text = ""
            TextBox1.Focus()
        End If
    End Sub

    Private Sub FolLock_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        MainForm.Show()
    End Sub
End Class