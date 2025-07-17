using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_POTLIM
    /// </summary>
    public interface IIndPotlimRepository
    {
        int Save(IndPotlimDTO entity);
        void Update(IndPotlimDTO entity);
        void Delete(int potlimcodi);
        IndPotlimDTO GetById(int potlimcodi);
        List<IndPotlimDTO> List();
        List<IndPotlimDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin);
        int Save(IndPotlimDTO indPotlim, IDbConnection connection, IDbTransaction transaction);
        void UpdatePartial(IndPotlimDTO entity);
        void UpdateEstado(IndPotlimDTO entity);
    }
}
