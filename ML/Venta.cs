namespace ML;

public class Venta
{
    public int IdVenta { get; set; }
    public decimal Total { get; set; }
    public string Fecha { get; set; }
    public MetodoPago MetodoPago { get; set; }
}
