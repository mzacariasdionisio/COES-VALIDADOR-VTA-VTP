using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TRN_MEDBORNE
    /// </summary>
    public interface ITrnMedborneRepository
    {
        int Save(TrnMedborneDTO entity);
        void Update(TrnMedborneDTO entity);
        void Delete(int pericodi, int version);
        TrnMedborneDTO GetById(int trnmebcodi);
        List<TrnMedborneDTO> List();
        List<TrnMedborneDTO> GetByCriteria();
    }
}
