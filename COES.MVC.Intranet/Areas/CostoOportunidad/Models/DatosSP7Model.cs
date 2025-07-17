using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.MVC.Intranet.Areas.CostoOportunidad.Models
{
    public class DatosSP7Model
    {
        #region Campos Comunes
        public bool TienePermisoNuevo { get; set; }
        public bool TienePermisoEditar { get; set; }
        public bool UsarLayoutModulo { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        #endregion

        public bool MostrarBtnTodos { get; set; }
        public bool MostrarBtnDF { get; set; }
        public List<int> ListaAnios { get; set; }
        public int Anio { get; set; }
        public List<CoPeriodoDTO> ListaPeriodos { get; set; }
        public List<CoVersionDTO> ListaVersiones { get; set; }
        public int IdPeriodo { get; set; }
    }

    public class CargaModel
    { 
        public List<CoMedicion48DTO> ListaURS { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public List<CoDatoSenialDTO> ListaConsulta { get;set; }
    }

}