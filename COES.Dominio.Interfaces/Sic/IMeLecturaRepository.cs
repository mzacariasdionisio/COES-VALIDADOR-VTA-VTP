using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_LECTURA
    /// </summary>
    public interface IMeLecturaRepository
    {
        void Save(MeLecturaDTO entity);
        void Update(MeLecturaDTO entity);
        void Delete(int lectcodi);
        MeLecturaDTO GetById(int lectcodi);
        List<MeLecturaDTO> List();
        List<MeLecturaDTO> GetByCriteria(string lectcodi);
    }
}
