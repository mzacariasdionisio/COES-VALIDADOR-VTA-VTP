using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_DATO
    /// </summary>
    public interface IPrRestricDatoRepository
    {
        int Save(PrRestricDatoDTO entity);
        void Update(PrRestricDatoDTO entity);
        void Delete(int resdatcodi);
        PrRestricDatoDTO GetById(int resdatcodi);
        List<PrRestricDatoDTO> List();
        List<PrRestricDatoDTO> GetByCriteria();
        List<PrRestricDatoDTO> ObtenerDataEnvio(DateTime fechaPeriodo, string codigos, int formato);
        List<PrRestricDatoDTO> ObtenerPorEnvio(int idEnvio);
    }
}
