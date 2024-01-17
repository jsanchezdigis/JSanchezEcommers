using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers;

public class ProductoController : Controller
{
    [HttpGet]
    public IActionResult GetAll()
    {
        ML.Producto producto = new();
        producto.Categoria = new ML.Categoria();
        ML.Result result = BL.Producto.GetAll(producto);
        ML.Result resultCategoria = BL.Categoria.GetAll();
        if (result.Correct)
        {
            producto.Productos = result.Objects;
            producto.Categoria.Categorias = resultCategoria.Objects;
            return View(producto);
        }
        else
        {
            return View(producto);
        }
    }

    [HttpPost]
    public ActionResult GetAll(ML.Producto producto)
    {
        ML.Result result = BL.Producto.GetAll(producto);
        ML.Result resultCategoria = BL.Categoria.GetAll();
        producto.Categoria = new ML.Categoria();
        producto.Descuento = new ML.Descuento();

        if (result.Correct && resultCategoria.Correct)
        {
            producto.Productos = result.Objects;
            producto.Categoria.Categorias = resultCategoria.Objects;
            return View(producto);
        }
        else
        {
            return View(producto);
        }
    }

    [HttpGet]
    public IActionResult Form(int? IdProducto)
    {
        ML.Producto producto = new();
        producto.Categoria = new ML.Categoria();
        producto.Descuento = new ML.Descuento();
        ML.Result resultCategoria = BL.Categoria.GetAll();
        ML.Result resultDescuento = BL.Descuento.GetAll();
        if (IdProducto == null)
        {
            producto.Categoria.Categorias = resultCategoria.Objects;
            producto.Descuento.Descuentos = resultDescuento.Objects;
            return View(producto);
        }
        else
        {
            ML.Result result = BL.Producto.GetById(IdProducto.Value);

            if (result.Correct)
            {
                producto = (ML.Producto)result.Object;
                producto.Categoria.Categorias = resultCategoria.Objects;
                producto.Descuento.Descuentos = resultDescuento.Objects;
                return View(producto);
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al consultar la informacion";
                return View("Modal");
            }
        }
    }

    [HttpPost]
    public IActionResult Form(ML.Producto producto)
    {
        IFormFile file = Request.Form.Files["fuImage"];
        if (file != null)
        {
            byte[] imagen = ConvertToBytes(file);
            producto.Imagen = Convert.ToBase64String(imagen);
        }

        if (producto.IdProducto == 0)
        {
            ML.Result result = BL.Producto.Add(producto);

            if (result.Correct)
            {
                ViewBag.Message = "Se completo el registro satisfactoriamente";
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al insertar el registro";
            }
            return View("Modal");
        }
        else
        {
            ML.Result result = BL.Producto.Update(producto);
            if (result.Correct)
            {
                ViewBag.Message = "Se actualizo el registro satisfactoriamente";
            }
            else
            {
                ViewBag.Message = "Ocurrio un error al actualizar el registro";
            }
            return View("Modal");
        }
    }

    [HttpGet]
    public IActionResult Delete(int? IdProducto)
    {
        ML.Result result = BL.Producto.Delete(IdProducto.Value);

        if (result.Correct)
        {
            ViewBag.Message = "Se inactivo el registro satisfactoriamente";
        }
        else
        {
            ViewBag.Message = "Ocurrio un error al consultar la informacion";
        }
        return View("Modal");
    }

    public static byte[] ConvertToBytes(IFormFile imagen)
    {
        using var fileStream = imagen.OpenReadStream();
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        return bytes;
    }
}
