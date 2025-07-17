using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_FICTECPROP
    /// </summary>
    public interface IFtFictecPropRepository
    {
        int Save(FtFictecPropDTO entity);
        void Update(FtFictecPropDTO entity);
        void Delete(int ftpropcodi);
        FtFictecPropDTO GetById(int ftpropcodi);
        List<FtFictecPropDTO> List();
        List<FtFictecPropDTO> GetByCriteria();
    }
}
