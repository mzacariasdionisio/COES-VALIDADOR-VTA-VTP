using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_DETALLEETAPA
    /// </summary>
    public interface ICpDetalleetapaRepository
    {
        List<CpDetalleetapaDTO> List(int topcodi);
        void CrearCopia(int topcodi1, int topcodi2);
        List<CpDetalleetapaDTO> Listar(string topcodis);
    }
}
