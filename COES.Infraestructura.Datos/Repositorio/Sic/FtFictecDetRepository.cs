using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla FT_FICTECDET
    /// </summary>
    public class FtFictecDetRepository : RepositoryBase, IFtFictecDetRepository
    {
        public FtFictecDetRepository(string strConn)
            : base(strConn)
        {
        }

        FtFictecDetHelper helper = new FtFictecDetHelper();

        public int Save(FtFictecDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftecdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, entity.Fteccodi);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftecdfecha, DbType.DateTime, entity.Ftecdfecha);
            dbProvider.AddInParameter(command, helper.Ftecdusuario, DbType.String, entity.Ftecdusuario);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFictecDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, entity.Fteccodi);
            dbProvider.AddInParameter(command, helper.Fteqcodi, DbType.Int32, entity.Fteqcodi);
            dbProvider.AddInParameter(command, helper.Ftecdcodi, DbType.Int32, entity.Ftecdcodi);
            dbProvider.AddInParameter(command, helper.Ftecdfecha, DbType.DateTime, entity.Ftecdfecha);
            dbProvider.AddInParameter(command, helper.Ftecdusuario, DbType.String, entity.Ftecdusuario);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftecdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftecdcodi, DbType.Int32, ftecdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByFteccodi(int fteccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteByFteccodi);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, fteccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFictecDetDTO GetById(int ftecdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftecdcodi, DbType.Int32, ftecdcodi);
            FtFictecDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFictecDetDTO> List()
        {
            List<FtFictecDetDTO> entitys = new List<FtFictecDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<FtFictecDetDTO> GetByCriteria()
        {
            List<FtFictecDetDTO> entitys = new List<FtFictecDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
