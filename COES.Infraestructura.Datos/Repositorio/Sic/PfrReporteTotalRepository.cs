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
    /// Clase de acceso a datos de la tabla PFR_REPORTE_TOTAL
    /// </summary>
    public class PfrReporteTotalRepository: RepositoryBase, IPfrReporteTotalRepository
    {
        public PfrReporteTotalRepository(string strConn): base(strConn)
        {
        }

        PfrReporteTotalHelper helper = new PfrReporteTotalHelper();  

        public int Save(PfrReporteTotalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;
            
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotunidadnomb, DbType.String, entity.Pfrtotunidadnomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotcv, DbType.Decimal, entity.Pfrtotcv));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpe, DbType.Decimal, entity.Pfrtotpe));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpea, DbType.Decimal, entity.Pfrtotpea));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotfi, DbType.Decimal, entity.Pfrtotfi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpf, DbType.Decimal, entity.Pfrtotpf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpfc, DbType.Decimal, entity.Pfrtotpfc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpd, DbType.Decimal, entity.Pfrtotpd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotcvf, DbType.Decimal, entity.Pfrtotcvf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpdd, DbType.Decimal, entity.Pfrtotpdd));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotpfr, DbType.Decimal, entity.Pfrtotpfr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotcrmesant, DbType.Int32, entity.Pfrtotcrmesant));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotfkmesant, DbType.Decimal, entity.Pfrtotfkmesant));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfrtotficticio, DbType.Int32, entity.Pfrtotficticio));
            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfrReporteTotalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfrtotcodi, DbType.Int32, entity.Pfrtotcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pfrtotunidadnomb, DbType.String, entity.Pfrtotunidadnomb);
            dbProvider.AddInParameter(command, helper.Pfresccodi, DbType.Int32, entity.Pfresccodi);
            dbProvider.AddInParameter(command, helper.Pfrtotcv, DbType.Decimal, entity.Pfrtotcv);
            dbProvider.AddInParameter(command, helper.Pfrtotpe, DbType.Decimal, entity.Pfrtotpe);
            dbProvider.AddInParameter(command, helper.Pfrtotpea, DbType.Decimal, entity.Pfrtotpea);
            dbProvider.AddInParameter(command, helper.Pfrtotfi, DbType.Decimal, entity.Pfrtotfi);
            dbProvider.AddInParameter(command, helper.Pfrtotpf, DbType.Decimal, entity.Pfrtotpf);
            dbProvider.AddInParameter(command, helper.Pfrtotpfc, DbType.Decimal, entity.Pfrtotpfc);
            dbProvider.AddInParameter(command, helper.Pfrtotpd, DbType.Decimal, entity.Pfrtotpd);
            dbProvider.AddInParameter(command, helper.Pfrtotcvf, DbType.Decimal, entity.Pfrtotcvf);
            dbProvider.AddInParameter(command, helper.Pfrtotpdd, DbType.Decimal, entity.Pfrtotpdd);
            dbProvider.AddInParameter(command, helper.Pfrtotpfr, DbType.Decimal, entity.Pfrtotpfr);
			dbProvider.AddInParameter(command, helper.Pfrtotcrmesant, DbType.Int32, entity.Pfrtotcrmesant);
            dbProvider.AddInParameter(command, helper.Pfrtotfkmesant, DbType.Decimal, entity.Pfrtotfkmesant);
            dbProvider.AddInParameter(command, helper.Pfrtotficticio, DbType.Int32, entity.Pfrtotficticio);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfrtotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfrtotcodi, DbType.Int32, pfrtotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfrReporteTotalDTO GetById(int pfrtotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfrtotcodi, DbType.Int32, pfrtotcodi);
            PfrReporteTotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfrReporteTotalDTO> List()
        {
            List<PfrReporteTotalDTO> entitys = new List<PfrReporteTotalDTO>();
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

        public List<PfrReporteTotalDTO> GetByCriteria()
        {
            List<PfrReporteTotalDTO> entitys = new List<PfrReporteTotalDTO>();
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
       

        public List<PfrReporteTotalDTO> ListByReportecodi(int pfrrptcodi)
        {
            List<PfrReporteTotalDTO> entitys = new List<PfrReporteTotalDTO>();

            string sql = string.Format(helper.SqlGetByReportecodi, pfrrptcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PfrReporteTotalDTO entity = helper.Create(dr);

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

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
