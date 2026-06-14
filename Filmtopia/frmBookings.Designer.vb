<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmBookings
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
        Me.dgvScreenings = New System.Windows.Forms.DataGridView()
        Me.lblSeatMap = New System.Windows.Forms.Label()
        Me.pnlSeats = New System.Windows.Forms.Panel()
        Me.lblKeyAvailable = New System.Windows.Forms.Label()
        Me.lblKeyChosen = New System.Windows.Forms.Label()
        Me.lblKeyBooked = New System.Windows.Forms.Label()
        Me.lblCustomerName = New System.Windows.Forms.Label()
        Me.txtCustomerName = New System.Windows.Forms.TextBox()
        Me.lblSeatsCaption = New System.Windows.Forms.Label()
        Me.lblChosenSeats = New System.Windows.Forms.Label()
        Me.lblTicketsCaption = New System.Windows.Forms.Label()
        Me.lblTickets = New System.Windows.Forms.Label()
        Me.lblTotalCaption = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.btnClearBooking = New System.Windows.Forms.Button()
        Me.btnBook = New System.Windows.Forms.Button()
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvScreenings
        '
        Me.dgvScreenings.AllowUserToAddRows = False
        Me.dgvScreenings.AllowUserToDeleteRows = False
        Me.dgvScreenings.AllowUserToResizeRows = False
        Me.dgvScreenings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvScreenings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvScreenings.Location = New System.Drawing.Point(16, 15)
        Me.dgvScreenings.MultiSelect = False
        Me.dgvScreenings.Name = "dgvScreenings"
        Me.dgvScreenings.ReadOnly = True
        Me.dgvScreenings.RowHeadersVisible = False
        Me.dgvScreenings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvScreenings.Size = New System.Drawing.Size(560, 300)
        Me.dgvScreenings.TabIndex = 0
        '
        'lblSeatMap
        '
        Me.lblSeatMap.AutoSize = True
        Me.lblSeatMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSeatMap.Location = New System.Drawing.Point(592, 15)
        Me.lblSeatMap.Name = "lblSeatMap"
        Me.lblSeatMap.Size = New System.Drawing.Size(255, 20)
        Me.lblSeatMap.TabIndex = 1
        Me.lblSeatMap.Text = "Pick a screening to see its seats"
        '
        'pnlSeats
        '
        Me.pnlSeats.AutoScroll = True
        Me.pnlSeats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlSeats.Location = New System.Drawing.Point(592, 44)
        Me.pnlSeats.Name = "pnlSeats"
        Me.pnlSeats.Size = New System.Drawing.Size(630, 400)
        Me.pnlSeats.TabIndex = 2
        '
        'lblKeyAvailable
        '
        Me.lblKeyAvailable.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.lblKeyAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKeyAvailable.Location = New System.Drawing.Point(592, 456)
        Me.lblKeyAvailable.Name = "lblKeyAvailable"
        Me.lblKeyAvailable.Size = New System.Drawing.Size(100, 24)
        Me.lblKeyAvailable.TabIndex = 3
        Me.lblKeyAvailable.Text = "Available"
        Me.lblKeyAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblKeyChosen
        '
        Me.lblKeyChosen.BackColor = System.Drawing.Color.Fuchsia
        Me.lblKeyChosen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKeyChosen.Location = New System.Drawing.Point(704, 456)
        Me.lblKeyChosen.Name = "lblKeyChosen"
        Me.lblKeyChosen.Size = New System.Drawing.Size(100, 24)
        Me.lblKeyChosen.TabIndex = 4
        Me.lblKeyChosen.Text = "Chosen"
        Me.lblKeyChosen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblKeyBooked
        '
        Me.lblKeyBooked.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblKeyBooked.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblKeyBooked.ForeColor = System.Drawing.Color.Black
        Me.lblKeyBooked.Location = New System.Drawing.Point(816, 456)
        Me.lblKeyBooked.Name = "lblKeyBooked"
        Me.lblKeyBooked.Size = New System.Drawing.Size(100, 24)
        Me.lblKeyBooked.TabIndex = 5
        Me.lblKeyBooked.Text = "Booked"
        Me.lblKeyBooked.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCustomerName
        '
        Me.lblCustomerName.AutoSize = True
        Me.lblCustomerName.Location = New System.Drawing.Point(16, 332)
        Me.lblCustomerName.Name = "lblCustomerName"
        Me.lblCustomerName.Size = New System.Drawing.Size(103, 16)
        Me.lblCustomerName.TabIndex = 6
        Me.lblCustomerName.Text = "Customer Name"
        '
        'txtCustomerName
        '
        Me.txtCustomerName.Location = New System.Drawing.Point(160, 328)
        Me.txtCustomerName.Name = "txtCustomerName"
        Me.txtCustomerName.Size = New System.Drawing.Size(300, 22)
        Me.txtCustomerName.TabIndex = 7
        '
        'lblSeatsCaption
        '
        Me.lblSeatsCaption.AutoSize = True
        Me.lblSeatsCaption.Location = New System.Drawing.Point(16, 369)
        Me.lblSeatsCaption.Name = "lblSeatsCaption"
        Me.lblSeatsCaption.Size = New System.Drawing.Size(42, 16)
        Me.lblSeatsCaption.TabIndex = 8
        Me.lblSeatsCaption.Text = "Seats"
        '
        'lblChosenSeats
        '
        Me.lblChosenSeats.Location = New System.Drawing.Point(160, 369)
        Me.lblChosenSeats.Name = "lblChosenSeats"
        Me.lblChosenSeats.Size = New System.Drawing.Size(416, 36)
        Me.lblChosenSeats.TabIndex = 9
        Me.lblChosenSeats.Text = "None"
        '
        'lblTicketsCaption
        '
        Me.lblTicketsCaption.AutoSize = True
        Me.lblTicketsCaption.Location = New System.Drawing.Point(16, 412)
        Me.lblTicketsCaption.Name = "lblTicketsCaption"
        Me.lblTicketsCaption.Size = New System.Drawing.Size(52, 16)
        Me.lblTicketsCaption.TabIndex = 10
        Me.lblTicketsCaption.Text = "Tickets"
        '
        'lblTickets
        '
        Me.lblTickets.AutoSize = True
        Me.lblTickets.Location = New System.Drawing.Point(160, 412)
        Me.lblTickets.Name = "lblTickets"
        Me.lblTickets.Size = New System.Drawing.Size(14, 16)
        Me.lblTickets.TabIndex = 11
        Me.lblTickets.Text = "0"
        '
        'lblTotalCaption
        '
        Me.lblTotalCaption.AutoSize = True
        Me.lblTotalCaption.Location = New System.Drawing.Point(16, 445)
        Me.lblTotalCaption.Name = "lblTotalCaption"
        Me.lblTotalCaption.Size = New System.Drawing.Size(38, 16)
        Me.lblTotalCaption.TabIndex = 12
        Me.lblTotalCaption.Text = "Total"
        '
        'lblTotal
        '
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(160, 443)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(48, 20)
        Me.lblTotal.TabIndex = 13
        Me.lblTotal.Text = "£0.00"
        '
        'btnClearBooking
        '
        Me.btnClearBooking.Location = New System.Drawing.Point(976, 668)
        Me.btnClearBooking.Name = "btnClearBooking"
        Me.btnClearBooking.Size = New System.Drawing.Size(120, 37)
        Me.btnClearBooking.TabIndex = 14
        Me.btnClearBooking.Text = "Clear"
        Me.btnClearBooking.UseVisualStyleBackColor = True
        '
        'btnBook
        '
        Me.btnBook.Location = New System.Drawing.Point(1104, 668)
        Me.btnBook.Name = "btnBook"
        Me.btnBook.Size = New System.Drawing.Size(120, 37)
        Me.btnBook.TabIndex = 15
        Me.btnBook.Text = "Book"
        Me.btnBook.UseVisualStyleBackColor = True
        '
        'frmBookings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1240, 720)
        Me.Controls.Add(Me.btnBook)
        Me.Controls.Add(Me.btnClearBooking)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblTotalCaption)
        Me.Controls.Add(Me.lblTickets)
        Me.Controls.Add(Me.lblTicketsCaption)
        Me.Controls.Add(Me.lblChosenSeats)
        Me.Controls.Add(Me.lblSeatsCaption)
        Me.Controls.Add(Me.txtCustomerName)
        Me.Controls.Add(Me.lblCustomerName)
        Me.Controls.Add(Me.lblKeyBooked)
        Me.Controls.Add(Me.lblKeyChosen)
        Me.Controls.Add(Me.lblKeyAvailable)
        Me.Controls.Add(Me.pnlSeats)
        Me.Controls.Add(Me.lblSeatMap)
        Me.Controls.Add(Me.dgvScreenings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmBookings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filmtopia - Bookings"
        CType(Me.dgvScreenings, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvScreenings As System.Windows.Forms.DataGridView
    Friend WithEvents lblSeatMap As System.Windows.Forms.Label
    Friend WithEvents pnlSeats As System.Windows.Forms.Panel
    Friend WithEvents lblKeyAvailable As System.Windows.Forms.Label
    Friend WithEvents lblKeyChosen As System.Windows.Forms.Label
    Friend WithEvents lblKeyBooked As System.Windows.Forms.Label
    Friend WithEvents lblCustomerName As System.Windows.Forms.Label
    Friend WithEvents txtCustomerName As System.Windows.Forms.TextBox
    Friend WithEvents lblSeatsCaption As System.Windows.Forms.Label
    Friend WithEvents lblChosenSeats As System.Windows.Forms.Label
    Friend WithEvents lblTicketsCaption As System.Windows.Forms.Label
    Friend WithEvents lblTickets As System.Windows.Forms.Label
    Friend WithEvents lblTotalCaption As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents btnClearBooking As System.Windows.Forms.Button
    Friend WithEvents btnBook As System.Windows.Forms.Button
End Class
