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
    /// Clase de acceso a datos de la tabla SRM_CRITICIDAD
    /// </summary>
    public class SrmCriticidadRepository: RepositoryBase, ISrmCriticidadRepository
    {
        public SrmCriticidadRepository(string strConn): base(strConn)
        {
        }

        SrmCriticidadHelper helper = new SrmCriticidadHelper();

        public int Save(SrmCriticidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Srmcrtdescrip, DbType.String, entity.Srmcrtdescrip);
            dbProvider.AddInParameter(command, helper.Srmcrtcolor, DbType.String, entity.Srmcrtcolor);
            dbProvider.AddInParameter(command, helper.Srmcrtusucreacion, DbType.String, entity.Srmcrtusucreacion);
            dbProvider.AddInParameter(command, helper.Srmcrtfeccreacion, DbType.DateTime, entity.Srmcrtfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmcrtusumodificacion, DbType.String, entity.Srmcrtusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmcrtfecmodificacion, DbType.DateTime, entity.Srmcrtfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SrmCriticidadDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Srmcrtdescrip, DbType.String, entity.Srmcrtdescrip);
            dbProvider.AddInParameter(command, helper.Srmcrtcolor, DbType.String, entity.Srmcrtcolor);
            dbProvider.AddInParameter(command, helper.Srmcrtusucreacion, DbType.String, entity.Srmcrtusucreacion);
            dbProvider.AddInParameter(command, helper.Srmcrtfeccreacion, DbType.DateTime, entity.Srmcrtfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmcrtusumodificacion, DbType.String, entity.Srmcrtusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmcrtfecmodificacion, DbType.DateTime, entity.Srmcrtfecmodificacion);
            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, entity.Srmcrtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int srmcrtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, srmcrtcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SrmCriticidadDTO GetById(int srmcrtcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Srmcrtcodi, DbType.Int32, srmcrtcodi);
            SrmCriticidadDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SrmCriticidadDTO> List()
        {
            List<SrmCriticidadDTO> entitys = new List<SrmCriticidadDTO>();
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

        public List<SrmCriticidadDTO> GetByCriteria()
        {
            List<SrmCriticidadDTO> entitys = new List<SrmCriticidadDTO>();
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
        /// Graba los datos de la tabla SRM_CRITICIDAD
        /// </summary>
        public int SaveSrmCriticidadId(SrmCriticidadDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Srmcrtcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Srmcrtcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<SrmCriticidadDTO> BuscarOperaciones(DateTime srmcrtFeccreacion,DateTime srmcrtFecmodificacion, int nroPage, int pageSize)
        {
            List<SrmCriticidadDTO> entitys = new List<SrmCriticidadDTO>();
            String sql = String.Format(this.helper.ObtenerListado, srmcrtFeccreacion.ToString(ConstantesBase.FormatoFecha),srmcrtFecmodificacion.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmCriticidadDTO entity = new SrmCriticidadDTO();

                    int iSrmcrtcodi = dr.GetOrdinal(this.helper.Srmcrtcodi);
                    if (!dr.IsDBNull(iSrmcrtcodi)) entity.Srmcrtcodi = Convert.ToInt32(dr.GetValue(iSrmcrtcodi));

                    int iSrmcrtdescrip = dr.GetOrdinal(this.helper.Srmcrtdescrip);
                    if (!dr.IsDBNull(iSrmcrtdescrip)) entity.Srmcrtdescrip = dr.GetString(iSrmcrtdescrip);

                    int iSrmcrtcolor = dr.GetOrdinal(this.helper.Srmcrtcolor);
                    if (!dr.IsDBNull(iSrmcrtcolor)) entity.Srmcrtcolor = dr.GetString(iSrmcrtcolor);

                    int iSrmcrtusucreacion = dr.GetOrdinal(this.helper.Srmcrtusucreacion);
                    if (!dr.IsDBNull(iSrmcrtusucreacion)) entity.Srmcrtusucreacion = dr.GetString(iSrmcrtusucreacion);

                    int iSrmcrtfeccreacion = dr.GetOrdinal(this.helper.Srmcrtfeccreacion);
                    if (!dr.IsDBNull(iSrmcrtfeccreacion)) entity.Srmcrtfeccreacion = dr.GetDateTime(iSrmcrtfeccreacion);

                    int iSrmcrtusumodificacion = dr.GetOrdinal(this.helper.Srmcrtusumodificacion);
                    if (!dr.IsDBNull(iSrmcrtusumodificacion)) entity.Srmcrtusumodificacion = dr.GetString(iSrmcrtusumodificacion);

                    int iSrmcrtfecmodificacion = dr.GetOrdinal(this.helper.Srmcrtfecmodificacion);
                    if (!dr.IsDBNull(iSrmcrtfecmodificacion)) entity.Srmcrtfecmodificacion = dr.GetDateTime(iSrmcrtfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime srmcrtFeccreacion,DateTime srmcrtFecmodificacion)
        {
            String sql = String.Format(this.helper.TotalRegistros, srmcrtFeccreacion.ToString(ConstantesBase.FormatoFecha),srmcrtFecmodificacion.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
