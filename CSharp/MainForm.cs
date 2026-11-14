using System;
using System.Drawing;
using System.Windows.Forms;
using Lengko.Alert.WinForms;

namespace Lengko.Alert.Demo.CSharp;

public class MainForm : Form
{
    private TextBox _interactiveBox = null!;
    private int _clickCount = 0;
    private Label _clickLabel = null!;

    public MainForm()
    {
        InitializeUi();
    }

    private void InitializeUi()
    {
        Text = "Lengko.Alert.WinForms — C# Demo";
        Width = 860;
        Height = 650;
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.FromArgb(248, 250, 252);

        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(20)
        };
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

        // Header
        var headerPanel = new Panel { Dock = DockStyle.Fill };
        var titleLabel = new Label
        {
            Text = "Lengko.Alert Demo (C# WinForms)",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = new Point(0, 5)
        };
        var subLabel = new Label
        {
            Text = "Zero external dependencies. Pure WinForms + GDI+ alerts, toasts, and prompts.",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = new Point(0, 36)
        };
        headerPanel.Controls.Add(titleLabel);
        headerPanel.Controls.Add(subLabel);
        mainPanel.Controls.Add(headerPanel, 0, 0);

        // Content (Flow of Cards)
        var contentFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        };

        // Section 1: Basic Alerts
        contentFlow.Controls.Add(CreateCard("1. Basic Modal Alerts", new Control[]
        {
            CreateButton("Success Alert", Color.FromArgb(34, 197, 94), (_, _) =>
                LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success)),

            CreateButton("Error Alert", Color.FromArgb(239, 68, 68), (_, _) =>
                LengkoAlert.Fire("Oops...", "Terjadi kesalahan saat memproses data.", AlertIcon.Error)),

            CreateButton("Warning Alert", Color.FromArgb(245, 158, 11), (_, _) =>
                LengkoAlert.Fire("Peringatan!", "Sesi login Anda akan segera berakhir.", AlertIcon.Warning)),

            CreateButton("Info Alert", Color.FromArgb(59, 130, 246), (_, _) =>
                LengkoAlert.Fire("Informasi", "Lengko.Alert berjalan tanpa dependensi eksternal.", AlertIcon.Info)),

            CreateButton("Question Alert", Color.FromArgb(139, 92, 246), (_, _) =>
                LengkoAlert.Fire("Pertanyaan", "Apakah Anda ingin melanjutkan proses ini?", AlertIcon.Question))
        }));

        // Section 2: Confirm & Prompt
        contentFlow.Controls.Add(CreateCard("2. Confirm & Prompt", new Control[]
        {
            CreateButton("Confirm (Sync)", Color.FromArgb(79, 70, 229), (_, _) =>
            {
                bool ok = LengkoAlert.Confirm("Konfirmasi Hapus", "Yakin ingin menghapus record ini?", "Ya, Hapus!", "Batal", AlertIcon.Warning);
                if (ok)
                    LengkoAlert.Fire("Terhapus!", "Record berhasil dihapus.", AlertIcon.Success);
            }),

            CreateButton("Confirm (Async)", Color.FromArgb(79, 70, 229), async (_, _) =>
            {
                bool ok = await LengkoAlert.ConfirmAsync("Konfirmasi Simpan", "Simpan semua perubahan ke database?", "Simpan", "Batal", AlertIcon.Question);
                if (ok)
                    await LengkoAlert.FireAsync("Tersimpan!", "Data perubahan telah diperbarui.", AlertIcon.Success);
            }),

            CreateButton("Prompt Text", Color.FromArgb(14, 165, 233), (_, _) =>
            {
                string? name = LengkoAlert.Prompt("Input Nama", "Masukkan nama lengkap Anda:", "Contoh: Budi Santoso");
                if (!string.IsNullOrWhiteSpace(name))
                    LengkoAlert.Fire("Halo!", $"Selamat datang, {name}!", AlertIcon.Info);
            }),

            CreateButton("Prompt with Validator", Color.FromArgb(14, 165, 233), async (_, _) =>
            {
                var res = await LengkoAlert.FireAsync(new AlertOptions
                {
                    Title = "Input Email",
                    Text = "Format harus mengandung '@'.",
                    Input = AlertInputType.Text,
                    InputPlaceholder = "user@domain.com",
                    ShowCancelButton = true,
                    InputValidator = val =>
                        string.IsNullOrWhiteSpace(val) || !val.Contains("@") ? "Email tidak valid (harus ada @)!" : null
                });
                if (res.IsConfirmed)
                    await LengkoAlert.FireAsync("Email Valid!", $"Email Anda: {res.Value}", AlertIcon.Success);
            })
        }));

        // Section 3: Toast Notifications (Always Non-Modal)
        contentFlow.Controls.Add(CreateCard("3. Toast Notifications (Non-Modal)", new Control[]
        {
            CreateButton("Toast TopEnd (Default)", Color.FromArgb(16, 185, 129), (_, _) =>
                LengkoAlert.ShowToast("Login Berhasil", "Selamat datang kembali!", AlertIcon.Success, 3000)),

            CreateButton("Toast BottomStart", Color.FromArgb(16, 185, 129), (_, _) =>
                LengkoAlert.ShowToast("Data Masuk", "Pojok kiri bawah", AlertPosition.BottomStart, AlertIcon.Info, 3000)),

            CreateButton("Toast BottomEnd", Color.FromArgb(16, 185, 129), (_, _) =>
                LengkoAlert.ShowToast("Disimpan", "Pojok kanan bawah", AlertPosition.BottomEnd, AlertIcon.Success, 3000)),

            CreateButton("Toast Custom Offset (30,30)", Color.FromArgb(16, 185, 129), (_, _) =>
                LengkoAlert.ShowToast(new AlertOptions
                {
                    Title = "Custom Margin",
                    Text = "Toast dengan offset 30px dari tepi layar.",
                    Position = AlertPosition.TopStart,
                    Offset = new Point(30, 30),
                    Icon = AlertIcon.Warning,
                    Timer = 3500
                }))
        }));

        // Section 4: Non-Modal Alert & Features
        contentFlow.Controls.Add(CreateCard("4. Non-Modal Alert & Styling", new Control[]
        {
            CreateButton("Non-Modal Alert (UI Aktif)", Color.FromArgb(225, 29, 72), (_, _) =>
            {
                LengkoAlert.Fire(new AlertOptions
                {
                    Title = "Non-Modal Alert",
                    Text = "Alert melayang! Coba klik tombol bawah atau ketik di text box.",
                    Icon = AlertIcon.Info,
                    Modal = false, // Form utama tetap bisa diklik & digeser
                    Position = AlertPosition.TopEnd,
                    Timer = 7000,
                    TimerProgressBar = true,
                    Owner = this
                });
            }),

            CreateButton("Oval Pill Buttons", Color.FromArgb(100, 116, 139), (_, _) =>
            {
                LengkoAlert.Fire(new AlertOptions
                {
                    Title = "Oval Pill Buttons",
                    Text = "Tombol aksi melengkung penuh berbentuk kapsul/oval.",
                    Icon = AlertIcon.Question,
                    ShowCancelButton = true,
                    OvalButtons = true
                });
            }),

            CreateButton("Bounce Animation", Color.FromArgb(100, 116, 139), (_, _) =>
            {
                LengkoAlert.Fire(new AlertOptions
                {
                    Title = "Animasi Membal (Bounce)",
                    Text = "Alert jatuh membal elastis dari atas layar.",
                    Icon = AlertIcon.Success,
                    OpenAnimation = AlertAnimation.Bounce,
                    CloseAnimation = AlertAnimation.Fade,
                    AnimationDuration = 180
                });
            }),

            CreateButton("Dark Theme", Color.FromArgb(30, 41, 59), (_, _) =>
            {
                LengkoAlert.Fire(new AlertOptions
                {
                    Title = "Dark Theme",
                    Text = "Tampilan dark slate dengan kontras tinggi.",
                    Theme = AlertTheme.Dark,
                    Icon = AlertIcon.Success,
                    ShowCancelButton = true
                });
            })
        }));

        mainPanel.Controls.Add(contentFlow, 0, 1);

        // Section 5: Interactive playground to test Non-Modal behavior
        var testBar = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(15, 10, 15, 10)
        };

        var playgroundLabel = new Label
        {
            Text = "Playground Uji Interaksi UI Utama (Tes Non-Modal):",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85),
            Location = new Point(15, 10),
            AutoSize = true
        };

        _interactiveBox = new TextBox
        {
            Location = new Point(15, 36),
            Width = 320,
            PlaceholderText = "Ketik teks di sini saat alert Non-Modal muncul..."
        };

        var clickBtn = new Button
        {
            Text = "Klik Saya Saat Alert Tampil",
            Location = new Point(345, 34),
            Width = 200,
            Height = 28,
            BackColor = Color.FromArgb(241, 245, 249),
            FlatStyle = FlatStyle.Flat
        };
        clickBtn.FlatAppearance.BorderColor = Color.FromArgb(203, 213, 225);

        _clickLabel = new Label
        {
            Text = "Counter Klik: 0",
            Location = new Point(560, 39),
            AutoSize = true,
            ForeColor = Color.FromArgb(71, 85, 105)
        };

        clickBtn.Click += (_, _) =>
        {
            _clickCount++;
            _clickLabel.Text = $"Counter Klik: {_clickCount}";
        };

        testBar.Controls.AddRange(new Control[] { playgroundLabel, _interactiveBox, clickBtn, _clickLabel });
        mainPanel.Controls.Add(testBar, 0, 2);

        Controls.Add(mainPanel);
    }

    private Panel CreateCard(string title, Control[] buttons)
    {
        var card = new Panel
        {
            Width = 385,
            Height = 215,
            BackColor = Color.White,
            Margin = new Padding(0, 0, 15, 15),
            Padding = new Padding(14)
        };

        var cardTitle = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 41, 59),
            Dock = DockStyle.Top,
            Height = 28
        };

        var flow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true
        };

        foreach (var btn in buttons)
            flow.Controls.Add(btn);

        card.Controls.Add(flow);
        card.Controls.Add(cardTitle);

        card.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1f);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        return card;
    }

    private Button CreateButton(string text, Color backColor, EventHandler onClick)
    {
        var btn = new Button
        {
            Text = text,
            Width = 350,
            Height = 32,
            Margin = new Padding(0, 3, 0, 3),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.Click += onClick;
        return btn;
    }
}
