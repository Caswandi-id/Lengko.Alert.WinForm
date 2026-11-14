Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports Lengko.Alert.WinForms

Namespace Lengko.Alert.Demo.VB

    Public Class MainForm
        Inherits Form

        Private _interactiveBox As TextBox
        Private _clickCount As Integer = 0
        Private _clickLabel As Label

        Public Sub New()
            InitializeUi()
        End Sub

        Private Sub InitializeUi()
            Text = "Lengko.Alert.WinForms — VB.NET Demo"
            Width = 860
            Height = 650
            StartPosition = FormStartPosition.CenterScreen
            Font = New Font("Segoe UI", 9.5F)
            BackColor = Color.FromArgb(248, 250, 252)

            Dim mainPanel As New TableLayoutPanel With {
                .Dock = DockStyle.Fill,
                .ColumnCount = 1,
                .RowCount = 3,
                .Padding = New Padding(20)
            }
            mainPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 70))
            mainPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            mainPanel.RowStyles.Add(New RowStyle(SizeType.Absolute, 80))

            ' Header
            Dim headerPanel As New Panel With {.Dock = DockStyle.Fill}
            Dim titleLabel As New Label With {
                .Text = "Lengko.Alert Demo (VB.NET WinForms)",
                .Font = New Font("Segoe UI", 16.0F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(15, 23, 42),
                .AutoSize = True,
                .Location = New Point(0, 5)
            }
            Dim subLabel As New Label With {
                .Text = "Zero external dependencies. Pure WinForms + GDI+ alerts, toasts, and prompts.",
                .Font = New Font("Segoe UI", 9.5F),
                .ForeColor = Color.FromArgb(100, 116, 139),
                .AutoSize = True,
                .Location = New Point(0, 36)
            }
            headerPanel.Controls.Add(titleLabel)
            headerPanel.Controls.Add(subLabel)
            mainPanel.Controls.Add(headerPanel, 0, 0)

            ' Content Flow
            Dim contentFlow As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .AutoScroll = True,
                .WrapContents = True,
                .FlowDirection = FlowDirection.LeftToRight
            }

            ' Section 1: Basic Modal Alerts
            contentFlow.Controls.Add(CreateCard("1. Basic Modal Alerts", {
                CreateButton("Success Alert", Color.FromArgb(34, 197, 94), Sub(s, e)
                    LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success)
                End Sub),
                CreateButton("Error Alert", Color.FromArgb(239, 68, 68), Sub(s, e)
                    LengkoAlert.Fire("Oops...", "Terjadi kesalahan saat memproses data.", AlertIcon.Error)
                End Sub),
                CreateButton("Warning Alert", Color.FromArgb(245, 158, 11), Sub(s, e)
                    LengkoAlert.Fire("Peringatan!", "Sesi login Anda akan segera berakhir.", AlertIcon.Warning)
                End Sub),
                CreateButton("Info Alert", Color.FromArgb(59, 130, 246), Sub(s, e)
                    LengkoAlert.Fire("Informasi", "Lengko.Alert berjalan di VB.NET tanpa dependensi eksternal.", AlertIcon.Info)
                End Sub),
                CreateButton("Question Alert", Color.FromArgb(139, 92, 246), Sub(s, e)
                    LengkoAlert.Fire("Pertanyaan", "Apakah Anda ingin melanjutkan proses ini?", AlertIcon.Question)
                End Sub)
            }))

            ' Section 2: Confirm & Prompt
            contentFlow.Controls.Add(CreateCard("2. Confirm & Prompt", {
                CreateButton("Confirm (Sync)", Color.FromArgb(79, 70, 229), Sub(s, e)
                    Dim ok As Boolean = LengkoAlert.Confirm("Konfirmasi Hapus", "Yakin ingin menghapus record ini?", "Ya, Hapus!", "Batal", AlertIcon.Warning)
                    If ok Then
                        LengkoAlert.Fire("Terhapus!", "Record berhasil dihapus.", AlertIcon.Success)
                    End If
                End Sub),
                CreateButton("Confirm (Async)", Color.FromArgb(79, 70, 229), Async Sub(s, e)
                    Dim ok As Boolean = Await LengkoAlert.ConfirmAsync("Konfirmasi Simpan", "Simpan semua perubahan ke database?", "Simpan", "Batal", AlertIcon.Question)
                    If ok Then
                        Await LengkoAlert.FireAsync("Tersimpan!", "Data perubahan telah diperbarui.", AlertIcon.Success)
                    End If
                End Sub),
                CreateButton("Prompt Text", Color.FromArgb(14, 165, 233), Sub(s, e)
                    Dim name As String = LengkoAlert.Prompt("Input Nama", "Masukkan nama lengkap Anda:", "Contoh: Siti Rahma")
                    If Not String.IsNullOrWhiteSpace(name) Then
                        LengkoAlert.Fire("Halo!", "Selamat datang, " & name & "!", AlertIcon.Info)
                    End If
                End Sub),
                CreateButton("Prompt with Validator", Color.FromArgb(14, 165, 233), Async Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Input Email",
                        .Text = "Format harus mengandung '@'.",
                        .Input = AlertInputType.Text,
                        .InputPlaceholder = "user@domain.com",
                        .ShowCancelButton = True,
                        .InputValidator = Function(val As String) As String
                            If String.IsNullOrWhiteSpace(val) OrElse Not val.Contains("@"c) Then
                                Return "Email tidak valid (harus ada @)!"
                            End If
                            Return Nothing
                        End Function
                    }
                    Dim res = Await LengkoAlert.FireAsync(opt)
                    If res.IsConfirmed Then
                        Await LengkoAlert.FireAsync("Email Valid!", "Email Anda: " & res.Value, AlertIcon.Success)
                    End If
                End Sub)
            }))

            ' Section 3: Toast Notifications (Always Non-Modal)
            contentFlow.Controls.Add(CreateCard("3. Toast Notifications (Non-Modal)", {
                CreateButton("Toast TopEnd (Default)", Color.FromArgb(16, 185, 129), Sub(s, e)
                    LengkoAlert.ShowToast("Login Berhasil", "Selamat datang kembali!", AlertIcon.Success, 3000)
                End Sub),
                CreateButton("Toast BottomStart", Color.FromArgb(16, 185, 129), Sub(s, e)
                    LengkoAlert.ShowToast("Data Masuk", "Pojok kiri bawah", AlertPosition.BottomStart, AlertIcon.Info, 3000)
                End Sub),
                CreateButton("Toast BottomEnd", Color.FromArgb(16, 185, 129), Sub(s, e)
                    LengkoAlert.ShowToast("Disimpan", "Pojok kanan bawah", AlertPosition.BottomEnd, AlertIcon.Success, 3000)
                End Sub),
                CreateButton("Toast Custom Offset (30,30)", Color.FromArgb(16, 185, 129), Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Custom Margin",
                        .Text = "Toast dengan offset 30px dari tepi layar.",
                        .Position = AlertPosition.TopStart,
                        .Offset = New Point(30, 30),
                        .Icon = AlertIcon.Warning,
                        .Timer = 3500
                    }
                    LengkoAlert.ShowToast(opt)
                End Sub)
            }))

            ' Section 4: Non-Modal Alert & Styling
            contentFlow.Controls.Add(CreateCard("4. Non-Modal Alert & Styling", {
                CreateButton("Non-Modal Alert (UI Aktif)", Color.FromArgb(225, 29, 72), Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Non-Modal Alert",
                        .Text = "Alert melayang! Coba klik tombol bawah atau ketik di text box.",
                        .Icon = AlertIcon.Info,
                        .Modal = False,
                        .Position = AlertPosition.TopEnd,
                        .Timer = 7000,
                        .TimerProgressBar = True,
                        .Owner = Me
                    }
                    LengkoAlert.Fire(opt)
                End Sub),
                CreateButton("Oval Pill Buttons", Color.FromArgb(100, 116, 139), Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Oval Pill Buttons",
                        .Text = "Tombol aksi melengkung penuh berbentuk kapsul/oval.",
                        .Icon = AlertIcon.Question,
                        .ShowCancelButton = True,
                        .OvalButtons = True
                    }
                    LengkoAlert.Fire(opt)
                End Sub),
                CreateButton("Bounce Animation", Color.FromArgb(100, 116, 139), Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Animasi Membal (Bounce)",
                        .Text = "Alert jatuh membal elastis dari atas layar.",
                        .Icon = AlertIcon.Success,
                        .OpenAnimation = AlertAnimation.Bounce,
                        .CloseAnimation = AlertAnimation.Fade,
                        .AnimationDuration = 180
                    }
                    LengkoAlert.Fire(opt)
                End Sub),
                CreateButton("Dark Theme", Color.FromArgb(30, 41, 59), Sub(s, e)
                    Dim opt As New AlertOptions With {
                        .Title = "Dark Theme",
                        .Text = "Tampilan dark slate dengan kontras tinggi.",
                        .Theme = AlertTheme.Dark,
                        .Icon = AlertIcon.Success,
                        .ShowCancelButton = True
                    }
                    LengkoAlert.Fire(opt)
                End Sub)
            }))

            mainPanel.Controls.Add(contentFlow, 0, 1)

            ' Section 5: Interactive playground
            Dim testBar As New Panel With {
                .Dock = DockStyle.Fill,
                .BackColor = Color.White,
                .Padding = New Padding(15, 10, 15, 10)
            }

            Dim playgroundLabel As New Label With {
                .Text = "Playground Uji Interaksi UI Utama (Tes Non-Modal):",
                .Font = New Font("Segoe UI", 9.5F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(51, 65, 85),
                .Location = New Point(15, 10),
                .AutoSize = True
            }

            _interactiveBox = New TextBox With {
                .Location = New Point(15, 36),
                .Width = 320,
                .PlaceholderText = "Ketik teks di sini saat alert Non-Modal muncul..."
            }

            Dim clickBtn As New Button With {
                .Text = "Klik Saya Saat Alert Tampil",
                .Location = New Point(345, 34),
                .Width = 200,
                .Height = 28,
                .BackColor = Color.FromArgb(241, 245, 249),
                .FlatStyle = FlatStyle.Flat
            }
            clickBtn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225)

            _clickLabel = New Label With {
                .Text = "Counter Klik: 0",
                .Location = New Point(560, 39),
                .AutoSize = True,
                .ForeColor = Color.FromArgb(71, 85, 105)
            }

            AddHandler clickBtn.Click, Sub(s, e)
                _clickCount += 1
                _clickLabel.Text = "Counter Klik: " & _clickCount
            End Sub

            testBar.Controls.AddRange({playgroundLabel, _interactiveBox, clickBtn, _clickLabel})
            mainPanel.Controls.Add(testBar, 0, 2)

            Controls.Add(mainPanel)
        End Sub

        Private Function CreateCard(title As String, buttons As Button()) As Panel
            Dim card As New Panel With {
                .Width = 385,
                .Height = 215,
                .BackColor = Color.White,
                .Margin = New Padding(0, 0, 15, 15),
                .Padding = New Padding(14)
            }

            Dim cardTitle As New Label With {
                .Text = title,
                .Font = New Font("Segoe UI", 10.5F, FontStyle.Bold),
                .ForeColor = Color.FromArgb(30, 41, 59),
                .Dock = DockStyle.Top,
                .Height = 28
            }

            Dim flow As New FlowLayoutPanel With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.TopDown,
                .WrapContents = False,
                .AutoScroll = True
            }

            For Each btn In buttons
                flow.Controls.Add(btn)
            Next

            card.Controls.Add(flow)
            card.Controls.Add(cardTitle)

            AddHandler card.Paint, Sub(s, e)
                Using p As New Pen(Color.FromArgb(226, 232, 240), 1.0F)
                    e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1)
                End Using
            End Sub

            Return card
        End Function

        Private Function CreateButton(text As String, backColor As Color, onClick As EventHandler) As Button
            Dim btn As New Button With {
                .Text = text,
                .Width = 350,
                .Height = 32,
                .Margin = New Padding(0, 3, 0, 3),
                .BackColor = backColor,
                .ForeColor = Color.White,
                .FlatStyle = FlatStyle.Flat,
                .Font = New Font("Segoe UI", 9.0F, FontStyle.Bold),
                .Cursor = Cursors.Hand
            }
            btn.FlatAppearance.BorderSize = 0
            AddHandler btn.Click, onClick
            Return btn
        End Function

    End Class

End Namespace
