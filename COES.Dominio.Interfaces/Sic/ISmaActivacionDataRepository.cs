using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_ACTIVACION_DATA
    /// </summary>
    public interface ISmaActivacionDataRepository
    {
        int Save(SmaActivacionDataDTO entity);
        int SaveTransaccional(SmaActivacionDataDTO entity, IDbConnection connection, DbTransaction transaction);
        void Update(SmaActivacionDataDTO entity);
        void Delete(int smaacdcodi);
        SmaActivacionDataDTO GetById(int smaacdcodi);
        List<SmaActivacionDataDTO> List();
        List<SmaActivacionDataDTO> GetByCriteria();
        List<SmaActivacionDataDTO> ObtenerPorActivacionesOferta(string smapaccodis);
    }
}
