using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_INFOADICIONAL
    /// </summary>
    public interface ITrnInfoadicionalRepository
    {
        int Save(TrnInfoadicionalDTO entity);
        void Update(TrnInfoadicionalDTO entity);
        void Delete(int infadicodi);
        TrnInfoadicionalDTO GetById(int infadicodi);
        List<TrnInfoadicionalDTO> List();
        List<TrnInfoadicionalDTO> ListVersiones(int infadicodi);
        List<TrnInfoadicionalDTO> GetByCriteria();
    }
}
