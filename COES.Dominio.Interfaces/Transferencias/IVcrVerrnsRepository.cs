using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERRNS
    /// </summary>
    public interface IVcrVerrnsRepository
    {
        int Save(VcrVerrnsDTO entity);
        void Update(VcrVerrnsDTO entity);
        void Delete(int vcrdsrcodi, int vcvrnstipocarga);
        VcrVerrnsDTO GetById(int vcvrnscodi);
        List<VcrVerrnsDTO> List();
        List<VcrVerrnsDTO> GetByCriteria(int vcrdsrcodi, int vcvrnstipocarga);
        List<VcrVerrnsDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcvrnsfecha, int vcvrnstipocarga);
    }
}
