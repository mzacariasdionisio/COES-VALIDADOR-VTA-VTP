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
    /// Clase de acceso a datos de la tabla SI_HISEMPPTO_DATA
    /// </summary>
    public class SiHisempptoDataRepository : RepositoryBase, ISiHisempptoDataRepository
    {
        public SiHisempptoDataRepository(string strConn) : base(strConn)
        {
        }

        SiHisempptoDataHelper helper = new SiHisempptoDataHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempptoDataDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hptdatfecha, DbType.DateTime, entity.Hptdatfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hptdatptoestado, DbType.String, entity.Hptdatptoestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodiold, DbType.Int32, entity.Ptomedicodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Ptomedicodiactual, DbType.Int32, entity.Ptomedicodiactual));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hptdatusucreacion, DbType.String, entity.Hptdatusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hptdatfeccreacion, DbType.DateTime, entity.Hptdatfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hptdatcodi, DbType.Int32, entity.Hptdatcodi));

            command.ExecuteNonQuery();
            return entity.Hptdatcodi;
        }

        public void Update(SiHisempptoDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hptdatfecha, DbType.DateTime, entity.Hptdatfecha);
            dbProvider.AddInParameter(command, helper.Hptdatptoestado, DbType.String, entity.Hptdatptoestado);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodiold, DbType.Int32, entity.Ptomedicodiold);
            dbProvider.AddInParameter(command, helper.Ptomedicodiactual, DbType.Int32, entity.Ptomedicodiactual);
            dbProvider.AddInParameter(command, helper.Hptdatusucreacion, DbType.String, entity.Hptdatusucreacion);
            dbProvider.AddInParameter(command, helper.Hptdatfeccreacion, DbType.DateTime, entity.Hptdatfeccreacion);

            dbProvider.AddInParameter(command, helper.Hptdatcodi, DbType.Int32, entity.Hptdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hptdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hptdatcodi, DbType.Int32, hptdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int hptdatcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Hptdatusucreacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Hptdatcodi, DbType.Int32, hptdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempptoDataDTO GetById(int hptdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hptdatcodi, DbType.Int32, hptdatcodi);
            SiHisempptoDataDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempptoDataDTO> List(string ptomedicodis)
        {
            List<SiHisempptoDataDTO> entitys = new List<SiHisempptoDataDTO>();

            string sql = string.Format(helper.SqlList, ptomedicodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iPtomedidesc = dr.GetOrdinal(this.helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempptoDataDTO> GetByCriteria()
        {
            List<SiHisempptoDataDTO> entitys = new List<SiHisempptoDataDTO>();
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
        public void DeleteXAnulacionMigra(List<int> puntos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran)
        {
            int Emprcodi = emprcodi1;
            string listaPuntos = string.Join(",", puntos);
            List<int> empresas = new List<int> { emprcodi1, emprcodi2 };
            string listaEmpresas = string.Join(",", empresas);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlDeleteXAnulacionMigra, listaPuntos, listaEmpresas, fechaCorte.ToString(ConstantesBase.FormatoFecha));
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        //update CODIGO EQUIPO ACTUAL
        public void UpdatePuntoActual(int ptomedicodiactual, int ptomedicodiold, int puntoAnterior, IDbConnection conn, DbTransaction tran)
        {
            List<int> puntos = new List<int> { ptomedicodiold, puntoAnterior };
            string listaPuntos = string.Join(",", puntos);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdatePuntoActual, ptomedicodiactual, listaPuntos);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }
    }
}