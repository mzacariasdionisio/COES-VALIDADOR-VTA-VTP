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
    /// Clase de acceso a datos de la tabla ME_RFRIA_UNIDADRESTRIC
    /// </summary>
    public class MeRfriaUnidadrestricRepository: RepositoryBase, IMeRfriaUnidadrestricRepository
    {
        public MeRfriaUnidadrestricRepository(string strConn): base(strConn)
        {
        }

        MeRfriaUnidadrestricHelper helper = new MeRfriaUnidadrestricHelper();

        public int Save(MeRfriaUnidadrestricDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Urfriacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Urfriafechaperiodo, DbType.DateTime, entity.Urfriafechaperiodo);
            dbProvider.AddInParameter(command, helper.Urfriafechaini, DbType.DateTime, entity.Urfriafechaini);
            dbProvider.AddInParameter(command, helper.Urfriafechafin, DbType.DateTime, entity.Urfriafechafin);
            dbProvider.AddInParameter(command, helper.Urfriausucreacion, DbType.String, entity.Urfriausucreacion);
            dbProvider.AddInParameter(command, helper.Urfriafeccreacion, DbType.DateTime, entity.Urfriafeccreacion);
            dbProvider.AddInParameter(command, helper.Urfriausumodificacion, DbType.String, entity.Urfriausumodificacion);
            dbProvider.AddInParameter(command, helper.Urfriafecmodificacion, DbType.DateTime, entity.Urfriafecmodificacion);
            dbProvider.AddInParameter(command, helper.Urfriaactivo, DbType.Int32, entity.Urfriaactivo);
            dbProvider.AddInParameter(command, helper.Urfriaobservacion, DbType.String, entity.Urfriaobservacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeRfriaUnidadrestricDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Urfriafechaperiodo, DbType.DateTime, entity.Urfriafechaperiodo);
            dbProvider.AddInParameter(command, helper.Urfriafechaini, DbType.DateTime, entity.Urfriafechaini);
            dbProvider.AddInParameter(command, helper.Urfriafechafin, DbType.DateTime, entity.Urfriafechafin);
            dbProvider.AddInParameter(command, helper.Urfriausucreacion, DbType.String, entity.Urfriausucreacion);
            dbProvider.AddInParameter(command, helper.Urfriafeccreacion, DbType.DateTime, entity.Urfriafeccreacion);
            dbProvider.AddInParameter(command, helper.Urfriausumodificacion, DbType.String, entity.Urfriausumodificacion);
            dbProvider.AddInParameter(command, helper.Urfriafecmodificacion, DbType.DateTime, entity.Urfriafecmodificacion);
            dbProvider.AddInParameter(command, helper.Urfriaactivo, DbType.Int32, entity.Urfriaactivo);
            dbProvider.AddInParameter(command, helper.Urfriaobservacion, DbType.String, entity.Urfriaobservacion);
            dbProvider.AddInParameter(command, helper.Urfriacodi, DbType.Int32, entity.Urfriacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int urfriacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Urfriacodi, DbType.Int32, urfriacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MeRfriaUnidadrestricDTO GetById(int urfriacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Urfriacodi, DbType.Int32, urfriacodi);
            MeRfriaUnidadrestricDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeRfriaUnidadrestricDTO> List()
        {
            List<MeRfriaUnidadrestricDTO> entitys = new List<MeRfriaUnidadrestricDTO>();
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

        public List<MeRfriaUnidadrestricDTO> GetByCriteria(DateTime fecha)
        {
            List<MeRfriaUnidadrestricDTO> entitys = new List<MeRfriaUnidadrestricDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeRfriaUnidadrestricDTO entity = helper.Create(dr);

                    int iEmpresanomb = dr.GetOrdinal(helper.Empresanomb);
                    if (!dr.IsDBNull(iEmpresanomb)) entity.Empresanomb = dr.GetString(iEmpresanomb);

                    int iCentralnomb = dr.GetOrdinal(helper.Centralnomb);
                    if (!dr.IsDBNull(iCentralnomb)) entity.Centralnomb = dr.GetString(iCentralnomb);

                    int iUnidadnomb = dr.GetOrdinal(helper.Unidadnomb);
                    if (!dr.IsDBNull(iUnidadnomb)) entity.Unidadnomb = dr.GetString(iUnidadnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
