using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Project_types : Redmine_DB_Parent
    {
        int id { get; set; }
        string name { get; set; }

        public Project_types(SQLiteDataReader data)
        {
            this.id = Convert.ToInt32(data[0]);
            this.name = data[1].ToString()+"";
        }
    }
}
