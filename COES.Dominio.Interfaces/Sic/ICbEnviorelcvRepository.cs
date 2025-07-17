using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_ENVIORELCV
    /// </summary>
    public interface ICbEnviorelcvRepository
    {
        int Save(CbEnviorelcvDTO entity);
        void Update(CbEnviorelcvDTO entity);
        void Delete(int cbcvcodi);
        CbEnviorelcvDTO GetById(int cbcvcodi);
        List<CbEnviorelcvDTO> List();
        List<CbEnviorelcvDTO> GetByCriteria();
        int SaveTransaccional(CbEnviorelcvDTO entity, IDbConnection connection, IDbTransaction transaction);
    }
}
