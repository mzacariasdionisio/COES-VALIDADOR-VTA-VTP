using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECDET
    /// </summary>
    public interface IFtFictecDetRepository
    {
        int Save(FtFictecDetDTO entity);
        void Update(FtFictecDetDTO entity);
        void Delete(int ftecdcodi);
        void DeleteByFteccodi(int fteccodi);
        FtFictecDetDTO GetById(int ftecdcodi);
        List<FtFictecDetDTO> List();
        List<FtFictecDetDTO> GetByCriteria();
    }
}
