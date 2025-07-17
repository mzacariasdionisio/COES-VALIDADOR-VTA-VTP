using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_EMPRESARSF
    /// </summary>
    public interface IVcrEmpresarsfRepository
    {
        int Save(VcrEmpresarsfDTO entity);
        void Update(VcrEmpresarsfDTO entity);
        void Delete(int vcrecacodi);
        VcrEmpresarsfDTO GetById(int vcersfcodi);
        List<VcrEmpresarsfDTO> List(int vcrecacodi);
        List<VcrEmpresarsfDTO> GetByCriteria();
        VcrEmpresarsfDTO GetByIdTotalMes(int vcrecacodi, int emprcodi);
    }
}
