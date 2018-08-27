Imports System.ComponentModel

Public Class SettingForm
    Dim vformats As String() = {".mp4", ".wmv", ".mkv", ".flv", ".avi", ".webm"}
    Dim pformats As String() = {".jpg", ".jpeg", ".png", ".ico", ".gif", ".tif", ".tiff", ".cr2", ".jpg-large"}
    Dim aformats As String() = {".mp3", ".wma", ".wav", ".aac", ".ogg", ".ac3", ".flac"}

    Public Sub refreshSettings()
        Try
            My.Settings.Save()
            My.Settings.Reload()

            TextBox1.Text = My.Settings.path
            TextBox2.Text = My.Settings.username
            TextBox3.Text = My.Settings.password
            TextBox4.Text = ""
            TextBox5.Text = ""
            TextBox6.Text = My.Settings.folpass
            CheckBox1.Checked = My.Settings.startmin

            If MainForm.ListView1.View = View.Tile Then
                ComboBox1.SelectedItem = "Tiles"
            ElseIf MainForm.ListView1.View = View.Details Then
                ComboBox1.SelectedItem = "Details"
            End If

            If My.Settings.colorful Then
                ComboBox4.SelectedItem = "Colorful"
            Else
                ComboBox4.SelectedItem = "Default"
            End If

            ComboBox2.Items.Clear()
            ComboBox3.Items.Clear()

            If My.Settings.vformats.Count = 0 Then
                My.Settings.vformats.AddRange(vformats)
            End If

            If My.Settings.pformats.Count = 0 Then
                My.Settings.pformats.AddRange(pformats)
            End If

            If My.Settings.aformats.Count = 0 Then
                My.Settings.aformats.AddRange(aformats)
            End If

            My.Settings.Save()
            My.Settings.Reload()

            For Each f As String In My.Settings.vformats
                ComboBox2.Items.Add(f)
            Next

            For Each f As String In My.Settings.pformats
                ComboBox3.Items.Add(f)
            Next

            For Each f As String In My.Settings.aformats
                ComboBox5.Items.Add(f)
            Next

            If My.Settings.vlcauto Then
                Button7.Text = "Current Mode: Auto-Find VLC"
            Else
                Button7.Text = "Current Mode: Set VLC Path Manually"
            End If

            TextBox10.Text = My.Settings.vlcPath
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAccessSettings)
        End Try
    End Sub

    Private Sub SettingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        refreshSettings()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Try
            If ComboBox1.SelectedItem.Equals("Tiles") Then
                My.Settings.view = 0
                MainForm.ListView1.View = View.Tile
            ElseIf ComboBox1.SelectedItem.Equals("Details") Then
                My.Settings.view = 1
                MainForm.ListView1.View = View.Details
            End If

            My.Settings.Save()
            My.Settings.Reload()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ComboBox2.SelectedIndex >= 0 And My.Settings.vformats.Contains(ComboBox2.SelectedItem) Then
            Try
                My.Settings.vformats.Remove(ComboBox2.SelectedItem)
                My.Settings.Save()
                My.Settings.Reload()

                refreshSettings()
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToRemoveFormat)
            End Try
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex >= 0 Then
            Button3.Enabled = True
        Else
            Button3.Enabled = False
        End If
    End Sub

    Private Sub TextBox4_TextChanged(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        If TextBox4.Text.Equals("") Then
            Button2.Enabled = False
        Else
            Button2.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not TextBox4.Text.Equals("") Then
            If Not TextBox4.Text.First = "." Then
                TextBox4.Text = "." & TextBox4.Text
            End If

            If Not My.Settings.vformats.Contains(TextBox4.Text) Then
                Try
                    My.Settings.vformats.Add(TextBox4.Text)
                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshSettings()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFormat)
                End Try
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FormatAlreadyExists)
            End If
        End If
    End Sub

    Private Sub TextBox4_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox4.KeyUp
        If e.KeyCode = Keys.Enter And Not TextBox4.Text.Equals("") Then
            If Not TextBox4.Text.First.Equals(".") Then
                TextBox4.Text = "." & TextBox4.Text
            End If

            If Not My.Settings.vformats.Contains(TextBox4.Text) Then
                Try
                    My.Settings.vformats.Add(TextBox4.Text)
                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshSettings()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFormat)
                End Try
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FormatAlreadyExists)
            End If
        End If
    End Sub

    Private Sub TextBox5_KeyUp(sender As Object, e As KeyEventArgs) Handles TextBox5.KeyUp
        If e.KeyCode = Keys.Enter And Not TextBox5.Text.Equals("") Then
            If Not TextBox5.Text.First.Equals(".") Then
                TextBox5.Text = "." & TextBox4.Text
            End If

            If Not My.Settings.pformats.Contains(TextBox5.Text) Then
                Try
                    My.Settings.pformats.Add(TextBox5.Text)
                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshSettings()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFormat)
                End Try
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FormatAlreadyExists)
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ComboBox3.SelectedIndex >= 0 And My.Settings.pformats.Contains(ComboBox3.SelectedItem) Then
            Try
                My.Settings.pformats.Remove(ComboBox3.SelectedItem)
                My.Settings.Save()
                My.Settings.Reload()

                refreshSettings()
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToRemoveFormat)
            End Try
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If Not TextBox5.Text.Equals("") Then
            If Not TextBox5.Text.First = "." Then
                TextBox5.Text = "." & TextBox5.Text
            End If

            If Not My.Settings.pformats.Contains(TextBox5.Text) Then
                Try
                    My.Settings.pformats.Add(TextBox5.Text)
                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshSettings()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFolder)
                End Try
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FormatAlreadyExists)
            End If
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        If Not TextBox6.Text.Equals("") And TextBox6.Text.Length >= 8 Then
            My.Settings.folpass = TextBox6.Text
            My.Settings.Save()
            My.Settings.Reload()
        Else
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.PasswordNotSecure)
        End If
    End Sub

    Private Sub TextBox5_TextChanged(sender As Object, e As EventArgs) Handles TextBox5.TextChanged
        If TextBox5.Text <> "" Then
            Button5.Enabled = True
        Else
            Button5.Enabled = False
        End If
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedIndex >= 0 Then
            Button4.Enabled = True
        Else
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click
        About.Show()
    End Sub

    Private Sub SettingForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Try
            My.Settings.username = TextBox2.Text
            My.Settings.password = TextBox3.Text

            If TextBox6.Text.Length >= 6 Then
                My.Settings.folpass = TextBox6.Text
            ElseIf TextBox6.Text.Length > 0 Then
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.PasswordNotSecure)
                Exit Sub
            End If

            If TextBox1.Text <> "" Then
                If Not TextBox1.Text.Substring(0, 6).Equals("ftp://") Then
                    TextBox1.Text = "ftp://" & TextBox1.Text
                End If

                If TextBox1.Text.Last <> "/" Then
                    TextBox1.Text = TextBox1.Text & "/"
                End If

                My.Settings.path = TextBox1.Text
            End If

            My.Settings.Save()
            My.Settings.Reload()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
        End Try
    End Sub

    Private Sub TextBox10_TextChanged(sender As Object, e As EventArgs) Handles TextBox10.TextChanged
        If TextBox10.Text.Length > 8 And IO.File.Exists(TextBox10.Text) Then
            Button1.Enabled = True
        Else
            Button1.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            My.Settings.vlcauto = False
            My.Settings.vlcPath = TextBox10.Text
            My.Settings.Save()
            My.Settings.Reload()

            refreshSettings()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            My.Settings.vlcauto = Not My.Settings.vlcauto
            My.Settings.Save()
            My.Settings.Reload()

            If My.Settings.vlcauto Then
                My.Settings.vlcPath = MainForm.isVLCinstalled()
            End If

            My.Settings.Save()
            My.Settings.Reload()

            refreshSettings()

            If My.Settings.vlcPath = "" Or Not IO.File.Exists(My.Settings.vlcPath) Then
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.VLCNotFound)
            End If
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
        End Try
    End Sub

    Private Sub TextBox10_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox10.KeyDown
        If e.KeyCode = Keys.Enter And Button1.Enabled Then
            Try
                My.Settings.vlcauto = False
                My.Settings.vlcPath = TextBox10.Text
                My.Settings.Save()
                My.Settings.Reload()

                refreshSettings()
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
            End Try
        End If
    End Sub

    Private Sub ComboBox4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox4.SelectedIndexChanged
        Try
            If ComboBox4.SelectedItem = "Default" Then
                My.Settings.colorful = False
            Else
                My.Settings.colorful = True
            End If

            My.Settings.Save()
            My.Settings.Reload()

            MainForm.refreshMain()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToChangeSetting)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If Not TextBox11.Text.Equals("") Then
            If Not TextBox11.Text.First = "." Then
                TextBox11.Text = "." & TextBox11.Text
            End If

            If Not My.Settings.aformats.Contains(TextBox11.Text) Then
                Try
                    My.Settings.aformats.Add(TextBox11.Text)
                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshSettings()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFormat)
                End Try
            Else
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.FormatAlreadyExists)
            End If
        End If
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If ComboBox5.SelectedIndex >= 0 And My.Settings.aformats.Contains(ComboBox5.SelectedItem) Then
            Try
                My.Settings.aformats.Remove(ComboBox5.SelectedItem)
                My.Settings.Save()
                My.Settings.Reload()

                refreshSettings()
            Catch ex As Exception
                TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToRemoveFormat)
            End Try
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        My.Settings.startmin = CheckBox1.Checked
        My.Settings.Save()
        My.Settings.Reload()
    End Sub
End Class