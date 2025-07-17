using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERINCUMPLIM
    /// </summary>
    public interface IVcrVerincumplimRepository
    {
        int Save(VcrVerincumplimDTO entity);
        void Update(VcrVerincumplimDTO entity);
        void Delete(int vcrinccodi);
        VcrVerincumplimDTO GetById(int vcrinccodi, DateTime vcrvinfecha);
        List<VcrVerincumplimDTO> List(int vcrinccodi, int equicodiuni, int equicodicen);
        List<VcrVerincumplimDTO> GetByCriteria(int vcrinccodi);
        VcrVerincumplimDTO GetByIdPorUnidad(int vcrinccodi, int equicodiuni, int equicodicen);
    }
}
