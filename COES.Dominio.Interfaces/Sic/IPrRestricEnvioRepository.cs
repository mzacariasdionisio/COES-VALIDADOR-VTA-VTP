using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_RESTRIC_ENVIO
    /// </summary>
    public interface IPrRestricEnvioRepository
    {
        int Save(PrRestricEnvioDTO entity);
        void Update(PrRestricEnvioDTO entity);
        void Delete(int resenvcodi);
        PrRestricEnvioDTO GetById(int resenvcodi);
        List<PrRestricEnvioDTO> List();
        List<PrRestricEnvioDTO> GetByCriteria();
        List<PrRestricEnvioDTO> ObtenerEnvioActivo(DateTime fechaPeriodo, string codigos, int formato, string estado);
    }
}
