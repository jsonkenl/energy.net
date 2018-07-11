namespace Energy.Core.Interfaces
{
    public interface IAdministratorRepository
    {
        void Add(Administrator newAdministrator);
        Administrator Get();
        void Commit();
    }
}
