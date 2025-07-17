using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTEC_VISUALENTIDAD
    /// </summary>
    public interface IFtFictecVisualentidadRepository
    {
        int Save(FtFictecVisualentidadDTO entity);
        void Update(FtFictecVisualentidadDTO entity);
        void Delete(int ftvercodi);
        FtFictecVisualentidadDTO GetById(int ftvercodi);
        List<FtFictecVisualentidadDTO> List();
        List<FtFictecVisualentidadDTO> GetByCriteria();
    }
}
