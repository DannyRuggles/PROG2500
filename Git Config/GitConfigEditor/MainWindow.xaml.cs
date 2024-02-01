using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            SetGitConfig("user.name", txtUsername.Text);
            SetGitConfig("user.email", txtEmail.Text);
        }

        private void SetGitConfig(string key, string value)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                Arguments = $"config --global {key} \"{value}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process proc = new Process() { StartInfo = startInfo };
            proc.Start();
            proc.WaitForExit();
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string GetGitConfig(string key)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("git")
            {
                Arguments = $"config --global --get {key}",
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
            txtUsername.Text = GetGitConfig("user.name");
            txtEmail.Text = GetGitConfig("user.email");
        }
    }
}