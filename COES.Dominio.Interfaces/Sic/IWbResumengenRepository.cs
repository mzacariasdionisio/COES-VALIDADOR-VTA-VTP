using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_RESUMENGEN
    /// </summary>
    public interface IWbResumengenRepository
    {
        int Save(WbResumengenDTO entity);
        void Update(WbResumengenDTO entity);
        void Delete(int resgencodi);
        WbResumengenDTO GetById(int resgencodi);
        List<WbResumengenDTO> List();
        WbResumengenDTO GetByCriteria(DateTime fecha);
    }
}
