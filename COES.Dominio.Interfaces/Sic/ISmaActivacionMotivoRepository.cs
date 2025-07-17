using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_ACTIVACION_MOTIVO
    /// </summary>
    public interface ISmaActivacionMotivoRepository
    {
        int Save(SmaActivacionMotivoDTO entity);
        int SaveTransaccional(SmaActivacionMotivoDTO entity, IDbConnection connection, DbTransaction transaction);
        void Update(SmaActivacionMotivoDTO entity);
        void Delete(int smaacmcodi);
        SmaActivacionMotivoDTO GetById(int smaacmcodi);
        List<SmaActivacionMotivoDTO> List();
        List<SmaActivacionMotivoDTO> GetByCriteria();
        List<SmaActivacionMotivoDTO> ObtenerPorActivacionesOferta(string smapaccodis);
    }
}
