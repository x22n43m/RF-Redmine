namespace RF_Redmine.Classes.Db_Classes
{
    public class Managers
    {
        /*`id` integer PRIMARY KEY,
  `name` varchar(255),
  `email` varchar(255),
  `password` varchar(255)*/
        int id { get;set; }
        string name { get;set; }
        string email { get;set; }
        string password { get;set; }
    }
}
