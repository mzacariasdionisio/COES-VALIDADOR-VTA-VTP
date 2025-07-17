using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;
using System.Data.Odbc;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla ME_SCADA_FILTRO_SP7
    /// </summary>
    public class MeScadaFiltroSp7Repository: RepositoryBase, IMeScadaFiltroSp7Repository
    {
        public MeScadaFiltroSp7Repository(string strConn): base(strConn)
        {
        }

        MeScadaFiltroSp7Helper helper = new MeScadaFiltroSp7Helper();

        public int Save(MeScadaFiltroSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Filtronomb, DbType.String, entity.Filtronomb);
            dbProvider.AddInParameter(command, helper.Filtrouser, DbType.String, entity.Filtrouser);
            dbProvider.AddInParameter(command, helper.Scdfifeccreacion, DbType.DateTime, entity.Scdfifeccreacion);
            dbProvider.AddInParameter(command, helper.Scdfiusumodificacion, DbType.String, entity.Scdfiusumodificacion);
            dbProvider.AddInParameter(command, helper.Scdfifecmodificacion, DbType.DateTime, entity.Scdfifecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(MeScadaFiltroSp7DTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Filtronomb, DbType.String, entity.Filtronomb);
            dbProvider.AddInParameter(command, helper.Filtrouser, DbType.String, entity.Filtrouser);
            dbProvider.AddInParameter(command, helper.Scdfifeccreacion, DbType.DateTime, entity.Scdfifeccreacion);
            dbProvider.AddInParameter(command, helper.Scdfiusumodificacion, DbType.String, entity.Scdfiusumodificacion);
            dbProvider.AddInParameter(command, helper.Scdfifecmodificacion, DbType.DateTime, entity.Scdfifecmodificacion);
            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, entity.Filtrocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int filtrocodi)
        {

           

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, filtrocodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public MeScadaFiltroSp7DTO GetById(int filtrocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Filtrocodi, DbType.Int32, filtrocodi);
            MeScadaFiltroSp7DTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<MeScadaFiltroSp7DTO> List()
        {
            List<MeScadaFiltroSp7DTO> entitys = new List<MeScadaFiltroSp7DTO>();
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

        public List<MeScadaFiltroSp7DTO> GetByCriteria()
        {
            List<MeScadaFiltroSp7DTO> entitys = new List<MeScadaFiltroSp7DTO>();
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
        /// <summary>
        /// Graba los datos de la tabla ME_SCADA_FILTRO_SP7
        /// </summary>
        public int SaveMeScadaFiltroSp7Id(MeScadaFiltroSp7DTO entity)
        {
            try
            {
                int id = 0;
                
                if (entity.Filtrocodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Filtrocodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<MeScadaFiltroSp7DTO> BuscarOperaciones(string filtro, string creador, string modificador, int nroPage, int pageSize)
        {
            List<MeScadaFiltroSp7DTO> entitys = new List<MeScadaFiltroSp7DTO>();
            String sql = String.Format(this.helper.ObtenerListado, filtro, creador, modificador, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MeScadaFiltroSp7DTO entity = new MeScadaFiltroSp7DTO();

                    int iFiltrocodi = dr.GetOrdinal(this.helper.Filtrocodi);
                    if (!dr.IsDBNull(iFiltrocodi)) entity.Filtrocodi = Convert.ToInt32(dr.GetValue(iFiltrocodi));

                    int iFiltronomb = dr.GetOrdinal(this.helper.Filtronomb);
                    if (!dr.IsDBNull(iFiltronomb)) entity.Filtronomb = dr.GetString(iFiltronomb);

                    int iFiltrouser = dr.GetOrdinal(this.helper.Filtrouser);
                    if (!dr.IsDBNull(iFiltrouser)) entity.Filtrouser = dr.GetString(iFiltrouser);

                    int iScdfifeccreacion = dr.GetOrdinal(this.helper.Scdfifeccreacion);
                    if (!dr.IsDBNull(iScdfifeccreacion)) entity.Scdfifeccreacion = dr.GetDateTime(iScdfifeccreacion);

                    int iScdfiusumodificacion = dr.GetOrdinal(this.helper.Scdfiusumodificacion);
                    if (!dr.IsDBNull(iScdfiusumodificacion)) entity.Scdfiusumodificacion = dr.GetString(iScdfiusumodificacion);

                    int iScdfifecmodificacion = dr.GetOrdinal(this.helper.Scdfifecmodificacion);
                    if (!dr.IsDBNull(iScdfifecmodificacion)) entity.Scdfifecmodificacion = dr.GetDateTime(iScdfifecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string filtro, string creador, string modificador)
        {
            List<MeScadaFiltroSp7DTO> entitys = new List<MeScadaFiltroSp7DTO>();
            String sql = String.Format(this.helper.TotalRegistros, filtro, creador, modificador);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
