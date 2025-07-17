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
    /// Clase de acceso a datos de la tabla RE_TIPO_INTERRUPCION
    /// </summary>
    public class ReTipoInterrupcionRepository: RepositoryBase, IReTipoInterrupcionRepository
    {
        public ReTipoInterrupcionRepository(string strConn): base(strConn)
        {
        }

        ReTipoInterrupcionHelper helper = new ReTipoInterrupcionHelper();

        public int Save(ReTipoInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Retintnombre, DbType.String, entity.Retintnombre);
            dbProvider.AddInParameter(command, helper.Retintestado, DbType.String, entity.Retintestado);
            dbProvider.AddInParameter(command, helper.Retintusucreacion, DbType.String, entity.Retintusucreacion);
            dbProvider.AddInParameter(command, helper.Retintfeccreacion, DbType.DateTime, entity.Retintfeccreacion);
            dbProvider.AddInParameter(command, helper.Retintusumodificacion, DbType.String, entity.Retintusumodificacion);
            dbProvider.AddInParameter(command, helper.Retintfecmodificacion, DbType.DateTime, entity.Retintfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(ReTipoInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Retintnombre, DbType.String, entity.Retintnombre);
            dbProvider.AddInParameter(command, helper.Retintestado, DbType.String, entity.Retintestado);
            dbProvider.AddInParameter(command, helper.Retintusucreacion, DbType.String, entity.Retintusucreacion);
            dbProvider.AddInParameter(command, helper.Retintfeccreacion, DbType.DateTime, entity.Retintfeccreacion);
            dbProvider.AddInParameter(command, helper.Retintusumodificacion, DbType.String, entity.Retintusumodificacion);
            dbProvider.AddInParameter(command, helper.Retintfecmodificacion, DbType.DateTime, entity.Retintfecmodificacion);
            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, entity.Retintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int retintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, retintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public ReTipoInterrupcionDTO GetById(int retintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Retintcodi, DbType.Int32, retintcodi);
            ReTipoInterrupcionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<ReTipoInterrupcionDTO> List()
        {
            List<ReTipoInterrupcionDTO> entitys = new List<ReTipoInterrupcionDTO>();
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

        public List<ReTipoInterrupcionDTO> GetByCriteria()
        {
            List<ReTipoInterrupcionDTO> entitys = new List<ReTipoInterrupcionDTO>();
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

        public List<ReTipoInterrupcionDTO> ObtenerConfiguracion()
        {
            List<ReTipoInterrupcionDTO> entitys = new List<ReTipoInterrupcionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerConfiguracion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ReTipoInterrupcionDTO entity = helper.Create(dr);

                    int iIndicadorEdicion = dr.GetOrdinal(helper.IndicadorEdicion);
                    entity.IndicadorEdicion = ConstantesBase.SI;
                    if (!dr.IsDBNull(iIndicadorEdicion))
                    {
                        int count  = Convert.ToInt32(dr.GetValue(iIndicadorEdicion));

                        if(count > 0)
                        {
                            entity.IndicadorEdicion = ConstantesBase.NO;
                        }
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
