using COES.Base.Core;
using System;
using System.Collections.Generic;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla CB_ENVIO
    /// </summary>
    public partial class CbEnvioDTO : EntityBase
    {
        public int Cbenvcodi { get; set; }
        public int Grupocodi { get; set; }
        public int Equicodi { get; set; }
        public int Emprcodi { get; set; }
        public int Fenergcodi { get; set; }
        public int Estenvcodi { get; set; }
        public DateTime Cbenvfecsolicitud { get; set; }
        public string Cbenvususolicitud { get; set; }
        public DateTime? Cbenvfecaprobacion { get; set; }
        public string Cbenvusuaprobacion { get; set; }
        public string Cbenvestado { get; set; }
        public int Estcomcodi { get; set; }
        public string Cbenvplazo { get; set; }
        public DateTime? Cbenvfecpreciovigente { get; set; }
        public string Cbenvmoneda { get; set; }
        public string Cbenvunidad { get; set; }
        public DateTime Cbenvfecfinrptasolicitud { get; set; }
        public DateTime? Cbenvfecfinsubsanarobs { get; set; }
        public DateTime? Cbenvfecmodificacion { get; set; }

        public string Cbenvobs { get; set; }
        public DateTime? Cbenvfecampl { get; set; }
        public string Cbenvitem106 { get; set; }

        public int? Cbftcodi { get; set; }
        public DateTime? Cbenvfechaperiodo { get; set; }
        public string Cbenvtipocentral { get; set; }
        public DateTime? Cbenvfecsistema { get; set; }
        public string Cbenvtipocarga { get; set; }
        public string Cbenvusucarga { get; set; }
        public int? Cbenvtipoenvio { get; set; }
    }

    public partial class CbEnvioDTO
    {
        public string Envioplazo { get; set; }
        public string ColorEnvioplazo { get; set; }
        public int DiffDiaHabil { get; set; }
        public int DiffDiaCalendario { get; set; }
        public int DiffHoras { get; set; }

        public string Estenvnomb { get; set; }
        public string Estenvcolor { get; set; }

        public string CbenvfeccreacionDesc { get; set; }
        public string CbenvfecsolicitudDesc { get; set; }
        public string CbenvfecmodificacionDesc { get; set; }
        public string FechaVigenciaDesc { get; set; }
        public string FechaAprobacionDesc { get; set; }
        public string CbenvfecfinrptasolicitudDesc { get; set; }
        public string CbenvfecfinsubsanarobsDesc { get; set; }
        public string CbenvfecamplDesc { get; set; }
        public string Emprnomb { get; set; }
        public string Fenergnomb { get; set; }
        public string Equinomb { get; set; }
        public string Gruponomb { get; set; }

        public CbVersionDTO VersionActual { get; set; }
        public CbLogenvioDTO LogEnvioActual { get; set; }
        public List<PrGrupodatDTO> ListaGrupodat { get; set; }

        //tipo de combustible
        public string Estcomnomb { get; set; }

        public bool EsEnvioAprobado { get; set; }
        public bool EsEditableExtranet { get; set; }
        public bool EsEditableIntranet { get; set; }
        public List<int> ListaRepcodiCv { get; set; }
        public List<string> ListaCorreoCCagente { get; set; }

        //public List<string> ListaNuevaPrecios { get; set; } = new List<string>();

        //Ultima version del envio
        public int Cbvercodi { get; set; }
        public string Cbverusucreacion { get; set; }
        public DateTime FechaRevisionCoes { get; set; }
        public DateTime FechaSubsanacionObs { get; set; }
        public string FechaDesaprobacionDesc { get; set; }

        #region region CCGAS.PR31

        public string MesVigenciaDesc { get; set; }            
        public string FechaAsignacionDesc { get; set; }
        public string CostoVigenteDesdeDesc { get; set; }
        public string EstadoDesc { get; set; }
        public string CbenvfecsolicitudDateDesc { get; set; }
        public string UsuariosAgentesTotales { get; set; }
        public string UsuarioUltimoEvento { get; set; }
        public List<EqEquipoDTO> ListaCentralesAprobadas { get; set; }
        public List<EqEquipoDTO> ListaCentralesDesaprobadas { get; set; }
        public string TipoOpcionAsignado { get; set; }
        public List<CbEnvioCentralDTO> ListaCentralesSinEnvio { get; set; }
        public string CbenvfecsistemaDesc { get; set; }
        public bool EsCancelable { get; set; }
        public string CbenvtipocentralDesc { get; set; }
        public int NumVersion { get; set; }

        #endregion
    }
}
