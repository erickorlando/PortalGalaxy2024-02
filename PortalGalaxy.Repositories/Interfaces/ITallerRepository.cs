using PortalGalaxy.Entities;

namespace PortalGalaxy.Repositories.Interfaces;

public interface ITallerRepository : IRepositoryBase<Taller>
{
    Task<ICollection<Taller>> ListarAsync();
}