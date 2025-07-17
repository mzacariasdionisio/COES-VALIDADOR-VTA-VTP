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
    /// Clase de acceso a datos de la tabla SPO_NUMCUADRO
    /// </summary>
    public class SpoNumcuadroRepository: RepositoryBase, ISpoNumcuadroRepository
    {
        public SpoNumcuadroRepository(string strConn): base(strConn)
        {
        }

        SpoNumcuadroHelper helper = new SpoNumcuadroHelper();

        public int Save(SpoNumcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Numcdescrip, DbType.String, entity.Numcdescrip);
            dbProvider.AddInParameter(command, helper.Numctitulo, DbType.String, entity.Numctitulo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SpoNumcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, entity.Numccodi);
            dbProvider.AddInParameter(command, helper.Numecodi, DbType.Int32, entity.Numecodi);
            dbProvider.AddInParameter(command, helper.Numcdescrip, DbType.String, entity.Numcdescrip);
            dbProvider.AddInParameter(command, helper.Numctitulo, DbType.String, entity.Numctitulo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int numccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, numccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SpoNumcuadroDTO GetById(int numccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Numccodi, DbType.Int32, numccodi);
            SpoNumcuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SpoNumcuadroDTO> List()
        {
            List<SpoNumcuadroDTO> entitys = new List<SpoNumcuadroDTO>();
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

        public List<SpoNumcuadroDTO> GetByCriteria(int numecodi)
        {
            List<SpoNumcuadroDTO> entitys = new List<SpoNumcuadroDTO>();
            string strSql = string.Format(helper.SqlGetByCriteria, numecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

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
