using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_PROVISIONBASE
    /// </summary>
    public interface IVcrProvisionbaseRepository
    {
        int Save(VcrProvisionbaseDTO entity);
        void Update(VcrProvisionbaseDTO entity);
        void Delete(int vcrpbcodi);
        VcrProvisionbaseDTO GetById(int vcrpbcodi);
        List<VcrProvisionbaseDTO> List();
        List<VcrProvisionbaseDTO> GetByCriteria();
        VcrProvisionbaseDTO GetByIdURS(int grupocodi, string periodo);

        List<VcrProvisionbaseDTO> ListIndex();

        VcrProvisionbaseDTO GetByIdView(int vcrpbcodi);
    }
}
