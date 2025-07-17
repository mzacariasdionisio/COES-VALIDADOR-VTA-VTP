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
    public class CpFuentegamsRepository: RepositoryBase, ICpFuentegamsRepository
    {
        public CpFuentegamsRepository(string strConn): base(strConn)
        {
        }

        CpFuentegamsHelper helper = new CpFuentegamsHelper();

        public int Save(CpFuentegamsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ftegnombre, DbType.String, entity.Ftegnombre);
            dbProvider.AddInParameter(command, helper.Ftgedefault, DbType.Int32, entity.Ftegdefault);
            dbProvider.AddInParameter(command, helper.Ftegestado, DbType.Int32, entity.Ftegestado);
            dbProvider.AddInParameter(command, helper.Ftemetodo, DbType.Int32, entity.Ftemetodo);
            dbProvider.AddInParameter(command, helper.Ftegusumodificacion, DbType.String, entity.Ftegusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftegfecmodificacion, DbType.DateTime, entity.Ftegfecmodificacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CpFuentegamsDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftegnombre, DbType.String, entity.Ftegnombre);
            dbProvider.AddInParameter(command, helper.Ftgedefault, DbType.Int32, entity.Ftegdefault);
            dbProvider.AddInParameter(command, helper.Ftegestado, DbType.Int32, entity.Ftegestado);
            dbProvider.AddInParameter(command, helper.Ftemetodo, DbType.Int32, entity.Ftemetodo);
            dbProvider.AddInParameter(command, helper.Ftegusumodificacion, DbType.String, entity.Ftegusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftegfecmodificacion, DbType.DateTime, entity.Ftegfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, entity.Ftegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int ftegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, ftegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpFuentegamsDTO GetByIdVersion(int fverscodi)
        {
            string strSql = string.Format(helper.SqlGetByIdVersion, fverscodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);

            CpFuentegamsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iFverscodi = dr.GetOrdinal(helper.Fverscodi);
                    if (!dr.IsDBNull(iFverscodi)) entity.Fverscodi = Convert.ToInt32(dr.GetValue(iFverscodi));
                    int iFversnum = dr.GetOrdinal(helper.Fversnum);
                    if (!dr.IsDBNull(iFversnum)) entity.Fversnum = Convert.ToInt32(dr.GetValue(iFversnum));
                    int iFversdescrip = dr.GetOrdinal(helper.Fversdescrip);
                    if (!dr.IsDBNull(iFversdescrip)) entity.Fversdescrip = dr.GetString(iFversdescrip);
                    int iFversinputdata = dr.GetOrdinal(helper.Fversinputdata);
                    if (!dr.IsDBNull(iFversinputdata)) entity.Fversinputdata = dr.GetString(iFversinputdata);

                    int iFversruncase = dr.GetOrdinal(helper.Fversruncase);
                    if (!dr.IsDBNull(iFversruncase)) entity.Fversruncase = dr.GetString(iFversruncase);

                    int iFverscodigoencrip = dr.GetOrdinal(helper.Fverscodigoencrip);
                    if (!dr.IsDBNull(iFverscodigoencrip)) entity.Fverscodigoencrip = (byte[])dr.GetValue(iFverscodigoencrip);

                    int iFversfecmodificacion = dr.GetOrdinal(helper.Fversfecmodificacion);
                    if (!dr.IsDBNull(iFversfecmodificacion)) entity.Fversfecmodificacion = dr.GetDateTime(iFversfecmodificacion);
                }
            }

            return entity;
        }

        public CpFuentegamsDTO GetById(int ftegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Ftegcodi, DbType.Int32, ftegcodi);
            CpFuentegamsDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iFverscodi = dr.GetOrdinal(helper.Fverscodi);
                    if (!dr.IsDBNull(iFverscodi)) entity.Fverscodi = Convert.ToInt32(dr.GetValue(iFverscodi));
                    int iFversnum = dr.GetOrdinal(helper.Fversnum);
                    if (!dr.IsDBNull(iFversnum)) entity.Fversnum = Convert.ToInt32(dr.GetValue(iFversnum));
                    int iFversdescrip = dr.GetOrdinal(helper.Fversdescrip);
                    if (!dr.IsDBNull(iFversdescrip)) entity.Fversdescrip = dr.GetString(iFversdescrip);
                    int iFversinputdata = dr.GetOrdinal(helper.Fversinputdata);
                    if (!dr.IsDBNull(iFversinputdata)) entity.Fversinputdata = dr.GetString(iFversinputdata);

                    int iFversruncase = dr.GetOrdinal(helper.Fversruncase);
                    if (!dr.IsDBNull(iFversruncase)) entity.Fversruncase = dr.GetString(iFversruncase);

                    int iFverscodigoencrip = dr.GetOrdinal(helper.Fverscodigoencrip);
                    if (!dr.IsDBNull(iFverscodigoencrip)) entity.Fverscodigoencrip = (byte[])dr.GetValue(iFverscodigoencrip);

                    int iFversfecmodificacion = dr.GetOrdinal(helper.Fversfecmodificacion);
                    if (!dr.IsDBNull(iFversfecmodificacion)) entity.Fversfecmodificacion = dr.GetDateTime(iFversfecmodificacion);
                }
            }

            return entity;
        }

        public List<CpFuentegamsDTO> List()
        {
            List<CpFuentegamsDTO> entitys = new List<CpFuentegamsDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            CpFuentegamsDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    if (entity.Ftegdefault == 1)
                        entity.Oficial = "Oficial";
                    int iFverscodi = dr.GetOrdinal(helper.Fverscodi);
                    if (!dr.IsDBNull(iFverscodi)) entity.Fverscodi = Convert.ToInt32(dr.GetValue(iFverscodi));
                    int iFversnum = dr.GetOrdinal(helper.Fversnum);
                    if (!dr.IsDBNull(iFversnum)) entity.Fversnum = Convert.ToInt32(dr.GetValue(iFversnum));
                    int iFversdescrip = dr.GetOrdinal(helper.Fversdescrip);
                    if (!dr.IsDBNull(iFversdescrip)) entity.Fversdescrip = dr.GetString(iFversdescrip);
                    int iFversinputdata = dr.GetOrdinal(helper.Fversinputdata);
                    if (!dr.IsDBNull(iFversinputdata)) entity.Fversinputdata = dr.GetString(iFversinputdata);

                    int iFversruncase = dr.GetOrdinal(helper.Fversruncase);
                    if (!dr.IsDBNull(iFversruncase)) entity.Fversruncase = dr.GetString(iFversruncase);

                    int iFverscodigoencrip = dr.GetOrdinal(helper.Fverscodigoencrip);
                    if (!dr.IsDBNull(iFverscodigoencrip)) entity.Fverscodigoencrip = (byte[])dr.GetValue(iFverscodigoencrip);

                    int iFversfecmodificacion = dr.GetOrdinal(helper.Fversfecmodificacion);
                    if (!dr.IsDBNull(iFversfecmodificacion)) entity.Fversfecmodificacion = dr.GetDateTime(iFversfecmodificacion);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public List<CpFuentegamsDTO> GetByCriteria()
        {
            List<CpFuentegamsDTO> entitys = new List<CpFuentegamsDTO>();
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

        public void ResetOficial()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlResetOficial);
            dbProvider.ExecuteNonQuery(command);
        }

        public void SetOficial(int ftegcodi)
        {
            string strSql = string.Format(helper.SqlOficial, ftegcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strSql);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
