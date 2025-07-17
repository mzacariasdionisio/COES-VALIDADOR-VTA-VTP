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
    /// Clase de acceso a datos de la tabla CM_MODELO_EMBALSE
    /// </summary>
    public class CmModeloEmbalseRepository : RepositoryBase, ICmModeloEmbalseRepository
    {
        public CmModeloEmbalseRepository(string strConn) : base(strConn)
        {
        }

        CmModeloEmbalseHelper helper = new CmModeloEmbalseHelper();

        public int Save(CmModeloEmbalseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Modembindyupana, DbType.String, entity.Modembindyupana);
            dbProvider.AddInParameter(command, helper.Modembfecvigencia, DbType.DateTime, entity.Modembfecvigencia);
            dbProvider.AddInParameter(command, helper.Modembestado, DbType.String, entity.Modembestado);
            dbProvider.AddInParameter(command, helper.Modembusucreacion, DbType.String, entity.Modembusucreacion);
            dbProvider.AddInParameter(command, helper.Modembfeccreacion, DbType.DateTime, entity.Modembfeccreacion);
            dbProvider.AddInParameter(command, helper.Modembusumodificacion, DbType.String, entity.Modembusumodificacion);
            dbProvider.AddInParameter(command, helper.Modembfecmodificacion, DbType.DateTime, entity.Modembfecmodificacion);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(CmModeloEmbalseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, entity.Modembcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Modembindyupana, DbType.String, entity.Modembindyupana);
            dbProvider.AddInParameter(command, helper.Modembfecvigencia, DbType.DateTime, entity.Modembfecvigencia);
            dbProvider.AddInParameter(command, helper.Modembestado, DbType.String, entity.Modembestado);
            dbProvider.AddInParameter(command, helper.Modembusucreacion, DbType.String, entity.Modembusucreacion);
            dbProvider.AddInParameter(command, helper.Modembfeccreacion, DbType.DateTime, entity.Modembfeccreacion);
            dbProvider.AddInParameter(command, helper.Modembusumodificacion, DbType.String, entity.Modembusumodificacion);
            dbProvider.AddInParameter(command, helper.Modembfecmodificacion, DbType.DateTime, entity.Modembfecmodificacion);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(CmModeloEmbalseDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Modembusumodificacion, DbType.String, entity.Modembusumodificacion);
            dbProvider.AddInParameter(command, helper.Modembfecmodificacion, DbType.DateTime, entity.Modembfecmodificacion);
            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, entity.Modembcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CmModeloEmbalseDTO GetById(int modembcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Modembcodi, DbType.Int32, modembcodi);
            CmModeloEmbalseDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                }
            }

            return entity;
        }

        public List<CmModeloEmbalseDTO> List()
        {
            List<CmModeloEmbalseDTO> entitys = new List<CmModeloEmbalseDTO>();
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

        public List<CmModeloEmbalseDTO> GetByCriteria(string estado, string recurcodis)
        {
            List<CmModeloEmbalseDTO> entitys = new List<CmModeloEmbalseDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, estado, recurcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CmModeloEmbalseDTO> ListHistorialXRecurso(int recurcodi)
        {
            List<CmModeloEmbalseDTO> entitys = new List<CmModeloEmbalseDTO>();

            string sql = string.Format(helper.SqlListHistorialXRecurso, recurcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iPtomedielenomb = dr.GetOrdinal(helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    int iPtomedibarranomb = dr.GetOrdinal(helper.Ptomedibarranomb);
                    if (!dr.IsDBNull(iPtomedibarranomb)) entity.Ptomedibarranomb = dr.GetString(iPtomedibarranomb);

                    int iPtomedidesc = dr.GetOrdinal(helper.Ptomedidesc);
                    if (!dr.IsDBNull(iPtomedidesc)) entity.Ptomedidesc = dr.GetString(iPtomedidesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
