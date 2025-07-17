using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_CLASEPROG
    /// </summary>
    public interface IInClaseProgRepository
    {
        //-----------------------------------------------------------------------------------
        // ASSETEC.SGH - 09/10/2017: FUNCIONES PERSONALIZADAS PARA LAS CLASES DE PROGRAMACION
        //-----------------------------------------------------------------------------------
        List<InClaseProgDTO> ListarComboClasesProgramacion();

    }
}
