using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Project_types : Redmine_DB_Parent
    {
        int _id;
        string _name;

        public int Id
        {
            get { return _id; }
        }

        public string Name
        {
            get { return _name; }
        }

        public Project_types(SQLiteDataReader data)
        {
            this._id = Convert.ToInt32(data[0]);
            this._name = data[1].ToString()+"";
        }
    }
}
