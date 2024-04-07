using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Project_developers : Redmine_DB_Parent
    {
        int project_id { get; set; }
        int developer_id { get; set; }

        public Project_developers(SQLiteDataReader data)
        {
            project_id = Convert.ToInt32(data[0]);
            developer_id = Convert.ToInt32(data[1]);
        }

    }
}
