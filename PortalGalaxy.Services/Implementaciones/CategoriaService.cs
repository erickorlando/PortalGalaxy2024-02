using AutoMapper;
using Microsoft.Extensions.Logging;
using PortalGalaxy.Entities;
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

    public async Task<BaseResponseGeneric<CategoriaDtoRequest>> FindByIdAsync(int id)
    {
        var response = new BaseResponseGeneric<CategoriaDtoRequest>();
        try
        {
            var categoria = await _repository.FindByIdAsync(id);
            if (categoria == null)
            {
                response.ErrorMessage = "Categoria no encontrada";
                return response;
            }

            response.Data = _mapper.Map<CategoriaDtoRequest>(categoria);
            response.Success = true;

        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al buscar la categoria por ID";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> AddAsync(CategoriaDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            await _repository.AddAsync(_mapper.Map<Categoria>(request));
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al agregar la categoria";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, CategoriaDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            var categoria = await _repository.FindByIdAsync(id);
            if (categoria == null)
            {
                response.ErrorMessage = "Categoria no encontrada";
                return response;
            }

            _mapper.Map(request, categoria);

            await _repository.UpdateAsync();

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al actualizar la categoria";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            await _repository.DeleteAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al eliminar la categoria";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }
}