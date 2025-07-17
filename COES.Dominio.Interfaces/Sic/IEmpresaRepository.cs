using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Scada
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SI_EMPRESA
    /// </summary>
    public interface IEmpresaRepository 
    {   
        List<EmpresaDTO> GetByCriteria(string nombre);
    }

}
