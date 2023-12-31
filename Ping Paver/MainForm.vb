Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Windows.Forms.DataVisualization.Charting

Public Class MainForm
    Private pingDuration As Integer = 300000 'ping duration
    Private pingInterval As Integer = 1000 'Ping interval in milliseconds
    Private targetAddress As String = "" ' Target IP or domain to monitor
    Private tracerouteResults As List(Of String) ' Store traceroute results
    Private pingThread As Thread ' Thread for pinging
    Private isPingLoopRunning As Boolean = False
    Private maxHops As Integer = 8
    Private hopTimeout As Integer = 300
    Private cancellationTokenSource As CancellationTokenSource
    Private pingHistory As New Dictionary(Of String, List(Of PingData))()


    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tracerouteResults = New List(Of String)()
    End Sub

    Private Sub PerformTraceroute()
        ' Use the traceroute command to get the network path
        Dim tracerouteProcess As New Process()
        tracerouteProcess.StartInfo.FileName = "tracert"
        tracerouteProcess.StartInfo.Arguments = "-h " & maxHops & " -w " & hopTimeout & " " & targetAddress
        tracerouteProcess.StartInfo.UseShellExecute = False
        tracerouteProcess.StartInfo.RedirectStandardOutput = True
        tracerouteProcess.StartInfo.CreateNoWindow = True

        Debug.Print("Running " & tracerouteProcess.StartInfo.FileName & " " & tracerouteProcess.StartInfo.Arguments)

        tracerouteProcess.Start()

        ' Read the output synchronously line by line
        Dim output As StringBuilder = New StringBuilder()
        While Not tracerouteProcess.StandardOutput.EndOfStream
            Dim line As String = tracerouteProcess.StandardOutput.ReadLine()
            output.AppendLine(line)
            ' Process the line as needed
            ProcessTracerouteLine(line)
            My.Application.DoEvents()
        End While

        ' Wait for the process to exit
        tracerouteProcess.WaitForExit()

        Debug.Print(output.ToString())

        ' ...

        ' Start the ping loop
        StartPingLoop()

        ' Update button text and flag
        StartBtn.Text = "Stop"
        isPingLoopRunning = True
    End Sub

    Private Sub ProcessTracerouteLine(line As String)
        Try

            ' Parse and store the traceroute results
            ' Extract the IP or hostname of each hop (e.g., "  1    <1 ms    <1 ms    <1 ms  [192.168.1.1]")
            Dim hopData As String = line.Trim()
            Dim hopAddress As String = ExtractHopAddress(hopData)
            Dim hopHostname As String = ExtractHopHostname(hopData)
            If Not String.IsNullOrEmpty(hopAddress) Then
                tracerouteResults.Add(hopAddress)
                ' Add a new row to the PingListBox (DataGridView)
                Dim row As DataGridViewRow = New DataGridViewRow()
                row.CreateCells(PingListBox)
                row.Cells(0).Value = hopAddress ' Set the IP/Hostname value
                If Not String.IsNullOrEmpty(hopHostname) Then
                    row.Cells(1).Value = hopHostname 'set the hostname
                Else
                    row.Cells(1).Value = "Unresolved"
                    row.Cells(1).ToolTipText = "Unable to resolve hostname, you may want to check your DNS settings if this reoccurs."
                End If
                ' Set values for other columns if needed
                row.Cells(2).Value = Nothing ' Current Ping
                row.Cells(3).Value = Nothing ' Lowest Ping
                row.Cells(4).Value = Nothing ' Highest Ping
                PingListBox.Rows.Add(row)

                ' ...

                ' Create a new list to store the ping history for the node
                Dim nodePingHistory As New List(Of PingData)()

                ' Store the ping history in the dictionary
                pingHistory.Add(hopAddress, nodePingHistory)
            End If

        Catch ex As Exception
            Debug.Print(ex.Message)
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error")
        End Try
    End Sub

    Private Function ExtractHopHostname(hopData As String) As String
        ' Find the first occurrence of "[" and "]" to extract the IP address or hostname
        Dim startIndex As Integer = hopData.LastIndexOf("  ")
        Dim endIndex As Integer = hopData.IndexOf(" [")
        If startIndex >= 0 AndAlso endIndex >= 0 AndAlso endIndex > startIndex Then
            Dim hopHostname As String = hopData.Substring(startIndex + 1, endIndex - startIndex - 1)
            Return hopHostname.Trim()
        End If

        Return String.Empty
    End Function

    Private Function ExtractHopAddress(hopData As String) As String
        Dim regexPattern As String = "\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b"
        Dim regex As New Regex(regexPattern)

        Dim match As Match = regex.Match(hopData)
        If match.Success Then
            Return match.Value.Trim()
        End If

        Return String.Empty
    End Function

    Private Sub StartPingLoop()
        Dim pingThread As New Thread(AddressOf PingLoop)
        pingThread.Start()
    End Sub

    Private Sub PingLoop()
        Dim pingSender As New Ping()
        Dim pingOptions As New PingOptions()
        pingOptions.DontFragment = True
        While True
            If cancellationTokenSource.Token.IsCancellationRequested Then
                Exit While
            End If

            ' Use Parallel.ForEach to execute pings concurrently
            Parallel.ForEach(tracerouteResults, Sub(hopAddress)
                                                    ' Create a new instance of Ping for each thread
                                                    Dim pingInstance As New Ping()
                                                    Dim reply As PingReply = pingInstance.Send(hopAddress, pingInterval)
                                                    Dim success As Boolean = (reply.Status = IPStatus.Success)

                                                    ' Process ping reply, extract RTT and packet loss information, and update the graph or display the data
                                                    ' Example: UpdateGraph(reply.RoundtripTime, reply.Status = IPStatus.Success)

                                                    SyncLock PingListBox
                                                        ' Find the corresponding row in PingListBox (DataGridView) based on hopAddress
                                                        Dim rowIndex As Integer = -1
                                                        For i As Integer = 0 To PingListBox.Rows.Count - 1
                                                            If PingListBox.Rows(i).Cells(0).Value.ToString() = hopAddress Then
                                                                rowIndex = i
                                                                Exit For
                                                            End If
                                                        Next

                                                        If rowIndex <> -1 Then

                                                            ' Set the background color for the "High" cell based on success/failure
                                                            If reply.Status = IPStatus.Success Then

                                                                PingListBox.Rows(rowIndex).Cells("Ping").Style.BackColor = Color.FromArgb(204, 255, 204)
                                                                ' Update the current ping
                                                                PingListBox.Rows(rowIndex).Cells("Ping").Value = reply.RoundtripTime

                                                                ' Update the lowest ping (column "Low") in the corresponding row
                                                                Dim currentLowest As Integer = -1
                                                                If PingListBox.Rows(rowIndex).Cells("Low").Value IsNot Nothing Then
                                                                    currentLowest = Integer.Parse(PingListBox.Rows(rowIndex).Cells("Low").Value.ToString())
                                                                End If
                                                                If currentLowest = -1 OrElse reply.RoundtripTime < currentLowest Then
                                                                    PingListBox.Rows(rowIndex).Cells("Low").Value = reply.RoundtripTime
                                                                End If

                                                                ' Update the new column "High" with the highest ping value if applicable
                                                                Dim currentHigh As Integer = -1
                                                                If PingListBox.Rows(rowIndex).Cells("High").Value IsNot Nothing Then
                                                                    currentHigh = Integer.Parse(PingListBox.Rows(rowIndex).Cells("High").Value.ToString())
                                                                End If
                                                                If currentHigh = -1 OrElse reply.RoundtripTime > currentHigh Then
                                                                    PingListBox.Rows(rowIndex).Cells("High").Value = reply.RoundtripTime
                                                                End If

                                                            Else
                                                                PingListBox.Rows(rowIndex).Cells("Ping").Style.BackColor = Color.PaleVioletRed
                                                            End If
                                                        End If

                                                        If pingHistory.ContainsKey(hopAddress) Then
                                                            Dim nodePingHistory As List(Of PingData) = pingHistory(hopAddress)
                                                            ' Remove ping data older than a certain duration
                                                            Dim maxAge As Integer = pingDuration
                                                            Dim cutoffTime As DateTime = DateTime.Now.AddMilliseconds(-maxAge)

                                                            ' Remove the oldest data before the cutoff time
                                                            nodePingHistory.RemoveAll(Function(pingData) pingData.Timestamp < cutoffTime)

                                                            ' Add the new ping data
                                                            nodePingHistory.Add(New PingData(DateTime.Now, reply.RoundtripTime, success))
                                                        End If
                                                    End SyncLock

                                                End Sub)

            Thread.Sleep(pingInterval)
        End While

        ' Reset button text and flag when the ping loop is stopped
        StartBtn.Invoke(Sub() StartBtn.Text = "Start")
        isPingLoopRunning = False
    End Sub

    Private Sub StopPingLoop()
        If cancellationTokenSource IsNot Nothing Then
            cancellationTokenSource.Cancel()
        End If
    End Sub

    Private Sub StartBtn_Click(sender As Object, e As EventArgs) Handles StartBtn.Click
        If isPingLoopRunning Then
            ' Ping loop is already running, stop it
            StopPingLoop()
            StartBtn.Text = "Stopping..."
        Else

            'let the user know we are about to trace the connection
            StartBtn.Text = "Tracing...please wait"

            'clear previous
            If chartRefreshTimer.Enabled = True Then chartRefreshTimer.Stop()
            PingListBox.ClearSelection()
            PingListBox.Rows.Clear()
            pingHistory.Clear()

            My.Application.DoEvents()

            'start
            pingInterval = CInt(IntervalTxt.Text)
            pingDuration = CInt(durationTxt.Text)
            targetAddress = TargetTxt.Text

            cancellationTokenSource = New CancellationTokenSource()
            PerformTraceroute()
        End If
    End Sub

    Private Sub PingListBox_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PingListBox.CellClick
        InitializeChart()
    End Sub


