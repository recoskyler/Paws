Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.ComponentModel

Public Class FTP

#Region " Definitions "
    Shared ftpReq As FtpWebRequest = Nothing
    Public Shared connected As Boolean = False
    Shared credentials As NetworkCredential
    Shared purl As String = ""
    Shared allDirOutput As New List(Of PawFile)
    Shared allSDirOutput As New List(Of String)
    Shared Property errors As Boolean = True
    Public WithEvents client As WebClient = New WebClient()
    Public fileSize As Long = 0
#End Region

#Region " Connection "
    Shared Function Connect(ByVal url As String, ByVal usrname As String, ByVal psword As String) As Boolean
        If connected Then
            Return True
        End If

        If Not CheckForInternetConnection() Then
            If errors Then
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.NoInternet)
            End If
        End If

        credentials = New NetworkCredential(usrname, psword)
        purl = url

        'Test FTP Connection '

        If errors Then
            CheckConnection(url)
        End If

        Return connected
    End Function

    Shared Function CheckConnection(ByVal url As String) As Boolean
        Try
            Dim request = DirectCast(WebRequest.Create(url), FtpWebRequest)
            request.Credentials = credentials
            request.Method = WebRequestMethods.Ftp.ListDirectory
            request.KeepAlive = False
            request.Timeout = 20000

            Try
                Using response As FtpWebResponse = DirectCast(request.GetResponse(), FtpWebResponse)
                    connected = True
                End Using
            Catch ex As WebException
                Dim response As FtpWebResponse = DirectCast(ex.Response, FtpWebResponse)

                If errors Then
                    If response.StatusCode = FtpStatusCode.NotLoggedIn Then
                        TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.InvalidCredentials)
                    Else
                        TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantConnect)
                    End If
                End If

                connected = False
            End Try
        Catch ex As Exception
            Debug.Print(ex.ToString)
            If errors Then
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantConnect)
            End If
            connected = False
        End Try

        Return connected
    End Function

    Public Shared Function CheckForInternetConnection() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function

    Shared Function Disconnect() As Boolean
        If connected Then
            Try
                ftpReq = Nothing
                credentials = Nothing
                purl = ""
            Catch ex As Exception
                Debug.Print(ex.ToString)
                Return False
            End Try
        Else
            Return True
        End If

        connected = False

        Return True
    End Function
#End Region

