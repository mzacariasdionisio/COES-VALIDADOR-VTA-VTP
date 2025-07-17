using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PSU_DESVCMGSNC
    /// </summary>
    public interface IPsuDesvcmgsncRepository
    {
        void Save(PsuDesvcmgsncDTO entity);
        void Update(PsuDesvcmgsncDTO entity);
        void Delete(DateTime desvfecha);
        PsuDesvcmgsncDTO GetById(DateTime desvfecha);
        List<PsuDesvcmgsncDTO> List();
        List<PsuDesvcmgsncDTO> GetByCriteria(DateTime fechaMes);
    }
}
