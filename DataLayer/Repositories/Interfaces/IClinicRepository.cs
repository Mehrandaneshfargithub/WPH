using DataLayer.EntityModels;

namespace DataLayer.Repositories.Interfaces
{
    public interface IClinicRepository : IRepository<Clinic>
    {
        Clinic GetFirst();
    }
}
