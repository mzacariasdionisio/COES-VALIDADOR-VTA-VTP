using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla CP_GRUPORECURSO
    /// </summary>
    public class CpGruporecursoRepository : RepositoryBase, ICpGruporecursoRepository
    {
        public CpGruporecursoRepository(string strConn) : base(strConn)
        {
        }

        CpGruporecursoHelper helper = new CpGruporecursoHelper();

        public void Save(CpGruporecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grurecorden, DbType.Int32, entity.Grurecorden);
            dbProvider.AddInParameter(command, helper.Grurecval4, DbType.Decimal, entity.Grurecval4);
            dbProvider.AddInParameter(command, helper.Grurecval3, DbType.Decimal, entity.Grurecval3);
            dbProvider.AddInParameter(command, helper.Grurecval2, DbType.Decimal, entity.Grurecval2);
            dbProvider.AddInParameter(command, helper.Grurecval1, DbType.Decimal, entity.Grurecval1);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Recurcodidet, DbType.Int32, entity.Recurcodidet);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpGruporecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grurecorden, DbType.Int32, entity.Grurecorden);
            dbProvider.AddInParameter(command, helper.Grurecval4, DbType.Decimal, entity.Grurecval4);
            dbProvider.AddInParameter(command, helper.Grurecval3, DbType.Decimal, entity.Grurecval3);
            dbProvider.AddInParameter(command, helper.Grurecval2, DbType.Decimal, entity.Grurecval2);
            dbProvider.AddInParameter(command, helper.Grurecval1, DbType.Decimal, entity.Grurecval1);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);
            dbProvider.AddInParameter(command, helper.Recurcodidet, DbType.Int32, entity.Recurcodidet);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi, int recurcodi, int recurcodidet)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Recurcodidet, DbType.Int32, recurcodidet);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpGruporecursoDTO GetById(int topcodi, int recurcodi, int recurcodidet)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            dbProvider.AddInParameter(command, helper.Recurcodidet, DbType.Int32, recurcodidet);
            CpGruporecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpGruporecursoDTO> List(int topcodi)
        {
            List<CpGruporecursoDTO> entitys = new List<CpGruporecursoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            CpGruporecursoDTO entity = new CpGruporecursoDTO();
            string CatcodiMain = "catcodimain";
            string Catcodisec = "catcodisec";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iCatcodiMain = dr.GetOrdinal(CatcodiMain);
                    if (!dr.IsDBNull(iCatcodiMain)) entity.Catcodimain = dr.GetInt32(iCatcodiMain);
                    int iCatcodisec = dr.GetOrdinal(Catcodisec);
                    if (!dr.IsDBNull(iCatcodisec)) entity.Catcodisec = dr.GetInt32(iCatcodisec);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpGruporecursoDTO> GetByCriteria(int pRecurso, int pTopologia)
        {
            List<CpGruporecursoDTO> entitys = new List<CpGruporecursoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, pRecurso);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, pTopologia);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public CpGruporecursoDTO GetRelacionURSSICOES(int topcodi, int recurcodiURS)
        {
            var query = string.Format(helper.SqlObtenerRelacionURSSICOES, topcodi, recurcodiURS);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpGruporecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iCatcodi = dr.GetOrdinal(helper.Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));
                }
            }

            return entity;
        }

        //Yupana Continuo
        public List<CpGruporecursoDTO> ListaGrupoPorCategoria(int categoria, int topologia)
        {
            List<CpGruporecursoDTO> entitys = new List<CpGruporecursoDTO>();
            string query = string.Format(helper.SqlListaGrupoPorCategoria, categoria, topologia);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpGruporecursoDTO entity = new CpGruporecursoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iCatcodiSec = dr.GetOrdinal(helper.Catcodisec);
                    if (!dr.IsDBNull(iCatcodiSec)) entity.Catcodisec = dr.GetInt32(iCatcodiSec);
                    int iRecurtoescenario = dr.GetOrdinal(helper.Recurtoescenario);
                    if (!dr.IsDBNull(iRecurtoescenario)) entity.Recurtoescenario = dr.GetInt32(iRecurtoescenario);
                    int iRecurconsideragams = dr.GetOrdinal(helper.Recurconsideragams);
                    if (!dr.IsDBNull(iRecurconsideragams)) entity.Recurconsideragams = dr.GetInt32(iRecurconsideragams);
                    entity.Recurcodisicoes = entity.Recurcodidet;
                    entitys.Add(entity);
                }
            }

            return entitys.OrderBy(x => x.Recurcodi).ThenBy(x => x.Recurcodidet).ToList();
        }

        public void CrearCopia(int topcodi1, int topcodi2)
        {
            string query = string.Format(helper.SqlCrearCopia, topcodi1, topcodi2);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(command);
        }

        #region Yupana Iteracion 3 
        public List<CpGruporecursoDTO> GetByCriteriaFamilia(int pCategoria, int pTopologia)
        {
            string sqlQuery = string.Format(helper.SqlGetByCriteriaFamilia, pCategoria, pTopologia);
            List<CpGruporecursoDTO> entitys = new List<CpGruporecursoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            CpGruporecursoDTO entity = new CpGruporecursoDTO();
            string Recurnombre = "recurnombre";
            string Catcodi = "Catcodi";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpGruporecursoDTO();
                    int iRecurnombre = dr.GetOrdinal(Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);
                    int iCatcodi = dr.GetOrdinal(Catcodi);
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodisec = Convert.ToInt32(dr.GetValue(iCatcodi));
                    int iRecurcodisicoes = dr.GetOrdinal(helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = Convert.ToInt32(dr.GetValue(iRecurcodisicoes));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion
    }
}
