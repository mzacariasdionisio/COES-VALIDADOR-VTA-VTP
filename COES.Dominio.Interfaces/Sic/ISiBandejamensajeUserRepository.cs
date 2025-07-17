using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_BANDEJAMENSAJE_USER
    /// </summary>
    public interface ISiBandejamensajeUserRepository
    {
        List<SiBandejamensajeUserDTO> listaCantEnCarpetaPorModYUser(int modcodi, string usuario, string correo);
        int SaveCarpeta(string nomCarpeta, string usuario, DateTime fecha);
    }
}
