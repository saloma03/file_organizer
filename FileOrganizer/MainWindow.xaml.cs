using System.Windows;
using FileOrganizer.Core;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.IO.Abstractions; // تأكد من إضافة هذا لاستخدام FolderBrowserDialog

namespace FileOrganizer
{
    public partial class MainWindow : Window
    {
        private FilesOrganizer fileOrganizer;

        public MainWindow()
        {
            InitializeComponent();
            IFileSystem realFileSystem = new FileSystem();

            var realFileManager = new FileManager(realFileSystem);
            var realSettingManager = new SettingManager(); // افترض أنه لا يعتمد على شيء آخر
            var realUndoManager = new UndoManager();       // افترض أنه لا يعتمد على شيء آخر

            // 3. أنشئ الكائن الأعلى مستوى وقم بحقن التبعيات التي أنشأتها للتو
            fileOrganizer = new FilesOrganizer(realFileManager, realSettingManager, realUndoManager);
        }

        private void OrganizeButton_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = FolderPathTextBox.Text.Trim();

            // التحقق من وجود المسار
            if (string.IsNullOrEmpty(folderPath))
            {
                StatusText.Text = "Please select a folder to organize.";
                return;
            }

            if (!System.IO.Directory.Exists(folderPath))
            {
                StatusText.Text = "The selected folder does not exist.";
                return;
            }

            try
            {
                // مسح السجلات القديمة
                FileOrganizer.Logging.ActionLogger.ClearLogs();

                // تشغيل المحاكاة
                fileOrganizer.StartOrganization(folderPath, simulate: true);

                // جمع السجلات التي تم محاكاتها
                ObservableCollection<string> simulationLogs = Logging.ActionLogger.Logs;

                // عرض نافذة المعاينة مع السجلات
                var previewWindow = new PreviewWindow(folderPath, simulationLogs , fileOrganizer);
                previewWindow.Owner = this;

                this.Hide(); // إخفاء النافذة الرئيسية
                previewWindow.ShowDialog(); // عرض نافذة المعاينة كـ modal
                this.Show(); // إعادة إظهار النافذة الرئيسية

                // تحديث حالة البرنامج بعد المعاينة
                StatusText.Text = previewWindow.UserConfirmed
                    ? "Organization completed successfully!"
                    : "Preview completed. No changes were made.";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error: {ex.Message}";
            }
        }

        private void UndoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fileOrganizer.UndoLastOperation();
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Undo failed: {ex.Message}";
            }
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "Select Folder to Organize";
                    folderDialog.ShowNewFolderButton = true;

                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        FolderPathTextBox.Text = folderDialog.SelectedPath;
                        StatusText.Text = $"Selected folder: {folderDialog.SelectedPath}";
                    }
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Error selecting folder: {ex.Message}";
            }
        }


        // Browse Button Click Handler
    }
}
