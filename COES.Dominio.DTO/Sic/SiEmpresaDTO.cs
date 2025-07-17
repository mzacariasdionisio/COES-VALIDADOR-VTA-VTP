using System;
using System.Collections.Generic;
using COES.Base.Core;

namespace COES.Dominio.DTO.Sic
{
    /// <summary>
    /// Clase que mapea la tabla SI_EMPRESA
    /// </summary>
    public partial class SiEmpresaDTO : EntityBase
    {
        public int Emprcodi { get; set; } 
        public string Emprnomb { get; set; } 
        public int Tipoemprcodi { get; set; } 
        public string Emprdire { get; set; } 
        public string Emprtele { get; set; } 
        public string Emprnumedocu { get; set; } 
        public string Tipodocucodi { get; set; } 
        public string Emprruc { get; set; } 
        public string Emprabrev { get; set; } 
        public int? Emprorden { get; set; } 
        public string Emprdom { get; set; } 
        public string Emprsein { get; set; } 
        public string Emprrazsocial { get; set; } 
        public string Emprcoes { get; set; } 
        public string Lastuser { get; set; } 
        public DateTime? Lastdate { get; set; } 
        public int? Compcode { get; set; } 
        public string Inddemanda { get; set; }
        public string UserEmail { get; set; }
        public string Emprestado { get; set; }
        public string Tipoemprdesc { get; set; }
        public int Scadacodi { get; set; }
        public string Etiqueta { get; set; }
        //- alpha.HDT - 26/10/2016: Cambio para atender el requerimiento. 
        public string EmprCodOsinergmin { get; set; }       
        public string Emprdomiciliada { get; set; }
        public string Emprambito { get; set; }
        public int Emprrubro { get; set; }
        public string Empragente { get; set; }
        public string EmprnombAnidado { get; set; }
        public DateTime EmprestadoFecha { get; set; }
        public string TiposEmpresas { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
        public string Emprindusutramite { get; set; }
        public DateTime? Emprfecusutramite { get; set; }

        public List<SiTipoComportamientoDTO> ListaTipoComportamiento { get; set; }
        public string Emprindproveedor { get; set; }

        #region MigracionSGOCOES-GrupoB
        public List<PrGrupoDTO> ListaPrgrupo = new List<PrGrupoDTO>();
        public string Descripcion { get; set; }

        public int OrdenArea { get; set; }
        public string AreaOperativa { get; set; }
        public int TipoRango { get; set; }
        public decimal? HP { get; set; }
        public decimal? HFP { get; set; }
        public decimal Total { get; set; }
        public decimal Maximo { get; set; }

        #endregion

        #region Titularidad-Instalaciones-Empresas

        public string EmprestadoDesc { get; set; }
        public string EmprcoesDesc { get; set; }
        public string EmprseinDesc { get; set; }
        public string EmpragenteDesc { get; set; }

        #endregion

        #region CPPA-ASSETEC-2024
        public string EmprnombConcatenado { get; set; }
        #endregion
    }

