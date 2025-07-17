using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public interface IEpoEstudioTerceroInvEoRepository
    {
        int Save(EpoEstudioTerceroInvEoDTO entity);
        void Delete(int esteocodi);
        List<EpoEstudioTerceroInvEoDTO> GetByCriteria(int esteocodi);
    }
}
