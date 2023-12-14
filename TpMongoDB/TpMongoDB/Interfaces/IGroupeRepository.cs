using TpMongoDB.Models;

namespace TpMongoDB.Interfaces
{
    public interface IGroupeRepository
    {
        List<Groupe> GetGroupeCollection();
    }
}
