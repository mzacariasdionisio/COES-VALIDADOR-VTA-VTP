using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ST_SISTEMATRANS
    /// </summary>
    public interface IStSistematransRepository
    {
        int Save(StSistematransDTO entity);
        void Update(StSistematransDTO entity);
        void Delete(int sistrncodi);
        void DeleteVersion(int strecacodi);
        StSistematransDTO GetById(int sistrncodi);
        List<StSistematransDTO> List(int strecacodi);
        List<StSistematransDTO> GetByCriteria(int recacodi);
        List<StSistematransDTO> ListByStSistemaTransVersion(int strecacodi);
        StSistematransDTO GetBySisTransNomb(int strecacodi, string sSisTransnombre);
        List<StSistematransDTO> ListByStSistemaTransReporte(int strecacodi);
    }
}
