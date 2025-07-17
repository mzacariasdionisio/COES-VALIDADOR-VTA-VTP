using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_MENUREPORTEDET
    /// </summary>
    public interface ISiMenureportedetRepository
    {
        int Save(SiMenureportedetDTO entity);
        void Update(SiMenureportedetDTO entity);
        void Delete(int mrepdcodigo);
        SiMenureportedetDTO GetById(int mrepdcodigo);
        List<SiMenureportedetDTO> List();
        List<SiMenureportedetDTO> GetByCriteria(int tmrepcodi);
    }
}
