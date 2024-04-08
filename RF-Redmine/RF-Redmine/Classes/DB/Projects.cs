using RF_Redmine.Classes.Interfaces;
using System.Data.SQLite;

namespace RF_Redmine.Classes.Db_Classes
{
    public class Projects : Redmine_DB_Parent,IJsonable
    {
        int _id;
        string _name;
        int _type_id;
        string _description;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int Type_id
        {
            get { return _type_id; }
            set { _type_id = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Type_Name
        {
            get
            {
                return Statics.database_values.Where(a => a is Project_types)
                    .Cast<Project_types>().
                    ToList().
                    Where(a => a.Id == _type_id).
                    ToArray()[0].Name;
            }
        }


        public Projects(SQLiteDataReader data)
        {
            this._id = Convert.ToInt32(data[0]);
            this._name = data[1].ToString()+"";
            this._type_id = Convert.ToInt32(data[2]);
            this._description = data[3].ToString() + "";
        }

        public Projects() { }
        public JsonContent ToJson => JsonContent.Create(new { id = this._id, name = this._name, type_id = this._type_id, desc = this._description, type_name = this.Type_Name });
    }
}
