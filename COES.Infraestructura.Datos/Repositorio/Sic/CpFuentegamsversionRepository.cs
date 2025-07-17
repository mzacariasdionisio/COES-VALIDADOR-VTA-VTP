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
    /// Clase de acceso a datos de la tabla CP_FUENTEGAMS
    /// </summary>
    public class CpFuentegamsversionRepository: RepositoryBase, ICpFuentegamsversionRepository
    {
        public CpFuentegamsversionRepository(string strConn)
            : base(strConn)
        {
        }

        CpFuentegamsversionHelper helper = new CpFuentegamsversionHelper();



        public int Save(CpFuentegamsversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            string sqlStr = string.Format(helper.SqlGetMaxVersion, entity.Ftegcodi);
            command = dbProvider.GetSqlStringCommand(sqlStr);
            result = dbProvider.ExecuteScalar(command);
            int id2 = 1;
            if (result != null) id2 = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fverscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, entity.Ftegcodi);
            dbProvider.AddInParameter(command, helper.Fversnum, DbType.Int32, id2);
            dbProvider.AddInParameter(command, helper.Fversdescrip, DbType.String, entity.Fversdescrip);
            dbProvider.AddInParameter(command, helper.Fversusumodificacion, DbType.String, entity.Fversusumodificacion);
            dbProvider.AddInParameter(command, helper.Fversfecmodificacion, DbType.DateTime, entity.Fversfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fversestado, DbType.Int32, entity.Fversestado);
            dbProvider.AddInParameter(command, helper.Fversinputdata, DbType.String, entity.Fversinputdata);
            dbProvider.AddInParameter(command, helper.Fversruncase, DbType.String, entity.Fversruncase);
            dbProvider.AddInParameter(command, helper.Fverscodigoencrip, DbType.Binary, entity.Fverscodigoencrip);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpFuentegamsversionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Fversdescrip, DbType.String, entity.Fversdescrip);
            dbProvider.AddInParameter(command, helper.Fversusumodificacion, DbType.String, entity.Fversusumodificacion);
            dbProvider.AddInParameter(command, helper.Fversfecmodificacion, DbType.DateTime, entity.Fversfecmodificacion);
            dbProvider.AddInParameter(command, helper.Fversestado, DbType.Int32, entity.Fversestado);
            dbProvider.AddInParameter(command, helper.Fversinputdata, DbType.String, entity.Fversinputdata);
            dbProvider.AddInParameter(command, helper.Fversruncase, DbType.String, entity.Fversruncase);
            dbProvider.AddInParameter(command, helper.Fverscodigoencrip, DbType.Binary, entity.Fverscodigoencrip);
            dbProvider.AddInParameter(command, helper.Fverscodi, DbType.Int32, entity.Fverscodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Fverscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, Fverscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpFuentegamsversionDTO GetById(int Fverscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, Fverscodi);
            CpFuentegamsversionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpFuentegamsversionDTO> List()
        {
            List<CpFuentegamsversionDTO> entitys = new List<CpFuentegamsversionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            CpFuentegamsversionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entitys.Add(entity);
                    
                }
            }

            return entitys;
        }

        public List<CpFuentegamsversionDTO> GetByCriteria(int topcodi)
        {
            List<CpFuentegamsversionDTO> entitys = new List<CpFuentegamsversionDTO>();
            string sqlQuery = string.Format(helper.SqlGetByCriteria, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            
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