#Region " General "
    ''' <summary>
    ''' Lists all file and directory details in a directory on FTP Server
    ''' </summary>
    ''' <param name="url">URL Of Directory To List</param>
    Shared Function ListDirDetails(ByVal url As String) As List(Of String)
        Dim output As New List(Of String)

        If Not connected Then
            Return output
        End If

        Try
            Dim c As Integer = 0

            ftpReq = DirectCast(WebRequest.Create(url), FtpWebRequest)
            ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 180000
            ftpReq.Credentials = credentials

            Dim response As FtpWebResponse = DirectCast(ftpReq.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim Lf As Char = Convert.ToChar(CByte(10))
            Dim Cr As Char = Convert.ToChar(CByte(13))
            Dim CrLf As Char() = {Cr, Lf}

            For Each fl As String In reader.ReadToEnd().Split(CrLf, StringSplitOptions.RemoveEmptyEntries)
                output.Add(Regex.Replace(fl, " {2,}", " "))
            Next

            response.Close()
            reader.Close()

            Return output
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FolderAccessDenied)
            Return output
        End Try
    End Function

    Shared Function ListDir(ByVal url As String, Optional ByVal formatted As Boolean = True) As List(Of PawFile)
        Dim output As New List(Of PawFile)

        If Not connected Then
            Return output
        End If

        If url.Last() <> "/" Then
            url = url & "/"
        End If

        Try
            Dim c As Integer = 0

            ftpReq = DirectCast(WebRequest.Create(url), FtpWebRequest)
            ftpReq.Method = WebRequestMethods.Ftp.ListDirectoryDetails
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 180000
            ftpReq.Credentials = credentials

            Dim response As FtpWebResponse = DirectCast(ftpReq.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim Lf As Char = Convert.ToChar(CByte(10))
            Dim Cr As Char = Convert.ToChar(CByte(13))
            Dim CrLf As Char() = {Cr, Lf}

            For Each fl As String In reader.ReadToEnd().Split(CrLf, StringSplitOptions.RemoveEmptyEntries)
                fl = Regex.Replace(fl, " {2,}", " ")

                Dim fin As String = fl.First()
                Dim flt As String = fl
                fl = fl.Split(" ")(8)

                If flt.Split(" ").Count > 9 Then
                    For i As Integer = 9 To flt.Split(" ").Count - 1
                        fl = fl & " " & flt.Split(" ")(i)
                    Next
                End If

                Dim t As PawFile.Types = PawFile.Types.Folder
                Dim lastmod As Date = MainForm.getLastModifiedDate(flt)

                If fin = "-" Then
                    t = PawFile.Types.File
                End If

                Dim ff As New PawFile With {.path = "", .type = t, .ftpPath = url & fl, .dt = lastmod, .size = Convert.ToUInt64(flt.Split(" ")(4)), .name = fl}
                output.Add(ff)
            Next

            response.Close()
            response.Dispose()
            reader.Dispose()
            reader.Close()

            Return output
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FolderAccessDenied)
            Return output
        End Try
    End Function

    Shared Function ListAllDirs(ByVal url As String, Optional ByVal child As Boolean = False) As List(Of PawFile)
        If Not child Then
            allDirOutput.Clear()
        End If

        If url.Last() <> "/" Then
            url = url & "/"
        End If

        'Debug.Print("URL: " & url)

        Dim dirs As List(Of PawFile) = ListDir(url, True)

        allDirOutput.AddRange(dirs)

        For Each dir As PawFile In dirs
            If dir.type = PawFile.Types.Folder Then
                ListAllDirs(dir.ftpPath, True)
            End If
        Next

        Return allDirOutput
    End Function

    Shared Function ListAllDirDetails(ByVal url As String, Optional ByVal child As Boolean = False) As List(Of String)
        If Not child Then
            allSDirOutput.Clear()
        End If

        Dim dirs As List(Of String) = ListDirDetails(url)
        allSDirOutput.AddRange(dirs)

        For Each dir As String In dirs
            If dir.StartsWith("d") Then
                ListAllDirDetails(url & dir.Substring(1) & "/", True)
            End If
        Next

        Return allSDirOutput
    End Function


#End Region

