using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_HTRABAJO_ESTADO
    /// </summary>
    public interface IPrHtrabajoEstadoRepository
    {
        int Save(PrHtrabajoEstadoDTO entity);
        void Update(PrHtrabajoEstadoDTO entity);
        void Delete(int htestcodi);
        PrHtrabajoEstadoDTO GetById(int htestcodi);
        List<PrHtrabajoEstadoDTO> List();
        List<PrHtrabajoEstadoDTO> GetByCriteria();
    }
}
