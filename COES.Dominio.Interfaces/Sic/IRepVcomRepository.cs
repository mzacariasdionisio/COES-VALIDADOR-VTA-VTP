using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla REP_VCOM
    /// </summary>
    public interface IRepVcomRepository
    {
        void Save(RepVcomDTO entity);
        void Update(RepVcomDTO entity);
        void Delete(int periodo);
        RepVcomDTO GetById(int periodo, string codigomodooperacion, string codigotipocombustible);
        List<RepVcomDTO> List();
        List<RepVcomDTO> GetByCriteria();
    }
}
