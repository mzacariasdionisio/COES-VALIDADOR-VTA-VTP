using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_INTERRUPCION_SUMINISTRO_DET
    /// </summary>
    public interface IReInterrupcionSuministroDetRepository
    {
        int Save(ReInterrupcionSuministroDetDTO entity);
        void Update(ReInterrupcionSuministroDetDTO entity);
        void Delete(int reintdcodi);
        ReInterrupcionSuministroDetDTO GetById(int reintdcodi);
        List<ReInterrupcionSuministroDetDTO> List();
        List<ReInterrupcionSuministroDetDTO> GetByCriteria();
        List<ReInterrupcionSuministroDetDTO> ObtenerPorEmpresaPeriodo(int idEmpresa, int idPeriodo);
        List<ReInterrupcionSuministroDetDTO> ObtenerInterrupcionesPorResponsable(int idEmpresa, int idPeriodo);
        void ActualizarObservacion(ReInterrupcionSuministroDetDTO entity);
        void ActualizarRespuesta(ReInterrupcionSuministroDetDTO entity);
        ReInterrupcionSuministroDetDTO ObtenerPorOrden(int idSuministro, int orden);
        List<ReEmpresaDTO> ObtenerResponsablesFinalesPorPeriodo(int repercodi);
        List<ReInterrupcionSuministroDetDTO> GetConformidadResponsableNO(int repercodi);
        void ActualizarArchivoObservacion(int id, string extension);
        void ActualizarArchivoRespuesta(int id, string extension);
        void ActualizarDesdeTrimestral(int idInterrupcionSemestral, int idInterrupcionTrimestral);
        List<ReInterrupcionSuministroDetDTO> ObtenerRegistrosConSustento(int idInterrupcionSemestral, int idInterrupcionTrimestral);
        void ActualizarDatosAdicionales(ReInterrupcionSuministroDetDTO entity);
    }
}
