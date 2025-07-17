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
    /// Clase de acceso a datos de la tabla SIO_TABLAPRIE
    /// </summary>
    public class SioTablaprieRepository : RepositoryBase, ISioTablaprieRepository
    {
        public SioTablaprieRepository(string strConn) : base(strConn)
        {
        }

        SioTablaprieHelper helper = new SioTablaprieHelper();

        public int Save(SioTablaprieDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Tpriedscripcion, DbType.String, entity.Tpriedscripcion);
            dbProvider.AddInParameter(command, helper.Tpriefechaplazo, DbType.DateTime, entity.Tpriefechaplazo);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Tprieabrev, DbType.String, entity.Tprieabrev);
            dbProvider.AddInParameter(command, helper.Tprieusumodificacion, DbType.String, entity.Tprieusumodificacion);
            dbProvider.AddInParameter(command, helper.Tpriefecmodificacion, DbType.DateTime, entity.Tpriefecmodificacion);
            dbProvider.AddInParameter(command, helper.Tprieusucreacion, DbType.String, entity.Tprieusucreacion);
            dbProvider.AddInParameter(command, helper.Tpriequery, DbType.String, entity.Tpriequery);
            dbProvider.AddInParameter(command, helper.Tprieffeccreacion, DbType.DateTime, entity.Tprieffeccreacion);
            dbProvider.AddInParameter(command, helper.Tprieresolucion, DbType.Int32, entity.Tprieresolucion);
            dbProvider.AddInParameter(command, helper.Tpriefechacierre, DbType.DateTime, entity.Tpriefechacierre);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SioTablaprieDTO entity)
        {
            string updateQuery = string.Format(helper.SqlUpdate, entity.Tpriecodi, entity.Tpriedscripcion,
                                         ((DateTime)entity.Tpriefechaplazo).ToString(ConstantesBase.FormatoFecha),
                                         entity.Areacodi, entity.Tprieabrev, entity.Tprieusumodificacion,
                                         ((DateTime)entity.Tpriefecmodificacion).ToString(ConstantesBase.FormatoFecha)
                                         );



            DbCommand command = dbProvider.GetSqlStringCommand(updateQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int tpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, tpriecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SioTablaprieDTO GetById(int tpriecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Tpriecodi, DbType.Int32, tpriecodi);
            SioTablaprieDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SioTablaprieDTO> List()
        {
            List<SioTablaprieDTO> entitys = new List<SioTablaprieDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);



            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SioTablaprieDTO entity = helper.Create(dr);
                    int iAreaabrev = dr.GetOrdinal("AREAABREV");
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SioTablaprieDTO> GetByCriteria()
        {
            List<SioTablaprieDTO> entitys = new List<SioTablaprieDTO>();
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

        public List<SioTablaprieDTO> GetByPeriodo(DateTime periodo)
        {
            List<SioTablaprieDTO> entitys = new List<SioTablaprieDTO>();
            var query = string.Format(helper.SqlGetByPeriodo, periodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SioTablaprieDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SioTablaprieDTO();

                    int iTpriecodi = dr.GetOrdinal(helper.Tpriecodi);
                    if (!dr.IsDBNull(iTpriecodi)) entity.Tpriecodi = dr.GetInt32(iTpriecodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iTpriedscripcion = dr.GetOrdinal(helper.Tpriedscripcion);
                    if (!dr.IsDBNull(iTpriedscripcion)) entity.Tpriedscripcion = dr.GetString(iTpriedscripcion);

                    int iTprieabrev = dr.GetOrdinal(helper.Tprieabrev);
                    if (!dr.IsDBNull(iTprieabrev)) entity.Tprieabrev = dr.GetString(iTprieabrev);

                    int iTpriequery = dr.GetOrdinal(helper.Tpriequery);
                    if (!dr.IsDBNull(iTpriequery)) entity.Tpriequery = dr.GetString(iTpriequery);

                    int iTprieresolucion = dr.GetOrdinal(helper.Tprieresolucion);
                    if (!dr.IsDBNull(iTprieresolucion)) entity.Tprieresolucion = dr.GetInt32(iTprieresolucion);

                    int iTpriefechaplazo = dr.GetOrdinal(helper.Tpriefechaplazo);
                    if (!dr.IsDBNull(iTpriefechaplazo)) entity.Tpriefechaplazo = dr.GetDateTime(iTpriefechaplazo);

                    int iTpriefechacierre = dr.GetOrdinal(helper.Tpriefechacierre);
                    if (!dr.IsDBNull(iTpriefechacierre)) entity.Tpriefechacierre = dr.GetDateTime(iTpriefechacierre);

                    //otros campos
                    int iCantidadVersion = dr.GetOrdinal(helper.CantidadVersion);
                    if (!dr.IsDBNull(iCantidadVersion)) entity.CantidadVersion = dr.GetInt32(iCantidadVersion);

                    int iCabpritieneregistros = dr.GetOrdinal(helper.Cabpritieneregistros);
                    if (!dr.IsDBNull(iCabpritieneregistros)) entity.Cabpritieneregistros = dr.GetInt32(iCabpritieneregistros);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
