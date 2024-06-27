using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Xml.Linq;
using static HiringStaff.Authorization;
using static System.Net.Mime.MediaTypeNames;

namespace HiringStaff
{
    /// <summary>
    /// Логика взаимодействия для AddOrChangeEmployee.xaml
    /// </summary>
    public partial class AddOrChangeEmployee : Window
    {
        public AddOrChangeEmployee()
        {
            InitializeComponent();
        }

        // Путь фотографии
        public string photoPath;

        // Переменая для хранениея статуса изменения преподоваемых предметов
        public bool changeItem = false;

        // Патерн для List itemList
        public class Items
        {
            public int Код_предмета { get; set; }
            public string Наименование { get; set; }
        }

        // Хранение данных о преподоваемых предметов
        public List<Items> itemList = new List<Items>();

        // Загрузка данных
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Photo.Visibility = Visibility.Collapsed;
            using (var dataBase = new HiringStaffEntities())
            {
                // Загрузка данных в ComboBox
                var postData = dataBase.Должность.Select(x => x.Наименование).ToList();
                var roomData = dataBase.Помещение.Select(x => x.Наименование).ToList();
                var classData = dataBase.Классы.Select(x => x.Наименование).ToList();
                var itemData = dataBase.Предмет.Select(x => x.Наименование).ToList();
                roomData.Add("Нет");
                classData.Add("Нет");

                Post.ItemsSource = postData;
                if(postData != null)
                    Post.SelectedIndex = 0;
                Room.ItemsSource = roomData;
                Room.SelectedItem = "Нет";
                Class.ItemsSource = classData;
                Class.SelectedItem = "Нет";
                Item.ItemsSource = itemData;

                ItemList.ItemsSource = itemList;

                // Загрузка данных сотрудника
                if (TempData.selectedEmployee != -1)
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
                                       room in dataBase.Помещение on employee.Код_сотрудника equals room.Код_отвественного_сотрудника into roomGroup
                                       from room in roomGroup.DefaultIfEmpty()
                                       join
                                       classes in dataBase.Классы on employee.Код_сотрудника equals classes.Код_классного_руководителя into classesGroup
                                       from classes in classesGroup.DefaultIfEmpty()
                                       where (employee.Код_сотрудника == TempData.selectedEmployee)
                                       select new
                                       {
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
                                           documents.Номер_медицинского_полюса,
                                           documents.Фотография,
                                           documents.Номер_трудового_договора,
                                           documents.Срок_действия_договора,
                                           authorization.Логин,
                                           authorization.Пароль,
                                           Помещение = room.Наименование,
                                           Класс = classes.Наименование
                                       }).FirstOrDefault();

                    Surname.Text = employeeData.Фамилия;
                    Name.Text = employeeData.Имя;
                    Patronymic.Text = employeeData.Отчество;
                    Phone.Text = employeeData.Номер_телефона;
                    Email.Text = employeeData.Email;
                    Birthday.SelectedDate = employeeData.Дата_рождения;
                    Experience.Text = employeeData.Стаж.ToString();
                    Address.Text = employeeData.Адрес;
                    DateOfEmployment.SelectedDate = employeeData.Дата_приема_на_работу;
                    Post.SelectedItem = employeeData.Должность;

                    PassportSeries.Text = employeeData.Серия_паспорта.ToString();
                    PassportNumber.Text = employeeData.Номер_паспорта.ToString();
                    INN.Text = employeeData.ИНН.ToString();
                    MedicalPoleNumber.Text = employeeData.Номер_медицинского_полюса.ToString();
                    NumberfEmploymentContract.Text = employeeData.Номер_трудового_договора.ToString();
                    TermAgreement.Text = employeeData.Срок_действия_договора.ToString();
                    if (employeeData.Помещение != null)
                    {
                        Room.SelectedItem = employeeData.Помещение;
                    }
                    if (employeeData.Класс != null)
                    {
                        Class.SelectedItem = employeeData.Класс;
                    }
                    Login.Text = employeeData.Логин;
                    Password.Text = employeeData.Пароль;

                    if (employeeData.Фотография != null)
                    {
                        byte[] imageData = employeeData.Фотография;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(imageData);
                        bitmapImage.EndInit();

                        Photo.Visibility = Visibility.Visible;
                        Photo.Source = bitmapImage;
                    }

