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
    
    public OrderResponse createOrder(OrderRequest orderRequest) => _orderDAO.createOrder(orderRequest);

    public OrderResponse getOrderById(int id) => _orderDAO.getOrderById(id);

    public List<OrderResponse> getAllOrders() => _orderDAO.getAllOrders();
}