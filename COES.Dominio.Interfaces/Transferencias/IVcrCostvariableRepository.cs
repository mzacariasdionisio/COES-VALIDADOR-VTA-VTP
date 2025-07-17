using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_COSTVARIABLE
    /// </summary>
    public interface IVcrCostvariableRepository
    {
        int Save(VcrCostvariableDTO entity);
        void Update(VcrCostvariableDTO entity);
        void Delete(int vcvarcodi);
        VcrCostvariableDTO GetById(int vcvarcodi);
        List<VcrCostvariableDTO> List(int vcrecacodi, int grupocodi, int equicodi, DateTime vcvarfecha);
        List<VcrCostvariableDTO> GetByCriteria(int vcrecacodi);
    }
}
