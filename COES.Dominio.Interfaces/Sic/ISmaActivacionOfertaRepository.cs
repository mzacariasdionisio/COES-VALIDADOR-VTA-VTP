using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;
using System;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_ACTIVACION_OFERTA
    /// </summary>
    public interface ISmaActivacionOfertaRepository
    {
        int Save(SmaActivacionOfertaDTO entity);
        int SaveTransaccional(SmaActivacionOfertaDTO entity, IDbConnection connection, DbTransaction transaction);
        void Update(SmaActivacionOfertaDTO entity);
        void Delete(int smapaccodi);
        SmaActivacionOfertaDTO GetById(int smapaccodi);
        List<SmaActivacionOfertaDTO> List();
        List<SmaActivacionOfertaDTO> GetByCriteria();
        List<SmaActivacionOfertaDTO> ListarActivacionesPorRangoFechas(DateTime fechaIni, DateTime fechaFin);
    }
}
