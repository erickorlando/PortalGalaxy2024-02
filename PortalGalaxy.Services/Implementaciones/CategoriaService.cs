using AutoMapper;
using Microsoft.Extensions.Logging;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Shared.Request;
using PortalGalaxy.Shared.Response;

namespace PortalGalaxy.Services.Implementaciones;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repository;
    private readonly ILogger<CategoriaService> _logger;
    private readonly IMapper _mapper;

    public CategoriaService(ICategoriaRepository repository, ILogger<CategoriaService> logger, IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BaseResponseGeneric<ICollection<CategoriaDtoResponse>>> ListAsync()
    {
        var response = new BaseResponseGeneric<ICollection<CategoriaDtoResponse>>();
        try
        {
            var collection = await _repository.ListAsync();

            response.Data = _mapper.Map<ICollection<CategoriaDtoResponse>>(collection);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al listar las categorias";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public Task<BaseResponseGeneric<CategoriaDtoRequest>> FindByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> AddAsync(CategoriaDtoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> UpdateAsync(int id, CategoriaDtoRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}