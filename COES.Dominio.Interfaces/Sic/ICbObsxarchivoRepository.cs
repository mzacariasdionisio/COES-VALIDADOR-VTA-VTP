using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CB_OBSXARCHIVO
    /// </summary>
    public interface ICbObsxarchivoRepository
    {
        int GetMaxId();
        int Save(CbObsxarchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(CbObsxarchivoDTO entity);
        void Delete(int cbobsacodi);
        CbObsxarchivoDTO GetById(int cbobsacodi);
        List<CbObsxarchivoDTO> List(string cbcentcodis);
        List<CbObsxarchivoDTO> ListByCbvercodi(int cbvercodi);
        List<CbObsxarchivoDTO> GetByCriteria();
    }
}
