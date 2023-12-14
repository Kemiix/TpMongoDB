using TpMongoDB.Models;

namespace TpMongoDB.Interfaces
{
    public interface IEquipeRepository
    {
        List<Equipe> GetAllEquipes();
    }
}
