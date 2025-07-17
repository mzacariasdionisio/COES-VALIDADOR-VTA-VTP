using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_CANAL_SP7
    /// </summary>
    public interface ITrCanalSp7Repository
    {
        int Save(TrCanalSp7DTO entity);
        void Update(TrCanalSp7DTO entity);
        void Delete(int canalcodi);
        TrCanalSp7DTO GetById(int canalcodi);
        TrCanalSp7DTO GetByIdBdTreal(int canalcodi);
        List<TrCanalSp7DTO> List();
        List<TrCanalSp7DTO> GetByCriteria();

        List<TrCanalSp7DTO> GetByIds(string canalcodi);
        List<TrCanalSp7DTO> GetByCanalnomb(string canalnomb);
        List<TrCanalSp7DTO> GetByZona(int zonacodi);
        List<TrCanalSp7DTO> GetByZonaAnalogico(int zonacodi);
        List<TrCanalSp7DTO> GetByFiltro(int filtrocodi);
        List<TrCanalSp7DTO> ListByZonaAndUnidad(string tipoPunto, int emprcodi, int zonacodi, string unidad);
        #region Mejoras IEOD
        List<TrCanalSp7DTO> ListarUnidadPorZona(int zonacodi);
        List<TrCanalSp7DTO> GetByCriteriaBdTreal(string canalcodis);
        #endregion       
        List<TrCanalSp7DTO> GetDatosSP7(string query);
    }
}
