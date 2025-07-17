using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    public interface IDpoMedicion96Repository
    {
        void Save(DpoMedicion96DTO entity);
        void Update(DpoMedicion96DTO entity);
        void Delete(int dpomedcodi, int dpotmecodi, int dpotdtcodi,
            int vergrpcodi, DateTime medifecha);
        List<DpoMedicion96DTO> List();        
        List<DpoMedicion96DTO> ListById(int dpomedcodi, 
            int dpotmecodi, int dpotdtcodi, int vergrpcodi,
            string fecini, string fecfin);
        List<DpoMedicion96DTO> ListByVersion(int dpotmecodi,
            int dpotdtcodi, int vergrpcodi, string fecini,
            string fecfin);
        void DeleteByVersion(int vergrpcodi);
        List<DpoMedicion96DTO> ListByFiltros(string dpomedcodi, int dpotmecodi,
                                                    int dpotdtcodi, int vergrpcodi,
                                                    int anio, string mes);
        List<DpoMedicion96DTO> ObtenerVersionComparacion(
            string medifecha, int vergrpcodi);
        List<DpoMedicion96DTO> ObtenerDatosMediciongrp(string grupocodi,
            string prnmgrtipo, string medifecha, string vergrpcodi);
        List<DpoMedicion96DTO> ObtenerDatosPorVersion(int dpotdtcodi,
            string dpotmecodi, int vergrpcodi, string dpomedfecha);
        List<DpoMedicion96DTO> ObtenerDatosPorVersionNulleable(int dpotdtcodi,
            string dpotmecodi, int vergrpcodi, string dpomedfecha);
        DpoMedicion96DTO ObtenerDatosPorId(int dpomedcodi, int dpotdtcodi,
            string dpotmecodi, int vergrpcodi, 
            string dpomedfecha);
        DpoMedicion96DTO ObtenerDatosAgrupados(string dpomedcodi, int dpotdtcodi,
            string dpotmecodi, int vergrpcodi,
            string dpomedfecha);
        DpoMedicion96DTO ObtenerDatosCompAgrupados(string grupocodi,
            int vergrpcodi, string dpomedfecha);
        List<DpoMedicion96DTO> ObtenerDemRDOPrevExtranet(int formatcodi, 
            int lectcodi, string dpomedfecha);
        DpoMedicion96DTO ObtenerDatosEscenarioYupana(int topcodi,
            int srestcodi, string medifecha);
        DpoMedicion96DTO ObtenerDatosSumEscenarioYupana(int topcodi,
            int srestcodi, string medifecha);
        DpoMedicion96DTO ObtenerDatosFormulaM48(int lectcodi,
            int tipoinfocodi, string ptomedicodi, string fecIni,
            string fecFin);
        DpoMedicion96DTO ObtenerDatosMedicion48(int lectcodi,
            int tipoinfocodi, string ptomedicodi, string fecIni,
            string fecFin);
    }
}