    /// <summary>
    /// Clase para reporte de Registro de Integrantes
    /// </summary>
    /// <summary>
    /// Clase para reporte de RI
    /// </summary>
    public partial class SiEmpresaDTO : EntityBase
    {
        public string Emprnombrecomercial { get; set; }
        public string Emprdomiciliolegal { get; set; }
        public string Emprsigla { get; set; }
        public string Emprnumpartidareg { get; set; }
        public string Emprtelefono { get; set; }
        public string Emprfax { get; set; }
        public string Emprpagweb { get; set; }
        public string Emprnombcomercial { get; set; }
        public string Emprcartadjunto { get; set; }
        public string Emprcartadjuntofilename { get; set; }
        public string Emprestadoregistro { get; set; }
        public string Emprcondicion { get; set; }
        public DateTime? Emprfecingreso { get; set; }
        public DateTime? Emprfecbaja { get; set; }
        public string RpteNombres { get; set; }
        public string RpteCorreoElectronico { get; set; }
        public string RpteTelefono { get; set; }
        public string RpteTelfMovil { get; set; }
        public string RpteTipo { get; set; }
        public string RpteTipRepresentanteLegal { get; set; }
        public string RpteDocIdentidad { get; set; }
        public string RpteCargoEmpresa { get; set; }
        public string RpteBaja { get; set; }
        public string ValorEvaluarModalidad { get; set; }
        public string Modalidad { get; set; }
        public string TipoAgente { get; set; }
        public string EmpresaEstado { get; set; }
        public int HorasSGI { get; set; }
        public int HorasDJR { get; set; }
        public int ReviiteracionDRJ { get; set; }
        public int ReviiteracionSGI { get; set; }
        public int ReviCodiSGI { get; set; }
        public int ReviCodiDJR { get; set; }
        public DateTime? RevifecrevisionSGI { get; set; }
        public DateTime? RevifecrevisionDJR { get; set; }
        public string ReviFinalizadoSGI { get; set; }
        public string ReviFinalizadoDJR { get; set; }
        public DateTime? ReviFecFinalizadoSGI { get; set; }
        public DateTime? ReviFecFinalizadoDJR { get; set; }
        public string ReviNotificadoSGI { get; set; }
        public string ReviNotificadoDJR { get; set; }
        public DateTime? ReviFecNotificadoSGI { get; set; }
        public DateTime? ReviFecNotificadoDJR { get; set; }
        public string ReviEstadoSGI { get; set; }
        public string ReviEstadoDJR { get; set; }
        public int Emprnroconstancia { get; set; }
        public string ReviEnviadoSGI { get; set; }
        public string ReviEnviadoDJR { get; set; }
        public DateTime? ReviFecEnviadoSGI { get; set; }
        public DateTime? ReviFecEnviadoDJR { get; set; }
        public string ReviTerminadoSGI { get; set; }
        public string ReviTerminadoDJR { get; set; }
        public DateTime? ReviFecTerminadoSGI { get; set; }
        public DateTime? ReviFecTerminadoDJR { get; set; }
        public DateTime? Emprfechainscripcion { get; set; }
        public DateTime EmprfechainscripcionR { get; set; }
        public DateTime? Emprfechacreacion { get; set; }
        public SiTipoComportamientoDTO TipoComportamiento { get; set; }
        public List<SiRepresentanteDTO> Representante { get; set; }
        public List<RiRevisionDTO> Revision { get; set; }
        public int? Solicodi { get; set; }
        public int Emflcodi { get; set; }
        public int Fljcodi { get; set; }
        public DateTime? FLJFECHAORIG { get; set; }
        public DateTime? FLJFECHARECEP { get; set; }
        public DateTime? FLJFECHAPROCE { get; set; }
        public string FLJESTADO { get; set; }
        public string observacion { get; set; }
        public decimal corrnumproc { get; set; }
        public int filecodi { get; set; }
        public string TisoNombre { get; set; }
        public string SoliEstado { get; set; }
        public DateTime? SoliFecSolicitud { get; set; }
        public DateTime? SoliFecEnviado { get; set; }
        public int? reviiteracion { get; set; }
        public string tiporevision { get; set; }
        public DateTime? revifeccreacion { get; set; }
        public DateTime? revifecrevision { get; set; }
        public string reviestado { get; set; }
        public int? hora { get; set; }
        public DateTime? RpteFechaVigenciaPoder { get; set; }
        public int Emprnroregistro { get; set; }
        
        public DateTime? SoliFecModificacion { get; set; }
        public DateTime? Fecregistro { get; set; }
        public DateTime Fecaceptacion { get; set; }
    }

    /// <summary>
    /// Clase para devolucacion aporte
    /// </summary>
    /// <summary>
    /// Clase para devolucacion aporte
    /// </summary>
    public partial class SiEmpresaDTO : EntityBase
    {
        public decimal Porcentaje { get; set; }
        public string Apormontoparticipacion { get; set; }
        public int Estado { get; set; }
    }

    public partial class SiEmpresaMMEDTO : EntityBase
    {
        public int Emprmmecodi { get; set; }
        public int Emprcodi { get; set; }
        public string Emprnomb { get; set; }
        public string Emprruc { get; set; }
        public int TipoEmprcodi { get; set; }
        public string EmprTipoParticipante { get; set; }
        public string Emprestado { get; set; }
        public DateTime? Emprfecparticipacion { get; set; }
        public DateTime? Emprfecretiro { get; set; }
        public string Emprcomentario { get; set; }
        public int Emprmmeestado { get; set; }
        public string Emprusucreacion { get; set; }
        public DateTime? Emprfeccreacion { get; set; }
        public string Emprusumodificacion { get; set; }
        public DateTime? Emprfecmodificacion { get; set; }
    }
}
