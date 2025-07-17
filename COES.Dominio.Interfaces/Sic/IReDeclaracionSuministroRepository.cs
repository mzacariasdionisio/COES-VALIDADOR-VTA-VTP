using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RE_DECLARACION_SUMINISTRO
    /// </summary>
    public interface IReDeclaracionSuministroRepository
    {
        int Save(ReDeclaracionSuministroDTO entity);
        void Update(ReDeclaracionSuministroDTO entity);
        void Delete(int redeccodi);
        ReDeclaracionSuministroDTO GetById(int redeccodi);
        List<ReDeclaracionSuministroDTO> List();
        List<ReDeclaracionSuministroDTO> GetByCriteria();
    }
}
