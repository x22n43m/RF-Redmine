namespace RF_Redmine.Classes.Db_Classes
{
    public class Projects
    {
        /*`id` integer PRIMARY KEY,
  `name` varchar(255),
  `type_id` integer,
  `description` varchar(255)*/
        int id { get; set; }
        string name { get; set; }
        int type_id { get; set; }
        string description { get; set; }
    }
}
