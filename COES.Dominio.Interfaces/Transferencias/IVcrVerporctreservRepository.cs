using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERINCUMPLIM
    /// </summary>
    public interface IVcrVerporctreservRepository
    {
        int Save(VcrVerporctreservDTO entity);
        void Update(VcrVerporctreservDTO entity);
        void Delete(int vcrvprcodi);
        VcrVerporctreservDTO GetById(int vcrvprcodi, DateTime vcrvprfecha);
        List<VcrVerporctreservDTO> List(int vcrvprcodi, int equicodiuni, int equicodicen);
        List<VcrVerporctreservDTO> GetByCriteria(int vcrvprcodi);
        VcrVerporctreservDTO GetByIdPorUnidad(int vcrvprcodi, int equicodiuni, int equicodicen);
    }
}
