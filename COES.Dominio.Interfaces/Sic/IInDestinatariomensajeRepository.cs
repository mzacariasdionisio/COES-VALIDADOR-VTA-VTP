using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_DESTINATARIOMENSAJE
    /// </summary>
    public interface IInDestinatariomensajeRepository
    {
        int Save(InDestinatariomensajeDTO entity);
        void Update(InDestinatariomensajeDTO entity);
        void Delete(int indemecodi);
        InDestinatariomensajeDTO GetById(int indemecodi);
        List<InDestinatariomensajeDTO> List();
        List<InDestinatariomensajeDTO> GetByCriteria();
        List<InDestinatariomensajeDTO> ObtenerConfiguracionNotificacion(int empresa, string estado);
        List<InDestinatariomensajeDTO> ObtenerHistorico(int empresa, int usuario);
        List<InDestinatariomensajeDTO> ObtenerConfiguracionVigente();
        List<InDestinatariomensajeDTO> ObtenerConfiguracionVigentePorUsuario(int usuario);
    }
}
