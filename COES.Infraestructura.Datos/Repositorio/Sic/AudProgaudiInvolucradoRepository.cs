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
    /// Clase de acceso a datos de la tabla AUD_PROGAUDI_INVOLUCRADO
    /// </summary>
    public class AudProgaudiInvolucradoRepository: RepositoryBase, IAudProgaudiInvolucradoRepository
    {
        public AudProgaudiInvolucradoRepository(string strConn): base(strConn)
        {
        }

        AudProgaudiInvolucradoHelper helper = new AudProgaudiInvolucradoHelper();

        public int Save(AudProgaudiInvolucradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Progaicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoinvolucrado, DbType.Int32, entity.Tabcdcoditipoinvolucrado);
            dbProvider.AddInParameter(command, helper.Percodiinvolucrado, DbType.Int32, entity.Percodiinvolucrado);
            dbProvider.AddInParameter(command, helper.Progaiactivo, DbType.String, entity.Progaiactivo);
            dbProvider.AddInParameter(command, helper.Progaihistorico, DbType.String, entity.Progaihistorico);
            dbProvider.AddInParameter(command, helper.Progaiusuregistro, DbType.String, entity.Progaiusuregistro);
            dbProvider.AddInParameter(command, helper.Progaifecregistro, DbType.DateTime, entity.Progaifecregistro);
            dbProvider.AddInParameter(command, helper.Progaiusumodificacion, DbType.String, entity.Progaiusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaifecmodificacion, DbType.DateTime, entity.Progaifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudProgaudiInvolucradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoinvolucrado, DbType.Int32, entity.Tabcdcoditipoinvolucrado);
            dbProvider.AddInParameter(command, helper.Percodiinvolucrado, DbType.Int32, entity.Percodiinvolucrado);
            dbProvider.AddInParameter(command, helper.Progaiactivo, DbType.String, entity.Progaiactivo);
            dbProvider.AddInParameter(command, helper.Progaihistorico, DbType.String, entity.Progaihistorico);
            dbProvider.AddInParameter(command, helper.Progaiusuregistro, DbType.String, entity.Progaiusuregistro);
            dbProvider.AddInParameter(command, helper.Progaifecregistro, DbType.DateTime, entity.Progaifecregistro);
            dbProvider.AddInParameter(command, helper.Progaiusumodificacion, DbType.String, entity.Progaiusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaifecmodificacion, DbType.DateTime, entity.Progaifecmodificacion);
            dbProvider.AddInParameter(command, helper.Progaicodi, DbType.Int32, entity.Progaicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudProgaudiInvolucradoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Progaiusumodificacion, DbType.String, entity.Progaiusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaifecmodificacion, DbType.DateTime, entity.Progaifecmodificacion);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoinvolucrado, DbType.Int32, entity.Tabcdcoditipoinvolucrado);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudProgaudiInvolucradoDTO GetById(int progaicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Progaicodi, DbType.Int32, progaicodi);
            AudProgaudiInvolucradoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public AudProgaudiInvolucradoDTO GetByIdinvolucrado(int progacodi, int percodi)
        {
            string sql = string.Format(helper.SqlGetByIdinvolucrado, progacodi, percodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            AudProgaudiInvolucradoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<AudProgaudiInvolucradoDTO> List(int audicodi)
        {
            List<AudProgaudiInvolucradoDTO> entitys = new List<AudProgaudiInvolucradoDTO>();

            string sql = string.Format(helper.SqlList, audicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiInvolucradoDTO involucrado = new AudProgaudiInvolucradoDTO();

                    int iResponsable = dr.GetOrdinal(helper.Responsable);
                    if (!dr.IsDBNull(iResponsable)) involucrado.Responsable = dr.GetString(iResponsable);

                    int iPeremail = dr.GetOrdinal(helper.Peremail);
                    if (!dr.IsDBNull(iPeremail)) involucrado.Peremail = dr.GetString(iPeremail);

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) involucrado.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iTabcdcoditipoinvolucrado = dr.GetOrdinal(helper.Tabcdcoditipoinvolucrado);
                    if (!dr.IsDBNull(iTabcdcoditipoinvolucrado)) involucrado.Tabcdcoditipoinvolucrado = Convert.ToInt32(dr.GetValue(iTabcdcoditipoinvolucrado));

                    entitys.Add(involucrado);
                }
            }

            return entitys;
        }

        public List<AudProgaudiInvolucradoDTO> GetByCriteria(int progacodi)
        {
            List<AudProgaudiInvolucradoDTO> entitys = new List<AudProgaudiInvolucradoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, progacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiInvolucradoDTO involucrado = helper.Create(dr);

                    int iResponsable = dr.GetOrdinal(helper.Responsable);
                    if (!dr.IsDBNull(iResponsable)) involucrado.Responsable = dr.GetString(iResponsable);

                    int iPeremail = dr.GetOrdinal(helper.Peremail);
                    if (!dr.IsDBNull(iPeremail)) involucrado.Peremail = dr.GetString(iPeremail);

                    int iPercodi = dr.GetOrdinal(helper.Percodi);
                    if (!dr.IsDBNull(iPercodi)) involucrado.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) involucrado.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(involucrado);
                }
            }

            return entitys;
        }
    }
}
