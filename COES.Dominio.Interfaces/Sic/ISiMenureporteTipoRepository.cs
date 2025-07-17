using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENUREPORTE_TIPO
    /// </summary>
    public interface ISiMenureporteTipoRepository
    {
        int Save(SiMenureporteTipoDTO entity);
        void Update(SiMenureporteTipoDTO entity);
        void Delete(int mreptipcodi);
        SiMenureporteTipoDTO GetById(int mreptipcodi);
        List<SiMenureporteTipoDTO> List();
        List<SiMenureporteTipoDTO> GetByCriteria(int mprojcodi);
    }
}
