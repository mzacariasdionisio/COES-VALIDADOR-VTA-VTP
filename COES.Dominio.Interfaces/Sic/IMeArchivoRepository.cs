using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ARCHIVO
    /// </summary>
    public interface IMeArchivoRepository
    {
        int Save(MeArchivoDTO entity);
        void Update(MeArchivoDTO entity);
        void Delete();
        MeArchivoDTO GetById();
        List<MeArchivoDTO> List();
        List<MeArchivoDTO> GetByCriteria();
    }
}
