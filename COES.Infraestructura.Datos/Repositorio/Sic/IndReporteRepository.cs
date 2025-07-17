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
    /// Clase de acceso a datos de la tabla IND_REPORTE
    /// </summary>
    public class IndReporteRepository : RepositoryBase, IIndReporteRepository
    {
        public IndReporteRepository(string strConn) : base(strConn)
        {
        }

        IndReporteHelper helper = new IndReporteHelper();

        public int Save(IndReporteDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irecacodi, DbType.Int32, entity.Irecacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Icuacodi, DbType.Int32, entity.Icuacodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptestado, DbType.String, entity.Irptestado));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpttipo, DbType.String, entity.Irpttipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irpttiempo, DbType.String, entity.Irpttiempo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptmedicionorigen, DbType.String, entity.Irptmedicionorigen));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptnumversion, DbType.Int32, entity.Irptnumversion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptesfinal, DbType.Int32, entity.Irptesfinal));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptreporteold, DbType.Int32, entity.Irptreporteold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptusucreacion, DbType.String, entity.Irptusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfeccreacion, DbType.DateTime, entity.Irptfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptusumodificacion, DbType.String, entity.Irptusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Irptfecmodificacion, DbType.DateTime, entity.Irptfecmodificacion));

            command.ExecuteNonQuery();
            return id;
        }

        public void UpdateAprobar(IndReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateAprobar);

            dbProvider.AddInParameter(command, helper.Irptesfinal, DbType.Int32, entity.Irptesfinal);
            dbProvider.AddInParameter(command, helper.Irptusumodificacion, DbType.String, entity.Irptusumodificacion);
            dbProvider.AddInParameter(command, helper.Irptfecmodificacion, DbType.DateTime, entity.Irptfecmodificacion);

            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateHistorico(IndReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateHistorico);

            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, entity.Irptcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int irptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, irptcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndReporteDTO GetById(int irptcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Irptcodi, DbType.Int32, irptcodi);
            IndReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iIperinombre = dr.GetOrdinal(helper.Iperinombre);
                    if (!dr.IsDBNull(iIperinombre)) entity.Iperinombre = dr.GetString(iIperinombre);

                    int iIrecanombre = dr.GetOrdinal(helper.Irecanombre);
                    if (!dr.IsDBNull(iIrecanombre)) entity.Irecanombre = dr.GetString(iIrecanombre);

                    int iIrecafechaini = dr.GetOrdinal(helper.Irecafechaini);
                    if (!dr.IsDBNull(iIrecafechaini)) entity.Irecafechaini = dr.GetDateTime(iIrecafechaini);

                    int iIrecafechafin = dr.GetOrdinal(helper.Irecafechafin);
                    if (!dr.IsDBNull(iIrecafechafin)) entity.Irecafechafin = dr.GetDateTime(iIrecafechafin);

                    int iIrecainforme = dr.GetOrdinal(helper.Irecainforme);
                    if (!dr.IsDBNull(iIrecainforme)) entity.Irecainforme = dr.GetString(iIrecainforme);

                }
            }

            return entity;
        }

        public List<IndReporteDTO> List()
        {
            List<IndReporteDTO> entitys = new List<IndReporteDTO>();
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

        public List<IndReporteDTO> GetByCriteria(int icuacodi, int irecacodi, string irptcodis)
        {
            List<IndReporteDTO> entitys = new List<IndReporteDTO>();

            string query = string.Format(helper.SqlGetByCriteria, irecacodi, icuacodi, irptcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndReporteDTO entity = helper.Create(dr);

                    int iIperinombre = dr.GetOrdinal(helper.Iperinombre);
                    if (!dr.IsDBNull(iIperinombre)) entity.Iperinombre = dr.GetString(iIperinombre);

                    int iIrecanombre = dr.GetOrdinal(helper.Irecanombre);
                    if (!dr.IsDBNull(iIrecanombre)) entity.Irecanombre = dr.GetString(iIrecanombre);

                    int iIrecafechaini = dr.GetOrdinal(helper.Irecafechaini);
                    if (!dr.IsDBNull(iIrecafechaini)) entity.Irecafechaini = dr.GetDateTime(iIrecafechaini);

                    int iIrecafechafin = dr.GetOrdinal(helper.Irecafechafin);
                    if (!dr.IsDBNull(iIrecafechafin)) entity.Irecafechafin = dr.GetDateTime(iIrecafechafin);

                    int iIrecainforme = dr.GetOrdinal(helper.Irecainforme);
                    if (!dr.IsDBNull(iIrecainforme)) entity.Irecainforme = dr.GetString(iIrecainforme);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Devuelve el reporte para PFR (factor k, CR en hidro y CR en termo), todo para el periodo anterior
        /// </summary>
        /// <param name="indrecacodiant"></param>
        /// <param name="esVersionValidado"></param>
        /// <param name="reportePR25Cuadro3FactorK"></param>
        /// <param name="reportePR25FactorProgTermico"></param>
        /// <param name="reportePR25FactorProgHidro"></param>
        /// <returns></returns>
        public List<IndReporteDTO> ListadoReportesparaPFR(int indrecacodiant, int esVersionValidado, int reportePR25Cuadro3FactorK, int reportePR25FactorProgTermico, int reportePR25FactorProgHidro)
        {
            List<IndReporteDTO> entitys = new List<IndReporteDTO>();
            string query = string.Format(helper.SqlReporteParaPFR, indrecacodiant, esVersionValidado, reportePR25Cuadro3FactorK, reportePR25FactorProgTermico, reportePR25FactorProgHidro);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
