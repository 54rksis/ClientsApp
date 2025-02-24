using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientsApp
{
    class Client
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int CityID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Client(int cityID, int statusID)
        {
            CityID = cityID;
            StatusID = statusID;
            try
            {
                string st = "";
                MySqlConnection con = new(Datab.con);
                con.Open();
                MySqlCommand cmd = new(Datab.ReadCom_City(cityID), con);
                MySqlDataReader r = cmd.ExecuteReader();
                if (r.Read())
                    City = r.GetString(1);
                con.Close();

                con.Open();
                cmd = new(Datab.ReadCom_Status(statusID), con);
                r = cmd.ExecuteReader();
                if (r.Read())
                    st = r.GetString(1);
                con.Close();
                switch (st)
                {
                    case "avaliable":
                        Status = "Свободен";
                        break;
                    case "Busy":
                        Status = "Занят";
                        break;
                    case "Do Not Disturb":
                        Status = "Не беспокоить";
                        break;
                    case "Offline":
                        Status = "Не в сети";
                        break;
                    default:
                        Status = "Свободен";
                        break;
                }
            }
            catch
            {}
        }
        public override string ToString()
        {
            return FIO + "\t" + "Пол: " + Gender + "\n" + Address + "\t" + City + "\n" + Phone + "\t" + Email + "\n" + "Статус: " + Status + "\n" + "Дата заключения договора: " + CreatedDate.ToShortDateString();
        }
    }
}
