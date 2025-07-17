using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_RAEJECUTADADET
    /// </summary>
    public interface ICoRaejecutadadetRepository
    {
        int Save(CoRaejecutadadetDTO entity);
        void Update(CoRaejecutadadetDTO entity);
        void Delete(int copercodi, int covercodi);
        CoRaejecutadadetDTO GetById(int coradecodi);
        List<CoRaejecutadadetDTO> List();
        List<CoRaejecutadadetDTO> GetByCriteria(int periodo, int version, DateTime fecha);
        List<CoRaejecutadadetDTO> ObtenerConsulta(int periodo, int version);
    }
}
