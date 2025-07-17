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
    /// Clase de acceso a datos de la tabla SMA_INDISPONIBILIDAD_TEMPORAL
    /// </summary>
    public class SmaIndisponibilidadTemporalRepository: RepositoryBase, ISmaIndisponibilidadTemporalRepository
    {
        public SmaIndisponibilidadTemporalRepository(string strConn): base(strConn)
        {
        }

        SmaIndisponibilidadTemporalHelper helper = new SmaIndisponibilidadTemporalHelper();

        public int Save(SmaIndisponibilidadTemporalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Smaintcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Smaintfecha, DbType.DateTime, entity.Smaintfecha);
            dbProvider.AddInParameter(command, helper.Smaintindexiste, DbType.String, entity.Smaintindexiste);
            dbProvider.AddInParameter(command, helper.Smainttipo, DbType.String, entity.Smainttipo);
            dbProvider.AddInParameter(command, helper.Smaintbanda, DbType.Decimal, entity.Smaintbanda);
            dbProvider.AddInParameter(command, helper.Smaintmotivo, DbType.String, entity.Smaintmotivo);
            dbProvider.AddInParameter(command, helper.Smaintusucreacion, DbType.String, entity.Smaintusucreacion);
            dbProvider.AddInParameter(command, helper.Smaintfeccreacion, DbType.DateTime, entity.Smaintfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaintusumodificacion, DbType.String, entity.Smaintusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaintfecmodificacion, DbType.DateTime, entity.Smaintfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SmaIndisponibilidadTemporalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Smaintcodi, DbType.Int32, entity.Smaintcodi);
            dbProvider.AddInParameter(command, helper.Urscodi, DbType.Int32, entity.Urscodi);
            dbProvider.AddInParameter(command, helper.Smaintfecha, DbType.DateTime, entity.Smaintfecha);
            dbProvider.AddInParameter(command, helper.Smaintindexiste, DbType.String, entity.Smaintindexiste);
            dbProvider.AddInParameter(command, helper.Smainttipo, DbType.String, entity.Smainttipo);
            dbProvider.AddInParameter(command, helper.Smaintbanda, DbType.Decimal, entity.Smaintbanda);
            dbProvider.AddInParameter(command, helper.Smaintmotivo, DbType.String, entity.Smaintmotivo);
            dbProvider.AddInParameter(command, helper.Smaintusucreacion, DbType.String, entity.Smaintusucreacion);
            dbProvider.AddInParameter(command, helper.Smaintfeccreacion, DbType.DateTime, entity.Smaintfeccreacion);
            dbProvider.AddInParameter(command, helper.Smaintusumodificacion, DbType.String, entity.Smaintusumodificacion);
            dbProvider.AddInParameter(command, helper.Smaintfecmodificacion, DbType.DateTime, entity.Smaintfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int smaintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Smaintcodi, DbType.Int32, smaintcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SmaIndisponibilidadTemporalDTO GetById(int smaintcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Smaintcodi, DbType.Int32, smaintcodi);
            SmaIndisponibilidadTemporalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SmaIndisponibilidadTemporalDTO> List()
        {
            List<SmaIndisponibilidadTemporalDTO> entitys = new List<SmaIndisponibilidadTemporalDTO>();
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

        public List<SmaIndisponibilidadTemporalDTO> GetByCriteria()
        {
            List<SmaIndisponibilidadTemporalDTO> entitys = new List<SmaIndisponibilidadTemporalDTO>();
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

        public List<SmaIndisponibilidadTemporalDTO> ListarPorFecha(DateTime fecha)
        {
            List<SmaIndisponibilidadTemporalDTO> entitys = new List<SmaIndisponibilidadTemporalDTO>();

            string query = string.Format(helper.SqlListarPorFecha, fecha.ToString(ConstantesBase.FormatoFechaPE));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SmaIndisponibilidadTemporalDTO entity = helper.Create(dr);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
       
    }
}
