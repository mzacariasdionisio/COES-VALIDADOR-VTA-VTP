using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_FLUJO_POTENCIA
    /// </summary>
    public interface ICmFlujoPotenciaRepository
    {
        int Save(CmFlujoPotenciaDTO entity);
        void Update(CmFlujoPotenciaDTO entity);
        void Delete(int flupotcodi);
        CmFlujoPotenciaDTO GetById(int flupotcodi);
        List<CmFlujoPotenciaDTO> List();
        List<CmFlujoPotenciaDTO> GetByCriteria();
        List<CmFlujoPotenciaDTO> ObtenerReporteFlujoPotencia(DateTime fechaInicio, DateTime fechaFin);
    }
}
