using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RI_HISTORICO
    /// </summary>
    public interface IRiHistoricoRepository
    {
        int Save(RiHistoricoDTO entity);
        void Update(RiHistoricoDTO entity);
        void Delete(int hisricodi);
        RiHistoricoDTO GetById(int hisricodi);
        List<RiHistoricoDTO> List();
        List<RiHistoricoDTO> GetByCriteria(int anio, string tipo);
        List<RiHistoricoDTO> ListAnios();

        List<RiHistoricoDTO> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin);
    }
}
