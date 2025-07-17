using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_FCOSTOF
    /// </summary>
    public interface ICpFcostofRepository
    {
        void Save(CpFcostofDTO entity);
        void Update(CpFcostofDTO entity);
        void Delete(int topcodi);
        CpFcostofDTO GetById(int topcodi);
        List<CpFcostofDTO> List();
        List<CpFcostofDTO> GetByCriteria();
        void CrearCopia(int topcodi1, int topcodi2);
    }
}
