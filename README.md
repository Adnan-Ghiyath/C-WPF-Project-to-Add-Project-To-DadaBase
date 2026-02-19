# 🖥️ Add Projects — WPF Desktop Admin Tool

A Windows desktop application built with **C# + WPF** that allows you to add new projects directly to your **Supabase** database — the same database used by the Portfolio website.

---

## ✨ Features

- 🖼️ **Image Picker** — Browse and select a project image from your computer
- 💾 **Image Copy** — Copies the selected image to your chosen destination automatically
- 🌐 **HTML File Picker** — Select a local HTML file as the web preview link
- 🔗 **GitHub Link** — Add a GitHub repository URL for the source code button
- 🏷️ **Tech Tags** — Add up to 4 technology tags per project
- ⭐ **Featured Toggle** — Mark projects as featured with a checkbox
- ☁️ **Supabase Upload** — Posts project data directly to Supabase via REST API
- 🔄 **Auto Reset** — Clears the form automatically after successful submission
- 🔒 **UI Lock** — Disables all inputs during submission to prevent double-clicks

---

## 🚀 Requirements

- Windows OS
- [.NET 8 or later](https://dotnet.microsoft.com/download)
- Visual Studio 2022+
- A Supabase project with a `Projects` table

---

## 🛠️ How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/Adnan-Ghiyath/Certificate-generator.git
   ```
2. Open `Add_Projects_IAdnan.sln` in **Visual Studio**
3. Update the Supabase API key in `MainWindow.xaml.cs`:
   ```csharp
   string apiKey = "your_supabase_api_key";
   ```
4. Build and run the project (`F5`)

---

## 📁 Project Structure

```
Add_Projects_IAdnan.sln       # Visual Studio solution file
Add_Projects_IAdnan.csproj    # Project configuration
App.xaml / App.xaml.cs        # Application entry point
MainWindow.xaml               # UI layout (XAML)
MainWindow.xaml.cs            # All logic (C#)
AssemblyInfo.cs               # Assembly metadata
```

---

## ⚙️ How It Works

```
User fills the form
      ↓
Select image → OpenFileDialog → copy to destination → get relative path
Select HTML file → OpenFileDialog → convert to relative path
      ↓
Click "Add Project"
      ↓
Serialize data to JSON
      ↓
POST to Supabase REST API → /rest/v1/Projects
      ↓
Success → show confirmation → reset form
Failure → show error message → re-enable form
```

**Key C# concepts used:**
- `HttpClient` with async/await for API calls
- `OpenFileDialog` / `SaveFileDialog` from `Microsoft.Win32`
- `JsonSerializer` from `System.Text.Json`
- `BitmapImage` for image preview in WPF
- `StringBuilder` for path processing

---

## 🗄️ Supabase Table Structure

| Column | Type | Description |
|--------|------|-------------|
| `Name` | text | Project name |
| `Title` | text | Project description |
| `SubTitle` | json | Array of up to 4 tech tags |
| `Img` | text | Relative image path |
| `Wep_PreviewBTN` | text | Relative HTML preview path |
| `Git_PreviewBTN` | text | GitHub repository URL |
| `is_featured` | boolean | Featured project flag |

---

## 🔗 Related Projects

This tool works together with the **Portfolio Website** — data added here appears automatically on the portfolio page.

---

## ⚠️ Security Note

> The Supabase API key is currently hardcoded in `MainWindow.xaml.cs`. Since this is a **private desktop tool**, this is acceptable — but consider moving it to a config file or environment variable if you share the source code publicly.

---

## 🛠️ Built With

![CSharp](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-.NET-512BD4?style=flat&logo=dotnet&logoColor=white)
![Supabase](https://img.shields.io/badge/Supabase-3ECF8E?style=flat&logo=supabase&logoColor=white)

- C# + WPF (.NET)
- Supabase REST API
- `System.Text.Json` for serialization
- `System.Net.Http.HttpClient` for API calls

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).

---

> Made with ❤️ by [Adnan-Ghiyath](https://github.com/Adnan-Ghiyath)
