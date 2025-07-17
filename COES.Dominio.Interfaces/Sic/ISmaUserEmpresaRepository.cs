using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_USER_EMPRESA
    /// </summary>
    public interface ISmaUserEmpresaRepository
    {               
        List<SmaUserEmpresaDTO> List(int codigoEmpresa);

        List<SmaUserEmpresaDTO> ListEmpresa(int codigoUser);

        List<SmaUserEmpresaDTO> GetByCriteria(int codigoEmpresa);
    }
}
