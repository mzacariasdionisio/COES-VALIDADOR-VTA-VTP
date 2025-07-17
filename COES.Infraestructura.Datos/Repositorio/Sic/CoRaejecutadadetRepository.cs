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
    /// Clase de acceso a datos de la tabla CO_RAEJECUTADADET
    /// </summary>
    public class CoRaejecutadadetRepository: RepositoryBase, ICoRaejecutadadetRepository
    {
        public CoRaejecutadadetRepository(string strConn): base(strConn)
        {
        }

        CoRaejecutadadetHelper helper = new CoRaejecutadadetHelper();

        public int Save(CoRaejecutadadetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Coradecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Coradefecha, DbType.DateTime, entity.Coradefecha);
            dbProvider.AddInParameter(command, helper.Coradeindice, DbType.Int32, entity.Coradeindice);
            dbProvider.AddInParameter(command, helper.Corademinutos, DbType.Int32, entity.Corademinutos);
            dbProvider.AddInParameter(command, helper.Coraderasub, DbType.Decimal, entity.Coraderasub);
            dbProvider.AddInParameter(command, helper.Coraderabaj, DbType.Decimal, entity.Coraderabaj);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CoRaejecutadadetDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Coradefecha, DbType.DateTime, entity.Coradefecha);
            dbProvider.AddInParameter(command, helper.Coradeindice, DbType.Int32, entity.Coradeindice);
            dbProvider.AddInParameter(command, helper.Corademinutos, DbType.Int32, entity.Corademinutos);
            dbProvider.AddInParameter(command, helper.Coraderasub, DbType.Decimal, entity.Coraderasub);
            dbProvider.AddInParameter(command, helper.Coraderabaj, DbType.Decimal, entity.Coraderabaj);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, entity.Copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, entity.Covercodi);
            dbProvider.AddInParameter(command, helper.Coradecodi, DbType.Int32, entity.Coradecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int copercodi, int covercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Copercodi, DbType.Int32, copercodi);
            dbProvider.AddInParameter(command, helper.Covercodi, DbType.Int32, covercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CoRaejecutadadetDTO GetById(int coradecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Coradecodi, DbType.Int32, coradecodi);
            CoRaejecutadadetDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CoRaejecutadadetDTO> List()
        {
            List<CoRaejecutadadetDTO> entitys = new List<CoRaejecutadadetDTO>();
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

        public List<CoRaejecutadadetDTO> GetByCriteria(int periodo, int version, DateTime fecha)
        {
            List<CoRaejecutadadetDTO> entitys = new List<CoRaejecutadadetDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, periodo, version, fecha.ToString(ConstantesBase.FormatoFecha));
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

        public List<CoRaejecutadadetDTO> ObtenerConsulta(int periodo, int version)
        {
            List<CoRaejecutadadetDTO> entitys = new List<CoRaejecutadadetDTO>();
            string sql = string.Format(helper.SqlObtenerConsulta, periodo, version);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CoRaejecutadadetDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int minutos = (int)entity.Coradeindice;

                    int d = minutos / 2;
                    int c = minutos % 2;

                    DateTime fecha = ((DateTime)entity.Coradefecha);
                    DateTime fecha1 = fecha.AddHours(d).AddMinutes((c != 0) ? 30 : 0);
                    DateTime fecha2 = fecha1.AddMinutes(-30);

                    entity.Bloquehorario = fecha2.ToString("HH:mm") + " - " + fecha1.ToString("HH:mm");

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
