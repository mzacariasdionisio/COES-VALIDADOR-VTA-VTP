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
    /// Clase de acceso a datos de la tabla EQ_CIRCUITO_DET
    /// </summary>
    public class EqCircuitoDetRepository : RepositoryBase, IEqCircuitoDetRepository
    {
        public EqCircuitoDetRepository(string strConn) : base(strConn)
        {
        }

        EqCircuitoDetHelper helper = new EqCircuitoDetHelper();

        public int Save(EqCircuitoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Circdtestado, DbType.Int32, entity.Circdtagrup);
            dbProvider.AddInParameter(command, helper.Circdtestado, DbType.Int32, entity.Circdtestado);
            dbProvider.AddInParameter(command, helper.Circdtfecvigencia, DbType.DateTime, entity.Circdtfecvigencia);
            dbProvider.AddInParameter(command, helper.Circdtfecmodificacion, DbType.DateTime, entity.Circdtfecmodificacion);
            dbProvider.AddInParameter(command, helper.Circdtusumodificacion, DbType.String, entity.Circdtusumodificacion);
            dbProvider.AddInParameter(command, helper.Circdtfeccreacion, DbType.DateTime, entity.Circdtfeccreacion);
            dbProvider.AddInParameter(command, helper.Circdtusucreacion, DbType.String, entity.Circdtusucreacion);
            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, entity.Circodi);
            dbProvider.AddInParameter(command, helper.Equicodihijo, DbType.Int32, entity.Equicodihijo);
            dbProvider.AddInParameter(command, helper.Circodihijo, DbType.Int32, entity.Circodihijo);
            dbProvider.AddInParameter(command, helper.Circdtcodi, DbType.Int32, id);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqCircuitoDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Circdtestado, DbType.Int32, entity.Circdtagrup);
            dbProvider.AddInParameter(command, helper.Circdtestado, DbType.Int32, entity.Circdtestado);
            dbProvider.AddInParameter(command, helper.Circdtfecvigencia, DbType.DateTime, entity.Circdtfecvigencia);
            dbProvider.AddInParameter(command, helper.Circdtfecmodificacion, DbType.DateTime, entity.Circdtfecmodificacion);
            dbProvider.AddInParameter(command, helper.Circdtusumodificacion, DbType.String, entity.Circdtusumodificacion);
            dbProvider.AddInParameter(command, helper.Circdtfeccreacion, DbType.DateTime, entity.Circdtfeccreacion);
            dbProvider.AddInParameter(command, helper.Circdtusucreacion, DbType.String, entity.Circdtusucreacion);
            dbProvider.AddInParameter(command, helper.Circodi, DbType.Int32, entity.Circodi);
            dbProvider.AddInParameter(command, helper.Equicodihijo, DbType.Int32, entity.Equicodihijo);
            dbProvider.AddInParameter(command, helper.Circodihijo, DbType.Int32, entity.Circodihijo);
            dbProvider.AddInParameter(command, helper.Circdtcodi, DbType.Int32, entity.Circdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int circdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Circdtcodi, DbType.Int32, circdtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqCircuitoDetDTO GetById(int circdtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Circdtcodi, DbType.Int32, circdtcodi);
            EqCircuitoDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EqCircuitoDetDTO> List()
        {
            List<EqCircuitoDetDTO> entitys = new List<EqCircuitoDetDTO>();
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

        public List<EqCircuitoDetDTO> GetByCriteria(int circodi, string listaEquicodi, int estado)
        {
            List<EqCircuitoDetDTO> entitys = new List<EqCircuitoDetDTO>();
            string query = string.Format(helper.SqlGetByCriteria, circodi, listaEquicodi, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCircnomb = dr.GetOrdinal(helper.Circnombhijo);
                    if (!dr.IsDBNull(iCircnomb)) entity.Circnombhijo = dr.GetString(iCircnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinombhijo);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinombhijo = dr.GetString(iEquinomb);

                    int iEmprnombequihijo = dr.GetOrdinal(helper.Emprnombequihijo);
                    if (!dr.IsDBNull(iEmprnombequihijo)) entity.Emprnombequihijo = dr.GetString(iEmprnombequihijo);

                    int iEmprnombcirchijo = dr.GetOrdinal(helper.Emprnombcirchijo);
                    if (!dr.IsDBNull(iEmprnombcirchijo)) entity.Emprnombcirchijo = dr.GetString(iEmprnombcirchijo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region MCP

        public List<EqCircuitoDetDTO> ObtenerListaCircuitosDet(string circodis)
        {
            List<EqCircuitoDetDTO> entitys = new List<EqCircuitoDetDTO>();
            string sqlQuery = string.Format(this.helper.SqlListarPorCircodis, circodis);
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

        public List<EqCircuitoDetDTO> GetDependientesAgrupados(int equicodi)
        {
            List<EqCircuitoDetDTO> entitys = new List<EqCircuitoDetDTO>();
            string sqlQuery = string.Format(this.helper.SqlListarDependientesAgrupados, equicodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquicodiAsociado = dr.GetOrdinal(helper.EquicodiAsociado);
                    if (!dr.IsDBNull(iEquicodiAsociado)) entity.EquicodiAsociado = dr.GetInt32(iEquicodiAsociado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
