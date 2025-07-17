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
    /// Clase de acceso a datos de la tabla PMPO_CONFIGURACION
    /// </summary>
    public class PmpoConfiguracionRepository : RepositoryBase, IPmpoConfiguracionRepository
    {
        public PmpoConfiguracionRepository(string strConn)
            : base(strConn)
        {
        }

        PmpoConfiguracionHelper helper = new PmpoConfiguracionHelper();

        public void Update(PmpoConfiguracionDTO entity)
        {
            string sqlQuery = string.Format(helper.SqlUpdate, entity.Confpmvalor, entity.Confpmusumodificacion, entity.Confpmcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstado(PmpoConfiguracionDTO entity)
        {

            string sqlQuery = string.Format(helper.SqlUpdateEstado, entity.Confpmestado, entity.Confpmcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            dbProvider.ExecuteNonQuery(command);
        }
        public int Save(PmpoConfiguracionDTO entity)
        {
            string sqlQuery;
            DbCommand command;

            //Consultar existente
            List<PmpoConfiguracionDTO> ListHist = this.List(entity.Confpmatributo, entity.Confpmparametro, "A");
            for (int i = 0; i < ListHist.Count; i++)
                if (ListHist[i].Confpmestado == "A")
                {
                    sqlQuery = string.Format(helper.SqlUpdateEstado, "H", ListHist[i].Confpmcodi);
                    command = dbProvider.GetSqlStringCommand(sqlQuery);
                    dbProvider.ExecuteNonQuery(command);
                }

            command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            sqlQuery = string.Format(helper.SqlSave, id, entity.Confpmatributo, entity.Confpmparametro, entity.Confpmvalor, entity.Confpmusucreacion);

            command = dbProvider.GetSqlStringCommand(sqlQuery);

            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public void Delete(int confpmcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Confpmcodi, DbType.DateTime, confpmcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PmpoConfiguracionDTO GetById(int idConfcodi)
        {
            string queryString = string.Format(helper.SqlGetById, idConfcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PmpoConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public PmpoConfiguracionDTO GetByParmanetroFech(string Fech, string Parametro)
        {
            string queryString = string.Format(helper.SqlGetByParmanetroFech, Parametro, Fech);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PmpoConfiguracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        /// <summary>
        /// Obtener listado completo de configuracion
        /// </summary>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public List<PmpoConfiguracionDTO> List()
        {
            List<PmpoConfiguracionDTO> entitys = new List<PmpoConfiguracionDTO>();
            string queryString = string.Format(helper.SqlList);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoConfiguracionDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de atributos de la configuracion
        /// </summary>
        /// <param name="atributo"></param>
        /// <returns></returns>
        public List<PmpoConfiguracionDTO> List(string atributo)
        {
            List<PmpoConfiguracionDTO> entitys = new List<PmpoConfiguracionDTO>();
            string queryString = string.Format(helper.SqlListAtributo, atributo);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoConfiguracionDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Obtener listado de valores de la configuracion en funcion al atributo y parametro
        /// </summary>
        /// <param name="atributo"></param>
        /// <param name="parametro"></param>
        /// /// <param name="estado"></param>
        /// <returns></returns>
        public List<PmpoConfiguracionDTO> List(string atributo, string parametro, string estado)
        {
            List<PmpoConfiguracionDTO> entitys = new List<PmpoConfiguracionDTO>();
            string queryString = string.Format(helper.SqlListValor, atributo, parametro, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmpoConfiguracionDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Trae solo el valor que esta activo de un atributo y parametro
        /// </summary>
        /// <param name="atributo"></param>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public PmpoConfiguracionDTO List(string atributo, string parametro)
        {
            PmpoConfiguracionDTO entity = new PmpoConfiguracionDTO();
            string queryString = string.Format(helper.SqlListValorActivo, atributo, parametro);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;

        }
        /// <summary>
        /// Eliminar parametro
        /// </summary>
        /// <param name="user"></param>
        /// <param name="confsmcorrelativo"></param>
        /// <returns></returns>
        public void Delete(string user, int confsmcorrelativo)
        {
            string queryString = string.Format(helper.SqlDelete, user, confsmcorrelativo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
