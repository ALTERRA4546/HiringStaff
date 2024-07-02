using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static HiringStaff.Authorization;

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для AddOrChangeWorkingHours.xaml
    /// </summary>
    public partial class AddOrChangeWorkingHours : Window
    {
        public AddOrChangeWorkingHours()
        {
            InitializeComponent();
        }

        public int selectedUser = -1;

        // Загрузка данных из базы данных
        private void Initialization(string selectedPost, string search)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    var employeesData = (from employee in dataBase.Сотрудник
                                         join
                                         post in dataBase.Должность on employee.Код_должности equals post.Код_должности into postGroup
                                         from post in postGroup.DefaultIfEmpty()
                                         where ((selectedPost == null || post.Наименование == selectedPost) && (search == null || employee.Фамилия.ToLower().Contains(search) || employee.Имя.ToLower().Contains(search) || employee.Отчество.ToLower().Contains(search) || employee.Номер_телефона.ToLower().Contains(search) || employee.Email.ToLower().Contains(search) || employee.Адрес.ToLower().Contains(search)))
                                         select new
                                         {
                                             employee.Код_сотрудника,
                                             employee.Фамилия,
                                             employee.Имя,
                                             employee.Отчество,
                                             employee.Номер_телефона,
                                             employee.Email,
                                             employee.Адрес,
                                             employee.Дата_рождения,
                                             employee.Дата_приема_на_работу,
                                             employee.Стаж,
                                             Должность = post.Наименование
                                         }).ToList();

                    EmployeeData.ItemsSource = employeesData;
                    EmployeeData.Columns[0].MaxWidth = 0;
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Подгрузка данных при запуске окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    var postData = dataBase.Должность.Select(x => x.Наименование).ToList();
                    postData.Add("Все");

                    ChoiceOfPosition.ItemsSource = postData;
                    ChoiceOfPosition.SelectedItem = "Все";

                    StartingWorkingDate.SelectedDate = DateTime.Now;
                    EndWorkingDate.SelectedDate = DateTime.Now;

                    Initialization(null, null);

                    int asd = TempData.selectedWorkingHours;

                    // Загрузка данных рабочего графика
                    if (TempData.selectedWorkingHours != -1)
                    {
                        var graphData = (from graph in dataBase.График_работы
                                         join
                                         employee in dataBase.Сотрудник on graph.Код_сотрудника equals employee.Код_сотрудника into employeeGroup
                                         from employee in employeeGroup.DefaultIfEmpty()
                                         where graph.Код_графика_работы == TempData.selectedWorkingHours
                                         select new
                                         {
                                             employee.Фамилия,
                                             employee.Имя,
                                             employee.Отчество,
                                             graph.Дата_начала_работы,
                                             graph.Дата_окончания_работы
                                         }).FirstOrDefault();

                        SelectedUser.Content = graphData.Фамилия + " " + graphData.Имя + " " + graphData.Отчество;
                        StartingWorkingDate.SelectedDate = graphData.Дата_начала_работы;
                        StartingWorkingTime.Text = graphData.Дата_начала_работы.TimeOfDay.ToString();
                        EndWorkingDate.SelectedDate = graphData.Дата_окончания_работы;
                        EndWorkingTime.Text = graphData.Дата_окончания_работы.TimeOfDay.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтрация
        private void Filter()
        {
            try
            {
                string search = "";
                string post = "";

                if (ChoiceOfPosition.SelectedItem.ToString() != "Все")
                    post = ChoiceOfPosition.SelectedItem.ToString();
                else
                    post = null;

                if (Search.Text != "")
                    search = Search.Text;

                Initialization(post, search);
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Поиск сотрудников
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Filter();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Фильтарция по должности
        private void ChoiceOfPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Filter();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Сохранение данных
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Подготовка данных
                string[] tempStartingWorkingDate = StartingWorkingDate.SelectedDate.ToString().Split(' ');
                string[] tempStartingWorkingTime = StartingWorkingTime.Text.Split(':');
                string[] tempEndWorkingDate = EndWorkingDate.SelectedDate.ToString().Split(' ');
                string[] tempEndWorkingTime = StartingWorkingTime.Text.Split(':');
                string pattern = @"\d{2}\:\d{2}\:\d{2}$";

                // Проверка корректности ввода времени
                if (Regex.IsMatch(StartingWorkingTime.Text, pattern) == false || (Convert.ToInt32(tempStartingWorkingTime[0]) < 0 || Convert.ToInt32(tempStartingWorkingTime[0]) > 24) || (Convert.ToInt32(tempStartingWorkingTime[1]) < 0 || Convert.ToInt32(tempStartingWorkingTime[1]) > 60) || (Convert.ToInt32(tempStartingWorkingTime[2]) < 0 || Convert.ToInt32(tempStartingWorkingTime[2]) > 60))
                {
                    MessageBox.Show("Время указано неверно (Пример: 12:00:00)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (Regex.IsMatch(EndWorkingTime.Text, pattern) == false || (Convert.ToInt32(tempEndWorkingTime[0]) < 0 || Convert.ToInt32(tempEndWorkingTime[0]) > 24) || (Convert.ToInt32(tempEndWorkingTime[1]) < 0 || Convert.ToInt32(tempEndWorkingTime[1]) > 60) || (Convert.ToInt32(tempEndWorkingTime[2]) < 0 || Convert.ToInt32(tempEndWorkingTime[2]) > 60))
                {
                    MessageBox.Show("Время указано неверно (Пример: 12:00:00)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверка выбора пользователя
                if (TempData.selectedWorkingHours == -1 && selectedUser == -1)
                {
                    MessageBox.Show("Сотрудник не был выбран", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                using (var dataBase = new HiringStaffEntities())
                {
                    if (TempData.selectedWorkingHours == -1)
                    {
                        var graph = new График_работы();
                        graph.Код_сотрудника = selectedUser;
                        graph.Дата_начала_работы = Convert.ToDateTime(tempStartingWorkingDate[0] + " " + StartingWorkingTime.Text);
                        graph.Дата_окончания_работы = Convert.ToDateTime(tempEndWorkingDate[0] + " " + EndWorkingTime.Text);
                        dataBase.График_работы.Add(graph);
                    }
                    else
                    {
                        var graph = dataBase.График_работы.FirstOrDefault(x => x.Код_графика_работы == TempData.selectedWorkingHours);
                        if (selectedUser != -1)
                            graph.Код_сотрудника = selectedUser;
                        graph.Дата_начала_работы = Convert.ToDateTime(tempStartingWorkingDate[0] + " " + StartingWorkingTime.Text);
                        graph.Дата_окончания_работы = Convert.ToDateTime(tempEndWorkingDate[0] + " " + EndWorkingTime.Text);
                    }

                    dataBase.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Выбор сотрудника
        private void EmployeeData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow row = (DataGridRow)EmployeeData.ItemContainerGenerator.ContainerFromIndex(EmployeeData.SelectedIndex);
                DataGridCell idCell = EmployeeData.Columns[0].GetCellContent(row).Parent as DataGridCell;
                DataGridCell surnameCell = EmployeeData.Columns[1].GetCellContent(row).Parent as DataGridCell;
                DataGridCell nameCell = EmployeeData.Columns[2].GetCellContent(row).Parent as DataGridCell;
                DataGridCell patronymicCell = EmployeeData.Columns[3].GetCellContent(row).Parent as DataGridCell;

                selectedUser = Convert.ToInt32(((TextBlock)idCell.Content).Text);
                string surname = ((TextBlock)surnameCell.Content).Text;
                string name = ((TextBlock)nameCell.Content).Text;
                string patronymic = ((TextBlock)patronymicCell.Content).Text;

                SelectedUser.Content = surname + " " + name + " " + patronymic;
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
