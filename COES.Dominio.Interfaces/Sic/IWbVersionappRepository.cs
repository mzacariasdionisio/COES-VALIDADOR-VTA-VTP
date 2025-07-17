using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_VERSIONAPP
    /// </summary>
    public interface IWbVersionappRepository
    {
        int Save(WbVersionappDTO entity);
        void Update(WbVersionappDTO entity);
        void Delete(int verappcodi);
        WbVersionappDTO GetById(int verappcodi);
        List<WbVersionappDTO> List();
        List<WbVersionappDTO> GetByCriteria();
        WbVersionappDTO ObtenerVersionActual();
    }
}
