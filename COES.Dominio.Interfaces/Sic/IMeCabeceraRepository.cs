using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_CABECERA
    /// </summary>
    public interface IMeCabeceraRepository
    {
        void Update(MeCabeceraDTO entity);
        void Delete();
        MeCabeceraDTO GetById();
        List<MeCabeceraDTO> List();
        List<MeCabeceraDTO> GetByCriteria();
    }
}
