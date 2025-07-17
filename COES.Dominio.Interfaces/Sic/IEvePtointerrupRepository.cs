using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_PTOINTERRUP
    /// </summary>
    public interface IEvePtointerrupRepository
    {
        int Save(EvePtointerrupDTO entity);
        void Update(EvePtointerrupDTO entity);
        void Delete(int ptointerrcodi);
        EvePtointerrupDTO GetById(int ptointerrcodi);
        List<EvePtointerrupDTO> List();
        List<EvePtointerrupDTO> GetByCriteria();

    }
}
