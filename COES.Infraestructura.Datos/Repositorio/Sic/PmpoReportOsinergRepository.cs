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
    /// Clase de acceso a datos de la tabla PMPO_REPORT_OSINERG
    /// </summary>
    public class PmpoReportOsinergRepository : RepositoryBase, IPmpoReportOsinergRepository
    {
        public PmpoReportOsinergRepository(string strConn)
            : base(strConn)
        {
        }

        PmpoReportOsinergHelper helper = new PmpoReportOsinergHelper();

        public int Save(PmpoReportOsinergDTO entity)
        {
            string sqlQuery;
            DbCommand command;

            entity.Repcodi = GetId();
            sqlQuery = string.Format(helper.SqlSave, entity.Repcodi, entity.Repdescripcion, entity.Repmeselaboracion, entity.Repusucreacion);

            command = dbProvider.GetSqlStringCommand(sqlQuery);

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repcodi, DbType.Int32, entity.Repcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repdescripcion, DbType.String, entity.Repdescripcion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repfecha, DbType.DateTime, entity.Repfecha));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repmeselaboracion, DbType.String, entity.Repmeselaboracion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repusucreacion, DbType.String, entity.Repusucreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repfeccreacion, DbType.DateTime, entity.Repfeccreacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repusumodificacion, DbType.String, entity.Repusumodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repfecmodificacion, DbType.DateTime, entity.Repfecmodificacion));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Repestado, DbType.String, entity.Repestado));

            dbProvider.ExecuteNonQuery(command);

            return entity.Repcodi;
        }

        public int GetId()
        {
            DbCommand command;

            command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;

        }

        public PmpoReportOsinergDTO GetById(int repcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Repcodi, DbType.Int32, repcodi);
            PmpoReportOsinergDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        /// <summary>
        /// Obtener listado completo de reportes historicos
        /// </summary>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public List<PmpoReportOsinergDTO> List(DateTime fechaPeriodo)
        {
            List<PmpoReportOsinergDTO> entitys = new List<PmpoReportOsinergDTO>();
            string queryString = string.Format(helper.SqlList, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoReportOsinergDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
