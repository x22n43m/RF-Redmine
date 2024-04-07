using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Projects : Redmine_DB_Parent
    {
        int id;
        string name;
        int type_id;
        string description;

        public Projects(SQLiteDataReader data)
        {
            this.id = Convert.ToInt32(data[0]);
            this.name = data[1].ToString()+"";
            this.type_id = Convert.ToInt32(data[2]);
            this.description = data[3].ToString() + "";
        }
    }
}
