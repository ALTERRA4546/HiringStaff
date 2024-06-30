using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
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

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для DrawingUpWorkSchedule.xaml
    /// </summary>
    public partial class DrawingUpWorkSchedule : Window
    {
        public DrawingUpWorkSchedule()
        {
            InitializeComponent();
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

                StartingDate.SelectedDate = Convert.ToDateTime("01.01.2010");
                EndDate.SelectedDate = DateTime.Now;

                DatePicker startingDate = StartingDate;
                DatePicker endDate = EndDate;

                Initial(startingDate, endDate, null, null);
            }
        }

        // Загрузка данных в DataGrid
        private void Initial(DatePicker startingDate, DatePicker endDate, string search, string selectedPost)
        {
            using (var dataBase = new HiringStaffEntities())
            {
                var workingHoursData = (from workingHour in dataBase.График_работы
                                         join
                                         employee in dataBase.Сотрудник on workingHour.Код_сотрудника equals employee.Код_сотрудника into employeeGroup
                                         from employee in employeeGroup.DefaultIfEmpty()
                                         join
                                         post in dataBase.Должность on employee.Код_должности equals post.Код_должности into postGroup
                                         from post in postGroup.DefaultIfEmpty()
                                         join
                                         classes in dataBase.Классы on employee.Код_сотрудника equals classes.Код_классного_руководителя into classGroup
                                         from classes in classGroup.DefaultIfEmpty()
                                         where ((selectedPost == null || post.Наименование == ChoiceOfPosition.SelectedItem.ToString()) && (search == null || employee.Фамилия.ToLower().Contains(search) || employee.Имя.ToLower().Contains(search) || employee.Отчество.ToLower().Contains(search) || classes.Наименование.ToLower().Contains(search)) && (DateChecker.IsChecked == false || (workingHour.Дата_начала_работы >= startingDate.SelectedDate.Value && workingHour.Дата_окончания_работы <= endDate.SelectedDate.Value)))
                                         select new
                                         {
                                             workingHour.Код_графика_работы,
                                             employee.Фамилия,
                                             employee.Имя,
                                             employee.Отчество,
                                             Должность = post.Наименование,
                                             Класс = classes.Наименование,
                                             workingHour.Дата_начала_работы,
                                             workingHour.Дата_окончания_работы
                                         }).ToList();

                WorkingHoursData.ItemsSource = workingHoursData;
                WorkingHoursData.Columns[0].MaxWidth = 0;
            }
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

            Initial(StartingDate, EndDate, search, post);
        }

        // Фильтрация по дате
        private void StartingDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

         // Изменение состояния CheckBox
        private void DateChecker_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        // Фильтарция по должности
        private void ChoiceOfPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        // Поиск сотрудников
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        // Открытие окна редактирования рабочего времени
        private void WorkingHoursData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = (DataGridRow)WorkingHoursData.ItemContainerGenerator.ContainerFromIndex(WorkingHoursData.SelectedIndex);
            DataGridCell cell = WorkingHoursData.Columns[0].GetCellContent(row).Parent as DataGridCell;
            TempData.selectedWorkingHours = Convert.ToInt32(((TextBlock)cell.Content).Text);

            AddOrChangeWorkingHours addOrChangeWorkingHours = new AddOrChangeWorkingHours();
            addOrChangeWorkingHours.ShowDialog();
            Filter();
        }

        // Открытие окна добавления рабочего времени
        private void AddGraph_Click(object sender, RoutedEventArgs e)
        {
            TempData.selectedWorkingHours = -1;
            AddOrChangeWorkingHours addOrChangeWorkingHours = new AddOrChangeWorkingHours();
            addOrChangeWorkingHours.ShowDialog();
            Filter();
        }

        // Удаление графика
        private void RemoveGraph_Click(object sender, RoutedEventArgs e)
        {
            using (var dataBase = new HiringStaffEntities())
            {
                // Проверка выделенной строки
                if (WorkingHoursData.SelectedIndex < 0)
                {
                    MessageBox.Show("Строка для удаления не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Запрос на подтверждение действий
                if (MessageBox.Show("Вы действительно хотите удалить запись в графике работы?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    DataGridRow row = (DataGridRow)WorkingHoursData.ItemContainerGenerator.ContainerFromIndex(WorkingHoursData.SelectedIndex);
                    DataGridCell cell = WorkingHoursData.Columns[0].GetCellContent(row).Parent as DataGridCell;
                    int idRemoveGraph = Convert.ToInt32(((TextBlock)cell.Content).Text);

                    var removeGraph = dataBase.График_работы.FirstOrDefault(x => x.Код_графика_работы == idRemoveGraph);
                    dataBase.График_работы.Remove(removeGraph);
                    dataBase.SaveChanges();

                    Filter();
                    MessageBox.Show("Запись была удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        // Режим экспорта
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (ChoiceOfPosition.SelectedItem.ToString() != "Все" || DateChecker.IsChecked == true)
            {
                if (MessageBox.Show("Экспортировать данные с использованием выставленных фильтров?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    string post = "";
                    bool dateChecker = false;

                    if (ChoiceOfPosition.SelectedItem.ToString() != "Все")
                        post = ChoiceOfPosition.SelectedItem.ToString();
                    else
                        post = null;

                    if (DateChecker.IsChecked == true)
                        dateChecker = true;
                    else
                        dateChecker = false;

                    ExportData(StartingDate, EndDate, post, dateChecker);
                }
                else
                {
                    ExportData(StartingDate, EndDate, null, false);
                }
            }
            else
            {
                ExportData(StartingDate, EndDate, null, false);
            }
        }

        // Экспорт данных
        private void ExportData(DatePicker startingDate, DatePicker endDate, string selectedPost, bool dateChecker)
        {
            // Запрос места и тип сохраняемого файла
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|CSV файл (*.csv)|*.csv|Excel (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == true)
            {
                // Загрузка экспортируемых данных из базы данных
                using (var dataBase = new HiringStaffEntities())
                {
                    var employeeData = (from workingHour in dataBase.График_работы
                                            join
                                            employee in dataBase.Сотрудник on workingHour.Код_сотрудника equals employee.Код_сотрудника into employeeGroup
                                            from employee in employeeGroup.DefaultIfEmpty()
                                            join
                                            post in dataBase.Должность on employee.Код_должности equals post.Код_должности into postGroup
                                            from post in postGroup.DefaultIfEmpty()
                                            join
                                            classes in dataBase.Классы on employee.Код_сотрудника equals classes.Код_классного_руководителя into classGroup
                                            from classes in classGroup.DefaultIfEmpty()
                                            where ((selectedPost == null || post.Наименование == ChoiceOfPosition.SelectedItem.ToString()) && (DateChecker.IsChecked == false || (workingHour.Дата_начала_работы >= startingDate.SelectedDate.Value && workingHour.Дата_окончания_работы <= endDate.SelectedDate.Value)))
                                            select new
                                            {
                                                employee.Код_сотрудника,
                                                employee.Фамилия,
                                                employee.Имя,
                                                employee.Отчество,
                                                Должность = post.Наименование,
                                                Класс = classes.Наименование,
                                                workingHour.Дата_начала_работы,
                                                workingHour.Дата_окончания_работы
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
                            PdfPTable table = new PdfPTable(8);
                            table.WidthPercentage = 100;

                            // Создание заголовков таблицы
                            table.AddCell(new Paragraph("Код сотрудника", font));
                            table.AddCell(new Paragraph("Фамилия", font));
                            table.AddCell(new Paragraph("Имя", font));
                            table.AddCell(new Paragraph("Отчество", font));
                            table.AddCell(new Paragraph("Должность", font));
                            table.AddCell(new Paragraph("Назначенный класс", font));
                            table.AddCell(new Paragraph("Дата начала работы", font));
                            table.AddCell(new Paragraph("Дата окончания работы", font));

                            // Заполнение таблицы полученными данными
                            foreach (var line in employeeData)
                            {
                                table.AddCell(new Paragraph(line.Код_сотрудника.ToString(), font));
                                table.AddCell(new Paragraph(line.Фамилия.ToString(), font));
                                table.AddCell(new Paragraph(line.Имя.ToString(), font));
                                table.AddCell(new Paragraph(line.Отчество.ToString(), font));
                                table.AddCell(new Paragraph(line.Должность.ToString(), font));
                                if (line.Класс != null)
                                    table.AddCell(new Paragraph(line.Класс.ToString(), font));
                                else
                                    table.AddCell(new Paragraph("", font));
                                table.AddCell(new Paragraph(line.Дата_начала_работы.ToString(), font));
                                table.AddCell(new Paragraph(line.Дата_окончания_работы.ToString(), font));

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
                                CSVWriter.WriteLine("Код сотрудника,Фамилия,Имя,Отчество,Должность,Назначенный класс,Дата начала работы,Дата окончания работы");

                                // Добавление полуенных данных
                                foreach (var line in employeeData)
                                {
                                    CSVWriter.WriteLine($"{line.Код_сотрудника},{line.Фамилия},{line.Имя},{line.Отчество},{line.Должность},{line.Класс},{line.Дата_начала_работы},{line.Дата_окончания_работы}");
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
                            worksheet.Cells["E1"].Value = "Должность";
                            worksheet.Cells["F1"].Value = "Класс";
                            worksheet.Cells["G1"].Value = "Дата начала работы";
                            worksheet.Cells["H1"].Value = "Время начала работы";
                            worksheet.Cells["I1"].Value = "Дата окончания работы";
                            worksheet.Cells["J1"].Value = "Время окончания работы";

                            // Заполнение полученными данными ячейки
                            int rowIndex = 2;
                            foreach (var employee in employeeData)
                            {
                                worksheet.Cells[rowIndex, 7].Style.Numberformat.Format = "dd-MM-yyyy";
                                worksheet.Cells[rowIndex, 8].Style.Numberformat.Format = "HH:mm:ss";
                                worksheet.Cells[rowIndex, 9].Style.Numberformat.Format = "dd-MM-yyyy";
                                worksheet.Cells[rowIndex, 10].Style.Numberformat.Format = "HH:mm:ss";

                                worksheet.Cells[rowIndex, 1].Value = employee.Код_сотрудника;
                                worksheet.Cells[rowIndex, 2].Value = employee.Фамилия;
                                worksheet.Cells[rowIndex, 3].Value = employee.Имя;
                                worksheet.Cells[rowIndex, 4].Value = employee.Отчество;
                                worksheet.Cells[rowIndex, 5].Value = employee.Должность;
                                worksheet.Cells[rowIndex, 6].Value = employee.Класс;
                                worksheet.Cells[rowIndex, 7].Value = employee.Дата_начала_работы.Date;
                                worksheet.Cells[rowIndex, 8].Value = employee.Дата_начала_работы.TimeOfDay;
                                worksheet.Cells[rowIndex, 9].Value = employee.Дата_окончания_работы.Date;
                                worksheet.Cells[rowIndex, 10].Value = employee.Дата_окончания_работы.TimeOfDay;

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
    }
}
