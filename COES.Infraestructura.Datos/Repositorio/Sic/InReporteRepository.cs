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
    /// Clase de acceso a datos de la tabla IN_REPORTE
    /// </summary>
    public class InReporteRepository : RepositoryBase, IInReporteRepository
    {
        public InReporteRepository(string strConn) : base(strConn)
        {
        }

        InReporteHelper helper = new InReporteHelper();

        public int Save(InReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Inrepnombre, DbType.String, entity.Inrepnombre);
            dbProvider.AddInParameter(command, helper.Inrephorizonte, DbType.Int32, entity.Inrephorizonte);
            dbProvider.AddInParameter(command, helper.Inreptipo, DbType.Int32, entity.Inreptipo);
            dbProvider.AddInParameter(command, helper.Inrepusucreacion, DbType.String, entity.Inrepusucreacion);
            dbProvider.AddInParameter(command, helper.Inrepfeccreacion, DbType.DateTime, entity.Inrepfeccreacion);
            dbProvider.AddInParameter(command, helper.Inrepusumodificacion, DbType.String, entity.Inrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Inrepfecmodificacion, DbType.DateTime, entity.Inrepfecmodificacion);
            dbProvider.AddInParameter(command, helper.Progrcodi, DbType.Int32, entity.Progrcodi);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Update(InReporteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Inrepusumodificacion, DbType.String, entity.Inrepusumodificacion);
            dbProvider.AddInParameter(command, helper.Inrepfecmodificacion, DbType.DateTime, entity.Inrepfecmodificacion);
            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, entity.Inrepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Inrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, Inrepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public InReporteDTO GetById(int Inrepcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Inrepcodi, DbType.Int32, Inrepcodi);
            InReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<InReporteDTO> List()
        {
            List<InReporteDTO> entitys = new List<InReporteDTO>();
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

        public List<InReporteDTO> GetByCriteria()
        {
            List<InReporteDTO> entitys = new List<InReporteDTO>();

            string query = string.Format(helper.SqlGetByCriteria);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    InReporteDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public InReporteDTO  ObtenerReportePorTipo(int tiporeporte, int progcodi)
        {
            string query = string.Format(helper.SqlObtenerReportePorTipo, tiporeporte, progcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            InReporteDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
