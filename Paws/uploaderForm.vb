Imports System.IO
Imports System.Threading

Public Class uploaderForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click 'File
        Hide()

        Try
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                MainForm.FailedLabel.Text = 0
                MainForm.SuccessfulLabel.Text = 0

                For Each fl As String In OpenFileDialog1.FileNames
                    Application.DoEvents()

                    If FTP.FileExists(MainForm.cpath & fl) Then
                        If TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskReplace, "File: " & fl) = DialogResult.Yes Then
                            FTP.DeleteFile(MainForm.cpath & fl)
                        Else
                            Continue For
                        End If
                    End If

                    Application.DoEvents()

                    If Not FTP.UploadFile(MainForm.cpath & Path.GetFileName(fl), fl) Then
                        MainForm.FailedLabel.Text = Convert.ToInt32(MainForm.FailedLabel.Text) + 1
                    Else
                        MainForm.SuccessfulLabel.Text = Convert.ToInt32(MainForm.SuccessfulLabel.Text) + 1
                    End If

                    Application.DoEvents()
                Next
            End If
        Catch ex As Exception
            'debug.print(ex.ToString)
        End Try

        MainForm.populateListView(MainForm.cpath)
    End Sub

    Public Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click 'Folder
        Hide()

        Try
            If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
                Dim fpath As String = FolderBrowserDialog1.SelectedPath
                Dim mainFolderName As String = fpath.Substring(fpath.LastIndexOfAny("\") + 1)
                Dim url As String = MainForm.cpath & mainFolderName & "/"

                uploadFolder(mainFolderName, fpath, url)
            End If
        Catch ex As Exception
            'debug.print(ex.ToString)
        End Try

        MainForm.populateListView(MainForm.cpath)
    End Sub

    Public Sub uploadFolder(ByVal mainFolderName As String, ByVal fpath As String, ByVal url As String)
        MainForm.FailedLabel.Text = 0
        MainForm.SuccessfulLabel.Text = 0

        Try
            If FTP.ListDirDetails(MainForm.cpath).Contains(mainFolderName) Then
                MainForm.deleteDir(url)
            End If

            If Not FTP.DirectoryExists(url) Then
                If Not FTP.MakeDir(url) Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantUpload, "\n Error: Failed to create parent directory.")
                    Exit Sub
                End If
            End If

            For Each folder As String In Directory.EnumerateDirectories(fpath, "*", SearchOption.AllDirectories)
                Application.DoEvents()

                Dim folderURL As String = url & folder.Substring(fpath.LastIndexOfAny("\") + mainFolderName.Length + 2)
                folderURL = folderURL.Replace("\", "/")

                If Not FTP.DirectoryExists(folderURL) Then
                    If Not FTP.MakeDir(folderURL) Then
                        MainForm.FailedLabel.Text = Convert.ToInt32(MainForm.FailedLabel.Text) + 1
                    Else
                        MainForm.SuccessfulLabel.Text = Convert.ToInt32(MainForm.SuccessfulLabel.Text) + 1
                    End If
                End If

            Next

            For Each fileTP As String In Directory.EnumerateFiles(fpath, "*.*", SearchOption.AllDirectories)
                Application.DoEvents()

                Dim fileURL As String = url & fileTP.Substring(fpath.Length + 1)
                fileURL = fileURL.Replace("\", "/")

                If Not FTP.FileExists(fileURL) Then
                    If Not FTP.UploadFile(fileURL, fileTP) Then
                        MainForm.FailedLabel.Text = Convert.ToInt32(MainForm.FailedLabel.Text) + 1
                    Else
                        MainForm.SuccessfulLabel.Text = Convert.ToInt32(MainForm.SuccessfulLabel.Text) + 1
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantUpload)
        End Try
    End Sub

    Public Shared Function sortArr(ByVal strarr As String()) As String()
        Dim sorted = strarr.OrderBy(Function(x) x.Length).ThenBy(Function(x) x).ToArray()

        Return sorted
    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Close()
    End Sub

    Private Sub uploaderForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class