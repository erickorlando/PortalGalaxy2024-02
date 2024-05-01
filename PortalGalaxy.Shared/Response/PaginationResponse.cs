namespace PortalGalaxy.Shared.Response;

public class PaginationResponse<T> : BaseResponse
{
    public ICollection<T>? Data { get; set; }
    public int TotalPages { get; set; }
}