using System.Threading.Tasks;
using System.Windows;

namespace Devoteam_ServiceNow_Migration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FilesInitialization filesinit;

        public MainWindow()
        {
            InitializeComponent();
            filesinit = new FilesInitialization(); // Initialize the class-level field
            filesinit.Initialization();
            ProgressBar.Visibility = Visibility.Hidden;
        }

        private async void MigrateButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide the Migrate button and show the ProgressBar
            MigrateButton.Visibility = Visibility.Hidden;
            ProgressBar.Visibility = Visibility.Visible;
    
            string internetAdress = InternetAddressTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            // Start the time-consuming operation on a background thread
            await Task.Run(() =>
            {
                foreach (var VARIABLE in filesinit.files)
                {
                    if (VARIABLE.param != "")
                    {
                        Import.DownloadFile(internetAdress + VARIABLE.name + "_list.do?XML&" + VARIABLE.param,
                            VARIABLE.folder + "/" + VARIABLE.name+".xml", username, password);
                    }
                    else
                    {
                        Import.DownloadFile(internetAdress + VARIABLE.name + "_list.do?XML", VARIABLE.folder + "/" + VARIABLE.name+".xml",
                            username, password);
                    }

                    // Report progress to the UI
                    Dispatcher.Invoke(() =>
                    {
                        ProgressBar.Value += (1.0 / filesinit.files.Count)*100;
                    });
                }
            });

            // Show a message when the operation is complete
            MessageBox.Show("Files downloaded!", "Download Status", MessageBoxButton.OK, MessageBoxImage.Information);

            // Reset ProgressBar and show the Migrate button
            ProgressBar.Value = 0;
            ProgressBar.Visibility = Visibility.Hidden;
            MigrateButton.Visibility = Visibility.Visible;
        }
    }
}