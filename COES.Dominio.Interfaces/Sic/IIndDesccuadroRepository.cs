using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_DESCCUADRO
    /// </summary>
    public interface IIndDesccuadroRepository
    {
        int Save(IndDesccuadroDTO entity);
        void Update(IndDesccuadroDTO entity);
        void Delete(int descucodi);
        IndDesccuadroDTO GetById(int descucodi);
        List<IndDesccuadroDTO> List();
        List<IndDesccuadroDTO> GetByCriteria();
    }
}
