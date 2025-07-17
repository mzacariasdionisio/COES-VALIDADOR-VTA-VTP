using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using System.Data;

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla FT_EXT_CORREOAREADET
    /// </summary>
    public interface IFtExtCorreoareadetRepository
    {
        int GetMaxId();
        int Save(FtExtCorreoareadetDTO entity);
        void Update(FtExtCorreoareadetDTO entity);
        void Delete(int faremdcodi);
        FtExtCorreoareadetDTO GetById(int faremdcodi);
        List<FtExtCorreoareadetDTO> List();
        List<FtExtCorreoareadetDTO> GetByCriteria();
        List<FtExtCorreoareadetDTO> ListarCorreosPorArea(string strFaremcodis);
        List<FtExtCorreoareadetDTO> ListarPorCorreo(string correo);
        int Save(FtExtCorreoareadetDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Update(FtExtCorreoareadetDTO entity, IDbConnection connection, IDbTransaction transaction);
        void Delete(int faremdcodi, IDbConnection connection, IDbTransaction transaction);
    }
}
