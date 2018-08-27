Imports System.IO
Imports System.Threading
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Imports System.ComponentModel

Public Class MainForm

#Region " OBJECTS - ENUMS "
    Public cpath As String = ""
    Public lastList As New List(Of String)
    Dim openAs As Boolean = False
    Public Shared openedFiles As New List(Of String)
    Public Shared syncstat As String = "Sync Status: Sync Complete"
    Dim drag As Boolean = False
    Dim mousex As Integer
    Dim mousey As Integer
    Dim maximized As Boolean = False
    Private searchmode As Boolean = False
    Private searchres As New List(Of ListViewItem)
    Private searchdone As Boolean = False
    Private cmenuitem As PawFile
    Private prevcon = True
    Private clipboard As New List(Of PawFile)
    Private clipmode As ClipAction = ClipAction.None
    Private dragging As Boolean = False
    Private closeapp As Boolean = False

    Private Enum ClipAction
        Copy = 0
        Cut = 1
        None = 2
    End Enum
#End Region

#Region " Copy-Cut Paste "

    Private Sub CopyCut(ByVal path As String, ByVal items As ListView.SelectedListViewItemCollection, Optional ByVal mode As ClipAction = ClipAction.Copy)
        Try
            If path.Last <> "/" Then
                path = path & "/"
            End If

            If Not items.Count > 0 Then
                Exit Sub
            End If

            clipmode = mode
            clipboard.Clear()

            For Each item As ListViewItem In items
                If item.Text <> "..." And item.SubItems(4).Text = "Unlocked" Then
                    Dim type As PawFile.Types = PawFile.Types.File

                    If item.SubItems(1).Text = "Folder" Then
                        type = PawFile.Types.Folder
                    End If

                    Dim pf As New PawFile With {.ftpPath = path & item.Text, .path = IO.Path.GetTempPath & item.Text, .name = item.Text, .type = type}
                    clipboard.Add(pf)
                End If
            Next
        Catch ex As Exception
            If mode = ClipAction.Copy Then
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantCopy)
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantCut)
            End If

            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub Paste(ByVal newpath As String)
        Try
            If newpath.Last <> "/" Then
                newpath = newpath & "/"
            End If

            If clipmode = ClipAction.None Or Not clipboard.Count > 0 Then
                Exit Sub
            End If

            If clipboard(0).ftpPath.Substring(0, clipboard(0).ftpPath.LastIndexOf("/") + 1) = newpath Then
                Exit Sub
            End If

            For Each item As PawFile In clipboard
                Dim np As String = newpath & item.name

                If clipmode = ClipAction.Cut Then
                    If item.type = PawFile.Types.File Then
                        If FTP.FileExists(np) Then
                            If TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskReplace, "File: " & item.name) = DialogResult.Yes Then
                                FTP.DeleteFile(np)
                            Else
                                Continue For
                            End If
                        End If

                        FTP.Move(item.ftpPath, np)
                    Else
                        If FTP.DirectoryExists(np) Then
                            If TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskReplace, "Folder: " & item.name) = DialogResult.Yes Then
                                deleteDir(np)
                            Else
                                Continue For
                            End If
                        End If

                        FTP.Move(item.ftpPath, np)
                    End If
                Else
                    If item.type = PawFile.Types.File Then
                        Try
                            If File.Exists(item.path) Then
                                File.Delete(item.path)
                            End If

                            If FTP.FileExists(np) Then
                                If TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskReplace, "File: " & item.name) = DialogResult.Yes Then
                                    FTP.DeleteFile(np)
                                Else
                                    Continue For
                                End If
                            End If

                            FTP.DownloadFile(item.ftpPath, item.path)
                            FTP.UploadFile(np, item.path)
                        Catch ex As Exception
                            Debug.Print(ex.ToString)
                        End Try
                    Else
                        Try
                            If Directory.Exists(item.path) Then
                                Directory.Delete(item.path)
                            End If

                            If FTP.DirectoryExists(np) Then
                                If TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskReplace, "Folder: " & item.name) = DialogResult.Yes Then
                                    deleteDir(np)
                                Else
                                    Continue For
                                End If
                            End If

                            FTP.DownloadFolder(item.ftpPath, item.path)
                            uploaderForm.uploadFolder(item.name, item.path, np)
                        Catch ex As Exception
                            Debug.Print(ex.ToString)
                        End Try
                    End If
                End If
            Next

            clipmode = ClipAction.None
            clipboard.Clear()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantPaste)
            Debug.Print(ex.ToString)
        End Try

        populateListView(cpath)
    End Sub

#End Region

