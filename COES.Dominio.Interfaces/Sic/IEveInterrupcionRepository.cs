using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_INTERRUPCION
    /// </summary>
    public interface IEveInterrupcionRepository
    {
        int Save(EveInterrupcionDTO entity);
        void Update(EveInterrupcionDTO entity);
        void Delete(int interrupcodi);
        EveInterrupcionDTO GetById(int interrupcodi);
        List<EveInterrupcionDTO> List();
        List<EveInterrupcionDTO> GetByCriteria(string idEvento);

        #region MigracionSGOCOES-GrupoB
        List<EveInterrupcionDTO> ListaCalidadSuministro(DateTime fecIni);
        #endregion
    }
}
