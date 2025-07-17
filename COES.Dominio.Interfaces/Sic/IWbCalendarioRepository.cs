using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_CALENDARIO
    /// </summary>
    public interface IWbCalendarioRepository
    {
        int Save(WbCalendarioDTO entity);
        void Update(WbCalendarioDTO entity);
        void Delete(int calendcodi);
        WbCalendarioDTO GetById(int calendcodi);
        List<WbCalendarioDTO> List();        
        List<WbCalendarioDTO> GetByCriteria(string nombre, DateTime fechaInicio, DateTime fechaFin);
    }
}
