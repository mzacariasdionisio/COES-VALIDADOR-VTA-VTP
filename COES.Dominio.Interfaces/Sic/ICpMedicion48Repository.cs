using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_MEDICION48
    /// </summary>
    public interface ICpMedicion48Repository
    {
        void Save(CpMedicion48DTO entity);
        void Update(CpMedicion48DTO entity);
        void Delete(int recurcodi, int srestcodi, int topcodi, DateTime medifecha);
        CpMedicion48DTO GetById(int recurcodi, int srestcodi, int topcodi, DateTime medifecha);
        List<CpMedicion48DTO> List(int topcodi);
        List<CpMedicion48DTO> GetByCriteria(string topcodi, DateTime medifecha, string srestcodi);
        List<CpMedicion48DTO> ObtenerDatosModelo(string topcodi, DateTime medifecha, string srestcodi, int sinrsf);
        List<CpMedicion48DTO> ListByTipoYSubrestriccion(int toptipo, int srestcodi, DateTime fechaIni, DateTime fechaFin);
        List<CpMedicion48DTO> GetByCriteriaRecurso(int pTopcodi, string pSubRestriccion, int recurcodi);
        List<CpMedicion48DTO> ObtieneRegistrosToDespacho(int topcodi, short srestcodi, int origlectcodi);
        List<CpMedicion48DTO> ObtieneRegistrosToDespachoPTermica1(int topcodi, short srestcodi);
        List<CpMedicion48DTO> ObtieneRegistrosToDespachoPTermica2(int topcodi, short srestcodi);
        List<CpMedicion48DTO> ObtieneRegistrosToDespachoPrGrupo(int topcodi, string srestcodi);
        List<CpMedicion48DTO> ObtieneRegistrosPHToDespacho(int topcodi, short srestcodi, int origlectcodi);
        List<CpMedicion48DTO> ObtieneCostoMarginalBarraEscenario(int topcodi, short srestcodi, DateTime fecha);
        List<CpMedicion48DTO> ObtieneRegistrosToBarra(int topcodi, short srestcodi, int origlectcodi);
        List<CpMedicion48DTO> ObtieneRegistrosToDespachoRerPrGrupo(int topcodi, string srestcodi);
        //Yupana
        List<CpMedicion48DTO> ListaRestricion(int pTopcodi, short pSubRestriccion);
        //- Cambio movisoft 19032021
        List<CpMedicion48DTO> ObtenerCongestionProgramada(int topcodi);

        //- Fin cambio movisoft 19032021

        #region Yupana Continuo
        List<CpMedicion48DTO> ListaSubRestriccionGams(int pTopcodi);
        void CrearCopia(int topcodi1, int topcodi2, DateTime fecha1, DateTime fecha2, int signo);
        void DeleteTopSubrestric(int topcodi, string lsurest);

        #endregion

        #region Mejoras CMgN
        List<CpMedicion48DTO> ObtenerProgramaPorRecurso(string topcodi, int recurcodi, string propcodi);
        #endregion

        #region SIOSEIN-PRIE-2021
        List<CpMedicion48DTO> ObtieneCostoMarginalBarraEscenarioParaUnaBarra(int barrcodi, int topcodi, short srestcodi, DateTime fecha);
        #endregion

        #region Intervenciones

        List<CpMedicion48DTO> ObtenerCapacidadNominal(int topcodi);
        List<CpMedicion48DTO> ObtenerConsumoGasNatural(int topcodi, int srestcodi);

        #endregion
    }
}
