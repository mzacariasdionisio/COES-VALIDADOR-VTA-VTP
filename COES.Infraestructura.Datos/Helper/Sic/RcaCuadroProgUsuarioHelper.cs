using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RCA_CUADRO_PROG_USUARIO
    /// </summary>
    public class RcaCuadroProgUsuarioHelper : HelperBase
    {
        public RcaCuadroProgUsuarioHelper(): base(Consultas.RcaCuadroProgUsuarioSql)
        {
        }

        public RcaCuadroProgUsuarioDTO Create(IDataReader dr)
        {
            RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

            int iRcprouusumodificacion = dr.GetOrdinal(this.Rcprouusumodificacion);
            if (!dr.IsDBNull(iRcprouusumodificacion)) entity.Rcprouusumodificacion = dr.GetString(iRcprouusumodificacion);

            int iRcproufecmodificacion = dr.GetOrdinal(this.Rcproufecmodificacion);
            if (!dr.IsDBNull(iRcproufecmodificacion)) entity.Rcproufecmodificacion = dr.GetDateTime(iRcproufecmodificacion);

            int iRcproucodi = dr.GetOrdinal(this.Rcproucodi);
            if (!dr.IsDBNull(iRcproucodi)) entity.Rcproucodi = Convert.ToInt32(dr.GetValue(iRcproucodi));

            int iRccuadcodi = dr.GetOrdinal(this.Rccuadcodi);
            if (!dr.IsDBNull(iRccuadcodi)) entity.Rccuadcodi = Convert.ToInt32(dr.GetValue(iRccuadcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iRcproudemanda = dr.GetOrdinal(this.Rcproudemanda);
            if (!dr.IsDBNull(iRcproudemanda)) entity.Rcproudemanda = dr.GetDecimal(iRcproudemanda);

            int iRcproufuente = dr.GetOrdinal(this.Rcproufuente);
            if (!dr.IsDBNull(iRcproufuente)) entity.Rcproufuente = dr.GetString(iRcproufuente);

            int iRcproudemandaracionar = dr.GetOrdinal(this.Rcproudemandaracionar);
            if (!dr.IsDBNull(iRcproudemandaracionar)) entity.Rcproudemandaracionar = dr.GetDecimal(iRcproudemandaracionar);

            int iRcprouestregistro = dr.GetOrdinal(this.Rcprouestregistro);
            if (!dr.IsDBNull(iRcprouestregistro)) entity.Rcprouestregistro = dr.GetString(iRcprouestregistro);

            int iRcprouusucreacion = dr.GetOrdinal(this.Rcprouusucreacion);
            if (!dr.IsDBNull(iRcprouusucreacion)) entity.Rcprouusucreacion = dr.GetString(iRcprouusucreacion);

            int iRcproufeccreacion = dr.GetOrdinal(this.Rcproufeccreacion);
            if (!dr.IsDBNull(iRcproufeccreacion)) entity.Rcproufeccreacion = dr.GetDateTime(iRcproufeccreacion);

            int iRcproucargarechazarcoord = dr.GetOrdinal(this.Rcproucargarechazarcoord);
            if (!dr.IsDBNull(iRcproucargarechazarcoord)) entity.Rcproucargarechazarcoord = dr.GetDecimal(iRcproucargarechazarcoord);

            int iRccuadhorinicoord = dr.GetOrdinal(this.Rccuadhorinicoord);
            if (!dr.IsDBNull(iRccuadhorinicoord)) entity.Rccuadhorinicoord = dr.GetString(iRccuadhorinicoord);

            int iRccuadhorfincoord = dr.GetOrdinal(this.Rccuadhorfincoord);
            if (!dr.IsDBNull(iRccuadhorfincoord)) entity.Rccuadhorfincoord = dr.GetString(iRccuadhorfincoord);

            int iRcproucargarechazarejec = dr.GetOrdinal(this.Rcproucargarechazarejec);
            if (!dr.IsDBNull(iRcproucargarechazarejec)) entity.Rcproucargarechazarejec = dr.GetDecimal(iRcproucargarechazarejec);

            int iRccuadhoriniejec = dr.GetOrdinal(this.Rccuadhoriniejec);
            if (!dr.IsDBNull(iRccuadhoriniejec)) entity.Rccuadhoriniejec = dr.GetString(iRccuadhoriniejec);

            int iRccuadhorfinejec = dr.GetOrdinal(this.Rccuadhorfinejec);
            if (!dr.IsDBNull(iRccuadhorfinejec)) entity.Rccuadhorfinejec = dr.GetString(iRccuadhorfinejec);

            return entity;
        }


        #region Mapeo de Campos

        public string Rcprouusumodificacion = "RCPROUUSUMODIFICACION";
        public string Rcproufecmodificacion = "RCPROUFECMODIFICACION";
        public string Rcproucodi = "RCPROUCODI";
        public string Rccuadcodi = "RCCUADCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodi = "EQUICODI";
        public string Rcproudemanda = "RCPROUDEMANDA";
        public string Rcproufuente = "RCPROUFUENTE";
        public string Rcproudemandaracionar = "RCPROUDEMANDARACIONAR";
        public string Rcprouestregistro = "RCPROUESTREGISTRO";
        public string Rcprouusucreacion = "RCPROUUSUCREACION";
        public string Rcproufeccreacion = "RCPROUFECCREACION";
        public string Rcproucargadisponible = "RCPROUCARGADISPONIBLE";
        public string Rcproufactork = "RCPROUFACTORK";
        public string Rcprouemprcodisuministrador = "RCPROUEMPRCODISUMINISTRADOR";

        public string Empresa = "EMPRESA";
        public string Suministrador = "SUMINISTRADOR";
        public string Subestacion = "SUBESTACION";
        public string Puntomedicion = "PUNTOMEDICION";
        public string Rcproudemandaatender = "RCPROUDEMANDAATENDER";
        public string Demanda = "DEMANDA";
        public string Rcdeulfuente = "RCDEULFUENTE";

        public string Rcejeucargarechazadapreliminar = "RCEJEUCARGARECHAZADAPRELIMINAR";
        public string Rcejeufechoriniciopreliminar = "RCEJEUFECHORINICIOPRELIMINAR";
        public string Rcejeufechorfinpreliminar = "RCEJEUFECHORFINPRELIMINAR";
        public string Rcejeucargarechazada = "RCEJEUCARGARECHAZADA";
        public string Rcejeufechorinicio = "RCEJEUFECHORINICIO";
        public string Rcejeufechorfin = "RCEJEUFECHORFIN";
        public string Cumplio = "CUMPLIO";
        public string Areacodi = "AREACODI";
        public string Areanomb = "AREANOMB";
        public string Anivelcodi = "ANIVELCODI";
        public string Osinergcodi = "OSINERGCODI";

        public string Rcproucargarechazarcoord = "RCPROUCARGARECHAZARCOORD";
        public string Rccuadhorinicoord = "RCCUADHORINICOORD";
        public string Rccuadhorfincoord = "RCCUADHORFINCOORD";
        public string Rcproucargarechazarejec = "RCPROUCARGARECHAZAREJEC";
        public string Rccuadhoriniejec = "RCCUADHORINIEJEC";
        public string Rccuadhorfinejec = "RCCUADHORFINEJEC";

        public string RcejeufechorfinRep = "RCEJEUFECHORFIN";
        public string RcejeufechorinicioRep = "RCEJEUFECHORINICIO";

        public string RccuadfechoriniRep = "RCCUADFECHORINICOORD";
        public string RccuadfechorfinRep = "RCCUADFECHORFINCOORD";

        public string RcreevpotenciaprompreviaRep = "RCREEVPOTENCIAPROMPREVIA";
        public string RccuadfechoriniPrevioRep = "HORINIPREVIO";
        public string RccuadfechorfinPrevioRep = "HORFINPREVIO";

        public string Duracion = "DURACION";
        public string Evaluacion = "EVALUACION";

        public string Rcproudemandareal = "RCPROUDEMANDAREAL";

        #endregion

        public string SqlListProgramaRechazoCarga
        {
            get { return base.GetSqlXml("ListProgramaRechazoCarga"); }
        }

        public string SqListEmpresasProgramaRechazoCarga
        {
            get { return base.GetSqlXml("ListEmpresasProgramaRechazoCarga"); }
        }

        public string SqlListSubEstacion
        {
            get { return base.GetSqlXml("ListSubEstacion"); }
        }

        public string SqlListEnvioArchivoMagnitud
        {
            get { return base.GetSqlXml("ListEnvioArchivoMagnitud"); }
        }

        public string SqlListSuministrador
        {
            get { return base.GetSqlXml("ListSuministrador"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("SqlUpdateEstado"); }
        }

        public string SqlListZonas
        {
            get { return base.GetSqlXml("ListZonas"); }
        }
        public string SqlCargarDemandaUsuarioLibre
        {
            get { return GetSqlXml("CargarDemandaUsuarioLibre"); }
        }

        public string SqlCargarDemandaUsuarioLibreSicli
        {
            get { return GetSqlXml("CargarDemandaUsuarioLibreSicli"); }
        }

        public string SqlActualizarDemandaUsuarioLibre
        {
            get { return GetSqlXml("ActualizarDemandaUsuarioLibre"); }
        }
        public string SqlEliminarDemandaUsuarioLibre
        {
            get { return GetSqlXml("EliminarDemandaUsuarioLibre"); }
        }
        public string SqlListarAntiguedadDatos
        {
            get { return GetSqlXml("ListarAntiguedadDatos"); }
        }
        public string SqlListarUltimoPeriodo
        {
            get { return GetSqlXml("ListarUltimoPeriodo"); }
        }

        public string SqlReporteTotalDatos
        {
            get { return base.GetSqlXml("ReporteTotalDatos"); }
        }
        public string SqlReporteDemoraEjecucion
        {
            get { return base.GetSqlXml("ReporteDemoraEjecucion"); }
        }
        public string SqlReporteDemoraReestablecimiento
        {
            get { return base.GetSqlXml("ReporteDemoraReestablecimiento"); }
        }
        public string SqlReporteInterrupcionesMenores
        {
            get { return base.GetSqlXml("ReporteInterrupcionesMenores"); }
        }
        public string SqlReporteDemorasFinalizar
        {
            get { return base.GetSqlXml("ReporteDemorasFinalizar"); }
        }
        public string SqlReporteDemorasResarcimiento
        {
            get { return base.GetSqlXml("ReporteDemorasResarcimiento"); }
        }

        public string SqlReporteEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ReporteEvaluacionCumplimiento"); }
        }
        public string SqlReporteInterrupInformeTecnico
        {
            get { return base.GetSqlXml("ReporteInterrupInformeTecnico"); }
        }

        #region 2. Reporte Evaluacion Cumplimiento


        public string SqlDeleteRcaMedidorMinutoTmp
        {
            get { return base.GetSqlXml("DeleteRcaMedidorMinutoTmp"); }
        }

        public string SqlDeleteRcaEvaluacionTmp
        {
            get { return base.GetSqlXml("DeleteRcaEvaluacionTmp"); }
        }
        public string SqlDeleteRcaResulEvaluacionTmp
        {
            get { return base.GetSqlXml("DeleteRcaResulEvaluacionTmp"); }
        }

        public string SqlObtenerDatosEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ObtenerDatosEvaluacionCumplimiento"); }
        }

        public string SqlInsertarTablaTempEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("InsertarTablaTempEvaluacionCumplimiento"); }
        }

        public string SqlActualizacionCalculoPorMinutoEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ActualizacionCalculoPorMinutoEvaluacionCumplimiento"); }
        }

        public string SqlRegistrarIntervaloEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("RegistrarIntervaloEvaluacionCumplimiento"); }
        }
        public string SqlObtenerValoresEvaluacionCliente
        {
            get { return base.GetSqlXml("ObtenerValoresEvaluacionCliente"); }
        }
        public string SqlActualizarEvaluacionCliente
        {
            get { return base.GetSqlXml("ActualizarEvaluacionCliente"); }
        }

        public string SqlObtenerValoresGeneralesEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ObtenerValoresGeneralesEvaluacionCumplimiento"); }
        }

        public string SqlObtenerValorPromedioEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ObtenerValorPromedioEvaluacionCumplimiento"); }
        }

        public string SqlActualizarValorPromedioEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ActualizarValorPromedioEvaluacionCumplimiento"); }
        }

        public string SqlObtenerDatosCalculoFinalEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ObtenerDatosCalculoFinalEvaluacionCumplimiento"); }
        }

        public string SqlActualizarDatosCalculoFinalEvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ActualizarDatosCalculoFinalEvaluacionCumplimiento"); }
        }
        public string SqlActualizarDatosCalculoFinal2EvaluacionCumplimiento
        {
            get { return base.GetSqlXml("ActualizarDatosCalculoFinal2EvaluacionCumplimiento"); }
        }

        public string SqlInsertarResultadoEvaluacionFinal
        {
            get { return base.GetSqlXml("InsertarResultadoEvaluacionFinal"); }
        }

        public string SqlActualizarResultadoEvaluacionFinal
        {
            get { return base.GetSqlXml("ActualizarResultadoEvaluacionFinal"); }
        }
        public string SqlActualizarResultadoEvaluacionFinal2
        {
            get { return base.GetSqlXml("ActualizarResultadoEvaluacionFinal2"); }
        }
        public string SqlActualizarResultadoEvaluacionFinal3
        {
            get { return base.GetSqlXml("ActualizarResultadoEvaluacionFinal3"); }
        }
        public string SqlActualizarResultadoEvaluacionFinalPotenciaRechazada
        {
            get { return base.GetSqlXml("ActualizarResultadoEvaluacionFinalPotenciaRechazada"); }
        }
        public string SqlActualizarResultadoEvaluacionFinalPotenciaRechazadaEvaluacion
        {
            get { return base.GetSqlXml("ActualizarResultadoEvaluacionFinalPotenciaRechazadaEvaluacion"); }
        }
       

        #endregion
    }
}
