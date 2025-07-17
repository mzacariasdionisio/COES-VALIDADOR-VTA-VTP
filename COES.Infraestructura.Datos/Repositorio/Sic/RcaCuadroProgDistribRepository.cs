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
    /// Clase de acceso a datos de la tabla RCA_CUADRO_PROG_DISTRIB
    /// </summary>
    public class RcaCuadroProgDistribRepository: RepositoryBase, IRcaCuadroProgDistribRepository
    {
        public RcaCuadroProgDistribRepository(string strConn): base(strConn)
        {
        }

        RcaCuadroProgDistribHelper helper = new RcaCuadroProgDistribHelper();

        public int Save(RcaCuadroProgDistribDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            string queryString = helper.SqlSave;
            command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Rcprodcodi, DbType.Int32, id);            
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rcprodsubestacion, DbType.String, entity.Rcprodsubestacion);           
            dbProvider.AddInParameter(command, helper.Rcproddemanda, DbType.Decimal, entity.Rcproddemanda);
            dbProvider.AddInParameter(command, helper.Rcprodcargarechazar, DbType.Decimal, entity.Rcprodcargarechazar);                  
            dbProvider.AddInParameter(command, helper.Rcprodestregistro, DbType.String, entity.Rcprodestregistro);            
            dbProvider.AddInParameter(command, helper.Rcprodusucreacion, DbType.String, entity.Rcprodusucreacion);
            dbProvider.AddInParameter(command, helper.Rcprodfeccreacion, DbType.DateTime, entity.Rcprodfeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCuadroProgDistribDTO entity)
        {
            string queryString = helper.SqlUpdate;

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Rcprodusumodificacion, DbType.String, entity.Rcprodusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcprodfecmodificacion, DbType.DateTime, entity.Rcprodfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Rcprodsubestacion, DbType.String, entity.Rcprodsubestacion);
            dbProvider.AddInParameter(command, helper.Rcproddemanda, DbType.Decimal, entity.Rcproddemanda);
            dbProvider.AddInParameter(command, helper.Rcprodcargarechazar, DbType.Decimal, entity.Rcprodcargarechazar);
            dbProvider.AddInParameter(command, helper.Rcprodestregistro, DbType.String, entity.Rcprodestregistro);
            dbProvider.AddInParameter(command, helper.Rcprodcodi, DbType.Int32, entity.Rcprodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcprodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcprodcodi, DbType.Int32, rcprodcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCuadroProgDistribDTO GetById(int rcprodcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcprodcodi, DbType.Int32, rcprodcodi);
            RcaCuadroProgDistribDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCuadroProgDistribDTO> List()
        {
            List<RcaCuadroProgDistribDTO> entitys = new List<RcaCuadroProgDistribDTO>();
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

        public List<RcaCuadroProgDistribDTO> GetByCriteria()
        {
            List<RcaCuadroProgDistribDTO> entitys = new List<RcaCuadroProgDistribDTO>();
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

        public List<RcaCuadroProgDistribDTO> ListCuadroProgDistrib(int codigoCuadroPrograma)
        {
            List<RcaCuadroProgDistribDTO> entities = new List<RcaCuadroProgDistribDTO>();

            //var condicion = String.Empty;            

            string queryString = helper.SqlListCuadroProgDistrib;
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, codigoCuadroPrograma);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgDistribDTO entity = new RcaCuadroProgDistribDTO();

                    entity = helper.Create(dr);

                    entities.Add(entity);
                }
            }

            return entities;
        }
       
    }
}
