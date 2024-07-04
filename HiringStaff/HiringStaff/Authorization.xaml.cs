using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        // Хранение значения показа пароля
        public bool showPassword = false;

        // Хранение переносимых переменных
        public static class TempData
        { 
            public static int userId { get; set; }
            public static int selectedEmployee { get; set; }
            public static int selectedWorkingHours { get; set; }
            public static int selectedReport { get; set; }
        }

        // Авторизация
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнености строки подключения к базе данных
                if (ConnectionString.Text == "")
                {
                    MessageBox.Show("Имя подключаемого сервера не заполнена", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                ChangingСonnectionString();

                using (var dataBase = new HiringStaffEntities())
                {
                    // Получение данных о сотруднике
                    var userData = dataBase.Авторизация.FirstOrDefault(x => x.Логин == Login.Text.ToString() && x.Пароль == Password.Text.ToString());

                    // Проверка логина и пароля сотрудника
                    if (userData != null)
                    {
                        // Получение данных о должности сотрудника
                        var employeePositions = (from employee in dataBase.Сотрудник
                                                 join
                                                 post in dataBase.Должность on employee.Код_должности equals post.Код_должности
                                                 where employee.Код_сотрудника == userData.Код_сотрудника
                                                 select new
                                                 {
                                                     post.Наименование
                                                 }).FirstOrDefault();

                        TempData.userId = userData.Код_сотрудника.Value;

                        // Открытие окна для должности сотрудника
                        switch (employeePositions.Наименование)
                        {
                            case "Директор":
                                Director director = new Director();
                                director.Show();
                                this.Hide();
                                break;
                            case "Администратор":
                                Administrator administrator = new Administrator();
                                administrator.Show();
                                this.Hide();
                                break;
                            default:
                                MessageBox.Show("Ваша должность не соответствует требованиям","Внимание",MessageBoxButton.OK, MessageBoxImage.Warning);
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Закрытие приложение
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Синхранизация пароля
        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (HidePassword.Password.Length != Password.Text.Length)
                {
                    HidePassword.Password = Password.Text;
                    Password.Focus();
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Синхранизация пароля
        private void HidePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (HidePassword.Password.Length != Password.Text.Length)
                {
                    Password.Text = HidePassword.Password;
                    HidePassword.Focus();
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Показ или скрытие пароля
        private void ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (showPassword == true)
                {
                    showPassword = false;
                    HidePassword.Visibility = Visibility.Visible;
                    Password.Visibility = Visibility.Collapsed;
                }
                else
                {
                    showPassword = true;
                    HidePassword.Visibility = Visibility.Collapsed;
                    Password.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Изменение строки подключения
        private void ChangingСonnectionString()
        {
            try
            {
                string fileСonnectionString = "DataBase.ini";

                // Название изменяемой строки подключения
                string connectionStringName = "HiringStaffEntities";
                string newDataSource = ConnectionString.Text;


                // Формирование текущей конфигурации проложения
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                // Получение ностроек строк подключения с именем HiringStaffEntities
                ConnectionStringSettings settings = config.ConnectionStrings.ConnectionStrings[connectionStringName];

                // Проверка получения настроек строки подключения
                if (settings != null)
                {
                    // Получение текущей строки подключения и смена источника данных
                    string currentConnectionString = settings.ConnectionString;
                    string newConnectionString = currentConnectionString.Replace("DESKTOP-09DGVTM\\SQLEXPRESS", newDataSource);

                    // Задание новой строки подключения в настройках
                    settings.ConnectionString = newConnectionString;

                    // Сохранеие измененой конфигурации в файле конфигурации
                    config.Save(ConfigurationSaveMode.Modified);

                    // Принудительная перезагрузка раздена конфигураций приложения
                    ConfigurationManager.RefreshSection("connectionStrings");

                    if (File.Exists(fileСonnectionString) && (File.ReadAllText(fileСonnectionString) != ConnectionString.Text))
                    {
                        File.Delete(fileСonnectionString);
                        File.WriteAllText(fileСonnectionString, ConnectionString.Text);
                    }
                    else
                    {
                        if (!File.Exists(fileСonnectionString))
                        {
                            File.WriteAllText(fileСonnectionString, ConnectionString.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileСonnectionString = "DataBase.ini";

                if (File.Exists(fileСonnectionString))
                {
                    ConnectionString.Text = File.ReadAllText(fileСonnectionString);
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
