using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_SUSTENTO_DET_REL_ARCHIVO
    /// </summary>
    public interface IInSustentoDetRelArchivoRepository
    {
        int GetMaxId();
        int Save(InSustentoDetRelArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InSustentoDetRelArchivoDTO entity);
        void Delete(int isdarcodi);
        InSustentoDetRelArchivoDTO GetById(int isdarcodi);
        List<InSustentoDetRelArchivoDTO> List();
        List<InSustentoDetRelArchivoDTO> GetByCriteria();
    }
}
