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
    /// Clase de acceso a datos de la tabla SI_HISEMPGRUPO_DATA
    /// </summary>
    public class SiHisempgrupoDataRepository : RepositoryBase, ISiHisempgrupoDataRepository
    {
        public SiHisempgrupoDataRepository(string strConn) : base(strConn)
        {
        }

        SiHisempgrupoDataHelper helper = new SiHisempgrupoDataHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(SiHisempgrupoDataDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hgrdatfecha, DbType.DateTime, entity.Hgrdatfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hgrdatestado, DbType.String, entity.Hgrdatestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodiold, DbType.Int32, entity.Grupocodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodiactual, DbType.Int32, entity.Grupocodiactual));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hgrdatusucreacion, DbType.String, entity.Hgrdatusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hgrdatfeccreacion, DbType.DateTime, entity.Hgrdatfeccreacion));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Hgrdatcodi, DbType.Int32, entity.Hgrdatcodi));

            command.ExecuteNonQuery();
            return entity.Hgrdatcodi;
        }

        public void Update(SiHisempgrupoDataDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hgrdatfecha, DbType.DateTime, entity.Hgrdatfecha);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hgrdatestado, DbType.String, entity.Hgrdatestado);
            dbProvider.AddInParameter(command, helper.Grupocodiold, DbType.Int32, entity.Grupocodiold);
            dbProvider.AddInParameter(command, helper.Grupocodiactual, DbType.Int32, entity.Grupocodiactual);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hgrdatusucreacion, DbType.String, entity.Hgrdatusucreacion);
            dbProvider.AddInParameter(command, helper.Hgrdatfeccreacion, DbType.DateTime, entity.Hgrdatfeccreacion);

            dbProvider.AddInParameter(command, helper.Hgrdatcodi, DbType.Int32, entity.Hgrdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hgrdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hgrdatcodi, DbType.Int32, hgrdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int hgrdatcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.Hgrdatusucreacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Hgrdatcodi, DbType.Int32, hgrdatcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiHisempgrupoDataDTO GetById(int hgrdatcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hgrdatcodi, DbType.Int32, hgrdatcodi);
            SiHisempgrupoDataDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiHisempgrupoDataDTO> List(string grupocodis)
        {
            List<SiHisempgrupoDataDTO> entitys = new List<SiHisempgrupoDataDTO>();

            string sql = string.Format(helper.SqlList, grupocodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiHisempgrupoDataDTO> GetByCriteria()
        {
            List<SiHisempgrupoDataDTO> entitys = new List<SiHisempgrupoDataDTO>();
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
        public void DeleteXAnulacionMigra(List<int> grupos, int emprcodi1, int emprcodi2, DateTime fechaCorte, IDbConnection conn, DbTransaction tran)
        {
            int Emprcodi = emprcodi1;
            string listaGrupos = string.Join(",", grupos);
            List<int> empresas = new List<int> { emprcodi1, emprcodi2 };
            string listaEmpresas = string.Join(",", empresas);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlDeleteXAnulacionMigra, listaGrupos, listaEmpresas, fechaCorte.ToString(ConstantesBase.FormatoFecha));
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }

        //update CODIGO EQUIPO ACTUAL
        public void UpdateGrupoActual(int grupocodiactual, int grupocodiold, int grupoAnterior, IDbConnection conn, DbTransaction tran)
        {
            List<int> grupos = new List<int> { grupocodiold, grupoAnterior };
            string listaGrupos = string.Join(",", grupos);

            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = string.Format(helper.SqlUpdateGrupoActual, grupocodiactual, listaGrupos);
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.ExecuteNonQuery();
        }
    }
}