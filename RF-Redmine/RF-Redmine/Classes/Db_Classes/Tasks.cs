namespace RF_Redmine.Classes.Db_Classes
{
    public class Tasks
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
        DateTime deadline { get; set; }
    }
}
