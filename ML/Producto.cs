namespace ML;

public class Producto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public decimal PrecioVenta { get; set; }

    public string? CodigoBarra { get; set; }

    public int Stock { get; set; }

    public string? Imagen { get; set; }

    public int Cantidad { get; set; }

    public decimal SubTotal { get; set; }

    public Categoria Categoria { get; set; }

    public Descuento Descuento { get; set; }

    public List<object> Productos { get; set; }
}
