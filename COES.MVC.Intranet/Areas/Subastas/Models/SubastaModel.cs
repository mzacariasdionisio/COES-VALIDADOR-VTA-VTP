using COES.Dominio.DTO.Sic;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;
using COES.Servicios.Aplicacion.FormatoMedicion;
using COES.Servicios.Aplicacion.Subastas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COES.MVC.Intranet.Areas.Subastas.Models
{

    #region Métodos Registro Usuarios
    public class SmaRegistroUsuariosModel
    {
        public List<SmaUserEmpresaDTO> SmaUserEmpresa { get; set; }

    }

    public class UsuariosUrsModel
    {

        public List<SmaUsuarioUrsDTO> ListaUsuarioUrs { get; set; }

        public int UursCodi { get; set; }
        public int? UrsCodi { get; set; }
        public string UursUsucreacion { get; set; }
        public string UursUsumodificacion { get; set; }
        public DateTime? UursFecmodificacion { get; set; }
        public int? UserCode { get; set; }
        public string UursEstado { get; set; }
        public DateTime? UursFeccreacion { get; set; }
        // JOIN
        public string UrsNomb { get; set; }
        public string UrsTipo { get; set; }
        public int GrupoCodi { get; set; }
        public string GrupoNom { get; set; }
        public string UserName { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public SmaUsuarioUrsDTO ListaComboTodos
        {
            get
            {
                SmaUsuarioUrsDTO a = new SmaUsuarioUrsDTO();
                a.Uurscodi = 0;
                a.Ursnomb = "(TODOS)";
                return a;
            }
        }
    }

    public class UrsModoOperacionModel
    {

        public int UrsCodi { get; set; }
        public string UrsNomb { get; set; }

        public int? Grupocodi { get; set; }

        public string Gruponomb { get; set; }

        public List<SmaUrsModoOperacionDTO> ListaUrsModoOperacion { get; set; }
        public List<SmaUrsModoOperacionDTO> ListaUrsModoOperacionMO { get; set; }

        public SmaUrsModoOperacionDTO ListaComboTodos
        {
            get
            {
                SmaUrsModoOperacionDTO a = new SmaUrsModoOperacionDTO();
                a.Urscodi = 0;
                a.Ursnomb = "(TODOS)";
                return a;
            }
        }

        public SmaUrsModoOperacionDTO ListaComboSeleccione
        {
            get
            {
                SmaUrsModoOperacionDTO a = new SmaUrsModoOperacionDTO();
                a.Urscodi = 0;
                a.Ursnomb = "(SELECCIONE)";
                return a;
            }
        }
    }

    public class EmpresaModel
    {

        public List<SmaUserEmpresaDTO> ListaEmpresaUsuarios { get; set; }

        public SmaUserEmpresaDTO ListaComboTodosUsuario
        {
            get
            {
                SmaUserEmpresaDTO a = new SmaUserEmpresaDTO();
                a.Usercode = -1;
                a.Username = "(TODOS)";
                return a;
            }
        }

        public SmaUserEmpresaDTO ListaComboTodosEmpresa
        {
            get
            {
                SmaUserEmpresaDTO a = new SmaUserEmpresaDTO();
                a.Emprcodi = -1;
                a.Emprnomb = "(TODOS)";
                return a;
            }
        }

        public int Usercode { get; set; }
        public int Emprcodi { get; set; }
        public string Username { get; set; }
        public string Emprnomb { get; set; }


    }
    #endregion

    #region Métodos Configuracion

    public class SmaConfiguracionModel
    {
        public int ConfsmCorrelativo { get; set; }
        public string ConfsmAtributo { get; set; }
        public string ConfsmParametro { get; set; }
        public string ConfsmValor { get; set; }
        public string ConfsmUsucreacion { get; set; }
        public DateTime? ConfsmFeccreacion { get; set; }
        public string ConfsmUsumodificacion { get; set; }
        public DateTime? ConfsmFecmodificacion { get; set; }
        public string ConfsmEstado { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public bool AccesoEditar { get; set; }
        public string Modulo { get; set; }

        //Lista parámetros
        public List<PrGrupoDTO> ListaModo { get; set; }
        public List<PrGrupodatDTO> ListaGrupodat { get; set; }
        public string Fecha { get; set; }
        public List<PrGrupodatDTO> ListaParametros { get; set; }

        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string BandaURS { get; set; }
        public string Acta { get; set; }
        public List<SmaUrsModoOperacionDTO> ModosOpList { get; set; }
        public int TipoAccion { get; set; }
        public int Deleted { get; set; }
        public string FechaData { get; set; }
        public int Grupocodi { get; set; }
        public string FechaConsulta { get; set; }

        public string TamanioMaxActa { get; set; }
        public List<FileData> ListaDocumentos { get; set; }
        public List<FileData> ListaDocumentosFiltrado { get; set; }

        //Motivos
        public List<SmaMaestroMotivoDTO> ListaMotivo { get; set; }

        //Ampliación de plazo
        public List<SmaAmpliacionPlazoDTO> ListaAmpliacion { get; set; }
        public string PlazoDefectoDia { get; set; }
        public string PlazoDefectoHora { get; set; }
        public string NuevoPlazoDefectoDia { get; set; }
    }

    #endregion

    #region Métodos Consultas Oferta
    public class OfertaModel
    {
        public List<SmaUserEmpresaDTO> ListaUser { get; set; }
        public List<SmaOfertaDTO> ListaOferta { get; set; }
        public int? OferTipo { get; set; }
        public DateTime? OferFechainicio { get; set; }
        public DateTime? OferFechafin { get; set; }
        public string OferCodenvio { get; set; }
        public string OferEstado { get; set; }
        public string OferUsucreacion { get; set; }
        public DateTime? OferFeccreacion { get; set; }
        public string OferUsumodificacion { get; set; }
        public DateTime? OferFecmodificacion { get; set; }
        public DateTime? OferFechaenvio { get; set; }
        public string OferfechaenvioDesc { get; set; }
        public int OferCodi { get; set; }
        public int UserCode { get; set; }

        public string Username { get; set; }
        public string Emprnomb { get; set; }
        public int OfdeCodi { get; set; }
        public DateTime? OfdeHorainicio { get; set; }
        public DateTime? OfdeHorafin { get; set; }
        public int GrupoCodi { get; set; }

        public string OferListMO { get; set; }

        // REPORTE/
        //public int Grupocodi { get; set; }

        public string GrupoNomb { get; set; }

        public DateTime? RepOfecha { get; set; }

        public int RepoIntvhini { get; set; } //Rango  30 mins
        public int RepoIntvmini { get; set; } //Rango  30 mins

        public int RepoIntvhfin { get; set; } // Rango 30 mins
        public int RepoIntvmfin { get; set; } // Rango 30 mins

        public string RepoHoraini { get; set; } // Hora Inicio
        public string RepoHorafin { get; set; } // Hora Fin

        public int UrsCodi { get; set; }

        public decimal RepoPotmaxofer { get; set; }

        public string RepoPrecio { get; set; }

        public decimal RepoMoneda { get; set; }

        public int? RepoNrounit { get; set; }

        public string GrupoTipo { get; set; }
        public string UrsTipo { get; set; }

        public int Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }

        public List<HandsonModel> ListaTab { get; set; }
        public string HtmlListaEnvio { get; set; }

        #region ReservaSecundaria
        public string FechaInicial { get; set; }
        public string FechaFinal { get; set; }
        public string ResultadoSubir { get; set; }
        public string ResultadoBajar { get; set; }
        public string TipoMensaje { get; set; }
        #endregion

        #region OfertasDefectoActivadas
        public List<SmaUserEmpresaDTO> ListaEmpresaUsuarios { get; set; }
        public List<SmaUserEmpresaDTO> ListaComboTodosUsuario { get; set; }
        public List<SmaUrsModoOperacionDTO> ListaUrsModoOperacion { get; set; }
        public List<SmaOfertaDTO> ListaOfertaSubir { get; set; }
        public List<SmaOfertaDTO> ListaOfertaBajar { get; set; }
        public string NombreArchivo { get; set; }
        public bool FlagTieneDatos { get; set; }
        public List<Bitacora> ListaBitacora { get; set; }
        #endregion

        #region Graficos
        public GraficoWeb Grafico { get; set; }
        public bool ExisteReserva { get; set; }
        #endregion
    }

    #endregion

    #region Métodos Consultas Oferta

    public class ProcesoModel
    {
        public string Fecha { get; set; }
        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public bool TienePermiso { get; set; }

        public string Fecha2 { get; set; }
        public bool TieneOpcionEspecialHabilitado { get; set; }

        public List<SmaParamProcesoDTO> ListProceso { get; set; }
        public List<SmaModoOperValDTO> ModoOperacion { get; set; }

        public string FechaMesActual { get; set; }
        public string FechaMesSig { get; set; }
    }

    #endregion

    #region Notificación

    public class NotificacionModel
    {
        public List<SiPlantillacorreoDTO> ListadoPlantillasCorreo { get; set; }
        public SiPlantillacorreoDTO PlantillaCorreo { get; set; }
        public List<SiTipoplantillacorreoDTO> ListaTipoPlantilla { get; set; }
        public List<FtExtEtapaDTO> ListaEtapa { get; set; }

        public string LogoEmail { get; set; }
        public int TipoCorreo { get; set; }
        public bool AccionGrabar { get; set; }

        public string Resultado { get; set; }
        public string Mensaje { get; set; }
        public string Detalle { get; set; }
        public string TieneEjecucionManual { get; set; }
    }

    #endregion
}
