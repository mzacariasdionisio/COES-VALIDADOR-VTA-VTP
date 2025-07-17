using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_CONJUNTOENLACE
    /// </summary>
    public interface ICmConjuntoenlaceRepository
    {
        int Save(CmConjuntoenlaceDTO entity);
        void Update(CmConjuntoenlaceDTO entity);
        void Delete(int idGrupo, int idLinea);
        CmConjuntoenlaceDTO GetById(int cnjenlcodi);
        List<CmConjuntoenlaceDTO> List();
        List<CmConjuntoenlaceDTO> GetByCriteria(int idGrupo, int idLinea);
    }
}
