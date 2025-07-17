using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENUREPORTE_HOJA
    /// </summary>
    public interface ISiMenureporteHojaRepository
    {
        int Save(SiMenureporteHojaDTO entity);
        void Update(SiMenureporteHojaDTO entity);
        void Delete(int mrephcodi);
        SiMenureporteHojaDTO GetById(int mrephcodi);
        List<SiMenureporteHojaDTO> List();
        List<SiMenureporteHojaDTO> GetByCriteria(int tmrepcodi);
    }
}
