using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_EVENTO
    /// </summary>
    public interface IFtExtEventoRepository
    {
        int Save(FtExtEventoDTO entity);
        void Update(FtExtEventoDTO entity);
        void Delete(int ftevcodi);
        FtExtEventoDTO GetById(int ftevcodi);
        List<FtExtEventoDTO> List();
        List<FtExtEventoDTO> GetByCriteria();
    }
}
