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
    /// Clase de acceso a datos de la tabla SRM_ESTADO
    /// </summary>
    public class SrmEstadoRepository: RepositoryBase, ISrmEstadoRepository
    {
        public SrmEstadoRepository(string strConn): base(strConn)
        {
        }

        SrmEstadoHelper helper = new SrmEstadoHelper();

        public int Save(SrmEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Srmstddescrip, DbType.String, entity.Srmstddescrip);
            dbProvider.AddInParameter(command, helper.Srmstdcolor, DbType.String, entity.Srmstdcolor);
            dbProvider.AddInParameter(command, helper.Srmstdusucreacion, DbType.String, entity.Srmstdusucreacion);
            dbProvider.AddInParameter(command, helper.Srmstdfeccreacion, DbType.DateTime, entity.Srmstdfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmstdsumodificacion, DbType.String, entity.Srmstdsumodificacion);
            dbProvider.AddInParameter(command, helper.Srmstdfecmodificacion, DbType.DateTime, entity.Srmstdfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SrmEstadoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Srmstddescrip, DbType.String, entity.Srmstddescrip);
            dbProvider.AddInParameter(command, helper.Srmstdcolor, DbType.String, entity.Srmstdcolor);
            dbProvider.AddInParameter(command, helper.Srmstdusucreacion, DbType.String, entity.Srmstdusucreacion);
            dbProvider.AddInParameter(command, helper.Srmstdfeccreacion, DbType.DateTime, entity.Srmstdfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmstdsumodificacion, DbType.String, entity.Srmstdsumodificacion);
            dbProvider.AddInParameter(command, helper.Srmstdfecmodificacion, DbType.DateTime, entity.Srmstdfecmodificacion);
            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, entity.Srmstdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int srmstdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, srmstdcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SrmEstadoDTO GetById(int srmstdcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Srmstdcodi, DbType.Int32, srmstdcodi);
            SrmEstadoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SrmEstadoDTO> List()
        {
            List<SrmEstadoDTO> entitys = new List<SrmEstadoDTO>();
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

        public List<SrmEstadoDTO> GetByCriteria()
        {
            List<SrmEstadoDTO> entitys = new List<SrmEstadoDTO>();
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
        /// Graba los datos de la tabla SRM_ESTADO
        /// </summary>
        public int SaveSrmEstadoId(SrmEstadoDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Srmstdcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Srmstdcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<SrmEstadoDTO> BuscarOperaciones(DateTime srmstdFeccreacion,DateTime srmstdFecmodificacion, int nroPage, int pageSize)
        {
            List<SrmEstadoDTO> entitys = new List<SrmEstadoDTO>();
            String sql = String.Format(this.helper.ObtenerListado, srmstdFeccreacion.ToString(ConstantesBase.FormatoFecha),srmstdFecmodificacion.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmEstadoDTO entity = new SrmEstadoDTO();

                    int iSrmstdcodi = dr.GetOrdinal(this.helper.Srmstdcodi);
                    if (!dr.IsDBNull(iSrmstdcodi)) entity.Srmstdcodi = Convert.ToInt32(dr.GetValue(iSrmstdcodi));

                    int iSrmstddescrip = dr.GetOrdinal(this.helper.Srmstddescrip);
                    if (!dr.IsDBNull(iSrmstddescrip)) entity.Srmstddescrip = dr.GetString(iSrmstddescrip);

                    int iSrmstdcolor = dr.GetOrdinal(this.helper.Srmstdcolor);
                    if (!dr.IsDBNull(iSrmstdcolor)) entity.Srmstdcolor = dr.GetString(iSrmstdcolor);

                    int iSrmstdusucreacion = dr.GetOrdinal(this.helper.Srmstdusucreacion);
                    if (!dr.IsDBNull(iSrmstdusucreacion)) entity.Srmstdusucreacion = dr.GetString(iSrmstdusucreacion);

                    int iSrmstdfeccreacion = dr.GetOrdinal(this.helper.Srmstdfeccreacion);
                    if (!dr.IsDBNull(iSrmstdfeccreacion)) entity.Srmstdfeccreacion = dr.GetDateTime(iSrmstdfeccreacion);

                    int iSrmstdsumodificacion = dr.GetOrdinal(this.helper.Srmstdsumodificacion);
                    if (!dr.IsDBNull(iSrmstdsumodificacion)) entity.Srmstdsumodificacion = dr.GetString(iSrmstdsumodificacion);

                    int iSrmstdfecmodificacion = dr.GetOrdinal(this.helper.Srmstdfecmodificacion);
                    if (!dr.IsDBNull(iSrmstdfecmodificacion)) entity.Srmstdfecmodificacion = dr.GetDateTime(iSrmstdfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(DateTime srmstdFeccreacion,DateTime srmstdFecmodificacion)
        {
            String sql = String.Format(this.helper.TotalRegistros, srmstdFeccreacion.ToString(ConstantesBase.FormatoFecha),srmstdFecmodificacion.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
