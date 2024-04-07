using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Managers : Redmine_DB_Parent
    {
        private int id;
        public string name { get; set; }
        private string email;
        private string password;

        public string GetName
        {
            get { return name; }
        }

        public string GetPassword
        {
            get { return password; }
        }

        public Managers(SQLiteDataReader data)
        {
            this.id = Convert.ToInt32(data[0]);
            this.name = data[1].ToString()+ "";
            this.email = data[2].ToString()+"";
            this.password = data[3].ToString()+"";
        }
    }
}
