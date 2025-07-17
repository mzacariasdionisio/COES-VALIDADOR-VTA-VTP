using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla AUD_ARCHIVO
    /// </summary>
    public interface IAudArchivoRepository
    {
        int Save(AudArchivoDTO entity);
        void Update(AudArchivoDTO entity);
        void Delete(int archcodi);
        AudArchivoDTO GetById(int archcodi);
        List<AudArchivoDTO> List();
        List<AudArchivoDTO> GetByCriteria();
    }
}
