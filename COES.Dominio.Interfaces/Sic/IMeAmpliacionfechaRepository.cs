using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_AMPLIACIONFECHA
    /// </summary>
    public interface IMeAmpliacionfechaRepository
    {
        void Save(MeAmpliacionfechaDTO entity);
        void Update(MeAmpliacionfechaDTO entity);
        void Delete(int amplicodi);
        MeAmpliacionfechaDTO GetById(DateTime fecha, int empresa, int formato);
        List<MeAmpliacionfechaDTO> List();
        List<MeAmpliacionfechaDTO> GetByCriteria(string formato, string empresa, DateTime fechaIni);
        List<MeAmpliacionfechaDTO> GetListaAmpliacion(DateTime fechaIni, DateTime fechaFin, int empresa, int formato);
        List<MeAmpliacionfechaDTO> GetListaMultiple(DateTime fechaIni, DateTime fechaFin, string sEmpresa, string sFormato);

        void UpdateById(MeAmpliacionfechaDTO entity);
        
        #region PR5
        List<MeAmpliacionfechaDTO> GetAmpliacionNow(int EmprCodi, string fecha);
        #endregion


        List<SiEmpresaDTO> ListaEmpresasAmpliacionPlazo();

        List<MeAmpliacionfechaDTO> GetListaAmpliacionFiltro(DateTime periodo, int empresa, int formato, int regIni, int regFin);

        int GetListaAmpliacionFiltroCount(DateTime periodo, int empresa, int formato);
    }
}
