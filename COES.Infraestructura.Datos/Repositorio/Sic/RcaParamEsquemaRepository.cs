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
    /// Clase de acceso a datos de la tabla RCA_PARAM_ESQUEMA
    /// </summary>
    public class RcaParamEsquemaRepository : RepositoryBase, IRcaParamEsquemaRepository
    {
        public RcaParamEsquemaRepository(string strConn)
            : base(strConn)
        {
        }

        RcaParamEsquemaHelper helper = new RcaParamEsquemaHelper();

        public int Save(RcaParamEsquemaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rcparecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            if (entity.Rcparehperacmf.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmf, DbType.Decimal, entity.Rcparehperacmf);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmf, DbType.Decimal, System.DBNull.Value);
            }

            if (entity.Rcparehperacmt.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmt, DbType.Decimal, entity.Rcparehperacmt);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmt, DbType.Decimal, DBNull.Value);
            }

            if (entity.Rcparehfperacmf.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmf, DbType.Decimal, entity.Rcparehfperacmf);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmf, DbType.Decimal, DBNull.Value);
            }

            if (entity.Rcparehfperacmt.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmt, DbType.Decimal, entity.Rcparehfperacmt);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmt, DbType.Decimal, DBNull.Value);
            }
            
            dbProvider.AddInParameter(command, helper.Rcpareestregistro, DbType.String, entity.Rcpareestregistro);
            dbProvider.AddInParameter(command, helper.Rcpareanio, DbType.Int32, entity.Rcpareanio);


            if (entity.Rcparehperacmf2.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmf2, DbType.Decimal, entity.Rcparehperacmf2);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehperacmf2, DbType.Decimal, DBNull.Value);
            }

            if (entity.Rcparehfperacmf2.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmf2, DbType.Decimal, entity.Rcparehfperacmf2);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehfperacmf2, DbType.Decimal, DBNull.Value);
            }            
            
            dbProvider.AddInParameter(command, helper.Rcparenroesquema, DbType.Int32, entity.Rcparenroesquema);

            if (entity.Rcparehpdemandaref.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehpdemandaref, DbType.Decimal, entity.Rcparehpdemandaref);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehpdemandaref, DbType.Decimal, DBNull.Value);
            }

            if (entity.Rcparehfpdemandaref.HasValue)
            {
                dbProvider.AddInParameter(command, helper.Rcparehfpdemandaref, DbType.Decimal, entity.Rcparehfpdemandaref);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcparehfpdemandaref, DbType.Decimal, DBNull.Value);
            }


            dbProvider.AddInParameter(command, helper.Rcpareusucreacion, DbType.String, entity.Rcpareusucreacion);
            dbProvider.AddInParameter(command, helper.Rcparefeccreacion, DbType.DateTime, entity.Rcparefeccreacion);
            //dbProvider.AddInParameter(command, helper.Rcpareusumodificacion, DbType.String, entity.Rcpareusumodificacion);
            //dbProvider.AddInParameter(command, helper.Rcparefecmodificacion, DbType.DateTime, entity.Rcparefecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaParamEsquemaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcparehperacmf, DbType.Decimal, entity.Rcparehperacmf);
            dbProvider.AddInParameter(command, helper.Rcparehperacmt, DbType.Decimal, entity.Rcparehperacmt);
            dbProvider.AddInParameter(command, helper.Rcparehfperacmf, DbType.Decimal, entity.Rcparehfperacmf);
            dbProvider.AddInParameter(command, helper.Rcparehfperacmt, DbType.Decimal, entity.Rcparehfperacmt);
            dbProvider.AddInParameter(command, helper.Rcpareestregistro, DbType.String, entity.Rcpareestregistro);
            //dbProvider.AddInParameter(command, helper.Rcpareusucreacion, DbType.String, entity.Rcpareusucreacion);
            //dbProvider.AddInParameter(command, helper.Rcparefeccreacion, DbType.DateTime, entity.Rcparefeccreacion);
            dbProvider.AddInParameter(command, helper.Rcpareusumodificacion, DbType.String, entity.Rcpareusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcparefecmodificacion, DbType.DateTime, entity.Rcparefecmodificacion);

            dbProvider.AddInParameter(command, helper.Rcparehperacmf2, DbType.Decimal, entity.Rcparehperacmf2);
            dbProvider.AddInParameter(command, helper.Rcparehfperacmf2, DbType.Decimal, entity.Rcparehfperacmf2);
            dbProvider.AddInParameter(command, helper.Rcparenroesquema, DbType.Int32, entity.Rcparenroesquema);

            dbProvider.AddInParameter(command, helper.Rcparehpdemandaref, DbType.Decimal, entity.Rcparehpdemandaref);
            dbProvider.AddInParameter(command, helper.Rcparehfpdemandaref, DbType.Decimal, entity.Rcparehfpdemandaref);

            dbProvider.AddInParameter(command, helper.Rcparecodi, DbType.Int32, entity.Rcparecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcparecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcparecodi, DbType.Int32, rcparecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaParamEsquemaDTO GetById(int rcparecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcparecodi, DbType.Int32, rcparecodi);
            RcaParamEsquemaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaParamEsquemaDTO> List()
        {
            List<RcaParamEsquemaDTO> entitys = new List<RcaParamEsquemaDTO>();
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

        public List<RcaParamEsquemaDTO> GetByCriteria()
        {
            List<RcaParamEsquemaDTO> entitys = new List<RcaParamEsquemaDTO>();
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

        public List<RcaParamEsquemaDTO> ListarPorFiltros(string anio, string tipoEmpresa)
        {
            List<RcaParamEsquemaDTO> entities = new List<RcaParamEsquemaDTO>();

            var condicion = String.Empty;
            if (!string.IsNullOrEmpty(tipoEmpresa)) 
            {
                condicion = string.Format(" AND CONVERT(UPPER(EMPR.EMPRRAZSOCIAL), 'US7ASCII') like '%{0}%'", tipoEmpresa.ToUpper());
            }
           
            string queryString = string.Format(helper.SqlListaPorFiltros, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Rcpareanio, DbType.Int32, anio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaParamEsquemaDTO entity = new RcaParamEsquemaDTO();

                    int iRcparecodi = dr.GetOrdinal(helper.Rcparecodi);
                    if (!dr.IsDBNull(iRcparecodi)) entity.Rcparecodi = Convert.ToInt32(dr.GetValue(iRcparecodi));

                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iRcparehfperacmf = dr.GetOrdinal(helper.Rcparehfperacmf);
                    if (!dr.IsDBNull(iRcparehfperacmf)) entity.Rcparehfperacmf = dr.GetDecimal(iRcparehfperacmf);

                    int iRcparehfperacmt = dr.GetOrdinal(helper.Rcparehfperacmt);
                    if (!dr.IsDBNull(iRcparehfperacmt)) entity.Rcparehfperacmt = dr.GetDecimal(iRcparehfperacmt);

                    int iRcparehperacmf = dr.GetOrdinal(helper.Rcparehperacmf);
                    if (!dr.IsDBNull(iRcparehperacmf)) entity.Rcparehperacmf = dr.GetDecimal(iRcparehperacmf);

                    int iRcparehperacmt = dr.GetOrdinal(helper.Rcparehperacmt);
                    if (!dr.IsDBNull(iRcparehperacmt)) entity.Rcparehperacmt = dr.GetDecimal(iRcparehperacmt);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //Nuevos campos
                    int iRcparehfperacmf2 = dr.GetOrdinal(helper.Rcparehfperacmf2);
                    if (!dr.IsDBNull(iRcparehfperacmf2)) entity.Rcparehfperacmf2 = dr.GetDecimal(iRcparehfperacmf2);

                    int iRcparehperacmf2 = dr.GetOrdinal(helper.Rcparehperacmf2);
                    if (!dr.IsDBNull(iRcparehperacmf2)) entity.Rcparehperacmf2 = dr.GetDecimal(iRcparehperacmf2);

                    int iRcparenroesquema = dr.GetOrdinal(helper.Rcparenroesquema);
                    if (!dr.IsDBNull(iRcparenroesquema)) entity.Rcparenroesquema = dr.GetInt16(iRcparenroesquema);

                    int iRcparehpdemandaref = dr.GetOrdinal(helper.Rcparehpdemandaref);
                    if (!dr.IsDBNull(iRcparehpdemandaref)) entity.Rcparehpdemandaref = dr.GetDecimal(iRcparehpdemandaref);

                    int iRcparehfpdemandaref = dr.GetOrdinal(helper.Rcparehfpdemandaref);
                    if (!dr.IsDBNull(iRcparehfpdemandaref)) entity.Rcparehfpdemandaref = dr.GetDecimal(iRcparehfpdemandaref);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<int> ListarAniosParametroEsquema()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarAnios);
            var anios = new List<int>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iRcpareanio = dr.GetOrdinal(helper.Rcpareanio);
                    if (!dr.IsDBNull(iRcpareanio)) anios.Add(Convert.ToInt32(dr.GetValue(iRcpareanio)));
                }
            }
            return anios;
        }

        public List<RcaParamEsquemaDTO> ListarPorPuntoMedicion(string listaPuntoMedicion)
        {
            List<RcaParamEsquemaDTO> entities = new List<RcaParamEsquemaDTO>();

            var condicion = String.Empty;
            if (string.IsNullOrEmpty(listaPuntoMedicion))
            {
                condicion = string.Format(" AND RCPE.EQUICODI IN ({0})", listaPuntoMedicion);
            }

            string queryString = string.Format(helper.SqlListarPorPuntoMedicion, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaParamEsquemaDTO entity = new RcaParamEsquemaDTO();

                    int iRcparecodi = dr.GetOrdinal(helper.Rcparecodi);
                    if (!dr.IsDBNull(iRcparecodi)) entity.Rcparecodi = Convert.ToInt32(dr.GetValue(iRcparecodi));                    

                    int iRcparehfperacmf = dr.GetOrdinal(helper.Rcparehfperacmf);
                    if (!dr.IsDBNull(iRcparehfperacmf)) entity.Rcparehfperacmf = dr.GetDecimal(iRcparehfperacmf);

                    int iRcparehfperacmt = dr.GetOrdinal(helper.Rcparehfperacmt);
                    if (!dr.IsDBNull(iRcparehfperacmt)) entity.Rcparehfperacmt = dr.GetDecimal(iRcparehfperacmt);

                    int iRcparehperacmf = dr.GetOrdinal(helper.Rcparehperacmf);
                    if (!dr.IsDBNull(iRcparehperacmf)) entity.Rcparehperacmf = dr.GetDecimal(iRcparehperacmf);

                    int iRcparehperacmt = dr.GetOrdinal(helper.Rcparehperacmt);
                    if (!dr.IsDBNull(iRcparehperacmt)) entity.Rcparehperacmt = dr.GetDecimal(iRcparehperacmt);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    //Nuevos campos
                    int iRcparehfperacmf2 = dr.GetOrdinal(helper.Rcparehfperacmf2);
                    if (!dr.IsDBNull(iRcparehfperacmf2)) entity.Rcparehfperacmf2 = dr.GetDecimal(iRcparehfperacmf2);

                    int iRcparehperacmf2 = dr.GetOrdinal(helper.Rcparehperacmf2);
                    if (!dr.IsDBNull(iRcparehperacmf2)) entity.Rcparehperacmf2 = dr.GetDecimal(iRcparehperacmf2);

                    int iRcparenroesquema = dr.GetOrdinal(helper.Rcparenroesquema);
                    if (!dr.IsDBNull(iRcparenroesquema)) entity.Rcparenroesquema = dr.GetInt16(iRcparenroesquema);

                    int iRcparehpdemandaref = dr.GetOrdinal(helper.Rcparehpdemandaref);
                    if (!dr.IsDBNull(iRcparehpdemandaref)) entity.Rcparehpdemandaref = dr.GetDecimal(iRcparehpdemandaref);

                    int iRcparehfpdemandaref = dr.GetOrdinal(helper.Rcparehfpdemandaref);
                    if (!dr.IsDBNull(iRcparehfpdemandaref)) entity.Rcparehfpdemandaref = dr.GetDecimal(iRcparehfpdemandaref);


                    entities.Add(entity);
                }
            }

            return entities;
        }
    }
}
