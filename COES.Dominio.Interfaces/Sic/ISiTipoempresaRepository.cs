using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_TIPOEMPRESA
    /// </summary>
    public interface ISiTipoempresaRepository
    {
        int Save(SiTipoempresaDTO entity);
        void Update(SiTipoempresaDTO entity);
        void Delete(int tipoemprcodi);
        SiTipoempresaDTO GetById(int tipoemprcodi);
        List<SiTipoempresaDTO> List();
        List<SiTipoempresaDTO> ObtenerTiposEmpresaContacto();
        List<SiTipoempresaDTO> GetByCriteria();
    }
}

