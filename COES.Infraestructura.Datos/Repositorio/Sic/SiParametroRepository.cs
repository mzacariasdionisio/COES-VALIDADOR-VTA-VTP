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
    /// Clase de acceso a datos de la tabla SI_PARAMETRO
    /// </summary>
    public class SiParametroRepository: RepositoryBase, ISiParametroRepository
    {
        public SiParametroRepository(string strConn): base(strConn)
        {
        }

        SiParametroHelper helper = new SiParametroHelper();

        public int Save(SiParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Siparabrev, DbType.String, entity.Siparabrev);
            dbProvider.AddInParameter(command, helper.Sipardescripcion, DbType.String, entity.Sipardescripcion);
            dbProvider.AddInParameter(command, helper.Siparusucreacion, DbType.String, entity.Siparusucreacion);
            dbProvider.AddInParameter(command, helper.Siparfeccreacion, DbType.DateTime, entity.Siparfeccreacion);
            dbProvider.AddInParameter(command, helper.Siparusumodificacion, DbType.String, entity.Siparusumodificacion);
            dbProvider.AddInParameter(command, helper.Siparfecmodificacion, DbType.DateTime, entity.Siparfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiParametroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Siparabrev, DbType.String, entity.Siparabrev);
            dbProvider.AddInParameter(command, helper.Sipardescripcion, DbType.String, entity.Sipardescripcion);
            dbProvider.AddInParameter(command, helper.Siparusucreacion, DbType.String, entity.Siparusucreacion);
            dbProvider.AddInParameter(command, helper.Siparfeccreacion, DbType.DateTime, entity.Siparfeccreacion);
            dbProvider.AddInParameter(command, helper.Siparusumodificacion, DbType.String, entity.Siparusumodificacion);
            dbProvider.AddInParameter(command, helper.Siparfecmodificacion, DbType.DateTime, entity.Siparfecmodificacion);
            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, entity.Siparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int siparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, siparcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiParametroDTO GetById(int siparcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Siparcodi, DbType.Int32, siparcodi);
            SiParametroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiParametroDTO> List()
        {
            List<SiParametroDTO> entitys = new List<SiParametroDTO>();
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

        public List<SiParametroDTO> GetByCriteria()
        {
            List<SiParametroDTO> entitys = new List<SiParametroDTO>();
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
        /// Graba los datos de la tabla SI_PARAMETRO
        /// </summary>
        public int SaveSiParametroId(SiParametroDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Siparcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Siparcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<SiParametroDTO> BuscarOperaciones(string abreviatura,string descripcion, int nroPage, int pageSize)
        {
            List<SiParametroDTO> entitys = new List<SiParametroDTO>();
            String sql = String.Format(this.helper.ObtenerListado, abreviatura, descripcion, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiParametroDTO entity = new SiParametroDTO();

                    int iSiparcodi = dr.GetOrdinal(this.helper.Siparcodi);
                    if (!dr.IsDBNull(iSiparcodi)) entity.Siparcodi = Convert.ToInt32(dr.GetValue(iSiparcodi));

                    int iSiparabrev = dr.GetOrdinal(this.helper.Siparabrev);
                    if (!dr.IsDBNull(iSiparabrev)) entity.Siparabrev = dr.GetString(iSiparabrev);

                    int iSipardescripcion = dr.GetOrdinal(this.helper.Sipardescripcion);
                    if (!dr.IsDBNull(iSipardescripcion)) entity.Sipardescripcion = dr.GetString(iSipardescripcion);

                    int iSiparusucreacion = dr.GetOrdinal(this.helper.Siparusucreacion);
                    if (!dr.IsDBNull(iSiparusucreacion)) entity.Siparusucreacion = dr.GetString(iSiparusucreacion);

                    int iSiparfeccreacion = dr.GetOrdinal(this.helper.Siparfeccreacion);
                    if (!dr.IsDBNull(iSiparfeccreacion)) entity.Siparfeccreacion = dr.GetDateTime(iSiparfeccreacion);

                    int iSiparusumodificacion = dr.GetOrdinal(this.helper.Siparusumodificacion);
                    if (!dr.IsDBNull(iSiparusumodificacion)) entity.Siparusumodificacion = dr.GetString(iSiparusumodificacion);

                    int iSiparfecmodificacion = dr.GetOrdinal(this.helper.Siparfecmodificacion);
                    if (!dr.IsDBNull(iSiparfecmodificacion)) entity.Siparfecmodificacion = dr.GetDateTime(iSiparfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string abreviatura, string descripcion)
        {
            String sql = String.Format(this.helper.TotalRegistros, abreviatura, descripcion);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
