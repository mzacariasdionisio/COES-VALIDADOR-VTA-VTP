using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CM_EQUIPOBARRA
    /// </summary>
    public interface ICmEquipobarraRepository
    {
        int Save(CmEquipobarraDTO entity);
        void Update(CmEquipobarraDTO entity);
        void Delete(int cmeqbacodi);
        CmEquipobarraDTO GetById(int cmeqbacodi);
        List<CmEquipobarraDTO> List();
        List<CmEquipobarraDTO> GetByCriteria(DateTime fecha);
        List<CmEquipobarraDTO> ObtenerHistorico(int idConfig);
    }
}
