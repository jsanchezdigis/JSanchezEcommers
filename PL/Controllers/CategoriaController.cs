using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CategoriaController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            ML.Categoria categoria = new();
            ML.Result result = BL.Categoria.GetAll();
            if (result.Correct)
            {
                categoria.Categorias = result.Objects;
                return View(categoria);
            }
            else
            {
                return View(categoria);
            }
        }

        [HttpGet]
        public IActionResult Form(int? IdCategoria)
        {
            ML.Categoria categoria = new();
            if (IdCategoria == null)
            {
                return View(categoria);
            }
            else
            {
                ML.Result result = BL.Categoria.GetById(IdCategoria.Value);

                if (result.Correct)
                {
                    categoria = (ML.Categoria)result.Object;
                    return View(categoria);
                }
                else
                {
                    ViewBag.Message = "Ocurrio un error al consultar la informacion";
                    return View("Modal");
                }
            }
        }

        [HttpPost]
        public IActionResult Form(ML.Categoria categoria)
        {
            if (categoria.IdCategoria == 0)
            {
                ML.Result result = BL.Categoria.Add(categoria);

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
                ML.Result result = BL.Categoria.Update(categoria);
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
        public IActionResult Delete(ML.Categoria categoria)
        {
            ML.Result result = BL.Categoria.Delete(categoria.IdCategoria);

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
    }
}
