using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_ENVIO
    /// </summary>
    public interface ISiEnviodetRepository
    {
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);        
        void Save(SiEnviodetDTO entity);
        void Update(SiEnviodetDTO entity);        
        SiEnviodetDTO GetById(int idEnvio);
        List<SiEnviodetDTO> List();        
    }
}

