using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EQ_TIPOAREA
    /// </summary>
    public interface IEqTipoareaRepository
    {
        int Save(EqTipoareaDTO entity);
        void Update(EqTipoareaDTO entity);
        void Delete(int tareacodi);
        EqTipoareaDTO GetById(int tareacodi);
        List<EqTipoareaDTO> List();
        List<EqTipoareaDTO> GetByCriteria();

        #region GESPROTEC 01/10/2025
        List<EqTipoareaDTO> ListProtecciones();
        #endregion

    }

}
