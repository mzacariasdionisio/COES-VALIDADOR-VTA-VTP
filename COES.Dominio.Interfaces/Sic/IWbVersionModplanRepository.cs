using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_VERSION_MODPLAN
    /// </summary>
    public interface IWbVersionModplanRepository
    {
        int Save(WbVersionModplanDTO entity);
        void Update(WbVersionModplanDTO entity);
        void Delete(int vermplcodi);
        WbVersionModplanDTO GetById(int vermplcodi);
        List<WbVersionModplanDTO> List(int tipo);
        List<WbVersionModplanDTO> GetByCriteria();
        List<WbVersionModplanDTO> ObtenerVersionPorPadre(int idPadre, int tipo);
    }
}
