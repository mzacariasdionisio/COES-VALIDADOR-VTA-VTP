using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using System.Collections;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models
{
    public class SrmComentarioModel
    {
        public SrmComentarioDTO SrmComentario { get; set; }
        public List<SrmRecomendacionDTO> ListaSrmRecomendacion { get; set; }
        public List<fwUserDTO> ListaFwUser { get; set; }
        public List<SiEmpresaDTO> ListaSiEmpresa { get; set; }
        public int SrmcomCodi { get; set; }
        public int Srmreccodi { get; set; }
        public int Usercode { get; set; }
        public int EmprCodi { get; set; }
        public string SrmcomFechacoment { get; set; }
        public string SrmcomGruporespons { get; set; }
        public string SrmcomComentario { get; set; }
        public string SrmcomActivo { get; set; }
        public string SrmcomUsucreacion { get; set; }
        public string SrmcomFeccreacion { get; set; }
        public string SrmcomUsumodificacion { get; set; }
        public string SrmcomFecmodificacion { get; set; }
        public string Srmrecfecharecomend { get; set; }
        public string Username { get; set; }
        public string EmprNomb { get; set; }
        public int Accion { get; set; }
        public bool IndicadorGrabar { get; set; }
    }

    public class BusquedaSrmComentarioModel
    {
        public List<SrmComentarioDTO> ListaSrmComentario { get; set; }
        public List<SrmRecomendacionDTO> ListaSrmRecomendacion { get; set; }
        public List<fwUserDTO> ListaFwUser { get; set; }
        public List<SiEmpresaDTO> ListaSiEmpresa { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }
        public Hashtable htFiles { get; set; }
    }

}
