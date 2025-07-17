using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IN_DESTINATARIOMENSAJE_DET
    /// </summary>
    public interface IInDestinatariomensajeDetRepository
    {
        int Save(InDestinatariomensajeDetDTO entity);
        void Update(InDestinatariomensajeDetDTO entity);
        void Delete(int indmdecodi);
        InDestinatariomensajeDetDTO GetById(int indmdecodi);
        List<InDestinatariomensajeDetDTO> List();
        List<InDestinatariomensajeDetDTO> GetByCriteria();
    }
}
