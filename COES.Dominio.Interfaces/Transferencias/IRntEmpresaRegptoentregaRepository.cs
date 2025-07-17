using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

using System.Data; //STS
using System.Data.Common; //STS


namespace COES.Dominio.Interfaces.Transferencias
{
    /// <summary>
    /// Interface de acceso a datos de la tabla RNT_EMPRESA_REGPTOENTREGA
    /// </summary>
    public interface IRntEmpresaRegptoentregaRepository
    {
        int GetMaxId();
        int Save(RntEmpresaRegptoentregaDTO entity, IDbConnection conn, DbTransaction tran, int corrId);
        void Update(RntEmpresaRegptoentregaDTO entity);
        void Delete(int empgencodi);
        RntEmpresaRegptoentregaDTO GetById(int empgencodi);
        List<RntEmpresaRegptoentregaDTO> List(int key);
        List<RntEmpresaRegptoentregaDTO> GetByCriteria();
    }
}
