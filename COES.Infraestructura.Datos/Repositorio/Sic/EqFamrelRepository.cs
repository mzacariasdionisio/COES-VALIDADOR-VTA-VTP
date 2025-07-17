using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Text.RegularExpressions;
using System.Reflection;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EQ_FAMREL
    /// </summary>
    public class EqFamrelRepository: RepositoryBase, IEqFamrelRepository
    {
        public EqFamrelRepository(string strConn): base(strConn)
        {
        }

        EqFamrelHelper helper = new EqFamrelHelper();

        public void Save(EqFamrelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, entity.Tiporelcodi);
            dbProvider.AddInParameter(command, helper.Famcodi1, DbType.Int32, entity.Famcodi1);
            dbProvider.AddInParameter(command, helper.Famcodi2, DbType.Int32, entity.Famcodi2);
            dbProvider.AddInParameter(command, helper.Famnumconec, DbType.Int32, entity.Famnumconec);
            dbProvider.AddInParameter(command, helper.Famreltension, DbType.String, entity.Famreltension);
            dbProvider.AddInParameter(command, helper.Famrelestado, DbType.String, entity.Famrelestado);
            dbProvider.AddInParameter(command, helper.Famrelusuariocreacion, DbType.String, entity.Famrelusuariocreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(EqFamrelDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);


            dbProvider.AddInParameter(command, helper.Famcodi1, DbType.Int32, entity.Famcodi1);
            dbProvider.AddInParameter(command, helper.Famcodi2, DbType.Int32, entity.Famcodi2);
            dbProvider.AddInParameter(command, helper.Famnumconec, DbType.Int32, entity.Famnumconec);
            dbProvider.AddInParameter(command, helper.Famreltension, DbType.String, entity.Famreltension);
            dbProvider.AddInParameter(command, helper.Famrelestado, DbType.String, entity.Famrelestado);
            dbProvider.AddInParameter(command, helper.Famrelusuarioupdate, DbType.String, entity.Famrelusuarioupdate);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, entity.Tiporelcodi);
            dbProvider.AddInParameter(command, helper.Famcodi1old, DbType.Int32, entity.Famcodi1old);
            dbProvider.AddInParameter(command, helper.Famcodi2old, DbType.Int32, entity.Famcodi2old);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tiporelcodi, int famcodi1, int famcodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Famcodi1, DbType.Int32, famcodi1);
            dbProvider.AddInParameter(command, helper.Famcodi2, DbType.Int32, famcodi2);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int tiporelcodi, int famcodi1, int famcodi2, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Famrelusuarioupdate, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Famcodi1, DbType.Int32, famcodi1);
            dbProvider.AddInParameter(command, helper.Famcodi2, DbType.Int32, famcodi2);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqFamrelDTO GetById(int tiporelcodi, int famcodi1, int famcodi2)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tiporelcodi, DbType.Int32, tiporelcodi);
            dbProvider.AddInParameter(command, helper.Famcodi1, DbType.Int32, famcodi1);
            dbProvider.AddInParameter(command, helper.Famcodi2, DbType.Int32, famcodi2);
            EqFamrelDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Famnomb1 = dr.GetString(dr.GetOrdinal("FAMILIA1"));
                    entity.Famnomb2 = dr.GetString(dr.GetOrdinal("FAMILIA2"));
                }
            }

            return entity;
        }

        public List<EqFamrelDTO> List()
        {
            List<EqFamrelDTO> entitys = new List<EqFamrelDTO>();
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

        public List<EqFamrelDTO> GetByCriteria()
        {
            List<EqFamrelDTO> entitys = new List<EqFamrelDTO>();
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



        public List<EqFamrelDTO> GetByTipoRel(int iTipoRel, string strEstado)
        {
            List<EqFamrelDTO> entitys = new List<EqFamrelDTO>();
            string strCommand = string.Format(helper.SqlGetByTipoRel, iTipoRel, strEstado);
            DbCommand command = dbProvider.GetSqlStringCommand(strCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oFamRel = helper.Create(dr);
                    oFamRel.Famnomb1 = dr.GetString(dr.GetOrdinal("FAMILIA1"));
                    oFamRel.Famnomb2 = dr.GetString(dr.GetOrdinal("FAMILIA2"));
                    entitys.Add(oFamRel);
                }
            }

            return entitys;
        }
    }
}
