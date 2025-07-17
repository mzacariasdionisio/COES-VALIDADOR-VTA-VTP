using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.InformeEjecutivoMen.Models
{
    public class BarrasModel
    {
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<TrnBarraAreaDTO> ListadoBarras { get; set; }

        public List<BarraDTO> ListaBarraTransferencia { get; set; }

        public TrnBarraAreaDTO Barra { get; set; }
        public List<IeeBarrazonaDTO> ListaAgrupacion { get; set; }

        public List<IeeBarrazonaDTO> ListaBarra { get; set; }
        public string NombreAgrupacion { get; set; }
        public int Zona { get; set; }
    }
}