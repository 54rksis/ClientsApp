using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace ClientsApp
{
    /// <summary>
    /// Логика взаимодействия для ClientWin.xaml
    /// </summary>
    public partial class ClientWin : Window
    {
        private const int MaxLength = 8;
        private List<City> cities = [];
        private int clID = 0;
        private bool IsValidPhone = false;
        public ClientWin()
        {
            InitializeComponent();
            ComB_ref(ref cities);
            AddB.Content = "Добавить";
        }
        public ClientWin(int ID)
        {
            InitializeComponent();
            AddB.Content = "Изменить";
            ComB_ref(ref cities);
            MySqlConnection con = new(Datab.con);
            try
            {
                con.Open();
                MySqlCommand cmd = new(Datab.ReadCom(ID), con);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                {
                    clID = r.GetInt32(0);
                    FirstNameB.Text = r.GetString(1);
                    LastNameB.Text = r.GetString(2);
                    GenComb.SelectedIndex = r.GetInt32(3) - 1;
                    AddressB.Text = r.GetString(4);
                    CityComb.SelectedIndex = r.GetInt32(5);
                    NumberTextBox.Text = r.GetString(6);
                    EmailB.Text = r.GetString(7);
                    StatusB.SelectedIndex = r.GetInt32(8);
                }
                con.Close();
            }
            catch { }
        }
        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Регулярное выражение для проверки формата xxx-xx-xx
            Regex regex = new Regex(@"^\d{0,3}-?\d{0,2}-?\d{0,2}$");
            string newText = NumberTextBox.Text.Insert(NumberTextBox.SelectionStart, e.Text);

            // Проверка, соответствует ли новый текст формату
            if (!regex.IsMatch(newText))
            {
               IsValidPhone = false;
            }
            else
            {
                IsValidPhone = true;
            }
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Регулярное выражение для проверки корректности email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        private void ComB_ref(ref List<City> cities)
        {
            if (cities.Count > 0)
            {
                CityComb.ItemsSource = null;
                CityComb.Items.Clear();
                cities.Clear();
            }
            MySqlConnection con = new(Datab.con);
            con.Open();
            MySqlCommand cmd = new(Datab.ReadCom_City(), con);
            MySqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                cities.Add(new City() { Id = r.GetInt32(0), Name = r.GetString(1) });
            }
            con.Close();
            CityComb.ItemsSource = cities;
        }

        private void AddB_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidPhone)
            {
                if (IsValidEmail(EmailB.Text))
                {
                    if (
                        FirstNameB.Text != "" && LastNameB.Text != "" &&
                        CityComb.SelectedIndex != -1 && GenComb.SelectedIndex != -1 &&
                        StatusB.SelectedIndex != -1 && NumberTextBox.Text != "" &&
                        AddressB.Text != "" && EmailB.Text != "" && DateB.SelectedDate != null
                      )
                    {
                        MySqlConnection con = new(Datab.con);
                        try
                        {
                            if (clID == 0)
                            {
                                con.Open();
                                MySqlCommand cmd = new(Datab.AddCom
                                    (
                                    FirstNameB.Text, LastNameB.Text,
                                    GenComb.SelectedIndex + 1,
                                    AddressB.Text,
                                    CityComb.SelectedIndex + 1,
                                    NumberTextBox.Text,
                                    EmailB.Text,
                                    StatusB.SelectedIndex + 1,
                                    DateB.SelectedDate.Value.ToString("yyy-MM-dd")
                                    ), con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Добавлено успешно!");
                                Close();
                            }
                            else
                            {
                                con.Open();
                                MySqlCommand cmd = new(Datab.UptCom
                                    (
                                    clID,
                                    FirstNameB.Text, LastNameB.Text,
                                    GenComb.SelectedIndex + 1,
                                    AddressB.Text,
                                    CityComb.SelectedIndex + 1,
                                    NumberTextBox.Text,
                                    EmailB.Text,
                                    StatusB.SelectedIndex + 1,
                                    DateB.SelectedDate.Value.ToString("yyy-MM-dd")
                                    ), con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                MessageBox.Show("Изменено успешно!");
                                Close();
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Нет подключения к базе данных");
                        }
                    }
                    else MessageBox.Show("Ошибка! Вы не ввели некоторые данные.");
                }
                else MessageBox.Show("Почта введена некорректно.");
            }
            else MessageBox.Show("Телефон введён неверно. Формат: xxx-xx-xx.");
        }
    }
}
