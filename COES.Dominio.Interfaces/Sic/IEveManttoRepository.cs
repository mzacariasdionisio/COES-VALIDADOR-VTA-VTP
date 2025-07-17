using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

// Agregado por ASSETEC
using System.Data;
using System.Data.Common;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_MANTTO
    /// </summary>
    public interface IEveManttoRepository
    {
        #region ASSETEC - CAMBIOS 05/07/2018
        IDbConnection BeginConnection();
        DbTransaction StartTransaction(IDbConnection conn);
        #endregion

        int GetMaxId();
        void Save(EveManttoDTO entity);
        void Update(EveManttoDTO entity);
        void Delete(int manttocodi);
        EveManttoDTO GetById(int manttocodi);
        List<EveManttoDTO> List();
        List<EveManttoDTO> GetByCriteria();
        List<EveManttoDTO> BuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas);
        int ObtenerNroRegistros(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);
        List<ReporteManttoDTO> ObtenerTotalManttoEmpresa(DateTime fechaInicio, DateTime fechaFin,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);
        List<EveManttoDTO> ObtenerReporteMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto);
        List<EveManttoDTO> ObtenerManttoEquipo(string idsEquipo, int evenClaseCodi, DateTime fechaInicio, DateTime fechaFin);
        List<EveManttoDTO> ObtenerManttoEquipoClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase);
        List<EveManttoDTO> ObtenerManttoEquipoSubcausaClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase, int subcausaCodi);
        List<EveManttoDTO> ObtenerManttoEquipoPadreClaseFecha(string idsEquipo, string fechaInicio, string fechaFin, int evenClase);
        List<EveManttoDTO> ObtenerMantenimientosProgramados(DateTime fechaInicio, DateTime fechaFin);

        List<EveManttoDTO> ObtenerMantenimientosProgramadosMovil(DateTime fechaInicio, DateTime fechaFin, int tipo);

        #region PR5
        List<EveManttoDTO> GetByFechaIni(DateTime fechaInicial, DateTime fechaFinal, string evenclasecodi, string famcodi);
        List<EveManttoDTO> GetIndispUniGeneracion(string idsTipoMantenimiento, string indispo,
          string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region INDISPONIBILIDADES
        EveManttoDTO GetById2(int manttocodi);
        #endregion

        #region SIOSEIN
        List<EveManttoDTO> GetListaHechosRelevantes(DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<EveManttoDTO> ListaManttosDigsilent(string evenclasecodi, DateTime fecha);
        List<EveManttoDTO> ListaMantenimientos25(int evenclasecodi, string evenclasedesc, DateTime fechaini, DateTime fechafin);
        #endregion

        #region INTERVENCIONES
        void Save(EveManttoDTO entity, IDbConnection conn, DbTransaction tran);
        int DeleteByPrograma(IDbConnection conn, DbTransaction tran, int evenclasecodi, DateTime fechaIni, DateTime fechaFin);
        int DeleteByIntercodi(IDbConnection conn, DbTransaction tran, int interCodi);
        #endregion

        #region SIOSEIN2
        List<EveManttoDTO> ObtenerManttoPorEquipoClaseFamilia(string evenclasecodi, string famcodi, DateTime eveniniInicio, DateTime eveiniFin);
        List<EveManttoDTO> ObtenerManttoEjecutadoProgramado(string evenclasecodi, string evenindispo, string tareacodi, string famcodi, string emprecodi, DateTime fechaInicio, DateTime fechaFin);
        #endregion

        #region Numerales Datos Base
        List<EveManttoDTO> ListaNumerales_DatosBase_5_6_1(string fechaIni, string fechaFin);
        List<EveManttoDTO> ListaNumerales_DatosBase_5_6_7(string fechaIni, string fechaFin);
        List<EveManttoDTO> ListaNumerales_DatosBase_5_6_8(string fechaIni, string fechaFin);
        List<EveManttoDTO> ListaNumerales_DatosBase_5_6_9(string fechaIni, string fechaFin);
        List<EveManttoDTO> ListaNumerales_DatosBase_5_6_10(string fechaIni, string fechaFin);


        #endregion

    }
}
