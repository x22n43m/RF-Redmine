using System.Data.SQLite;

namespace RF_Redmine.Classes
{
    public static class db
    {
        public static SQLiteCommand parancs;
        public static SQLiteConnection kapcsolat;
        public static SQLiteDataReader adat;

        public static void kapcsol(string abeleres)
        {
            kapcsolat = new SQLiteConnection(abeleres);
            kapcsolat.Open();
        }

        public static void bezar()
        {
            kapcsolat.Close();
        }
    }
}
