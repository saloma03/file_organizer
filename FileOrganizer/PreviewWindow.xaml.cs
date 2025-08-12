using System.Collections.ObjectModel;
using System.Windows;
using FileOrganizer.Core;  // يجب إضافة هذه المكتبة لتمكين ObservableCollection

namespace FileOrganizer
{
    public partial class PreviewWindow : Window
    {
        private string _folderPath;
        private FilesOrganizer organizer; 

        private ObservableCollection<string> _simulationLogs;
        public bool UserConfirmed { get; private set; }

        public PreviewWindow(string folderPath, ObservableCollection<string> simulationLogs, FilesOrganizer organizer)
        {
            try
            {
                InitializeComponent();
                _folderPath = folderPath;
                _simulationLogs = simulationLogs;
                DisplaySimulationLogs();
                this.organizer = organizer; // Store the organizer instance

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing PreviewWindow: {ex.InnerException?.Message ?? ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void DisplaySimulationLogs()
        {
            simulationLogListBox.ItemsSource = _simulationLogs; // ربط الـ ObservableCollection بـ ListBox
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            // تنفيذ العملية الفعلية بعد تأكيد المستخدم
            organizer.StartOrganization(_folderPath, false); // تشغيل التنظيم الفعلي

            MessageBox.Show("تم تنظيم الملفات بنجاح!", "نجاح", MessageBoxButton.OK, MessageBoxImage.Information);
            UserConfirmed = true; // تأكيد العملية
            this.Close(); // إغلاق شاشة المعاينة
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // إذا اختار المستخدم الرجوع، لا يتم تنفيذ العملية
            UserConfirmed = false;
            this.Close(); // إغلاق شاشة المعاينة
        }
    }
}
