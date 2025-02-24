using MySql.Data.MySqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientsApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ClientsList : Window
    {
        private List<Client> clients = [];
        public ClientsList()
        {
            InitializeComponent();
            Refresh(ref clients);
        }
        private void Refresh(ref List<Client> clients)
        {
            if (clients.Count > 0)
            {
                ClientsView.ItemsSource = null;
                clients.Clear();
                ClientsView.Items.Clear();
            }
            MySqlConnection con = new(Datab.con);

            try
            {
                con.Open();
                MySqlCommand cmd = new(Datab.ReadCom(), con);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    clients.Add(new Client(dr.GetInt32(5), dr.GetInt32(8))
                    {
                        ID = dr.GetInt32(0),
                        FIO = dr.GetString(1) + dr.GetString(2),
                        Gender = dr.GetInt32(3) == 1 ? "М" : "Ж",
                        Address = dr.GetString(4),
                        Phone = dr.GetString(6),
                        Email = dr.GetString(7),
                        CreatedDate = dr.GetDateTime(9),
                    });
                }
                con.Close();

                ClientsView.ItemsSource = clients;
            }
            catch
            {
                MessageBox.Show("Отсутствует соединение с сервером");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWin win = new();
            Hide();
            win.ShowDialog();
            Refresh(ref clients);
            ShowDialog();
        }

        private void ClientsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ClientWin win = new(clients[ClientsView.SelectedIndex].ID);
            Hide();
            win.ShowDialog();
            Refresh(ref clients);
            ShowDialog();
        }
    }
}
