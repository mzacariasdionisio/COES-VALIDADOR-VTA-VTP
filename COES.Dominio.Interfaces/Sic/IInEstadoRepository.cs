using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_ESTADO
    /// </summary>
    public interface IInEstadoRepository
    {
        //-----------------------------------------------------------------------------------------------
        // ASSETEC.SGH - 12/10/2017: FUNCIONES PARA ESTADOS
        //-----------------------------------------------------------------------------------------------
        List<InEstadoDTO> ListarComboEstados(int iEscenario);

    }
}