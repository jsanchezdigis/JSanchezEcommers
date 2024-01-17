using Aspose.BarCode.Generation;
using Microsoft.EntityFrameworkCore;
using ML;

namespace BL;

public class Producto
{
    public static Result Add(ML.Producto producto)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            Result resultDescuento = Descuento.GetById(producto.Descuento.IdDescuento);
            var descuento = (ML.Descuento)resultDescuento.Object;

            producto.PrecioVenta = CalcularDescuento(descuento.Valor, producto.Precio);

            producto.CodigoBarra = GenerarCodigoBarrasAspose(producto.Nombre);

            var query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}','{producto.Descripcion}','{producto.Precio}','{producto.PrecioVenta}','{producto.CodigoBarra}','{producto.Stock}','{producto.Imagen}','{producto.Categoria.IdCategoria}','{producto.Descuento.IdDescuento}'");

            if (query >= 1)
            {
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se inserto el registro";
            }

        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.Ex = ex;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public static Result Update(ML.Producto producto)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            Result resultDescuento = Descuento.GetById(producto.Descuento.IdDescuento);
            var descuento = (ML.Descuento)resultDescuento.Object;

            producto.PrecioVenta = CalcularDescuento(descuento.Valor, producto.Precio);

            producto.CodigoBarra = GenerarCodigoBarrasAspose(producto.Nombre);

            var query = context.Database.ExecuteSqlRaw($"ProductoUpdate '{producto.IdProducto}', '{producto.Nombre}','{producto.Descripcion}','{producto.Precio}','{producto.PrecioVenta}','{producto.CodigoBarra}','{producto.Stock}','{producto.Imagen}','{producto.Categoria.IdCategoria}','{producto.Descuento.IdDescuento}'");

            if (query >= 1)
            {
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se actualizo el registro";
            }

        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.Ex = ex;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public static Result Delete(int IdProducto)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Database.ExecuteSqlRaw($"ProductoDelete '{IdProducto}'");

            if (query >= 1)
            {
                result.Correct = true;
            }
            else
            {
                result.Correct = false;
                result.ErrorMessage = "No se inactivo el registro";
            }

        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.Ex = ex;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public static Result GetAll(ML.Producto producto)
    {
        Result result = new();
        producto.Categoria ??= new ML.Categoria();

        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}','{producto.Categoria.IdCategoria}'").ToList();

            if (query != null)
            {
                result.Objects = new();
                foreach (var obj in query)
                {
                    producto = new();

                    producto.IdProducto = obj.Idproducto;
                    producto.Nombre = obj.Nombre;
                    producto.Descripcion = obj.Descripcion;
                    producto.Precio = obj.Precio;
                    producto.PrecioVenta = obj.Precioventa;
                    producto.CodigoBarra = obj.Codigobarra;
                    producto.Stock = obj.Stock;
                    producto.Imagen = obj.Imagen;
                    producto.Categoria = new ML.Categoria();
                    producto.Categoria.IdCategoria = obj.Idcategoria;
                    producto.Categoria.Nombre = obj.NombreCategoria;
                    producto.Descuento = new ML.Descuento();
                    producto.Descuento.IdDescuento = obj.Iddescuento;
                    producto.Descuento.Valor = obj.Valor;

                    result.Objects.Add(producto);
                }
            }
            result.Correct = true;
        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.Ex = ex;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    public static Result GetById(int IdProducto)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Productos.FromSqlRaw($"ProductoGetById '{IdProducto}'").AsEnumerable().FirstOrDefault();

            if (query != null)
            {
                result.Object = new List<object>();
                var obj = query;
                ML.Producto producto = new();

                producto.IdProducto = obj.Idproducto;
                producto.Nombre = obj.Nombre;
                producto.Descripcion = obj.Descripcion;
                producto.Precio = obj.Precio;
                producto.PrecioVenta = obj.Precioventa;
                producto.CodigoBarra = obj.Codigobarra;
                producto.Stock = obj.Stock;
                producto.Imagen = obj.Imagen;
                producto.Categoria = new ML.Categoria();
                producto.Categoria.IdCategoria = obj.Idcategoria;
                producto.Categoria.Nombre = obj.NombreCategoria;
                producto.Descuento = new ML.Descuento();
                producto.Descuento.IdDescuento = obj.Iddescuento;
                producto.Descuento.Valor = obj.Valor;

                result.Object = producto;
            }
            result.Correct = true;
        }
        catch (Exception ex)
        {
            result.Correct = false;
            result.Ex = ex;
            result.ErrorMessage = ex.Message;
        }
        return result;
    }

    private static string GenerarCodigoBarrasAspose(string contenido)
    {
        using MemoryStream stream = new MemoryStream();
        BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.Code128, contenido);
        generator.Save(stream, BarCodeImageFormat.Png);
        byte[] imageBytes = stream.ToArray();
        return Convert.ToBase64String(imageBytes);
    }

    public static decimal CalcularDescuento(decimal porcentajeDescuento, decimal cantidad)
    {
        if (porcentajeDescuento < 0 || porcentajeDescuento > 1)
        {
            throw new ArgumentException("El porcentaje de descuento debe estar entre 0 y 1.");
        }

        if (cantidad < 0)
        {
            throw new ArgumentException("La cantidad no puede ser negativa.");
        }

        decimal descuento = cantidad * porcentajeDescuento;
        decimal precioConDescuento = cantidad - descuento;

        return precioConDescuento;
    }

}
