using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Framework.Base.Tools;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.SeguimientoRecomendacion.Models
{
    public class SrmRecomendacionModel
    {
        public SrmRecomendacionDTO SrmRecomendacion { get; set; }
        public List<EveEventoDTO> ListaEveEvento { get; set; }
        public List<EqEquipoDTO> ListaEqEquipo { get; set; }
        public List<SrmCriticidadDTO> ListaSrmCriticidad { get; set; }
        public List<SrmEstadoDTO> ListaSrmEstado { get; set; }
        public List<fwUserDTO> ListaFwUser { get; set; }
        public int SrmrecCodi { get; set; }
        public int EvenCodi { get; set; }
        public int EquiCodi { get; set; }
        public int Srmcrtcodi { get; set; }
        public int Srmstdcodi { get; set; }
        public int Usercode { get; set; }
        public string SrmrecFecharecomend { get; set; }
        public string SrmrecFechavencim { get; set; }
        public int? SrmrecDianotifplazo { get; set; }
        public string SrmrecTitulo { get; set; }
        public string SrmrecRecomendacion { get; set; }
        public string SrmrecActivo { get; set; }
        public string SrmrecUsucreacion { get; set; }
        public string SrmrecFeccreacion { get; set; }
        public string SrmrecUsumodificacion { get; set; }
        public string SrmrecFecmodificacion { get; set; }
        public string EvenIni { get; set; }
        public string EquiAbrev { get; set; }
        public string Srmcrtdescrip { get; set; }
        public string Srmstddescrip { get; set; }
        public string Username { get; set; }
        public int Accion { get; set; }
        public bool IndicadorGrabar{ get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        
    }

    public class BusquedaSrmRecomendacionModel
    {
        public List<SrmRecomendacionDTO> ListaSrmRecomendacion { get; set; }
        public List<EveEventoDTO> ListaEveEvento { get; set; }
        public List<EqEquipoDTO> ListaEqEquipo { get; set; }
        public List<SrmCriticidadDTO> ListaSrmCriticidad { get; set; }
        public List<SrmEstadoDTO> ListaSrmEstado { get; set; }
        public List<fwUserDTO> ListaFwUser { get; set; }
        public string FechaIni { get; set; }
        public string FechaFin { get; set; }
        public int NroPaginas { get; set; }
        public int NroMostrar { get; set; }
        public bool IndicadorPagina { get; set; }
        public bool AccionNuevo { get; set; }
        public bool AccionEditar { get; set; }
        public bool AccionEliminar { get; set; }

        public string Tipo { get; set; }
        public string Evenasunto { get; set; }
        public string Equiabrev { get; set; }
        public string Emprnomb { get; set; }
        public int Evencodi { get; set; }
    }

    public class FileModel
    {
        public string Fecha { get; set; }
        public List<EqFamiliaDTO> ListaFamilias { get; set; }
        public string Famabrev { get; set; }
        public string Famnomb { get; set; }
        public string Famestado { get; set; }
        public int Famcodi { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
    }

}
