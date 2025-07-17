using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CP_TERMINAL
    /// </summary>
    public interface ICpTerminalRepository
    {

        List<CpTerminalDTO> List();
        int ObtenerNodoTopologico(int recurcodi,int ttermcodi, int topcodi);
        void CrearCopia(int topcodi1, int topcodi2);
        void CrearCopiaNodoConectividad(int topcodi1, int topcodi2);
    }
}
