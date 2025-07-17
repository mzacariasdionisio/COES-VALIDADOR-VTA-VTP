using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla WB_PROVEEDOR
    /// </summary>
    public interface IWbProveedorRepository
    {
        int Save(WbProveedorDTO entity);
        void Update(WbProveedorDTO entity);
        void Delete(int provcodi);
        WbProveedorDTO GetById(int provcodi);
        List<WbProveedorDTO> List();
        List<WbProveedorDTO> GetByCriteria(string nombre, string tipo, DateTime? fechaD, DateTime? fechaH);
        List<string> GetByCriteriaTipo();
    }
}
