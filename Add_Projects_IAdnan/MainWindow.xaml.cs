using Microsoft.Win32;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Add_Projects_IAdnan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string? selectedImagePath;
        private string? _newPath;
        private string? _selectedHtmlPath;
        private string? _newHtmlPath;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ImageBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select Project Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    selectedImagePath = openFileDialog.FileName;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedImagePath);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // مهم لتحرير الملف بعد تحميله
                    bitmap.EndInit();

                    // هنا يتم ربط الصورة
                    imgProject.Source = bitmap;
                    // إخفاء النص التوضيحي
                    txtPlaceholder.Visibility = Visibility.Collapsed;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }
            MoveImageAndGetPath(selectedImagePath);
        }
        public string? MoveImageAndGetPath(string originalFilePath)
        {
            // التحقق من وجود الملف
            if (!System.IO.File.Exists(originalFilePath)) return null;

            // إعداد نافذة حفظ الملف
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();

            // استخدام System.IO.Path بشكل صريح لمنع التضارب
            string extension = System.IO.Path.GetExtension(originalFilePath);
            saveFileDialog.Filter = $"Image Files (*{extension})|*{extension}";
            saveFileDialog.FileName = System.IO.Path.GetFileName(originalFilePath);

            // إظهار النافذة
            if (saveFileDialog.ShowDialog() == true)
            {
                string newPath = saveFileDialog.FileName;
                try
                {
                    // نقل الملف
                    System.IO.File.Copy(originalFilePath, newPath);
                    _newPath= newPath;

                    return _newPath = ProcessPath(ref _newPath);
                }
                catch (Exception) // حذفنا ex لأننا لا نستخدمه، لمنع التحذير
                {
                    // يمكنك هنا إظهار رسالة خطأ إذا أردت
                    // MessageBox.Show("حدث خطأ أثناء النقل");
                    return null;
                }
            }

            return null;
        }

        private void SelectHtmlFile_Click(object sender, RoutedEventArgs e)
        {
            // 1. إنشاء نافذة اختيار الملفات
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "اختر ملف HTML",
                // 2. تحديد نوع الملفات لتظهر ملفات الويب فقط
                Filter = "HTML Files (*.html;*.htm)|*.html;*.htm|All Files (*.*)|*.*",
                InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
            };

            // 3. إظهار النافذة والتأكد أن المستخدم ضغط "Open"
            if (openFileDialog.ShowDialog() == true)
            {
                // 4. جلب المسار الكامل ووضعه في الـ TextBox
                string selectedFilePath = openFileDialog.FileName;
                _selectedHtmlPath = selectedFilePath;
            }
            _selectedHtmlPath   = ProcessPath(ref _selectedHtmlPath);
            txtWebPreview.Text=_selectedHtmlPath;
        }
        private string ProcessPath(ref string ?input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            // 1. إنشاء الـ StringBuilder وتعبئته بالنص الأصلي
            StringBuilder sb = new StringBuilder(input);

            // 2. البحث عن موقع أول رقم '0' في النص
            int index = -1;
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '0')
                {
                    index = i;
                    break;
                }
            }

            // 3. إذا وجدنا الرقم 0، نقوم بحذف كل ما قبله
            if (index != -1)
            {
                // نحذف من البداية (index 0) بطول (index)
                sb.Remove(0, index);
            }

            // 4. إضافة الكلمة (../) في بداية النص الجديد
            sb.Insert(0, "../");

            // 5. تحويل الـ StringBuilder إلى نص عادي (string) في النهاية
            return sb.ToString();
        }
        private void Reast()
        {
            txtName.Text = "";
            txtTitle1.Text = "";
            txtSubTitle1.Text = "";
            txtSubTitle2.Text = "";
            txtSubTitle3.Text = "";
            txtSubTitle4.Text = "";
            selectedImagePath = "";
            txtWebPreview.Text = "";
            txtGitPreview.Text = "";
            chkFeatured.IsChecked = false;
        }

        private void onClick()
        {
            txtName.IsEnabled = false;
            txtTitle1.IsEnabled = false;
            txtSubTitle1.IsEnabled = false;
            txtSubTitle2.IsEnabled = false;
            txtSubTitle3.IsEnabled = false;
            txtSubTitle4.IsEnabled = false;
            txtWebPreview.IsEnabled = false;
            txtGitPreview.IsEnabled = false;
            chkFeatured.IsEnabled = false;
            MainBTN.IsEnabled = false;
        }
        private void AfteronClick()
        {
            Reast();
            txtName.IsEnabled = true;
            txtTitle1.IsEnabled = true;
            txtSubTitle1.IsEnabled = true;
            txtSubTitle2.IsEnabled = true;
            txtSubTitle3.IsEnabled = true;
            txtSubTitle4.IsEnabled = true;
              txtWebPreview.IsEnabled = true;
            txtGitPreview.IsEnabled = true;
            chkFeatured.IsEnabled = true;
            MainBTN.IsEnabled = true;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            onClick();
           
            try
            {
                // بيانات المشروع
                var project = new
                {
                    Name = txtName.Text,
                    Title = txtTitle1.Text,
                    SubTitle = new[] { txtSubTitle1.Text, txtSubTitle2.Text, txtSubTitle3.Text, txtSubTitle4.Text },
                    Img = _newPath.ToString(),
                    Wep_PreviewBTN = _selectedHtmlPath.ToString(),
                    Git_PreviewBTN = txtGitPreview.Text,
                    is_featured = chkFeatured.IsChecked ?? false
            };

                string json = JsonSerializer.Serialize(project);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    // الطريقة الصحيحة لإضافة Headers
                    string apiKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJoeHFod2pyc3Ztd2JtcGpreHdsIiwicm9sZSI6ImFub24iLCJpYXQiOjE3Njk0NDQ3NDgsImV4cCI6MjA4NTAyMDc0OH0.F9EyFL9BKFolQeGF7GoOoKW7S6nuBJVMZ5P8sQl-K-w";

                    client.DefaultRequestHeaders.Add("apikey", apiKey);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                    client.DefaultRequestHeaders.Add("Prefer", "return=representation");

                    var response = await client.PostAsync(
                        "https://rhxqhwjrsvmwbmpjkxwl.supabase.co/rest/v1/Projects",
                        content
                    );

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Project added successfully!\n{responseBody}", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        AfteronClick();
                    }
                    else
                    {
                        AfteronClick();
                        MessageBox.Show($"Failed: {response.StatusCode}\n\nDetails:\n{responseBody}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                AfteronClick();
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}