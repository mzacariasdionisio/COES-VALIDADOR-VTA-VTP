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
    /// Clase de acceso a datos de la tabla SI_PROCESO
    /// </summary>
    public class SiProcesoRepository : RepositoryBase, ISiProcesoRepository
    {
        public SiProcesoRepository(string strConn) : base(strConn)
        {
        }

        SiProcesoHelper helper = new SiProcesoHelper();

        public int Save(SiProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Prcsnomb, DbType.String, entity.Prcsnomb);
            dbProvider.AddInParameter(command, helper.Prcsestado, DbType.String, entity.Prcsestado);
            dbProvider.AddInParameter(command, helper.Prcsmetodo, DbType.String, entity.Prcsmetodo);
            dbProvider.AddInParameter(command, helper.Prscfrecuencia, DbType.String, entity.Prscfrecuencia);
            dbProvider.AddInParameter(command, helper.Prschorainicio, DbType.Int32, entity.Prschorainicio);
            dbProvider.AddInParameter(command, helper.Prscminutoinicio, DbType.Int32, entity.Prscminutoinicio);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Prscbloque, DbType.Int32, entity.Prscbloque);


            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void ExecIndicadores(DateTime fechaProceso, int gps)
        {
            // DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //DbCommand command = dbProvider.GetStoredProcCommand(helper.SpIndicador);
            DbCommand command = dbProvider.GetStoredProcCommand("SP_FR_INDICADORES");

            dbProvider.AddInParameter(command, helper.pGPS, DbType.Int32, gps);
            dbProvider.AddInParameter(command, helper.pFecha, DbType.DateTime, fechaProceso);
            dbProvider.ExecuteNonQuery(command);
        }
        public void Update(SiProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Prcsnomb, DbType.String, entity.Prcsnomb);
            dbProvider.AddInParameter(command, helper.Prcsestado, DbType.String, entity.Prcsestado);
            dbProvider.AddInParameter(command, helper.Prcsmetodo, DbType.String, entity.Prcsmetodo);
            dbProvider.AddInParameter(command, helper.Prscfrecuencia, DbType.String, entity.Prscfrecuencia);
            dbProvider.AddInParameter(command, helper.Prschorainicio, DbType.Int32, entity.Prschorainicio);
            dbProvider.AddInParameter(command, helper.Prscminutoinicio, DbType.Int32, entity.Prscminutoinicio);
            dbProvider.AddInParameter(command, helper.Modcodi, DbType.Int32, entity.Modcodi);
            dbProvider.AddInParameter(command, helper.Prscbloque, DbType.Int32, entity.Prscbloque);
            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, entity.Prcscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int prcscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, prcscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiProcesoDTO GetById(int prcscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, prcscodi);
            SiProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiProcesoDTO> List()
        {
            List<SiProcesoDTO> entitys = new List<SiProcesoDTO>();
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

        public List<SiProcesoDTO> GetByCriteria()
        {
            List<SiProcesoDTO> entitys = new List<SiProcesoDTO>();
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

        public SiProcesoDTO ObtenerParametros(int idProceso)
        {
            SiProcesoDTO entity = null;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerParametros);
            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, idProceso);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iPathFile = dr.GetOrdinal(helper.PathFile);
                    if (!dr.IsDBNull(iPathFile)) entity.PathFile = dr.GetString(iPathFile);
                }
            }

            return entity;
        }

        public void ActualizarEstado(int idProceso, string estado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarEstado);
            dbProvider.AddInParameter(command, helper.Prcsestado, DbType.String, estado);
            dbProvider.AddInParameter(command, helper.Prcscodi, DbType.Int32, idProceso);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
