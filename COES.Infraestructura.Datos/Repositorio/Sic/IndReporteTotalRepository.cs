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
    /// Clase de acceso a datos de la tabla IND_REPORTE_TOTAL
    /// </summary>
    public class IndReporteTotalRepository : RepositoryBase, IIndReporteTotalRepository
    {
        public IndReporteTotalRepository(string strConn) : base(strConn)
        {
        }

        IndReporteTotalHelper helper = new IndReporteTotalHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(IndReporteTotalDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcodi, DbType.Int32, entity.Itotcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotunidadnomb, DbType.String, entity.Itotunidadnomb));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotopcom, DbType.String, entity.Itotopcom));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotincremental, DbType.Int32, entity.Itotincremental));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotminip, DbType.Decimal, entity.Itotminip));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotminif, DbType.Decimal, entity.Itotminif));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotminipparcial, DbType.Decimal, entity.Itotminipparcial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotminifparcial, DbType.Decimal, entity.Itotminifparcial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotpe, DbType.Decimal, entity.Itotpe));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfactork, DbType.Decimal, entity.Itotfactork));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfactorif, DbType.Decimal, entity.Itotfactorif));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfactoripm, DbType.Decimal, entity.Itotfactoripm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfactoripa, DbType.Decimal, entity.Itotfactoripa));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcr, DbType.String, entity.Itotcr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotindmas15d, DbType.String, entity.Itotindmas15d));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotinddiasxmes, DbType.Int32, entity.Itotinddiasxmes));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfactorpresm, DbType.Decimal, entity.Itotfactorpresm));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotnumho, DbType.Decimal, entity.Itotnumho));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotnumarranq, DbType.Int32, entity.Itotnumarranq));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotdescadic, DbType.String, entity.Itotdescadic));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotjustf, DbType.String, entity.Itotjustf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcodiold, DbType.Int32, entity.Itotcodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itottipocambio, DbType.String, entity.Itottipocambio));
            //INICIO: IND.PR25.2022
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotpcm3, DbType.Decimal, entity.Itotpcm3));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itot1ltvalor, DbType.Decimal, entity.Itot1ltvalor));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itot1ltunidad, DbType.String, entity.Itot1ltunidad));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfgte, DbType.Decimal, entity.Itotfgte));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotfrc, DbType.Decimal, entity.Itotfrc));

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotconsval, DbType.Int32, entity.Itotconsval));
            //FIN: IND.PR25.2022

            command.ExecuteNonQuery();
            return entity.Itotcodi;
        }

        public void Delete(int itotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Itotcodi, DbType.Int32, itotcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndReporteTotalDTO GetById(int itotcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Itotcodi, DbType.Int32, itotcodi);
            IndReporteTotalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndReporteTotalDTO> List()
        {
            List<IndReporteTotalDTO> entitys = new List<IndReporteTotalDTO>();
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

        public List<IndReporteTotalDTO> GetByCriteria(string irptcodi)
        {
            List<IndReporteTotalDTO> entitys = new List<IndReporteTotalDTO>();

            string query = string.Format(helper.SqlGetByCriteria, irptcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndReporteTotalDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iIrecafechaini = dr.GetOrdinal(helper.Irecafechaini);
                    if (!dr.IsDBNull(iIrecafechaini)) entity.Irecafechaini = dr.GetDateTime(iIrecafechaini);

                    int iIrecafechafin = dr.GetOrdinal(helper.Irecafechafin);
                    if (!dr.IsDBNull(iIrecafechafin)) entity.Irecafechafin = dr.GetDateTime(iIrecafechafin);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //Assetec [IND.PR25.2022]
        public List<IndReporteTotalDTO> ListConservarValorByPeriodoCuadro(int icuacodi, int ipericodi)
        {
            IndReporteTotalDTO entity = new IndReporteTotalDTO();
            List<IndReporteTotalDTO> entitys = new List<IndReporteTotalDTO>();
            string query = string.Format(helper.SqlListConservarValorByPeriodoCuadro, icuacodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndReporteTotalDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iItotpe = dr.GetOrdinal(helper.Itotpe);
                    if (!dr.IsDBNull(iItotpe)) entity.Itotpe = dr.GetDecimal(iItotpe);

                    int iItotfactork = dr.GetOrdinal(helper.Itotfactork);
                    if (!dr.IsDBNull(iItotfactork)) entity.Itotfactork = dr.GetDecimal(iItotfactork);

                    int iItotdescadic = dr.GetOrdinal(helper.Itotdescadic);
                    if (!dr.IsDBNull(iItotdescadic)) entity.Itotdescadic = dr.GetString(iItotdescadic);

                    int iItotjustf = dr.GetOrdinal(helper.Itotjustf);
                    if (!dr.IsDBNull(iItotjustf)) entity.Itotjustf = dr.GetString(iItotjustf);

                    int iItotconsval = dr.GetOrdinal(helper.Itotconsval);
                    if (!dr.IsDBNull(iItotconsval)) entity.Itotconsval = Convert.ToInt32(dr.GetValue(iItotconsval));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
