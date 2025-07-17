using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_ETEMPDETEQ
    /// </summary>
    public interface IFtExtEtempdeteqRepository
    {
        int Save(FtExtEtempdeteqDTO entity);
        int Save(FtExtEtempdeteqDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtEtempdeteqDTO entity);
        void Update(FtExtEtempdeteqDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int feeeqcodi);
        FtExtEtempdeteqDTO GetById(int feeeqcodi);
        List<FtExtEtempdeteqDTO> List();
        List<FtExtEtempdeteqDTO> GetByCriteria();
        FtExtEtempdeteqDTO GetByRelacionUnidadEmpresaEtapa(int? equicodi, int? grupocodi, int emprcodi, int idEtapa);
        List<FtExtEtempdeteqDTO> GetByEmpresaEtapa(int emprcodi, int ftetcodi);
        List<FtExtEtempdeteqDTO> ListarPorRelEmpresaEtapa(string estado, int Fetempcodi);
    }
}
