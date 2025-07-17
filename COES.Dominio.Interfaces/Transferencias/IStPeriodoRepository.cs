using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_PERIODO
    /// </summary>
    public interface IStPeriodoRepository
    {
        int Save(StPeriodoDTO entity);
        void Update(StPeriodoDTO entity);
        void Delete(int stpercodi);
        StPeriodoDTO GetById(int stpercodi);
        List<StPeriodoDTO> List();
        List<StPeriodoDTO> GetByCriteria(string nombre);
        StPeriodoDTO GetByIdPeriodoAnterior(int stpercodi);
    }
}
