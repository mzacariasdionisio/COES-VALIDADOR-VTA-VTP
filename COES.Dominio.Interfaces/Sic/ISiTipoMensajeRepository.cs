using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    public interface ISiTipoMensajeRepository
    {
        List<SiTipoMensajeDTO> Listar();

        #region Siosein
        List<SiTipoMensajeDTO> ListarXMod(int modcodi);
        #endregion
    }
}
