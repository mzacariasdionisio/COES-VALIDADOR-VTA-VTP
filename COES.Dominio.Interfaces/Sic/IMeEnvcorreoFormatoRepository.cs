using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla ME_ENVCORREO_FORMATO
    /// </summary>
    public interface IMeEnvcorreoFormatoRepository
    {
        int Save(MeEnvcorreoFormatoDTO entity);
        void Update(MeEnvcorreoFormatoDTO entity);
        void Delete(int idEmpresa, string formatos);
        MeEnvcorreoFormatoDTO GetById(int ecformcodi);
        List<MeEnvcorreoFormatoDTO> List();
        List<MeEnvcorreoFormatoDTO> GetByCriteria();
        List<MeEnvcorreoFormatoDTO> ObtenerCorreoEmpresa();
    }
}
