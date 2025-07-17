using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_CMPENSOPER
    /// </summary>
    public interface IVcrCmpensoperRepository
    {
        int Save(VcrCmpensoperDTO entity);
        void Update(VcrCmpensoperDTO entity);
        void Delete(int vcrecacodi);
        VcrCmpensoperDTO GetById(int vcmpopcodi);
        List<VcrCmpensoperDTO> List(int vcrrecacodi);
        List<VcrCmpensoperDTO> GetByCriteria();
    }
}
