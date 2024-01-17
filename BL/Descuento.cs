using Microsoft.EntityFrameworkCore;
using ML;

namespace BL;

public class Descuento
{
    public static Result GetAll()
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Descuentos.FromSqlRaw("DescuentoGetAll").ToList();

            if (query != null)
            {
                result.Objects = new();
                foreach (var obj in query)
                {
                    ML.Descuento descuento = new();

                    descuento.IdDescuento = obj.Iddescuento;
                    descuento.Valor = obj.Valor;
                    result.Objects.Add(descuento);
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

    public static Result GetById(int IdDescuento)
    {
        Result result = new();
        try
        {
            using DL.JsanchezEcommersContext context = new();

            var query = context.Descuentos.FromSqlRaw($"DescuentoGetById '{IdDescuento}'").AsEnumerable().FirstOrDefault();

            if (query != null)
            {
                result.Object = new();
                var obj = query;
                ML.Descuento descuento = new()
                {
                    IdDescuento = obj.Iddescuento,
                    Valor = obj.Valor
                };
                result.Object = descuento;
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
