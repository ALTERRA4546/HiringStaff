using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
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
using static HiringStaff.Authorization;

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для Director.xaml
    /// </summary>
    public partial class Director : Window
    {
        public Director()
        {
            InitializeComponent();
        }

        // Публичная перемена для хранениея режежима выхода из приложения
        public bool exitMode = false;

        // Загрузка данных из базы данных
        private void Initial(string tempPost, string search)
        {
            using (var dataBase = new HiringStaffEntities())
            {
                var userData = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == TempData.userId);

                UserName.Content = userData.Фамилия + " " + userData.Имя + " " + userData.Отчество;

                var employeesData = (from employee in dataBase.Сотрудник
                                     join
                                     post in dataBase.Должность on employee.Код_должности equals post.Код_должности into postGroup
                                     from post in postGroup.DefaultIfEmpty()
                                     where ((tempPost == null || post.Наименование == tempPost) && (search == null || employee.Фамилия.ToLower().Contains(search) || employee.Имя.ToLower().Contains(search) || employee.Отчество.ToLower().Contains(search) || employee.Номер_телефона.ToLower().Contains(search) || employee.Email.ToLower().Contains(search) || employee.Адрес.ToLower().Contains(search)))
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

        // Подгрузка данных при запуске окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var dataBase = new HiringStaffEntities())
            {
                var postData = dataBase.Должность.Select(x => x.Наименование).ToList();
                postData.Add("Все");

                ChoiceOfPosition.ItemsSource = postData;
                ChoiceOfPosition.SelectedItem = "Все";

                Initial(null, null);
            }
        }

        // Выход на окно авторизации
        private void Exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exitMode = true;
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        // Закрытие окна
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!exitMode)
                Application.Current.Shutdown();
        }

        // Переход в окно добавления сотрудника
        private void AddEmployees_Click(object sender, RoutedEventArgs e)
        {
            AddOrChangeEmployee addOrChangeEmployee = new AddOrChangeEmployee();
            TempData.selectedEmployee = -1;
            addOrChangeEmployee.ShowDialog();
            Filter();
        }

        // Удаление сотрудника
        private void RemoveEmployees_Click(object sender, RoutedEventArgs e)
        {
            // Проверка выделенной строки
            if (EmployeeData.SelectedIndex < 0)
            {
                MessageBox.Show("Строка для удаления не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получение кода сотрудника из строки dataGrid
            DataGridRow row = (DataGridRow)EmployeeData.ItemContainerGenerator.ContainerFromIndex(EmployeeData.SelectedIndex);
            DataGridCell cell = EmployeeData.Columns[0].GetCellContent(row).Parent as DataGridCell;
            int idRemoveEmployee = Convert.ToInt32(((TextBlock)cell.Content).Text);

            // Запрос на подтверждение действий
            if (MessageBox.Show("Вы действительно хотите удалить сотрудника", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                // Удаление сотрудника
                using (var dataBase = new HiringStaffEntities())
                {
                    var removeDocument = dataBase.Документы.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                    var removeAuthorization = dataBase.Авторизация.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                    var removeEmployee = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                    var removeRoom = dataBase.Помещение.FirstOrDefault(x => x.Код_отвественного_сотрудника == idRemoveEmployee);
                    var removeClass = dataBase.Классы.FirstOrDefault(x => x.Код_классного_руководителя == idRemoveEmployee);
                    var removeSubjectsTaught = dataBase.Преподаваемые_предметы.Where(x => x.Код_учителя == idRemoveEmployee).ToList();

                    if(removeDocument != null)
                        dataBase.Документы.Remove(removeDocument);
                    if(removeAuthorization != null)
                        dataBase.Авторизация.Remove(removeAuthorization);
                    if (removeRoom != null)
                        removeRoom.Код_отвественного_сотрудника = null;
                    if (removeClass != null)
                        removeClass.Код_классного_руководителя = null;
                    if (removeSubjectsTaught != null)
                        dataBase.Преподаваемые_предметы.RemoveRange(removeSubjectsTaught);
                    dataBase.Сотрудник.Remove(removeEmployee);
                    dataBase.SaveChanges();

                    Filter();
                    MessageBox.Show("Сотрудника был удален","Информация",MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Редактирование сотрудника
        private void EmployeeData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)EmployeeData.ItemContainerGenerator.ContainerFromIndex(EmployeeData.SelectedIndex);
            DataGridCell cell = EmployeeData.Columns[0].GetCellContent(row).Parent as DataGridCell;
            TempData.selectedEmployee = Convert.ToInt32(((TextBlock)cell.Content).Text);

            AddOrChangeEmployee addOrChangeEmployee = new AddOrChangeEmployee();
            addOrChangeEmployee.ShowDialog();
            Initial(null, null);
        }

        // Фильтрация
        private void Filter()
        {
            string search = "";
            string post = "";

            if (ChoiceOfPosition.SelectedItem.ToString() != "Все")
                post = ChoiceOfPosition.SelectedItem.ToString();
            else
                post = null;

            if (Search.Text != "")
                search = Search.Text;

            Initial(post, search);

        }

        // Поиск сотрудников
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        // Фильтарция по должности
        private void ChoiceOfPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
