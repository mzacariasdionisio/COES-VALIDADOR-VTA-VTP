using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_CONFIGURACION_URS
    /// </summary>
    public interface ICoConfiguracionUrsRepository
    {
        int Save(CoConfiguracionUrsDTO entity);
        void Update(CoConfiguracionUrsDTO entity);
        void Delete(int conurscodi);
        CoConfiguracionUrsDTO GetById(int idPeriodo, int idVersion, int idUrs);
        List<CoConfiguracionUrsDTO> List();
        List<CoConfiguracionUrsDTO> GetByCriteria();
        List<CoConfiguracionUrsDTO> GetPorVersion(int idVersion);



    }
}
