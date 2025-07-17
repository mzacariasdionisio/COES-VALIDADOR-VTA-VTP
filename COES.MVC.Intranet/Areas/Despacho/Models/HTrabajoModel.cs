using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.Despacho.Models
{
    public class HTrabajoModel
    {
        public string Fecha { get; set; }

        public List<HtCentralCfgDTO> ListaConfiguracionRER { get; set; }
        public HtCentralCfgDTO Entidad { get; set; }
        public List<EqEquipoDTO> ListaCentrales { get; set; }
        //public List<MePtomedicionDTO> ListaPto { get; set; 
        public List<HtCentralCfgdetDTO> ListaConfiguracionPto { get; set; }
        public List<HtCentralCfgdetDTO> ListaConfiguracionCanal { get; set; }
        public List<HtCentralCfgdetDTO> ListaConfiguracion { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermisoAdmin { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public ItemElemento Elemento { get; set; }
    }

    public class ItemElemento
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Factor { get; set; }
        public int Tipo { get; set; }
    }

}