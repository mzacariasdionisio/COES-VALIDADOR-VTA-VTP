using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ETEMPDETPRY
    /// </summary>
    public interface IFtExtEtempdetpryRepository
    {
        int Save(FtExtEtempdetpryDTO entity);
        int Save(FtExtEtempdetpryDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtEtempdetpryDTO entity);
        void Update(FtExtEtempdetpryDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int feeprycodi);
        FtExtEtempdetpryDTO GetById(int feeprycodi);
        FtExtEtempdetpryDTO GetByEmpresaEtapaProyecto(int emprcodi, int ftetcodi, int ftprycodi);
        List<FtExtEtempdetpryDTO> List();
        List<FtExtEtempdetpryDTO> GetByCriteria();
        List<FtExtEtempdetpryDTO> ListarPorRelEmpresaEtapa(string estado, int Fetempcodi);
        List<FtExtEtempdetpryDTO> GetByProyectos(string strFtprycodis);
        
    }
}
