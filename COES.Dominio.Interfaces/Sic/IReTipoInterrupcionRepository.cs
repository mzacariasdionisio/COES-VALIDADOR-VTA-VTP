using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_TIPO_INTERRUPCION
    /// </summary>
    public interface IReTipoInterrupcionRepository
    {
        int Save(ReTipoInterrupcionDTO entity);
        void Update(ReTipoInterrupcionDTO entity);
        void Delete(int retintcodi);
        ReTipoInterrupcionDTO GetById(int retintcodi);
        List<ReTipoInterrupcionDTO> List();
        List<ReTipoInterrupcionDTO> GetByCriteria();

        List<ReTipoInterrupcionDTO> ObtenerConfiguracion();
    }
}
