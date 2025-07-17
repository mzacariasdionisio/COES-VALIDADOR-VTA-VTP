using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPO_ESTUDIO_EO
    /// </summary>
    public interface IEpoEstudioTerceroInvEpoRepository
    {
        int Save(EpoEstudioTerceroInvEpoDTO entity);
        void Delete(int estepocodi);
        List<EpoEstudioTerceroInvEpoDTO> GetByCriteria(int estepocodi);
    }
}
