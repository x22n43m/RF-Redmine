namespace RF_Redmine.Classes.Interfaces
{
    public interface IJsonable
    {
        public abstract JsonContent ToJson { get; }
    }
}
