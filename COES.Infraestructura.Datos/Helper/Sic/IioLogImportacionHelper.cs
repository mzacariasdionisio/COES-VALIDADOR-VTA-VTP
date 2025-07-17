// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: alpha
//
// Fecha creacion: 10/04/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================


using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_LOG_IMPORTACION
    /// </summary>
    public class IioLogImportacionHelper : HelperBase
    {

        #region CAMPOS: Variables de la clase.

        public string Ulogcodi = "ULOGCODI";
        public string Psiclicodi = "PSICLICODI";
        public string Ulogusucreacion = "ULOGUSUCREACION";
        public string Ulogfeccreacion = "ULOGFECCREACION";
        public string Ulogproceso = "ULOGPROCESO";
        public string Ulogtablaafectada = "ULOGTABLAAFECTADA";
        public string Ulognroregistrosafectados = "ULOGNROREGISTROSAFECTADOS";
        public string Ulogmensaje = "ULOGMENSAJE";
        public string Rcimcodi = "RCIMCODI";

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        public string Ulogtablacoes = "ULOGTABLACOES";
        
        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        public string Ulogidregistrocoes = "ULOGIDREGISTROCOES";

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        public string Ulogtipoincidencia = "ULOGTIPOINCIDENCIA";

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public string AnioMes = "aniomes";

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public string Empresaosinergminin = "empresaosinergminin";

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 

        #region Propiedades Tabla 04 y 05

        public string IdSuministrador = "IDSUMINISTRADOR";

        public string CodSuministradorSicli = "CODSUMINISTRADORSICLI";

        public string SuministradorSicli = "SUMINISTRADORSICLI";

        public string Ruc = "RUC";

        public string UsuarioLibre = "USUARIOLIBRE";

        public string CodUsuarioLibre = "CODUSUARIOLIBRE";

        public string BarraSuministro = "BARRASUMINISTRO";

        public string AreaDemanda = "AREADEMANDA";

        public string PagaVad = "PAGAVAD";

        public string ConsumoEnergiaHp = "CONSUMOENERGIAHP";

        public string ConsumoEnergiaHfp = "CONSUMOENERGIAHFP";

        public string MaximaDemandaHp = "MAXIMADEMANDAHP";

        public string MaximaDemandaHfp = "MAXIMADEMANDAHFP";

        public string CIIU = "CIIU";

        #endregion

        #region Propiedades Tabla 05

        public string CodSuministro = "CODSUMINISTRO";

        public string NombrePtoMedicion = "NombrePtoMedicion";

        public string FechaMedicion = "FechaMedicion";

        public string H1 = "H1";

        public string H2 = "H2";

        public string H3 = "H3";

        public string H4 = "H4";

        public string H5 = "H5";

        public string H6 = "H6";

        public string H7 = "H7";

        public string H8 = "H8";

        public string H9 = "H9";

        public string H10 = "H10";

        public string H11 = "H11";

        public string H12 = "H12";

        public string H13 = "H13";

        public string H14 = "H14";

        public string H15 = "H15";

        public string H16 = "H16";

        public string H17 = "H17";

        public string H18 = "H18";

        public string H19 = "H19";

        public string H20 = "H20";

        public string H21 = "H21";

        public string H22 = "H22";

        public string H23 = "H23";

        public string H24 = "H24";

        public string H25 = "H25";

        public string H26 = "H26";

        public string H27 = "H27";

        public string H28 = "H28";

        public string H29 = "H29";

        public string H30 = "H30";

        public string H31 = "H31";

        public string H32 = "H32";

        public string H33 = "H33";

        public string H34 = "H34";

        public string H35 = "H35";

        public string H36 = "H36";

        public string H37 = "H37";

        public string H38 = "H38";

        public string H39 = "H39";

        public string H40 = "H40";

        public string H41 = "H41";

        public string H42 = "H42";

        public string H43 = "H43";

        public string H44 = "H44";

        public string H45 = "H45";

        public string H46 = "H46";

        public string H47 = "H47";

        public string H48 = "H48";

        public string H49 = "H49";

        public string H50 = "H50";

        public string H51 = "H51";

        public string H52 = "H52";

        public string H53 = "H53";

        public string H54 = "H54";

        public string H55 = "H55";

        public string H56 = "H56";

        public string H57 = "H57";

        public string H58 = "H58";

        public string H59 = "H59";

        public string H60 = "H60";

        public string H61 = "H61";

        public string H62 = "H62";

        public string H63 = "H63";

        public string H64 = "H64";

        public string H65 = "H65";

        public string H66 = "H66";

        public string H67 = "H67";

        public string H68 = "H68";

        public string H69 = "H69";

        public string H70 = "H70";

        public string H71 = "H71";

        public string H72 = "H72";

        public string H73 = "H73";

        public string H74 = "H74";

        public string H75 = "H75";

        public string H76 = "H76";

        public string H77 = "H77";

        public string H78 = "H78";

        public string H79 = "H79";

        public string H80 = "H80";

        public string H81 = "H81";

        public string H82 = "H82";

        public string H83 = "H83";

        public string H84 = "H84";

        public string H85 = "H85";

        public string H86 = "H86";

        public string H87 = "H87";

        public string H88 = "H88";

        public string H89 = "H89";

        public string H90 = "H90";

        public string H91 = "H91";

        public string H92 = "H92";

        public string H93 = "H93";

        public string H94 = "H94";

        public string H95 = "H95";

        public string H96 = "H96";

        //Assetec - Demanda PO - Iteracion 2
        public string Meditotal = "MEDITOTAL";
        public string Ptomedicodi = "PTOMEDICODI";
        #endregion

        //- HDT Fin

        //- Adicionales

        public string Mensaje = "MENSAJE";

        public string Correlativo = "CORRELATIVO";

        #endregion

        #region CONSTRUCTORES: Definicion de constructores de la clase.

        public IioLogImportacionHelper()
            : base(Consultas.IioLogImportacionSql)
        {

        }

        #endregion

        #region PROPIEDADES: Propiedades de la clase.

        public string SqlDuplicadosConfiguracionCOES
        {
            get { return GetSqlXml("DuplicadosConfiguracionCOES"); }
        }

        public string SqlIncidentesSinPuntoMedicionCOES
        {
            get { return GetSqlXml("IncidentesSinPuntoMedicionCOES"); }
        }

        public string SqlDeleteByRcimcodi
        {
            get { return GetSqlXml("DeleteByRcimcodi"); }
        }

        public string SqlGetIncidenciasImportacion
        {
            get { return GetSqlXml("GetIncidenciasImportacion"); }
        }

        //- alpha.HDT - 12/07/2017: Cambio para atender el requerimiento. 
        public string SqlGetIncidenciasImportacionSuministro
        {
            get { return GetSqlXml("GetIncidenciasImportacionSuministro"); }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public string SqlReporteTabla04
        {
            get { return GetSqlXml("ReporteTabla04"); }
        }

        //- alpha.HDT - 26/04/2017: Cambio para atender el requerimiento. 
        public string SqlReporteTabla05
        {
            get { return GetSqlXml("ReporteTabla05"); }
        }

        #endregion

        #region METODOS: Metodos de la clase.

        public IioLogImportacionDTO Create(IDataReader dr)
        {

            IioLogImportacionDTO entity = new IioLogImportacionDTO();

            int iUlogcodi = dr.GetOrdinal(this.Ulogcodi);
            if (!dr.IsDBNull(iUlogcodi)) entity.UlogCodi = dr.GetInt32(iUlogcodi);

            int iPsiclicodi = dr.GetOrdinal(this.Psiclicodi);
            if (!dr.IsDBNull(iPsiclicodi)) entity.PsicliCodi = dr.GetString(iPsiclicodi);

            int iUlogusucreacion = dr.GetOrdinal(this.Ulogusucreacion);
            if (!dr.IsDBNull(iUlogusucreacion)) entity.UlogUsuCreacion = dr.GetString(iUlogusucreacion);

            int iUlogfeccreacion = dr.GetOrdinal(this.Ulogfeccreacion);
            if (!dr.IsDBNull(iUlogfeccreacion)) entity.UlogFecCreacion = dr.GetDateTime(iUlogfeccreacion);

            int iUlogProceso = dr.GetOrdinal(this.Ulogproceso);
            if (!dr.IsDBNull(iUlogProceso)) entity.UlogProceso = dr.GetString(iUlogProceso);

            int iUlogtablaafectada = dr.GetOrdinal(this.Ulogtablaafectada);
            if (!dr.IsDBNull(iUlogtablaafectada)) entity.UlogTablaAfectada = dr.GetString(iUlogtablaafectada);

            int iUlognroregistrosafectados = dr.GetOrdinal(this.Ulognroregistrosafectados);
            if (!dr.IsDBNull(iUlognroregistrosafectados)) entity.UlogNroRegistrosAfectados = dr.GetInt32(iUlognroregistrosafectados);

            int iUlogmensaje = dr.GetOrdinal(this.Ulogmensaje);
            if (!dr.IsDBNull(iUlogmensaje)) entity.UlogMensaje = dr.GetString(iUlogmensaje);

            int iRcimcodi = dr.GetOrdinal(this.Rcimcodi);
            if (!dr.IsDBNull(iRcimcodi)) entity.RcimCodi = dr.GetInt32(iRcimcodi);

            return entity;
        }

        #endregion

        #region Assetec - Demanda PO - Iteracion 2
        public string SqlListaMedidorDemandaSicli
        {
            get { return GetSqlXml("ListaMedidorDemandaSicli"); }
        } 
        public string SqlListGroupByMonthYear
        {
            get { return GetSqlXml("ListGroupByMonthYear"); }
        }
        public string SqlListDatosSICLI
        {
            get { return GetSqlXml("ListDatosSICLI"); }
        }
        public string SqlListSicliByDateRange
        {
            get { return GetSqlXml("ListSicliByDateRange"); }
        }
        #endregion
    }
}