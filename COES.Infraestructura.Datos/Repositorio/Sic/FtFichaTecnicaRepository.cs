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
    /// Clase de acceso a datos de la tabla FT_FICHATECNICA
    /// </summary>
    public class FtFichaTecnicaRepository : RepositoryBase, IFtFichaTecnicaRepository
    {
        public FtFichaTecnicaRepository(string strConn)
            : base(strConn)
        {
        }

        FtFichaTecnicaHelper helper = new FtFichaTecnicaHelper();

        public int Save(FtFichaTecnicaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, id);

            dbProvider.AddInParameter(command, helper.Ftecnombre, DbType.String, entity.Ftecnombre);
            dbProvider.AddInParameter(command, helper.Ftecprincipal, DbType.Int32, entity.Ftecprincipal);
            dbProvider.AddInParameter(command, helper.Ftecestado, DbType.String, entity.Ftecestado);
            dbProvider.AddInParameter(command, helper.Ftecusucreacion, DbType.String, entity.Ftecusucreacion);
            dbProvider.AddInParameter(command, helper.Ftecusumodificacion, DbType.String, entity.Ftecusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftecfecmodificacion, DbType.DateTime, entity.Ftecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftecfeccreacion, DbType.DateTime, entity.Ftecfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftecambiente, DbType.Int32, entity.Ftecambiente);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(FtFichaTecnicaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Ftecnombre, DbType.String, entity.Ftecnombre);
            dbProvider.AddInParameter(command, helper.Ftecprincipal, DbType.Int32, entity.Ftecprincipal);
            dbProvider.AddInParameter(command, helper.Ftecestado, DbType.String, entity.Ftecestado);
            dbProvider.AddInParameter(command, helper.Ftecusucreacion, DbType.String, entity.Ftecusucreacion);
            dbProvider.AddInParameter(command, helper.Ftecusumodificacion, DbType.String, entity.Ftecusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftecfecmodificacion, DbType.DateTime, entity.Ftecfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ftecfeccreacion, DbType.DateTime, entity.Ftecfeccreacion);
            dbProvider.AddInParameter(command, helper.Ftecambiente, DbType.Int32, entity.Ftecambiente);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, entity.Fteccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(FtFichaTecnicaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Ftecprincipal, DbType.Int32, entity.Ftecprincipal);
            dbProvider.AddInParameter(command, helper.Ftecusumodificacion, DbType.String, entity.Ftecusumodificacion);
            dbProvider.AddInParameter(command, helper.Ftecfecmodificacion, DbType.DateTime, entity.Ftecfecmodificacion);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, entity.Fteccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public FtFichaTecnicaDTO GetById(int fteccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fteccodi, DbType.Int32, fteccodi);
            FtFichaTecnicaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<FtFichaTecnicaDTO> List()
        {
            List<FtFichaTecnicaDTO> entitys = new List<FtFichaTecnicaDTO>();
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

        public List<FtFichaTecnicaDTO> GetByCriteria(string estado)
        {
            List<FtFichaTecnicaDTO> entitys = new List<FtFichaTecnicaDTO>();

            string query = string.Format(helper.SqlGetByCriteria, estado);

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
        /// Método que devuelve la Ficha Maestra Principal
        /// </summary>
        /// <returns></returns>
        public FtFichaTecnicaDTO GetFichaMaestraPrincipal(int ambiente)
        {

            string query = string.Format(helper.SqlGetFichaMaestraPrincipal, ambiente);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<FtFichaTecnicaDTO> lista = new List<FtFichaTecnicaDTO>();
            FtFichaTecnicaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    lista.Add(entity);
                }
            }

            if (lista.Count > 1)
            {
                throw new Exception("Existen más de una Ficha Maestra Principal");
            }

            return entity;
        }
    }
}
