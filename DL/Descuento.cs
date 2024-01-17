using System;
using System.Collections.Generic;

namespace DL;

public partial class Descuento
{
    public int Iddescuento { get; set; }

    public decimal Valor { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
