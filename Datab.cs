using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientsApp
{
    class Datab
    {
        public const string con = "server=127.0.0.1;port=3306;userid=root;password=root;database=client";

        public static string ReadCom()
        {
            return "SELECT * FROM `clients`";
        }
        public static string ReadCom(int ID)
        {
            return $"SELECT * FROM `clients` WHERE `ID` = {ID}";
        }
        public static string AddCom
            (
                string FirstName, string LastName, int Gender, 
                string Address, int City, string Phone, 
                string Email, int Status, string CreatedDate
            )
        {
            return "INSERT INTO `clients`(`FirstName`, `LastName`, `Gender`, `Address`, `City`, `Phone`, `Email`, `Status`, `CreatedDate`)" +
                $" VALUES ('{FirstName}','{LastName}', {Gender}, '{Address}', {City}, '{Phone}','{Email}', {Status}, '{CreatedDate}')";
        }
        public static string UptCom
            (
                int ID,
                string FirstName, string LastName, int Gender,
                string Address, int City, string Phone,
                string Email, int Status, string CreatedDate
            )
        {
            return $"UPDATE `clients` SET `FirstName`='{FirstName}',`LastName`='{LastName}',`Gender`={Gender},`Address`='{Address}',`City`={City},`Phone`='{Phone}',`Email`='{Email}',`Status`={Status},`CreatedDate`='{CreatedDate}' WHERE `ID`={ID}";
        }
        public static string ReadCom_City()
        {
            return $"SELECT * FROM `cities`";
        }
        public static string ReadCom_City(int ID)
        {
            return $"SELECT * FROM `cities` WHERE `ID` = {ID}";
        }
        public static string ReadCom_Status(int ID)
        {
            return $"SELECT * FROM `status` WHERE `ID` = {ID}";
        }

    }
}
