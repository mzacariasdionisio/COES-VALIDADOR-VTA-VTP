using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_DEMANDA_MLIBRE
    /// </summary>
    public interface IMeDemandaMercadoLibreRepository
    {
        void Save(string periodo, string periodoSicli, int maxId, string usuario, string fechaDemandaMaxima, string fechaDemandaMaximaSicli);

        void Delete(string periodo);

        int GetMaxId();

        void Update(string usuario, string periodo);
        List<MeDemandaMLibreDTO> ListDemandaMercadoLibreReporte(string periodo, string suministrador, string empresa, int regIni, int regFin);

        int ListDemandaMercadoLibreReporteCount(string periodo, string suministrador, string empresa);

        System.Data.IDataReader ListDemandaMercadoLibreReporteExcel(string periodo, string periodoSICLI, string suministrador, string empresa);
        List<IioPeriodoSicliDTO> ListPeriodoSicli(string permiso);

        int UpdatePeriodoDemandaSicli(string usuario, string periodo, string estadoPeriodo);
        
    }
}
