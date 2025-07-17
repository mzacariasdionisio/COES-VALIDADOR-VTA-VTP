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
    /// Clase de acceso a datos de la tabla PF_REPORTE_DET
    /// </summary>
    public class PfReporteDetRepository : RepositoryBase, IPfReporteDetRepository
    {
        public PfReporteDetRepository(string strConn) : base(strConn)
        {
        }

        PfReporteDetHelper helper = new PfReporteDetHelper();

        public int Save(PfReporteDetDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdetcodi, DbType.Int32, id));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pftotcodi, DbType.Int32, entity.Pftotcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdettipo, DbType.Int32, entity.Pfdettipo));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdetfechaini, DbType.DateTime, entity.Pfdetfechaini));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdetfechafin, DbType.DateTime, entity.Pfdetfechafin));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdetenergia, DbType.Decimal, entity.Pfdetenergia));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Pfdetnumdiapoc, DbType.Int32, entity.Pfdetnumdiapoc));

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfReporteDetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pfdetcodi, DbType.Int32, entity.Pfdetcodi);
            dbProvider.AddInParameter(command, helper.Pftotcodi, DbType.Int32, entity.Pftotcodi);
            dbProvider.AddInParameter(command, helper.Pfdettipo, DbType.Int32, entity.Pfdettipo);
            dbProvider.AddInParameter(command, helper.Pfdetfechaini, DbType.DateTime, entity.Pfdetfechaini);
            dbProvider.AddInParameter(command, helper.Pfdetfechafin, DbType.DateTime, entity.Pfdetfechafin);
            dbProvider.AddInParameter(command, helper.Pfdetenergia, DbType.Decimal, entity.Pfdetenergia);
            dbProvider.AddInParameter(command, helper.Pfdetnumdiapoc, DbType.Int32, entity.Pfdetnumdiapoc);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfdetcodi, DbType.Int32, pfdetcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfReporteDetDTO GetById(int pfdetcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfdetcodi, DbType.Int32, pfdetcodi);
            PfReporteDetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfReporteDetDTO> List()
        {
            List<PfReporteDetDTO> entitys = new List<PfReporteDetDTO>();
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

        public List<PfReporteDetDTO> GetByCriteria(int pfrptcodi)
        {
            List<PfReporteDetDTO> entitys = new List<PfReporteDetDTO>();

            string query = string.Format(helper.SqlGetByCriteria, pfrptcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
