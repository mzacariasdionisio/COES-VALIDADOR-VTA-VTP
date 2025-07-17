using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data.Common;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_INTERVENCION_REL_ARCHIVO
    /// </summary>
    public interface IInIntervencionRelArchivoRepository
    {
        int GetMaxId();
        int Save(InIntervencionRelArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(InIntervencionRelArchivoDTO entity);
        void Delete(int irarchcodi);
        InIntervencionRelArchivoDTO GetById(int irarchcodi);
        List<InIntervencionRelArchivoDTO> List();
        List<InIntervencionRelArchivoDTO> GetByCriteria();
    }
}
