using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PSU_RPFHID
    /// </summary>
    public interface IPsuRpfhidRepository
    {
        void Save(PsuRpfhidDTO entity);
        void Update(PsuRpfhidDTO entity);
        void Delete(DateTime rpfhidfecha);
        PsuRpfhidDTO GetById(DateTime rpfhidfecha);
        List<PsuRpfhidDTO> List();
        List<PsuRpfhidDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
    }
}
