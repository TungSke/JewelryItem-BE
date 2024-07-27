using DAOs.Request;
using DAOs.Response;

namespace Repository;

public interface IOrderRepo
{
    Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
    Task<OrderResponse?> GetOrderByIdAsync(int id);

    Task<List<OrderResponse>> GetAllOrdersAsync();
}