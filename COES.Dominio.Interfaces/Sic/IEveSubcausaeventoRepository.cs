using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EVE_SUBCAUSAEVENTO
    /// </summary>
    public interface IEveSubcausaeventoRepository
    {
        int Save(EveSubcausaeventoDTO entity);
        void Update(EveSubcausaeventoDTO entity);
        void Delete(int subcausacodi);
        EveSubcausaeventoDTO GetById(int subcausacodi);
        List<EveSubcausaeventoDTO> List();
        List<EveSubcausaeventoDTO> GetByCriteria(int idTipoEvento);
        List<EveSubcausaeventoDTO> ObtenerPorCausa(int idCausaEvento);
        List<EveSubcausaeventoDTO> ObtenerSubcausaEvento(int subcausacodi);
        
        // Inicio de Agregado - Sistema de Compensaciones
        List<EveSubcausaeventoDTO> ListTipoOperacion(int pericodi);
        int GetSubCasusaCodi(string desc);
        List<EveSubcausaeventoDTO> ListSubCausaEvento(int pericodi);
        // Fin de Agregado - Sistema de Compensaciones

        #region PR5
        List<EveSubcausaeventoDTO> ObtenerXCausaXCmg(int idCausaEvento, string subcausacmg);
        List<EveSubcausaeventoDTO> GetSubcausaEventoXId(int subcausacodi);
        #endregion

        #region Horas Operacion EMS
        List<EveSubcausaeventoDTO> ListarTipoOperacionHO();
        #endregion

        #region INTERVENCIONES
        List<EveSubcausaeventoDTO> ListarComboCausas(int iEscenario);
        #endregion

        List<EveSubcausaeventoDTO> ObtenerSubcausaEventoByAreausuaria(int subcausacodi, int areacode);

        #region FIT - VALORIZACION DIARIA
        int GetCodByAbrev(string calificacion);
        #endregion

        #region PRONOSTICO DEMANDA

        void UpdateBy(EveSubcausaeventoDTO entity);
        #endregion

        #region SIOSEIN
        List<EveSubcausaeventoDTO> GetListByIds(string subcausacodi);
        #endregion
    }
}

