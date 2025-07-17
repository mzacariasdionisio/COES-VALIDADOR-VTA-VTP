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
    /// Clase de acceso a datos de la tabla EVE_SUBCAUSA_FAMILIA
    /// </summary>
    public class EveSubcausaFamiliaRepository: RepositoryBase, IEveSubcausaFamiliaRepository
    {
        public EveSubcausaFamiliaRepository(string strConn): base(strConn)
        {
        }

        EveSubcausaFamiliaHelper helper = new EveSubcausaFamiliaHelper();

        public int Save(EveSubcausaFamiliaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Scaufacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Scaufaeliminado, DbType.String, entity.Scaufaeliminado);
            dbProvider.AddInParameter(command, helper.Scaufausucreacion, DbType.String, entity.Scaufausucreacion);
            dbProvider.AddInParameter(command, helper.Scaufafeccreacion, DbType.DateTime, entity.Scaufafeccreacion);
            dbProvider.AddInParameter(command, helper.Scaufausumodificacion, DbType.String, entity.Scaufausumodificacion);
            dbProvider.AddInParameter(command, helper.Scaufafecmodificacion, DbType.DateTime, entity.Scaufafecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveSubcausaFamiliaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Scaufaeliminado, DbType.String, entity.Scaufaeliminado);
            dbProvider.AddInParameter(command, helper.Scaufausucreacion, DbType.String, entity.Scaufausucreacion);
            dbProvider.AddInParameter(command, helper.Scaufafeccreacion, DbType.DateTime, entity.Scaufafeccreacion);
            dbProvider.AddInParameter(command, helper.Scaufausumodificacion, DbType.String, entity.Scaufausumodificacion);
            dbProvider.AddInParameter(command, helper.Scaufafecmodificacion, DbType.DateTime, entity.Scaufafecmodificacion);
            dbProvider.AddInParameter(command, helper.Scaufacodi, DbType.Int32, entity.Scaufacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int scaufacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Scaufacodi, DbType.Int32, scaufacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveSubcausaFamiliaDTO GetById(int scaufacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Scaufacodi, DbType.Int32, scaufacodi);
            EveSubcausaFamiliaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EveSubcausaFamiliaDTO> List()
        {
            List<EveSubcausaFamiliaDTO> entitys = new List<EveSubcausaFamiliaDTO>();
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


        public string ListFamilia(int subcausacodi)
        {
            string familia = "";

            EveSubcausaFamiliaDTO entity = new EveSubcausaFamiliaDTO();
            
            String sql = String.Format(this.helper.SqlListFamilia, subcausacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    familia += entity.Famcodi + ",";


                }
            }

            if (familia != "")
            {
                familia = familia.Substring(0, familia.Length - 1);
            }
            else
            {
                familia = "-1";
            }
                
            return familia;
        }

        public List<EveSubcausaFamiliaDTO> GetByCriteria()
        {
            List<EveSubcausaFamiliaDTO> entitys = new List<EveSubcausaFamiliaDTO>();
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
        /// Graba los datos de la tabla EVE_SUBCAUSA_FAMILIA
        /// </summary>
        public int SaveEveSubcausaFamiliaId(EveSubcausaFamiliaDTO entity)
        {
            try
            {
                int id = 0;

                if (entity.Scaufacodi==0)
                    id = Save(entity);
                else
                { 
                    Update(entity);
                    id = entity.Scaufacodi;
                }

                return id;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public List<EveSubcausaFamiliaDTO> BuscarOperaciones(string estado, int subcausaCodi,int famCodi, int nroPage, int pageSize)
        {
            List<EveSubcausaFamiliaDTO> entitys = new List<EveSubcausaFamiliaDTO>();
            String sql = String.Format(this.helper.ObtenerListado, estado, subcausaCodi, famCodi, nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveSubcausaFamiliaDTO entity = new EveSubcausaFamiliaDTO();

                    int iScaufacodi = dr.GetOrdinal(this.helper.Scaufacodi);
                    if (!dr.IsDBNull(iScaufacodi)) entity.Scaufacodi = Convert.ToInt32(dr.GetValue(iScaufacodi));

                    int iSubcausacodi = dr.GetOrdinal(this.helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iScaufaeliminado = dr.GetOrdinal(this.helper.Scaufaeliminado);
                    if (!dr.IsDBNull(iScaufaeliminado)) entity.Scaufaeliminado = dr.GetString(iScaufaeliminado);

                    int iScaufausucreacion = dr.GetOrdinal(this.helper.Scaufausucreacion);
                    if (!dr.IsDBNull(iScaufausucreacion)) entity.Scaufausucreacion = dr.GetString(iScaufausucreacion);

                    int iScaufafeccreacion = dr.GetOrdinal(this.helper.Scaufafeccreacion);
                    if (!dr.IsDBNull(iScaufafeccreacion)) entity.Scaufafeccreacion = dr.GetDateTime(iScaufafeccreacion);

                    int iScaufausumodificacion = dr.GetOrdinal(this.helper.Scaufausumodificacion);
                    if (!dr.IsDBNull(iScaufausumodificacion)) entity.Scaufausumodificacion = dr.GetString(iScaufausumodificacion);

                    int iScaufafecmodificacion = dr.GetOrdinal(this.helper.Scaufafecmodificacion);
                    if (!dr.IsDBNull(iScaufafecmodificacion)) entity.Scaufafecmodificacion = dr.GetDateTime(iScaufafecmodificacion);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string estado, int subcausaCodi,int famCodi)
        {
            String sql = String.Format(this.helper.TotalRegistros, estado, subcausaCodi, famCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }
    }
}
