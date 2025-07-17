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
    /// Clase de acceso a datos de la tabla ME_HEADCOLUMN
    /// </summary>
    public class MeHeadcolumnRepository: RepositoryBase, IMeHeadcolumnRepository
    {
        public MeHeadcolumnRepository(string strConn): base(strConn)
        {
        }

        MeHeadcolumnHelper helper = new MeHeadcolumnHelper();

        public void Save(MeHeadcolumnDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Headpos, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Headlen, DbType.Int32, entity.Headlen);
            dbProvider.AddInParameter(command, helper.Headrow, DbType.Int32, entity.Headrow);
            dbProvider.AddInParameter(command, helper.Headnombre, DbType.String, entity.Headnombre);

            dbProvider.ExecuteNonQuery(command);
        }      

        public void Update(MeHeadcolumnDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Headlen, DbType.Int32, entity.Headlen);
            dbProvider.AddInParameter(command, helper.Headrow, DbType.Int32, entity.Headrow);
            dbProvider.AddInParameter(command, helper.Headnombre, DbType.String, entity.Headnombre);

            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, entity.Formatcodi);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, entity.Hojacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Headpos, DbType.Int32, entity.Headpos);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int formato, int hoja, int empresa, int pos)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formato);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, hoja);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
            dbProvider.AddInParameter(command, helper.Headpos, DbType.Int32, pos);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeHeadcolumnDTO GetById(int formato,int hoja,int empresa,int pos)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formato);
            dbProvider.AddInParameter(command, helper.Hojacodi, DbType.Int32, hoja);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
            dbProvider.AddInParameter(command, helper.Headpos, DbType.Int32, pos);
            MeHeadcolumnDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeHeadcolumnDTO> List()
        {
            List<MeHeadcolumnDTO> entitys = new List<MeHeadcolumnDTO>();
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

        public List<MeHeadcolumnDTO> GetByCriteria(int formato,int empresa)
        {
            List<MeHeadcolumnDTO> entitys = new List<MeHeadcolumnDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Formatcodi, DbType.Int32, formato);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, empresa);
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
