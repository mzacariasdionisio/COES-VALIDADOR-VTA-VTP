using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_CUADRO3
    /// </summary>
    public interface IIndCuadro3Repository
    {
        int Save(IndCuadro3DTO entity);
        void Update(IndCuadro3DTO entity);
        void Delete(int cuadr3codi);
        IndCuadro3DTO GetById(int cuadr3codi);
        List<IndCuadro3DTO> List();
        List<IndCuadro3DTO> GetByCriteria();
    }
}
