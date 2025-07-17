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
    /// Clase de acceso a datos de la tabla RE_EVENTO_PERIODO
    /// </summary>
    public class ReEventoPeriodoRepository: RepositoryBase, IReEventoPeriodoRepository
    {
        public ReEventoPeriodoRepository(string strConn): base(strConn)
        {
        }

        ReEventoPeriodoHelper helper = new ReEventoPeriodoHelper();

        public int Save(ReEventoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reevecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reevedescripcion, DbType.String, entity.Reevedescripcion);
            dbProvider.AddInParameter(command, helper.Reevefecha, DbType.DateTime, entity.Reevefecha);
            dbProvider.AddInParameter(command, helper.Reeveempr1, DbType.Int32, entity.Reeveempr1);
            dbProvider.AddInParameter(command, helper.Reeveempr2, DbType.Int32, entity.Reeveempr2);
            dbProvider.AddInParameter(command, helper.Reeveempr3, DbType.Int32, entity.Reeveempr3);
            dbProvider.AddInParameter(command, helper.Reeveempr4, DbType.Int32, entity.Reeveempr4);
            dbProvider.AddInParameter(command, helper.Reeveempr5, DbType.Int32, entity.Reeveempr5);
            dbProvider.AddInParameter(command, helper.Reeveporc1, DbType.Decimal, entity.Reeveporc1);
            dbProvider.AddInParameter(command, helper.Reeveporc2, DbType.Decimal, entity.Reeveporc2);
            dbProvider.AddInParameter(command, helper.Reeveporc3, DbType.Decimal, entity.Reeveporc3);
            dbProvider.AddInParameter(command, helper.Reeveporc4, DbType.Decimal, entity.Reeveporc4);
            dbProvider.AddInParameter(command, helper.Reeveporc5, DbType.Decimal, entity.Reeveporc5);
            dbProvider.AddInParameter(command, helper.Reevecomentario, DbType.String, entity.Reevecomentario);
            dbProvider.AddInParameter(command, helper.Reeveestado, DbType.String, entity.Reeveestado);
            dbProvider.AddInParameter(command, helper.Reeveusucreacion, DbType.String, entity.Reeveusucreacion);
            dbProvider.AddInParameter(command, helper.Reevefeccreacion, DbType.DateTime, entity.Reevefeccreacion);
            dbProvider.AddInParameter(command, helper.Reeveusumodificacion, DbType.String, entity.Reeveusumodificacion);
            dbProvider.AddInParameter(command, helper.Reevefecmodificacion, DbType.DateTime, entity.Reevefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReEventoPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reevedescripcion, DbType.String, entity.Reevedescripcion);
            dbProvider.AddInParameter(command, helper.Reevefecha, DbType.DateTime, entity.Reevefecha);
            dbProvider.AddInParameter(command, helper.Reeveempr1, DbType.Int32, entity.Reeveempr1);
            dbProvider.AddInParameter(command, helper.Reeveempr2, DbType.Int32, entity.Reeveempr2);
            dbProvider.AddInParameter(command, helper.Reeveempr3, DbType.Int32, entity.Reeveempr3);
            dbProvider.AddInParameter(command, helper.Reeveempr4, DbType.Int32, entity.Reeveempr4);
            dbProvider.AddInParameter(command, helper.Reeveempr5, DbType.Int32, entity.Reeveempr5);
            dbProvider.AddInParameter(command, helper.Reeveporc1, DbType.Decimal, entity.Reeveporc1);
            dbProvider.AddInParameter(command, helper.Reeveporc2, DbType.Decimal, entity.Reeveporc2);
            dbProvider.AddInParameter(command, helper.Reeveporc3, DbType.Decimal, entity.Reeveporc3);
            dbProvider.AddInParameter(command, helper.Reeveporc4, DbType.Decimal, entity.Reeveporc4);
            dbProvider.AddInParameter(command, helper.Reeveporc5, DbType.Decimal, entity.Reeveporc5);
            dbProvider.AddInParameter(command, helper.Reevecomentario, DbType.String, entity.Reevecomentario);
            dbProvider.AddInParameter(command, helper.Reeveestado, DbType.String, entity.Reeveestado);
            //dbProvider.AddInParameter(command, helper.Reeveusucreacion, DbType.String, entity.Reeveusucreacion);
            //dbProvider.AddInParameter(command, helper.Reevefeccreacion, DbType.DateTime, entity.Reevefeccreacion);
            dbProvider.AddInParameter(command, helper.Reeveusumodificacion, DbType.String, entity.Reeveusumodificacion);
            dbProvider.AddInParameter(command, helper.Reevefecmodificacion, DbType.DateTime, entity.Reevefecmodificacion);
            dbProvider.AddInParameter(command, helper.Reevecodi, DbType.Int32, entity.Reevecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reevecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Reevecodi, DbType.Int32, reevecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReEventoPeriodoDTO GetById(int reevecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reevecodi, DbType.Int32, reevecodi);
            ReEventoPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReEventoPeriodoDTO> List()
        {
            List<ReEventoPeriodoDTO> entitys = new List<ReEventoPeriodoDTO>();
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

        public List<ReEventoPeriodoDTO> GetByCriteria(int periodo)
        {
            List<ReEventoPeriodoDTO> entitys = new List<ReEventoPeriodoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEventoPeriodoDTO entity = helper.Create(dr);

                    int iResponsablenomb1 = dr.GetOrdinal(helper.Responsablenomb1);
                    if (!dr.IsDBNull(iResponsablenomb1)) entity.Responsablenomb1 = dr.GetString(iResponsablenomb1);

                    int iResponsablenomb2 = dr.GetOrdinal(helper.Responsablenomb2);
                    if (!dr.IsDBNull(iResponsablenomb2)) entity.Responsablenomb2 = dr.GetString(iResponsablenomb2);

                    int iResponsablenomb3 = dr.GetOrdinal(helper.Responsablenomb3);
                    if (!dr.IsDBNull(iResponsablenomb3)) entity.Responsablenomb3 = dr.GetString(iResponsablenomb3);

                    int iResponsablenomb4 = dr.GetOrdinal(helper.Responsablenomb4);
                    if (!dr.IsDBNull(iResponsablenomb4)) entity.Responsablenomb4 = dr.GetString(iResponsablenomb4);

                    int iResponsablenomb5 = dr.GetOrdinal(helper.Responsablenomb5);
                    if (!dr.IsDBNull(iResponsablenomb5)) entity.Responsablenomb5 = dr.GetString(iResponsablenomb5);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReEventoPeriodoDTO> ObtenerEventosUtilizadosPorPeriodo(int periodo, int idEmpresa)
        {
            List<ReEventoPeriodoDTO> entitys = new List<ReEventoPeriodoDTO>();
            string sql = string.Format(helper.SqlObtenerEventosUtilizadosPorPeriodo, periodo, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReEventoPeriodoDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
