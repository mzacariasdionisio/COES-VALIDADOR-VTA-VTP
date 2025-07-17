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
    /// Clase de acceso a datos de la tabla ABI_PRODGENERACION
    /// </summary>
    public class AbiProdgeneracionRepository : RepositoryBase, IAbiProdgeneracionRepository
    {
        private string strConexion;
        public AbiProdgeneracionRepository(string strConn) : base(strConn)
        {
            strConexion = strConn;
        }

        AbiProdgeneracionHelper helper = new AbiProdgeneracionHelper();

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(AbiProdgeneracionDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgenfecmodificacion, DbType.DateTime, entity.Pgenfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgenusumodificacion, DbType.String, entity.Pgenusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgentipogenerrer, DbType.String, entity.Pgentipogenerrer));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgenintegrante, DbType.String, entity.Pgenintegrante));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgenvalor, DbType.Decimal, entity.Pgenvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgenfecha, DbType.DateTime, entity.Pgenfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pgencodi, DbType.Int32, entity.Pgencodi));

            command.ExecuteNonQuery();
            return entity.Pgencodi;
        }

        public void Update(AbiProdgeneracionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pgenfecmodificacion, DbType.DateTime, entity.Pgenfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pgenusumodificacion, DbType.String, entity.Pgenusumodificacion);
            dbProvider.AddInParameter(command, helper.Pgentipogenerrer, DbType.String, entity.Pgentipogenerrer);
            dbProvider.AddInParameter(command, helper.Pgenintegrante, DbType.String, entity.Pgenintegrante);
            dbProvider.AddInParameter(command, helper.Pgenvalor, DbType.Decimal, entity.Pgenvalor);
            dbProvider.AddInParameter(command, helper.Pgenfecha, DbType.DateTime, entity.Pgenfecha);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Tgenercodi, DbType.Int32, entity.Tgenercodi);
            dbProvider.AddInParameter(command, helper.Pgencodi, DbType.Int32, entity.Pgencodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteByRango(DateTime fechaIni, DateTime fechaFin, IDbConnection conn, DbTransaction tran)
        {
            string sql = String.Format(this.helper.SqlDeleteByRango, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = sql;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        public AbiProdgeneracionDTO GetById(int pgencodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pgencodi, DbType.Int32, pgencodi);
            AbiProdgeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AbiProdgeneracionDTO> List(DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER)
        {
            List<AbiProdgeneracionDTO> entitys = new List<AbiProdgeneracionDTO>();
            string sql = string.Format(helper.SqlList, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), flagIntegrante, flagRER);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AbiProdgeneracionDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string flagIntegrante, string flagRER)
        {
            List<AbiProdgeneracionDTO> entitys = new List<AbiProdgeneracionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), flagIntegrante, flagRER);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

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
