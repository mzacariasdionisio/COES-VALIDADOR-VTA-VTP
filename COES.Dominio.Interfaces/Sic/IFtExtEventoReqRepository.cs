using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_EVENTO_REQ
    /// </summary>
    public interface IFtExtEventoReqRepository
    {
        int Save(FtExtEventoReqDTO entity);
        void Update(FtExtEventoReqDTO entity);
        void Delete(int fevrqcodi);
        FtExtEventoReqDTO GetById(int fevrqcodi);
        List<FtExtEventoReqDTO> List();
        List<FtExtEventoReqDTO> GetByCriteria();
    }
}
