using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_VERDEFICIT
    /// </summary>
    public interface IVcrVerdeficitRepository
    {
        int Save(VcrVerdeficitDTO entity);
        void Update(VcrVerdeficitDTO entity);
        void Delete(int vcrvdecodi);
        VcrVerdeficitDTO GetById(int vcrvdecodi);
        List<VcrVerdeficitDTO> List();
        List<VcrVerdeficitDTO> GetByCriteria(int vcrdsrcodi);
        List<VcrVerdeficitDTO> ListDia(int vcrdsrcodi, int grupocodi, DateTime vcrvdefecha);
        List<VcrVerdeficitDTO> ListDiaHFHI(int vcrdsrcodi, DateTime vcrvdefecha);
    }
}
