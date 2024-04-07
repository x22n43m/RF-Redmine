using RF_Redmine.Classes.Db_Classes;
using System.Data.Entity;

namespace RF_Redmine.Classes
{
    public class Statics
    {
        public static List<Redmine_DB_Parent> database_values = new List<Redmine_DB_Parent>();
        public static string abeleres = "Data Source = ";
        public static string ns = "RF_Redmine.Classes.Db_Classes";
        /*public static Dictionary<string, string[]> FieldNames = new Dictionary<string, string[]> 
        {
            { "developers",new string[] { "id","name","email" } },
            { "managers",new string[] { "name","email","password" } },
            { "projects",new string[] { "name","email","password" } },
            { "project_developers",new string[] { "developer_id","project_id" } },
            { "project_types",new string[] { "id","name" } },
        };*/
    }
}
