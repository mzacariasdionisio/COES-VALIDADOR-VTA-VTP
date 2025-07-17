using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_CONVOCATORIAS
    /// </summary>
    public interface IWbConvocatoriasRepository
    {
        int Save(WbConvocatoriasDTO entity);
        void Update(WbConvocatoriasDTO entity);
        void Delete(int convodi);
        WbConvocatoriasDTO GetById(int convcodi);
        List<WbConvocatoriasDTO> List();
        List<WbConvocatoriasDTO> GetByCriteria();
    }
}
