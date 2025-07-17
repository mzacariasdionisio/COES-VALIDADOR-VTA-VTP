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
    /// Clase de acceso a datos de la tabla PR_GENFORZADA_MAESTRO
    /// </summary>
    public class PrGenforzadaMaestroRepository: RepositoryBase, IPrGenforzadaMaestroRepository
    {
        public PrGenforzadaMaestroRepository(string strConn): base(strConn)
        {
        }

        PrGenforzadaMaestroHelper helper = new PrGenforzadaMaestroHelper();

        public int Save(PrGenforzadaMaestroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Genformaecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);            
            dbProvider.AddInParameter(command, helper.Genformaesimbolo, DbType.String, entity.Genformaesimbolo);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);            
            dbProvider.AddInParameter(command, helper.Genfortipo, DbType.String, entity.Genfortipo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrGenforzadaMaestroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
                        
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);
            dbProvider.AddInParameter(command, helper.Genformaesimbolo, DbType.String, entity.Genformaesimbolo);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);
            dbProvider.AddInParameter(command, helper.Genfortipo, DbType.String, entity.Genfortipo);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);            
            dbProvider.AddInParameter(command, helper.Genformaecodi, DbType.Int32, entity.Genformaecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int genformaecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Genformaecodi, DbType.Int32, genformaecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGenforzadaMaestroDTO GetById(int genformaecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Genformaecodi, DbType.Int32, genformaecodi);
            PrGenforzadaMaestroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGenforzadaMaestroDTO> List()
        {
            List<PrGenforzadaMaestroDTO> entitys = new List<PrGenforzadaMaestroDTO>();
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

        public List<PrGenforzadaMaestroDTO> GetByCriteria()
        {
            List<PrGenforzadaMaestroDTO> entitys = new List<PrGenforzadaMaestroDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGenforzadaMaestroDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iNombarra = dr.GetOrdinal(helper.Nombarra);
                    if (!dr.IsDBNull(iNombarra)) entity.Nombarra = dr.GetString(iNombarra);

                    int iIdgener = dr.GetOrdinal(helper.Idgener);
                    if (!dr.IsDBNull(iIdgener)) entity.Idgener = dr.GetString(iIdgener);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ValidarExistenciaPorRelacion(int relacioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistenciaPorRelacion);
            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, relacioncodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null) 
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }
    }
}
