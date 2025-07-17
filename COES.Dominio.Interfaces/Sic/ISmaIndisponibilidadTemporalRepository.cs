using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
    /// </summary>
    public interface ISmaIndisponibilidadTemporalRepository
    {
        int Save(SmaIndisponibilidadTemporalDTO entity);
        void Update(SmaIndisponibilidadTemporalDTO entity);
        void Delete(int smaintcodi);
        SmaIndisponibilidadTemporalDTO GetById(int smaintcodi);
        List<SmaIndisponibilidadTemporalDTO> List();
        List<SmaIndisponibilidadTemporalDTO> GetByCriteria();
        List<SmaIndisponibilidadTemporalDTO> ListarPorFecha(DateTime fecha);
    }
}
