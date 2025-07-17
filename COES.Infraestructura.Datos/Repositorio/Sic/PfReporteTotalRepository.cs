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
    /// Clase de acceso a datos de la tabla PF_REPORTE_TOTAL
    /// </summary>
    public class PfReporteTotalRepository : RepositoryBase, IPfReporteTotalRepository
    {
        public PfReporteTotalRepository(string strConn) : base(strConn)
        {
        }

        PfReporteTotalHelper helper = new PfReporteTotalHelper();

        public int Save(PfReporteTotalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotpe, DbType.Decimal, entity.Pftotpe));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotenergia, DbType.Decimal, entity.Pftotenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotminsincu, DbType.Decimal, entity.Pftotminsincu));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotpprom, DbType.Decimal, entity.Pftotpprom));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotpf, DbType.Decimal, entity.Pftotpf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotfi, DbType.Decimal, entity.Pftotfi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotfp, DbType.Decimal, entity.Pftotfp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotpg, DbType.Decimal, entity.Pftotpg));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfescecodi, DbType.Int32, entity.Pfescecodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotincremental, DbType.Int32, entity.Pftotincremental));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotunidadnomb, DbType.String, entity.Pftotunidadnomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotnumdiapoc, DbType.Int32, entity.Pftotnumdiapoc));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfReporteTotalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pftotcodi, DbType.Int32, entity.Pftotcodi);
            dbProvider.AddInParameter(command, helper.Pftotpe, DbType.Decimal, entity.Pftotpe);
            dbProvider.AddInParameter(command, helper.Pftotenergia, DbType.Decimal, entity.Pftotenergia);
            dbProvider.AddInParameter(command, helper.Pftotminsincu, DbType.Decimal, entity.Pftotminsincu);
            dbProvider.AddInParameter(command, helper.Pftotpprom, DbType.Decimal, entity.Pftotpprom);
            dbProvider.AddInParameter(command, helper.Pftotpf, DbType.Decimal, entity.Pftotpf);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Pftotfi, DbType.Decimal, entity.Pftotfi);
            dbProvider.AddInParameter(command, helper.Pftotfp, DbType.Decimal, entity.Pftotfp);
            dbProvider.AddInParameter(command, helper.Pftotpg, DbType.Decimal, entity.Pftotpg);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfescecodi, DbType.Int32, entity.Pfescecodi);
            dbProvider.AddInParameter(command, helper.Pftotincremental, DbType.Int32, entity.Pftotincremental);
            dbProvider.AddInParameter(command, helper.Pftotunidadnomb, DbType.String, entity.Pftotunidadnomb);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pftotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pftotcodi, DbType.Int32, pftotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfReporteTotalDTO GetById(int pftotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pftotcodi, DbType.Int32, pftotcodi);
            PfReporteTotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfReporteTotalDTO> List()
        {
            List<PfReporteTotalDTO> entitys = new List<PfReporteTotalDTO>();
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

        public List<PfReporteTotalDTO> GetByCriteria(string pfrptcodi)
        {
            List<PfReporteTotalDTO> entitys = new List<PfReporteTotalDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, pfrptcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PfReporteTotalDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iPfrperanio = dr.GetOrdinal(helper.Pfperianio);
                    if (!dr.IsDBNull(iPfrperanio)) entity.Pfperianio = Convert.ToInt32(dr.GetValue(iPfrperanio));

                    int iPfrpermes = dr.GetOrdinal(helper.Pfperimes);
                    if (!dr.IsDBNull(iPfrpermes)) entity.Pfperimes = Convert.ToInt32(dr.GetValue(iPfrpermes));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
