using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECITEM_NOTA
    /// </summary>
    public interface IFtFictecItemNotaRepository
    {
        int Save(FtFictecItemNotaDTO entity);
        void Update(FtFictecItemNotaDTO entity);
        void Delete(int ftitntcodi);
        void DeleteByFtitcodi(int ftitcodi);
        FtFictecItemNotaDTO GetById(int ftitntcodi);
        List<FtFictecItemNotaDTO> List();
        List<FtFictecItemNotaDTO> GetByCriteria();
        List<FtFictecItemNotaDTO> ListByFteqcodi(int fteqcodi);
    }
}
