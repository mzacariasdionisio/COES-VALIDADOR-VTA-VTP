using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_PERIODO
    /// </summary>
    public interface ICoPeriodoRepository
    {
        int Save(CoPeriodoDTO entity);
        void Update(CoPeriodoDTO entity);
        void Delete(int copercodi);
        CoPeriodoDTO GetById(int copercodi);
        List<CoPeriodoDTO> List();
        List<CoPeriodoDTO> GetByCriteria(int anio);
        bool ValidarExistencia(int anio, int mes);
        CoPeriodoDTO GetMensualByFecha(string fecha);
    }
}
