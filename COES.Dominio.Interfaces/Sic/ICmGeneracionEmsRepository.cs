using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_GENERACION_EMS
    /// </summary>
    public interface ICmGeneracionEmsRepository
    {
        int Save(CmGeneracionEmsDTO entity);
        void Update(CmGeneracionEmsDTO entity);
        void Delete(int genemscodi);
        CmGeneracionEmsDTO GetById(int genemscodi);
        List<CmGeneracionEmsDTO> List();
        List<CmGeneracionEmsDTO> GetByCriteria(DateTime fechaHora);
        void DeleteByFecha(DateTime genemsfecha, string estimador);
        List<CmGeneracionEmsDTO> ObtenerGeneracionPorCorrelativo(int correlativo);
        List<CmGeneracionEmsDTO> ObtenerGeneracionPorFechas(DateTime fechaInicio, DateTime fechaFin, string estimador);
        void ActualizarModoOperacion(CmGeneracionEmsDTO entity);

        #region Horas Operacion EMS
        List<CmGeneracionEmsDTO> GetListaGeneracionByEquipoFecha(DateTime fechaInicio, DateTime fechaFin, string empresa, string famcodi);
        #endregion

        #region Mejoras CMgN
        List<CmGeneracionEmsDTO> ObtenerGeneracionCostoIncremental(string equipos, DateTime fecha);
        #endregion

    }
}
