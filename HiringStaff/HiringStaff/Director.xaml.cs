using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static HiringStaff.Authorization;
using OfficeOpenXml;

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
        private void Initialization(string selectedPost, string search)
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    var userData = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == TempData.userId);

                    UserName.Content = userData.Фамилия + " " + userData.Имя + " " + userData.Отчество;

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

                    Initialization(null, null);
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            try
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
                if (MessageBox.Show("Вы действительно хотите удалить сотрудника?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Удаление сотрудника
                    using (var dataBase = new HiringStaffEntities())
                    {
                        var removeDocument = dataBase.Документы.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                        var removeAuthorization = dataBase.Авторизация.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                        var removeEmployee = dataBase.Сотрудник.FirstOrDefault(x => x.Код_сотрудника == idRemoveEmployee);
                        var removeRoom = dataBase.Помещение.FirstOrDefault(x => x.Код_ответственного_сотрудника == idRemoveEmployee);
                        var removeClass = dataBase.Классы.FirstOrDefault(x => x.Код_классного_руководителя == idRemoveEmployee);
                        var removeSubjectsTaught = dataBase.Преподаваемые_предметы.Where(x => x.Код_учителя == idRemoveEmployee).ToList();
                        var removeGraph = dataBase.График_работы.Where(x => x.Код_сотрудника == idRemoveEmployee).ToList();

                        if (removeDocument != null)
                            dataBase.Документы.Remove(removeDocument);
                        if (removeAuthorization != null)
                            dataBase.Авторизация.Remove(removeAuthorization);
                        if (removeRoom != null)
                            removeRoom.Код_ответственного_сотрудника = null;
                        if (removeClass != null)
                            removeClass.Код_классного_руководителя = null;
                        if (removeSubjectsTaught != null)
                            dataBase.Преподаваемые_предметы.RemoveRange(removeSubjectsTaught);
                        if (removeGraph != null)
                            dataBase.График_работы.RemoveRange(removeGraph);
                        dataBase.Сотрудник.Remove(removeEmployee);
                        dataBase.SaveChanges();

                        Filter();
                        MessageBox.Show("Сотрудника был удален", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Редактирование сотрудника
        private void EmployeeData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow row = (DataGridRow)EmployeeData.ItemContainerGenerator.ContainerFromIndex(EmployeeData.SelectedIndex);
                DataGridCell cell = EmployeeData.Columns[0].GetCellContent(row).Parent as DataGridCell;
                TempData.selectedEmployee = Convert.ToInt32(((TextBlock)cell.Content).Text);
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            AddOrChangeEmployee addOrChangeEmployee = new AddOrChangeEmployee();
            addOrChangeEmployee.ShowDialog();
            Initialization(null, null);
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

            Initialization(post, search);
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

        // Экспорт данных
        private void ExportData(string tempPost)
        {
            try
            {
                // Запрос места и тип сохраняемого файла
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|CSV файл (*.csv)|*.csv|Excel (*.xlsx)|*.xlsx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    // Загрузка экспортируемых данных из базы данных
                    using (var dataBase = new HiringStaffEntities())
                    {
                        var employeeData = (from employee in dataBase.Сотрудник
                                            join
                                            post in dataBase.Должность on employee.Код_должности equals post.Код_должности into postGroup
                                            from post in postGroup.DefaultIfEmpty()
                                            join
                                            documents in dataBase.Документы on employee.Код_сотрудника equals documents.Код_сотрудника into documentsGroup
                                            from documents in documentsGroup.DefaultIfEmpty()
                                            join
                                            authorization in dataBase.Авторизация on employee.Код_сотрудника equals authorization.Код_сотрудника into authorizationGroup
                                            from authorization in authorizationGroup.DefaultIfEmpty()
                                            join
                                            room in dataBase.Помещение on employee.Код_сотрудника equals room.Код_ответственного_сотрудника into roomGroup
                                            from room in roomGroup.DefaultIfEmpty()
                                            join
                                            classes in dataBase.Классы on employee.Код_сотрудника equals classes.Код_классного_руководителя into classesGroup
                                            from classes in classesGroup.DefaultIfEmpty()
                                            where (tempPost == null || post.Наименование == tempPost)
                                            select new
                                            {
                                                employee.Код_сотрудника,
                                                employee.Фамилия,
                                                employee.Имя,
                                                employee.Отчество,
                                                employee.Номер_телефона,
                                                employee.Email,
                                                employee.Дата_рождения,
                                                employee.Стаж,
                                                employee.Адрес,
                                                employee.Дата_приема_на_работу,
                                                Должность = post.Наименование,
                                                documents.Серия_паспорта,
                                                documents.Номер_паспорта,
                                                documents.ИНН,
                                                documents.СНИЛС,
                                                documents.Номер_медицинского_полиса,
                                                documents.Номер_трудового_договора,
                                                documents.Срок_действия_договора,
                                                Помещение = room.Наименование,
                                                Класс = classes.Наименование
                                            }).ToList();

                        // Сохранение в выбранном формате
                        switch (saveFileDialog.FilterIndex)
                        {
                            // Сохранение в PDF
                            case 1:
                                // Создание документа
                                Document doc = new Document();
                                PdfWriter PDFWriter = PdfWriter.GetInstance(doc, new FileStream($@"{saveFileDialog.FileName}", FileMode.Create));
                                doc.SetPageSize(PageSize.A3.Rotate());

                                // Создание шрифта
                                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                                Font font = new Font(baseFont, 8);

                                // Открытие документа
                                doc.Open();

                                // Создание таблицы
                                PdfPTable table = new PdfPTable(20);
                                table.WidthPercentage = 100;

                                // Создание заголовков таблицы
                                table.AddCell(new Paragraph("Код сотрудника", font));
                                table.AddCell(new Paragraph("Фамилия", font));
                                table.AddCell(new Paragraph("Имя", font));
                                table.AddCell(new Paragraph("Отчество", font));
                                table.AddCell(new Paragraph("Номер телефона", font));
                                table.AddCell(new Paragraph("Email", font));
                                table.AddCell(new Paragraph("Дата рождения", font));
                                table.AddCell(new Paragraph("Стаж", font));
                                table.AddCell(new Paragraph("Адрес", font));
                                table.AddCell(new Paragraph("Дата приема на работу", font));
                                table.AddCell(new Paragraph("Должность", font));
                                table.AddCell(new Paragraph("Серия паспорта", font));
                                table.AddCell(new Paragraph("Номер паспорта", font));
                                table.AddCell(new Paragraph("ИНН", font));
                                table.AddCell(new Paragraph("СНИЛС", font));
                                table.AddCell(new Paragraph("Номер медицинского полиса", font));
                                table.AddCell(new Paragraph("Номер трудового договора", font));
                                table.AddCell(new Paragraph("Срок действия договора", font));
                                table.AddCell(new Paragraph("Назначенное помещение", font));
                                table.AddCell(new Paragraph("Назначенный класс", font));

                                // Заполнение таблицы полученными данными
                                foreach (var line in employeeData)
                                {
                                    table.AddCell(new Paragraph(line.Код_сотрудника.ToString(), font));
                                    table.AddCell(new Paragraph(line.Фамилия.ToString(), font));
                                    table.AddCell(new Paragraph(line.Имя.ToString(), font));
                                    table.AddCell(new Paragraph(line.Отчество.ToString(), font));
                                    table.AddCell(new Paragraph(line.Номер_телефона.ToString(), font));
                                    table.AddCell(new Paragraph(line.Email.ToString(), font));
                                    table.AddCell(new Paragraph(line.Дата_рождения.ToString(), font));
                                    table.AddCell(new Paragraph(line.Стаж.ToString(), font));
                                    table.AddCell(new Paragraph(line.Адрес.ToString(), font));
                                    table.AddCell(new Paragraph(line.Дата_приема_на_работу.ToString(), font));
                                    table.AddCell(new Paragraph(line.Должность.ToString(), font));
                                    table.AddCell(new Paragraph(line.Серия_паспорта.ToString(), font));
                                    table.AddCell(new Paragraph(line.Номер_паспорта.ToString(), font));
                                    table.AddCell(new Paragraph(line.ИНН.ToString(), font));
                                    table.AddCell(new Paragraph(line.СНИЛС.ToString(), font));
                                    table.AddCell(new Paragraph(line.Номер_медицинского_полиса.ToString(), font));
                                    table.AddCell(new Paragraph(line.Номер_трудового_договора.ToString(), font));
                                    table.AddCell(new Paragraph(line.Срок_действия_договора.ToString(), font));
                                    if (line.Помещение != null)
                                        table.AddCell(new Paragraph(line.Помещение.ToString(), font));
                                    else
                                        table.AddCell(new Paragraph("", font));
                                    if (line.Класс != null)
                                        table.AddCell(new Paragraph(line.Класс.ToString(), font));
                                    else
                                        table.AddCell(new Paragraph("", font));
                                }

                                // Добавление таблицы и закрытие документа
                                doc.Add(table);
                                doc.Close();
                                break;

                            // Сохранение в CSV 
                            case 2:
                                // Открытие потока для записи файла
                                using (var CSVWriter = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                                {
                                    // Добавление заголовков
                                    CSVWriter.WriteLine("Код сотрудника,Фамилия,Имя,Отчество,Номер телефона,Email,Дата рождения,Стаж,Адрес,Дата приема на работу,Должность,Серия паспорта,Номер паспорта,ИНН,СНИЛС,Номер медицинского полиса,Номер трудового договора,Срок действия договора,Назначенное помещение,Назначенный класс");

                                    // Добавление полуенных данных
                                    foreach (var line in employeeData)
                                    {
                                        CSVWriter.WriteLine($"{line.Код_сотрудника},{line.Фамилия},{line.Имя},{line.Отчество},{line.Номер_телефона},{line.Email},{line.Дата_рождения},{line.Стаж},{line.Адрес},{line.Дата_приема_на_работу},{line.Должность},{line.Серия_паспорта},{line.Номер_паспорта},{line.ИНН},{line.СНИЛС},{line.Номер_медицинского_полиса},{line.Номер_трудового_договора},{line.Срок_действия_договора},{line.Помещение},{line.Класс}");
                                    }
                                }
                                break;

                            // Сохранение в Excel
                            case 3:
                                // Обявление лицензии
                                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                                // Создание испульземого пакета
                                var excelPackage = new ExcelPackage();

                                // Создание листа
                                var worksheet = excelPackage.Workbook.Worksheets.Add("Сотрудники");

                                // Добавление заголовков в ячейки
                                worksheet.Cells["A1"].Value = "Код сотрудника";
                                worksheet.Cells["B1"].Value = "Фамилия";
                                worksheet.Cells["C1"].Value = "Имя";
                                worksheet.Cells["D1"].Value = "Отчество";
                                worksheet.Cells["E1"].Value = "Номер телефона";
                                worksheet.Cells["F1"].Value = "Email";
                                worksheet.Cells["G1"].Value = "Дата рождения";
                                worksheet.Cells["H1"].Value = "Стаж";
                                worksheet.Cells["I1"].Value = "Адрес";
                                worksheet.Cells["J1"].Value = "Дата приема на работу";
                                worksheet.Cells["K1"].Value = "Должность";
                                worksheet.Cells["L1"].Value = "Серия паспорта";
                                worksheet.Cells["M1"].Value = "Номер паспорта";
                                worksheet.Cells["N1"].Value = "ИНН";
                                worksheet.Cells["O1"].Value = "СНИЛС";
                                worksheet.Cells["P1"].Value = "Номер медицинского полиса";
                                worksheet.Cells["Q1"].Value = "Номер трудового договора";
                                worksheet.Cells["R1"].Value = "Срок действия договора";
                                worksheet.Cells["S1"].Value = "Назначенное помещение";
                                worksheet.Cells["T1"].Value = "Назначенный класс";

                                // Заполнение полученными данными ячейки
                                int rowIndex = 2;
                                foreach (var employee in employeeData)
                                {
                                    worksheet.Cells[rowIndex, 5].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 12].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 13].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 14].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 15].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 16].Style.Numberformat.Format = "0";
                                    worksheet.Cells[rowIndex, 17].Style.Numberformat.Format = "0";


                                    worksheet.Cells[rowIndex, 1].Value = employee.Код_сотрудника;
                                    worksheet.Cells[rowIndex, 2].Value = employee.Фамилия;
                                    worksheet.Cells[rowIndex, 3].Value = employee.Имя;
                                    worksheet.Cells[rowIndex, 4].Value = employee.Отчество;
                                    worksheet.Cells[rowIndex, 5].Value = employee.Номер_телефона;
                                    worksheet.Cells[rowIndex, 6].Value = employee.Email;
                                    worksheet.Cells[rowIndex, 7].Value = employee.Дата_рождения;
                                    worksheet.Cells[rowIndex, 8].Value = employee.Стаж;
                                    worksheet.Cells[rowIndex, 9].Value = employee.Адрес;
                                    worksheet.Cells[rowIndex, 10].Value = employee.Дата_приема_на_работу;
                                    worksheet.Cells[rowIndex, 11].Value = employee.Должность;
                                    worksheet.Cells[rowIndex, 12].Value = employee.Серия_паспорта;
                                    worksheet.Cells[rowIndex, 13].Value = employee.Номер_паспорта;
                                    worksheet.Cells[rowIndex, 14].Value = employee.ИНН;
                                    worksheet.Cells[rowIndex, 15].Value = employee.СНИЛС;
                                    worksheet.Cells[rowIndex, 16].Value = employee.Номер_медицинского_полиса;
                                    worksheet.Cells[rowIndex, 17].Value = employee.Номер_трудового_договора;
                                    worksheet.Cells[rowIndex, 18].Value = employee.Срок_действия_договора;
                                    worksheet.Cells[rowIndex, 19].Value = employee.Помещение;
                                    worksheet.Cells[rowIndex, 20].Value = employee.Класс;

                                    rowIndex++;
                                }

                                // Выставление автоматической ширины для столбцов
                                worksheet.Cells.AutoFitColumns();

                                // Сохранение файла
                                excelPackage.SaveAs(new FileInfo(saveFileDialog.FileName));
                                break;
                        }

                        MessageBox.Show("Данные успешно экспортированны", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Режим экспорта
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (ChoiceOfPosition.SelectedItem.ToString() != "Все")
            {
                if (MessageBox.Show("Экспортировать данные с использованием выставленных фильтров?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    string post = "";

                    if (ChoiceOfPosition.SelectedItem.ToString() != "Все")
                        post = ChoiceOfPosition.SelectedItem.ToString();
                    else
                        post = null;

                    ExportData(post);
                }
                else
                {
                    ExportData(null);
                }
            }
            else
            {
                ExportData(null);
            }
        }

        // Выход на окно авторизации
        private void ChangeUser_Click(object sender, RoutedEventArgs e)
        {
            exitMode = true;
            Authorization authorization = new Authorization();
            authorization.Show();
            this.Close();
        }

        // Закрытие приложения
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Открытие окна графика расписания
        private void WorkSchedule_Click(object sender, RoutedEventArgs e)
        {
            DrawingUpWorkSchedule drawingUpWorkSchedule = new DrawingUpWorkSchedule();
            drawingUpWorkSchedule.ShowDialog();
        }

        // Отчет о зарплате сотрудников
        private void SalaryReport_Click(object sender, RoutedEventArgs e)
        {
            TempData.selectedReport = 0;
            MultiReport multiReport = new MultiReport();
            multiReport.ShowDialog();
        }

        // Отчет о отработанных часах
        private void ReportOnHoursWorked_Click(object sender, RoutedEventArgs e)
        {
            TempData.selectedReport = 1;
            MultiReport multiReport = new MultiReport();
            multiReport.ShowDialog();
        }
    }
}
