using System.Data.SQLite;
using System.Text.Json.Serialization;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Developers : Redmine_DB_Parent
    {
        int id;
        string name;
        string email;

        public Developers(SQLiteDataReader data)
        {
            this.id = Convert.ToInt32(data[0]);
            this.name = data[1].ToString()+"";
            this.email = data[2].ToString()+"";
        }
    }
}
