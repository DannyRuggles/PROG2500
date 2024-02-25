using System.Diagnostics;
using System.Windows;
using Microsoft.Win32; // Add this for OpenFileDialog

namespace GitConfigEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DisplayCurrentGitConfig();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var scope = chkGlobal.IsChecked == true ? "--global" : "";
            SetGitConfig("user.name", txtUsername.Text, scope);
            SetGitConfig("user.email", txtEmail.Text, scope);
        }

        private void SetGitConfig(string key, string value, string scope)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                Arguments = $"config {scope} {key} \"{value}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();
            proc.WaitForExit();
        }

        private string GetGitConfig(string key, string scope)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                Arguments = $"config {scope} --get {key}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();
            string result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return result.Trim();
        }

        private void DisplayCurrentGitConfig()
        {
            var scope = chkGlobal.IsChecked == true ? "--global" : "";
            txtUsername.Text = GetGitConfig("user.name", scope);
            txtEmail.Text = GetGitConfig("user.email", scope);
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            DisplayCurrentGitConfig();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Use OpenFileDialog to select a folder indirectly
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select a folder and close this dialog",
                Filter = "All files (*.*)|*.*",
                Title = "Select a folder"
            };

            if (dialog.ShowDialog() == true)
            {
                // Set the selected folder path to the TextBox
                txtRepositoryPath.Text = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
