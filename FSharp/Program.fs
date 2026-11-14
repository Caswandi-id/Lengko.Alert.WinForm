namespace Lengko.Alert.Demo.FSharp

open System
open System.Drawing
open System.Windows.Forms
open Lengko.Alert.WinForms

module Program =

    let createButton text (bg: Color) onClick =
        let btn = new Button(
            Text = text,
            BackColor = bg,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9.0f, FontStyle.Bold),
            Size = Size(180, 38),
            Cursor = Cursors.Hand,
            Margin = Padding(4)
        )
        btn.FlatAppearance.BorderSize <- 0
        btn.Click.Add(fun _ -> onClick ())
        btn

    let createCard title (controls: Control seq) =
        let group = new GroupBox(
            Text = title,
            Font = new Font("Segoe UI", 10.0f, FontStyle.Bold),
            ForeColor = Color.FromArgb(51, 65, 85),
            Size = Size(390, 200),
            Margin = Padding(8),
            Padding = Padding(12)
        )
        let flow = new FlowLayoutPanel(
            Dock = DockStyle.Fill,
            AutoScroll = true,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        )
        for c in controls do
            flow.Controls.Add(c)
        group.Controls.Add(flow)
        group

    [<STAThread>]
    [<EntryPoint>]
    let main _ =
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(false)

        let form = new Form(
            Text = "Lengko.Alert.WinForms — F# Demo",
            Width = 860,
            Height = 650,
            StartPosition = FormStartPosition.CenterScreen,
            Font = new Font("Segoe UI", 9.5f),
            BackColor = Color.FromArgb(248, 250, 252)
        )

        let mainPanel = new TableLayoutPanel(
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = Padding(20)
        )
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70.0f)) |> ignore
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f)) |> ignore
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80.0f)) |> ignore

        // Header
        let header = new Panel(Dock = DockStyle.Fill)
        let titleLabel = new Label(
            Text = "Lengko.Alert Demo (F# WinForms)",
            Font = new Font("Segoe UI", 16.0f, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42),
            AutoSize = true,
            Location = Point(0, 5)
        )
        let subLabel = new Label(
            Text = "Zero external dependencies. Pure WinForms + GDI+ alerts, toasts, and prompts.",
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(100, 116, 139),
            AutoSize = true,
            Location = Point(0, 36)
        )
        header.Controls.Add(titleLabel)
        header.Controls.Add(subLabel)
        mainPanel.Controls.Add(header, 0, 0)

        // Content
        let contentFlow = new FlowLayoutPanel(
            Dock = DockStyle.Fill,
            AutoScroll = true,
            WrapContents = true,
            FlowDirection = FlowDirection.LeftToRight
        )

        // 1. Basic Alerts
        contentFlow.Controls.Add(createCard "1. Basic Modal Alerts" [
            createButton "Success Alert" (Color.FromArgb(34, 197, 94)) (fun () ->
                LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success) |> ignore)
            createButton "Error Alert" (Color.FromArgb(239, 68, 68)) (fun () ->
                LengkoAlert.Fire("Oops...", "Terjadi kesalahan saat memproses data.", AlertIcon.Error) |> ignore)
            createButton "Warning Alert" (Color.FromArgb(245, 158, 11)) (fun () ->
                LengkoAlert.Fire("Peringatan!", "Sesi login Anda akan segera berakhir.", AlertIcon.Warning) |> ignore)
            createButton "Info Alert" (Color.FromArgb(59, 130, 246)) (fun () ->
                LengkoAlert.Fire("Informasi", "Lengko.Alert berjalan tanpa dependensi eksternal.", AlertIcon.Info) |> ignore)
            createButton "Question Alert" (Color.FromArgb(139, 92, 246)) (fun () ->
                LengkoAlert.Fire("Pertanyaan", "Apakah Anda ingin melanjutkan proses ini?", AlertIcon.Question) |> ignore)
        ])

        // 2. Confirm & Prompt
        contentFlow.Controls.Add(createCard "2. Confirm & Prompt" [
            createButton "Confirm Dialog" (Color.FromArgb(79, 70, 229)) (fun () ->
                let ok = LengkoAlert.Confirm("Konfirmasi Hapus", "Yakin ingin menghapus record ini?", "Ya, Hapus!", "Batal", AlertIcon.Warning)
                if ok then
                    LengkoAlert.Fire("Terhapus!", "Record berhasil dihapus.", AlertIcon.Success) |> ignore)
            createButton "Prompt Input" (Color.FromArgb(79, 70, 229)) (fun () ->
                let value = LengkoAlert.Prompt("Input Data", "Masukkan nama Anda:", "Nama...")
                if not (String.IsNullOrEmpty(value)) then
                    LengkoAlert.Fire("Halo!", sprintf "Selamat datang, %s!" value, AlertIcon.Success) |> ignore)
        ])

        // 3. Toasts
        contentFlow.Controls.Add(createCard "3. Corner Toast Notifications" [
            createButton "Toast Success" (Color.FromArgb(16, 185, 129)) (fun () ->
                LengkoAlert.ShowToast("Data berhasil disinkronisasi!", AlertIcon.Success, 3000))
            createButton "Toast Error" (Color.FromArgb(239, 68, 68)) (fun () ->
                LengkoAlert.ShowToast("Koneksi jaringan terputus.", AlertIcon.Error, 3000))
            createButton "Toast Info" (Color.FromArgb(59, 130, 246)) (fun () ->
                LengkoAlert.ShowToast("Pembaruan sistem tersedia.", AlertIcon.Info, 3000))
            createButton "Toast Bottom-End" (Color.FromArgb(100, 116, 139)) (fun () ->
                let opt = new AlertOptions(
                    Title = "Download selesai",
                    Icon = AlertIcon.Success,
                    Toast = true,
                    Position = AlertPosition.BottomEnd,
                    Timer = 3500
                )
                LengkoAlert.ShowToast(opt))
        ])

        // 4. Advanced & Custom
        contentFlow.Controls.Add(createCard "4. Advanced & Options" [
            createButton "Auto-Close Timer" (Color.FromArgb(234, 88, 12)) (fun () ->
                let opt = new AlertOptions(
                    Title = "Auto Close Alert",
                    Text = "Dialog ini akan otomatis tertutup dalam 3 detik.",
                    Icon = AlertIcon.Info,
                    Timer = 3000,
                    TimerProgressBar = true
                )
                LengkoAlert.Fire(opt) |> ignore)
            createButton "Custom Colors" (Color.FromArgb(20, 184, 166)) (fun () ->
                let opt = new AlertOptions(
                    Title = "Custom Theming",
                    Text = "Warna tombol dan background dapat dikustomisasi bebas.",
                    Icon = AlertIcon.Question,
                    ConfirmButtonText = "Keren!",
                    ConfirmButtonColor = Color.FromArgb(20, 184, 166),
                    ShowCancelButton = true,
                    CancelButtonText = "Tutup",
                    CancelButtonColor = Color.FromArgb(100, 116, 139)
                )
                LengkoAlert.Fire(opt) |> ignore)
        ])

        mainPanel.Controls.Add(contentFlow, 0, 1)

        // Footer
        let footer = new Panel(Dock = DockStyle.Fill)
        let noteLabel = new Label(
            Text = "Lengko.Alert.WinForms - Pure WinForms GDI+ Dialogs with C#, VB.NET, and F# support.",
            ForeColor = Color.FromArgb(148, 163, 184),
            AutoSize = true,
            Location = Point(0, 20)
        )
        footer.Controls.Add(noteLabel)
        mainPanel.Controls.Add(footer, 0, 2)

        form.Controls.Add(mainPanel)
        Application.Run(form)
        0
