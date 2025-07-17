using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Models
{
    public class ParValorDescripcion<T>
    {
        public T Valor { get; set; }
        public string Descripcion { get; set; }
    }
}