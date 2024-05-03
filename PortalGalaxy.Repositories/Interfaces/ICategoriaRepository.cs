using PortalGalaxy.Entities;

namespace PortalGalaxy.Repositories.Interfaces;

public interface ICategoriaRepository : IRepositoryBase<Categoria>
{
    Task<ICollection<Categoria>> ListarEliminados();
}