#Region " File IO"

    ''' <summary>
    ''' Gets the size of a file in Bytes
    ''' </summary>
    ''' <param name="url">URL of file</param>
    ''' <returns>File size in Bytes</returns>
    Shared Function GetSize(ByVal url As String) As Long
        Dim fileSize As Long = 0

        Try
            For Each file As PawFile In ListDir(url.Substring(0, url.LastIndexOf("/") + 1))
                If file.name = url.Substring(url.LastIndexOf("/") + 1) Then
                    Return file.size
                End If
            Next

            Return 0
        Catch ex As Exception
            Debug.Print(ex.ToString)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Moves a file or folder on Server
    ''' </summary>
    ''' <param name="url">Current URL of file/folder</param>
    ''' <param name="newurl">New URL of file/folder</param>
    ''' <returns>True if successful</returns>
    Shared Function Move(ByVal url As String, ByVal newurl As String) As Boolean
        Try
            'Debug.Print(url)
            'Debug.Print(url.Substring(0, url.LastIndexOf("/")) & newurl.Replace(url.Substring(0, url.LastIndexOf("/")), ""))

            If RenameFile(url, newurl) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantMove)
            Debug.Print(ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Checks if a file exists on Server
    ''' </summary>
    ''' <param name="url">File URL to check</param>
    ''' <returns>True if file exists</returns>
    Shared Function FileExists(ByVal url As String) As Boolean
        Debug.Print("URL: " & url)

        If Not connected Then
            Return False
        End If

        Try
            For Each f As PawFile In FTP.ListDir(url.Substring(0, url.LastIndexOf("/") + 1))
                If f.ftpPath = url And f.type = PawFile.Types.File Then
                    Return True
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FileAccessDenied)
        End Try

        Return False
    End Function

    ''' <summary>
    ''' Downloads a file from FTP Server
    ''' </summary>
    ''' <param name="url">File URL</param>
    ''' <param name="downloadPath">File Download Path</param>
    ''' <returns>True If Download Is Successful</returns>
    Shared Function DownloadFile(ByVal url As String, ByVal downloadPath As String) As Boolean
        If Not connected Then
            Return False
        End If

        MainForm.ProgressBar1.Minimum = 0
        MainForm.ProgressBar1.Maximum = 100
        MainForm.ProgressBar1.Value = 0

        If File.Exists(downloadPath) Then
            Try
                File.Delete(downloadPath)
            Catch ex As Exception
                Debug.Print(ex.ToString)
                'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDelete)
                Return False
            End Try
        End If

        url = "ftp://" & My.Settings.username & ":" & My.Settings.password & "@" & url.Substring(6)

        Try
            Dim f As New FTP

            f.fileSize = GetSize(url)
            f.client = New WebClient()
            f.client.DownloadFileAsync(New Uri(url), downloadPath)
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDonwload)
            Debug.Print(ex.ToString)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Uploads a file to FTP Server
    ''' </summary>
    ''' <param name="url">FileToUpload URL On FTP Server</param>
    ''' <param name="fileToUpload">Path Of File On The Computer</param>
    ''' <param name="fileNameOnServer">New file name on server</param>
    ''' <returns>True If Upload Is Successful</returns>
    Shared Function UploadFile(ByVal url As String, ByVal fileToUpload As String, Optional fileNameOnServer As String = "") As Boolean
        If Not connected Then
            Return False
        End If

        Try
            Dim infoReader As FileInfo
            infoReader = My.Computer.FileSystem.GetFileInfo(fileToUpload)

            Dim filename As String = Path.GetFileName(fileToUpload)

            If url.Substring(url.LastIndexOf("/") + 1).Equals(filename) Then
                url = url.Substring(0, url.LastIndexOf("/") + 1)
            End If

            If Not fileNameOnServer.Equals("") Then
                filename = fileNameOnServer
            End If

            Dim bytesread As Integer = 0
            Dim buffer As Integer = 2048
            ftpReq = CType(FtpWebRequest.Create(New Uri(url & filename)), FtpWebRequest)
            ftpReq.Credentials = credentials
            ftpReq.KeepAlive = False
            ftpReq.UseBinary = True
            ftpReq.Method = WebRequestMethods.Ftp.UploadFile
            ftpReq.Timeout = 180000
            ftpReq.ContentLength = infoReader.Length

            MainForm.ProgressBar1.Maximum = (infoReader.Length / buffer) + 2
            MainForm.ProgressBar1.Minimum = 0
            MainForm.ProgressBar1.Value = 0

            Dim fs As FileStream = File.OpenRead(fileToUpload)
            Dim bfile(buffer - 1) As Byte
            Dim fstream As Stream = ftpReq.GetRequestStream
            Dim contentLen As Integer = fs.Read(bfile, 0, buffer)

            Do While contentLen <> 0
                fstream.Write(bfile, 0, contentLen)
                contentLen = fs.Read(bfile, 0, buffer)

                MainForm.ProgressBar1.Value = MainForm.ProgressBar1.Value + 1
                Application.DoEvents()
            Loop

            fstream.Close()
            fstream.Dispose()
            fs.Close()
            fs.Dispose()

            MainForm.ProgressBar1.Value = 0
        Catch ex As Exception
            Debug.Print(ex.ToString)
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantUpload)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Deletes a file from FTP Server
    ''' </summary>
    ''' <param name="url">URL Of File To Delete</param>
    ''' <returns>True If File Deleted</returns>
    Shared Function DeleteFile(ByVal url As String) As Boolean
        If Not connected Then
            Return False
        End If

        Try
            ftpReq = DirectCast(WebRequest.Create(url), FtpWebRequest)
            ftpReq.Method = WebRequestMethods.Ftp.DeleteFile
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 100000000
            ftpReq.Credentials = credentials

            Dim response As FtpWebResponse = DirectCast(ftpReq.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Dim Lf As Char = Convert.ToChar(CByte(10))
            Dim Cr As Char = Convert.ToChar(CByte(13))
            Dim CrLf As Char() = {Cr, Lf}

            For Each str As String In reader.ReadToEnd().Split(CrLf, StringSplitOptions.RemoveEmptyEntries)
                'debug.print(str)
            Next

            response.Close()
            reader.Close()

            Return True
        Catch ex As Exception
            Debug.Print(ex.ToString)
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDelete)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Renames a file on FTP Server
    ''' </summary>
    ''' <param name="url">URL Of File</param>
    ''' <param name="newname">New Name Of The File</param>
    ''' <returns></returns>
    Shared Function RenameFile(ByVal url As String, ByVal newname As String)
        If Not connected Then
            Return False
        End If

        Try
            If newname.Substring(0, 6) = "ftp://" Then
                newname = "/" & newname.Replace(My.Settings.path, "")
            End If

            Debug.Print(url)
            Debug.Print(newname)

            ftpReq = CType(System.Net.FtpWebRequest.Create(url), FtpWebRequest)
            ftpReq.Method = WebRequestMethods.Ftp.Rename
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 100000000
            ftpReq.Credentials = credentials
            ftpReq.RenameTo() = newname

            Dim response As FtpWebResponse = DirectCast(ftpReq.GetResponse(), FtpWebResponse)
            Dim responseStream As Stream = response.GetResponseStream()
            Dim reader As New StreamReader(responseStream)
            Debug.Print(reader.ReadToEnd)

            response.Close()
            reader.Close()

            Return True
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantRename)
            Debug.Print(ex.ToString)
            Return False
        End Try
    End Function

    Shared Function GetDateTimeStamp(ByVal url As String) As Date
        Dim dte As Date = DateTime.Now

        'Debug.Print("DTSURL: " & url)

        Try
            Dim ftp As Net.FtpWebRequest = Net.FtpWebRequest.Create(url)
            ftp.Method = Net.WebRequestMethods.Ftp.GetDateTimestamp
            ftp.Credentials = credentials
            ftp.KeepAlive = False
            ftp.Timeout = 20000
            Using response = CType(ftp.GetResponse(), Net.FtpWebResponse)
                dte = response.LastModified
            End Using

            Return dte
        Catch ex As Exception
            Debug.Print(ex.ToString())

            Return dte
        End Try
    End Function

