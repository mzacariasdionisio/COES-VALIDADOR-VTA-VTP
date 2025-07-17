using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VTD_LOGPROCESO
    /// </summary>
    public class VtdLogProcesoRepository: RepositoryBase, IVtdLogProcesoRepository
    {
        public VtdLogProcesoRepository(string strConn): base(strConn)
        {
        }

        VtdLogProcesoHelper helper = new VtdLogProcesoHelper();

        public int Save(VtdLogProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Logpcodi, DbType.Decimal, id);
            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, entity.Valocodi);
            dbProvider.AddInParameter(command, helper.Logpfecha, DbType.DateTime, entity.Logpfecha);
            dbProvider.AddInParameter(command, helper.Logphorainicio, DbType.DateTime, entity.Logphorainicio);
            dbProvider.AddInParameter(command, helper.Logphorafin, DbType.DateTime, entity.Logphorafin);
            dbProvider.AddInParameter(command, helper.Logplog, DbType.String, entity.Logplog);
            dbProvider.AddInParameter(command, helper.Logptipo, DbType.String, entity.Logptipo);
            dbProvider.AddInParameter(command, helper.Logpestado, DbType.String, entity.Logpestado);
            dbProvider.AddInParameter(command, helper.Logpsucreacion, DbType.String, entity.Logpusucreacion);
            dbProvider.AddInParameter(command, helper.Logpfeccreacion, DbType.DateTime, entity.Logpfeccreacion);
            dbProvider.AddInParameter(command, helper.Logpusumodificacion, DbType.String, entity.Logpusumodificacion);
            dbProvider.AddInParameter(command, helper.Logpfecmodificacion, DbType.DateTime, entity.Logpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VtdLogProcesoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Logpcodi, DbType.Decimal, entity.Logpcodi);
            dbProvider.AddInParameter(command, helper.Valocodi, DbType.Decimal, entity.Valocodi);
            dbProvider.AddInParameter(command, helper.Logpfecha, DbType.DateTime, entity.Logpfecha);
            dbProvider.AddInParameter(command, helper.Logphorainicio, DbType.DateTime, entity.Logphorainicio);
            dbProvider.AddInParameter(command, helper.Logphorafin, DbType.DateTime, entity.Logphorafin);
            dbProvider.AddInParameter(command, helper.Logplog, DbType.String, entity.Logplog);
            dbProvider.AddInParameter(command, helper.Logptipo, DbType.String, entity.Logptipo);
            dbProvider.AddInParameter(command, helper.Logpestado, DbType.String, entity.Logpestado);
            dbProvider.AddInParameter(command, helper.Logpsucreacion, DbType.String, entity.Logpusucreacion);
            dbProvider.AddInParameter(command, helper.Logpfeccreacion, DbType.DateTime, entity.Logpfeccreacion);
            dbProvider.AddInParameter(command, helper.Logpusumodificacion, DbType.String, entity.Logpusumodificacion);
            dbProvider.AddInParameter(command, helper.Logpfecmodificacion, DbType.DateTime, entity.Logpfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Logpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Logpcodi, DbType.Decimal, Logpcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VtdLogProcesoDTO GetById(int Logpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Logpcodi, DbType.Int32, Logpcodi);
            VtdLogProcesoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VtdLogProcesoDTO> List()
        {
            List<VtdLogProcesoDTO> entitys = new List<VtdLogProcesoDTO>();
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

        public List<VtdLogProcesoDTO> GetByCriteria()
        {
            List<VtdLogProcesoDTO> entitys = new List<VtdLogProcesoDTO>();
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

        public List<VtdLogProcesoDTO> GetListByDate(DateTime date)
        {
            List<VtdLogProcesoDTO> entitys = new List<VtdLogProcesoDTO>();

            string sCommand = string.Format(helper.GetListFullByDate, date.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdLogProcesoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VtdLogProcesoDTO> GetListPageByDate(DateTime date, int nroPage, int pageSize)
        {
            List<VtdLogProcesoDTO> entitys = new List<VtdLogProcesoDTO>();

            string sCommand = string.Format(helper.GetListPagedByDate, date.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdLogProcesoDTO entity = helper.Create(dr);                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
