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
    /// Clase de acceso a datos de la tabla WB_ARCHIVO_MODPLAN
    /// </summary>
    public class WbArchivoModplanRepository: RepositoryBase, IWbArchivoModplanRepository
    {
        public WbArchivoModplanRepository(string strConn): base(strConn)
        {
        }

        WbArchivoModplanHelper helper = new WbArchivoModplanHelper();

        public int Save(WbArchivoModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Arcmplcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, entity.Vermplcodi);
            dbProvider.AddInParameter(command, helper.Arcmplnombre, DbType.String, entity.Arcmplnombre);
            dbProvider.AddInParameter(command, helper.Arcmplindtc, DbType.String, entity.Arcmplindtc);
            dbProvider.AddInParameter(command, helper.Arcmplestado, DbType.String, entity.Arcmplestado);
            dbProvider.AddInParameter(command, helper.Arcmplext, DbType.String, entity.Arcmplext);
            dbProvider.AddInParameter(command, helper.Arcmpltipo, DbType.Int32, entity.Arcmpltipo);
            dbProvider.AddInParameter(command, helper.Arcmpldesc, DbType.String, entity.Arcmpldesc);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbArchivoModplanDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, entity.Vermplcodi);
            dbProvider.AddInParameter(command, helper.Arcmplnombre, DbType.String, entity.Arcmplnombre);
            dbProvider.AddInParameter(command, helper.Arcmplindtc, DbType.String, entity.Arcmplindtc);
            dbProvider.AddInParameter(command, helper.Arcmplestado, DbType.String, entity.Arcmplestado);
            dbProvider.AddInParameter(command, helper.Arcmplext, DbType.String, entity.Arcmplext);
            dbProvider.AddInParameter(command, helper.Arcmpltipo, DbType.Int32, entity.Arcmpltipo);
            dbProvider.AddInParameter(command, helper.Arcmpldesc, DbType.String, entity.Arcmpldesc);
            dbProvider.AddInParameter(command, helper.Arcmplcodi, DbType.Int32, entity.Arcmplcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int arcmplcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Arcmplcodi, DbType.Int32, arcmplcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbArchivoModplanDTO GetById(int arcmplcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Arcmplcodi, DbType.Int32, arcmplcodi);
            WbArchivoModplanDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbArchivoModplanDTO> List()
        {
            List<WbArchivoModplanDTO> entitys = new List<WbArchivoModplanDTO>();
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

        public List<WbArchivoModplanDTO> GetByCriteria(int vercodi)
        {
            List<WbArchivoModplanDTO> entitys = new List<WbArchivoModplanDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, vercodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public WbArchivoModplanDTO ObtenerDocumento(int idVersion, string tipo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerDocumento);

            dbProvider.AddInParameter(command, helper.Vermplcodi, DbType.Int32, idVersion);
            dbProvider.AddInParameter(command, helper.Arcmplindtc, DbType.String, tipo);

            WbArchivoModplanDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}
