using Microsoft.AspNetCore.Mvc;
using RF_Redmine.Classes.Db_Classes;
using RF_Redmine.Controllers;
using System.Data.SQLite;
using System.Reflection;

namespace RF_Redmine.Classes
{
    public class db
    {
        public static SQLiteCommand parancs;
        public static SQLiteCommand parancs2;
        public static SQLiteConnection kapcsolat;
        public static SQLiteDataReader eredmeny;
        public static SQLiteDataReader eredmeny2;
        
        public static void kapcsolodik()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database\\redmine.db");
            kapcsolat = new SQLiteConnection(Statics.abeleres + "" + dbPath);
            kapcsolat.Open();
            parancs = kapcsolat.CreateCommand();
            parancs2 = kapcsolat.CreateCommand();
            LoadDataBase();
        }

        public static void LoadDataBase()
        {
            loadTablesNames();
        }

        static void loadTablesNames()
        {
            parancs.CommandText = "select count(*) from sqlite_master where type='table' and name != 'sqlite_sequence'";
            int tableamount = Convert.ToInt32(parancs.ExecuteScalar());
            string[] tablenames = new string[tableamount];
            parancs.CommandText = "select name from sqlite_master where type='table' and name != 'sqlite_sequence'";
            eredmeny = parancs.ExecuteReader();
            int i = 0;
            while (eredmeny.Read())
            {
                tablenames[i] = eredmeny[0] + "";
                i++;
            }
            eredmeny.Close();
            foreach (string tn in tablenames)
            {
                parancs.CommandText = "select * from " + tn;
                eredmeny = parancs.ExecuteReader();
                while (eredmeny.Read())
                {
                    ClassRouter(tn);
                }
                eredmeny.Close();
            }
        }

        static void ClassRouter(string tablename)
        {
            switch (tablename)
            {
                case "developers": Statics.database_values.Add(new Developers(eredmeny)); break;
                case "managers": Statics.database_values.Add(new Managers(eredmeny)); break;
                case "project_developers": Statics.database_values.Add(new Project_developers(eredmeny)); break;
                case "project_types": Statics.database_values.Add(new Project_types(eredmeny)); break;
                case "projects": Statics.database_values.Add(new Projects(eredmeny)); break;
                case "tasks": Statics.database_values.Add(new Tasks(eredmeny)); break;
            }
        }

        public static void kapcsolatBont()
        {
            kapcsolat.Close();
        }
    }
}
