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
    /// Clase de acceso a datos de la tabla RNT_PERIODO
    /// </summary>
    public class RntPeriodoRepository: RepositoryBase, IRntPeriodoRepository
    {
        public RntPeriodoRepository(string strConn): base(strConn)
        {
        }

        RntPeriodoHelper helper = new RntPeriodoHelper();

        public int Save(RntPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCodigoGenerado);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Perdanio, DbType.Int32, entity.PerdAnio);
            dbProvider.AddInParameter(command, helper.Perdsemestre, DbType.String, entity.PerdSemestre);
            dbProvider.AddInParameter(command, helper.Perdestado, DbType.String, entity.PerdEstado);
            dbProvider.AddInParameter(command, helper.Perdusuariocreacion, DbType.String, entity.PerdUsuarioCreacion);
            dbProvider.AddInParameter(command, helper.Perdfechacreacion, DbType.DateTime, entity.PerdFechaCreacion);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RntPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Perdanio, DbType.Int32, entity.PerdAnio);
            dbProvider.AddInParameter(command, helper.Perdsemestre, DbType.String, entity.PerdSemestre);
            dbProvider.AddInParameter(command, helper.Perdestado, DbType.String, entity.PerdEstado);
            dbProvider.AddInParameter(command, helper.Perdusuarioupdate, DbType.String, entity.PerdUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Perdfechaupdate, DbType.DateTime, entity.PerdFechaUpdate);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, entity.PeriodoCodi);
            

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(RntPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Perdusuarioupdate, DbType.String, entity.PerdUsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Perdfechaupdate, DbType.DateTime, entity.PerdFechaUpdate);
            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, entity.PeriodoCodi);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public RntPeriodoDTO GetById(int periodocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Periodocodi, DbType.Int32, periodocodi);
            RntPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RntPeriodoDTO> List()
        {
            List<RntPeriodoDTO> entitys = new List<RntPeriodoDTO>();
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

        public List<RntPeriodoDTO> ListCombo()
        {
            List<RntPeriodoDTO> entitys = new List<RntPeriodoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCombo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RntPeriodoDTO> GetByCriteria()
        {
            List<RntPeriodoDTO> entitys = new List<RntPeriodoDTO>();
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
    }
}
