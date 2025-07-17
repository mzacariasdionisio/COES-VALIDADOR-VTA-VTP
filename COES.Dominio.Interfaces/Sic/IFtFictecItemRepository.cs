using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECITEM
    /// </summary>
    public interface IFtFictecItemRepository
    {
        int Save(FtFictecItemDTO entity);
        void Update(FtFictecItemDTO entity);
        void Delete(FtFictecItemDTO entity);
        FtFictecItemDTO GetById(int ftitcodi);
        List<FtFictecItemDTO> List();
        List<FtFictecItemDTO> GetByCriteria();
        List<FtFictecItemDTO> ListarPorIds(string ftitcodis);        
        List<FtFictecItemDTO> ListarItemsByFichaTecnica(int fteqcodi);
    }
}
