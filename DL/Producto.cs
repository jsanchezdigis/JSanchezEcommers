using System;
using System.Collections.Generic;

namespace DL;

public partial class Producto
{
    public int Idproducto { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal Precio { get; set; }

    public decimal Precioventa { get; set; }

    public string Codigobarra { get; set; } = null!;

    public int Stock { get; set; }

    public string Imagen { get; set; } = null!;

    public int Idcategoria { get; set; }

    public int Iddescuento { get; set; }

    public string NombreCategoria { get; set; }

    public decimal Valor { get; set; }

    public virtual Categorium IdcategoriaNavigation { get; set; } = null!;

    public virtual Descuento IddescuentoNavigation { get; set; } = null!;
}
