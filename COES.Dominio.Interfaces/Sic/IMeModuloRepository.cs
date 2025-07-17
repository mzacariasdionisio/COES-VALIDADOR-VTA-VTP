using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_MODULO
    /// </summary>
    public interface IMeModuloRepository
    {
        void Update(MeModuloDTO entity);
        void Delete();
        MeModuloDTO GetById();
        List<MeModuloDTO> List();
        List<MeModuloDTO> GetByCriteria();
    }
}
