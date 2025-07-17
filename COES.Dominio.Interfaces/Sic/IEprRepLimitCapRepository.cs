using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla EPR_REP_LIMIT_CAP
    /// </summary>
    public interface IEprRepLimitCapRepository
    {
        List<EprRepLimitCapDTO> ListCapacidadTransmision(int idAreaExcel);
        List<EprRepLimitCapDTO> ListActualizaciones();
        List<EprRepLimitCapDTO> ListRevisiones();
        List<EprRepLimitCapDTO> ListCapacidadTransformador(int idAreaExcel);
        List<EprRepLimitCapDTO> ListCapacidadAcoplaminento(int idAreaExcel, int tension);
        List<EprRepLimitCapDTO> ListaEmpresaSigla();
    }
}
