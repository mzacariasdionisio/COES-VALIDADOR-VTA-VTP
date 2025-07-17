using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SPO_NUMCUADRO
    /// </summary>
    public interface ISpoNumcuadroRepository
    {
        int Save(SpoNumcuadroDTO entity);
        void Update(SpoNumcuadroDTO entity);
        void Delete(int numccodi);
        SpoNumcuadroDTO GetById(int numccodi);
        List<SpoNumcuadroDTO> List();
        List<SpoNumcuadroDTO> GetByCriteria(int numecodi);
    }
}
