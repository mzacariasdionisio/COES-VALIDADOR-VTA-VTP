using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ABI_POTEFEC
    /// </summary>
    public interface IAbiPotefecRepository
    {
        int Save(AbiPotefecDTO entity);
        void Update(AbiPotefecDTO entity);
        void Delete(int pefeccodi);
        void DeleteByMes(DateTime fechaPeriodo);
        AbiPotefecDTO GetById(int pefeccodi);
        List<AbiPotefecDTO> List();
        List<AbiPotefecDTO> GetByCriteria();
        List<AbiPotefecDTO> ListaPorMes(DateTime fecIni, DateTime fecFin);
    }
}
