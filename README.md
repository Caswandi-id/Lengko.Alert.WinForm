<p align="center">
  <img src="logo.png" alt="Lengko Alert WinForms Logo" width="140" />
</p>

# Lengko.Alert.WinForms — Demos & Samples

Modern, high-performance alert dialogs, modal prompts, and toast notifications for **Windows Forms** with zero external dependencies. Pure WinForms + GDI+ double-buffered rendering.

[![Platform](https://img.shields.io/badge/Platform-Windows%20Forms-brightgreen.svg)]()
[![Languages](https://img.shields.io/badge/Languages-C%23%20%7C%20VB.NET%20%7C%20F%23-blue.svg)]()
[![Target](https://img.shields.io/badge/.NET-8.0--windows-512BD4.svg)]()
[![License](https://img.shields.io/badge/License-Freeware-orange.svg)](LICENSE.txt)

---


## Projects in this Folder

| Language | Project File | Description |
|---|---|---|
| **C#** | `CSharp/Lengko.Alert.Demo.CSharp.csproj` | Comprehensive C# showcase covering Success, Error, Warning, Info, Confirm, Prompt, and multi-position toasts. |
| **VB.NET** | `VB/Lengko.Alert.Demo.VB.vbproj` | Full-featured VB.NET WinForms demo demonstrating modal dialogs and asynchronous alerts. |
| **F#** | `FSharp/Lengko.Alert.Demo.FSharp.fsproj` | Functional F# WinForms demo with dialog flows and toast notifications. |

Dedicated solution: **`Lengko.Alert.WinForms.sln`** (builds all 3 demos alongside the library).

---
---

## Installation (NuGet)

[![NuGet](https://img.shields.io/nuget/v/Lengko.Alert.WinForm.svg)](https://www.nuget.org/packages/Lengko.Alert.WinForm)
[![Downloads](https://img.shields.io/nuget/dt/Lengko.Alert.WinForm.svg)](https://www.nuget.org/packages/Lengko.Alert.WinForm)

Package: [Lengko.Alert.WinForm](https://www.nuget.org/packages/Lengko.Alert.WinForm) on NuGet.org.

### .NET CLI

```powershell
dotnet add package Lengko.Alert.WinForm
```

### Package Manager Console (Visual Studio)

```powershell
Install-Package Lengko.Alert.WinForm
```

### PackageReference (.csproj / .vbproj / .fsproj)

```xml
<ItemGroup>
  <PackageReference Include="Lengko.Alert.WinForm" Version="1.0.0" />
</ItemGroup>
```

### Visual Studio GUI

1. Right-click the project, then select **Manage NuGet Packages...**
2. Open the **Browse** tab and search for `Lengko.Alert.WinForm`.
3. Click **Install**.

Supported frameworks: `net472`, `net48`, `netcoreapp3.1`, `net5.0-windows` to `net10.0-windows`.

---


## Quick Start / How to Run

### Via Visual Studio
Open `Lengko.Alert.WinForms.sln`, select any demo project as the Startup Project, and press `F5`.

### Via .NET CLI

```powershell
# Run C# Demo
dotnet run --project CSharp/Lengko.Alert.Demo.CSharp.csproj

# Run VB.NET Demo
dotnet run --project VB/Lengko.Alert.Demo.VB.vbproj

# Run F# Demo
dotnet run --project FSharp/Lengko.Alert.Demo.FSharp.fsproj
```

---

## Code Examples

### C#
```csharp
using Lengko.Alert.WinForms;

// 1. Success Alert
LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success);

// 2. Confirmation Dialog
bool confirmed = LengkoAlert.Confirm("Hapus Data", "Yakin ingin menghapus record ini?", "Ya, Hapus", "Batal", AlertIcon.Warning);
if (confirmed)
{
    LengkoAlert.ShowToast("Data berhasil dihapus!", AlertIcon.Success);
}

// 3. Quick Corner Toast
LengkoAlert.ShowToast("Pembaruan sistem tersedia.", AlertIcon.Info, timerMs: 3000);
```

### VB.NET
```vb
Imports Lengko.Alert.WinForms

' 1. Success Alert
LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success)

' 2. Confirmation Dialog
Dim confirmed As Boolean = LengkoAlert.Confirm("Hapus Data", "Yakin ingin menghapus?", "Ya, Hapus", "Batal", AlertIcon.Warning)
If confirmed Then
    LengkoAlert.ShowToast("Data berhasil dihapus!", AlertIcon.Success)
End If

' 3. Quick Corner Toast
LengkoAlert.ShowToast("Pembaruan sistem tersedia.", AlertIcon.Info, 3000)
```

### F#
```fsharp
open Lengko.Alert.WinForms

// 1. Success Alert
LengkoAlert.Fire("Berhasil!", "Data tersimpan dengan sukses.", AlertIcon.Success) |> ignore

// 2. Confirmation Dialog
let confirmed = LengkoAlert.Confirm("Hapus Data", "Yakin ingin menghapus?", "Ya, Hapus", "Batal", AlertIcon.Warning)
if confirmed then
    LengkoAlert.ShowToast("Data berhasil dihapus!", AlertIcon.Success, 3000)

// 3. Quick Corner Toast
LengkoAlert.ShowToast("Pembaruan sistem tersedia.", AlertIcon.Info, 3000)
```

<img width="877" height="456" alt="image" src="https://github.com/user-attachments/assets/b8821355-877c-473e-b4bd-2d117e7c2551" />
<img width="421" height="234" alt="image" src="https://github.com/user-attachments/assets/9d538859-b533-4141-b1e8-393248bfd0d0" />



---

## Features
- **Modal Alerts**: `Success`, `Error`, `Warning`, `Info`, `Question` with smooth drop shadows.
- **Confirm & Prompt**: Synchronous & asynchronous confirmation modals and user text prompts.
- **Toasts**: Non-intrusive floating toasts with auto-close timers and customizable screen positions (`TopEnd`, `BottomEnd`, `TopStart`, `BottomStart`, etc.).
- **Timers**: Animated progress bar countdown indicator on auto-closing dialogs.
- **Custom Theming**: Customizable background colors, confirm/cancel buttons, icons, and fonts.

---

## License
Proprietary Freeware License (PFL). Free for personal, educational, and commercial use. See [LICENSE.txt](LICENSE.txt).
