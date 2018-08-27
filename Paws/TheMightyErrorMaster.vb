Public Class TheMightyErrorMaster
    Shared errorList As New List(Of PawsError)

    Public Enum ErrorCode
        Unidentified = 0
        CantConnect = 1
        ConnectionLost = 2
        InvalidCredentials = 3
        SyncError = 4
        FolderNotFound = 5
        FileNotFound = 6
        CantMakeDir = 7
        CantUpload = 8
        CantDonwload = 9
        CantDelete = 10
        AdminRequired = 11
        CantLock = 12
        CantUnlock = 13
        InvalidURL = 14
        CantRename = 15
        SearchNotFound = 16
        SearchFailed = 17
        UnableToChangeSetting = 18
        UnableToAddFolder = 19
        UnableToRemoveFolder = 20
        FileAccessDenied = 21
        FolderAccessDenied = 22
        UnableToMakeLink = 23
        NoInternet = 24
        NoURLSet = 25
        SyncInProgress = 26
        NoLockPasswordSet = 27
        UnableToAccessSettings = 28
        PasswordNotSecure = 29
        FormatAlreadyExists = 30
        UnableToReadRegistryValue = 31
        VLCNotFound = 32
        UnableToSetRegistryValue = 33
        UnableToAddFormat = 34
        UnableToRemoveFormat = 35
        CantMove = 36
        FirstUnlock = 37
        CantCopy = 38
        CantCut = 39
        CantPaste = 40
        AccessDenied = 41
        AskReplace = 42
        AskDelete = 43
    End Enum

    Public Class PawsError
        Public Property title As String = "Paws Error"
        Public Property text As String = "Dang! Believe me or not, but an error occured!"
        Public Property code As ErrorCode = ErrorCode.Unidentified
        Public Property icon As MessageBoxIcon = MessageBoxIcon.Error
        Public Property buttons As MessageBoxButtons = MessageBoxButtons.OK
        Public Property defaultButton As MessageBoxDefaultButton = MessageBoxDefaultButton.Button1
    End Class

    Shared Sub Initialize()
        Try
            errorList.Add(New PawsError With {.title = "Access Denied", .text = "Access to the FTP file/folder denied.", .code = ErrorCode.AccessDenied})
            errorList.Add(New PawsError With {.title = "Connection Failure", .text = "Can't connect to the server.", .code = ErrorCode.CantConnect})
            errorList.Add(New PawsError With {.title = "Connection Lost", .text = "Connection lost between computer and the server.", .code = ErrorCode.ConnectionLost})
            errorList.Add(New PawsError With {.title = "Incorrect Credentials", .text = "Your login credentials are incorrect. Please check them, and try again by clicking Refresh.", .code = ErrorCode.InvalidCredentials})
            errorList.Add(New PawsError With {.title = "Sync Failed", .text = "Paw Sync failed to sync.", .code = ErrorCode.SyncError})
            errorList.Add(New PawsError With {.title = "Folder IO Error", .text = "Folder not found.", .code = ErrorCode.FolderNotFound})
            errorList.Add(New PawsError With {.title = "File IO Error", .text = "File not found.", .code = ErrorCode.FileNotFound})
            errorList.Add(New PawsError With {.title = "Folder IO Error", .text = "Unable to create new folder.", .code = ErrorCode.CantMakeDir})
            errorList.Add(New PawsError With {.title = "Upload Failed", .text = "Unable to upload file/folder.", .code = ErrorCode.CantUpload})
            errorList.Add(New PawsError With {.title = "Download Failed", .text = "Unable to download file/folder.", .code = ErrorCode.CantDonwload})
            errorList.Add(New PawsError With {.title = "Delete Failed", .text = "Unable to delete file/folder.", .code = ErrorCode.CantDelete})
            errorList.Add(New PawsError With {.title = "Admin Privilages Required", .text = "You should run Paws as administrator in order to perform this action/change this setting.", .code = ErrorCode.AdminRequired})
            errorList.Add(New PawsError With {.title = "Lock Failed", .text = "Unable to lock folder.", .code = ErrorCode.CantLock})
            errorList.Add(New PawsError With {.title = "Unlock Failed", .text = "Unable to unlock folder.", .code = ErrorCode.CantUnlock})
            errorList.Add(New PawsError With {.title = "Invalid URL", .text = "The URL you've entered is invalid, or can't view/open the URL.", .code = ErrorCode.InvalidURL})
            errorList.Add(New PawsError With {.title = "Rename Failed", .text = "Unable to rename file/folder.", .code = ErrorCode.CantRename})
            errorList.Add(New PawsError With {.title = "No Result", .text = "Search did not return any results.", .code = ErrorCode.SearchNotFound, .icon = MessageBoxIcon.Information})
            errorList.Add(New PawsError With {.title = "Search Failed", .text = "Could not perform search here.", .code = ErrorCode.SearchFailed})
            errorList.Add(New PawsError With {.title = "Setting Error", .text = "Unable to save changes to the settings.", .code = ErrorCode.UnableToChangeSetting})
            errorList.Add(New PawsError With {.title = "Add Failed", .text = "Unable to add file/folder.", .code = ErrorCode.UnableToAddFolder})
            errorList.Add(New PawsError With {.title = "Remove Failed", .text = "Unable to remove file/folder.", .code = ErrorCode.UnableToRemoveFolder})
            errorList.Add(New PawsError With {.title = "Access Denied", .text = "Access to the file denied.", .code = ErrorCode.FileAccessDenied})
            errorList.Add(New PawsError With {.title = "Access Denied", .text = "Access to the folder denied.", .code = ErrorCode.FolderAccessDenied})
            errorList.Add(New PawsError With {.title = "Link Error", .text = "Unable to make/add link to the file.", .code = ErrorCode.UnableToMakeLink})
            errorList.Add(New PawsError With {.title = "No Internet", .text = "There is no internet connection. Please try again later.", .code = ErrorCode.NoInternet})
            errorList.Add(New PawsError With {.title = "No Default URL", .text = "You haven't set a default URL yet. Please set one from Settings.", .code = ErrorCode.NoURLSet, .icon = MessageBoxIcon.Information})
            errorList.Add(New PawsError With {.title = "Sync In Progress", .text = "Sorry but Paw Sync is in progress, and it's dangerous to stop it suddenly as it might break your files. Please wait until Paw Sync icon turns green. It may take a minute.", .code = ErrorCode.SyncInProgress, .icon = MessageBoxIcon.Exclamation})
            errorList.Add(New PawsError With {.title = "No Password Set", .text = "You haven't set a password for locked files/folders. Please set one from Settings, before trying to lock again.", .code = ErrorCode.NoLockPasswordSet, .icon = MessageBoxIcon.Exclamation})
            errorList.Add(New PawsError With {.title = "Unable To Read Settings", .text = "Unable to read/access setting/config file.", .code = ErrorCode.UnableToAccessSettings})
            errorList.Add(New PawsError With {.title = "Password Not Strong Enough", .text = "Your password must be at least 6 characters long.", .code = ErrorCode.PasswordNotSecure})
            errorList.Add(New PawsError With {.title = "Format Already Exists", .text = "Format, that you are trying to add, already exists.", .code = ErrorCode.FormatAlreadyExists})
            errorList.Add(New PawsError With {.title = "Unable To Read Registry", .text = "Paws couldn't read a registry value.", .code = ErrorCode.UnableToReadRegistryValue})
            errorList.Add(New PawsError With {.title = "VLC Not Installed", .text = "VLC Player is not installed on your system. (Or Paws could not find it.) Install VLC Player for Paws Auto-Stream support.", .code = ErrorCode.VLCNotFound, .icon = MessageBoxIcon.Information})
            errorList.Add(New PawsError With {.title = "Unable To Set Registry", .text = "Paws couldn't set a registry value. Please run Paws as administrator, and try again.", .code = ErrorCode.UnableToSetRegistryValue})
            errorList.Add(New PawsError With {.title = "Unable To Add Format", .text = "Couldn't add this format to the list.", .code = ErrorCode.UnableToAddFormat})
            errorList.Add(New PawsError With {.title = "Unable To Remove Format", .text = "Couldn't remove this format from the list.", .code = ErrorCode.UnableToRemoveFormat})
            errorList.Add(New PawsError With {.title = "Move Failed", .text = "Couldn't move file/folder (s).", .code = ErrorCode.CantMove})
            errorList.Add(New PawsError With {.title = "Locked", .text = "You must first unlock this file/folder to perform this action.", .code = ErrorCode.FirstUnlock})
            errorList.Add(New PawsError With {.title = "Copy Error", .text = "Paws couldn't copy this/these item(s).", .code = ErrorCode.CantCopy})
            errorList.Add(New PawsError With {.title = "Cut Error", .text = "Paws couldn't cut this/these item(s).", .code = ErrorCode.CantCut})
            errorList.Add(New PawsError With {.title = "Paste Error", .text = "Paws couldn't paste this/these item(s).", .code = ErrorCode.CantPaste})
            errorList.Add(New PawsError With {.title = "Already Exists", .text = "A file/folder with the same name already exists. Would you like to replace? Paws will pass this item if you answer No.", .code = ErrorCode.AskReplace, .buttons = MessageBoxButtons.YesNo, .icon = MessageBoxIcon.Question})
            errorList.Add(New PawsError With {.title = "Delete", .text = "Are you sure you want to delete this/these item(s) permannently?", .code = ErrorCode.AskDelete, .buttons = MessageBoxButtons.YesNo, .icon = MessageBoxIcon.Question})
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Sub

    Shared Function ShowError(ByVal err As PawsError) As DialogResult
        Try
            Return MessageBox.Show(err.text, err.title, err.buttons, err.icon, err.defaultButton)
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

        Return DialogResult.Cancel
    End Function

    Shared Function ShowError(ByVal errCode As ErrorCode, Optional ByVal extraMessage As String = "") As DialogResult
        Try
            For Each e As PawsError In errorList
                If e.code = errCode Then
                    Return MessageBox.Show(e.text & " " & extraMessage, e.title, e.buttons, e.icon, e.defaultButton)
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

        Return DialogResult.Cancel
    End Function
End Class