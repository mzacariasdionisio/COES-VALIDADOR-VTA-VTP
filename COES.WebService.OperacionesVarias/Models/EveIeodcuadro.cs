using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.WebService.OperacionesVarias.Models
{
    /// <summary>
    /// Clase que mapea la tabla EVE_IEODCUADRO
    /// </summary>
    public class EveIeodcuadro
    {
        public int Iccodi { get; set; }
        public int? Equicodi { get; set; }
        public int? Subcausacodi { get; set; }
        public DateTime? Ichorini { get; set; }
        public DateTime? Ichorfin { get; set; }
        public string Icdescrip1 { get; set; }
        public string Icdescrip2 { get; set; }
        public string Icdescrip3 { get; set; }
        public string Iccheck1 { get; set; }
        public decimal? Icvalor1 { get; set; }
        public string Icvalor1Visible { get; set; }
        public string Lastuser { get; set; }
        public DateTime Lastdate { get; set; }
        public decimal? Numtrsgsubit { get; set; }
        public decimal? Numtrsgsostn { get; set; }
        public string Iccheck2 { get; set; }
        public int? Evenclasecodi { get; set; }
        public DateTime? Ichor3 { get; set; }
        public DateTime? Ichor4 { get; set; }
        public string genini { get; set; }
        public string genfin { get; set; }
        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
        public string Subcausadesc { get; set; }
        public string Iccheck3 { get; set; }
        public string Iccheck4 { get; set; }
        public decimal? Icvalor2 { get; set; }
        public string DemHP { get; set; }
        public string DemFP { get; set; }
        public string Icestado { get; set; }
        public string Icnombarchenvio { get; set; }
        public string Icnombarchfisico { get; set; }
        public string IcnombarchfisicoAnt { get; set; }
        public int CambioArchivo { get; set; }
        public string Equinomb { get; set; }
        public int Emprcodi { get; set; }
        public string Tareaabrev { get; set; }
        /// <summary>
        /// -1: eliminar, 0:lectura, 1:crear, 2:actualizar 
        /// </summary>
        public int opCrud { get; set; }

        #region PR5
        public string HoraIni { get; set; }
        public string HoraFin { get; set; }
        public int Areacodi { get; set; }
        public string Areaabrev { get; set; }
        public int Gpscodi { get; set; }
        public string Gpsnombre { get; set; }
        public string ListaNombreGps { get; set; }
        #endregion

        #region MigracionSGOCOES-GrupoB

        public string Emprabrev { get; set; }

        #endregion

        #region Indisponibilidades
        public string Areadesc { get; set; }
        public int Famcodi { get; set; }
        public string Iiccotipoindisp { get; set; }
        public decimal? Iiccopr { get; set; }
        public string Descripcion { get; set; }
        #endregion

        public List<EveIeodcuadroEquipos> ListadoEquipos { get; set; }
}

    
    public class EveIeodcuadroEquipos
    {
        public int Iccodi { get; set; }
        public int Equicodi { get; set; }
        public string Icdetcheck1 { get; set; }

        public string Emprnomb { get; set; }
        public string Areanomb { get; set; }
        public string Famabrev { get; set; }
        public string Equiabrev { get; set; }
    }
}

