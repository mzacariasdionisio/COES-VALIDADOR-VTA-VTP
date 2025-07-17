using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_VERSION
    /// </summary>
    public interface ICoVersionRepository
    {
        int Save(CoVersionDTO entity);
        void Update(CoVersionDTO entity);
        void Delete(int covercodi);
        CoVersionDTO GetById(int covercodi);
        List<CoVersionDTO> List();
        List<CoVersionDTO> GetByCriteria(int idPeriodo);
        CoVersionDTO ObtenerVersionPorFecha(DateTime fecha);
        List<CoVersionDTO> GetVersionesPorMes(int mes, int anio);
        
    }
}
