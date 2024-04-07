using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Tasks : Redmine_DB_Parent
    {
        /*  `id` integer PRIMARY KEY,
  `name` varchar(255),
  `description` varchar(255),
  `project_id` integer,
  `user_id` integer,
  `deadline` datetime*/
        int id { get; set; }
        string name { get; set; }
        string description { get; set; }
        int project_id { get; set; }
        int user_id { get; set; }
        string deadline { get; set; }

        public Tasks(SQLiteDataReader data)
        {
            this.id = Convert.ToInt32(data[0]);
            this.name = data[1].ToString() + "";
            this.description = data[2].ToString() + "";
            this.project_id = Convert.ToInt32(data[3]);
            this.user_id = Convert.ToInt32(data[4]);
            this.deadline = data[5].ToString() + "";
        }
    }
}
