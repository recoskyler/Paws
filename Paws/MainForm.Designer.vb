<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenAsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DownloadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewFolderToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PasteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LockToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnlockToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveToParentFolderMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Button16 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.Button6 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.SelectedItemsLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.SizeLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LastModifiedLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.SuccessfulLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.FailedLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ChangeViewButton = New System.Windows.Forms.ToolStripSplitButton()
        Me.ViewDetails = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewTiles = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer2 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer3 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer4 = New System.Windows.Forms.Timer(Me.components)
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.DetailsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TilesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.imageList1.Images.SetKeyName(0, "folder.png")
        Me.imageList1.Images.SetKeyName(1, "file.png")
        Me.imageList1.Images.SetKeyName(2, "back.png")
        Me.imageList1.Images.SetKeyName(3, "PDF.png")
        Me.imageList1.Images.SetKeyName(4, "PPT.png")
        Me.imageList1.Images.SetKeyName(5, "DOCX.png")
        Me.imageList1.Images.SetKeyName(6, "DOC.png")
        Me.imageList1.Images.SetKeyName(7, "XLSX.png")
        Me.imageList1.Images.SetKeyName(8, "EXE.png")
        Me.imageList1.Images.SetKeyName(9, "iso.png")
        Me.imageList1.Images.SetKeyName(10, "tiff.png")
        Me.imageList1.Images.SetKeyName(11, "gif.png")
        Me.imageList1.Images.SetKeyName(12, "TXT.png")
        Me.imageList1.Images.SetKeyName(13, "MP3.png")
        Me.imageList1.Images.SetKeyName(14, "zip.png")
        Me.imageList1.Images.SetKeyName(15, "JPG.png")
        Me.imageList1.Images.SetKeyName(16, "XLS.png")
        Me.imageList1.Images.SetKeyName(17, "RAR.png")
        Me.imageList1.Images.SetKeyName(18, "BMP.png")
        Me.imageList1.Images.SetKeyName(19, "AAC.png")
        Me.imageList1.Images.SetKeyName(20, "PNG.png")
        Me.imageList1.Images.SetKeyName(21, "FLAC.png")
        Me.imageList1.Images.SetKeyName(22, "MP4.png")
        Me.imageList1.Images.SetKeyName(23, "MPG.png")
        Me.imageList1.Images.SetKeyName(24, "MKV.png")
        Me.imageList1.Images.SetKeyName(25, "SVG.png")
        Me.imageList1.Images.SetKeyName(26, "avi.png")
        Me.imageList1.Images.SetKeyName(27, "SUBSRT.png")
        Me.imageList1.Images.SetKeyName(28, "HTML.png")
        Me.imageList1.Images.SetKeyName(29, "SWF.png")
        Me.imageList1.Images.SetKeyName(30, "JAVA.png")
        Me.imageList1.Images.SetKeyName(31, "JS.png")
        Me.imageList1.Images.SetKeyName(32, "msi.png")
        Me.imageList1.Images.SetKeyName(33, "webm.png")
        '
        'ListView1
        '
        Me.ListView1.Alignment = System.Windows.Forms.ListViewAlignment.[Default]
        Me.ListView1.AllowDrop = True
        Me.ListView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.Cursor = System.Windows.Forms.Cursors.Default
        Me.ListView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListView1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.ListView1.HideSelection = False
        Me.ListView1.LargeImageList = Me.imageList1
        Me.ListView1.Location = New System.Drawing.Point(0, 60)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(1268, 660)
        Me.ListView1.SmallImageList = Me.imageList1
        Me.ListView1.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.ListView1.StateImageList = Me.imageList1
        Me.ListView1.TabIndex = 0
        Me.ListView1.TileSize = New System.Drawing.Size(300, 50)
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Name"
        Me.ColumnHeader1.Width = 500
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Type"
        Me.ColumnHeader2.Width = 200
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Last Modified"
        Me.ColumnHeader3.Width = 315
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Size"
        Me.ColumnHeader4.Width = 121
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Status"
        Me.ColumnHeader5.Width = 95
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.OpenAsMenuItem, Me.DownloadToolStripMenuItem, Me.RenameToolStripMenuItem, Me.NewFolderToolStripMenuItem, Me.CopyToolStripMenuItem, Me.CutToolStripMenuItem, Me.PasteToolStripMenuItem, Me.LockToolStripMenuItem, Me.UnlockToolStripMenuItem, Me.MoveToParentFolderMenuItem, Me.DeleteToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(190, 268)
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'OpenAsMenuItem
        '
        Me.OpenAsMenuItem.Name = "OpenAsMenuItem"
        Me.OpenAsMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.OpenAsMenuItem.Text = "Open (Browser)"
        '
        'DownloadToolStripMenuItem
        '
        Me.DownloadToolStripMenuItem.Name = "DownloadToolStripMenuItem"
        Me.DownloadToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.DownloadToolStripMenuItem.Text = "Download"
        '
        'RenameToolStripMenuItem
        '
        Me.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem"
        Me.RenameToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.RenameToolStripMenuItem.Text = "Rename"
        '
        'NewFolderToolStripMenuItem
        '
        Me.NewFolderToolStripMenuItem.Name = "NewFolderToolStripMenuItem"
        Me.NewFolderToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.NewFolderToolStripMenuItem.Text = "New Folder"
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CopyToolStripMenuItem.Text = "Copy"
        '
        'CutToolStripMenuItem
        '
        Me.CutToolStripMenuItem.Name = "CutToolStripMenuItem"
        Me.CutToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.CutToolStripMenuItem.Text = "Cut"
        '
        'PasteToolStripMenuItem
        '
        Me.PasteToolStripMenuItem.Name = "PasteToolStripMenuItem"
        Me.PasteToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.PasteToolStripMenuItem.Text = "Paste"
        '
        'LockToolStripMenuItem
        '
        Me.LockToolStripMenuItem.Name = "LockToolStripMenuItem"
        Me.LockToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.LockToolStripMenuItem.Text = "Lock"
        '
        'UnlockToolStripMenuItem
        '
        Me.UnlockToolStripMenuItem.Name = "UnlockToolStripMenuItem"
        Me.UnlockToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.UnlockToolStripMenuItem.Text = "Unlock"
        '
        'MoveToParentFolderMenuItem
        '
        Me.MoveToParentFolderMenuItem.Name = "MoveToParentFolderMenuItem"
        Me.MoveToParentFolderMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.MoveToParentFolderMenuItem.Text = "Move to parent folder"
        '
        'DeleteToolStripMenuItem
        '
        Me.DeleteToolStripMenuItem.Name = "DeleteToolStripMenuItem"
        Me.DeleteToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.DeleteToolStripMenuItem.Text = "Delete"
        '
        'TextBox1
        '
        Me.TextBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.TextBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList
        Me.TextBox1.BackColor = System.Drawing.Color.White
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(0, 0)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(1268, 19)
        Me.TextBox1.TabIndex = 1
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Multiselect = True
        Me.OpenFileDialog1.Title = "Copy Files To FTP Server"
        '
        'Panel1
        '
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(1, 1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1268, 743)
        Me.Panel1.TabIndex = 22
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.Controls.Add(Me.ListView1)
        Me.Panel4.Controls.Add(Me.Panel2)
        Me.Panel4.Controls.Add(Me.TextBox1)
        Me.Panel4.Controls.Add(Me.StatusStrip1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel4.Location = New System.Drawing.Point(0, 0)
        Me.Panel4.Margin = New System.Windows.Forms.Padding(5)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1268, 743)
        Me.Panel4.TabIndex = 24
        '
        'Panel2
        '
        Me.Panel2.AutoScroll = True
        Me.Panel2.AutoSize = True
        Me.Panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.Controls.Add(Me.Button16)
        Me.Panel2.Controls.Add(Me.TextBox2)
        Me.Panel2.Controls.Add(Me.Button12)
        Me.Panel2.Controls.Add(Me.Button11)
        Me.Panel2.Controls.Add(Me.Button10)
        Me.Panel2.Controls.Add(Me.Button6)
        Me.Panel2.Controls.Add(Me.Button9)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Controls.Add(Me.Button3)
        Me.Panel2.Controls.Add(Me.Button5)
        Me.Panel2.Controls.Add(Me.Button8)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.Button7)
        Me.Panel2.Controls.Add(Me.Button4)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel2.Location = New System.Drawing.Point(0, 19)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1268, 41)
        Me.Panel2.TabIndex = 1
        '
        'Button16
        '
        Me.Button16.BackgroundImage = Global.Paws.My.Resources.Resources.if_search_stroked_293645
        Me.Button16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button16.FlatAppearance.BorderSize = 0
        Me.Button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button16.Location = New System.Drawing.Point(645, 11)
        Me.Button16.Name = "Button16"
        Me.Button16.Size = New System.Drawing.Size(20, 20)
        Me.Button16.TabIndex = 28
        Me.Button16.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(504, 11)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(138, 20)
        Me.TextBox2.TabIndex = 26
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Button12.BackgroundImage = Global.Paws.My.Resources.Resources.if_52_Cloud_Sync_183362
        Me.Button12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button12.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button12.FlatAppearance.BorderSize = 0
        Me.Button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button12.Location = New System.Drawing.Point(463, 3)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(35, 35)
        Me.Button12.TabIndex = 25
        Me.ToolTip1.SetToolTip(Me.Button12, "Sync Complete")
        Me.Button12.UseVisualStyleBackColor = False
        '
        'Button11
        '
        Me.Button11.BackgroundImage = Global.Paws.My.Resources.Resources.unlock
        Me.Button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button11.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button11.FlatAppearance.BorderSize = 0
        Me.Button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button11.Location = New System.Drawing.Point(332, 3)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(35, 35)
        Me.Button11.TabIndex = 24
        Me.ToolTip1.SetToolTip(Me.Button11, "Unlock Folder")
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.BackgroundImage = Global.Paws.My.Resources.Resources.if_lock_24_103178
        Me.Button10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button10.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button10.FlatAppearance.BorderSize = 0
        Me.Button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button10.Location = New System.Drawing.Point(291, 3)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(35, 35)
        Me.Button10.TabIndex = 23
        Me.ToolTip1.SetToolTip(Me.Button10, "Lock Folder")
        Me.Button10.UseVisualStyleBackColor = True
        '
        'Button6
        '
        Me.Button6.BackgroundImage = Global.Paws.My.Resources.Resources._1497117509_delete
        Me.Button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button6.FlatAppearance.BorderSize = 0
        Me.Button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button6.Location = New System.Drawing.Point(45, 3)
        Me.Button6.Name = "Button6"
        Me.Button6.Size = New System.Drawing.Size(35, 35)
        Me.Button6.TabIndex = 12
        Me.ToolTip1.SetToolTip(Me.Button6, "Delete")
        Me.Button6.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.BackgroundImage = Global.Paws.My.Resources.Resources._1497476362_text
        Me.Button9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button9.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button9.FlatAppearance.BorderSize = 0
        Me.Button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button9.Image = Global.Paws.My.Resources.Resources._1497053367_Update
        Me.Button9.Location = New System.Drawing.Point(168, 3)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(35, 35)
        Me.Button9.TabIndex = 21
        Me.ToolTip1.SetToolTip(Me.Button9, "Rename File/Folder")
        Me.Button9.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.BackgroundImage = Global.Paws.My.Resources.Resources._1497053367_Update
        Me.Button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Image = Global.Paws.My.Resources.Resources._1497053367_Update
        Me.Button2.Location = New System.Drawing.Point(209, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(35, 35)
        Me.Button2.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Button2, "Refresh and Sync")
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.BackgroundImage = Global.Paws.My.Resources.Resources._1497053379_Play
        Me.Button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button3.FlatAppearance.BorderSize = 0
        Me.Button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button3.Location = New System.Drawing.Point(381, 3)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(35, 35)
        Me.Button3.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.Button3, "Go")
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.BackgroundImage = Global.Paws.My.Resources.Resources._1497053373_Settings
        Me.Button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Location = New System.Drawing.Point(422, 3)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(35, 35)
        Me.Button5.TabIndex = 9
        Me.ToolTip1.SetToolTip(Me.Button5, "Settings")
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button8
        '
        Me.Button8.BackgroundImage = Global.Paws.My.Resources.Resources.if_home_326656
        Me.Button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button8.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button8.FlatAppearance.BorderSize = 0
        Me.Button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button8.Location = New System.Drawing.Point(250, 3)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(35, 35)
        Me.Button8.TabIndex = 15
        Me.ToolTip1.SetToolTip(Me.Button8, "Home")
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.BackgroundImage = Global.Paws.My.Resources.Resources._1497462010_179_Upload
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(86, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(35, 35)
        Me.Button1.TabIndex = 10
        Me.ToolTip1.SetToolTip(Me.Button1, "Upload Files/Folder")
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.BackgroundImage = Global.Paws.My.Resources.Resources._1497118240_178_Download
        Me.Button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button7.FlatAppearance.BorderSize = 0
        Me.Button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button7.Location = New System.Drawing.Point(127, 3)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(35, 35)
        Me.Button7.TabIndex = 13
        Me.ToolTip1.SetToolTip(Me.Button7, "Download Files/Folders")
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.BackgroundImage = Global.Paws.My.Resources.Resources._1497066550_editor_folder_add_glyph
        Me.Button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Button4.FlatAppearance.BorderSize = 0
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Location = New System.Drawing.Point(4, 3)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(35, 35)
        Me.Button4.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.Button4, "New Folder" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        Me.Button4.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelectedItemsLabel, Me.SizeLabel, Me.LastModifiedLabel, Me.ProgressBar1, Me.SuccessfulLabel, Me.FailedLabel, Me.ChangeViewButton})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 720)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.StatusStrip1.Size = New System.Drawing.Size(1268, 23)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.Stretch = False
        Me.StatusStrip1.TabIndex = 25
        '
        'SelectedItemsLabel
        '
        Me.SelectedItemsLabel.AutoSize = False
        Me.SelectedItemsLabel.Name = "SelectedItemsLabel"
        Me.SelectedItemsLabel.Size = New System.Drawing.Size(80, 18)
        Me.SelectedItemsLabel.Text = "100 Selected"
        Me.SelectedItemsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'SizeLabel
        '
        Me.SizeLabel.AutoSize = False
        Me.SizeLabel.Name = "SizeLabel"
        Me.SizeLabel.Size = New System.Drawing.Size(150, 18)
        Me.SizeLabel.Text = "Size: 1023.99 Bytes"
        '
        'LastModifiedLabel
        '
        Me.LastModifiedLabel.AutoSize = False
        Me.LastModifiedLabel.Name = "LastModifiedLabel"
        Me.LastModifiedLabel.Size = New System.Drawing.Size(250, 18)
        Me.LastModifiedLabel.Text = "Last Modified: 01/02/2017 19:20:21"
        Me.LastModifiedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(100, 17)
        '
        'SuccessfulLabel
        '
        Me.SuccessfulLabel.AutoSize = False
        Me.SuccessfulLabel.ForeColor = System.Drawing.Color.SpringGreen
        Me.SuccessfulLabel.Name = "SuccessfulLabel"
        Me.SuccessfulLabel.Size = New System.Drawing.Size(50, 18)
        Me.SuccessfulLabel.Text = "0"
        '
        'FailedLabel
        '
        Me.FailedLabel.AutoSize = False
        Me.FailedLabel.ForeColor = System.Drawing.Color.Crimson
        Me.FailedLabel.Name = "FailedLabel"
        Me.FailedLabel.Size = New System.Drawing.Size(50, 18)
        Me.FailedLabel.Text = "0"
        '
        'ChangeViewButton
        '
        Me.ChangeViewButton.AutoSize = False
        Me.ChangeViewButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ChangeViewButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewDetails, Me.ViewTiles})
        Me.ChangeViewButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ChangeViewButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ChangeViewButton.Margin = New System.Windows.Forms.Padding(20, 2, 0, 0)
        Me.ChangeViewButton.Name = "ChangeViewButton"
        Me.ChangeViewButton.Size = New System.Drawing.Size(76, 21)
        Me.ChangeViewButton.Tag = ""
        Me.ChangeViewButton.Text = "View Style"
        Me.ChangeViewButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ViewDetails
        '
        Me.ViewDetails.Name = "ViewDetails"
        Me.ViewDetails.Size = New System.Drawing.Size(109, 22)
        Me.ViewDetails.Text = "Details"
        '
        'ViewTiles
        '
        Me.ViewTiles.Name = "ViewTiles"
        Me.ViewTiles.Size = New System.Drawing.Size(109, 22)
        Me.ViewTiles.Text = "Tiles"
        '
        'ToolTip1
        '
        Me.ToolTip1.BackColor = System.Drawing.Color.Gainsboro
        Me.ToolTip1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        '
        'Timer1
        '
        Me.Timer1.Interval = 120000
        '
        'Timer2
        '
        Me.Timer2.Enabled = True
        Me.Timer2.Interval = 1000
        '
        'Timer3
        '
        Me.Timer3.Interval = 300
        '
        'Timer4
        '
        Me.Timer4.Interval = 2000
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.SupportMultiDottedExtensions = True
        Me.SaveFileDialog1.Title = "Download Files/Folders"
        '
        'DetailsToolStripMenuItem
        '
        Me.DetailsToolStripMenuItem.Name = "DetailsToolStripMenuItem"
        Me.DetailsToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.DetailsToolStripMenuItem.Text = "Details"
        '
        'TilesToolStripMenuItem
        '
        Me.TilesToolStripMenuItem.Name = "TilesToolStripMenuItem"
        Me.TilesToolStripMenuItem.Size = New System.Drawing.Size(109, 22)
        Me.TilesToolStripMenuItem.Text = "Tiles"
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "Paws"
        Me.NotifyIcon1.Visible = True
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1270, 745)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(960, 250)
        Me.Name = "MainForm"
        Me.Opacity = 0R
        Me.Padding = New System.Windows.Forms.Padding(1)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Paws"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents imageList1 As ImageList
    Friend WithEvents ListView1 As ListView
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents Button5 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents Button4 As Button
    Friend WithEvents Button6 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Button8 As Button
    Friend WithEvents Button9 As Button
    Friend WithEvents ColumnHeader2 As ColumnHeader
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ColumnHeader3 As ColumnHeader
    Friend WithEvents ColumnHeader4 As ColumnHeader
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents Button11 As Button
    Friend WithEvents Button10 As Button
    Friend WithEvents ColumnHeader5 As ColumnHeader
    Public WithEvents Timer1 As Timer
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Button12 As Button
    Friend WithEvents Timer2 As Timer
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Button16 As Button
    Friend WithEvents Timer3 As Timer
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents OpenToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DownloadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RenameToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents NewFolderToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents LockToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UnlockToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeleteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer4 As Timer
    Friend WithEvents OpenAsMenuItem As ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents MoveToParentFolderMenuItem As ToolStripMenuItem
    Friend WithEvents CopyToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PasteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents SelectedItemsLabel As ToolStripStatusLabel
    Friend WithEvents SizeLabel As ToolStripStatusLabel
    Friend WithEvents LastModifiedLabel As ToolStripStatusLabel
    Friend WithEvents ChangeViewButton As ToolStripSplitButton
    Friend WithEvents ViewDetails As ToolStripMenuItem
    Friend WithEvents ViewTiles As ToolStripMenuItem
    Friend WithEvents DetailsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TilesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SuccessfulLabel As ToolStripStatusLabel
    Friend WithEvents FailedLabel As ToolStripStatusLabel
    Public WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents NotifyIcon1 As NotifyIcon
End Class
