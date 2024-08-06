

using System.Reflection;

namespace AppLauncherUpdater
{
    public partial class LauncherForm : Form
    {

        private string extractionPath = "";

        public LauncherForm()
        {
            string executablePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(executablePath) ?? "";
            extractionPath = Path.Combine(executableDirectory, "App/");

            InitializeComponent();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            CheckForUpdatesAsync();

        }

    }


}