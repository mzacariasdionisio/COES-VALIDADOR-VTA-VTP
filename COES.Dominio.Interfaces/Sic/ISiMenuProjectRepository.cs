using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENU_PROJECT
    /// </summary>
    public interface ISiMenuProjectRepository
    {
        int Save(SiMenuProjectDTO entity);
        void Update(SiMenuProjectDTO entity);
        void Delete(int mprojcodi);
        SiMenuProjectDTO GetById(int mprojcodi);
        List<SiMenuProjectDTO> List();
        List<SiMenuProjectDTO> GetByCriteria();
    }
}
