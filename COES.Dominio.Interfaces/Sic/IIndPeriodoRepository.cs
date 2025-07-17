using COES.Dominio.DTO.Sic;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_PERIODO
    /// </summary>
    public interface IIndPeriodoRepository
    {
        int Save(IndPeriodoDTO entity);
        void Update(IndPeriodoDTO entity);
        void Delete(int ipericodi);
        IndPeriodoDTO GetById(int ipericodi);
        List<IndPeriodoDTO> List();
        List<IndPeriodoDTO> GetByCriteria(string horizonte, int anio);
    }
}
