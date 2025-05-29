using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ZooManager
{
    public partial class AdminPanel : Window
    {
        private Users _currentUser;
        private Utilities _utilities;

        public AdminPanel(Users currentUser, Utilities utilities)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _utilities = utilities;
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                var users = await _utilities.GetAllUsersAsync();
                UsersDataGrid.ItemsSource = users;

                var habitats = await _utilities.GetAllHabitatsAsync();
                HabitatsDataGrid.ItemsSource = habitats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private async void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NewUsernameTextBox.Text) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            var newUser = new Users
            {
                Username = NewUsernameTextBox.Text,
                Password = NewPasswordBox.Password,
                IsAdmin = IsAdminCheckBox.IsChecked ?? false
            };

            bool success = await _utilities.AddUserAsync(newUser);
            if (success)
            {
                MessageBox.Show("User added successfully!");
                NewUsernameTextBox.Clear();
                NewPasswordBox.Clear();
                IsAdminCheckBox.IsChecked = false;
                LoadData();
            }
            else
            {
                MessageBox.Show("Failed to add user.");
            }
        }

        private async void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is Users selectedUser)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user '{selectedUser.Username}'?",
                                           "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _utilities.DeleteUserAsync(selectedUser.Id);
                    if (success)
                    {
                        MessageBox.Show("User deleted successfully!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete user.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.");
            }
        }

        private async void AddHabitatButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(HabitatNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AnimalTypeTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!int.TryParse(CapacityTextBox.Text, out int capacity) ||
                !int.TryParse(CurrentAnimalsTextBox.Text, out int currentAnimals))
            {
                MessageBox.Show("Please enter valid numbers for capacity and current animals.");
                return;
            }

            var newHabitat = new Habitat(HabitatNameTextBox.Text, AnimalTypeTextBox.Text, capacity, currentAnimals);

            bool success = await _utilities.AddHabitatAsync(newHabitat);
            if (success)
            {
                MessageBox.Show("Habitat added successfully!");
                HabitatNameTextBox.Clear();
                AnimalTypeTextBox.Clear();
                CapacityTextBox.Clear();
                CurrentAnimalsTextBox.Clear();
                LoadData();
            }
            else
            {
                MessageBox.Show("Failed to add habitat.");
            }
        }

        private async void DeleteHabitatButton_Click(object sender, RoutedEventArgs e)
        {
            if (HabitatsDataGrid.SelectedItem is Habitat selectedHabitat)
            {
                var result = MessageBox.Show($"Are you sure you want to delete habitat '{selectedHabitat.Name}'?",
                                           "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _utilities.DeleteHabitatAsync(selectedHabitat.Id);
                    if (success)
                    {
                        MessageBox.Show("Habitat deleted successfully!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete habitat.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a habitat to delete.");
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
