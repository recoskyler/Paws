Public Class NIcontext
    Private Sub NIcontext_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()
    End Sub

    Private Sub NIcontext_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ContextMenuStrip1.Show(Cursor.Position) 'Shows the Right click menu on the cursor position
        Me.Left = ContextMenuStrip1.Left + 1 'Puts the form behind the menu horizontally
        Me.Top = ContextMenuStrip1.Top + 1 'Puts the form behind the menu vertically
    End Sub

    Private Sub OpenPawsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenPawsToolStripMenuItem.Click
        MainForm.Show()
        Me.Hide()
        MainForm.NotifyIcon1.Visible = False
    End Sub

    Private Sub ExitPawsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitPawsToolStripMenuItem.Click
        If MainForm.syncstat.Contains("Syncing") Then
            MessageBox.Show("Please wait until sync is complete, or you may lose your files.", "Paws Warning", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Me.Hide()
        MainForm.NotifyIcon1.Visible = False
        Application.DoEvents()
        MainForm.cleanUp()
        FTP.Disconnect()
        Application.ExitThread()
        Application.Exit()
    End Sub
End Class