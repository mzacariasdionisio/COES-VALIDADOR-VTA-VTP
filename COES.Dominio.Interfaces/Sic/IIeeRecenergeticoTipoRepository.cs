using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IEE_RECENERGETICO_TIPO
    /// </summary>
    public interface IIeeRecenergeticoTipoRepository
    {
        int Save(IeeRecenergeticoTipoDTO entity);
        void Update(IeeRecenergeticoTipoDTO entity);
        void Delete(int renertipcodi);
        IeeRecenergeticoTipoDTO GetById(int renertipcodi);
        List<IeeRecenergeticoTipoDTO> List();
        List<IeeRecenergeticoTipoDTO> GetByCriteria();
    }
}
