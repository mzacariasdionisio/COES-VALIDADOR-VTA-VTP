using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_PROPRECURSO
    /// </summary>
    public class CpProprecursoRepository : RepositoryBase, ICpProprecursoRepository
    {
        public CpProprecursoRepository(string strConn) : base(strConn)
        {
        }

        CpProprecursoHelper helper = new CpProprecursoHelper();

        public void Save(CpProprecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Variaccodi, DbType.Int32, entity.Variaccodi);
            dbProvider.AddInParameter(command, helper.Fechaproprecur, DbType.DateTime, entity.Fechaproprecur);
            dbProvider.AddInParameter(command, helper.Valor, DbType.String, entity.Valor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpProprecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, entity.Propcodi);
            dbProvider.AddInParameter(command, helper.Variaccodi, DbType.Int32, entity.Variaccodi);
            dbProvider.AddInParameter(command, helper.Fechaproprecur, DbType.DateTime, entity.Fechaproprecur);
            dbProvider.AddInParameter(command, helper.Valor, DbType.String, entity.Valor);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int recurcodi, int topcodi, int propcodi, int variaccodi, DateTime fechaproprecur)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Propcodi, DbType.Int32, propcodi);
            dbProvider.AddInParameter(command, helper.Variaccodi, DbType.Int32, variaccodi);
            dbProvider.AddInParameter(command, helper.Fechaproprecur, DbType.DateTime, fechaproprecur);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpProprecursoDTO GetById(int recurcodi, int propcodi, DateTime fechaproprecur, int topcodi)
        {
            string sqlStr = string.Format(helper.SqlGetById, topcodi, recurcodi, propcodi, fechaproprecur.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sqlStr);
            CpProprecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpProprecursoDTO> List()
        {
            List<CpProprecursoDTO> entitys = new List<CpProprecursoDTO>();
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

        public List<CpProprecursoDTO> GetByCriteria()
        {
            List<CpProprecursoDTO> entitys = new List<CpProprecursoDTO>();
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

        #region Yupana Continuo
        public List<CpProprecursoDTO> ListarPropiedadxRecurso2(int pOrden, string scatecodi, int pTopologia, int toescenario)
        {
            List<CpProprecursoDTO> entitys = new List<CpProprecursoDTO>();
            string sqlQuery = string.Format(helper.GetSqlListarPropiedadxRecurso2, pTopologia, 1, pOrden, scatecodi, toescenario);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            CpProprecursoDTO entity = new CpProprecursoDTO();
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpProprecursoDTO();
                    entity = helper.Create(dr);
                    entity.CambiaValor = 0;
                    int iPropOrden = dr.GetOrdinal(helper.Proporden);
                    if (!dr.IsDBNull(iPropOrden)) entity.Proporden = Convert.ToInt16(dr.GetValue(iPropOrden));
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt16(dr.GetValue(iCatcodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpProprecursoDTO> ListarPropiedadxRecursoToGams(int pOrden, string scatecodi, int pTopologia, int consideragams)
        {
            List<CpProprecursoDTO> entitys = new List<CpProprecursoDTO>();
            string sqlQuery = string.Format(helper.SqlListarPropiedadxRecursoToGams, pTopologia, 1 , pOrden, scatecodi, consideragams);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            CpProprecursoDTO entity = new CpProprecursoDTO();
           
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpProprecursoDTO();
                    entity = helper.Create(dr);
                    entity.CambiaValor = 0;
                    int iPropOrden = dr.GetOrdinal(helper.Proporden);
                    if (!dr.IsDBNull(iPropOrden)) entity.Proporden = Convert.ToInt16(dr.GetValue(iPropOrden));
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt16(dr.GetValue(iCatcodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpProprecursoDTO> ListarPropiedadxRecurso(int pRecurso, int pTopologia, string sqlSoloManual)
        {
            List<CpProprecursoDTO> entitys = new List<CpProprecursoDTO>();
            string sqlQuery = string.Format(helper.GetSqlListarPropiedadxRecurso, pRecurso, pTopologia, 1 , sqlSoloManual);
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            CpProprecursoDTO entity = new CpProprecursoDTO();
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpProprecursoDTO();
                    entity = helper.Create(dr);
                    entity.CambiaValor = 0;
                    int iPropOrden = dr.GetOrdinal(helper.Proporden);
                    if (!dr.IsDBNull(iPropOrden)) entity.Proporden = Convert.ToInt16(dr.GetValue(iPropOrden));
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt16(dr.GetValue(iCatcodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        #endregion
    }
}
