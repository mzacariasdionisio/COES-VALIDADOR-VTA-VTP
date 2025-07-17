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
    /// Clase de acceso a datos de la tabla IND_REPORTE_DET
    /// </summary>
    public class IndReporteDetRepository : RepositoryBase, IIndReporteDetRepository
    {
        public IndReporteDetRepository(string strConn) : base(strConn)
        {
        }

        IndReporteDetHelper helper = new IndReporteDetHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public int Save(IndReporteDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetcodi, DbType.Int32, entity.Idetcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Itotcodi, DbType.Int32, entity.Itotcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetopcom, DbType.String, entity.Idetopcom));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetincremental, DbType.Int32, entity.Idetincremental));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetdia, DbType.Int32, entity.Idetdia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idettipoindisp, DbType.String, entity.Idettipoindisp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idettieneexc, DbType.String, entity.Idettieneexc));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idethoraini, DbType.DateTime, entity.Idethoraini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idethorafin, DbType.DateTime, entity.Idethorafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetmin, DbType.Int32, entity.Idetmin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetmw, DbType.Decimal, entity.Idetmw));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetpr, DbType.Decimal, entity.Idetpr));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetminparcial, DbType.Decimal, entity.Idetminparcial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetminifparcial, DbType.Decimal, entity.Idetminifparcial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetminipparcial, DbType.Decimal, entity.Idetminipparcial));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idettienedisp, DbType.Int32, entity.Idettienedisp));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetfactork, DbType.Decimal, entity.Idetfactork));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetpe, DbType.Decimal, entity.Idetpe));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetpa, DbType.Decimal, entity.Idetpa));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetminif, DbType.Decimal, entity.Idetminif));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetminip, DbType.Decimal, entity.Idetminip));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetnumho, DbType.Decimal, entity.Idetnumho));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetnumarranq, DbType.Int32, entity.Idetnumarranq));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetfechainifort7d, DbType.DateTime, entity.Idetfechainifort7d));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetfechafinfort7d, DbType.DateTime, entity.Idetfechafinfort7d));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetfechainiprog7d, DbType.DateTime, entity.Idetfechainiprog7d));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetfechafinprog7d, DbType.DateTime, entity.Idetfechafinprog7d));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetdescadic, DbType.String, entity.Idetdescadic));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetjustf, DbType.String, entity.Idetjustf));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetcodiold, DbType.Int32, entity.Idetcodiold));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idettipocambio, DbType.String, entity.Idettipocambio));
            //Se agrega nuevo campo -Assetec (RAC)
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Idetconsval, DbType.Int32, entity.Idetconsval));

            command.ExecuteNonQuery();
            return entity.Idetcodi;
        }

        public void Delete(int idetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Idetcodi, DbType.Int32, idetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndReporteDetDTO GetById(int idetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Idetcodi, DbType.Int32, idetcodi);
            IndReporteDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndReporteDetDTO> List()
        {
            List<IndReporteDetDTO> entitys = new List<IndReporteDetDTO>();
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

        public List<IndReporteDetDTO> GetByCriteria(string irptcodi)
        {
            List<IndReporteDetDTO> entitys = new List<IndReporteDetDTO>();

            string query = string.Format(helper.SqlGetByCriteria, irptcodi);
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

        //Assetec [IND.PR25.2022]
        public List<IndReporteDetDTO> ListConservarValorByPeriodoCuadro(int icuacodi, int ipericodi)
        {
            IndReporteDetDTO entity = new IndReporteDetDTO();
            List<IndReporteDetDTO> entitys = new List<IndReporteDetDTO>();
            string query = string.Format(helper.SqlListConservarValorByPeriodoCuadro, icuacodi, ipericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndReporteDetDTO();

                    int iIdetcodi = dr.GetOrdinal(helper.Idetcodi);
                    if (!dr.IsDBNull(iIdetcodi)) entity.Idetcodi = Convert.ToInt32(dr.GetValue(iIdetcodi));

                    int iIdetdia= dr.GetOrdinal(helper.Idetdia);
                    if (!dr.IsDBNull(iIdetdia)) entity.Idetdia = Convert.ToInt32(dr.GetValue(iIdetdia));

                    int iIdettipoindisp = dr.GetOrdinal(helper.Idettipoindisp);
                    if (!dr.IsDBNull(iIdettipoindisp)) entity.Idettipoindisp = dr.GetString(iIdettipoindisp);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iIdetopcom = dr.GetOrdinal(helper.Idetopcom);
                    if (!dr.IsDBNull(iIdetopcom)) entity.Idetopcom = dr.GetString(iIdetopcom);

                    int iIdetpr = dr.GetOrdinal(helper.Idetpr);
                    if (!dr.IsDBNull(iIdetpr)) entity.Idetpr = dr.GetDecimal(iIdetpr);

                    int iIdetminparcial = dr.GetOrdinal(helper.Idetminparcial);
                    if (!dr.IsDBNull(iIdetminparcial)) entity.Idetminparcial = dr.GetDecimal(iIdetminparcial);

                    int iIdethorafin = dr.GetOrdinal(helper.Idethorafin);
                    if (!dr.IsDBNull(iIdethorafin)) entity.Idethorafin = dr.GetDateTime(iIdethorafin);

                    int iIdethoraini = dr.GetOrdinal(helper.Idethoraini);
                    if (!dr.IsDBNull(iIdethoraini)) entity.Idethoraini = dr.GetDateTime(iIdethoraini);

                    int iIdetjustf = dr.GetOrdinal(helper.Idetjustf);
                    if (!dr.IsDBNull(iIdetjustf)) entity.Idetjustf = dr.GetString(iIdetjustf);

                    int iIdetconsval = dr.GetOrdinal(helper.Idetconsval);
                    if (!dr.IsDBNull(iIdetconsval)) entity.Idetconsval = Convert.ToInt32(dr.GetValue(iIdetconsval));

                    int iIdettienedisp = dr.GetOrdinal(helper.Idettienedisp);
                    if (!dr.IsDBNull(iIdettienedisp)) entity.Idettienedisp = Convert.ToInt32(dr.GetValue(iIdettienedisp));

                    //Assetec: ajuste del 20230503 - Incorpora el valor de Idetmin
                    int iIdetmin = dr.GetOrdinal(helper.Idetmin);
                    if (!dr.IsDBNull(iIdetmin)) entity.Idetmin = Convert.ToInt32(dr.GetValue(iIdetmin));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
