<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NIcontext
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenPawsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitPawsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenPawsToolStripMenuItem, Me.ExitPawsToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(153, 70)
        '
        'OpenPawsToolStripMenuItem
        '
        Me.OpenPawsToolStripMenuItem.Name = "OpenPawsToolStripMenuItem"
        Me.OpenPawsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.OpenPawsToolStripMenuItem.Text = "Open Paws"
        '
        'ExitPawsToolStripMenuItem
        '
        Me.ExitPawsToolStripMenuItem.Name = "ExitPawsToolStripMenuItem"
        Me.ExitPawsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.ExitPawsToolStripMenuItem.Text = "Exit Paws"
        '
        'NIcontext
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "NIcontext"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "NIcontext"
        Me.TopMost = True
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents OpenPawsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitPawsToolStripMenuItem As ToolStripMenuItem
End Class
