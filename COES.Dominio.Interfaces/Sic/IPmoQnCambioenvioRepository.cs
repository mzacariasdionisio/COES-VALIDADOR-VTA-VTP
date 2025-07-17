using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_QN_CAMBIOENVIO
    /// </summary>
    public interface IPmoQnCambioenvioRepository
    {
        int Save(PmoQnCambioenvioDTO entity);
        void Update(PmoQnCambioenvioDTO entity);
        void Delete(int qncmbecodi);
        PmoQnCambioenvioDTO GetById(int qncmbecodi);
        List<PmoQnCambioenvioDTO> List();
        List<PmoQnCambioenvioDTO> GetByCriteria();
        List<PmoQnCambioenvioDTO> ListByEnvio(string enviocodis);
        int Save(PmoQnCambioenvioDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
