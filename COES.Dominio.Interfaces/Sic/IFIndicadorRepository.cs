using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla F_INDICADOR
    /// </summary>
    public interface IFIndicadorRepository
    {
        void Update(FIndicadorDTO entity);
        void Delete();
        FIndicadorDTO GetById();
        List<FIndicadorDTO> List();
        List<FIndicadorDTO> GetByCriteria();
        DataTable Get_cadena_transgresion(DateTime dtFecha, int li_gpscodi, string ls_indiccodi);
        int Get_fallaacumulada(DateTime dtFecha,int li_gpscodi, string ls_indiccodi);
        DataTable Get_cadena_transgresionFrec(DateTime dtFecha, int li_gpscodi, string ls_indiccodi);
        int Get_fallaacumuladaFrec(DateTime dtFecha, int li_gpscodi, string ls_indiccodi);

        #region PR5
        List<FIndicadorDTO> ListarReporteVariacionesFrecuenciaSEIN(string empresas, string gps, DateTime dtFechaIni, DateTime dtFechaFin);
        List<FIndicadorDTO> ListarTransgresionXRango(DateTime fechaIni, DateTime fechaFin, int gpscodi, string indiccodi);
        int GetFallaAcumuladaXRango(DateTime fechaIni, DateTime fechaFin, int gpscodi, string indiccodi);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<FIndicadorDTO> ListaIndicador(DateTime fecIni, string indiccodi);
        List<FIndicadorDTO> ListaIndicadorAcu(DateTime fecIni, string indiccodi);
        #endregion
    }
}
