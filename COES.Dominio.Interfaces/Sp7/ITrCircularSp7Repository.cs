using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_CIRCULAR_SP7
    /// </summary>
    public interface ITrCircularSp7RepositorySp7
    {
        void Save(TrCircularSp7DTO entity);
        void Update(TrCircularSp7DTO entity);
        void Delete(int canalcodi, DateTime canalfhsist);
        TrCircularSp7DTO GetById(int canalcodi, DateTime canalfhsist);
        List<TrCircularSp7DTO> List();
        List<TrCircularSp7DTO> GetByCriteria();
        int SaveTrCircularSp7Id(TrCircularSp7DTO entity);
        List<TrCircularSp7DTO> BuscarOperaciones(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize);
        List<TrCircularSp7DTO> BuscarOperacionesRango(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize);
        List<TrCircularSp7DTO> BuscarOperaciones(int canalcodi, DateTime canalFhsistInicio, DateTime canalFhsistFin, int nroPage, int pageSize, string analisis, int calidadNotRenew, int calidadHisNotCollected);
        
        int ObtenerNroFilas(string canalcodis, DateTime canalFhsistInicio, DateTime canalFhsistFin, string analisis, int calidadNotRenew, int calidadHisNotCollected);
        void CrearTabla(int i,string fecha);

        List<TrCircularSp7DTO> ObtenerConsultaFlujos(string table, string canales);
        List<TrCircularSp7DTO> ObtenerCircularesPorFecha(DateTime fecha);
        List<int> ObtenerCodigosDeCanalesParaMuestraInstantanea();
        List<TrCircularSp7DTO> ObtenerCircularesPorIntervalosDeFechaMuestraInstantanea(int canalcodigo, DateTime fechaDesde, DateTime fechaHasta);
        TrCanalSp7DTO GetCanalById(int canalcodi);
        void EliminarRegistroMuestraInstantanea(int canalcodi);
        void GenerarRegistroMuestraInstantanea(TrCanalSp7DTO canal, TrCircularSp7DTO circular, string usuario);
        List<TrCalidadSp7DTO> GetCalidades();
    }
}
