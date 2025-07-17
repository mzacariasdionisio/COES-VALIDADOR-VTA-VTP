using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_CORREO_ARCHIVO
    /// </summary>
    public interface ISiCorreoArchivoRepository
    {
        int GetMaxId();
        int Save(SiCorreoArchivoDTO entity);
        int Save(SiCorreoArchivoDTO entity, IDbConnection conn, DbTransaction tran);
        void Update(SiCorreoArchivoDTO entity);
        void Delete(int earchcodi);
        SiCorreoArchivoDTO GetById(int earchcodi);
        List<SiCorreoArchivoDTO> List();
        List<SiCorreoArchivoDTO> GetByCriteria();
        List<SiCorreoArchivoDTO> GetByCorreos(string corrcodis);
    }
}
