using iTextSharp.text.pdf;
using Microsoft.Win32;
using OfficeOpenXml.Drawing.Slicer.Style;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для Administrator.xaml
    /// </summary>
    public partial class Administrator : Window
    {
        public Administrator()
        {
            InitializeComponent();
        }

        // Режим выхода из приложения
        public bool exitMode = false;

        // Смена измененой таблицы
        public bool changingTable = false;

        // Выбраная таблицы
        public int selectionTable = -1;

        // Листы для хранениея изменений в таблице
        List<Сотрудник> updatedEmployees = new List<Сотрудник>();
        List<Документы> updatedDocuments = new List<Документы>();
        List<Авторизация> updatedAuthorization = new List<Авторизация>();
        List<График_работы> updatedWorkSchedule = new List<График_работы>();
        List<Зарплаты_сотрудников> updatedEmployeeSalaries = new List<Зарплаты_сотрудников>();
        List<Должность> updatedPost = new List<Должность>();
        List<Помещение> updatedRoom = new List<Помещение>();
        List<Классы> updatedClass = new List<Классы>();
        List<Предмет> updatedItem = new List<Предмет>();
        List<Преподаваемые_часы> updatedHoursTaught = new List<Преподаваемые_часы>();
        List<Преподаваемые_предметы> updatedSubjectsTaught = new List<Преподаваемые_предметы>();

        // Подгрузка данных при запуске окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    var userData = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == TempData.userId);

                    UserName.Content = userData.Фамилия + " " + userData.Имя + " " + userData.Отчество;
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Закрытие приложения
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (exitMode != true)
                Application.Current.Shutdown();
        }

        // Загрузка данных в DataGrid
        private void Initial()
        {
            try
            {
                changingTable = false;

                using (var dataBase = new HiringStaffEntities())
                {
                    switch (selectionTable)
                    {
                        // Загрузка данных о сотрудниках
                        case 0:
                            SelectedTable.Content = "Сотрудник";
                            var employeeData = dataBase.Сотрудник.ToList();
                            TabularData.ItemsSource = employeeData;
                            TabularData.Columns[0].IsReadOnly = true;
                            for (int i = 11; i < 18; i++)
                                TabularData.Columns[i].MaxWidth = 0;
                            break;

                        // Загрузка данных о документах
                        case 1:
                            SelectedTable.Content = "Документы";
                            var documentsData = dataBase.Документы.ToList();
                            TabularData.ItemsSource = documentsData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[10].MaxWidth = 0;
                            break;

                        // Загрузка данных о авторизации
                        case 2:
                            SelectedTable.Content = "Авторизация";
                            var authorizationData = dataBase.Авторизация.ToList();
                            TabularData.ItemsSource = authorizationData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[4].MaxWidth = 0;
                            break;

                        // Загрузка данных о графике работы
                        case 3:
                            SelectedTable.Content = "График работы";
                            var workScheduleData = dataBase.График_работы.ToList();
                            TabularData.ItemsSource = workScheduleData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[4].MaxWidth = 0;
                            break;

                        // Загрузка данных о зарплате сотрудников
                        case 4:
                            SelectedTable.Content = "Зарплаты сотрудников";
                            var employeeSalariesData = dataBase.Зарплаты_сотрудников.ToList();
                            TabularData.ItemsSource = employeeSalariesData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[4].MaxWidth = 0;
                            break;

                        // Загрузка данных о должностях
                        case 5:
                            SelectedTable.Content = "Должность";
                            var postData = dataBase.Должность.ToList();
                            TabularData.ItemsSource = postData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[2].MaxWidth = 0;
                            TabularData.Columns[3].MaxWidth = 0;
                            break;

                        // Загрузка данных о помещениях
                        case 6:
                            SelectedTable.Content = "Помещение";
                            var roomData = dataBase.Помещение.ToList();
                            TabularData.ItemsSource = roomData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[4].MaxWidth = 0;
                            TabularData.Columns[5].MaxWidth = 0;
                            break;

                        // Загрузка данных о классах
                        case 7:
                            SelectedTable.Content = "Классы";
                            var classData = dataBase.Классы.ToList();
                            TabularData.ItemsSource = classData;
                            TabularData.Columns[0].IsReadOnly = true;
                            for (int i = 5; i < 8; i++)
                                TabularData.Columns[i].MaxWidth = 0;
                            break;

                        // Загрузка данных о предметах
                        case 8:
                            SelectedTable.Content = "Предмет";
                            var itemData = dataBase.Предмет.ToList();
                            TabularData.ItemsSource = itemData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[2].MaxWidth = 0;
                            TabularData.Columns[3].MaxWidth = 0;
                            break;

                        // Загрузка данных о преподоваемых часах
                        case 9:
                            SelectedTable.Content = "Преподоваемые часы";
                            var hoursTaughtData = dataBase.Преподаваемые_часы.ToList();
                            TabularData.ItemsSource = hoursTaughtData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[5].MaxWidth = 0;
                            TabularData.Columns[6].MaxWidth = 0;
                            break;

                        // Загрузка данных о преподоваемых предметах
                        case 10:
                            SelectedTable.Content = "Преподоваемые предметы";
                            var subjectsTaughtData = dataBase.Преподаваемые_предметы.ToList();
                            TabularData.ItemsSource = subjectsTaughtData;
                            TabularData.Columns[0].IsReadOnly = true;
                            TabularData.Columns[3].MaxWidth = 0;
                            TabularData.Columns[4].MaxWidth = 0;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрзка таблицы сотрудник
        private void Employee_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true) 
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 0;
                    Initial();
                }
            }
            else
            {
                selectionTable = 0;
                Initial();
            }
        }

        // Загрузка таблицы документы
        private void Documents_Click(object sender, RoutedEventArgs e)
        {
            // Предупреждение о смене таблицы без сохранения изменений
            if (changingTable == true)
            {
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 1;
                    Initial();
                }
            }
            else
            {
                selectionTable = 1;
                Initial();
            }
        }

        // Загрузка таблицы авторизация
        private void Authorization_Click(object sender, RoutedEventArgs e)
        {
            // Предупреждение о смене таблицы без сохранения изменений
            if (changingTable == true)
            {
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 2;
                    Initial();
                }
            }
            else
            {
                selectionTable = 2;
                Initial();
            }
        }

        // Загрузка таблицы график работы
        private void WorkSchedule_Click(object sender, RoutedEventArgs e)
        {
            // Предупреждение о смене таблицы без сохранения изменений
            if (changingTable == true)
            {
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 3;
                    Initial();
                }
            }
            else
            {
                selectionTable = 3;
                Initial();
            }
        }

        // Загрузка таблицы зарплаты сотрудников
        private void EmployeeSalaries_Click(object sender, RoutedEventArgs e)
        {
            // Предупреждение о смене таблицы без сохранения изменений
            if (changingTable == true)
            {
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 4;
                    Initial();
                }
            }
            else
            {
                selectionTable = 4;
                Initial();
            }
        }

        // Загрузка таблицы должность
        private void Post_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 5;
                    Initial();
                }
            }
            else
            {
                selectionTable = 5;
                Initial();
            }
        }

        // Загрузка таблицы помещения
        private void Room_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 6;
                    Initial();
                }
            }
            else
            {
                selectionTable = 6;
                Initial();
            }
        }

        // Загрузка таблицы классы
        private void Class_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 7;
                    Initial();
                }
            }
            else
            {
                selectionTable = 7;
                Initial();
            }
        }

        // Загрузка таблицы предмет
        private void Item_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 8;
                    Initial();
                }
            }
            else
            {
                selectionTable = 8;
                Initial();
            }
        }

        // Загрузка таблицы преподоваемые часы
        private void HoursTaught_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 9;
                    Initial();
                }
            }
            else
            {
                selectionTable = 9;
                Initial();
            }
        }

        // Загрузка таблицы преподаваемые предметы
        private void SubjectsTaught_Click(object sender, RoutedEventArgs e)
        {
            if (changingTable == true)
            {
                // Предупреждение о смене таблицы без сохранения изменений
                if (MessageBox.Show("Данные в таблицы были изменены, вы хотите перейти в эту таблицу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    selectionTable = 10;
                    Initial();
                }
            }
            else
            {
                selectionTable = 10;
                Initial();
            }
        }

        // Смена пользователя
        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            exitMode = true;
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Hide();
        }

        // Выход из приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Сохранение изменений
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    switch (selectionTable)
                    {
                        // Сохранение данных сотрудника в базе данных
                        case 0:
                            foreach (var employee in updatedEmployees)
                            {
                                if (employee.Код_сотрудника == 0)
                                {
                                    dataBase.Сотрудник.Add(employee);
                                }
                                else
                                {
                                    dataBase.Entry(employee).State = EntityState.Modified;
                                }
                            }

                            dataBase.SaveChanges();
                            updatedEmployees.Clear();
                            break;

                        // Сохранение данных документа в базе данных
                        case 1:
                            foreach (var documents in updatedDocuments)
                            {
                                if (documents.Код_документа == 0)
                                {
                                    dataBase.Документы.Add(documents);
                                }
                                else
                                {
                                    dataBase.Entry(documents).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedDocuments.Clear();
                            break;

                        // Сохранение данных авторизации в базе данных
                        case 2:
                            foreach (var authorization in updatedAuthorization)
                            {
                                if (authorization.Код_авторизации == 0)
                                {
                                    dataBase.Авторизация.Add(authorization);
                                }
                                else
                                {
                                    dataBase.Entry(authorization).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedAuthorization.Clear();
                            break;

                        // Сохранение данных графика работы в базе данных
                        case 3:
                            foreach (var workSchedule in updatedWorkSchedule)
                            {
                                if (workSchedule.Код_графика_работы == 0)
                                {
                                    dataBase.График_работы.Add(workSchedule);
                                }
                                else
                                {
                                    dataBase.Entry(workSchedule).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedWorkSchedule.Clear();
                            break;

                        // Сохранение данных зарплат сотрудников в базе данных
                        case 4:
                            foreach (var employeeSalaries in updatedEmployeeSalaries)
                            {
                                if (employeeSalaries.Код_зарплаты == 0)
                                {
                                    dataBase.Зарплаты_сотрудников.Add(employeeSalaries);
                                }
                                else
                                {
                                    dataBase.Entry(employeeSalaries).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedEmployeeSalaries.Clear();
                            break;

                        // Сохранение данных должности в базе данных
                        case 5:
                            foreach (var post in updatedPost)
                            {
                                if (post.Код_должности == 0)
                                {
                                    dataBase.Должность.Add(post);
                                }
                                else
                                {
                                    dataBase.Entry(post).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedPost.Clear();
                            break;

                        // Сохранение данных помещения в базе данных
                        case 6:
                            foreach (var room in updatedRoom)
                            {
                                if (room.Код_помещения == 0)
                                {
                                    dataBase.Помещение.Add(room);
                                }
                                else
                                {
                                    dataBase.Entry(room).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedRoom.Clear();
                            break;

                        // Сохранение данных классов в базе данных
                        case 7:
                            foreach (var classes in updatedClass)
                            {
                                if (classes.Код_класса == 0)
                                {
                                    dataBase.Классы.Add(classes);
                                }
                                else
                                {
                                    dataBase.Entry(classes).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedClass.Clear();
                            break;

                        // Сохранение данных предметов в базе данных
                        case 8:
                            foreach (var item in updatedItem)
                            {
                                if (item.Код_предмета == 0)
                                {
                                    dataBase.Предмет.Add(item);
                                }
                                else
                                {
                                    dataBase.Entry(item).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedItem.Clear();
                            break;

                        // Сохранение данных преподоваемых часов в базе данных
                        case 9:
                            foreach (var hoursTaught in updatedHoursTaught)
                            {
                                if (hoursTaught.Код_преподаваемых_часов == 0)
                                {
                                    dataBase.Преподаваемые_часы.Add(hoursTaught);
                                }
                                else
                                {
                                    dataBase.Entry(hoursTaught).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedHoursTaught.Clear();
                            break;

                        // Сохранение данных преподоваемых предметов в базе данных
                        case 10:
                            foreach (var subjectsTaught in updatedSubjectsTaught)
                            {
                                if (subjectsTaught.Код_преподоваемого_предмета == 0)
                                {
                                    dataBase.Преподаваемые_предметы.Add(subjectsTaught);
                                }
                                else
                                {
                                    dataBase.Entry(subjectsTaught).State = EntityState.Modified;
                                }
                            }
                            dataBase.SaveChanges();
                            updatedSubjectsTaught.Clear();
                            break;
                    }
                    Initial();
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Запись измененный и добавленных данных в DataGrid
        private void TabularData_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            try
            {
                changingTable = true;

                switch (selectionTable)
                {
                    // Запись изменяемых данных о сотрудниках в List 
                    case 0:
                        Сотрудник employee = e.Row.DataContext as Сотрудник;
                        if (!updatedEmployees.Contains(employee))
                        {
                            updatedEmployees.Add(employee);
                        }
                        break;

                    // Запись изменяемых данных о документах в List 
                    case 1:
                        Документы documents = e.Row.DataContext as Документы;
                        if (!updatedDocuments.Contains(documents))
                        {
                            updatedDocuments.Add(documents);
                        }
                        break;

                    // Запись изменяемых данных о авторизации в List 
                    case 2:
                        Авторизация authorization = e.Row.DataContext as Авторизация;
                        if (!updatedAuthorization.Contains(authorization))
                        {
                            updatedAuthorization.Add(authorization);
                        }
                        break;

                    // Запись изменяемых данных о графике работы в List 
                    case 3:
                        График_работы workSchedule = e.Row.DataContext as График_работы;
                        if (!updatedWorkSchedule.Contains(workSchedule))
                        {
                            updatedWorkSchedule.Add(workSchedule);
                        }
                        break;

                    // Запись изменяемых данных о зарплате сотрудников в List 
                    case 4:
                        Зарплаты_сотрудников employeeSalaries = e.Row.DataContext as Зарплаты_сотрудников;
                        if (!updatedEmployeeSalaries.Contains(employeeSalaries))
                        {
                            updatedEmployeeSalaries.Add(employeeSalaries);
                        }
                        break;

                    // Запись изменяемых данных о должностях в List 
                    case 5:
                        Должность post = e.Row.DataContext as Должность;
                        if (!updatedPost.Contains(post))
                        {
                            updatedPost.Add(post);
                        }
                        break;

                    // Запись изменяемых данных о помещениях в List 
                    case 6:
                        Помещение room = e.Row.DataContext as Помещение;
                        if (!updatedRoom.Contains(room))
                        {
                            updatedRoom.Add(room);
                        }
                        break;

                    // Запись изменяемых данных о классах в List 
                    case 7:
                        Классы classes = e.Row.DataContext as Классы;
                        if (!updatedClass.Contains(classes))
                        {
                            updatedClass.Add(classes);
                        }
                        break;

                    // Запись изменяемых данных о предметах в List 
                    case 8:
                        Предмет item = e.Row.DataContext as Предмет;
                        if (!updatedItem.Contains(item))
                        {
                            updatedItem.Add(item);
                        }
                        break;


                    // Запись изменяемых данных о преподоваемых часах в List 
                    case 9:
                        Преподаваемые_часы hoursTaught = e.Row.DataContext as Преподаваемые_часы;
                        if (!updatedHoursTaught.Contains(hoursTaught))
                        {
                            updatedHoursTaught.Add(hoursTaught);
                        }
                        break;

                    // Запись изменяемых данных о преподоваемых предметах в List 
                    case 10:
                        Преподаваемые_предметы subjectsTaught = e.Row.DataContext as Преподаваемые_предметы;
                        if (!updatedSubjectsTaught.Contains(subjectsTaught))
                        {
                            updatedSubjectsTaught.Add(subjectsTaught);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Обнавление данных
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Initial();
        }

        // Удаление записи из базы данных
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы действительно хотите удалить запись", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Получение кода удаляемой строки
                    DataGridRow row = (DataGridRow)TabularData.ItemContainerGenerator.ContainerFromIndex(TabularData.SelectedIndex);
                    DataGridCell cell = TabularData.Columns[0].GetCellContent(row).Parent as DataGridCell;
                    int removeRow = Convert.ToInt32(((TextBlock)cell.Content).Text);

                    using (var dataBase = new HiringStaffEntities())
                    {
                        switch (selectionTable)
                        {
                            // Удаление сотрудника
                            case 0:
                                var removeEmployee = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == removeRow);
                                dataBase.Сотрудник.Remove(removeEmployee);
                                dataBase.SaveChanges();
                                break;

                            // Удаление документа
                            case 1:
                                var removeDocuments = dataBase.Документы.FirstOrDefault(x => x.Код_документа == removeRow);
                                dataBase.Документы.Remove(removeDocuments);
                                dataBase.SaveChanges();
                                break;

                            // Удаление авторизации
                            case 2:
                                var removeAuthorization = dataBase.Авторизация.FirstOrDefault(x => x.Код_авторизации == removeRow);
                                dataBase.Авторизация.Remove(removeAuthorization);
                                dataBase.SaveChanges();
                                break;

                            // Удаление графика работы
                            case 3:
                                var removeWorkSchedule = dataBase.График_работы.FirstOrDefault(x => x.Код_графика_работы == removeRow);
                                dataBase.График_работы.Remove(removeWorkSchedule);
                                dataBase.SaveChanges();
                                break;

                            // Удаление зарплаты сотрудников
                            case 4:
                                var removeEmployeeSalaries = dataBase.Зарплаты_сотрудников.FirstOrDefault(x => x.Код_зарплаты == removeRow);
                                dataBase.Зарплаты_сотрудников.Remove(removeEmployeeSalaries);
                                dataBase.SaveChanges();
                                break;

                            // Удаление должности
                            case 5:
                                var removePost = dataBase.Должность.FirstOrDefault(x => x.Код_должности == removeRow);
                                dataBase.Должность.Remove(removePost);
                                dataBase.SaveChanges();
                                break;

                            // Удаление помещения
                            case 6:
                                var removeRoom = dataBase.Помещение.FirstOrDefault(x => x.Код_помещения == removeRow);
                                dataBase.Помещение.Remove(removeRoom);
                                dataBase.SaveChanges();
                                break;

                            // Удаление класса
                            case 7:
                                var removeClass = dataBase.Классы.FirstOrDefault(x => x.Код_класса == removeRow);
                                dataBase.Классы.Remove(removeClass);
                                dataBase.SaveChanges();
                                break;

                            // Удаление предмета
                            case 8:
                                var removeItem = dataBase.Предмет.FirstOrDefault(x => x.Код_предмета == removeRow);
                                dataBase.Предмет.Remove(removeItem);
                                dataBase.SaveChanges();
                                break;

                            // Удаление преподоваемых часов
                            case 9:
                                var removeHoursTaught = dataBase.Преподаваемые_часы.FirstOrDefault(x => x.Код_преподаваемых_часов == removeRow);
                                dataBase.Преподаваемые_часы.Remove(removeHoursTaught);
                                dataBase.SaveChanges();
                                break;

                            // Удаление преподоваемых предметов
                            case 10:
                                var removeSubjectsTaught = dataBase.Преподаваемые_предметы.FirstOrDefault(x => x.Код_преподоваемого_предмета == removeRow);
                                dataBase.Преподаваемые_предметы.Remove(removeSubjectsTaught);
                                dataBase.SaveChanges();
                                break;
                        }
                        Initial();
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Создание полной резервной копии базы данных
        private void FullBackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = @"BackUp (*.bak)|*.bak";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var backUp = dataBase.Database.SqlQuery<HiringStaffEntities>($"BACKUP DATABASE [HiringStaff] TO DISK = '{saveFileDialog.FileName}'").ToList();
                        MessageBox.Show("Резервное копирование выполненно", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Создание разностной резервной копии базы данных
        private void DifferentialBackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = @"BackUp (*.bak)|*.bak";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var backUp = dataBase.Database.SqlQuery<HiringStaffEntities>($"BACKUP DATABASE [HiringStaff] TO DISK = '{saveFileDialog.FileName}' WITH DIFFERENTIAL").ToList();
                        MessageBox.Show("Резервное копирование выполненно", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Создание журнала транзакций базы данных
        private void TransactionLogBackUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = @"BackUp (*.bak)|*.bak";

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        var backUp = dataBase.Database.SqlQuery<HiringStaffEntities>($"BACKUP LOG [HiringStaff] TO DISK = '{saveFileDialog.FileName}'").ToList();
                        Initial();
                        MessageBox.Show("Резервное копирование выполненно", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Востановление базы данных
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Для востановления базы данных используеться только полная резервная копия!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            using (var dataBase = new HiringStaffEntities())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"BackUp (*.bak)|*.bak";

                if (openFileDialog.ShowDialog() == true)
                {
                    var backUp = dataBase.Database.SqlQuery<HiringStaffEntities>($"USE master; ALTER DATABASE [HiringStaff] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [HiringStaff] FROM DISK = '{openFileDialog.FileName}' WITH REPLACE; ALTER DATABASE [HiringStaff] SET MULTI_USER;").ToList();
                    Initial();
                    MessageBox.Show("Востановление выполнено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
