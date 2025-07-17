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
    /// Clase de acceso a datos de la tabla RI_HISTORICO
    /// </summary>
    public class RiHistoricoRepository: RepositoryBase, IRiHistoricoRepository
    {
        public RiHistoricoRepository(string strConn): base(strConn)
        {
        }

        RiHistoricoHelper helper = new RiHistoricoHelper();

        public int Save(RiHistoricoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Hisricodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hisrianio, DbType.Int32, entity.Hisrianio);
            dbProvider.AddInParameter(command, helper.Hisritipo, DbType.String, entity.Hisritipo);
            dbProvider.AddInParameter(command, helper.Hisridesc, DbType.String, entity.Hisridesc);
            dbProvider.AddInParameter(command, helper.Hisrifecha, DbType.DateTime, entity.Hisrifecha);
            dbProvider.AddInParameter(command, helper.Hisriestado, DbType.String, entity.Hisriestado);
            dbProvider.AddInParameter(command, helper.Hisriusucreacion, DbType.String, entity.Hisriusucreacion);
            dbProvider.AddInParameter(command, helper.Hisrifeccreacion, DbType.DateTime, entity.Hisrifeccreacion);
            dbProvider.AddInParameter(command, helper.Hisriusumodificacion, DbType.String, entity.Hisriusumodificacion);
            dbProvider.AddInParameter(command, helper.Hisrifecmodificacion, DbType.DateTime, entity.Hisrifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RiHistoricoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Hisrianio, DbType.Int32, entity.Hisrianio);
            dbProvider.AddInParameter(command, helper.Hisritipo, DbType.String, entity.Hisritipo);
            dbProvider.AddInParameter(command, helper.Hisridesc, DbType.String, entity.Hisridesc);
            dbProvider.AddInParameter(command, helper.Hisrifecha, DbType.DateTime, entity.Hisrifecha);
            dbProvider.AddInParameter(command, helper.Hisriestado, DbType.String, entity.Hisriestado);
            dbProvider.AddInParameter(command, helper.Hisriusucreacion, DbType.String, entity.Hisriusucreacion);
            dbProvider.AddInParameter(command, helper.Hisrifeccreacion, DbType.DateTime, entity.Hisrifeccreacion);
            dbProvider.AddInParameter(command, helper.Hisriusumodificacion, DbType.String, entity.Hisriusumodificacion);
            dbProvider.AddInParameter(command, helper.Hisrifecmodificacion, DbType.DateTime, entity.Hisrifecmodificacion);
            dbProvider.AddInParameter(command, helper.Hisricodi, DbType.Int32, entity.Hisricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hisricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hisricodi, DbType.Int32, hisricodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RiHistoricoDTO GetById(int hisricodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hisricodi, DbType.Int32, hisricodi);
            RiHistoricoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RiHistoricoDTO> List()
        {
            List<RiHistoricoDTO> entitys = new List<RiHistoricoDTO>();
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
        public List<RiHistoricoDTO> ListAnios()
        {
            List<RiHistoricoDTO> entitys = new List<RiHistoricoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAnio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RiHistoricoDTO entity = new RiHistoricoDTO();
                    int iHistianio = dr.GetOrdinal(helper.Hisrianio);
                    if (!dr.IsDBNull(iHistianio)) entity.Hisrianio = Convert.ToInt32(dr.GetValue(iHistianio).ToString());

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RiHistoricoDTO> GetByCriteria(int anio, string tipo)
        {
            List<RiHistoricoDTO> entitys = new List<RiHistoricoDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, anio, tipo);
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

        public List<RiHistoricoDTO> ObtenerPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            List<RiHistoricoDTO> entitys = new List<RiHistoricoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerPorFecha, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}
