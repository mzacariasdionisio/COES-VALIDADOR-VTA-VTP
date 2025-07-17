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
    /// Clase de acceso a datos de la tabla CM_OPERACIONREGISTRO
    /// </summary>
    public class CmOperacionregistroRepository: RepositoryBase, ICmOperacionregistroRepository
    {
        public CmOperacionregistroRepository(string strConn): base(strConn)
        {
        }

        CmOperacionregistroHelper helper = new CmOperacionregistroHelper();

        public int Save(CmOperacionregistroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Operegcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Operegfecinicio, DbType.DateTime, entity.Operegfecinicio);
            dbProvider.AddInParameter(command, helper.Operegfecfin, DbType.DateTime, entity.Operegfecfin);
            dbProvider.AddInParameter(command, helper.Operegusucreacion, DbType.String, entity.Operegusucreacion);
            dbProvider.AddInParameter(command, helper.Operegfeccreacion, DbType.DateTime, entity.Operegfeccreacion);
            dbProvider.AddInParameter(command, helper.Operegusumodificacion, DbType.String, entity.Operegusumodificacion);
            dbProvider.AddInParameter(command, helper.Operegfecmodificacion, DbType.DateTime, entity.Operegfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmOperacionregistroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Operegfecinicio, DbType.DateTime, entity.Operegfecinicio);
            dbProvider.AddInParameter(command, helper.Operegfecfin, DbType.DateTime, entity.Operegfecfin);
            dbProvider.AddInParameter(command, helper.Operegusucreacion, DbType.String, entity.Operegusucreacion);
            dbProvider.AddInParameter(command, helper.Operegfeccreacion, DbType.DateTime, entity.Operegfeccreacion);
            dbProvider.AddInParameter(command, helper.Operegusumodificacion, DbType.String, entity.Operegusumodificacion);
            dbProvider.AddInParameter(command, helper.Operegfecmodificacion, DbType.DateTime, entity.Operegfecmodificacion);
            dbProvider.AddInParameter(command, helper.Operegcodi, DbType.Int32, entity.Operegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int operegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Operegcodi, DbType.Int32, operegcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmOperacionregistroDTO GetById(int operegcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Operegcodi, DbType.Int32, operegcodi);
            CmOperacionregistroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmOperacionregistroDTO> List()
        {
            List<CmOperacionregistroDTO> entitys = new List<CmOperacionregistroDTO>();
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

        public List<CmOperacionregistroDTO> GetByCriteria()
        {
            List<CmOperacionregistroDTO> entitys = new List<CmOperacionregistroDTO>();
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
        /// Graba los datos de la tabla CM_OPERACIONREGISTRO
        /// </summary>
        public int SaveCmOperacionregistroId(CmOperacionregistroDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Operegcodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Operegcodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<CmOperacionregistroDTO> BuscarOperaciones(int grupocodi,int subcausaCodi,DateTime operegFecinicio,DateTime operegFecfin, int nroPage, int pageSize)
        {
            List<CmOperacionregistroDTO> entitys = new List<CmOperacionregistroDTO>();
            String sql = String.Format(this.helper.ObtenerListado, grupocodi,subcausaCodi,operegFecinicio.ToString(ConstantesBase.FormatoFecha),operegFecfin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmOperacionregistroDTO entity = new CmOperacionregistroDTO();

                    int iOperegcodi = dr.GetOrdinal(this.helper.Operegcodi);
                    if (!dr.IsDBNull(iOperegcodi)) entity.Operegcodi = Convert.ToInt32(dr.GetValue(iOperegcodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iOperegfecinicio = dr.GetOrdinal(this.helper.Operegfecinicio);
                    if (!dr.IsDBNull(iOperegfecinicio)) entity.Operegfecinicio = dr.GetDateTime(iOperegfecinicio);

                    int iOperegfecfin = dr.GetOrdinal(this.helper.Operegfecfin);
                    if (!dr.IsDBNull(iOperegfecfin)) entity.Operegfecfin = dr.GetDateTime(iOperegfecfin);

                    int iOperegusucreacion = dr.GetOrdinal(this.helper.Operegusucreacion);
                    if (!dr.IsDBNull(iOperegusucreacion)) entity.Operegusucreacion = dr.GetString(iOperegusucreacion);

                    int iOperegfeccreacion = dr.GetOrdinal(this.helper.Operegfeccreacion);
                    if (!dr.IsDBNull(iOperegfeccreacion)) entity.Operegfeccreacion = dr.GetDateTime(iOperegfeccreacion);

                    int iOperegusumodificacion = dr.GetOrdinal(this.helper.Operegusumodificacion);
                    if (!dr.IsDBNull(iOperegusumodificacion)) entity.Operegusumodificacion = dr.GetString(iOperegusumodificacion);

                    int iOperegfecmodificacion = dr.GetOrdinal(this.helper.Operegfecmodificacion);
                    if (!dr.IsDBNull(iOperegfecmodificacion)) entity.Operegfecmodificacion = dr.GetDateTime(iOperegfecmodificacion);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int grupocodi,int subcausaCodi,DateTime operegFecinicio,DateTime operegFecfin)
        {
            String sql = String.Format(this.helper.TotalRegistros, grupocodi,subcausaCodi,operegFecinicio.ToString(ConstantesBase.FormatoFecha),operegFecfin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
