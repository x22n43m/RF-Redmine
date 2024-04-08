using RF_Redmine.Classes.Db_Classes;
using System.Data.Entity;

namespace RF_Redmine.Classes
{
    public class Statics
    {
        public static List<Redmine_DB_Parent> database_values = new List<Redmine_DB_Parent>();
        public static string abeleres = "Data Source = ";
    }

    public class Statics<T>
    {
        public static List<T> ListTypeGetter()
        {
            List<T> re = new List<T>();
            foreach (Redmine_DB_Parent data in Statics.database_values)
            {
                if (data is T)
                {
                    re.Add((T)(object)data);
                }
            }
            return re;
        }
    }
}
