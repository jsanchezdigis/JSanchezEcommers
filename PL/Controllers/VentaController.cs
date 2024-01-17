using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL.Controllers;

public class VentaController : Controller
{
    public ActionResult ProductoGetAll()
    {
        ML.Result resultCat = BL.Categoria.GetAll();

        ML.Producto producto = new ML.Producto();
        producto.Categoria = new ML.Categoria();

        ML.Result result = BL.Producto.GetAll(producto);


        if (resultCat.Correct && result.Correct)
        {
            producto.Productos = result.Objects;
            producto.Categoria.Categorias = resultCat.Objects;
        }

        return View(producto);

    }

    [HttpPost]
    public ActionResult ProductoGetAll(ML.Producto producto)
    {
        IFormFile file = Request.Form.Files["ImgProducto"];

        if (file != null)
        {
            byte[] imagen = ConvertToBytes(file);

            producto.Imagen = Convert.ToBase64String(imagen);
        }


        ML.Result resultCat = BL.Categoria.GetAll();

        ML.Result result = BL.Producto.GetAll(producto);


        if (resultCat.Correct && result.Correct)
        {
            producto.Productos = result.Objects;
            producto.Categoria.Categorias = resultCat.Objects;
        }

        return View(producto);
    }

    //CARRITO
    public ActionResult Cart()
    {
        return View();
    }

    [HttpGet]
    public ActionResult CartPost(int IdProducto)
    {
        Result result = BL.Producto.GetById(IdProducto);
        ML.Producto producto = (ML.Producto)result.Object;

        bool existe = false;
        ML.VentaProducto ventaProducto = new()
        {
            VentaProductos = []
        };

        if (HttpContext.Session.GetString("Producto") == null)
        {
            producto.Cantidad = producto.Cantidad = 1;
            producto.SubTotal = producto.PrecioVenta * producto.Cantidad;
            ventaProducto.VentaProductos.Add(producto);
            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
            var session = HttpContext.Session.GetString("Producto");
        }
        else
        {
            GetCarrito(ventaProducto);
            foreach (ML.Producto venta in ventaProducto.VentaProductos.ToList())
            {
                if (producto.IdProducto == venta.IdProducto)
                {
                    venta.Cantidad++;
                    venta.SubTotal = venta.PrecioVenta * venta.Cantidad;
                    existe = true;
                }
                else
                {
                    existe = false;
                }

                if (existe == true)
                {
                    break;
                }
            }
            if (existe == false)
            {
                producto.Cantidad = producto.Cantidad = 1;
                producto.SubTotal = producto.Cantidad * producto.PrecioVenta;
                ventaProducto.VentaProductos.Add(producto);
            }
            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
        }
        if (HttpContext.Session.GetString("Producto") != null)
        {
            ViewBag.Message = "Se ha agregado el producto a tu carrito!";
            return PartialView("Modal");
        }
        else
        {
            ViewBag.Message = "No se pudo agregar el producto a tu carrito!";
            return PartialView("Modal");
        }

    }
    [HttpGet]
    public ActionResult ResumenCompra(VentaProducto ventaProducto)
    {
        if (HttpContext.Session.GetString("Producto") == null)
        {
            return View();
        }
        else
        {
            ventaProducto.VentaProductos = [];
            GetCarrito(ventaProducto);
        }

        return View(ventaProducto);
    }

    [HttpGet]
    public ActionResult Add(int IdProducto)
    {
        Result result = BL.Producto.GetById(IdProducto);
        ML.Producto producto = (ML.Producto)result.Object;

        bool existe = false;
        ML.VentaProducto ventaProducto = new()
        {
            VentaProductos = []
        };

        GetCarrito(ventaProducto);
        foreach (ML.Producto venta in ventaProducto.VentaProductos.ToList())
        {
            if (producto.IdProducto == venta.IdProducto)
            {
                venta.Cantidad++;
                venta.SubTotal = venta.PrecioVenta * venta.Cantidad;
                existe = true;
            }
            else
            {
                existe = false;
            }

            if (existe == true)
            {
                break;
            }
        }
        if (existe == false)
        {
            producto.Cantidad = producto.Cantidad = 1;
            producto.SubTotal = producto.Cantidad * producto.PrecioVenta;
            ventaProducto.VentaProductos.Add(producto);
        }
        HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));

        return RedirectToAction("ResumenCompra", "Venta");
    }

    [HttpGet]
    public ActionResult Delete(int IdProducto)
    {
        ML.VentaProducto ventaProducto = new ML.VentaProducto
        {
            VentaProductos = []
        };

        GetCarrito(ventaProducto);

        var productoExistente = ventaProducto.VentaProductos.FirstOrDefault(p => ((ML.Producto)p).IdProducto == IdProducto);

        if (productoExistente != null)
        {
            ML.Producto producto = (ML.Producto)productoExistente;

            producto.Cantidad--;

            if (producto.Cantidad == 0)
            {
                ventaProducto.VentaProductos.Remove(productoExistente);
            }

            producto.SubTotal = producto.PrecioVenta * producto.Cantidad;

            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
        }

        return RedirectToAction("ResumenCompra", "Venta");
    }


    public VentaProducto GetCarrito(VentaProducto ventaProducto)
    {
        var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

        foreach (var obj in ventaSession)
        {
            ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
            ventaProducto.VentaProductos.Add(objProducto);
            ventaProducto.Total += objProducto.SubTotal;
        }
        return ventaProducto;
    }

    public static byte[] ConvertToBytes(IFormFile imagen)
    {
        using var fileStream = imagen.OpenReadStream();

        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        return bytes;
    }
}
