using Microsoft.EntityFrameworkCore;
using ML;

namespace BL;

public class Categoria
{
    public static Result Add(ML.Categoria categoria)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Database.ExecuteSqlRaw($"CategoriaAdd '{categoria.Nombre}'");

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

    public static Result Update(ML.Categoria categoria)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Database.ExecuteSqlRaw($"CategoriaUpdate '{categoria.IdCategoria}','{categoria.Nombre}'");

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

    public static Result Delete(int IdCategoria)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Database.ExecuteSqlRaw($"CategoriaDelete '{IdCategoria}'");

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

    public static Result GetAll()
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Categoria.FromSqlRaw("CategoriaGetAll").ToList();

            if (query != null)
            {
                result.Objects = new();
                foreach (var obj in query)
                {
                    ML.Categoria categoria = new();

                    categoria.IdCategoria = obj.Idcategoria;
                    categoria.Nombre = obj.Nombre;
                    result.Objects.Add(categoria);
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

    public static Result GetById(int IdCategoria)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Categoria.FromSqlRaw($"CategoriaGetById '{IdCategoria}'").AsEnumerable().FirstOrDefault();

            if (query != null)
            {
                result.Object = new();
                var obj = query;
                ML.Categoria categoria = new()
                {
                    IdCategoria = obj.Idcategoria,
                    Nombre = obj.Nombre
                };
                result.Object = categoria;
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
}
