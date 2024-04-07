using RF_Redmine.Classes.Db_Classes;
using RF_Redmine.Enums;
using RF_Redmine.Classes;
using System;
using System.ComponentModel.DataAnnotations;

namespace RF_Redmine.Models
{
    public class Login
    {
        [Required]
        string username { get; set; }
        [Required]
        string password { get; set; }

        public ELoginState LoginState;

        public override string ToString()
        {
            return username + " - " + password;
        }

        public Login(IFormCollection credentials)
        {
            username = credentials["username"].ToString();
            password = credentials["password"].ToString();
            LoginState = checkValidity();
        }

        ELoginState checkValidity()
        {
            //List<Managers> managerek = Statics.database_values.Where(a => a is Managers).ToList();
            List<Managers> managerek = new List<Managers>();
            foreach (Redmine_DB_Parent a in Statics.database_values)
            {
                if (a is Managers) managerek.Add((Managers)a);
            }
            Managers m;
            try
            {
                m = managerek.Where(a => a.GetName == username).ToArray()[0];
            }
            catch (Exception e)
            {
                return ELoginState.Non_Existent_Credentials;
            }
            if (m.GetPassword == password)
            {
                return ELoginState.Valid_Credentials;
            }
            return ELoginState.Invalid_Credentials;
        }
        
    }
}
