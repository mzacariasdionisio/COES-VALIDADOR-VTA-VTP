using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_GENFORZADA_MAESTRO
    /// </summary>
    public interface IPrGenforzadaMaestroRepository
    {
        int Save(PrGenforzadaMaestroDTO entity);
        void Update(PrGenforzadaMaestroDTO entity);
        void Delete(int genformaecodi);
        PrGenforzadaMaestroDTO GetById(int genformaecodi);
        List<PrGenforzadaMaestroDTO> List();
        List<PrGenforzadaMaestroDTO> GetByCriteria();
        int ValidarExistenciaPorRelacion(int relacioncodi);
    }
}
