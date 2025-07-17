using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ETAPA
    /// </summary>
    public interface IFtExtEtapaRepository
    {
        int Save(FtExtEtapaDTO entity);
        void Update(FtExtEtapaDTO entity);
        void Delete(int ftetcodi);
        FtExtEtapaDTO GetById(int ftetcodi);
        List<FtExtEtapaDTO> List();
        List<FtExtEtapaDTO> GetByCriteria();
    }
}
