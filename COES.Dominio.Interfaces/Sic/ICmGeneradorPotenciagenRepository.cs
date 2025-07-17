using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_GENERADOR_POTENCIAGEN
    /// </summary>
    public interface ICmGeneradorPotenciagenRepository
    {
        int Save(CmGeneradorPotenciagenDTO entity);
        void Update(CmGeneradorPotenciagenDTO entity);
        void Delete(int genpotcodi);
        CmGeneradorPotenciagenDTO GetById(int genpotcodi);
        List<CmGeneradorPotenciagenDTO> List();
        List<CmGeneradorPotenciagenDTO> GetByCriteria(int relacionCodi);
    }
}