                    // Загрузка данных о преподоваемых предметов
                    var selectedItemData = (from employee in dataBase.Сотрудник
                                           join
                                           subjectsTaught in dataBase.Преподаваемые_предметы on employee.Код_сотрудника equals subjectsTaught.Код_учителя
                                           join
                                           item in dataBase.Предмет on subjectsTaught.Код_предмета equals item.Код_предмета
                                           where (employee.Код_сотрудника == TempData.selectedEmployee)
                                           select new
                                           {
                                               subjectsTaught.Код_преподоваемого_предмета,
                                               item.Код_предмета,
                                               item.Наименование
                                           }).ToList();

                    foreach (var line in selectedItemData)
                    {
                        Items tempItem = new Items();
                        tempItem.Код_предмета = line.Код_предмета;
                        tempItem.Наименование = line.Наименование;
                        itemList.Add(tempItem);
                    }
                    ItemList.Items.Refresh();
                }
            }
        }

        // Сохраненние изменений
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Проверка заполнения данных
            if (Surname.Text == "" || Name.Text == "" || Patronymic.Text == "" || Birthday.SelectedDate == null || Experience.Text == "" || Address.Text == "" || DateOfEmployment.SelectedDate == null)
            {
                MessageBox.Show("Данные не введены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string patern = @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$";
            if (Regex.IsMatch(Phone.Text.ToString(),patern) == false)
            {
                MessageBox.Show("Номер телефона введен в неверном формате (+7(999)999-99-99)", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Email.Text == "" || (Email.Text.Contains("@mail.ru") == false && Email.Text.Contains("@gmail.com") == false))
            {
                MessageBox.Show("Email введен неверно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (int.TryParse(Experience.Text, out _) == false)
            {
                MessageBox.Show("Формат стажа указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (int.TryParse(PassportSeries.Text,out _) == false || PassportSeries.Text.Length != 4)
            {
                MessageBox.Show("Формат серии паспорта указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(int.TryParse(PassportNumber.Text, out _) == false || PassportNumber.Text.Length != 6)
            {
                MessageBox.Show("Формат номера паспорта указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(UInt64.TryParse(INN.Text, out _) == false || (INN.Text.Length < 10 || INN.Text.Length > 12))
            {
                MessageBox.Show("Формат ИНН указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (UInt64.TryParse(SNILS.Text, out _) == false || SNILS.Text.Length == 11)
            {
                MessageBox.Show("Формат СНИЛС указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(UInt64.TryParse(MedicalPoleNumber.Text, out _) == false || MedicalPoleNumber.Text.Length != 16)
            {
                MessageBox.Show("Формат номера мед. полюса указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(UInt64.TryParse(NumberfEmploymentContract.Text, out _) == false)
            {
                MessageBox.Show("Формат номера труд. договора указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(int.TryParse(TermAgreement.Text, out _) == false)
            {
                MessageBox.Show("Формат срок действия договора указан не верно", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(Login.Text == "" || Password.Text == "")
            {
                MessageBox.Show("Логин или пароль не введены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Сохранений изменений
            using (var dataBase = new HiringStaffEntities())
            {
                var checkLogin = dataBase.Авторизация.FirstOrDefault(x=>x.Логин == Login.Text);
                if (checkLogin != null && TempData.selectedEmployee == -1)
                {
                    MessageBox.Show("Данный логин уже занят", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (TempData.selectedEmployee == -1)
                {
                    var employee = new Сотрудник();
                    employee.Фамилия = Surname.Text;
                    employee.Имя = Name.Text;
                    employee.Отчество = Patronymic.Text;
                    employee.Номер_телефона = Phone.Text;
                    employee.Email = Email.Text;
                    employee.Дата_рождения = Birthday.SelectedDate.Value;
                    employee.Стаж = Convert.ToInt32(Experience.Text);
                    employee.Адрес = Address.Text;
                    employee.Дата_приема_на_работу = DateOfEmployment.SelectedDate.Value;
                    var idSelectedPost = dataBase.Должность.FirstOrDefault(x => x.Наименование == Post.SelectedItem.ToString());
                    employee.Код_должности = idSelectedPost.Код_должности;
                    dataBase.Сотрудник.Add(employee);
                    dataBase.SaveChanges();

                    var documents = new Документы();
                    documents.Код_сотрудника = employee.Код_сотрудника;
                    documents.Серия_паспорта = Convert.ToInt32(PassportSeries.Text);
                    documents.Номер_паспорта = Convert.ToInt32(PassportNumber.Text);
                    documents.ИНН = Convert.ToInt64(INN.Text);
                    documents.СНИЛС = Convert.ToInt64(SNILS.Text);
                    documents.Номер_медицинского_полюса = Convert.ToInt64(MedicalPoleNumber.Text);
                    documents.Номер_трудового_договора = Convert.ToInt64(NumberfEmploymentContract.Text);
                    documents.Срок_действия_договора = Convert.ToInt32(TermAgreement.Text);
                    if (photoPath != null)
                    {
                        byte[] imageData = File.ReadAllBytes(photoPath);
                        documents.Фотография = imageData;
                    }
                    dataBase.Документы.Add(documents);

                    if (Room.SelectedItem.ToString() != "Нет")
                    {
                        var idSelectedRoom = dataBase.Помещение.FirstOrDefault(x => x.Наименование == Room.SelectedItem.ToString());
                        var room = dataBase.Помещение.FirstOrDefault(x => x.Код_помещения == idSelectedRoom.Код_помещения);
                        room.Код_отвественного_сотрудника = employee.Код_сотрудника;
                    }

                    if (Class.SelectedItem.ToString() != "Нет")
                    {
                        var idSelectedClass = dataBase.Классы.FirstOrDefault(x => x.Наименование == Class.SelectedItem.ToString());
                        var classes = dataBase.Классы.FirstOrDefault(x => x.Код_класса == idSelectedClass.Код_класса);
                        classes.Код_классного_руководителя = employee.Код_сотрудника;
                    }

                    var authorization = new Авторизация();
                    authorization.Код_сотрудника = employee.Код_сотрудника;
                    authorization.Логин = Login.Text;
                    authorization.Пароль = Password.Text;
                    dataBase.Авторизация.Add(authorization);

                    if (itemList.Count != 0)
                    {
                        foreach (var line in itemList)
                        {
                            var subjectTaught = new Преподаваемые_предметы();
                            subjectTaught.Код_учителя = employee.Код_сотрудника;
                            subjectTaught.Код_предмета = line.Код_предмета;
                            dataBase.Преподаваемые_предметы.Add(subjectTaught);
                        }
                    }

                    dataBase.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                else
                {
                    var employee = dataBase.Сотрудник.FirstOrDefault(x=>x.Код_сотрудника == TempData.selectedEmployee);
                    employee.Фамилия = Surname.Text;
                    employee.Имя = Name.Text;
                    employee.Отчество = Patronymic.Text;
                    employee.Номер_телефона = Phone.Text;
                    employee.Email = Email.Text;
                    employee.Дата_рождения = Birthday.SelectedDate.Value;
                    employee.Стаж = Convert.ToInt32(Experience.Text);
                    employee.Адрес = Address.Text;
                    employee.Дата_приема_на_работу = DateOfEmployment.SelectedDate.Value;
                    var idSelectedPost = dataBase.Должность.FirstOrDefault(x => x.Наименование == Post.SelectedItem.ToString());
                    employee.Код_должности = idSelectedPost.Код_должности;

                    var documents = dataBase.Документы.FirstOrDefault(x=>x.Код_сотрудника == TempData.selectedEmployee);
                    documents.Серия_паспорта = Convert.ToInt32(PassportSeries.Text);
                    documents.Номер_паспорта = Convert.ToInt32(PassportNumber.Text);
                    documents.ИНН = Convert.ToInt64(INN.Text);
                    documents.Номер_медицинского_полюса = Convert.ToInt64(MedicalPoleNumber.Text);
                    documents.Номер_трудового_договора = Convert.ToInt64(NumberfEmploymentContract.Text);
                    documents.Срок_действия_договора = Convert.ToInt32(TermAgreement.Text);
                    if (photoPath != null)
                    {
                        byte[] imageData = File.ReadAllBytes(photoPath);
                        documents.Фотография = imageData;
                    }

                    if (Room.SelectedItem.ToString() != "Нет")
                    {
                        var idSelectedRoom = dataBase.Помещение.FirstOrDefault(x => x.Наименование == Room.SelectedItem.ToString());
                        var room = dataBase.Помещение.FirstOrDefault(x => x.Код_помещения == idSelectedRoom.Код_помещения);
                        room.Код_отвественного_сотрудника = TempData.selectedEmployee;
                    }
                    else
                    {
                        var idSelectedRoom = dataBase.Помещение.FirstOrDefault(x => x.Код_отвественного_сотрудника == TempData.selectedEmployee);
                        if (idSelectedRoom != null)
                        {
                            var room = dataBase.Помещение.FirstOrDefault(x => x.Код_помещения == idSelectedRoom.Код_помещения);
                            room.Код_отвественного_сотрудника = null;
                        }
                    }

                    if (Class.SelectedItem.ToString() != "Нет")
                    {
                        var idSelectedClass = dataBase.Классы.FirstOrDefault(x => x.Наименование == Class.SelectedItem.ToString());
                        var classes = dataBase.Классы.FirstOrDefault(x => x.Код_класса == idSelectedClass.Код_класса);
                        classes.Код_классного_руководителя = TempData.selectedEmployee;
                    }
                    else
                    {
                        var idSelectedClass = dataBase.Классы.FirstOrDefault(x => x.Код_классного_руководителя == TempData.selectedEmployee);
                        if (idSelectedClass != null)
                        {
                            var classes = dataBase.Классы.FirstOrDefault(x => x.Код_класса == idSelectedClass.Код_класса);
                            classes.Код_классного_руководителя = null;
                        }
                    }

                    var authorization = dataBase.Авторизация.FirstOrDefault(x=>x.Код_сотрудника == TempData.selectedEmployee);
                    authorization.Логин = Login.Text;
                    authorization.Пароль = Password.Text;

                    if (itemList.Count != 0 && changeItem == true)
                    {
                        var itemData = (from employees in dataBase.Сотрудник
                                                join
                                                subjectsTaught in dataBase.Преподаваемые_предметы on employees.Код_сотрудника equals subjectsTaught.Код_учителя
                                                join
                                                item in dataBase.Предмет on subjectsTaught.Код_предмета equals item.Код_предмета
                                                where (employees.Код_сотрудника == TempData.selectedEmployee)
                                                select new
                                                {
                                                    subjectsTaught.Код_преподоваемого_предмета,
                                                    item.Код_предмета,
                                                    item.Наименование
                                                }).ToList();


                        foreach (var line in itemData)
                        {
                            var removeData = dataBase.Преподаваемые_предметы.FirstOrDefault(x=>x.Код_преподоваемого_предмета == line.Код_преподоваемого_предмета);
                            dataBase.Преподаваемые_предметы.Remove(removeData);
                        }

                        foreach (var line in itemList)
                        {
                            var subjectTaught = new Преподаваемые_предметы();
                            subjectTaught.Код_учителя = TempData.selectedEmployee;
                            subjectTaught.Код_предмета = line.Код_предмета;
                            dataBase.Преподаваемые_предметы.Add(subjectTaught);
                        }
                    }

                    dataBase.SaveChanges();
                    MessageBox.Show("Данные сохранены", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
        }

        // Загрузка фото в Image
        private void LoadPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "image(*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (fileDialog.ShowDialog() == true)
            {
                photoPath = fileDialog.FileName;
                ImageSource imageSource = new BitmapImage(new Uri(fileDialog.FileName));
                Photo.Source = imageSource;
                Photo.Visibility = Visibility.Visible;
            }
        }

        // Добавление предмета в List
        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            // Преверка на дублирование предмета
            foreach (var line in itemList)
            {
                if (line.Наименование == Item.SelectedItem.ToString())
                {
                    MessageBox.Show("Данный предмет уже добавлен","Внимание",MessageBoxButton.OK,MessageBoxImage.Warning);
                    return;
                }    
            }

            // Добавление предмета в List и его отображение
            using (var dataBase = new HiringStaffEntities())
            {
                var itemData = dataBase.Предмет.FirstOrDefault(x => x.Наименование == Item.SelectedItem.ToString());
                Items tempItemData = new Items();
                tempItemData.Код_предмета = itemData.Код_предмета;
                tempItemData.Наименование = itemData.Наименование;
                itemList.Add(tempItemData);
                ItemList.Items.Refresh();
                changeItem = true;
            }
        }

        // Удаление предмета из List и его отображение
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemList.SelectedIndex < 0)
            {
                MessageBox.Show("Предмет для удаления не выборан", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            itemList.RemoveAt(ItemList.SelectedIndex);
            ItemList.Items.Refresh();
            changeItem = true;
        }
    }
}
