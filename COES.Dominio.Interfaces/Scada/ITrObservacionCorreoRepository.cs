using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_OBSERVACION_CORREO
    /// </summary>
    public interface ITrObservacionCorreoRepository
    {
        int Save(TrObservacionCorreoDTO entity);
        void Update(TrObservacionCorreoDTO entity);
        void Delete(int obscorcodi);
        TrObservacionCorreoDTO GetById(int obscorcodi);
        List<TrObservacionCorreoDTO> List();
        List<TrObservacionCorreoDTO> GetByCriteria(int idEmpresa);
    }
}