#Region "Uncharted Chart"
    '#
    '#

    ' Define a Timer object for refreshing the chart
    Private WithEvents chartRefreshTimer As New System.Windows.Forms.Timer()

    Private Sub InitializeChart()
        ' Set up the chart properties
        Chart1.ChartAreas.Clear()
        Chart1.Series.Clear()

        Chart1.ChartAreas.Add("ChartArea1")
        'Chart1.ChartAreas("ChartArea1").AxisX = "MM:ss"
        Chart1.ChartAreas("ChartArea1").AxisX.LabelStyle.Format = "HH:mm:ss"
        Chart1.ChartAreas("ChartArea1").AxisX.MajorGrid.LineColor = Color.LightGray
        Chart1.ChartAreas("ChartArea1").AxisY.MajorGrid.LineColor = Color.LightGray


        ' Create a new series for packet loss values
        Dim lossSeries As New Series("LossSeries")
        lossSeries("PointWidth") = "5" ' Set the point width
        lossSeries.Color = Color.Red ' Set the color for packet loss values

        ' Create a new series for ping values
        Dim pingSeries As New Series("PingSeries")
        pingSeries("PointWidth") = "1" ' Set the point width
        pingSeries.Color = Color.Blue ' Set the color for ping values

        Chart1.Series.Add(lossSeries)
        Chart1.Series.Add(pingSeries)

        Chart1.Series("PingSeries").ChartType = SeriesChartType.StepLine
        Chart1.Series("PingSeries").XValueType = ChartValueType.DateTime
        Chart1.Series("PingSeries").YValueType = ChartValueType.Int32
        Chart1.Series("PingSeries").IsValueShownAsLabel = True

        Chart1.Series("LossSeries").ChartType = SeriesChartType.ErrorBar
        Chart1.Series("LossSeries").XValueType = ChartValueType.DateTime
        Chart1.Series("LossSeries").YValueType = ChartValueType.Int32
        Chart1.Series("LossSeries").IsValueShownAsLabel = True

        Dim chartpingHistory As Dictionary(Of String, List(Of PingData)) = pingHistory

        ' Set up the chart refresh timer
        chartRefreshTimer.Interval = pingInterval ' interval
        chartRefreshTimer.Start()
    End Sub

    Private Sub chartRefreshTimer_Tick(sender As Object, e As EventArgs) Handles chartRefreshTimer.Tick
        'we create a copy of the pingHistory to use incase data changes while in generation
        Dim chartpingHistory As Dictionary(Of String, List(Of PingData)) = pingHistory.ToDictionary(Function(entry) entry.Key, Function(entry) entry.Value.ToList())

        ' Check if the chart is currently visible
        If Chart1.Visible Then
            ' Get the selected node from the DataGridView
            Dim selectedNode As String = PingListBox.SelectedCells(0).Value
            If selectedNode Is Nothing Then Exit Sub
            ' Check if the selected node exists in the ping history dictionary
            If chartpingHistory.ContainsKey(selectedNode) Then
                    ' Get the ping history for the selected node
                    Dim nodePingHistory As List(Of PingData) = chartpingHistory(selectedNode)

                    ' Clear the chart series
                    Chart1.Series("LossSeries").Points.Clear()
                    Chart1.Series("PingSeries").Points.Clear()

                    ' Calculate packet loss
                    Dim totalPings As Integer = nodePingHistory.Count
                    Dim failedPings As Integer = nodePingHistory.Where(Function(pingData) Not pingData.Success).Count()
                    Dim packetLoss As Double = (failedPings / totalPings) * 100

                    ' Add data points to the series
                    For Each pingData As PingData In nodePingHistory
                        ' Create a new data point for packet loss value
                        Dim lossDataPoint As New DataPoint(pingData.Timestamp.ToOADate(), packetLoss)
                        Chart1.Series("LossSeries").Points.Add(lossDataPoint)

                        ' Create a new data point for ping value
                        Dim pingDataPoint As New DataPoint(pingData.Timestamp.ToOADate(), pingData.Ping)
                        Chart1.Series("PingSeries").Points.Add(pingDataPoint)
                        ' Display data labels at the data points
                    Next

                End If
            End If
    End Sub




    Private Sub chartScroll_Scroll(sender As Object, e As ScrollEventArgs) Handles ChartScroll.Scroll
        'Stop Refreshing the chart
        chartRefreshTimer.Stop()


        If PingListBox.Rows.Count < 1 Then Exit Sub

        ' Get the selected node from the DataGridView
        Dim selectedNode As String = PingListBox.SelectedCells(0).Value

        ' Check if the selected node exists in the ping history dictionary
        If pingHistory.ContainsKey(selectedNode) Then
            ' Get the ping history for the selected node
            Dim nodePingHistory As List(Of PingData) = pingHistory(selectedNode)

            ' Calculate the range of data to display based on the scroll position
            Dim startIndex As Integer = e.NewValue
            Dim endIndex As Integer = startIndex + ChartScroll.LargeChange - 1

            ' Clear the chart series
            Chart1.Series("PingSeries").Points.Clear()

            ' Add data points to the series within the selected range
            For i As Integer = startIndex To endIndex
                If i >= 0 AndAlso i < nodePingHistory.Count Then
                    Dim pingData As PingData = nodePingHistory(i)

                    ' Create a new data point
                    Dim dataPoint As New DataPoint(pingData.Timestamp.ToOADate(), pingData.Ping)

                    ' Set the color based on the success status
                    If pingData.Success Then
                        dataPoint.Color = Color.Green
                    Else
                        dataPoint.Color = Color.Red
                    End If

                    ' Add the data point to the series
                    Chart1.Series("PingSeries").Points.Add(dataPoint)

                    ' Display data labels at the data points
                    Chart1.Series("PingSeries").IsValueShownAsLabel = True
                End If
            Next
        End If

        'Start Refreshing the chart again
        chartRefreshTimer.Stop()
    End Sub
    '#
    '#
#End Region




End Class

Public Class PingData
    Public Property Timestamp As DateTime
    Public Property Ping As Integer
    Public Property Success As Boolean

    Public Sub New(timestamp As DateTime, ping As Integer, success As Boolean)
        Me.Timestamp = timestamp
        Me.Ping = ping
        Me.Success = success
    End Sub
End Class