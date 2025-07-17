using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PMO_QN_MEDICION
    /// </summary>
    public interface IPmoQnMedicionRepository
    {
        int Save(PmoQnMedicionDTO entity);
        void Update(PmoQnMedicionDTO entity);
        void Delete(int qnmedcodi);
        PmoQnMedicionDTO GetById(int qnmedcodi);
        List<PmoQnMedicionDTO> List();
        List<PmoQnMedicionDTO> GetByCriteria(int enviocodi);
        int Save(PmoQnMedicionDTO entity, IDbConnection connection, IDbTransaction transaction);
        void DeleteMedicionXEnvio(int qnbenvcodi);
    }
}
