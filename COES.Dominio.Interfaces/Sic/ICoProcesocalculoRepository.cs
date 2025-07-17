using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla CO_PROCESOCALCULO
    /// </summary>
    public interface ICoProcesocalculoRepository
    {
        int Save(CoProcesocalculoDTO entity);
        void Update(CoProcesocalculoDTO entity);
        void Delete(int coprcacodi);
        CoProcesocalculoDTO GetById(int coprcacodi);
        List<CoProcesocalculoDTO> List();
        List<CoProcesocalculoDTO> GetByCriteria();
        int ValidarExistencia(int idPeriodo, int idVersion);
    }
}
