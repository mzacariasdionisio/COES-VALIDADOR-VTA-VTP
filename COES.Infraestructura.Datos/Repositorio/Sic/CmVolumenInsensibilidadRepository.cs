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
    /// Clase de acceso a datos de la tabla CM_VOLUMEN_INSENSIBILIDAD
    /// </summary>
    public class CmVolumenInsensibilidadRepository: RepositoryBase, ICmVolumenInsensibilidadRepository
    {
        public CmVolumenInsensibilidadRepository(string strConn): base(strConn)
        {
        }

        CmVolumenInsensibilidadHelper helper = new CmVolumenInsensibilidadHelper();

        public int Save(CmVolumenInsensibilidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Volinscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Volinsfecha, DbType.DateTime, entity.Volinsfecha);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Volinsvolmin, DbType.Decimal, entity.Volinsvolmin);
            dbProvider.AddInParameter(command, helper.Volinsvolmax, DbType.Decimal, entity.Volinsvolmax);
            dbProvider.AddInParameter(command, helper.Volinsinicio, DbType.DateTime, entity.Volinsinicio);
            dbProvider.AddInParameter(command, helper.Volinsfin, DbType.DateTime, entity.Volinsfin);
            dbProvider.AddInParameter(command, helper.Volinsusucreacion, DbType.String, entity.Volinsusucreacion);
            dbProvider.AddInParameter(command, helper.Volinsfecreacion, DbType.DateTime, entity.Volinsfecreacion);
            dbProvider.AddInParameter(command, helper.Volinsusumodificacion, DbType.String, entity.Volinsusumodificacion);
            dbProvider.AddInParameter(command, helper.Volinsfecmodificacion, DbType.DateTime, entity.Volinsfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmVolumenInsensibilidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Volinsfecha, DbType.DateTime, entity.Volinsfecha);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Volinsvolmin, DbType.Decimal, entity.Volinsvolmin);
            dbProvider.AddInParameter(command, helper.Volinsvolmax, DbType.Decimal, entity.Volinsvolmax);
            dbProvider.AddInParameter(command, helper.Volinsinicio, DbType.DateTime, entity.Volinsinicio);
            dbProvider.AddInParameter(command, helper.Volinsfin, DbType.DateTime, entity.Volinsfin);
            dbProvider.AddInParameter(command, helper.Volinsusucreacion, DbType.String, entity.Volinsusucreacion);
            dbProvider.AddInParameter(command, helper.Volinsfecreacion, DbType.DateTime, entity.Volinsfecreacion);
            dbProvider.AddInParameter(command, helper.Volinsusumodificacion, DbType.String, entity.Volinsusumodificacion);
            dbProvider.AddInParameter(command, helper.Volinsfecmodificacion, DbType.DateTime, entity.Volinsfecmodificacion);
            dbProvider.AddInParameter(command, helper.Volinscodi, DbType.Int32, entity.Volinscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int volinscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Volinscodi, DbType.Int32, volinscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmVolumenInsensibilidadDTO GetById(int volinscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Volinscodi, DbType.Int32, volinscodi);
            CmVolumenInsensibilidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmVolumenInsensibilidadDTO> List()
        {
            List<CmVolumenInsensibilidadDTO> entitys = new List<CmVolumenInsensibilidadDTO>();
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

        public List<CmVolumenInsensibilidadDTO> GetByCriteria(DateTime fecha)
        {
            List<CmVolumenInsensibilidadDTO> entitys = new List<CmVolumenInsensibilidadDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
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

        public List<CmVolumenInsensibilidadDTO> ObtenerRegistros(DateTime fecha)
        {
            List<CmVolumenInsensibilidadDTO> entitys = new List<CmVolumenInsensibilidadDTO>();
            string sql = string.Format(helper.SqlObtenerRegistros, fecha.ToString(ConstantesBase.FormatoFechaHora));
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
    }
}
