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
    /// Clase de acceso a datos de la tabla SRM_COMENTARIO
    /// </summary>
    public class SrmComentarioRepository: RepositoryBase, ISrmComentarioRepository
    {
        public SrmComentarioRepository(string strConn): base(strConn)
        {
        }

        SrmComentarioHelper helper = new SrmComentarioHelper();

        public int Save(SrmComentarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Srmcomcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, entity.Srmreccodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Srmcomfechacoment, DbType.DateTime, entity.Srmcomfechacoment);
            dbProvider.AddInParameter(command, helper.Srmcomgruporespons, DbType.String, entity.Srmcomgruporespons);
            dbProvider.AddInParameter(command, helper.Srmcomcomentario, DbType.String, entity.Srmcomcomentario);
            dbProvider.AddInParameter(command, helper.Srmcomactivo, DbType.String, entity.Srmcomactivo);
            dbProvider.AddInParameter(command, helper.Srmcomusucreacion, DbType.String, entity.Srmcomusucreacion);
            dbProvider.AddInParameter(command, helper.Srmcomfeccreacion, DbType.DateTime, entity.Srmcomfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmcomusumodificacion, DbType.String, entity.Srmcomusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmcomfecmodificacion, DbType.DateTime, entity.Srmcomfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SrmComentarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Srmreccodi, DbType.Int32, entity.Srmreccodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Srmcomfechacoment, DbType.DateTime, entity.Srmcomfechacoment);
            dbProvider.AddInParameter(command, helper.Srmcomgruporespons, DbType.String, entity.Srmcomgruporespons);
            dbProvider.AddInParameter(command, helper.Srmcomcomentario, DbType.String, entity.Srmcomcomentario);
            dbProvider.AddInParameter(command, helper.Srmcomactivo, DbType.String, entity.Srmcomactivo);
            dbProvider.AddInParameter(command, helper.Srmcomusucreacion, DbType.String, entity.Srmcomusucreacion);
            dbProvider.AddInParameter(command, helper.Srmcomfeccreacion, DbType.DateTime, entity.Srmcomfeccreacion);
            dbProvider.AddInParameter(command, helper.Srmcomusumodificacion, DbType.String, entity.Srmcomusumodificacion);
            dbProvider.AddInParameter(command, helper.Srmcomfecmodificacion, DbType.DateTime, entity.Srmcomfecmodificacion);
            dbProvider.AddInParameter(command, helper.Srmcomcodi, DbType.Int32, entity.Srmcomcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int srmcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Srmcomcodi, DbType.Int32, srmcomcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SrmComentarioDTO GetById(int srmcomcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Srmcomcodi, DbType.Int32, srmcomcodi);
            SrmComentarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SrmComentarioDTO> List()
        {
            List<SrmComentarioDTO> entitys = new List<SrmComentarioDTO>();
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

        public List<SrmComentarioDTO> GetByCriteria()
        {
            List<SrmComentarioDTO> entitys = new List<SrmComentarioDTO>();
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
        /// Graba los datos de la tabla SRM_COMENTARIO
        /// </summary>
        public int SaveSrmComentarioId(SrmComentarioDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Srmcomcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Srmcomcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<SrmComentarioDTO> BuscarOperaciones(int srmreccodi, string activo, int nroPage, int pageSize)
        {
            List<SrmComentarioDTO> entitys = new List<SrmComentarioDTO>();
            String sql = String.Format(this.helper.ObtenerListado, srmreccodi, activo, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SrmComentarioDTO entity = new SrmComentarioDTO();

                    int iSrmcomcodi = dr.GetOrdinal(this.helper.Srmcomcodi);
                    if (!dr.IsDBNull(iSrmcomcodi)) entity.Srmcomcodi = Convert.ToInt32(dr.GetValue(iSrmcomcodi));

                    int iSrmcomfechacoment = dr.GetOrdinal(this.helper.Srmcomfechacoment);
                    if (!dr.IsDBNull(iSrmcomfechacoment)) entity.Srmcomfechacoment = dr.GetDateTime(iSrmcomfechacoment);

                    int iSrmcomgruporespons = dr.GetOrdinal(this.helper.Srmcomgruporespons);
                    if (!dr.IsDBNull(iSrmcomgruporespons)) entity.Srmcomgruporespons = dr.GetString(iSrmcomgruporespons);
                    
                    int iUsername = dr.GetOrdinal(this.helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    
                    int iSrmcomcomentario = dr.GetOrdinal(this.helper.Srmcomcomentario);
                    if (!dr.IsDBNull(iSrmcomcomentario)) entity.Srmcomcomentario = dr.GetString(iSrmcomcomentario);

                    int iSrmcomactivo = dr.GetOrdinal(this.helper.Srmcomactivo);
                    if (!dr.IsDBNull(iSrmcomactivo)) entity.Srmcomactivo = dr.GetString(iSrmcomactivo);

                    int iSrmcomusucreacion = dr.GetOrdinal(this.helper.Srmcomusucreacion);
                    if (!dr.IsDBNull(iSrmcomusucreacion)) entity.Srmcomusucreacion = dr.GetString(iSrmcomusucreacion);

                    int iSrmcomfeccreacion = dr.GetOrdinal(this.helper.Srmcomfeccreacion);
                    if (!dr.IsDBNull(iSrmcomfeccreacion)) entity.Srmcomfeccreacion = dr.GetDateTime(iSrmcomfeccreacion);

                    int iSrmcomusumodificacion = dr.GetOrdinal(this.helper.Srmcomusumodificacion);
                    if (!dr.IsDBNull(iSrmcomusumodificacion)) entity.Srmcomusumodificacion = dr.GetString(iSrmcomusumodificacion);

                    int iSrmcomfecmodificacion = dr.GetOrdinal(this.helper.Srmcomfecmodificacion);
                    if (!dr.IsDBNull(iSrmcomfecmodificacion)) entity.Srmcomfecmodificacion = dr.GetDateTime(iSrmcomfecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int srmreccodi, string activo)
        {
            String sql = String.Format(this.helper.TotalRegistros, srmreccodi, activo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
