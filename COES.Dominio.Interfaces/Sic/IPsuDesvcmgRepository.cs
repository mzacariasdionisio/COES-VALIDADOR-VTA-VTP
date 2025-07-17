using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PSU_DESVCMG
    /// </summary>
    public interface IPsuDesvcmgRepository
    {
        void Save(PsuDesvcmgDTO entity);
        void Update(PsuDesvcmgDTO entity);
        void Delete(DateTime desvfecha);
        PsuDesvcmgDTO GetById(DateTime desvfecha);
        List<PsuDesvcmgDTO> List();
        List<PsuDesvcmgDTO> GetByCriteria();
    }
}
