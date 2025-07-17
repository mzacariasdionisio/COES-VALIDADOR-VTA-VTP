using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EMPRESAMENSAJEDET
    /// </summary>
    public interface ISiEmpresamensajedetRepository
    {
        int Save(SiEmpresamensajedetDTO entity);
        void Update(SiEmpresamensajedetDTO entity);
        void Delete(int emsjdtcodi);
        SiEmpresamensajedetDTO GetById(int emsjdtcodi);
        List<SiEmpresamensajedetDTO> List();
        List<SiEmpresamensajedetDTO> GetByCriteria(int empresaLectura, int msgcodi, string intercodis);
    }
}
