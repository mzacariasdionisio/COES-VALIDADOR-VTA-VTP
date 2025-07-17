using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_ROL
    /// </summary>
    public interface IFwRolRepository
    {
        int Save(FwRolDTO entity);
        void Update(FwRolDTO entity);
        void Delete(int rolcode);
        FwRolDTO GetById(int rolcode);
        List<FwRolDTO> List();
        List<FwRolDTO> GetByCriteria();
    }
}
