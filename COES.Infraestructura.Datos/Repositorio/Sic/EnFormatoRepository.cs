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
    /// Clase de acceso a datos de la tabla EN_FORMATO
    /// </summary>
    public class EnFormatoRepository : RepositoryBase, IEnFormatoRepository
    {
        public EnFormatoRepository(string strConn)
            : base(strConn)
        {
        }

        EnFormatoHelper helper = new EnFormatoHelper();


        public void Update(EnFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, entity.Formatocodi);
            dbProvider.AddInParameter(command, helper.Formatodesc, DbType.String, entity.Formatodesc);
            dbProvider.AddInParameter(command, helper.Formatotipoarchivo, DbType.Int32, entity.Formatotipoarchivo);
            dbProvider.AddInParameter(command, helper.Formatopadre, DbType.Int32, entity.Formatopadre);
            dbProvider.AddInParameter(command, helper.Formatoprefijo, DbType.String, entity.Formatoprefijo);
            dbProvider.AddInParameter(command, helper.Formatonumero, DbType.Decimal, entity.Formatonumero);
            dbProvider.AddInParameter(command, helper.Formatoestado, DbType.Int32, entity.Formatoestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(EnFormatoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Formatodesc, DbType.String, entity.Formatodesc);
            dbProvider.AddInParameter(command, helper.Formatotipoarchivo, DbType.Int32, entity.Formatotipoarchivo);
            dbProvider.AddInParameter(command, helper.Formatopadre, DbType.Int32, entity.Formatopadre);
            dbProvider.AddInParameter(command, helper.Formatoprefijo, DbType.String, entity.Formatoprefijo);
            dbProvider.AddInParameter(command, helper.Formatonumero, DbType.Decimal, entity.Formatonumero);
            dbProvider.AddInParameter(command, helper.Formatoestado, DbType.Int32, entity.Formatoestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Delete(int formatocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, formatocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EnFormatoDTO GetById(int formatocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Formatocodi, DbType.Int32, formatocodi);
            EnFormatoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<EnFormatoDTO> List()
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
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

        public List<EnFormatoDTO> GetByCriteria()
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
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


        public List<EnFormatoDTO> ListarFormatosActuales()
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
            string query = string.Format(helper.SqlListarFormatosActuales);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EnFormatoDTO> ListarFormatosActualesTodos()
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
            string query = string.Format(helper.SqlListarFormatosActualesTodos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EnFormatoDTO> ListarFormatosPorPadre(int idPadre)
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
            string query = string.Format(helper.SqlListarFormatosPorPadre, idPadre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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
        /// Formatos activos     
        /// </summary>
        /// <returns></returns>
        public List<EnFormatoDTO> ListarFormatosActivos()
        {
            List<EnFormatoDTO> entitys = new List<EnFormatoDTO>();
            string query = string.Format(helper.SqlListarFormatosActivos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EnFormatoDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}
