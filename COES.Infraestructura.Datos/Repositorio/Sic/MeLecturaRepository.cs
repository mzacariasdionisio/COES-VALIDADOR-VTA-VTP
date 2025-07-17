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
    /// Clase de acceso a datos de la tabla ME_LECTURA
    /// </summary>
    public class MeLecturaRepository: RepositoryBase, IMeLecturaRepository
    {
        public MeLecturaRepository(string strConn): base(strConn)
        {
        }

        MeLecturaHelper helper = new MeLecturaHelper();

        public void Save(MeLecturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Lectnro, DbType.Int32, entity.Lectnro);
            dbProvider.AddInParameter(command, helper.Lectnomb, DbType.String, entity.Lectnomb);
            dbProvider.AddInParameter(command, helper.Lectabrev, DbType.String, entity.Lectabrev);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MeLecturaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lectnro, DbType.Int32, entity.Lectnro);
            dbProvider.AddInParameter(command, helper.Lectnomb, DbType.String, entity.Lectnomb);
            dbProvider.AddInParameter(command, helper.Lectabrev, DbType.String, entity.Lectabrev);
            dbProvider.AddInParameter(command, helper.Origlectcodi, DbType.Int32, entity.Origlectcodi);
            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, entity.Lectcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int lectcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeLecturaDTO GetById(int lectcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Lectcodi, DbType.Int32, lectcodi);
            MeLecturaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeLecturaDTO> List()
        {
            List<MeLecturaDTO> entitys = new List<MeLecturaDTO>();
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

        public List<MeLecturaDTO> GetByCriteria(string lectcodi)
        {
            List<MeLecturaDTO> entitys = new List<MeLecturaDTO>();
            string query = string.Format(helper.SqlGetByCriteria, lectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
