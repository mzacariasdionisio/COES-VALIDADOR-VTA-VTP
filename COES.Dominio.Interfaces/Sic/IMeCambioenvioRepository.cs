using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_CAMBIOENVIO
    /// </summary>
    public interface IMeCambioenvioRepository
    {
        int GetMaxId();
        void Save(MeCambioenvioDTO entity);
        int SaveTransaccional(MeCambioenvioDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(MeCambioenvioDTO entity);
        void Delete(int idEnvio);

        List<MeCambioenvioDTO> List(int idPto, int idTipoInfo, int idFormato, DateTime fecha);
        List<MeCambioenvioDTO> GetById(int idEnvio);
        List<MeCambioenvioDTO> GetByCriteria(string idsEmpresa, DateTime fecha, int idFormato);
        List<MeCambioenvioDTO> GetAllCambioEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, int idEnvio, int idEmpresa);
        List<MeCambioenvioDTO> GetAllOrigenEnvio(int idFormato, DateTime fechaInicio, DateTime fechaFin, DateTime fechaPeriodo, int idEmpresa);
        List<MeCambioenvioDTO> ListByEnvio(string enviocodis);
    }
}
