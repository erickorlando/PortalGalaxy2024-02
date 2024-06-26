﻿using PortalGalaxy.Shared.Request;
using PortalGalaxy.Shared.Response;

namespace PortalGalaxy.Services.Interfaces;

public interface ITallerService
{
    Task<PaginationResponse<TallerDtoResponse>> ListAsync(BusquedaTallerRequest request);
    Task<PaginationResponse<InscritosPorTallerDtoResponse>> ListAsync(BusquedaInscritosPorTallerRequest request);
    Task<BaseResponseGeneric<ICollection<TallerSimpleDtoResponse>>> ListSimpleAsync();
    Task<PaginationResponse<TallerHomeDtoResponse>> ListarTalleresHomeAsync(BusquedaTallerHomeRequest request);
    Task<BaseResponse> AddAsync(TallerDtoRequest request);
    Task<BaseResponseGeneric<TallerDtoRequest>> FindByIdAsync(int id);

    Task<BaseResponseGeneric<TallerHomeDtoResponse>> GetTallerHomeAsync(int id);

    Task<BaseResponseGeneric<ICollection<TalleresPorMesDto>>> ReporteTalleresPorMes(int anio);

    Task<BaseResponseGeneric<ICollection<TalleresPorInstructorDto>>> ReporteTalleresPorInstructor(int anio);

    Task<BaseResponse> UpdateAsync(int id, TallerDtoRequest request);
    Task<BaseResponse> DeleteAsync(int id);
}