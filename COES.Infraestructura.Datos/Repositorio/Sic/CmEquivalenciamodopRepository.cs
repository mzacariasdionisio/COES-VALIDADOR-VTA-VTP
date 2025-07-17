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
    /// Clase de acceso a datos de la tabla CM_EQUIVALENCIAMODOP
    /// </summary>
    public class CmEquivalenciamodopRepository: RepositoryBase, ICmEquivalenciamodopRepository
    {
        public CmEquivalenciamodopRepository(string strConn): base(strConn)
        {
        }

        CmEquivalenciamodopHelper helper = new CmEquivalenciamodopHelper();

        public int Save(CmEquivalenciamodopDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equimocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equimonombrencp, DbType.String, entity.Equimonombrencp);
            dbProvider.AddInParameter(command, helper.Equimousucreacion, DbType.String, entity.Equimousucreacion);
            dbProvider.AddInParameter(command, helper.Equimofeccreacion, DbType.DateTime, entity.Equimofeccreacion);
            dbProvider.AddInParameter(command, helper.Equimousumodificacion, DbType.String, entity.Equimousumodificacion);
            dbProvider.AddInParameter(command, helper.Equimofecmodificacion, DbType.DateTime, entity.Equimofecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmEquivalenciamodopDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equimonombrencp, DbType.String, entity.Equimonombrencp);
            dbProvider.AddInParameter(command, helper.Equimousucreacion, DbType.String, entity.Equimousucreacion);
            dbProvider.AddInParameter(command, helper.Equimofeccreacion, DbType.DateTime, entity.Equimofeccreacion);
            dbProvider.AddInParameter(command, helper.Equimousumodificacion, DbType.String, entity.Equimousumodificacion);
            dbProvider.AddInParameter(command, helper.Equimofecmodificacion, DbType.DateTime, entity.Equimofecmodificacion);
            dbProvider.AddInParameter(command, helper.Equimocodi, DbType.Int32, entity.Equimocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equimocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equimocodi, DbType.Int32, equimocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmEquivalenciamodopDTO GetById(int equimocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equimocodi, DbType.Int32, equimocodi);
            CmEquivalenciamodopDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CmEquivalenciamodopDTO> List()
        {
            List<CmEquivalenciamodopDTO> entitys = new List<CmEquivalenciamodopDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquivalenciamodopDTO entity = helper.Create(dr);
                    
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmEquivalenciamodopDTO> GetByCriteria()
        {
            List<CmEquivalenciamodopDTO> entitys = new List<CmEquivalenciamodopDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquivalenciamodopDTO entity = new CmEquivalenciamodopDTO();
                    //entitys.Add(helper.Create(dr));
                    int iEquimocodi = dr.GetOrdinal(helper.Equimocodi);
                    if (!dr.IsDBNull(iEquimocodi)) entity.Equimocodi = Convert.ToInt32(dr.GetValue(iEquimocodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb= dr.GetString(iGruponomb);


                    int iEquimonombrencp = dr.GetOrdinal(helper.Equimonombrencp);
                    if (!dr.IsDBNull(iEquimonombrencp)) entity.Equimonombrencp = dr.GetString(iEquimonombrencp);

                    int iEquimousucreacion = dr.GetOrdinal(helper.Equimousucreacion);
                    if (!dr.IsDBNull(iEquimousucreacion)) entity.Equimousucreacion = dr.GetString(iEquimousucreacion);

                    int iEquimofeccreacion = dr.GetOrdinal(helper.Equimofeccreacion);
                    if (!dr.IsDBNull(iEquimofeccreacion)) entity.Equimofeccreacion = dr.GetDateTime(iEquimofeccreacion);

                    int iEquimousumodificacion = dr.GetOrdinal(helper.Equimousumodificacion);
                    if (!dr.IsDBNull(iEquimousumodificacion)) entity.Equimousumodificacion = dr.GetString(iEquimousumodificacion);

                    int iEquimofecmodificacion = dr.GetOrdinal(helper.Equimofecmodificacion);
                    if (!dr.IsDBNull(iEquimofecmodificacion)) entity.Equimofecmodificacion = dr.GetDateTime(iEquimofecmodificacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        /// <summary>
        /// Graba los datos de la tabla CM_EQUIVALENCIAMODOP
        /// </summary>
        public int SaveCmEquivalenciamodopId(CmEquivalenciamodopDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Equimocodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Equimocodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<CmEquivalenciamodopDTO> BuscarOperaciones(int grupocodi, int nroPage, int pageSize)
        {
            List<CmEquivalenciamodopDTO> entitys = new List<CmEquivalenciamodopDTO>();
            String sql = String.Format(this.helper.ObtenerListado, grupocodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CmEquivalenciamodopDTO entity = new CmEquivalenciamodopDTO();

                    int iEquimocodi = dr.GetOrdinal(this.helper.Equimocodi);
                    if (!dr.IsDBNull(iEquimocodi)) entity.Equimocodi = Convert.ToInt32(dr.GetValue(iEquimocodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iEquimonombrencp = dr.GetOrdinal(this.helper.Equimonombrencp);
                    if (!dr.IsDBNull(iEquimonombrencp)) entity.Equimonombrencp = dr.GetString(iEquimonombrencp);

                    int iEquimousucreacion = dr.GetOrdinal(this.helper.Equimousucreacion);
                    if (!dr.IsDBNull(iEquimousucreacion)) entity.Equimousucreacion = dr.GetString(iEquimousucreacion);

                    int iEquimofeccreacion = dr.GetOrdinal(this.helper.Equimofeccreacion);
                    if (!dr.IsDBNull(iEquimofeccreacion)) entity.Equimofeccreacion = dr.GetDateTime(iEquimofeccreacion);

                    int iEquimousumodificacion = dr.GetOrdinal(this.helper.Equimousumodificacion);
                    if (!dr.IsDBNull(iEquimousumodificacion)) entity.Equimousumodificacion = dr.GetString(iEquimousumodificacion);

                    int iEquimofecmodificacion = dr.GetOrdinal(this.helper.Equimofecmodificacion);
                    if (!dr.IsDBNull(iEquimofecmodificacion)) entity.Equimofecmodificacion = dr.GetDateTime(iEquimofecmodificacion);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(int grupocodi)
        {
            String sql = String.Format(this.helper.TotalRegistros, grupocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
