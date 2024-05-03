using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class AlumnoRepository(PortalGalaxyDbContext context) : RepositoryBase<Alumno>(context), IAlumnoRepository
{
    
}