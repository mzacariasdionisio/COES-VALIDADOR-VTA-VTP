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
    /// Clase de acceso a datos de la tabla AUD_PROGAUDI_ELEMENTO
    /// </summary>
    public class AudProgaudiElementoRepository : RepositoryBase, IAudProgaudiElementoRepository
    {
        public AudProgaudiElementoRepository(string strConn) : base(strConn)
        {
        }

        AudProgaudiElementoHelper helper = new AudProgaudiElementoHelper();

        public int Save(AudProgaudiElementoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, entity.Elemcodi);
            dbProvider.AddInParameter(command, helper.Progaeiniciorevision, DbType.DateTime, entity.Progaeiniciorevision);
            dbProvider.AddInParameter(command, helper.Progaefinrevision, DbType.DateTime, entity.Progaefinrevision);
            dbProvider.AddInParameter(command, helper.Progaetamanomuestra, DbType.Int32, entity.Progaetamanomuestra);
            dbProvider.AddInParameter(command, helper.Progaemuestraseleccionada, DbType.String, entity.Progaemuestraseleccionada);
            dbProvider.AddInParameter(command, helper.Progaeprocedimientoprueba, DbType.String, entity.Progaeprocedimientoprueba);
            dbProvider.AddInParameter(command, helper.Progaeactivo, DbType.String, entity.Progaeactivo);
            dbProvider.AddInParameter(command, helper.Progaehistorico, DbType.String, entity.Progaehistorico);
            dbProvider.AddInParameter(command, helper.Progaeusucreacion, DbType.String, entity.Progaeusucreacion);
            dbProvider.AddInParameter(command, helper.Progaefechacreacion, DbType.DateTime, entity.Progaefechacreacion);
            dbProvider.AddInParameter(command, helper.Progaeusumodificacion, DbType.String, entity.Progaeusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaefechamodificacion, DbType.DateTime, entity.Progaefechamodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudProgaudiElementoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);
            dbProvider.AddInParameter(command, helper.Elemcodi, DbType.Int32, entity.Elemcodi);
            dbProvider.AddInParameter(command, helper.Progaeiniciorevision, DbType.DateTime, entity.Progaeiniciorevision);
            dbProvider.AddInParameter(command, helper.Progaefinrevision, DbType.DateTime, entity.Progaefinrevision);
            dbProvider.AddInParameter(command, helper.Progaetamanomuestra, DbType.Int32, entity.Progaetamanomuestra);
            dbProvider.AddInParameter(command, helper.Progaemuestraseleccionada, DbType.String, entity.Progaemuestraseleccionada);
            dbProvider.AddInParameter(command, helper.Progaeprocedimientoprueba, DbType.String, entity.Progaeprocedimientoprueba);
            dbProvider.AddInParameter(command, helper.Progaeusumodificacion, DbType.String, entity.Progaeusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaefechamodificacion, DbType.DateTime, entity.Progaefechamodificacion);
            dbProvider.AddInParameter(command, helper.Progaeactivo, DbType.String, entity.Progaeactivo);
            dbProvider.AddInParameter(command, helper.Progaehistorico, DbType.String, entity.Progaehistorico);
            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, entity.Progaecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudProgaudiElementoDTO entity)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Progaeusumodificacion, DbType.String, entity.Progaeusumodificacion);
            dbProvider.AddInParameter(command, helper.Progaefechamodificacion, DbType.DateTime, entity.Progaefechamodificacion);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudProgaudiElementoDTO GetById(int progaecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Progaecodi, DbType.Int32, progaecodi);
            AudProgaudiElementoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);
                }
            }

            return entity;
        }

        public AudProgaudiElementoDTO GetByElemcodi(int progracodi, int elemcodi)
        {
            string sql = string.Format(helper.SqlGetByElemcodi, progracodi, elemcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            AudProgaudiElementoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);
                }
            }

            return entity;
        }

        public List<AudProgaudiElementoDTO> List(int audicodi)
        {
            List<AudProgaudiElementoDTO> entitys = new List<AudProgaudiElementoDTO>();

            string sql = string.Format(helper.SqlList, audicodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiElementoDTO entity = helper.Create(dr);

                    int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudProgaudiElementoDTO> GetByCriteria(int progacodi)
        {
            List<AudProgaudiElementoDTO> entitys = new List<AudProgaudiElementoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, progacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiElementoDTO entity = helper.Create(dr);

                    int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudProgaudiElementoDTO> GetByCriteriaPorAuditoria(int audicodi)
        {
            List<AudProgaudiElementoDTO> entitys = new List<AudProgaudiElementoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaPorAuditoria);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, audicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgaudiElementoDTO entity = helper.Create(dr);

                    int iProccodi = dr.GetOrdinal(helper.Proccodi);
                    if (!dr.IsDBNull(iProccodi)) entity.Proccodi = Convert.ToInt32(dr.GetValue(iProccodi));

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