#Region " Form Code "

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refreshMain()

        If My.Settings.path = "" Then
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.NoURLSet)
        End If
    End Sub 'Load

    Public Sub refreshMain()
        TheMightyErrorMaster.Initialize()

        SizeLabel.Text = "Size: "
        SelectedItemsLabel.Text = "0 Selected"
        LastModifiedLabel.Text = "Last Modified: "

        My.Settings.Save()
        My.Settings.Reload()

        Me.MaximumSize = Screen.FromRectangle(Me.Bounds).WorkingArea.Size

        Select Case My.Settings.view
            Case 0
                ListView1.View = View.Tile
                ViewTiles.Checked = True
                ViewDetails.Checked = False
            Case 1
                ListView1.View = View.Details
                ViewTiles.Checked = False
                ViewDetails.Checked = True
        End Select

        If My.Settings.vlcauto Then
            My.Settings.vlcPath = isVLCinstalled()
        End If

        My.Settings.Save()
        My.Settings.Reload()

        If My.Settings.colorful And My.Settings.synced.Count > 0 Then
            Button12.BackColor = Color.DeepSkyBlue
        ElseIf Not My.Settings.colorful Or Not My.Settings.synced.Count > 0 Then
            Button12.BackColor = Panel2.BackColor
        ElseIf Not My.Settings.synced.Count > 0 And My.Settings.colorful Then
            Button12.BackColor = Color.SpringGreen
        End If

        If Not My.Settings.path.Equals("") And Not My.Settings.password.Equals("") And Not My.Settings.username.Equals("") Then
            cpath = "ftp://" & My.Settings.path.Substring(6)
            TextBox1.Text = cpath
            Application.DoEvents()

            If Not FTP.Connect(cpath, My.Settings.username, My.Settings.password) Then
                If My.Settings.colorful Then
                    TextBox1.BackColor = Color.MistyRose
                Else
                    TextBox1.BackColor = Color.White
                End If

                Exit Sub
            Else
                If My.Settings.colorful Then
                    TextBox1.BackColor = Color.PaleGreen
                Else
                    TextBox1.BackColor = Color.White
                End If
            End If

            Try
                populateListView(cpath)

                If My.Settings.synced.Count >= 1 Then
                    ToolTip1.SetToolTip(Me.Button12, "Syncing...")

                    Dim t1 As New Thread(AddressOf sync)
                    t1.Start()
                End If

                Timer1.Interval = 300000
                Timer1.Start()
                Timer2.Interval = 1000
                Timer2.Start()
                Timer4.Interval = 2000
                'Timer4.Start()
            Catch ex As Exception
                'debug.print(ex.ToString)
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.Unidentified, "Error: " & ex.Message)
            End Try
        End If
    End Sub

    Public Sub populateListView(ByVal path As String)
        Try
            ListView1.Items.Clear()
            TextBox1.Text = path
            lastList.Clear()

            If Not cpath.Equals("ftp://" & My.Settings.path.Substring(6)) Then
                Dim item As ListViewItem = New ListViewItem("...", 2)

                If ListView1.View = View.Tile Then
                    If searchmode Then
                        item.SubItems.Add("Go Back To")
                        item.SubItems.Add("Search Results")
                    Else
                        item.SubItems.Add("Go To")
                        item.SubItems.Add("Parent Folder")
                    End If

                    item.SubItems.Add("")
                End If

                ListView1.Items.Add(item)
                item = Nothing
            End If

            For Each fl As String In FTP.ListDirDetails(path)
                Dim t As Integer = 0
                Dim lastmod As Date

                Try
                    lastmod = getLastModifiedDate(fl)
                Catch ex As Exception
                    'Debug.Print(ex.ToString)
                End Try

                Dim fsize As ULong = 0

                lastList.Add(fl)

                Try
                    fsize = Convert.ToUInt64(fl.Split(" ")(4))
                Catch ex As Exception
                    'Debug.Print(ex.ToString)
                End Try

                Dim flt As String = fl
                fl = fl.Split(" ")(8)

                If flt.Split(" ").Count > 9 Then
                    For i As Integer = 9 To flt.Split(" ").Count - 1
                        fl = fl & " " & flt.Split(" ")(i)
                    Next
                End If

                Dim item As ListViewItem = New ListViewItem(fl, t)

                If flt.StartsWith("-") Then
                    t = getImage(fl)

                    If fl.LastIndexOf(".") >= 0 Then
                        item.SubItems.Add(fl.Substring(fl.LastIndexOf(".")) & " File")
                    Else
                        item.SubItems.Add("File")
                    End If
                Else
                    item.SubItems.Add("Folder")
                End If

                item.SubItems.Add(lastmod)

                If flt.StartsWith("-") Then
                    item.SubItems.Add(getSize(fsize))
                Else
                    item.SubItems.Add("")
                End If

                If My.Settings.lockedPaths.Contains(path & fl) Then
                    item.SubItems.Add("Locked")
                ElseIf Not My.Settings.lockedPaths.Contains(path & fl) Then
                    item.SubItems.Add("Unlocked")
                End If

                item.ImageIndex = t

                ListView1.Items.Add(item)
            Next

            'For Each fl As String In lastList
            '    Debug.Print(fl)
            'Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.Unidentified, "Error: " & ex.Message)
        End Try
    End Sub 'Populate ListView1

    ''' <summary>
    ''' Returns file size in a more readable way
    ''' </summary>
    ''' <param name="s">File Size In Bytes</param>
    ''' <returns>String File Size</returns>
    Function getSize(ByVal s As ULong) As String
        If s = 0 Then
            Return "0 Bytes"
        End If

        Try
            Dim labels() As String = {"Bytes", "KB", "MB", "GB", "TB"}
            Dim d As Double = Convert.ToDouble(s)
            Dim sl As Long = 0

            While (d > 1)
                d = d / 1024
                sl += 1
            End While

            d = d * 1024
            sl -= 1

            Return (Str(Math.Round(d, 1)) & " " & labels(sl))
        Catch ex As Exception
            Debug.Print("SIZE: " & s)
            Debug.Print(ex.ToString)
        End Try

        Return "0 Bytes"
    End Function

    Private Sub populateSearch(ByVal res As List(Of ListViewItem))
        ListView1.Items.Clear()
        ListView1.Items.AddRange(res.ToArray)
    End Sub

    Private Sub ListView1_ItemActivate(sender As Object, e As EventArgs) Handles ListView1.ItemActivate
        open()
    End Sub 'Item Click

    Public Sub openFile(ByVal path As String)
        Try
            path = "ftp://" & My.Settings.username & ":" & My.Settings.password & "@" & path.Substring(6)

            If My.Settings.vformats.Contains(IO.Path.GetExtension(path)) And Not openAs And File.Exists(My.Settings.vlcPath) Then
                If checkSub(path).Count > 0 Then
                    Dim subs As String = ""

                    For Each s As String In checkSub(path)
                        subs = subs & " --sub-file=" & Chr(34) & s & Chr(34)
                    Next

                    Process.Start(My.Settings.vlcPath, Chr(34) & path & Chr(34) & subs)
                Else
                    Process.Start(My.Settings.vlcPath, (Chr(34) & path & Chr(34)))
                End If
            ElseIf My.Settings.pformats.Contains(path.Substring(path.LastIndexOf(".")).ToLower) And Not openAs Then
                Dim dpath As String = IO.Path.GetTempPath & path.Substring(path.LastIndexOf("/") + 1)

                Dim dt As New Thread(Sub() FTP.DownloadFile(path, dpath))
                dt.Start()
                dt.Join()

                If File.Exists(dpath) Then
                    Process.Start(dpath)
                    openedFiles.Add(dpath)
                End If
            ElseIf My.Settings.aformats.Contains(path.Substring(path.LastIndexOf(".")).ToLower) And Not openAs Then
                Dim dpath As String = IO.Path.GetTempPath & path.Substring(path.LastIndexOf("/") + 1)

                Dim dt As New Thread(Sub() FTP.DownloadFile(path, dpath))
                dt.Start()
                dt.Join()

                If File.Exists(dpath) Then
                    Process.Start(dpath)
                    openedFiles.Add(dpath)
                End If
            Else
                Process.Start(path)
            End If
        Catch ex As Exception
            'debug.print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.Unidentified, "Error: " & ex.Message)
        Finally
            openAs = False
        End Try
    End Sub 'Open File/Folder

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        refreshPage()
    End Sub 'Refresh

    Public Sub go()
        Try
            If TextBox1.Text.Last = "/" Then
                TextBox1.Text = TextBox1.Text.Substring(0, TextBox1.Text.Length - 1)
            End If

            If Not FTP.DirectoryExists(TextBox1.Text) Then
                If Not FTP.FileExists(TextBox1.Text) Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.InvalidURL)
                    Exit Sub
                End If
            End If

            If checkNotFolder(TextBox1.Text) Then
                If My.Settings.lockedPaths.Contains(TextBox1.Text) Then
                    FolLock.unlockPass(TextBox1.Text, 0)
                Else
                    openFile(TextBox1.Text)
                End If
            Else
                If My.Settings.lockedPaths.Contains(TextBox1.Text) Then
                    FolLock.unlockPass(TextBox1.Text, 1)
                Else
                    cpath = TextBox1.Text
                    populateListView(cpath)
                End If
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.Unidentified, "Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        go()
    End Sub 'Go

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        SettingForm.Show()
    End Sub 'Show Settings

    Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
        Dim cnt As Integer = 0
        For Each c As Char In value
            If c = ch Then
                cnt += 1
            End If
        Next
        Return cnt
    End Function 'Counts Characters In A String

    Private Sub refreshPage()
        FTP.errors = True

        If searchmode Then
            searchdone = False
            ListView1.Items.Clear()

            Timer3.Start()
            Dim t1 As New Thread(AddressOf search)
            t1.Start(TextBox2.Text)
        Else
            FTP.Disconnect()
            If FTP.Connect(cpath, My.Settings.username, My.Settings.password) Then
                populateListView(cpath)

                Timer4.Start()
            End If
        End If
    End Sub

    Private Sub ListView1_Key_Up(sender As Object, e As KeyEventArgs) Handles ListView1.KeyUp
        Try
            e.Handled = True

            If e.KeyCode = Keys.F5 Then
                refreshPage()
                Exit Sub
            End If

            If e.KeyCode = Keys.Back Then
                If CountCharacter(TextBox1.Text, "/") > 3 Then
                    cpath = cpath.Substring(0, cpath.LastIndexOfAny("/"))
                    cpath = cpath.Substring(0, cpath.LastIndexOfAny("/") + 1)
                    populateListView(cpath)
                    Exit Sub
                ElseIf CountCharacter(TextBox1.Text, "/") <= 3 Then
                    Exit Sub
                End If
            End If

            If Not ListView1.SelectedItems.Count > 0 Then
                Exit Sub
            End If

            If e.KeyCode = Keys.Delete Then
                If Not TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskDelete) = DialogResult.Yes Then
                    Exit Sub
                End If

                For Each it As ListViewItem In ListView1.SelectedItems
                    Application.DoEvents()

                    If it.SubItems(1).Text.Contains(" File") Then
                        FTP.DeleteFile(cpath & it.Text)
                        populateListView(cpath)
                    ElseIf it.SubItems(1).Text.Equals("Folder") Then
                        deleteDir(cpath & it.Text)
                        populateListView(cpath)
                    End If
                Next

                Exit Sub
            End If

            If e.KeyCode = Keys.Enter And Not ListView1.SelectedItems(0).Text.Equals("...") Then
                open()

                Exit Sub
            End If
        Catch ex As Exception
            'debug.print(ex.ToString)
        End Try
    End Sub 'Key Press

    Private Sub open()
        If ListView1.SelectedItems().Count <= 0 Then
            Exit Sub
        End If

        If searchmode Then
            Dim i As ListViewItem = ListView1.SelectedItems(0)
            Dim path As String = cpath & i.Text

            If i.Text = "..." Then
                If i.SubItems(1).Text = "End Search" Then
                    TextBox2.Text = ""
                    Exit Sub
                Else
                    populateSearch(searchres)
                    Exit Sub
                End If
            End If

            If i.SubItems(1).Text = "Folder" Then
                If My.Settings.lockedPaths.Contains(path) Then
                    FolLock.unlockPass(path, 1)
                Else
                    populateListView(path)
                End If
            Else
                If My.Settings.lockedPaths.Contains(path) Then
                    FolLock.unlockPass(path, 0)
                Else
                    openFile(path)
                End If
            End If
        Else
            Dim item As ListViewItem = ListView1.SelectedItems(0)

            If item.Text.Equals("...") And CountCharacter(TextBox1.Text, "/") > 3 Then
                cpath = cpath.Substring(0, cpath.LastIndexOfAny("/"))
                cpath = cpath.Substring(0, cpath.LastIndexOfAny("/") + 1)
                populateListView(cpath)
                Exit Sub
            ElseIf item.Text.Equals("...") And CountCharacter(TextBox1.Text, "/") <= 3 Then
                Exit Sub
            End If

            If Not checkNotFolder(cpath & item.Text) Then
                Dim tpath As String = cpath
                If tpath.Substring(tpath.Length - 1).Equals("/") Then
                    tpath = tpath & item.Text & "/"
                Else
                    tpath = tpath & "/" & item.Text & "/"
                End If

                If My.Settings.lockedPaths.Contains(tpath.Substring(0, tpath.Length - 1)) Then
                    FolLock.unlockPass(tpath.Substring(0, tpath.Length - 1), 1)
                Else
                    cpath = tpath
                    populateListView(tpath)
                End If
            Else
                Dim fpath As String = ""

                If cpath.Substring(cpath.Length - 1).Equals("/") Then
                    fpath = cpath & item.Text
                Else
                    fpath = cpath & "/" & item.Text
                End If

                If My.Settings.lockedPaths.Contains(fpath) Then
                    FolLock.unlockPass(fpath, 0)
                Else
                    openFile(fpath)
                End If
            End If
        End If

        openAs = False
        SyncForm.TextBox2.Text = cpath
    End Sub

    ''' <summary>
    ''' Checks whether a FTP url is a file or folder
    ''' </summary>
    ''' <param name="dir">URL of file/folder</param>
    ''' <returns>True if file, false if folder</returns>
    Public Function checkNotFolder(ByVal dir As String) As Boolean
        'Debug.Print("CNF: " & dir)

        Try
            If dir.Last = "/" Then
                dir = dir.Substring(0, dir.Length - 1)
            End If

            For Each f As PawFile In FTP.ListDir(dir.Substring(0, dir.LastIndexOf("/")))
                If f.ftpPath = dir Then
                    If f.type = PawFile.Types.File Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            Next
        Catch ex As Exception
            'Debug.Print(ex.ToString)
        End Try

        Return True
    End Function 'Check Not Folder -> File : True | Folder : False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        uploaderForm.Show()
        uploaderForm.Focus()
    End Sub ' Upload File

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        makeDir()
    End Sub ' Make Dir

    Private Sub makeDir()
        Try
            Dim fl As String = InputBox("New Folder Name:", "Create Folder On FTP Server", "New Folder")

            If Not FTP.DirectoryExists(cpath & fl) And Not fl.Equals("") Then
                If Not FTP.MakeDir(cpath & fl) Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantMakeDir)
                End If
            End If

            populateListView(cpath)
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantMakeDir)
        End Try
    End Sub

    Private Function checkSub(ByVal path As String) As String()
        Try
            Dim subfile As New List(Of String)
            Dim downloadpath As New List(Of String)
            Dim c As Integer = 0
            Dim sube As Boolean = False

            For Each s As String In lastList
                If s.Contains(".srt") Or s.Contains(".sub") Then
                    sube = True
                    subfile.Add(s)
                End If
            Next

            If Not sube Then
                Return {}
            End If

            For Each s As String In subfile
                Dim slt As String = s
                s = s.Split(" ")(8)

                If slt.Split(" ").Count > 9 Then
                    For i As Integer = 9 To slt.Split(" ").Count - 1
                        s = s & " " & slt.Split(" ")(i)
                    Next
                End If

                If File.Exists(IO.Path.GetTempPath & s) Then
                    Try
                        File.Delete(IO.Path.GetTempPath & s)
                    Catch ex As Exception
                        'debug.print(ex.ToString)
                    End Try
                End If

                Dim dt As New Thread(Sub() FTP.DownloadFile(cpath & s, IO.Path.GetTempPath & s))
                dt.Start()
                dt.Join()

                If File.Exists(IO.Path.GetTempPath & s) Then
                    downloadpath.Add(IO.Path.GetTempPath & s)
                End If
            Next

            Return downloadpath.ToArray
        Catch ex As Exception
            'debug.print(ex.ToString)
            Return {}
        End Try
    End Function ' Download Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If Not TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.AskDelete) = DialogResult.Yes Then
            Exit Sub
        End If

        FailedLabel.Text = 0
        SuccessfulLabel.Text = 0

        For Each item As ListViewItem In ListView1.SelectedItems
            Application.DoEvents()

            If item.SubItems(1).Text.Contains(" File") Then
                Dim t1 As New Thread(AddressOf FTP.DeleteFile)
                t1.Start(cpath & item.Text)
                t1.Join()

                SuccessfulLabel.Text = Convert.ToInt32(SuccessfulLabel.Text) + 1

                populateListView(cpath)
            ElseIf item.SubItems(1).Text.Equals("Folder") Then
                Dim t1 As New Thread(AddressOf deleteDir)
                t1.Start(cpath & item.Text)
                t1.Join()

                SuccessfulLabel.Text = Convert.ToInt32(SuccessfulLabel.Text) + 1

                populateListView(cpath)
            End If
        Next
    End Sub 'Delete Files / Folders

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        download()
    End Sub 'Download Files

    Private Sub download()
        If ListView1.SelectedItems.Count > 0 Then
            FailedLabel.Text = 0
            SuccessfulLabel.Text = 0

            For Each file As ListViewItem In ListView1.SelectedItems
                Application.DoEvents()
                SaveFileDialog1.FileName = file.Text

                If file.SubItems(1).Text = "Folder" Then
                    SaveFileDialog1.Filter = "Folders (*)|*"
                    SaveFileDialog1.AddExtension = False
                    SaveFileDialog1.DefaultExt = ""
                    SaveFileDialog1.Title = "Save Folder"
                Else
                    Dim ext As String = file.Text.Substring(file.Text.LastIndexOf("."))
                    SaveFileDialog1.Filter = ext & " Files (*" & ext & "*)|*" & ext
                    SaveFileDialog1.AddExtension = True
                    SaveFileDialog1.DefaultExt = ext
                    SaveFileDialog1.Title = "Save File"
                End If


                If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
                    Application.DoEvents()

                    Try
                        If file.SubItems(1).Text = "Folder" Then
                            If Not FTP.DownloadFolder(cpath & file.Text, SaveFileDialog1.FileName) Then
                                FailedLabel.Text = Convert.ToInt32(FailedLabel.Text) + 1
                            End If
                        Else
                            If Not FTP.DownloadFile(cpath & file.Text, SaveFileDialog1.FileName) Then
                                FailedLabel.Text = Convert.ToInt32(FailedLabel.Text) + 1
                            End If
                        End If
                    Catch ex As Exception
                        TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDonwload)
                        FailedLabel.Text = Convert.ToInt32(FailedLabel.Text) + 1
                    End Try

                    Application.DoEvents()
                End If
            Next
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If Not My.Settings.path.Equals("") And Not My.Settings.password.Equals("") And Not My.Settings.username.Equals("") Then
            cpath = "ftp://" & My.Settings.path.Substring(6)
            TextBox1.Text = cpath
            Application.DoEvents()

            FTP.Disconnect()
            FTP.Connect(cpath, My.Settings.username, My.Settings.password)
            Try
                populateListView(cpath)
            Catch ex As Exception
                'debug.print(ex.ToString)
            End Try
        End If
    End Sub 'Go to homepage

    Public Function getImage(ByVal file As String) As Integer
        Dim ext As String = file.Substring(file.LastIndexOfAny(".") + 1)
        Dim cnt As Integer = 0
        'Debug.Print(ext)

        If ext.ToLower.Equals("png") Then
            Return 20
        End If

        For Each img As String In imageList1.Images.Keys
            If img.IndexOf(ext, 0, StringComparison.CurrentCultureIgnoreCase) > -1 Then
                Return cnt
            End If

            cnt += 1
        Next

        Return 1
    End Function 'Get Image of Extension

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        rename()
    End Sub 'Rename File

    Private Sub rename()
        Try
            Dim item As ListViewItem = ListView1.SelectedItems(0)

            If checkNotFolder(cpath & item.Text) Then
                Dim fl As String = InputBox("New File Name:", "Rename File On FTP Server", item.Text)

                If Not FTP.RenameFile(cpath & item.Text, fl) Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantRename)
                End If
            Else
                Dim fl As String = InputBox("New Folder Name:", "Rename Folder On FTP Server", item.Text)

                If Not FTP.RenameFile(cpath & item.Text, fl) Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantRename)
                End If
            End If

            populateListView(cpath)
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Deletes selected folders and their content
    ''' </summary>
    Public Sub deleteDir(ByVal url As String)
        Try
            If url.Last <> "/" Then
                url = url & "/"
            End If

            Dim fl As List(Of String) = FTP.ListDirDetails(url)

            If Not fl.Count <= 0 Then
                For Each file As String In fl
                    Application.DoEvents()

                    If file.StartsWith("-") Then
                        Dim flt As String = file
                        file = file.Split(" ")(8)

                        If flt.Split(" ").Count > 9 Then
                            For i As Integer = 9 To flt.Split(" ").Count - 1
                                file = file & " " & flt.Split(" ")(i)
                            Next
                        End If

                        'Debug.Print("FILE: " & url & file)

                        FTP.DeleteFile(url & file)
                    ElseIf file.StartsWith("d") Then
                        Dim flt As String = file
                        file = file.Split(" ")(8)

                        If flt.Split(" ").Count > 9 Then
                            For i As Integer = 9 To flt.Split(" ").Count - 1
                                file = file & " " & flt.Split(" ")(i)
                            Next
                        End If

                        'Debug.Print("FOLDER: " & url & file)

                        deleteDir(url & file)
                    End If
                Next
            Else
                FTP.DeleteDir(url)
                Exit Sub
            End If

            FTP.DeleteDir(url)
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDelete)
        End Try
    End Sub

    Public Function cleanUp()
        For Each fl As String In openedFiles
            Try
                File.Delete(fl)
            Catch ex As Exception
                Debug.Print(ex.ToString)
                Return True
            End Try
        Next

        Return True
    End Function

    Public Function isVLCinstalled() As String
        Try
            Dim KeyValue As String = ""
            Dim baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
            Dim regkey = baseKey.OpenSubKey("SOFTWARE\VideoLAN\VLC")
            If regkey IsNot Nothing Then KeyValue = CStr(regkey.GetValue(""))

            If KeyValue = "" Then
                Return KeyValue
            End If

            If Not File.Exists(KeyValue) Then
                Return ""
            End If

            Return KeyValue

            'If readValue Is Nothing Then
            '    Return "Not Installed"
            'End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToReadRegistryValue)
            Return ""
        End Try
    End Function

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ListView1.SelectedItems.Count <= 0 Then
            Exit Sub
        End If

        If Not My.Settings.folpass.Equals("") Then
            For Each item As ListViewItem In ListView1.SelectedItems
                Try
                    Debug.Print("Locked: " & cpath & item.Text)
                    My.Settings.lockedPaths.Add(cpath & item.Text)
                    My.Settings.Save()
                    My.Settings.Reload()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantLock)
                End Try
            Next

            populateListView(cpath)
        Else
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.NoLockPasswordSet)
        End If
    End Sub 'Lock

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If ListView1.SelectedItems.Count <= 0 Then
            Exit Sub
        End If

        Dim ls As New List(Of String)

        For Each item As ListViewItem In ListView1.SelectedItems
            Try
                Debug.Print("Unlocked: " & cpath & item.Text)
                ls.Add(cpath & item.Text)
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantUnlock)
            End Try
        Next

        FolLock.unlockFolders(ls)
    End Sub 'Unlock

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        SyncForm.Show()
        SyncForm.Focus()
        SyncForm.TextBox2.Text = cpath
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not syncstat.Contains("Syncing") Then
            If My.Settings.colorful Then
                Me.Button12.BackColor = Color.DeepSkyBlue
            End If

            ToolTip1.SetToolTip(Me.Button12, "Syncing...")

            Dim t1 As New Thread(AddressOf sync)
            t1.Start()
        End If
    End Sub

    Public Function syncTime() As TimeSpan
        If My.Settings.syncPaths.Count <= 0 Then
            Exit Function
        End If

        Dim ts As New TimeSpan
        Dim ct As Date
        Dim ut As Date
        Dim path As String = IO.Path.GetTempPath & "ts.txt"
        Dim upath As String = My.Settings.syncPaths(0)

        If upath.Last() <> "/" Then
            upath = upath & "/"
        End If

        Try
            If File.Exists(path) Then
                File.Delete(path)
            End If

            Using sw As StreamWriter = File.CreateText(path)
                sw.WriteLine("TS")
            End Using

            ct = DateTime.Now
            FTP.UploadFile(upath & "ts.txt", path)
            ut = FTP.GetDateTimeStamp(upath & "ts.txt")
            FTP.DeleteFile(upath & "ts.txt")

            Debug.Print("CT: " & ct)
            Debug.Print("UT: " & ut)

            ts = ct.Subtract(ut)
        Catch ex As Exception
            'Debug.Print(ex.ToString)
        End Try

        Return ts
    End Function

    Public Async Sub sync()
        If My.Settings.synced.Count < 1 Then
            Exit Sub
        End If

        Debug.Print("Syncing ...")
        syncstat = "Sync Status: Syncing"

        Timer1.Stop()

        Timer2.Interval = 1000
        Timer2.Start()

        ' Sync Time '
        Dim timeDiff As TimeSpan
        timeDiff = syncTime()

        Debug.Print("TIME DIFFERENCE: " & timeDiff.ToString)

        ' PREP '
        Dim prevPC As New List(Of String)
        Dim prevFTP As New List(Of String)

        For Each folder As String In My.Settings.synced
            If IO.Directory.Exists(folder) Then
                ' Create Folders

                Try
                    For Each subfolder As String In Directory.EnumerateDirectories(folder, "*", System.IO.SearchOption.AllDirectories)
                        Try
                            Dim dest As String = My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))

                            If dest.Last() <> "\" Then
                                dest = dest & "\"
                            End If

                            Dim folderURL As String = (dest & subfolder.Substring(folder.LastIndexOfAny("\") + folder.Substring(folder.LastIndexOf("\") + 1).Length + 2))

                            If Not Directory.Exists(folderURL) Then
                                Directory.CreateDirectory(folderURL)
                            End If
                        Catch ex As Exception
                            Debug.Print(ex.ToString)
                        End Try
                    Next
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Try
                    For Each file As String In Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                        If closeapp Then
                            syncstat = "Sync Status: Sync Complete"
                            My.Settings.lastSync = DateTime.Now
                            Exit Sub
                        End If

                        Try
                            If folder.Last.Equals("\") Then
                                folder = folder.Substring(0, folder.Length - 2)
                            End If

                            Dim dest As String = My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))

                            If dest.Last() <> "\" Then
                                dest = dest & "\"
                            End If

                            If file.Last() = "\" Then
                                file = file.Substring(0, file.Length - 2)
                            End If

                            Dim fileURL As String = (dest & file.Substring(folder.Length + 1))
                            Dim infoReader As System.IO.FileInfo
                            infoReader = My.Computer.FileSystem.GetFileInfo(file)

                            Dim lastmod As Date = infoReader.LastWriteTime
                            lastmod = lastmod.AddSeconds(Convert.ToDouble(60 - lastmod.Second))

                            Dim cf As New PawFile With {.path = file, .dt = lastmod, .size = infoReader.Length, .ftpPath = fileURL, .type = PawFile.Types.File}

                            If IO.File.Exists(fileURL) Then
                                infoReader = My.Computer.FileSystem.GetFileInfo(fileURL)
                                lastmod = infoReader.LastWriteTime
                                lastmod = lastmod.AddSeconds(Convert.ToDouble(60 - lastmod.Second))
                            End If

                            Dim df As New PawFile With {.path = fileURL, .dt = lastmod, .size = infoReader.Length, .ftpPath = file, .type = PawFile.Types.File}

                            Debug.Print("Source:      " & cf.path)
                            Debug.Print("Destination: " & df.path)

                            If IO.File.Exists(df.path) Then
                                Dim datec As Integer = DateTime.Compare(cf.dt, df.dt)

                                If datec < 0 Then ' Dest newer
                                    IO.File.Delete(cf.path)

                                    Using SourceStream As FileStream = IO.File.Open(cf.path, FileMode.Open)
                                        Using DestinationStream As FileStream = IO.File.Create(df.path)
                                            Await SourceStream.CopyToAsync(DestinationStream)
                                        End Using
                                    End Using

                                    IO.File.SetLastWriteTime(df.path, df.dt)
                                    IO.File.SetLastWriteTime(cf.path, df.dt)
                                ElseIf datec > 0 Then ' Source newer 
                                    IO.File.Delete(df.path)

                                    Using SourceStream As FileStream = IO.File.Open(cf.path, FileMode.Open)
                                        Using DestinationStream As FileStream = IO.File.Create(df.path)
                                            Await SourceStream.CopyToAsync(DestinationStream)
                                        End Using
                                    End Using

                                    IO.File.SetLastWriteTime(df.path, cf.dt)
                                    IO.File.SetLastWriteTime(cf.path, cf.dt)
                                End If
                            Else
                                Using SourceStream As FileStream = IO.File.Open(cf.path, FileMode.Open)
                                    Using DestinationStream As FileStream = IO.File.Create(df.path)
                                        Await SourceStream.CopyToAsync(DestinationStream)
                                    End Using
                                End Using

                                IO.File.SetLastWriteTime(df.path, cf.dt)
                                IO.File.SetLastWriteTime(cf.path, cf.dt)
                            End If
                        Catch ex As Exception
                            Debug.Print(ex.ToString)
                        End Try
                    Next
                Catch ex As Exception
                    Debug.Print(ex.ToString)
                End Try

                Continue For
            End If

            ' Sync FTP '

            If Not FTP.DirectoryExists(My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))) Then
                FTP.MakeDir(My.Settings.syncPaths(My.Settings.synced.IndexOf(folder)))
            End If

            Try
                If Directory.Exists(folder) Then
                    Try
                        For Each subfolder As String In Directory.EnumerateDirectories(folder, "*", System.IO.SearchOption.AllDirectories)
                            Try
                                Dim url As String = My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))

                                If url.Last() <> "/" Then
                                    url = url & "/"
                                End If

                                Dim folderURL As String = (url & subfolder.Substring(folder.LastIndexOfAny("\") + folder.Substring(folder.LastIndexOf("\") + 1).Length + 2)).Replace("\", "/")

                                If Not FTP.DirectoryExists(folderURL) Then
                                    FTP.MakeDir(folderURL)
                                End If
                            Catch ex As Exception
                                Debug.Print(ex.ToString)
                            End Try
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    Dim fileList As New List(Of PawFile)
                    Dim PawFileList As New List(Of PawFile)
                    Dim tfileList As New List(Of PawFile)
                    Dim tPawFileList As New List(Of PawFile)

                    ' PC FILES '
                    Try
                        For Each file As String In Directory.EnumerateFiles(folder, "*.*", SearchOption.AllDirectories)
                            If closeapp Then
                                syncstat = "Sync Status: Sync Complete"
                                My.Settings.lastSync = DateTime.Now
                                Exit Sub
                            End If

                            Try
                                If folder.Last.Equals("\") Then
                                    folder = folder.Substring(0, folder.Length - 2)
                                End If

                                Dim url As String = My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))

                                If url.Last() <> "/" Then
                                    url = url & "/"
                                End If

                                Dim fileURL As String = (url & file.Substring(folder.Length + 1)).Replace("\", "/")
                                Dim infoReader As System.IO.FileInfo
                                infoReader = My.Computer.FileSystem.GetFileInfo(file)

                                Dim lastmod As Date = infoReader.LastWriteTime
                                lastmod = lastmod.AddSeconds(Convert.ToDouble(60 - lastmod.Second))
                                'Debug.Print("FURL: " & fileURL)
                                Dim cf As New PawFile With {.path = file, .dt = lastmod, .size = infoReader.Length, .ftpPath = fileURL, .type = PawFile.Types.File}
                                fileList.Add(cf)
                            Catch ex As Exception
                                Debug.Print(ex.ToString)
                            End Try
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    ' FTP FILES '
                    Try
                        For Each file As PawFile In FTP.ListAllDirs(My.Settings.syncPaths(My.Settings.synced.IndexOf(folder)))
                            If closeapp Then
                                syncstat = "Sync Status: Sync Complete"
                                My.Settings.lastSync = DateTime.Now
                                Exit Sub
                            End If

                            Try
                                If folder.Last.Equals("\") Then
                                    folder = folder.Substring(0, folder.Length - 2)
                                End If

                                Dim url As String = My.Settings.syncPaths(My.Settings.synced.IndexOf(folder))

                                If url.Last() <> "/" Then
                                    url = url & "/"
                                End If

                                Dim filePath As String = (folder & "\" & file.ftpPath.Replace(url, "")).Replace("/", "\")
                                Dim lastmod As Date = file.dt.AddSeconds(60 - file.dt.Second)

                                Dim cf As New PawFile With {.path = filePath, .dt = lastmod, .size = file.size, .ftpPath = file.ftpPath, .type = file.type}

                                If cf.type = PawFile.Types.File Then
                                    PawFileList.Add(cf)
                                    'Debug.Print("FFURL: " & cf.ftpPath)
                                End If
                            Catch ex As Exception
                                Debug.Print(ex.ToString)
                            End Try

                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    tfileList = fileList
                    tPawFileList = PawFileList

                    'SYNC'

                    Try
                        For Each file As PawFile In tfileList
                            If closeapp Then
                                syncstat = "Sync Status: Sync Complete"
                                My.Settings.lastSync = DateTime.Now
                                Exit Sub
                            End If

                            Dim exist As Boolean = False

                            Try
                                For Each cfile As PawFile In tPawFileList
                                    If closeapp Then
                                        syncstat = "Sync Status: Sync Complete"
                                        My.Settings.lastSync = DateTime.Now
                                        Exit Sub
                                    End If

                                    Try
                                        If cfile.ftpPath = file.ftpPath Then
                                            exist = True
                                            Dim dc As Integer = DateTime.Compare(file.dt, cfile.dt)
                                            Dim dm As Integer = file.dt.Subtract(cfile.dt).Minutes

                                            If dc = 0 Then
                                                Exit For
                                            End If

                                            If dm >= 3 Then
                                                If dc < 0 Then
                                                    Try
                                                        Dim nd As Date = DateTime.Now

                                                        Debug.Print("PFILE: " & file.ftpPath)
                                                        Debug.Print("PDATE: " & file.dt)
                                                        Debug.Print("FDATE: " & cfile.dt)

                                                        If Not My.Settings.secure Then
                                                            IO.File.Delete(cfile.path)
                                                            FTP.DownloadFile(file.ftpPath, file.path)
                                                        End If

                                                        IO.File.SetLastWriteTime(file.path, nd)

                                                        fileList.Item(fileList.IndexOf(file)).dt = nd

                                                        Debug.Print("PC UPDATED")
                                                    Catch ex As Exception
                                                        Debug.Print(ex.ToString)
                                                    End Try
                                                ElseIf dc > 0 Then
                                                    Debug.Print("PFILE: " & file.ftpPath)
                                                    Debug.Print("PDATE: " & file.dt)
                                                    Debug.Print("FDATE: " & cfile.dt)

                                                    Try
                                                        FTP.DeleteFile(file.ftpPath)
                                                        FTP.UploadFile(file.ftpPath, cfile.path)

                                                        Dim nd As Date = FTP.GetDateTimeStamp(file.ftpPath)

                                                        IO.File.SetLastWriteTime(file.path, nd)

                                                        PawFileList.Item(PawFileList.IndexOf(cfile)).dt = nd

                                                        Debug.Print("FTP UPDATED")
                                                    Catch ex As Exception
                                                        'Debug.Print(ex.ToString)
                                                    End Try
                                                End If
                                            End If

                                            Exit For
                                        End If
                                    Catch ex As Exception
                                        Debug.Print(ex.ToString)
                                    End Try
                                Next
                            Catch ex As Exception
                                Debug.Print(ex.ToString)
                            End Try

                            ' DEL FROM FTP '
                            If Not exist Then
                                If Not My.Settings.prevFTP.Contains(file.ftpPath) And IO.File.Exists(file.path) Then
                                    Try
                                        FTP.UploadFile(file.ftpPath, file.path)
                                        IO.File.SetLastWriteTime(file.path, FTP.GetDateTimeStamp(file.ftpPath))
                                        PawFileList.Add(file)

                                        Debug.Print("PFILE: " & file.ftpPath)
                                        Debug.Print("FTP UPLOADED")
                                    Catch ex As Exception
                                        Debug.Print(ex.ToString)
                                    End Try
                                ElseIf My.Settings.prevFTP.Contains(file.ftpPath) And IO.File.Exists(file.path) Then
                                    Try
                                        If Not My.Settings.secure Then
                                            IO.File.Delete(file.path)
                                        End If

                                        fileList.RemoveAt(fileList.IndexOf(file))

                                        Debug.Print("PFILE: " & file.path)
                                        Debug.Print("PC DELETED")
                                    Catch ex As Exception
                                        Debug.Print(ex.ToString)
                                    End Try
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    tfileList = fileList
                    tPawFileList = PawFileList

                    Try
                        For Each file As PawFile In tPawFileList
                            If closeapp Then
                                syncstat = "Sync Status: Sync Complete"
                                My.Settings.lastSync = DateTime.Now
                                Exit Sub
                            End If

                            'Debug.Print("FFILE: " & file.ftpPath)
                            Dim exist As Boolean = False

                            For Each cfile As PawFile In tfileList
                                If closeapp Then
                                    syncstat = "Sync Status: Sync Complete"
                                    My.Settings.lastSync = DateTime.Now
                                    Exit Sub
                                End If

                                If cfile.ftpPath = file.ftpPath Then
                                    exist = True
                                    Dim dc As Integer = DateTime.Compare(cfile.dt, file.dt)
                                    Dim dm As Integer = My.Settings.lastSync.Subtract(file.dt).Minutes

                                    If dm >= 3 Then
                                        Debug.Print("DM: " & dm)

                                        If dc < 0 Then
                                            Try
                                                Dim nd As Date = FTP.GetDateTimeStamp(file.ftpPath)

                                                Debug.Print("PFILE: " & file.ftpPath)
                                                Debug.Print("PDATE: " & cfile.dt)
                                                Debug.Print("FDATE: " & file.dt)

                                                If Not My.Settings.secure Then
                                                    IO.File.Delete(cfile.path)
                                                    FTP.DownloadFile(file.ftpPath, file.path)
                                                End If

                                                IO.File.SetLastWriteTime(file.path, nd)

                                                fileList.Item(fileList.IndexOf(file)).dt = nd

                                                Debug.Print("PC UPDATED")
                                            Catch ex As Exception
                                                'Debug.Print(ex.ToString)
                                            End Try
                                        ElseIf dc > 0 Then
                                            Debug.Print("PFILE: " & file.ftpPath)
                                            Debug.Print("PDATE: " & cfile.dt)
                                            Debug.Print("FDATE: " & file.dt)

                                            Try
                                                FTP.DeleteFile(file.ftpPath)
                                                FTP.UploadFile(file.ftpPath, file.path)

                                                Dim nd As Date = FTP.GetDateTimeStamp(file.ftpPath)

                                                IO.File.SetLastWriteTime(file.path, nd)

                                                PawFileList.Item(PawFileList.IndexOf(cfile)).dt = nd

                                                Debug.Print("FTP UPDATED")
                                            Catch ex As Exception
                                                'Debug.Print(ex.ToString)
                                            End Try
                                        End If
                                    End If

                                    Exit For
                                End If
                            Next

                            ' DEL FROM FTP '
                            If Not exist Then
                                If My.Settings.prevPC.Contains(file.ftpPath) Then
                                    Try
                                        FTP.DeleteFile(file.ftpPath)

                                        PawFileList.RemoveAt(PawFileList.IndexOf(file))

                                        Debug.Print("PFILE: " & file.ftpPath)
                                        Debug.Print("FTP DELETED")
                                    Catch ex As Exception
                                        Debug.Print(ex.ToString)
                                    End Try
                                Else
                                    Try
                                        FTP.DownloadFile(file.ftpPath, file.path)
                                        IO.File.SetLastWriteTime(file.path, DateTime.Now)

                                        fileList.Add(file)
                                        IO.File.SetLastWriteTime(file.path, FTP.GetDateTimeStamp(file.ftpPath))

                                        Debug.Print("PFILE: " & file.ftpPath)
                                        Debug.Print("PC DOWNLOADED")
                                    Catch ex As Exception
                                        Debug.Print(ex.ToString)
                                    End Try
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try

                    ' Sync '

                    Try
                        For Each file As PawFile In PawFileList
                            prevFTP.Add(file.ftpPath)
                        Next

                        For Each file As PawFile In fileList
                            prevPC.Add(file.ftpPath)
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
                End If
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.SyncError)
            End Try
        Next

        Try
            My.Settings.prevPC.Clear()
            My.Settings.prevFTP.Clear()
            My.Settings.lastSync = DateTime.Now.AddSeconds(Convert.ToDouble(60 - DateTime.Now.Second))
            My.Settings.Save()
            My.Settings.Reload()

            My.Settings.prevFTP.AddRange(prevFTP.ToArray)
            My.Settings.prevPC.AddRange(prevPC.ToArray)

            My.Settings.Save()
            My.Settings.Reload()

            SyncForm.Label3.Text = "Sync Status: Sync Complete"
            SyncForm.Button4.Enabled = True
            syncstat = "Sync Status: Sync Complete"
            Debug.Print("Sync Complete")

            Timer1.Interval = 120000
            Timer1.Start()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting, "Also there is a Paw Sync error.")
        End Try
    End Sub

    Public Shared Function sortArr(ByVal strarr As String()) As String()
        Dim sorted = strarr.OrderBy(Function(x) x.Length).ThenBy(Function(x) x).ToArray()

        Return sorted
    End Function

    Public Function getAllDirDetails(ByVal dir As String) As List(Of String)
        Dim output As New List(Of String)
        Try
            For Each file As String In Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories)
                output.Add(file)
            Next

            For Each folder As String In Directory.GetDirectories(dir, "*", SearchOption.AllDirectories)
                output.Add(folder)
            Next
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.Unidentified, "Error: " & ex.Message)
        End Try

        Return output
    End Function

    Private Sub Button14_Click(sender As Object, e As EventArgs)
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        Me.Show()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs)
        If syncstat.Contains("Syncing") Then
            closeapp = True
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.SyncInProgress)
            Exit Sub
        End If

        Me.Hide()
        Application.DoEvents()
        cleanUp()
        FTP.Disconnect()
        Application.ExitThread()
        Application.Exit()
    End Sub

    Public Function getLastModifiedDate(ByVal fileDetails As String) As DateTime
        Dim dt As DateTime

        Try
            Dim year As Integer = DateTime.Now.Year
            Dim month As Integer = DateTime.Now.Month
            Dim day As Integer = DateTime.Now.Day
            Dim hour As Integer = DateTime.Now.Hour
            Dim minute As Integer = DateTime.Now.Second
            Dim months() As String = {"jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec"}

            month = Array.IndexOf(months, fileDetails.Split(" ")(5).ToLower()) + 1

            day = Convert.ToInt32(fileDetails.Split(" ")(6))

            If fileDetails.Split(" ")(7).Contains(":") Then
                hour = Convert.ToInt32(fileDetails.Split(" ")(7).Split(":")(0)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours
                minute = Convert.ToInt32(fileDetails.Split(" ")(7).Split(":")(1)) + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Minutes

                If minute >= 60 Then
                    minute = minute - 60
                    hour = hour + 1
                End If

                If hour >= 24 Then
                    hour = hour - 24
                End If

                dt = New DateTime(DateTime.Now.Year, month, day, hour, minute, 0)
            Else
                year = Convert.ToInt32(fileDetails.Split(" ")(7))

                dt = New DateTime(year, month, day, 12, 0, 0)
            End If
        Catch ex As Exception
            Debug.Print(fileDetails)
            Debug.Print(ex.ToString)
        End Try

        Return dt
    End Function

    Public Sub updateLabels()
        Try
            If ListView1.SelectedItems.Count > 0 Then
                Dim st As Integer = ListView1.SelectedItems.Count

                For Each i As ListViewItem In ListView1.SelectedItems()
                    If i.Text = "..." Then
                        st = st - 1
                        Exit For
                    End If
                Next

                SelectedItemsLabel.Text = st & " Selected"
            Else
                SelectedItemsLabel.Text = "0 Selected"
            End If

            If ListView1.SelectedItems.Count = 1 Then
                If ListView1.SelectedItems(0).Text <> "..." Then
                    SizeLabel.Text = "Size: " & ListView1.SelectedItems(0).SubItems(3).Text
                    LastModifiedLabel.Text = "Last Modified: " & ListView1.SelectedItems(0).SubItems(2).Text
                Else
                    SelectedItemsLabel.Text = "0 Selected"
                End If
            Else
                SizeLabel.Text = "Size: "
                LastModifiedLabel.Text = "Last Modified: "
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        updateLabels()

        If ListView1.SelectedItems.Count > 0 Then
            If ListView1.SelectedItems(0).Text <> "..." Then
                Button6.Enabled = True
                Button7.Enabled = True
                Button9.Enabled = False
                Button10.Enabled = True
                Button11.Enabled = False

                If ListView1.SelectedItems.Count = 1 Then
                    Button9.Enabled = True

                    If ListView1.SelectedItems(0).SubItems(4).Text = "Locked" Then
                        Button11.Enabled = True
                    Else
                        Button10.Enabled = True
                    End If
                End If
            Else
                Button6.Enabled = False
                Button7.Enabled = False
                Button9.Enabled = False
                Button10.Enabled = False
                Button11.Enabled = False
            End If
        Else
            Button6.Enabled = False
            Button7.Enabled = False
            Button9.Enabled = False
            Button10.Enabled = False
            Button11.Enabled = False
        End If

        If searchmode Then
            Button1.Enabled = False
            Button4.Enabled = False
            Button9.Enabled = False
        Else
            Button1.Enabled = True
            Button4.Enabled = True
        End If
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        e.Handled = True

        If e.KeyCode = Keys.Enter And TextBox1.Text.Contains("ftp://") And TextBox1.Text.Length > 10 Then
            go()
        End If

        If e.KeyCode = Keys.Enter And TextBox1.Text.Contains("tatlinkmaker>") Then
            If cpath.Last <> "/" Then
                cpath = cpath & "/"
            End If

            tatlnkmkr.makeLinks(cpath & TextBox1.Text.Split(">")(1))
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Not syncstat.Contains("Syncing") Then
            If My.Settings.colorful Then
                Button12.BackColor = Color.SpringGreen
            End If

            ToolTip1.SetToolTip(Me.Button12, "Sync Complete")
        Else
            If My.Settings.colorful Then
                Button12.BackColor = Color.DeepSkyBlue
            End If

            ToolTip1.SetToolTip(Me.Button12, "Syncing")
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        If TextBox2.Text <> "" Then
            ListView1.Items.Clear()
            searchdone = False
            ProgressBar1.Style = ProgressBarStyle.Marquee

            Dim t1 As New Thread(AddressOf search)
            t1.Start(TextBox2.Text)
        End If
    End Sub

    Private Sub TextBox2_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox2.KeyUp
        e.Handled = True

        If TextBox2.Text <> "" And e.KeyCode = Keys.Enter Then
            searchdone = False
            ListView1.Items.Clear()

            ProgressBar1.Style = ProgressBarStyle.Marquee

            Timer3.Start()
            Dim t1 As New Thread(AddressOf search)
            t1.Start(TextBox2.Text)
        End If
    End Sub

    Private Sub search(ByVal str As String)
        Try
            Debug.Print("Searching for: " & str)
            searchmode = True
            searchres.Clear()

            Dim exitsearchitem As New ListViewItem("...", 2)
            exitsearchitem.SubItems.Add("End Search")
            searchres.Add(exitsearchitem)

            For Each f As PawFile In FTP.ListAllDirs(cpath)
                If f.ftpPath.Replace(cpath, "").ToLower.Contains(str.ToLower) Then
                    Try
                        Dim fname As String = f.ftpPath.Replace(cpath, "")
                        Dim ftype As String = "Folder"
                        Dim fsize As String = ""
                        Dim fdate As String = f.dt
                        Dim fimg As Integer = 0

                        If f.type = PawFile.Types.File Then
                            fimg = getImage(fname)
                            ftype = fname.Substring(fname.LastIndexOf("."))
                            fsize = getSize(f.size)
                        End If

                        Dim item As New ListViewItem(fname, fimg)

                        item.SubItems.Add(ftype)
                        item.SubItems.Add(fdate)
                        item.SubItems.Add(fsize)

                        If My.Settings.lockedPaths.Contains(f.ftpPath) Then
                            item.SubItems.Add("Locked")
                        Else
                            item.SubItems.Add("Unlocked")
                        End If

                        searchres.Add(item)
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
                End If
            Next
        Catch ex As Exception
            searchmode = False
            searchdone = True
            populateListView(cpath)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.SearchFailed)
            Exit Sub
        End Try

        searchdone = True
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox2.Text = "" Then
            searchmode = False
            populateListView(cpath)
            Button16.Enabled = False
        Else
            Button16.Enabled = True
        End If
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Application.DoEvents()

        If searchdone Then
            If searchres.Count > 1 Then
                Timer3.Stop()
                ProgressBar1.Style = ProgressBarStyle.Blocks
                populateSearch(searchres)
            Else
                Timer3.Stop()
                ProgressBar1.Style = ProgressBarStyle.Blocks
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.SearchNotFound)
                searchmode = False
                populateListView(cpath)
            End If
        End If
    End Sub
#End Region

#Region " CONTEXT MENU "
    Private Sub OpenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenToolStripMenuItem.Click
        open()
    End Sub

    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        If ListView1.SelectedItems.Count > 0 Then
            CopyCut(cpath, ListView1.SelectedItems)
        End If
    End Sub

    Private Sub CutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CutToolStripMenuItem.Click
        If ListView1.SelectedItems.Count > 0 Then
            CopyCut(cpath, ListView1.SelectedItems, ClipAction.Cut)
        End If
    End Sub

    Private Sub PasteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PasteToolStripMenuItem.Click
        Paste(cpath)
    End Sub

    Private Sub DownloadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadToolStripMenuItem.Click
        If ListView1.SelectedItems.Count > 0 Then
            If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
                Try
                    If cmenuitem.type = PawFile.Types.File Then
                        Dim dt As New Thread(Sub() FTP.DownloadFile(cmenuitem.ftpPath, FolderBrowserDialog1.SelectedPath & "\" & cmenuitem.ftpPath.Substring(cmenuitem.ftpPath.LastIndexOf("/") + 1)))
                        dt.Start()
                    Else
                        Dim dt As New Thread(Sub() FTP.DownloadFolder(cmenuitem.ftpPath, FolderBrowserDialog1.SelectedPath & "\" & cmenuitem.ftpPath.Substring(cmenuitem.ftpPath.LastIndexOf("/") + 1)))
                        dt.Start()
                    End If
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantDonwload)
                End Try

                Application.DoEvents()
            End If
        End If
    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameToolStripMenuItem.Click
        Try
            If cmenuitem.type = PawFile.Types.File Then
                Dim fl As String = InputBox("New File Name:", "Rename File On FTP Server", cmenuitem.ftpPath.Substring(cmenuitem.ftpPath.LastIndexOf("/") + 1))

                FTP.RenameFile(cmenuitem.ftpPath, fl)
            Else
                Dim fl As String = InputBox("New Folder Name:", "Rename Folder On FTP Server", cmenuitem.ftpPath.Substring(cmenuitem.ftpPath.LastIndexOf("/") + 1))

                FTP.RenameFile(cmenuitem.ftpPath, fl)
            End If

            populateListView(cpath)
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantRename)
        End Try
    End Sub

    Private Sub NewFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewFolderToolStripMenuItem.Click
        makeDir()
    End Sub

    Private Sub LockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LockToolStripMenuItem.Click
        If Not My.Settings.folpass.Equals("") Then
            For Each item As ListViewItem In ListView1.SelectedItems
                Try
                    Debug.Print("Locked: " & cpath & item.Text)
                    My.Settings.lockedPaths.Add(cpath & item.Text)
                    My.Settings.Save()
                    My.Settings.Reload()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantLock)
                End Try
            Next

            populateListView(cpath)
        Else
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.NoLockPasswordSet)
        End If
    End Sub

    Private Sub UnlockToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UnlockToolStripMenuItem.Click
        Dim ls As New List(Of String)

        For Each item As ListViewItem In ListView1.SelectedItems
            Try
                Debug.Print("Unlocked: " & cpath & item.Text)
                ls.Add(cpath & item.Text)
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantUnlock)
            End Try
        Next

        FolLock.unlockFolders(ls)
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteToolStripMenuItem.Click
        If cmenuitem.type = PawFile.Types.File Then
            FTP.DeleteFile(cmenuitem.ftpPath)
            populateListView(cpath)
        Else
            deleteDir(cmenuitem.ftpPath)
            populateListView(cpath)
        End If
    End Sub

    Private Sub ListView1_MouseDown(sender As Object, e As MouseEventArgs) Handles ListView1.MouseDown
        Try
            Dim lvi As ListViewItem = ListView1.GetItemAt(e.Location.X, e.Location.Y)

            If e.Button = MouseButtons.Right And lvi IsNot Nothing Then
                OpenToolStripMenuItem.Enabled = True
                OpenAsMenuItem.Enabled = True
                DeleteToolStripMenuItem.Enabled = True
                LockToolStripMenuItem.Enabled = True
                UnlockToolStripMenuItem.Enabled = True
                RenameToolStripMenuItem.Enabled = True
                DownloadToolStripMenuItem.Enabled = True
                MoveToParentFolderMenuItem.Enabled = True
                CutToolStripMenuItem.Enabled = True
                CopyToolStripMenuItem.Enabled = True

                Dim t As PawFile.Types = PawFile.Types.File
                Dim s As Boolean = False

                If lvi.SubItems(4).Text = "Locked" Then
                    s = True
                End If

                If lvi.SubItems(1).Text = "Folder" Then
                    t = PawFile.Types.Folder
                End If

                Dim item As New PawFile With {.dt = Convert.ToDateTime(lvi.SubItems(2).Text), .ftpPath = (cpath & lvi.Text), .type = t, .locked = s}
                cmenuitem = item

                If searchmode Then
                    NewFolderToolStripMenuItem.Enabled = False
                Else
                    NewFolderToolStripMenuItem.Enabled = True
                End If

                If item.locked Then
                    UnlockToolStripMenuItem.Enabled = True
                    LockToolStripMenuItem.Enabled = False
                    MoveToParentFolderMenuItem.Enabled = False
                Else
                    UnlockToolStripMenuItem.Enabled = False
                    LockToolStripMenuItem.Enabled = True
                End If
            Else
                OpenToolStripMenuItem.Enabled = False
                OpenAsMenuItem.Enabled = False
                DeleteToolStripMenuItem.Enabled = False
                LockToolStripMenuItem.Enabled = False
                UnlockToolStripMenuItem.Enabled = False
                RenameToolStripMenuItem.Enabled = False
                DownloadToolStripMenuItem.Enabled = False
                MoveToParentFolderMenuItem.Enabled = False
                CutToolStripMenuItem.Enabled = False
                CopyToolStripMenuItem.Enabled = False
            End If

            If clipboard.Count > 0 Then
                PasteToolStripMenuItem.Enabled = True
            Else
                PasteToolStripMenuItem.Enabled = False
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        FTP.errors = False
        Dim t1 As New Thread(AddressOf FTP.CheckConnection)
        t1.Start(My.Settings.path)
        t1.Join()

        If Not FTP.connected Then
            If FTP.Connect(My.Settings.path, My.Settings.username, My.Settings.password) Then
                prevcon = True
            End If
        End If

        If FTP.connected Then
            If My.Settings.colorful Then
                TextBox1.BackColor = Color.PaleGreen
            End If

            FTP.errors = True
        ElseIf prevcon = True And FTP.connected = False Then
            If My.Settings.colorful Then
                TextBox1.BackColor = Color.MistyRose
            End If

            prevcon = False
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.ConnectionLost)
        End If

        prevcon = FTP.connected
        Timer4.Stop()
    End Sub

    Private Sub OpenAsMenuItem_Click(sender As Object, e As EventArgs) Handles OpenAsMenuItem.Click
        openAs = True
        open()
    End Sub
#End Region

#Region " DRAG DROP "
    Private Sub ListView_ItemDrag(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemDragEventArgs) Handles ListView1.ItemDrag, ListView1.ItemDrag
        Try
            Dim myItems As New List(Of ListViewItem)

            For Each item As ListViewItem In ListView1.SelectedItems
                myItems.Add(item)
            Next

            sender.DoDragDrop(ListView1.SelectedItems, DragDropEffects.Move)
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub ListView_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragEnter, ListView1.DragEnter
        Try
            Dim i As Integer
            For i = 0 To e.Data.GetFormats().Length - 1
                If e.Data.GetFormats()(i).Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection") Then
                    e.Effect = DragDropEffects.Move
                    dragging = True
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Private Sub ListView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ListView1.DragDrop, ListView1.DragDrop
        Try
            'Return if the items are not selected in the ListView control.
            If ListView1.SelectedItems.Count = 0 Then Return
            'Returns the location of the mouse pointer in the ListView control.
            Dim p As Point = ListView1.PointToClient(New Point(e.X, e.Y))
            'Obtain the item that is located at the specified location of the mouse pointer.
            Dim dragToItem As ListViewItem = ListView1.GetItemAt(p.X, p.Y)
            If dragToItem Is Nothing Then Return
            'Obtain the index of the item at the mouse pointer.
            Dim dragIndex As Integer = dragToItem.Index
            Dim i As Integer
            Dim sel(ListView1.SelectedItems.Count) As ListViewItem
            For i = 0 To ListView1.SelectedItems.Count - 1
                sel(i) = ListView1.SelectedItems.Item(i)
            Next
            For i = 0 To ListView1.SelectedItems.Count - 1
                'Obtain the ListViewItem to be dragged to the target location.
                Dim dragItem As ListViewItem = sel(i)
                Dim itemIndex As Integer = dragIndex

                If itemIndex = dragItem.Index Then Return

                If dragItem.Index < itemIndex Then
                    itemIndex = itemIndex + 1
                Else
                    itemIndex = dragIndex + i
                End If

                If dragItem.SubItems(4).Text = "Locked" Then
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FirstUnlock, "File/Folder: " & dragItem.Text)
                    Continue For
                End If

                If dragToItem.Text = "..." Then
                    Dim newpath As String = cpath

                    If newpath.Last = "/" Then
                        newpath = newpath.Substring(0, newpath.Length - 2)
                    End If

                    newpath = newpath.Substring(0, newpath.LastIndexOf("/") + 1) & dragItem.Text

                    FTP.Move(cpath & dragItem.Text, newpath)

                    Return
                End If

                If dragToItem.SubItems(1).Text = "Folder" Then
                    FTP.Move(cpath & dragItem.Text, cpath & dragToItem.Text & "/" & dragItem.Text)
                Else
                    Return
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

        populateListView(cpath)
        dragging = False
    End Sub

    Private Sub MoveToParentFolderMenuItem_Click(sender As Object, e As EventArgs) Handles MoveToParentFolderMenuItem.Click
        Try
            For Each item As ListViewItem In ListView1.SelectedItems
                If item.Text <> "..." Then
                    If item.SubItems(4).Text = "Locked" Then
                        TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FirstUnlock, "File/Folder: " & item.Text)
                        Continue For
                    End If

                    Dim newpath As String = cpath

                    If newpath.Last = "/" Then
                        newpath = newpath.Substring(0, newpath.Length - 2)
                    End If

                    newpath = newpath.Substring(0, newpath.LastIndexOf("/") + 1) & item.Text

                    FTP.Move(cpath & item.Text, newpath)
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.CantMove)
        End Try

        populateListView(cpath)
    End Sub

    Private Sub ViewDetails_Click(sender As Object, e As EventArgs) Handles ViewDetails.Click
        ViewDetails.Checked = True
        ViewTiles.Checked = False
        ListView1.View = View.Details
    End Sub

    Private Sub ViewTiles_Click(sender As Object, e As EventArgs) Handles ViewTiles.Click
        ViewTiles.Checked = True
        ViewDetails.Checked = False
        ListView1.View = View.Tile
    End Sub

    Private Sub MainForm_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        If cleanUp() Then
            Application.Exit()
        End If
    End Sub

    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            Me.Hide()
            NotifyIcon1.Visible = True
        End If
    End Sub

    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        NotifyIcon1.Visible = False
        Show()
        Visible = True
        Me.Opacity = 100
        Focus()
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Application.DoEvents()

        If My.Settings.startmin Then
            Me.Opacity = 0
            Me.Visible = False
            Hide()
            NotifyIcon1.Visible = True
        Else
            NotifyIcon1.Visible = False
            Me.Visible = True
            Me.Opacity = 100
        End If
    End Sub

#End Region

End Class