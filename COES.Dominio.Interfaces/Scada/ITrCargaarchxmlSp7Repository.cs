using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public interface ITrCargaarchxmlSp7Repository
    {
        List<TrCargaarchxmlSp7DTO> GetByFecha(DateTime fechaInicial, DateTime fechaFinal);
    }
}
