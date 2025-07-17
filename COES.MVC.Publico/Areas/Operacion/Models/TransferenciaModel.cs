using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Publico.Areas.Operacion.Models
{
    public class TransferenciaModel
    {
        public List<int> ListaMeses { get; set; }
        public List<int> ListaAnios { get; set; }
        public List<BarraDTO> ListaBarras { get; set; }
        public List<BarraDTO> ListaBarrasDTR { get; set; }
        public int Anio { get; set; }
        public int Mes { get; set; }
    }
}