using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_COSTOPORTUNIDAD
    /// </summary>
    public interface IVcrCostoportunidadRepository
    {
        int Save(VcrCostoportunidadDTO entity);
        void Update(VcrCostoportunidadDTO entity);
        void Delete(int vcrecacodi);
        VcrCostoportunidadDTO GetById(int vcrcopcodi);
        List<VcrCostoportunidadDTO> List();
        List<VcrCostoportunidadDTO> GetByCriteria();
        VcrCostoportunidadDTO GetByIdEmpresa(int vcrecacodi, int emprcodi);
    }
}
