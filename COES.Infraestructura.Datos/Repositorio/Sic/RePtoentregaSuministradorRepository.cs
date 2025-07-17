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
    /// Clase de acceso a datos de la tabla RE_PTOENTREGA_SUMINISTRADOR
    /// </summary>
    public class RePtoentregaSuministradorRepository: RepositoryBase, IRePtoentregaSuministradorRepository
    {
        public RePtoentregaSuministradorRepository(string strConn): base(strConn)
        {
        }

        RePtoentregaSuministradorHelper helper = new RePtoentregaSuministradorHelper();

        public int Save(RePtoentregaSuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Reptsmcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reptsmusucreacion, DbType.String, entity.Reptsmusucreacion);
            dbProvider.AddInParameter(command, helper.Reptsmfeccreacion, DbType.DateTime, entity.Reptsmfeccreacion);
            dbProvider.AddInParameter(command, helper.Reptsmusumodificacion, DbType.String, entity.Reptsmusumodificacion);
            dbProvider.AddInParameter(command, helper.Reptsmfecmodificacion, DbType.DateTime, entity.Reptsmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RePtoentregaSuministradorDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Reptsmcodi, DbType.Int32, entity.Reptsmcodi);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, entity.Repercodi);
            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, entity.Repentcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Reptsmusucreacion, DbType.String, entity.Reptsmusucreacion);
            dbProvider.AddInParameter(command, helper.Reptsmfeccreacion, DbType.DateTime, entity.Reptsmfeccreacion);
            dbProvider.AddInParameter(command, helper.Reptsmusumodificacion, DbType.String, entity.Reptsmusumodificacion);
            dbProvider.AddInParameter(command, helper.Reptsmfecmodificacion, DbType.DateTime, entity.Reptsmfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int idPtoEntrega, int idPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Repentcodi, DbType.Int32, idPtoEntrega);
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, idPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public void EliminarPorPeriodo(int idPeriodo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarPorPeriodo);
                        
            dbProvider.AddInParameter(command, helper.Repercodi, DbType.Int32, idPeriodo);

            dbProvider.ExecuteNonQuery(command);
        }

        public RePtoentregaSuministradorDTO GetById(int reptsmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Reptsmcodi, DbType.Int32, reptsmcodi);
            RePtoentregaSuministradorDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RePtoentregaSuministradorDTO> List()
        {
            List<RePtoentregaSuministradorDTO> entitys = new List<RePtoentregaSuministradorDTO>();
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

        public List<RePtoentregaSuministradorDTO> GetByCriteria(int idPeriodo)
        {
            List<RePtoentregaSuministradorDTO> entitys = new List<RePtoentregaSuministradorDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RePtoentregaSuministradorDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iRepentnpombre = dr.GetOrdinal(helper.Repentnombre);
                    if (!dr.IsDBNull(iRepentnpombre)) entity.Repentnombre = dr.GetString(iRepentnpombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RePtoentregaSuministradorDTO> ObtenerPorPuntoEntregaPeriodo(int idPtoEntrega, int idPeriodo)
        {
            List<RePtoentregaSuministradorDTO> entitys = new List<RePtoentregaSuministradorDTO>();
            string query = string.Format(helper.SqlObtenerPorPuntoEntregaPeriodo, idPtoEntrega, idPeriodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RePtoentregaSuministradorDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
