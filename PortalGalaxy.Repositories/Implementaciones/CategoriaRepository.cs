using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class CategoriaRepository(PortalGalaxyDbContext context) : 
    RepositoryBase<Categoria>(context), ICategoriaRepository
{
    
}