using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Admin.Models
{
    public class SiParametroModel
    {
        public SiParametroDTO SiParametro { get; set; }
        public int SiparCodi { get; set; }
        public string SiparAbrev { get; set; }
        public string SiparDescripcion { get; set; }
        public string SiparUsuCreacion { get; set; }
        public string SiparFecCreacion { get; set; }
        public string SiparUsuModificacion { get; set; }
        public string SiparFecModificacion { get; set; }
        public int Accion { get; set; }
    }

    public class BusquedaSiParametroModel
    {
        public List<SiParametroDTO> ListaSiParametro { get; set; }
        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
    }
}
