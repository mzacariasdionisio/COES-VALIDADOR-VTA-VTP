using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.IntercambioOsinergmin.Models.Maestros
{
    public class MaestrosModel 
    {
        public string CodigoEmpresa { get; set; }
        public string NombreEmpresa { get; set; }
        public DateTime RcimFecHorImportacion { get; set; }
        public string RcimEstadoImportacion { get; set; }

    }
}