using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_RESPAGO
    /// </summary>
    public interface IStRespagoRepository
    {
        int Save(StRespagoDTO entity);
        void Update(StRespagoDTO entity);
        void Delete(int strecacodi);
        StRespagoDTO GetById(int respagcodi);
        List<StRespagoDTO> List();
        List<StRespagoDTO> GetByCriteria(int recacodi);
        List<StRespagoDTO> ListByStRespagoVersion(int strecacodi);
        List<StRespagoDTO> GetByCodElem(int strecacodi, int stcompcodi);
    }
}
