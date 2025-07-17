using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_SERVICIORSF
    /// </summary>
    public interface IVcrServiciorsfRepository
    {
        int Save(VcrServiciorsfDTO entity);
        void Update(VcrServiciorsfDTO entity);
        void Delete(int vcrecacodi);
        VcrServiciorsfDTO GetById(int vcsrsfcodi);
        List<VcrServiciorsfDTO> List(int vcrecacodi);
        List<VcrServiciorsfDTO> GetByCriteria();
        VcrServiciorsfDTO GetByIdValoresDia(int vcrecacodi, DateTime vcsrsffecha);
    }
}
