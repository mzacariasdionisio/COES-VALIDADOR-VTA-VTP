using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_PERIODO_PROG
    /// </summary>
    public interface ICoPeriodoProgRepository
    {
        int Save(CoPeriodoProgDTO entity);
        void Update(CoPeriodoProgDTO entity);
        void Delete(int perprgcodi);
        CoPeriodoProgDTO GetById(int perprgcodi);
        List<CoPeriodoProgDTO> List();
        List<CoPeriodoProgDTO> GetByCriteria();
        CoPeriodoProgDTO ObtenerPeriodoProgVigente(DateTime fecha);
    }
}
