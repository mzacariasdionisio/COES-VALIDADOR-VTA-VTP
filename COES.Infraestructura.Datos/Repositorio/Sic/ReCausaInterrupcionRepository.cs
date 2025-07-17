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
    /// Clase de acceso a datos de la tabla RE_CAUSA_INTERRUPCION
    /// </summary>
    public class ReCausaInterrupcionRepository: RepositoryBase, IReCausaInterrupcionRepository
    {
        public ReCausaInterrupcionRepository(string strConn): base(strConn)
        {
        }

        ReCausaInterrupcionHelper helper = new ReCausaInterrupcionHelper();

        public int Save(ReCausaInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);
            dbProvider.AddInParameter(command, helper.Recintnombre, DbType.String, entity.Recintnombre);
            dbProvider.AddInParameter(command, helper.Recintestado, DbType.String, entity.Recintestado);
            dbProvider.AddInParameter(command, helper.Recintusucreacion, DbType.String, entity.Recintusucreacion);
            dbProvider.AddInParameter(command, helper.Recintfeccreacion, DbType.DateTime, entity.Recintfeccreacion);
            dbProvider.AddInParameter(command, helper.Recintusumodificacion, DbType.String, entity.Recintusumodificacion);
            dbProvider.AddInParameter(command, helper.Recintfecmodificacion, DbType.DateTime, entity.Recintfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReCausaInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);
            dbProvider.AddInParameter(command, helper.Recintnombre, DbType.String, entity.Recintnombre);
            dbProvider.AddInParameter(command, helper.Recintestado, DbType.String, entity.Recintestado);
            dbProvider.AddInParameter(command, helper.Recintusucreacion, DbType.String, entity.Recintusucreacion);
            dbProvider.AddInParameter(command, helper.Recintfeccreacion, DbType.DateTime, entity.Recintfeccreacion);
            dbProvider.AddInParameter(command, helper.Recintusumodificacion, DbType.String, entity.Recintusumodificacion);
            dbProvider.AddInParameter(command, helper.Recintfecmodificacion, DbType.DateTime, entity.Recintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, entity.Recintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int recintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, recintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReCausaInterrupcionDTO GetById(int recintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Recintcodi, DbType.Int32, recintcodi);
            ReCausaInterrupcionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReCausaInterrupcionDTO> List()
        {
            List<ReCausaInterrupcionDTO> entitys = new List<ReCausaInterrupcionDTO>();
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

        public List<ReCausaInterrupcionDTO> GetByCriteria(int periodo)
        {
            List<ReCausaInterrupcionDTO> entitys = new List<ReCausaInterrupcionDTO>();
            string query = string.Format(helper.SqlGetByCriteria, periodo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReCausaInterrupcionDTO entity = helper.Create(dr);

                    int iRetintnombre = dr.GetOrdinal(helper.Retintnombre);
                    if (!dr.IsDBNull(iRetintnombre)) entity.Retintnombre = dr.GetString(iRetintnombre);

                    int iReindki = dr.GetOrdinal(helper.Reindki);
                    if (!dr.IsDBNull(iReindki)) entity.Reindki = dr.GetDecimal(iReindki);

                    int iReindni = dr.GetOrdinal(helper.Reindni);
                    if (!dr.IsDBNull(iReindni)) entity.Reindni = dr.GetDecimal(iReindni);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReCausaInterrupcionDTO> ObtenerConfiguracion(int idTipo)
        {
            List<ReCausaInterrupcionDTO> entitys = new List<ReCausaInterrupcionDTO>();
            string query = string.Format(helper.SqlObenerConfiguracion, idTipo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReCausaInterrupcionDTO entity = helper.Create(dr);

                    int iRetintnombre = dr.GetOrdinal(helper.Retintnombre);
                    if (!dr.IsDBNull(iRetintnombre)) entity.Retintnombre = dr.GetString(iRetintnombre);

                    entity.IndicadorEdicion = ConstantesBase.SI;
                    int iIndicadorEdicion = dr.GetOrdinal(helper.IndicadorEdicion);
                    if (!dr.IsDBNull(iIndicadorEdicion))
                    {
                        int count = Convert.ToInt32(dr.GetValue(iIndicadorEdicion));
                        if (count > 0)
                            entity.IndicadorEdicion = ConstantesBase.NO;
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ReCausaInterrupcionDTO> ObtenerCausasInterrupcionUtilizadosPorPeriodo(int idPeriodo, int idSuministrador)
        {
            List<ReCausaInterrupcionDTO> entitys = new List<ReCausaInterrupcionDTO>();
            string query = string.Format(helper.SqlObtenerCausasInterrupcionUtilizadosPorPeriodo, idPeriodo, idSuministrador);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReCausaInterrupcionDTO entity = helper.Create(dr);                   

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
