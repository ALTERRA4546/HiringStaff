using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для RestoreDataBase.xaml
    /// </summary>
    public partial class RestoreDataBase : Window
    {
        public RestoreDataBase()
        {
            InitializeComponent();
        }

        // Состояния режимов
        

        // Включение режима полного востановления базы данных
        private void СheckingFullRestore_Click(object sender, RoutedEventArgs e)
        {
            if (СheckingFullRestore.IsChecked == false)
            {
                СheckingDifferentialRestore.IsChecked = false;
                СheckingTransactionLogRestore.IsChecked = false;

                PathDifferentialRestore.IsEnabled = false;
                PathTransactionLogRestore.IsEnabled = false;

                SelectedPathDifferentialRestore.IsEnabled = false;
                SelectedPathTransactionLogRestore.IsEnabled = false;
            }
        }

        // Выбор пути к файлу полной резервной копии базы данных
        private void SelectedPathFullRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"BackUp (*.bak)|*.bak";

            if (openFileDialog.ShowDialog() == true)
            { 
                PathFullRestore.Text = openFileDialog.FileName;
            }
        }

        // Включение режима разностного востановления базы данных
        private void СheckingDifferentialRestore_Click(object sender, RoutedEventArgs e)
        {
            if (СheckingDifferentialRestore.IsChecked == false)
            {
                СheckingTransactionLogRestore.IsChecked = false;

                PathDifferentialRestore.IsEnabled = false;
                PathTransactionLogRestore.IsEnabled = false;

                SelectedPathDifferentialRestore.IsEnabled = false;
                SelectedPathTransactionLogRestore.IsEnabled = false;
            }
            else
            {
                СheckingFullRestore.IsChecked = true;

                PathDifferentialRestore.IsEnabled = true;

                SelectedPathDifferentialRestore.IsEnabled = true;
            }
        }
        // Выбор пути к файлу разностной резервной копии базы данных
        private void SelectedPathDifferentialRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"BackUp (*.bak)|*.bak";

            if (openFileDialog.ShowDialog() == true)
            {
                PathDifferentialRestore.Text = openFileDialog.FileName;
            }

        }

        // Включение режима востановления журнала транзакций
        private void СheckingTransactionLogRestore_Click(object sender, RoutedEventArgs e)
        {
            if (СheckingTransactionLogRestore.IsChecked == false)
            {
                PathTransactionLogRestore.IsEnabled = false;

                SelectedPathTransactionLogRestore.IsEnabled = false;
            }
            else
            {
                СheckingFullRestore.IsChecked = true;
                СheckingDifferentialRestore.IsChecked = true;

                PathDifferentialRestore.IsEnabled = true;
                PathTransactionLogRestore.IsEnabled = true;

                SelectedPathDifferentialRestore.IsEnabled = true;
                SelectedPathTransactionLogRestore.IsEnabled = true;
            }
        }

        // Выбор пути к файлу журнала транзакций
        private void SelectedPathTransactionLogRestore_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = @"BackUp (*.bak)|*.bak";

            if (openFileDialog.ShowDialog() == true)
            {
                PathTransactionLogRestore.Text = openFileDialog.FileName;
            }
        }

        // Востановление базы данных
        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            using (var dataBase = new HiringStaffEntities())
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = @"BackUp (*.bak)|*.bak";

                if (openFileDialog.ShowDialog() == true)
                {
                    var backUp = dataBase.Database.SqlQuery<HiringStaffEntities>($@"USE master;  RESTORE LOG [HiringStaff] FROM DISK = '{openFileDialog.FileName}' WITH RECOVERY").ToList();
                    MessageBox.Show("Востановление выполнено", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
