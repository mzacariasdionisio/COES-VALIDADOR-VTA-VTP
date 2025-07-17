using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_REGISTRO_MODPLAN
    /// </summary>
    public interface IWbRegistroModplanRepository
    {
        int Save(WbRegistroModplanDTO entity);
        void Update(WbRegistroModplanDTO entity);
        void Delete(int regmodcodi);
        WbRegistroModplanDTO GetById(int regmodcodi);
        List<WbRegistroModplanDTO> List();
        List<WbRegistroModplanDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        List<WbRegistroModplanDTO> ObtenerReporte(int idVersion, int tipo);
    }
}
