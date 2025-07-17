using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_REDUCCPAGOEJE
    /// </summary>
    public interface IVcrReduccpagoejeRepository
    {
        int Save(VcrReduccpagoejeDTO entity);
        void Update(VcrReduccpagoejeDTO entity);
        void Delete(int vcrecacodi);
        VcrReduccpagoejeDTO GetById(int vcrecacodi, int equicodi);
        List<VcrReduccpagoejeDTO> List(int vcrecacodi);
        List<VcrReduccpagoejeDTO> GetByCriteria();
    }
}
