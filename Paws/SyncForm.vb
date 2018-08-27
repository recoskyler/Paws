Public Class SyncForm
    Private Sub SyncForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Settings.Save()
        My.Settings.Reload()

        refreshLists()
    End Sub

    Public Sub refreshLists()
        Try
            TextBox1.Text = ""
            TextBox2.Text = ""

            ListView1.Items.Clear()

            For Each folder As String In My.Settings.synced
                Dim item As New ListViewItem(folder)
                item.SubItems.Add(My.Settings.syncPaths(My.Settings.synced.IndexOf(folder)))
                ListView1.Items.Add(item)
            Next

            Button1.Enabled = False
            Button2.Enabled = False

            If MainForm.syncstat.Contains("Syncing") Then
                Button4.Enabled = False
            Else
                Button4.Enabled = True
            End If

            Label3.Text = MainForm.syncstat
            CheckBox1.Checked = My.Settings.secure
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAccessSettings)
        End Try

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text.Length > 0 And TextBox2.Text.Length > 0 Then
            If IO.Directory.Exists(TextBox1.Text) And Not My.Settings.synced.Contains(TextBox1.Text) And Not My.Settings.syncPaths.Contains(TextBox2.Text) Then
                Button1.Enabled = True
            Else
                Button1.Enabled = False
            End If
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            My.Settings.syncPaths.Add(TextBox2.Text)
            My.Settings.synced.Add(TextBox1.Text)

            My.Settings.Save()
            My.Settings.Reload()

            refreshLists()
        Catch ex As Exception
            TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToAddFolder)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            TextBox1.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Not MainForm.syncstat.Contains("Syncing") Then
            Dim t1 As New Threading.Thread(AddressOf MainForm.sync)
            t1.Start()
            MainForm.Button12.BackColor = Color.DeepSkyBlue
            MainForm.ToolTip1.SetToolTip(MainForm.Button12, "Syncing...")
            Label3.Text = "Sync Status: Syncing"
            Button4.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ListView1.SelectedItems.Count > 0 Then
            For Each item As ListViewItem In ListView1.SelectedItems
                Try
                    My.Settings.synced.Remove(item.Text)
                    My.Settings.syncPaths.Remove(item.SubItems(1).Text)

                    My.Settings.Save()
                    My.Settings.Reload()

                    refreshLists()
                Catch ex As Exception
                    TheMightyErrorMaster.ShowError(TheMightyErrorMaster.ErrorCode.UnableToRemoveFolder)
                End Try
            Next
        End If
    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        If TextBox1.Text.Length > 0 And TextBox2.Text.Length > 0 Then
            If IO.Directory.Exists(TextBox1.Text) And Not My.Settings.synced.Contains(TextBox1.Text) And Not My.Settings.syncPaths.Contains(TextBox2.Text) Then
                Button1.Enabled = True
            Else
                Button1.Enabled = False
            End If
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
        If ListView1.SelectedItems.Count > 0 Then
            Button2.Enabled = True
        Else
            Button2.Enabled = False
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        My.Settings.secure = CheckBox1.Checked
        My.Settings.Save()
        My.Settings.Reload()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            TextBox2.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
End Class