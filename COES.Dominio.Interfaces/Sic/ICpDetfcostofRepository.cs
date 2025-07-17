using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_DETFCOSTOF
    /// </summary>
    public interface ICpDetfcostofRepository
    {
        void Save(CpDetfcostofDTO entity);
        void Delete(int topcodi);
        List<CpDetfcostofDTO> GetByCriteria(int topcodi);
        void CrearCopia(int topcodi1, int topcodi2);
    }
}
