using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EQ_CIRCUITO
    /// </summary>
    public class EqCircuitoRepository : RepositoryBase, IEqCircuitoRepository
    {
        public EqCircuitoRepository(string strConn) : base(strConn)
        {
        }

        EqCircuitoHelper helper = new EqCircuitoHelper();

        public int Save(EqCircuitoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Circfecmodificacion, DbType.DateTime, entity.Circfecmodificacion);
            dbProvider.AddInParameter(command, helper.Circusumodificacion, DbType.String, entity.Circusumodificacion);
            dbProvider.AddInParameter(command, helper.Circfeccreacion, DbType.DateTime, entity.Circfeccreacion);
            dbProvider.AddInParameter(command, helper.Circusucreacion, DbType.String, entity.Circusucreacion);
            dbProvider.AddInParameter(command, helper.Circestado, DbType.Int32, entity.Circestado);
            dbProvider.AddInParameter(command, helper.Circnomb, DbType.String, entity.Circnomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqCircuitoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Circfecmodificacion, DbType.DateTime, entity.Circfecmodificacion);
            dbProvider.AddInParameter(command, helper.Circusumodificacion, DbType.String, entity.Circusumodificacion);
            dbProvider.AddInParameter(command, helper.Circfeccreacion, DbType.DateTime, entity.Circfeccreacion);
            dbProvider.AddInParameter(command, helper.Circusucreacion, DbType.String, entity.Circusucreacion);
            dbProvider.AddInParameter(command, helper.Circestado, DbType.Int32, entity.Circestado);
            dbProvider.AddInParameter(command, helper.Circnomb, DbType.String, entity.Circnomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, entity.Circodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int circodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, circodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int circodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Circusumodificacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, circodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqCircuitoDTO GetById(int circodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, circodi);
            EqCircuitoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    return entity;
                }
            }

            return entity;
        }

        public List<EqCircuitoDTO> List()
        {
            List<EqCircuitoDTO> entitys = new List<EqCircuitoDTO>();
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

        public List<EqCircuitoDTO> GetByCriteria(string emprcodi, string listaEquicodi, int estado)
        {
            List<EqCircuitoDTO> entitys = new List<EqCircuitoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, emprcodi, listaEquicodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqCircuitoDTO entity = helper.Create(dr);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEmprenomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprenomb)) entity.Emprnomb = dr.GetString(iEmprenomb);
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       

        public EqCircuitoDTO GetByEquicodi(int equicodi)
        {
            string queryString = string.Format(helper.SqlGetByEquicodi, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            EqCircuitoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }




        public List<EqCircuitoDTO> ObtenerListaCircuitos(string lstCircodis)
        {
            var entitys = new List<EqCircuitoDTO>();
            string queryString = string.Format(helper.SqlGetByCircodis, lstCircodis);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

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
