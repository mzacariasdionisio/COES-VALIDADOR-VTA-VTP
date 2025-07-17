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
    /// Clase de acceso a datos de la tabla CM_EQUIPOBARRA
    /// </summary>
    public class CmEquipobarraRepository: RepositoryBase, ICmEquipobarraRepository
    {
        public CmEquipobarraRepository(string strConn): base(strConn)
        {
        }

        CmEquipobarraHelper helper = new CmEquipobarraHelper();

        public int Save(CmEquipobarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Cmeqbaestado, DbType.String, entity.Cmeqbaestado);
            dbProvider.AddInParameter(command, helper.Cmeqbavigencia, DbType.DateTime, entity.Cmeqbavigencia);
            dbProvider.AddInParameter(command, helper.Cmeqbaexpira, DbType.DateTime, entity.Cmeqbaexpira);
            dbProvider.AddInParameter(command, helper.Cmeqbausucreacion, DbType.String, entity.Cmeqbausucreacion);
            dbProvider.AddInParameter(command, helper.Cmeqbafeccreacion, DbType.DateTime, entity.Cmeqbafeccreacion);
            dbProvider.AddInParameter(command, helper.Cmeqbausumodificacion, DbType.String, entity.Cmeqbausumodificacion);
            dbProvider.AddInParameter(command, helper.Cmeqbafecmodificacion, DbType.DateTime, entity.Cmeqbafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmEquipobarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Configcodi, DbType.Int32, entity.Configcodi);
            dbProvider.AddInParameter(command, helper.Cmeqbaestado, DbType.String, entity.Cmeqbaestado);
            dbProvider.AddInParameter(command, helper.Cmeqbavigencia, DbType.DateTime, entity.Cmeqbavigencia);
            dbProvider.AddInParameter(command, helper.Cmeqbaexpira, DbType.DateTime, entity.Cmeqbaexpira);
            dbProvider.AddInParameter(command, helper.Cmeqbausucreacion, DbType.String, entity.Cmeqbausucreacion);
            dbProvider.AddInParameter(command, helper.Cmeqbafeccreacion, DbType.DateTime, entity.Cmeqbafeccreacion);
            dbProvider.AddInParameter(command, helper.Cmeqbausumodificacion, DbType.String, entity.Cmeqbausumodificacion);
            dbProvider.AddInParameter(command, helper.Cmeqbafecmodificacion, DbType.DateTime, entity.Cmeqbafecmodificacion);
            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, entity.Cmeqbacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int cmeqbacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, cmeqbacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmEquipobarraDTO GetById(int cmeqbacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Cmeqbacodi, DbType.Int32, cmeqbacodi);
            CmEquipobarraDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmEquipobarraDTO> List()
        {
            List<CmEquipobarraDTO> entitys = new List<CmEquipobarraDTO>();
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

        public List<CmEquipobarraDTO> GetByCriteria(DateTime fecha)
        {
            List<CmEquipobarraDTO> entitys = new List<CmEquipobarraDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquipobarraDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entity.Vigencia = (entity.Cmeqbavigencia != null) ?
                       ((DateTime)entity.Cmeqbavigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmeqbafecmodificacion != null) ?
                        ((DateTime)entity.Cmeqbafecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmEquipobarraDTO> ObtenerHistorico(int idConfig)
        {
            List<CmEquipobarraDTO> entitys = new List<CmEquipobarraDTO>();
            string sql = string.Format(helper.SqlOtenerHistorico, idConfig);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquipobarraDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entity.Vigencia = (entity.Cmeqbavigencia != null) ?
                       ((DateTime)entity.Cmeqbavigencia).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Modificacion = (entity.Cmeqbafecmodificacion != null) ?
                        ((DateTime)entity.Cmeqbafecmodificacion).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entity.Expiracion = (entity.Cmeqbaexpira != null) ?
                      ((DateTime)entity.Cmeqbaexpira).ToString(ConstantesBase.FormatoFechaBase) : string.Empty;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
