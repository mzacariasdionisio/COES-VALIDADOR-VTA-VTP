using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECXTIPOEQUIPO
    /// </summary>
    public interface IFtFictecXTipoEquipoRepository
    {
        int Save(FtFictecXTipoEquipoDTO entity);
        void Update(FtFictecXTipoEquipoDTO entity);
        void Delete(FtFictecXTipoEquipoDTO entity);
        FtFictecXTipoEquipoDTO GetById(int fteqcodi);
        List<FtFictecXTipoEquipoDTO> List();
        List<FtFictecXTipoEquipoDTO> GetByCriteria(string estado);
        List<FtFictecXTipoEquipoDTO> ListByFteccodi(int fteccodi);
        List<FtFictecXTipoEquipoDTO> ListAllByFteccodi(int fteccodi);
        List<FtFictecXTipoEquipoDTO> ListByFteqpadre(int fteqpadre);
    }
}
