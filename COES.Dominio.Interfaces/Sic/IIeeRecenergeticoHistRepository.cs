using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IEE_RECENERGETICO_HIST
    /// </summary>
    public interface IIeeRecenergeticoHistRepository
    {
        int Save(IeeRecenergeticoHistDTO entity);
        void Update(IeeRecenergeticoHistDTO entity);
        void Delete(int renercodi);
        IeeRecenergeticoHistDTO GetById(int renercodi);
        List<IeeRecenergeticoHistDTO> List();
        List<IeeRecenergeticoHistDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin);
    }
}
