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
    /// Clase de acceso a datos de la tabla CP_RECURSO
    /// </summary>
    public class CpRecursoRepository : RepositoryBase, ICpRecursoRepository
    {
        public CpRecursoRepository(string strConn) : base(strConn)
        {
        }

        CpRecursoHelper helper = new CpRecursoHelper();

        public void Save(CpRecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Recurconsideragams, DbType.Int32, entity.Recurconsideragams);
            dbProvider.AddInParameter(command, helper.Recurfamsic, DbType.Int32, entity.Recurfamsic);
            dbProvider.AddInParameter(command, helper.Recurlogico, DbType.Int32, entity.Recurlogico);
            dbProvider.AddInParameter(command, helper.Recurformula, DbType.String, entity.Recurformula);
            dbProvider.AddInParameter(command, helper.Recurtoescenario, DbType.Int32, entity.Recurtoescenario);
            dbProvider.AddInParameter(command, helper.Recurorigen3, DbType.Int32, entity.Recurorigen3);
            dbProvider.AddInParameter(command, helper.Recurorigen2, DbType.Int32, entity.Recurorigen2);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Catcodi, DbType.Int32, entity.Catcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Recurestado, DbType.Int32, entity.Recurestado);
            dbProvider.AddInParameter(command, helper.Tablasicoes, DbType.String, entity.Tablasicoes);
            dbProvider.AddInParameter(command, helper.Recurcodisicoes, DbType.Int32, entity.Recurcodisicoes);
            dbProvider.AddInParameter(command, helper.Recurorigen, DbType.Int32, entity.Recurorigen);
            dbProvider.AddInParameter(command, helper.Recurpadre, DbType.Int32, entity.Recurpadre);
            dbProvider.AddInParameter(command, helper.Recurnombre, DbType.String, entity.Recurnombre);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(CpRecursoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Recurconsideragams, DbType.Int32, entity.Recurconsideragams);
            dbProvider.AddInParameter(command, helper.Recurfamsic, DbType.Int32, entity.Recurfamsic);
            dbProvider.AddInParameter(command, helper.Recurlogico, DbType.Int32, entity.Recurlogico);
            dbProvider.AddInParameter(command, helper.Recurformula, DbType.String, entity.Recurformula);
            dbProvider.AddInParameter(command, helper.Recurtoescenario, DbType.Int32, entity.Recurtoescenario);
            dbProvider.AddInParameter(command, helper.Recurorigen3, DbType.Int32, entity.Recurorigen3);
            dbProvider.AddInParameter(command, helper.Recurorigen2, DbType.Int32, entity.Recurorigen2);
            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, entity.Topcodi);
            dbProvider.AddInParameter(command, helper.Catcodi, DbType.Int32, entity.Catcodi);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Recurestado, DbType.Int32, entity.Recurestado);
            dbProvider.AddInParameter(command, helper.Tablasicoes, DbType.String, entity.Tablasicoes);
            dbProvider.AddInParameter(command, helper.Recurcodisicoes, DbType.Int32, entity.Recurcodisicoes);
            dbProvider.AddInParameter(command, helper.Recurorigen, DbType.Int32, entity.Recurorigen);
            dbProvider.AddInParameter(command, helper.Recurpadre, DbType.Int32, entity.Recurpadre);
            dbProvider.AddInParameter(command, helper.Recurnombre, DbType.String, entity.Recurnombre);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, entity.Recurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int topcodi, int recurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public CpRecursoDTO GetById(int topcodi, int recurcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Topcodi, DbType.Int32, topcodi);
            dbProvider.AddInParameter(command, helper.Recurcodi, DbType.Int32, recurcodi);
            CpRecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CpRecursoDTO> List()
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
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

        public List<CpRecursoDTO> GetByCriteria()
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
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

        public List<CpRecursoDTO> ObtenerPorTopologiaYCategoria(int topcodi, string propcodi)
        {
            string query = string.Format(helper.SqlObtenerPorTopologiaYCategoria, topcodi, propcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            CpRecursoDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iRecurcodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = dr.GetInt32(iRecurcodi);

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    int iRecurcodisicoes = dr.GetOrdinal(helper.Recurcodisicoes);
                    if (!dr.IsDBNull(iRecurcodisicoes)) entity.Recurcodisicoes = dr.GetInt32(iRecurcodisicoes);

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = dr.GetInt32(iPropcodi);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetString(iValor);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpRecursoDTO> ObtenerListaRelacionBarraCentral(int topcodi)
        {
            string query = string.Format(helper.SqlObtenerListaRelacionBarraCentral, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            CpRecursoDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();

                    int iRecurcodicentral = dr.GetOrdinal(helper.Recurcodicentral);
                    if (!dr.IsDBNull(iRecurcodicentral)) entity.Recurcodicentral = Convert.ToInt32(dr.GetValue(iRecurcodicentral));

                    int iRecurcodisicoescentral = dr.GetOrdinal(helper.Recurcodisicoescentral);
                    if (!dr.IsDBNull(iRecurcodisicoescentral)) entity.Recurcodisicoescentral = Convert.ToInt32(dr.GetValue(iRecurcodisicoescentral));

                    int iRecurcodibarra = dr.GetOrdinal(helper.Recurcodibarra);
                    if (!dr.IsDBNull(iRecurcodibarra)) entity.Recurcodibarra = Convert.ToInt32(dr.GetValue(iRecurcodibarra));

                    int iRecurcodisicoesbarra = dr.GetOrdinal(helper.Recurcodisicoesbarra);
                    if (!dr.IsDBNull(iRecurcodisicoesbarra)) entity.Recurcodisicoesbarra = Convert.ToInt32( dr.GetValue(iRecurcodisicoesbarra));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public CpRecursoDTO GetByCriteria(int topcodi, int catcodi, int recurcodisicoes)
        {
            string query = string.Format(helper.SqlGetByCriteria, topcodi, catcodi, recurcodisicoes);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpRecursoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        // Yupana
        public List<CpRecursoDTO> ListaUrsEmpresaAnexo5(int catcodi, int catecodiGrupo, int topcodi)
        {
            string query = string.Format(helper.SqlListaUrsEmpresaAnexo5, topcodi, catecodiGrupo, catcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            CpRecursoDTO entity = null;
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iGequicodi = dr.GetOrdinal(helper.Gequicodi);
                    if (!dr.IsDBNull(iGequicodi)) entity.Gequicodi = Convert.ToInt32(dr.GetValue(iGequicodi));
                    int iGequinomb = dr.GetOrdinal(helper.Gequinomb);
                    if (!dr.IsDBNull(iGequinomb)) entity.Gequinomb = dr.GetString(iGequinomb);
                    int iUrsmax = dr.GetOrdinal(helper.Ursmax);
                    if (!dr.IsDBNull(iUrsmax)) entity.Ursmax = dr.GetDecimal(iUrsmax);
                    int iUrsmin = dr.GetOrdinal(helper.Ursmin);
                    if (!dr.IsDBNull(iUrsmin)) entity.Ursmin = dr.GetDecimal(iUrsmin);
                    int iCentralnomb = dr.GetOrdinal(helper.Centralnomb);
                    if (!dr.IsDBNull(iCentralnomb)) entity.Centralnomb = dr.GetString(iCentralnomb);
                    int iEmpresanomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmpresanomb)) entity.Emprnomb = dr.GetString(iEmpresanomb);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Yupana Continuo
        public List<CpRecursoDTO> ListarRecursoPorTopologia(int topcodi)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string QueryString = string.Format(helper.GetSqlRecursoxTopologia, topcodi);
            CpRecursoDTO entity = new CpRecursoDTO();

            DbCommand command = dbProvider.GetSqlStringCommand(QueryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();
                    entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpCategoriaDTO> ListaCategoria()
        {

            List<CpCategoriaDTO> entitys = new List<CpCategoriaDTO>();
            string strQuery = strQuery = string.Format(helper.SqlListaCategoria);
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            CpCategoriaDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpCategoriaDTO();
                    int iCatnombre = dr.GetOrdinal("Catnombre");
                    if (!dr.IsDBNull(iCatnombre)) entity.Catnombre = dr.GetString(iCatnombre);
                    int iCatmatrizgams = dr.GetOrdinal("Catmatrizgams");
                    if (!dr.IsDBNull(iCatmatrizgams)) entity.Catmatrizgams = dr.GetString(iCatmatrizgams);
                    int iCatdescripcion = dr.GetOrdinal("Catdescripcion");
                    if (!dr.IsDBNull(iCatdescripcion)) entity.Catdescripcion = dr.GetString(iCatdescripcion);
                    int iCatprefijo = dr.GetOrdinal("Catprefijo");
                    if (!dr.IsDBNull(iCatprefijo)) entity.Catprefijo = dr.GetString(iCatprefijo);
                    int iCatcodi = dr.GetOrdinal("Catcodi");
                    if (!dr.IsDBNull(iCatcodi)) entity.Catcodi = Convert.ToInt32(dr.GetValue(iCatcodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpRecursoDTO> ListarLinea(int tipoRecurso, int pTopologiaID)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string queryString = string.Empty;
            if (pTopologiaID != 0)
            {
                queryString = string.Format(helper.GetSqlListarLinea01, tipoRecurso, pTopologiaID);

            }
            else
            {
                queryString = string.Format(helper.GetSqlListarLinea02, tipoRecurso, pTopologiaID);
            }

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            // string Recurcodigams = "RecursoIDGAMS";
            string Recnodotoporigen = "RecNodoTopOrigen";
            string Recnodotopdestino = "RecNodoTopDestino";
            string Recnodotoporigenid = "RecNodoTopOrigenID";
            string Recnodotopdestinoid = "RecNodoTopDestinoID";
            CpRecursoDTO entity = new CpRecursoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();
                    entity = helper.Create(dr);
                    int iRecNodoTopOrigen = dr.GetOrdinal(Recnodotoporigen);
                    if (!dr.IsDBNull(iRecNodoTopOrigen)) entity.RecNodoTopOrigen = dr.GetString(iRecNodoTopOrigen);
                    int iRecNodoTopDestino = dr.GetOrdinal(Recnodotopdestino);
                    if (!dr.IsDBNull(iRecNodoTopDestino)) entity.RecNodoTopDestino = dr.GetString(iRecNodoTopDestino);
                    int iRecNodoTopOrigenID = dr.GetOrdinal(Recnodotoporigenid);
                    if (!dr.IsDBNull(iRecNodoTopOrigenID)) entity.RecNodoTopOrigenID = Convert.ToInt32(dr.GetValue(iRecNodoTopOrigenID));
                    int iRecNodoTopDestinoID = dr.GetOrdinal(Recnodotopdestinoid);
                    if (!dr.IsDBNull(iRecNodoTopDestinoID)) entity.RecNodoTopDestinoID = Convert.ToInt32(dr.GetValue(iRecNodoTopDestinoID));
                    entity.ConsideraEquipo = entity.Recurtoescenario.ToString();
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpRecursoDTO> ListaRecursoXCategoria(int tipoRecurso, int pTopologiaID)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string QueryString = string.Empty;
            switch (tipoRecurso)
            {
                case ConstantesBase.PlantaNoConvenO:
                    QueryString = string.Format(helper.SqlRecursoxCategoriaGrupo, tipoRecurso, pTopologiaID, 0, 1);
                    break;
                default:
                    QueryString = string.Format(helper.GetSqlRecursoxCategoria2, tipoRecurso, pTopologiaID, 0, 1);
                    break;
            }


            DbCommand command = dbProvider.GetSqlStringCommand(QueryString);
            //string Recurcodigams = "RecursoIDGAMS";
            CpRecursoDTO entity = new CpRecursoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();
                    entity = helper.Create(dr);
                    int iRecurnombSicoes = dr.GetOrdinal(helper.Recurnombsicoes);
                    if (!dr.IsDBNull(iRecurnombSicoes)) entity.Recurnombsicoes = dr.GetString(iRecurnombSicoes);
                    if (entity.Recurcodisicoes == 0)
                        entity.Recurnombsicoes = "";
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CpRecursoDTO> List(int topcodi, string codigos)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string strQuery = string.Format(helper.SqlList, topcodi, codigos);
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CpRecursoDTO> ListaRecursoXCategoriaInNodoT(int tipoRecurso, int pTopologiaID, int ttermcodi)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string QueryString = string.Format(helper.GetSqlRecursoxCategoria4, tipoRecurso, pTopologiaID, ttermcodi, ConstantesBase.TerNodoT);
            DbCommand command = dbProvider.GetSqlStringCommand(QueryString);
            string RecIdNodo = "Recidnodo";
            CpRecursoDTO entity = new CpRecursoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();
                    entity = helper.Create(dr);
                    int iRecIdNodo = dr.GetOrdinal(RecIdNodo);
                    if (!dr.IsDBNull(iRecIdNodo)) entity.RecNodoID = Convert.ToInt32(dr.GetValue(iRecIdNodo));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpRecursoDTO> ListaModoInNodoT(int tipoRecurso, int pTopologiaID, int ttermcodi)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string QueryString = string.Format(helper.SqlListaModosXNodoT, tipoRecurso, pTopologiaID, 0, 1, 1, 2, 0, ttermcodi, ConstantesBase.TerNodoT);
            DbCommand command = dbProvider.GetSqlStringCommand(QueryString);
            string RecIdNodo = "Recidnodo";
            CpRecursoDTO entity = new CpRecursoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new CpRecursoDTO();
                    entity = helper.Create(dr);
                    int iRecIdNodo = dr.GetOrdinal(RecIdNodo);
                    if (!dr.IsDBNull(iRecIdNodo)) entity.RecNodoID = Convert.ToInt32(dr.GetValue(iRecIdNodo));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<CpRecursoDTO> ListarEquiposConecANodoTop(int topcodi)
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();
            string strQuery = string.Format(helper.SqlEquiposConecANodoTop, topcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(strQuery);
            CpRecursoDTO entity = new CpRecursoDTO();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iRecurcodiConec = dr.GetOrdinal("recurcodiconec");
                    if (!dr.IsDBNull(iRecurcodiConec)) entity.RecurcodiConec = Convert.ToInt32(dr.GetValue(iRecurcodiConec));
                    int iCatcodiConec = dr.GetOrdinal("catcodiconec");
                    if (!dr.IsDBNull(iCatcodiConec)) entity.CatcodiConec = Convert.ToInt32(dr.GetValue(iCatcodiConec));
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

        #region CMgCP_PR07

        public List<CpRecursoDTO> ObtenerEmbalsesYupana()
        {
            List<CpRecursoDTO> entitys = new List<CpRecursoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmbalsesYUPANA);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CpRecursoDTO entity = new CpRecursoDTO();

                    int iRecurcodi = dr.GetOrdinal(helper.Recurcodi);
                    if (!dr.IsDBNull(iRecurcodi)) entity.Recurcodi = Convert.ToInt32(dr.GetValue(iRecurcodi));

                    int iRecurnombre = dr.GetOrdinal(helper.Recurnombre);
                    if (!dr.IsDBNull(iRecurnombre)) entity.Recurnombre = dr.GetString(iRecurnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}
