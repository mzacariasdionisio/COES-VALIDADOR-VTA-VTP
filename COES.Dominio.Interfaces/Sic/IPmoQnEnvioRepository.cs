using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_QN_ENVIO
    /// </summary>
    public interface IPmoQnEnvioRepository
    {
        int Save(PmoQnEnvioDTO entity);
        void Update(PmoQnEnvioDTO entity);
        void Delete(int qnbenvcodi);
        PmoQnEnvioDTO GetById(int qnbenvcodi);
        List<PmoQnEnvioDTO> List();
        List<PmoQnEnvioDTO> GetByCriteria(int anio, int tipo);
        int Save(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateBajaEnvio(int codigoEnvio, IDbConnection connection, IDbTransaction transaction);
        void UpdateVigente(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction);
        void UpdateOficial(PmoQnEnvioDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
