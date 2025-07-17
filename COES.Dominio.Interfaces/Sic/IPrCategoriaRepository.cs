using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla PR_CATEGORIA
    /// </summary>
    public interface IPrCategoriaRepository
    {
        int Save(PrCategoriaDTO entity);
        void Update(PrCategoriaDTO entity);
        void Delete(int catecodi);
        PrCategoriaDTO GetById(int catecodi);
        List<PrCategoriaDTO> List();
        List<PrCategoriaDTO> GetByCriteria();
        List<PrCategoriaDTO> ListByOriglectcodiYEmprcodi(int origlectcodi, int emprcodi);
    }
}
