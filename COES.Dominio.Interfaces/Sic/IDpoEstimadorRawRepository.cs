using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoEstimadorRawRepository
    {
        void Save(DpoEstimadorRawTmpDTO entity);
        void Update(DpoEstimadorRawTmpDTO entity);
        void Delete(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha);
        List<DpoEstimadorRawTmpDTO> List();
        DpoEstimadorRawTmpDTO GetById(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha);

        void BulkInsert(List<DpoEstimadorRawTmpDTO> entitys, string nombreTabla);
        void TruncateTablaTemporal(string nombreTabla);
        void DeleteRawsHistoricos(DateTime dporawfecha);
        List<DpoEstimadorRawTmpDTO> ListFileRawTmp();
        void MigrarRawsProcesadosHora(string sufAnioMes, string fecha, string hora, DateTime dFecha60, string sTabla);
        void InsertLogRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso);
        void DeleteLogRaw(string nomArchivoRaw, DateTime fecArchivoRaw);
        List<DpoEstimadorRawLogDTO> ListFilesLogRaw(string tipoProceso, DateTime dporawfecha);
        List<DpoEstimadorRawLogDTO> ListFilesLogRawHora(string tipoProceso, DateTime dporawfechainicio, DateTime dporawfechafinal);
        DpoEstimadorRawTmpDTO GetFileLogRaw(string nomarchivoraw, DateTime dporawfecha);
        List<DpoEstimadorRawTmpDTO> GetByIdFileRaw(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha);
        void UpdateRawsProcesadosHora(string sufAnioMes, string campoHn);
        void DeleteRawsProcesados(int ptomedicodi, int dpovarcodi, int dporawfuente, int dporawtipomedi, DateTime dporawfecha);
        void InsertFileRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso, string flag);
        void UpdateFileRaw(string nomArchivoRaw, DateTime fecArchivoRaw, string tipoProceso, string flag);
        List<DpoEstimadorRawFilesDTO> ListFilesRaw(int tipoProceso, DateTime dporawfecha);
        DpoEstimadorRawFilesDTO GetFileRaw(string nomarchivoraw);

        void UpdateRawsProcesados30Minutos(string sufAnioMes, string sFechaProceso);

        void DeleteREstimadorRawFileByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo);
        void DeleteREstimadorRawLogByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin, int tipo);
        void DeleteREstimadorRawTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin);
        void DeleteREstimadorRawCmTemporalByDiaProceso(DateTime fechaHoraInicio, DateTime fechaHoraFin);
        void DeleteREstimadorRawByDiaProceso(string sufAnioMes, DateTime fechaHoraInicio, DateTime fechaHoraFin);
        void DeleteREstimadorRawFileByNomArchivo(string sNomArchivo, int tipo);
        List<DpoEstimadorRawManualDTO> ListFileRawManual();
        List<DpoEstimadorRawManualDTO> ListEstimadorRawManualHora();
        List<DpoEstimadorRawManualDTO> ListEstimadorRawManualMinuto();
        void UpdateRawsProcesadosHoraManual(string sufAnioMes, string campoHn, DateTime fechaTemp, DateTime fecha);


        List<DpoEstimadorRawFilesDTO> ListFilesRawPorMinuto();
        List<DpoEstimadorRawFilesDTO> ListFilesRawIeod();

        List<DpoEstimadorRawDTO> ObtenerPorRangoFuente(int dporawfuente, string fecini, string fecfin);
        List<DpoEstimadorRawDTO> DatosPorPtoMedicion(string nombreTabla, int dporawfuente, int ptomedicion);


        DpoEstimadorRawFilesDTO ObtenerUltimoEstimadorRawFiles();
        string UpdateTMP();
        string UpdateRAW(DateTime dporawfecha);
        string DeleteEstimadorRawTemporal();
        DpoEstimadorRawFilesDTO ObtenerEstimadorRawFilesFlag();
        List<DpoEstimadorRawDTO> verificarFaltantesDia(DateTime dporawfecha);
        void UpdateRawsProcesados60Minutos(string sufAnioMes, string sFechaProceso, string sFechaSiguiente);
        List<DpoEstimadorRawDTO> ObtenerDemActivaPorDia(string nombreTabla,
            int dporawfuente, string ptomedicodi, string prnvarcodi,
            string dporawfecha);
        List<DpoEstimadorRawDTO> ReporteDatosEstimadorxHora(
            string nomTabla, string fecIni, string fecFin,
            int tipo);
    }
}
