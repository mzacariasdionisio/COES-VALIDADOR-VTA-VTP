using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla IND_DETCUADRO7
    /// </summary>
    public interface IIndDetcuadro7Repository
    {
        int Save(IndDetcuadro7DTO entity);
        void Update(IndDetcuadro7DTO entity);
        void Delete(int cuadr7codi);
        IndDetcuadro7DTO GetById(int cuadr7codi);
        List<IndDetcuadro7DTO> List();
        List<IndDetcuadro7DTO> GetByCriteria();
        List<IndDetcuadro7DTO> GetCargarViewCuadro7(int percuacodi);
    }
}
