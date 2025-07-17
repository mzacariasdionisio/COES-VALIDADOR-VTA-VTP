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
    /// Clase de acceso a datos de la tabla RE_PTOENTREGA_PERIODO
    /// </summary>
    public class RePtoentregaPeriodoRepository: RepositoryBase, IRePtoentregaPeriodoRepository
    {
        public RePtoentregaPeriodoRepository(string strConn): base(strConn)
        {
        }

        RePtoentregaPeriodoHelper helper = new RePtoentregaPeriodoHelper();

        public int Save(RePtoentregaPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reptopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reptopusucreacion, DbType.String, entity.Reptopusucreacion);
            dbProvider.AddInParameter(command, helper.Reptopfeccreacion, DbType.DateTime, entity.Reptopfeccreacion);
            dbProvider.AddInParameter(command, helper.Reptopusumodificacion, DbType.String, entity.Reptopusumodificacion);
            dbProvider.AddInParameter(command, helper.Reptopfecmodificacion, DbType.DateTime, entity.Reptopfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RePtoentregaPeriodoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Reptopusucreacion, DbType.String, entity.Reptopusucreacion);
            dbProvider.AddInParameter(command, helper.Reptopfeccreacion, DbType.DateTime, entity.Reptopfeccreacion);
            dbProvider.AddInParameter(command, helper.Reptopusumodificacion, DbType.String, entity.Reptopusumodificacion);
            dbProvider.AddInParameter(command, helper.Reptopfecmodificacion, DbType.DateTime, entity.Reptopfecmodificacion);
            dbProvider.AddInParameter(command, helper.Reptopcodi, DbType.Int32, entity.Reptopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int reptopcodi, int periodo)
        {
            string sql = string.Format(helper.SqlDelete, reptopcodi, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            dbProvider.AddInParameter(command, helper.Reptopcodi, DbType.Int32, reptopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RePtoentregaPeriodoDTO GetById(int reptopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reptopcodi, DbType.Int32, reptopcodi);
            RePtoentregaPeriodoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RePtoentregaPeriodoDTO> List()
        {
            List<RePtoentregaPeriodoDTO> entitys = new List<RePtoentregaPeriodoDTO>();
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

        public List<RePtoentregaPeriodoDTO> GetByCriteria(int periodo)
        {
            List<RePtoentregaPeriodoDTO> entitys = new List<RePtoentregaPeriodoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RePtoentregaPeriodoDTO entity = helper.Create(dr);

                    int iRepentnombre = dr.GetOrdinal(helper.Repentnombre);
                    if (!dr.IsDBNull(iRepentnombre)) entity.Repentnombre = dr.GetString(iRepentnombre);

                    int iRentabrev = dr.GetOrdinal(helper.Rentabrev);
                    if (!dr.IsDBNull(iRentabrev)) entity.Rentabrev = dr.GetString(iRentabrev);

                    int iRentcodi = dr.GetOrdinal(helper.Rentcodi);
                    if (!dr.IsDBNull(iRentcodi)) entity.Rentcodi = Convert.ToInt32(dr.GetValue(iRentcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerPorPtoEntrega(int ptoEntregaCodi, int pericodi)
        {
            string sql = string.Format(helper.SqlObtenerPorPtoEntrega, ptoEntregaCodi, pericodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            int count = 0;
            object result = dbProvider.ExecuteScalar(command);
            if(result != null)
            {
                count = Convert.ToInt32(result);
            }
            return count;
        }

        public List<RePtoentregaPeriodoDTO> ObtenerPtoEntregaUtilizadosPorPeriodo(int idPeriodo, int idSuministrador, string tipo)
        {
            List<RePtoentregaPeriodoDTO> entitys = new List<RePtoentregaPeriodoDTO>();
            string sql = string.Format(helper.SqlObtenerPtoEntregaUtilizadosPorPeriodo, idPeriodo, idSuministrador, tipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RePtoentregaPeriodoDTO entity = new RePtoentregaPeriodoDTO();

                    int iReptopcodi = dr.GetOrdinal(helper.Repentcodi);
                    if (!dr.IsDBNull(iReptopcodi)) entity.Repentcodi = Convert.ToInt32(dr.GetValue(iReptopcodi));

                    int iRepentnombre = dr.GetOrdinal(helper.Repentnombre);
                    if (!dr.IsDBNull(iRepentnombre)) entity.Repentnombre = dr.GetString(iRepentnombre);
                                      

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
