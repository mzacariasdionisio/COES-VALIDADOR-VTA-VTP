using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ETEMPDETPRYEQ
    /// </summary>
    public interface IFtExtEtempdetpryeqRepository
    {
        int Save(FtExtEtempdetpryeqDTO entity);
        int Save(FtExtEtempdetpryeqDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtEtempdetpryeqDTO entity);
        void Update(FtExtEtempdetpryeqDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int feepeqcodi);
        FtExtEtempdetpryeqDTO GetById(int feepeqcodi);
        List<FtExtEtempdetpryeqDTO> List();
        List<FtExtEtempdetpryeqDTO> GetByCriteria();
        List<FtExtEtempdetpryeqDTO> ListarPorRelProyectoEtapaEmpresa(int feeprycodi, string feepeqestado);
        FtExtEtempdetpryeqDTO GetByProyectoYUnidad(int feeprycodi, int? equicodi, int? grupocodi, string feepeqestado);
        FtExtEtempdetpryeqDTO GetByProyectoUnidadEmpresaEtapa(int? equicodi, int? grupocodi, int ftprycodi, int emprcodi, int ftetcodi, string feepeqestado);
        List<FtExtEtempdetpryeqDTO> ListarDetallesPorRelEmpresaEtapaProyecto(string feepryestado, string feepeqestado, int feeprycodi);
    }
}
