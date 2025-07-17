using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RCA_CUADRO_PROG_USUARIO
    /// </summary>
    public interface IRcaCuadroProgUsuarioRepository
    {
        int Save(RcaCuadroProgUsuarioDTO entity);
        void Update(RcaCuadroProgUsuarioDTO entity);
        void Delete(int rcproucodi);
        RcaCuadroProgUsuarioDTO GetById(int rcproucodi);
        List<RcaCuadroProgUsuarioDTO> List();
        List<RcaCuadroProgUsuarioDTO> GetByCriteria();
        List<RcaCuadroProgUsuarioDTO> ListProgramaRechazoCarga(string empresasId, string codigoCuadroPrograma);
        List<RcaCuadroProgUsuarioDTO> ListEmpresasProgramaRechazoCarga(string bloqueHorario, string zona, string estacion, decimal medicion, string nombreEmpresa, string empresasId, string equiposId);

        List<AreaDTO> ListSubEstacion(int codigoZona);
        List<RcaCuadroProgUsuarioDTO> ListEnvioArchivoMagnitud(int programa, int cuadro, int suministrador, string cumplio);
        List<SiEmpresaDTO> ListSuministrador(int rccuadcodi);
        void UpdateEstado(int codigoCuadroPrograma, string estado);

        List<AreaDTO> ListZonas(int codigoNivel);

        void CargarDemandaUsuarioLibre();
        void CargarDemandaUsuarioLibreSicli(int indiceHP, int indiceHFP, string periodo, string fechaInicio, int idLectura, int tipoInfoCodi);
        void EliminarDemandaUsuarioLibre();

        void ActualizarDemandaUsuarioLibre(string fecha);
        int ListAntiguedadDatos();
        string ListUltimoPeriodo();

        //Reportes
        List<RcaCuadroProgUsuarioDTO> ReporteTotalDatos(int codigoCuadroPrograma, string eventoCTAF);
        List<RcaCuadroProgUsuarioDTO> ReporteDemoraEjecucion(int codigoCuadroPrograma, string eventoCTAF);
        List<RcaCuadroProgUsuarioDTO> ReporteDemoraReestablecimiento(int codigoCuadroPrograma, string eventoCTAF);
        List<RcaCuadroProgUsuarioDTO> ReporteInterrupcionesMenores(int codigoCuadroPrograma, string eventoCTAF);
        List<RcaCuadroProgUsuarioDTO> ReporteDemorasFinalizar(int codigoCuadroPrograma, string eventoCTAF);
        List<RcaCuadroProgUsuarioDTO> ReporteDemorasResarcimiento(int codigoCuadroPrograma, string eventoCTAF);

        #region Reporte Evaluacion Cumplimiento

        void DeleteTablasTemporalesReporteCumplimiento(string eventoCTAF);        
        List<RcaCuadroProgUsuarioDTO> ObtenerDatosEvaluacionCumplimiento(string eventoCTAF);
        void InsertarTablaTempEvaluacionCumplimiento(string eventoCTAF, int emprcodi, int equicodi, DateTime fechaInicioEjec, DateTime fechaFinEjec);
        void ActualizacionCalculoPorMinutoEvaluacionCumplimiento(string eventoCTAF);
        void RegistrarIntervaloEvaluacionCumplimiento(string eventoCTAF);
        List<RcaEvaluacionTempoDTO> ObtenerValoresEvaluacionCliente(string eventoCTAF);
        void ActualizarEvaluacionCliente(string eventoCTAF, int grupo, int equicodi, DateTime fechorini, DateTime fechorfin);
        List<RcaEvaluacionTempoDTO> ObtenerValoresGeneralesEvaluacionCumplimientoTmp(string eventoCTAF);
        decimal ObtenerValorPromedioEvaluacionCumplimiento(string eventoCTAF, int equicodi, DateTime fechaInicio);
        void ActualizarValorPromedioEvaluacionCumplimiento(string eventoCTAF, int equicodi, decimal valorPromedio, int grupo);
        List<RcaEvaluacionTempoDTO> ObtenerDatosCalculoFinalTempEvaluacionCumplimiento(string eventoCTAF);
        void ActualizarDatosCalculoFinalEvaluacionCumplimiento(string eventoCTAF, decimal potenciaNoRechazada, decimal potenciaPrevia, 
            DateTime fechaInicioEjec, DateTime fechaFinEjec, int equicodi);

        void InsertarEvaluacionFinal(string eventoCTAF);
        void ActualizarEvaluacionFinal(string eventoCTAF);

        List<RcaCuadroProgUsuarioDTO> ReporteEvalucionCumplimientoRMC(int codigoCuadroPrograma, string eventoCTAF);


        #endregion

        List<RcaCuadroProgUsuarioDTO> ReporteInterrrupInformeTecnico(int codigoCuadroPrograma, string eventoCTAF);
    }
}
