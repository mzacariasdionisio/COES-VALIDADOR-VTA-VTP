using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_MODULO
    /// </summary>
    public interface IFwModuloRepository
    {
        int Save(FwModuloDTO entity);
        void Update(FwModuloDTO entity);
        void Delete(int modcodi);
        FwModuloDTO GetById(int modcodi);
        List<FwModuloDTO> List();
        List<FwModuloDTO> GetByCriteria();
    }
}