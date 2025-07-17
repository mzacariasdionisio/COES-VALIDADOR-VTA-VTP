using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data.Common;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PmoPeriodoRepository : RepositoryBase, IPmoPeriodoRepository
    {
        public PmoPeriodoRepository(string strConn)
            : base(strConn)
        {
        }

        PmoPeriodoHelper helper = new PmoPeriodoHelper();

        public List<PmoPeriodoDTO> List()
        {
            List<PmoPeriodoDTO> entitys = new List<PmoPeriodoDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoPeriodoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PmoPeriodoDTO GetById(int id)
        {
            PmoPeriodoDTO entity = new PmoPeriodoDTO();
            string queryString = string.Format(helper.SqlGetById);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, id);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int Save(PmoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.PmPeriNombre, DbType.String, entity.PmPeriNombre);
            dbProvider.AddInParameter(command, helper.PmPeriAniOMes, DbType.Int32, entity.PmPeriAniOMes);
            dbProvider.AddInParameter(command, helper.PmPeriTipo, DbType.String, entity.PmPeriTipo);
            dbProvider.AddInParameter(command, helper.PmPeriEstado, DbType.String, entity.PmPeriEstado);
            dbProvider.AddInParameter(command, helper.PmPeriUsuCreacion, DbType.String, entity.PmPeriUsuCreacion);
            dbProvider.AddInParameter(command, helper.PmPeriFecCreacion, DbType.DateTime, entity.PmPeriFecCreacion);
            dbProvider.AddInParameter(command, helper.PmPeriUsuModificacion, DbType.String, entity.PmPeriUsuModificacion);
            dbProvider.AddInParameter(command, helper.PmPeriFecModificacion, DbType.DateTime, entity.PmPeriFecModificacion);
            dbProvider.AddInParameter(command, helper.PmPeriFechaPeriodo, DbType.DateTime, entity.PmPeriFechaPeriodo);
            dbProvider.AddInParameter(command, helper.Pmperifecini, DbType.DateTime, entity.Pmperifecini);
            dbProvider.AddInParameter(command, helper.Pmperifecinimes, DbType.DateTime, entity.Pmperifecinimes);
            dbProvider.AddInParameter(command, helper.Pmperinumsem, DbType.Int32, entity.Pmperinumsem);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void UpdateFechasMantenimiento(PmoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateFechasMantenimiento);
            dbProvider.AddInParameter(command, helper.PmPeriFecIniMantAnual, DbType.DateTime, entity.PmPeriFecIniMantAnual);
            dbProvider.AddInParameter(command, helper.PmPeriFecFinMantAnual, DbType.DateTime, entity.PmPeriFecFinMantAnual);
            dbProvider.AddInParameter(command, helper.PmPeriFecIniMantMensual, DbType.DateTime, entity.PmPeriFecIniMantMensual);
            dbProvider.AddInParameter(command, helper.PmPeriFecFinMantMensual, DbType.DateTime, entity.PmPeriFecFinMantMensual);
            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PmPeriCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PmoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pmperifecini, DbType.DateTime, entity.Pmperifecini);
            dbProvider.AddInParameter(command, helper.Pmperifecinimes, DbType.DateTime, entity.Pmperifecinimes);
            dbProvider.AddInParameter(command, helper.PmPeriNombre, DbType.String, entity.PmPeriNombre);
            dbProvider.AddInParameter(command, helper.PmPeriAniOMes, DbType.Int32, entity.PmPeriAniOMes);
            dbProvider.AddInParameter(command, helper.PmPeriTipo, DbType.String, entity.PmPeriTipo);
            dbProvider.AddInParameter(command, helper.PmPeriEstado, DbType.String, entity.PmPeriEstado);
            dbProvider.AddInParameter(command, helper.PmPeriUsuCreacion, DbType.String, entity.PmPeriUsuCreacion);
            dbProvider.AddInParameter(command, helper.PmPeriFecCreacion, DbType.DateTime, entity.PmPeriFecCreacion);
            dbProvider.AddInParameter(command, helper.PmPeriUsuModificacion, DbType.String, entity.PmPeriUsuModificacion);
            dbProvider.AddInParameter(command, helper.PmPeriFecModificacion, DbType.DateTime, entity.PmPeriFecModificacion);
            dbProvider.AddInParameter(command, helper.PmPeriFechaPeriodo, DbType.DateTime, entity.PmPeriFechaPeriodo);
            dbProvider.AddInParameter(command, helper.PmPeriFecIniMantAnual, DbType.DateTime, entity.PmPeriFecIniMantAnual);
            dbProvider.AddInParameter(command, helper.PmPeriFecFinMantAnual, DbType.DateTime, entity.PmPeriFecFinMantAnual);
            dbProvider.AddInParameter(command, helper.PmPeriFecIniMantMensual, DbType.DateTime, entity.PmPeriFecIniMantMensual);
            dbProvider.AddInParameter(command, helper.PmPeriFecFinMantMensual, DbType.DateTime, entity.PmPeriFecFinMantMensual);
            dbProvider.AddInParameter(command, helper.Pmperinumsem, DbType.Int32, entity.Pmperinumsem);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, entity.PmPeriCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PmoPeriodoDTO> GetByCriteria(int anio)
        {
            List<PmoPeriodoDTO> entitys = new List<PmoPeriodoDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, anio);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void Delete(int pmpericodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, pmpericodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}