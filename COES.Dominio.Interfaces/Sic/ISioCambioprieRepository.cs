using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SIO_CAMBIOPRIE
    /// </summary>
    public interface ISioCambioprieRepository
    {
        int Save(SioCambioprieDTO entity);
        int Save(SioCambioprieDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(SioCambioprieDTO entity);
        void Delete(int campricodi);
        SioCambioprieDTO GetById(int campricodi);
        List<SioCambioprieDTO> List();
        List<SioCambioprieDTO> GetByCriteria();
    }
}
