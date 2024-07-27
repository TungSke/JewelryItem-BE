using DAOs.Request;
using DAOs.Response;

namespace Repository;

public interface IOrderRepo
{
    OrderResponse createOrder(OrderRequest orderRequest);

    OrderResponse getOrderById(int id);

    List<OrderResponse> getAllOrders();
}