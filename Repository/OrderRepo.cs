using DAOs;
using DAOs.Request;
using DAOs.Response;

namespace Repository;

public class OrderRepo : IOrderRepo 
{
    private readonly OrderDAO _orderDAO;

    public OrderRepo(OrderDAO orderDAO)
    {
        _orderDAO = orderDAO;
    }
    public Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest) => _orderDAO.CreateOrderAsync(orderRequest);

    public Task<OrderResponse?> GetOrderByIdAsync(int id) => _orderDAO.GetOrderByIdAsync(id);

    public Task<List<OrderResponse>> GetAllOrdersAsync() => _orderDAO.GetAllOrdersAsync();
}