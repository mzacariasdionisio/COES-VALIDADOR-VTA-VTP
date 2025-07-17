using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.FormatoMedicion;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.IND.Models
{
    public class CumplimientoDiarioModel
    {
        public object Mensaje { get; set; }
        public string TipoMensaje { get; set; }
        public List<IndPeriodoDTO> ListaPeriodo { get; set; }
        public List<SiEmpresaDTO> ListaEmpresa { get; set; }
        public List<IndCrdSugadDTO> ListaCumplimiento { get; set; }
        public object CrdEstado { get; set; }
    }
}