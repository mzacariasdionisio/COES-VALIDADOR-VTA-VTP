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
    /// Clase de acceso a datos de la tabla SI_HISEMPEQ_DATA
    /// </summary>
    public class SiHisempeqDataRepository : RepositoryBase, ISiHisempeqDataRepository
    {
        public SiHisempeqDataRepository(string strConn) : base(strConn)
        {
        }

        SiHisempeqDataHelper helper = new SiHisempeqDataHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempeqDataDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Heqdatfecha, DbType.DateTime, entity.Heqdatfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Heqdatestado, DbType.String, entity.Heqdatestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiold, DbType.Int32, entity.Equicodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodiactual, DbType.Int32, entity.Equicodiactual));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Heqdatusucreacion, DbType.String, entity.Heqdatusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Heqdatfeccreacion, DbType.DateTime, entity.Heqdatfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Heqdatcodi, DbType.Int32, entity.Heqdatcodi));

            command.ExecuteNonQuery();
            return entity.Heqdatcodi;
        }

        public void Update(SiHisempeqDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Heqdatfecha, DbType.DateTime, entity.Heqdatfecha);
            dbProvider.AddInParameter(command, helper.Heqdatestado, DbType.String, entity.Heqdatestado);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Equicodiold, DbType.Int32, entity.Equicodiold);
            dbProvider.AddInParameter(command, helper.Equicodiactual, DbType.Int32, entity.Equicodiactual);
            dbProvider.AddInParameter(command, helper.Heqdatusucreacion, DbType.String, entity.Heqdatusucreacion);
            dbProvider.AddInParameter(command, helper.Heqdatfeccreacion, DbType.DateTime, entity.Heqdatfeccreacion);

            dbProvider.AddInParameter(command, helper.Heqdatcodi, DbType.Int32, entity.Heqdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int heqdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Heqdatcodi, DbType.Int32, heqdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int heqdatcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Heqdatusucreacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Heqdatcodi, DbType.Int32, heqdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempeqDataDTO GetById(int heqdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Heqdatcodi, DbType.Int32, heqdatcodi);
            SiHisempeqDataDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempeqDataDTO> List(string equicodis)
        {
            List<SiHisempeqDataDTO> entitys = new List<SiHisempeqDataDTO>();

            string sql = string.Format(helper.SqlList, equicodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempeqDataDTO> GetByCriteria()
        {
            List<SiHisempeqDataDTO> entitys = new List<SiHisempeqDataDTO>();

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

        // métodos anular transferencia
        public void DeleteXAnulacionMigra(List<int> equipos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran)
        {
            int Emprcodi = emprcodi1;
            string listaEquipos = string.Join(",", equipos);
            List<int> empresas = new List<int> { emprcodi1, emprcodi2 };
            string listaEmpresas = string.Join(",", empresas);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlDeleteXAnulacionMigra, listaEquipos, listaEmpresas, fechaCorte.ToString(ConstantesBase.FormatoFecha));
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();

        }

        //update CODIGO EQUIPO ACTUAL
        public void UpdateEquipoActual(int equicodiactual, int equicodiold, int equipoAnterior, IDbConnection conn, DbTransaction tran)
        {
            List<int> equipos = new List<int> { equicodiold, equipoAnterior };
            string listaEquipos = string.Join(",", equipos);


            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateEquipoActual, equicodiactual, listaEquipos);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

    }
}