using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_RELACION_RPT
    /// </summary>
    public interface IIndRelacionRptRepository
    {
        int GetMaxId();
        int Save(IndRelacionRptDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(IndRelacionRptDTO entity);
        void Delete(int irelrpcodi);
        IndRelacionRptDTO GetById(int irelrpcodi);
        List<IndRelacionRptDTO> List();
        List<IndRelacionRptDTO> GetByCriteria(int irptprinc);
    }
}
