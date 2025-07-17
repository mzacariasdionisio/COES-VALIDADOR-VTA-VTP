using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Repositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_OBSERVACION_ITEM
    /// </summary>
    public class TrObservacionItemRepository : RepositoryBase, ITrObservacionItemRepository
    {
        public TrObservacionItemRepository(string strConn)
            : base(strConn)
        {
        }

        TrObservacionItemHelper helper = new TrObservacionItemHelper();

        public int Save(TrObservacionItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Obsiteestado, DbType.String, entity.Obsiteestado);
            dbProvider.AddInParameter(command, helper.Obsitecomentario, DbType.String, entity.Obsitecomentario);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, entity.Obscancodi);
            dbProvider.AddInParameter(command, helper.Obsiteusuario, DbType.String, entity.Obsiteusuario);
            dbProvider.AddInParameter(command, helper.Obsitefecha, DbType.DateTime, entity.Obsitefecha);
            dbProvider.AddInParameter(command, helper.Obsitecomentarioagente, DbType.String, entity.Obsitecomentarioagente);

            #region "FIT - Señales no Disponibles Save"

            dbProvider.AddInParameter(command, helper.Obsiteproceso, DbType.String, entity.Obsiteproceso);

            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(TrObservacionItemDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Obsiteestado, DbType.String, entity.Obsiteestado);
            dbProvider.AddInParameter(command, helper.Obsitecomentario, DbType.String, entity.Obsitecomentario);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, entity.Obscancodi);
            dbProvider.AddInParameter(command, helper.Obsiteusuario, DbType.String, entity.Obsiteusuario);
            dbProvider.AddInParameter(command, helper.Obsitefecha, DbType.DateTime, entity.Obsitefecha);
            dbProvider.AddInParameter(command, helper.Obsitecomentarioagente, DbType.String, entity.Obsitecomentarioagente);
            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, entity.Obsitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int obsitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, obsitecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public TrObservacionItemDTO GetById(int obsitecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Obsitecodi, DbType.Int32, obsitecodi);
            TrObservacionItemDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TrObservacionItemDTO> List()
        {
            List<TrObservacionItemDTO> entitys = new List<TrObservacionItemDTO>();
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

        public List<TrObservacionItemDTO> GetByCriteria(int obscodi)
        {
            List<TrObservacionItemDTO> entitys = new List<TrObservacionItemDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Obscancodi, DbType.Int32, obscodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrObservacionItemDTO entity = helper.Create(dr);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canalpointtype = dr.GetString(iCanalpointtype);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<TrObservacionItemDTO> ObtenerReporteSeniales(int idEmpresa)
        {
            List<TrObservacionItemDTO> entitys = new List<TrObservacionItemDTO>();

            string sql = string.Format(helper.SqlObtenerReporteSeniales, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrObservacionItemDTO entity = helper.Create(dr);

                    int iCanalnomb = dr.GetOrdinal(helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetString(iCanalnomb);

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iCanalunidad = dr.GetOrdinal(helper.Canalunidad);
                    if (!dr.IsDBNull(iCanalunidad)) entity.Canalunidad = dr.GetString(iCanalunidad);

                    int iCanalabrev = dr.GetOrdinal(helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iCanalpointtype = dr.GetOrdinal(helper.Canalpointtype);
                    if (!dr.IsDBNull(iCanalpointtype)) entity.Canalpointtype = dr.GetString(iCanalpointtype);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iZonanomb = dr.GetOrdinal(helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    int iObsitefecha = dr.GetOrdinal(helper.Obsitefecha);
                    if (!dr.IsDBNull(iObsitefecha)) entity.Obsitefecha = dr.GetDateTime(iObsitefecha);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
