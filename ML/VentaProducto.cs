namespace ML;

public class VentaProducto
{
    public int IdVentaProducto { get; set; }
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public Producto Producto { get; set; }
    public Venta Venta { get; set; }
    public List<object> VentaProductos { get; set; }
}
