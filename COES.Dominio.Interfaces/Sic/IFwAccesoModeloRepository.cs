using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FW_ACCESO_MODELO
    /// </summary>
    public interface IFwAccesoModeloRepository
    {
        int Save(FwAccesoModeloDTO entity);
        void Update(FwAccesoModeloDTO entity);
        void Delete(int acmodcodi);
        FwAccesoModeloDTO GetById(int idEmpresa, int idModulo, int idEmpresaCorreo);
        List<FwAccesoModeloDTO> List();
        List<FwAccesoModeloDTO> GetByCriteria(int idEmpresa, int idModulo);
        void UpdateClave(FwAccesoModeloDTO entity);
        void EliminarPorContacto(int idEmpresaCorreo);
    }
}
