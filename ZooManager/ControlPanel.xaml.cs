using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ZooManager
{
    public partial class ControlPanel : Window, INotifyPropertyChanged
    {
        private Users _currentUser;
        private Utilities _utilities;
        private ObservableCollection<Habitat> _habitats;
        private ObservableCollection<Activity> _recentActivities;
        private Habitat _selectedHabitat;

        public ObservableCollection<Habitat> Habitats
        {
            get => _habitats;
            set
            {
                _habitats = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Activity> RecentActivities
        {
            get => _recentActivities;
            set
            {
                _recentActivities = value;
                OnPropertyChanged();
            }
        }

        public Habitat SelectedHabitat
        {
            get => _selectedHabitat;
            set
            {
                _selectedHabitat = value;
                OnPropertyChanged();
                LoadActivitiesForSelectedHabitat();
            }
        }

        public ControlPanel(Users currentUser, Utilities utilities)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _utilities = utilities;

            DataContext = this;
            InitializeUI();
            LoadData();
        }

        private void InitializeUI()
        {
            CurrentUserTextBlock.Text = _currentUser.Username;
            UserRoleTextBlock.Text = _currentUser.IsAdmin ? "Admin" : "User";

            // Hide Admin Panel button for non-admin users
            AdminPanelButton.Visibility = _currentUser.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void LoadData()
        {
            try
            {
                var habitats = await _utilities.GetAllHabitatsAsync();
                Habitats = new ObservableCollection<Habitat>(habitats);

                // Load recent activities (last 10)
                var allActivities = new List<Activity>();
                foreach (var habitat in habitats)
                {
                    var activities = await _utilities.GetActivitiesByHabitatAsync(habitat.Id);
                    allActivities.AddRange(activities.Take(5)); // Take 5 from each habitat
                }

                RecentActivities = new ObservableCollection<Activity>(
                    allActivities.OrderByDescending(a => a.PerformedAt).Take(10)
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private async void LoadActivitiesForSelectedHabitat()
        {
            if (SelectedHabitat != null)
            {
                try
                {
                    var activities = await _utilities.GetActivitiesByHabitatAsync(SelectedHabitat.Id);
                    RecentActivities = new ObservableCollection<Activity>(activities.Take(10));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading activities: {ex.Message}");
                }
            }
        }

        private void AdminPanelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser.IsAdmin)
            {
                AdminPanel adminPanel = new AdminPanel(_currentUser, _utilities);
                adminPanel.ShowDialog(); // Use ShowDialog to keep this window open
                LoadData(); // Refresh data when admin panel closes
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
