Imports System.Windows.Forms.DataVisualization.Charting

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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim ChartArea1 As ChartArea = New ChartArea()
        Dim Series1 As Series = New Series()
        StartBtn = New Button()
        TargetTxt = New TextBox()
        IntervalTxt = New ComboBox()
        durationTxt = New ComboBox()
        MenuStrip1 = New MenuStrip()
        ChartScroll = New HScrollBar()
        SplitContainer1 = New SplitContainer()
        PingListBox = New DataGridView()
        Host = New DataGridViewTextBoxColumn()
        Hostname = New DataGridViewTextBoxColumn()
        Ping = New DataGridViewTextBoxColumn()
        Low = New DataGridViewTextBoxColumn()
        High = New DataGridViewTextBoxColumn()
        Chart1 = New Chart()
        Panel = New Panel()
        CType(SplitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        SplitContainer1.Panel1.SuspendLayout()
        SplitContainer1.Panel2.SuspendLayout()
        SplitContainer1.SuspendLayout()
        CType(PingListBox, ComponentModel.ISupportInitialize).BeginInit()
        CType(Chart1, ComponentModel.ISupportInitialize).BeginInit()
        Panel.SuspendLayout()
        SuspendLayout()
        ' 
        ' StartBtn
        ' 
        StartBtn.FlatAppearance.BorderColor = Color.DimGray
        StartBtn.FlatAppearance.MouseDownBackColor = Color.FromArgb(CByte(224), CByte(224), CByte(224))
        StartBtn.FlatAppearance.MouseOverBackColor = Color.Silver
        StartBtn.FlatStyle = FlatStyle.Flat
        StartBtn.Location = New Point(489, 16)
        StartBtn.Name = "StartBtn"
        StartBtn.Size = New Size(146, 23)
        StartBtn.TabIndex = 4
        StartBtn.Text = "Start"
        StartBtn.UseVisualStyleBackColor = True
        ' 
        ' TargetTxt
        ' 
        TargetTxt.BorderStyle = BorderStyle.FixedSingle
        TargetTxt.Location = New Point(12, 16)
        TargetTxt.Name = "TargetTxt"
        TargetTxt.Size = New Size(281, 23)
        TargetTxt.TabIndex = 1
        ' 
        ' IntervalTxt
        ' 
        IntervalTxt.DisplayMember = "0.5"
        IntervalTxt.FlatStyle = FlatStyle.Flat
        IntervalTxt.FormattingEnabled = True
        IntervalTxt.Items.AddRange(New Object() {"500", "1000", "2000", "5000", "10000", "15000"})
        IntervalTxt.Location = New Point(299, 16)
        IntervalTxt.Name = "IntervalTxt"
        IntervalTxt.Size = New Size(89, 23)
        IntervalTxt.TabIndex = 2
        IntervalTxt.Text = "1000"
        ' 
        ' durationTxt
        ' 
        durationTxt.DisplayMember = "0.5"
        durationTxt.FlatStyle = FlatStyle.Flat
        durationTxt.FormattingEnabled = True
        durationTxt.Items.AddRange(New Object() {"500", "1000", "2000", "5000", "10000", "15000"})
        durationTxt.Location = New Point(394, 16)
        durationTxt.Name = "durationTxt"
        durationTxt.Size = New Size(89, 23)
        durationTxt.TabIndex = 3
        durationTxt.Text = "30000"
        ' 
        ' MenuStrip1
        ' 
        MenuStrip1.Location = New Point(0, 0)
        MenuStrip1.MinimumSize = New Size(0, 50)
        MenuStrip1.Name = "MenuStrip1"
        MenuStrip1.Size = New Size(1138, 50)
        MenuStrip1.TabIndex = 6
        MenuStrip1.Text = "MenuStrip1"
        ' 
        ' ChartScroll
        ' 
        ChartScroll.Dock = DockStyle.Bottom
        ChartScroll.Location = New Point(0, 659)
        ChartScroll.Name = "ChartScroll"
        ChartScroll.Size = New Size(1138, 25)
        ChartScroll.TabIndex = 7
        ' 
        ' SplitContainer1
        ' 
        SplitContainer1.Dock = DockStyle.Fill
        SplitContainer1.Location = New Point(0, 0)
        SplitContainer1.Name = "SplitContainer1"
        SplitContainer1.Orientation = Orientation.Horizontal
        ' 
        ' SplitContainer1.Panel1
        ' 
        SplitContainer1.Panel1.Controls.Add(PingListBox)
        ' 
        ' SplitContainer1.Panel2
        ' 
        SplitContainer1.Panel2.Controls.Add(Chart1)
        SplitContainer1.Size = New Size(1138, 609)
        SplitContainer1.SplitterDistance = 281
        SplitContainer1.TabIndex = 6
        ' 
        ' PingListBox
        ' 
        PingListBox.AllowUserToAddRows = False
        PingListBox.AllowUserToDeleteRows = False
        PingListBox.AllowUserToResizeRows = False
        PingListBox.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        PingListBox.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        PingListBox.BorderStyle = BorderStyle.None
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        PingListBox.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        PingListBox.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        PingListBox.Columns.AddRange(New DataGridViewColumn() {Host, Hostname, Ping, Low, High})
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point)
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        PingListBox.DefaultCellStyle = DataGridViewCellStyle2
        PingListBox.Dock = DockStyle.Fill
        PingListBox.Location = New Point(0, 0)
        PingListBox.Name = "PingListBox"
        PingListBox.RowHeadersVisible = False
        PingListBox.RowTemplate.Height = 25
        PingListBox.Size = New Size(1138, 281)
        PingListBox.TabIndex = 4
        ' 
        ' Host
        ' 
        Host.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Host.HeaderText = "IP"
        Host.Name = "Host"
        Host.ReadOnly = True
        ' 
        ' Hostname
        ' 
        Hostname.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        Hostname.HeaderText = "Hostname"
        Hostname.Name = "Hostname"
        Hostname.ReadOnly = True
        ' 
        ' Ping
        ' 
        Ping.HeaderText = "Current Ping"
        Ping.Name = "Ping"
        Ping.ReadOnly = True
        Ping.Width = 99
        ' 
        ' Low
        ' 
        Low.HeaderText = "Lowest Ping"
        Low.Name = "Low"
        Low.ReadOnly = True
        Low.Width = 96
        ' 
        ' High
        ' 
        High.HeaderText = "Highest Ping"
        High.Name = "High"
        High.ReadOnly = True
        ' 
        ' Chart1
        ' 
        ChartArea1.Name = "ChartArea1"
        Chart1.ChartAreas.Add(ChartArea1)
        Chart1.Dock = DockStyle.Fill
        Chart1.Location = New Point(0, 0)
        Chart1.Margin = New Padding(3, 3, 3, 100)
        Chart1.Name = "Chart1"
        Series1.ChartArea = "ChartArea1"
        Series1.Name = "Series1"
        Chart1.Series.Add(Series1)
        Chart1.Size = New Size(1138, 324)
        Chart1.TabIndex = 0
        Chart1.Text = "Chart1"
        ' 
        ' Panel
        ' 
        Panel.Controls.Add(SplitContainer1)
        Panel.Dock = DockStyle.Fill
        Panel.Location = New Point(0, 50)
        Panel.Name = "Panel"
        Panel.Size = New Size(1138, 609)
        Panel.TabIndex = 8
        ' 
        ' MainForm
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.Silver
        ClientSize = New Size(1138, 684)
        Controls.Add(Panel)
        Controls.Add(ChartScroll)
        Controls.Add(durationTxt)
        Controls.Add(IntervalTxt)
        Controls.Add(TargetTxt)
        Controls.Add(StartBtn)
        Controls.Add(MenuStrip1)
        MainMenuStrip = MenuStrip1
        Name = "MainForm"
        Text = "Ping Paver"
        SplitContainer1.Panel1.ResumeLayout(False)
        SplitContainer1.Panel2.ResumeLayout(False)
        CType(SplitContainer1, ComponentModel.ISupportInitialize).EndInit()
        SplitContainer1.ResumeLayout(False)
        CType(PingListBox, ComponentModel.ISupportInitialize).EndInit()
        CType(Chart1, ComponentModel.ISupportInitialize).EndInit()
        Panel.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub
    Friend WithEvents StartBtn As Button
    Friend WithEvents TargetTxt As TextBox
    Friend WithEvents IntervalTxt As ComboBox
    Friend WithEvents durationTxt As ComboBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ChartScroll As HScrollBar
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents PingListBox As DataGridView
    Friend WithEvents Host As DataGridViewTextBoxColumn
    Friend WithEvents Hostname As DataGridViewTextBoxColumn
    Friend WithEvents Ping As DataGridViewTextBoxColumn
    Friend WithEvents Low As DataGridViewTextBoxColumn
    Friend WithEvents High As DataGridViewTextBoxColumn
    Friend WithEvents Chart1 As Chart
    Friend WithEvents Panel As Panel
End Class
