using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_USER
    /// </summary>
    public interface IFwUserRepository
    {
        int Save(FwUserDTO entity);
        void Update(FwUserDTO entity);
        void Delete(int usercode);
        FwUserDTO GetById(int usercode);
        List<FwUserDTO> List();
        List<FwUserDTO> GetByCriteria();

        List<FwUserDTO> ObtenerCorreos();
    }
}
