using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_IEODCUADRO
    /// </summary>
    public interface IEveIeodcuadroRepository
    {
        int Save(EveIeodcuadroDTO entity);
        void Update(EveIeodcuadroDTO entity);
        void Delete(int iccodi);
        EveIeodcuadroDTO GetById(int iccodi);
        List<EveIeodcuadroDTO> List();
        List<EveIeodcuadroDTO> GetByCriteria();
        List<EveIeodcuadroDTO> BuscarOperaciones(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        List<EveIeodcuadroDTO> BuscarOperacionesDetallado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        List<EveIeodcuadroDTO> BuscarOperacionesSinPaginado(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, string idsEmpresa, string idsTipoEquipo);
        int ObtenerNroFilas(int evenClase, int subCausacodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize);
        EveIeodcuadroDTO ObtenerIeodcuadro(int iccodi);
        EveIeodcuadroDTO ObtenerDatosEquipo(int idEquipo);
        List<EveIeodcuadroDTO> ListarEveIeodCuadroxEmpresa(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, int emprcodi);
        List<EveIeodcuadroDTO> GetCriteriaxPKCodis(string pkCodis);
        void BorradoLogico(int iccodi);

        #region PR5
        List<EveIeodcuadroDTO> ListarEveIeodCuadroxEmpresaxEquipos(DateTime fechaInicio, DateTime FechaFin, int subCausaCodi, string emprcodi, string equipos, int nPagina, int nRegistros);
        int ObtenerNroElementosConsultaRestricciones(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos);
        List<EveIeodcuadroDTO> ListarReporteOperacionVaria(DateTime fechaInicio, DateTime FechaFin, string clase, string subCausaCodi);
        int ContarEveIeodCuadroxEmpresaxEquipos(DateTime fechaIni, DateTime fechaFin, int subcausaCodi, string emprcodi, string equipos, string areacodi);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<EveIeodcuadroDTO> ListaBitacora(DateTime fecIni, string subcausacodi, string famcodi);
        List<EveIeodcuadroDTO> ListaReqPropios(DateTime fecIni, DateTime fecFin);
        #endregion
    }
}