#End Region

#Region " Folder IO "

    ''' <summary>
    ''' Checks if a directory exists on Server
    ''' </summary>
    ''' <param name="url">URL of the directory to check</param>
    ''' <returns>True if directory exists</returns>
    Shared Function DirectoryExists(ByVal url As String) As Boolean
        Dim exists As Boolean = False

        If url.Last = "/" Then
            url = url.Substring(0, url.Length - 1)
        End If

        'Debug.Print("URLEXISTS: " & url)
        'Debug.Print(url.Substring(0, url.LastIndexOf("/")))

        For Each fol As PawFile In ListDir(url.Substring(0, url.LastIndexOf("/")), True)
            If fol.type = PawFile.Types.Folder And (fol.ftpPath = url Or fol.ftpPath = url.Substring(0, url.Length - 1)) Then
                exists = True
            End If
        Next

        Return exists
    End Function

    ''' <summary>
    ''' Downloads a folder on Server
    ''' </summary>
    ''' <param name="url">URL of the folder to download</param>
    ''' <param name="downloadPath">Path of the folder on computer</param>
    ''' <returns>True if successful</returns>
    Shared Function DownloadFolder(ByVal url As String, ByVal downloadPath As String) As Boolean
        Try
            If Not Directory.Exists(downloadPath) Then
                Directory.CreateDirectory(downloadPath)
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
            Return False
        End Try

        Try
            For Each fl As String In FTP.ListDirDetails(url)
                If fl.StartsWith("-") Then                                               'File
                    Dim flt As String = fl
                    fl = fl.Split(" ")(8)

                    If flt.Split(" ").Count > 9 Then
                        For i As Integer = 9 To flt.Split(" ").Count - 1
                            fl = fl & " " & flt.Split(" ")(i)
                        Next
                    End If

                    If Not downloadPath.EndsWith("\") Then
                        downloadPath = downloadPath & "\"
                    End If

                    DownloadFile(url & "/" & fl, downloadPath & fl)
                Else                                                                     'Folder
                    Dim flt As String = fl
                    fl = fl.Split(" ")(8)

                    If flt.Split(" ").Count > 9 Then
                        For i As Integer = 9 To flt.Split(" ").Count - 1
                            fl = fl & " " & flt.Split(" ")(i)
                        Next
                    End If

                    If Not downloadPath.EndsWith("\") Then
                        downloadPath = downloadPath & "\"
                    End If

                    DownloadFolder(url & "/" & fl, downloadPath & fl)
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDonwload)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Creates a directory on FTP Server
    ''' </summary>
    ''' <param name="url">URL Of New Directory</param>
    ''' <returns>True If Directory Created</returns>
    Shared Function MakeDir(ByVal url As String) As Boolean
        If Not connected Then
            Return False
        End If

        If url.Last() = "/" Then
            url = url.Substring(0, url.Length - 1)
        End If

        Try
            ftpReq = CType(WebRequest.Create(url), FtpWebRequest)
            ftpReq.Method = WebRequestMethods.Ftp.MakeDirectory
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 100000000
            ftpReq.Credentials = credentials

            Dim FTPRes As FtpWebResponse

            FTPRes = CType(ftpReq.GetResponse, FtpWebResponse)

            Return True
        Catch ex As Exception
            Debug.Print(ex.ToString)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Deletes a directory from FTP Server
    ''' </summary>
    ''' <param name="url">URL Of Directory To Delete</param>
    ''' <returns>True If Directory Deleted</returns>
    Shared Function DeleteDir(ByVal url As String) As Boolean
        If Not connected Then
            Return False
        End If

        Try
            Debug.Print(url)
            ftpReq = WebRequest.Create(url)
            ftpReq.Method = WebRequestMethods.Ftp.RemoveDirectory
            ftpReq.KeepAlive = False
            ftpReq.Timeout = 100000
            ftpReq.Credentials = credentials

            Dim ftpResp As FtpWebResponse = ftpReq.GetResponse
            ftpResp.Close()

            Return True
        Catch ex As Exception
            'TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToRemoveFolder)
            Return False
        End Try
    End Function


#End Region

    Private Sub client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles client.DownloadProgressChanged
        Try
            Dim percentage As Double = e.BytesReceived / fileSize * 100
            percentage = Convert.ToInt32(Math.Round(percentage - 0.05))

            If Convert.ToInt32(Math.Round(percentage - 0.05)) = 100 Then
                MainForm.ProgressBar1.Value = 0

                If e.BytesReceived >= fileSize Then
                    client.Dispose()
                    MainForm.SuccessfulLabel.Text = Convert.ToInt32(MainForm.SuccessfulLabel.Text) + 1
                End If
            Else
                MainForm.ProgressBar1.Value = percentage
            End If

            Application.DoEvents()
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub client_DownloadFileCompleted(sender As Object, e As AsyncCompletedEventArgs) Handles client.DownloadFileCompleted
        MainForm.ProgressBar1.Value = 0
    End Sub
End Class
