using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECNOTA
    /// </summary>
    public interface IFtFictecNotaRepository
    {
        int Save(FtFictecNotaDTO entity);
        void Update(FtFictecNotaDTO entity);
        void Delete(FtFictecNotaDTO entity);
        FtFictecNotaDTO GetById(int ftnotacodi);
        List<FtFictecNotaDTO> List();
        List<FtFictecNotaDTO> ListByFteqcodi(int fteqcodi);
        List<FtFictecNotaDTO> GetByCriteria();
    }
}
