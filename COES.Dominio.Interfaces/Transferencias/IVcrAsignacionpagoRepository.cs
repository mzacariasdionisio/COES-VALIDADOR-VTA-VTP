using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla VCR_ASIGNACIONPAGO
    /// </summary>
    public interface IVcrAsignacionpagoRepository
    {
        int Save(VcrAsignacionpagoDTO entity);
        void Update(VcrAsignacionpagoDTO entity);
        void Delete(int vcrecacodi);
        VcrAsignacionpagoDTO GetById(int vcrapcodi);
        List<VcrAsignacionpagoDTO> List(int vcrecacodi);
        List<VcrAsignacionpagoDTO> GetByCriteria(int vcrecacodi, int emprcodi, int equicodicen, int equicodiuni);
        VcrAsignacionpagoDTO GetByIdMesUnidad(int vcrecacodi, int equicodiuni);
        List<VcrAsignacionpagoDTO> ListEmpresaMes(int vcrecacodi);
    }
}
