﻿using System;
using System.Collections.Generic;
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
            Application.Current.Shutdown();
        }

        // Синхранизация пароля
        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (HidePassword.Password.Length != Password.Text.Length)
            {
                HidePassword.Password = Password.Text;
                Password.Focus();
            }
        }

        // Синхранизация пароля
        private void HidePassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (HidePassword.Password.Length != Password.Text.Length)
            {
                Password.Text = HidePassword.Password;
                HidePassword.Focus();
            }
        }

        // Показ или скрытие пароля
        private void ShowPassword_Click(object sender, RoutedEventArgs e)
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
    }
}
