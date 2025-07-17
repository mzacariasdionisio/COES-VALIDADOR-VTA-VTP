using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_HORAOPERACION
    /// </summary>
    public interface IEveHoraoperacionRepository
    {
        int Save(EveHoraoperacionDTO entity);
        void Update(EveHoraoperacionDTO entity);
        void Delete(int hopcodi);
        EveHoraoperacionDTO GetById(int hopcodi);
        List<EveHoraoperacionDTO> List();
        List<EveHoraoperacionDTO> GetByCriteria(DateTime fecha);
        List<EveHoraoperacionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
        List<EveHoraoperacionDTO> GetByDetalleHO(DateTime fecha);
        List<EveHoraoperacionDTO> GetByCriteriaXEmpresaxFecha(int emprcodi, DateTime fecha, DateTime fechafin, int idCentral);
        List<EveHoraoperacionDTO> GetByCriteriaUnidadesXEmpresaxFecha(int emprcodi, DateTime fecha, DateTime fechafin, int idCentral);
        List<EveHoraoperacionDTO> ListEquiposHorasOperacionxFormato(int formatcodi, int emprcodi, DateTime fechaIni, DateTime fechaFin);
        
        List<EveHoraoperacionDTO> ListarHorasOperacxEmpresaxFechaxTipoOPxFam(int emprcodi, DateTime Fecha, DateTime fechaFinal, string idTipoOperacion, int famcodi);
        List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam(int emprcodi, DateTime FechaIni, DateTime fechaFinal, string idTipoOperacion, int famcodi);
        List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam2(int emprcodi, DateTime fechaini, DateTime fechafin, string idTipoOperacion, int idCentral);
        List<EveHoraoperacionDTO> GetCriteriaxPKCodis(string pkCodis);
        List<EveHoraoperacionDTO> GetCriteriaUnidadesxPKCodis(string pkCodis);
        List<EveHoraoperacionDTO> GetHorasURS(DateTime fechaIni, DateTime fechaFin);

        #region Horas Operacion EMS
        List<EveHoraoperacionDTO> ListarHorasOperacionByCriteria(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales, int tipoListado);
        List<EveHoraoperacionDTO> ListarHorasOperacionByCriteriaUnidades(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales);
        #endregion

        #region MigracionSGOCOES-GrupoB
        List<EveHoraoperacionDTO> ListaEstadoOperacion(DateTime fechaIni, DateTime fechaFin);
        List<EveHoraoperacionDTO> ListaProdTipCombustible(DateTime fechaIni, DateTime fechaFin);
        List<EveHoraoperacionDTO> ListaOperacionTension(DateTime fecIni);
        #endregion

        #region Numerales Datos Base
        List<EveHoraoperacionDTO> ListaNumerales_DatosBase_5_1_2(string fechaIni, string fechaFin);
        List<EveHoraoperacionDTO> ListaNumerales_DatosBase_5_6_2(string fechaIni, string fechaFin);
        List<EveHoraoperacionDTO> ListaEstadoOperacion90(DateTime fechaIni, DateTime fechaFin);
        #endregion

        #region Mejoras CMgN

        List<EveHoraoperacionDTO> ObtenerHorasOperacionCompartivoCM(DateTime fecha);

        #endregion
    }
}
