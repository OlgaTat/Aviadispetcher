using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Aviadispetcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //список записів із файлу даних
        public List<Flight> fList = new List<Flight>(85);

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// відкриття файлу БД зі списком рейсів
        /// </summary>
        private void OpenDbFile()
        {
            try
            {
                //рядок з'єднання з БД
                string connStr = "Server = localhost; Database = aviadispetcher; Uid = root; Pwd = ;";
                //змінні для роботи із MySQL
                MySqlConnection conn = new MySqlConnection(connStr);
                MySqlCommand command = new MySqlCommand();
                //запит вибору даних із БД
                string commandString = "SELECT * FROM rozklad;";
                command.CommandText = commandString;
                command.Connection = conn;
                //читання відповіді сервера
                MySqlDataReader reader;
                command.Connection.Open();
                reader = command.ExecuteReader();
                //запис зчитаних даних у масив та список для виведення на екран
                int i = 0;
                while (reader.Read())
                {
                    //формування запису про рейс, зчитаного із БД
                    fList.Add(new Flight((string)reader["number"], (string)reader["city"], (System.TimeSpan)reader["depature_time"], (int)reader["free_seats"]));
                    i += 1;
                }
                reader.Close();
                //передача колекції рейсів у DataGrid 
                flightListDG.ItemsSource = fList;
            }
            catch (Exception ex)
            {
                string errMsg = "";
                if (ex.Message == "Unable to connect to any of the specified MySQL hosts.")
                {
                    errMsg = "Підключіть веб-сервер MySQL та виконайте команду Файл-Завантажити";
                }
                else
                {
                    errMsg = "Для завантаження файлу виконайте команду Файл-Завантажити";
                }               
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) + char.ConvertFromUtf32(13) +
                                errMsg, "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// завантаження головної форми програми
        /// </summary>
        private void InfoFlightForm_Loaded(object sender, RoutedEventArgs e)
        {
            //читання даних із БД
            OpenDbFile();
            //зміна розмірів форми під розміри DataGrid
            this.Width = flightListDG.Margin.Left+ flightListDG.RenderSize.Width+50;
            this.Height = flightListDG.Margin.Top + flightListDG.RenderSize.Height + 50;

            numFlightTextBox.Text = "";
        }

        /// <summary>
        /// виклик головного меню Файл-Завантажити
        /// виконує очистку flightListDG (DataGrid) від даних та
        /// завантажує у нього дані із БД aviadispetcher.sql
        /// </summary>
        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //очищення DataGrid
            try
            {
                flightListDG.ItemsSource = null;
                fList.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13),
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            //читання даних із БД
            OpenDbFile();
        }
    }
}
