using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENUREPORTE
    /// </summary>
    public interface ISiMenureporteRepository
    {
        int Save(SiMenureporteDTO entity);
        void Update(SiMenureporteDTO entity);
        void Delete(int repcodi);
        SiMenureporteDTO GetById(int repcodi);
        List<SiMenureporteDTO> List();
        List<SiMenureporteDTO> GetByCriteria();
        List<SiMenureporteDTO> GetListaAdmReporte(int tmrepcodi);

        #region siosein2
        SiMenureporteDTO GetSimenureportebyIndex(string mrepabrev, int tmrepcodi);
        #endregion
    }
}
