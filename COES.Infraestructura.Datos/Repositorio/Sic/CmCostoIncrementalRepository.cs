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
    /// Clase de acceso a datos de la tabla CM_COSTO_INCREMENTAL
    /// </summary>
    public class CmCostoIncrementalRepository: RepositoryBase, ICmCostoIncrementalRepository
    {
        public CmCostoIncrementalRepository(string strConn): base(strConn)
        {
        }

        CmCostoIncrementalHelper helper = new CmCostoIncrementalHelper();

        public int Save(CmCostoIncrementalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmcicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cmcifecha, DbType.DateTime, entity.Cmcifecha);
            dbProvider.AddInParameter(command, helper.Cmciperiodo, DbType.Int32, entity.Cmciperiodo);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmcitramo1, DbType.Decimal, entity.Cmcitramo1);
            dbProvider.AddInParameter(command, helper.Cmcitramo2, DbType.Decimal, entity.Cmcitramo2);
            dbProvider.AddInParameter(command, helper.Cmciusucreacion, DbType.String, entity.Cmciusucreacion);
            dbProvider.AddInParameter(command, helper.Cmcifeccreacion, DbType.DateTime, entity.Cmcifeccreacion);
            dbProvider.AddInParameter(command, helper.Cmciusumodificacion, DbType.String, entity.Cmciusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmcifecmodificacion, DbType.DateTime, entity.Cmcifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmCostoIncrementalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cmcifecha, DbType.DateTime, entity.Cmcifecha);
            dbProvider.AddInParameter(command, helper.Cmciperiodo, DbType.Int32, entity.Cmciperiodo);
            dbProvider.AddInParameter(command, helper.Cmgncorrelativo, DbType.Int32, entity.Cmgncorrelativo);
            dbProvider.AddInParameter(command, helper.Cmcitramo1, DbType.Decimal, entity.Cmcitramo1);
            dbProvider.AddInParameter(command, helper.Cmcitramo2, DbType.Decimal, entity.Cmcitramo2);
            dbProvider.AddInParameter(command, helper.Cmciusucreacion, DbType.String, entity.Cmciusucreacion);
            dbProvider.AddInParameter(command, helper.Cmcifeccreacion, DbType.DateTime, entity.Cmcifeccreacion);
            dbProvider.AddInParameter(command, helper.Cmciusumodificacion, DbType.String, entity.Cmciusumodificacion);
            dbProvider.AddInParameter(command, helper.Cmcifecmodificacion, DbType.DateTime, entity.Cmcifecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmcicodi, DbType.Int32, entity.Cmcicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int periodo, DateTime fechaDatos)
        {
            string query = string.Format(helper.SqlDelete, fechaDatos.ToString(ConstantesBase.FormatoFecha), periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmCostoIncrementalDTO GetById(int cmcicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmcicodi, DbType.Int32, cmcicodi);
            CmCostoIncrementalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmCostoIncrementalDTO> List()
        {
            List<CmCostoIncrementalDTO> entitys = new List<CmCostoIncrementalDTO>();
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

        public List<CmCostoIncrementalDTO> GetByCriteria(DateTime fecha)
        {
            List<CmCostoIncrementalDTO> entitys = new List<CmCostoIncrementalDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmCostoIncrementalDTO entity = helper.Create(dr);

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iNombretna = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iNombretna)) entity.Equinomb = dr.GetString(iNombretna);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
