using PortalGalaxy.DataAccess;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;

namespace PortalGalaxy.Repositories.Implementaciones;

public class InstructorRepository(PortalGalaxyDbContext context) : 
    RepositoryBase<Instructor>(context), IInstructorRepository
{
    
}