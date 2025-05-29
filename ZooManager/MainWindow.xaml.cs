using System.Windows;

namespace ZooManager
{
    public partial class MainWindow : Window
    {
        private Utilities _utilities = new Utilities();
        private Users _currentUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            _currentUser = _utilities.ValidateLogin(username, password);

            if (_currentUser != null)
            {
                ControlPanel controlPanel = new ControlPanel(_currentUser, _utilities);
                controlPanel.Show();
                this.Hide();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Invalid username or password.";
                UsernameTextBox.Clear();
                PasswordBox.Clear();
            }
        }
    }
}
