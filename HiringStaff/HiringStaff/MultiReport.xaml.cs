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
using System.Data.Entity;

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для MultiReport.xaml
    /// </summary>
    public partial class MultiReport : Window
    {
        public MultiReport()
        {
            InitializeComponent();
        }

        // Загрузка данных в ComboBox
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                YearPanel.Visibility = Visibility.Collapsed;

                List<string> selectYear = new List<string>();
                for (int year = DateTime.Now.Year + 1; year > 1900; year--)
                    selectYear.Add(year.ToString());

                SelectedYear.ItemsSource = selectYear;
                SelectedYear.SelectedIndex = 0;
                Initialization();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка данных из запросов в DataGrid
        private void Initialization()
        {
            try
            {
                using (var dataBase = new HiringStaffEntities())
                {
                    switch (TempData.selectedReport)
                    {
                        case 0:
                            ReportName.Content = "Отчет о зараплате";
                            var salaryReport = (from employee in dataBase.Сотрудник
                                                join
                                                post in dataBase.Должность on employee.Код_должности equals post.Код_должности
                                                join
                                                salary in dataBase.Зарплаты_сотрудников on post.Код_должности equals salary.Код_должности
                                                where (salary.Стаж <= employee.Стаж)
                                                group new { employee, post, salary } by new { employee.Фамилия, employee.Имя, employee.Отчество, employee.Стаж, Должность = post.Наименование } into dataGroup
                                                select new
                                                {
                                                    dataGroup.Key.Фамилия,
                                                    dataGroup.Key.Имя,
                                                    dataGroup.Key.Отчество,
                                                    dataGroup.Key.Должность,
                                                    dataGroup.Key.Стаж,
                                                    Зарплата = dataGroup.Max(x => x.salary.Зарплата)
                                                }).ToList();
                            ReportData.ItemsSource = salaryReport;
                            break;

                        case 1:
                            ReportName.Content = "Отчет о отработанных часах";
                            YearPanel.Visibility = Visibility.Visible;
                            int year = Convert.ToInt32(SelectedYear.SelectedItem);
                            var hoursWorkedReport = (from employee in dataBase.Сотрудник
                                                     join graph in dataBase.График_работы on employee.Код_сотрудника equals graph.Код_сотрудника
                                                     where (ChekedSelectedYear.IsChecked == false || (ChekedSelectedYear.IsChecked == true && graph.Дата_начала_работы.Year == year && graph.Дата_окончания_работы.Year == year))
                                                     group new { employee, graph } by new { employee.Код_сотрудника, employee.Фамилия, employee.Имя, employee.Отчество } into employeeGroup
                                                     select new
                                                     {
                                                         employeeGroup.Key.Фамилия,
                                                         employeeGroup.Key.Имя,
                                                         employeeGroup.Key.Отчество,
                                                         Отработанное_время = employeeGroup.Sum(x => DbFunctions.DiffHours(x.graph.Дата_начала_работы, x.graph.Дата_окончания_работы))
                                                     }).ToList();

                            ReportData.ItemsSource = hoursWorkedReport;
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

        // Экспрот
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (TempData.selectedReport)
                {
                    case 0:
                        ExportingSalaryReport();
                        break;

                    case 1:
                        if (ChekedSelectedYear.IsChecked == true)
                        {
                            if (MessageBox.Show("Экспортировать отчет с учетом включеного фильтра?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                ExportingHoursWorkedReport();
                            }
                            else
                            {
                                ChekedSelectedYear.IsChecked = false;
                                ExportingHoursWorkedReport();
                            }
                        }
                        else
                        {
                            ExportingHoursWorkedReport();
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

        // Экспрот отчета о зарплатах
        private void ExportingSalaryReport()
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
                        var salaryReport = (from employee in dataBase.Сотрудник
                                            join
                                            post in dataBase.Должность on employee.Код_должности equals post.Код_должности
                                            join
                                            salary in dataBase.Зарплаты_сотрудников on post.Код_должности equals salary.Код_должности
                                            where (salary.Стаж <= employee.Стаж)
                                            group new { employee, post, salary } by new { employee.Код_сотрудника, employee.Фамилия, employee.Имя, employee.Отчество, employee.Стаж, Должность = post.Наименование } into dataGroup
                                            select new
                                            {
                                                dataGroup.Key.Код_сотрудника,
                                                dataGroup.Key.Фамилия,
                                                dataGroup.Key.Имя,
                                                dataGroup.Key.Отчество,
                                                dataGroup.Key.Должность,
                                                dataGroup.Key.Стаж,
                                                Зарплата = dataGroup.Max(x => x.salary.Зарплата)
                                            }).ToList();

                        // Сохранение в выбранном формате
                        switch (saveFileDialog.FilterIndex)
                        {
                            // Сохранение в PDF
                            case 1:
                                // Создание документа
                                Document doc = new Document();
                                PdfWriter PDFWriter = PdfWriter.GetInstance(doc, new FileStream($@"{saveFileDialog.FileName}", FileMode.Create));
                                doc.SetPageSize(PageSize.A3);

                                // Создание шрифта
                                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                                Font font = new Font(baseFont, 8);

                                // Открытие документа
                                doc.Open();

                                // Создание таблицы
                                PdfPTable table = new PdfPTable(7);
                                table.WidthPercentage = 100;

                                // Создание заголовков таблицы
                                table.AddCell(new Paragraph("Код сотрудника", font));
                                table.AddCell(new Paragraph("Фамилия", font));
                                table.AddCell(new Paragraph("Имя", font));
                                table.AddCell(new Paragraph("Отчество", font));
                                table.AddCell(new Paragraph("Должность", font));
                                table.AddCell(new Paragraph("Стаж", font));
                                table.AddCell(new Paragraph("Зарплата", font)); ;

                                // Заполнение таблицы полученными данными
                                foreach (var line in salaryReport)
                                {
                                    table.AddCell(new Paragraph(line.Код_сотрудника.ToString(), font));
                                    table.AddCell(new Paragraph(line.Фамилия.ToString(), font));
                                    table.AddCell(new Paragraph(line.Имя.ToString(), font));
                                    table.AddCell(new Paragraph(line.Отчество.ToString(), font));
                                    table.AddCell(new Paragraph(line.Должность.ToString(), font));
                                    table.AddCell(new Paragraph(line.Стаж.ToString(), font));
                                    table.AddCell(new Paragraph(line.Зарплата.ToString(), font));

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
                                    CSVWriter.WriteLine("Код сотрудника,Фамилия,Имя,Отчество,Должность,Стаж,Зарплата");

                                    // Добавление полуенных данных
                                    foreach (var line in salaryReport)
                                    {
                                        CSVWriter.WriteLine($"{line.Код_сотрудника},{line.Фамилия},{line.Имя},{line.Отчество},{line.Должность},{line.Стаж},{line.Зарплата}");
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
                                worksheet.Cells["F1"].Value = "Стаж";
                                worksheet.Cells["G1"].Value = "Зарплата";

                                // Заполнение полученными данными ячейки
                                int rowIndex = 2;
                                foreach (var employee in salaryReport)
                                {
                                    worksheet.Cells[rowIndex, 7].Style.Numberformat.Format = "₽0.00";

                                    worksheet.Cells[rowIndex, 1].Value = employee.Код_сотрудника;
                                    worksheet.Cells[rowIndex, 2].Value = employee.Фамилия;
                                    worksheet.Cells[rowIndex, 3].Value = employee.Имя;
                                    worksheet.Cells[rowIndex, 4].Value = employee.Отчество;
                                    worksheet.Cells[rowIndex, 5].Value = employee.Должность;
                                    worksheet.Cells[rowIndex, 6].Value = employee.Стаж;
                                    worksheet.Cells[rowIndex, 7].Value = employee.Зарплата;

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

        // Экспорт отчета о отработанных часах
        private void ExportingHoursWorkedReport()
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
                        int year = Convert.ToInt32(SelectedYear.SelectedItem);
                        var hoursWorkedReport = (from employee in dataBase.Сотрудник
                                                 join graph in dataBase.График_работы on employee.Код_сотрудника equals graph.Код_сотрудника
                                                 where (ChekedSelectedYear.IsChecked == false || (ChekedSelectedYear.IsChecked == true && graph.Дата_начала_работы.Year == year && graph.Дата_окончания_работы.Year == year))
                                                 group new { employee, graph } by new { employee.Код_сотрудника, employee.Фамилия, employee.Имя, employee.Отчество } into employeeGroup
                                                 select new
                                                 {
                                                     employeeGroup.Key.Код_сотрудника,
                                                     employeeGroup.Key.Фамилия,
                                                     employeeGroup.Key.Имя,
                                                     employeeGroup.Key.Отчество,
                                                     Отработанное_время = employeeGroup.Sum(x => DbFunctions.DiffHours(x.graph.Дата_начала_работы, x.graph.Дата_окончания_работы))
                                                 }).ToList();

                        // Сохранение в выбранном формате
                        switch (saveFileDialog.FilterIndex)
                        {
                            // Сохранение в PDF
                            case 1:
                                // Создание документа
                                Document doc = new Document();
                                PdfWriter PDFWriter = PdfWriter.GetInstance(doc, new FileStream($@"{saveFileDialog.FileName}", FileMode.Create));
                                doc.SetPageSize(PageSize.A3);

                                // Создание шрифта
                                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                                Font font = new Font(baseFont, 8);

                                // Открытие документа
                                doc.Open();

                                // Создание таблицы
                                PdfPTable table = new PdfPTable(5);
                                table.WidthPercentage = 100;

                                // Создание заголовков таблицы
                                table.AddCell(new Paragraph("Код сотрудника", font));
                                table.AddCell(new Paragraph("Фамилия", font));
                                table.AddCell(new Paragraph("Имя", font));
                                table.AddCell(new Paragraph("Отчество", font));
                                table.AddCell(new Paragraph("Отработанное время", font)); ;

                                // Заполнение таблицы полученными данными
                                foreach (var line in hoursWorkedReport)
                                {
                                    table.AddCell(new Paragraph(line.Код_сотрудника.ToString(), font));
                                    table.AddCell(new Paragraph(line.Фамилия.ToString(), font));
                                    table.AddCell(new Paragraph(line.Имя.ToString(), font));
                                    table.AddCell(new Paragraph(line.Отчество.ToString(), font));
                                    table.AddCell(new Paragraph(line.Отработанное_время.ToString(), font));

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
                                    CSVWriter.WriteLine("Код сотрудника,Фамилия,Имя,Отчество,Отработанное время");

                                    // Добавление полуенных данных
                                    foreach (var line in hoursWorkedReport)
                                    {
                                        CSVWriter.WriteLine($"{line.Код_сотрудника},{line.Фамилия},{line.Имя},{line.Отчество},{line.Отработанное_время}");
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
                                worksheet.Cells["E1"].Value = "Отработанное время";

                                // Заполнение полученными данными ячейки
                                int rowIndex = 2;
                                foreach (var employee in hoursWorkedReport)
                                {
                                    worksheet.Cells[rowIndex, 7].Style.Numberformat.Format = "₽0.00";

                                    worksheet.Cells[rowIndex, 1].Value = employee.Код_сотрудника;
                                    worksheet.Cells[rowIndex, 2].Value = employee.Фамилия;
                                    worksheet.Cells[rowIndex, 3].Value = employee.Имя;
                                    worksheet.Cells[rowIndex, 4].Value = employee.Отчество;
                                    worksheet.Cells[rowIndex, 5].Value = employee.Отработанное_время;

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

        // Включение фильтрации по году
        private void ChekedSelectedYear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Initialization();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Изменение года
        private void SelectedYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Initialization();
            }
            catch (Exception ex)
            {
                // Обработка искючений
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
