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
    /// Clase de acceso a datos de la tabla PR_GRUPODAT
    /// </summary>
    public class PrGrupodatRepository : RepositoryBase, IPrGrupodatRepository
    {
        public PrGrupodatRepository(string strConn) : base(strConn)
        {
        }

        PrGrupodatHelper helper = new PrGrupodatHelper();

        public void Save(PrGrupodatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.Fechadat);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Formuladat, DbType.String, entity.Formuladat);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);
            dbProvider.AddInParameter(command, helper.Fechaact, DbType.DateTime, entity.Fechaact ?? DateTime.Now);
            dbProvider.AddInParameter(command, helper.Gdatcomentario, DbType.String, entity.Gdatcomentario);
            dbProvider.AddInParameter(command, helper.Gdatsustento, DbType.String, entity.Gdatsustento);
            dbProvider.AddInParameter(command, helper.Gdatcheckcero, DbType.Int32, entity.Gdatcheckcero);

            dbProvider.ExecuteNonQuery(command);
        }

        public void SaveTransaccional(PrGrupodatDTO entity, IDbConnection conn, DbTransaction tran)
        {
            DbCommand command = (DbCommand)conn.CreateCommand();
            command.CommandText = helper.SqlSave;
            command.Transaction = tran;
            command.Connection = (DbConnection)conn;

            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fechadat, DbType.DateTime, entity.Fechadat));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Lastuser, DbType.String, entity.Lastuser));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Formuladat, DbType.String, entity.Formuladat));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Deleted, DbType.Int32, entity.Deleted));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Fechaact, DbType.DateTime, entity.Fechaact ?? DateTime.Now));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Gdatcomentario, DbType.String, entity.Gdatcomentario));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Gdatsustento, DbType.String, entity.Gdatsustento));
            command.Parameters.Add(dbProvider.CreateParameter(command, helper.Gdatcheckcero, DbType.Int32, entity.Gdatcheckcero));

            command.ExecuteNonQuery();
        }

        public void Update(PrGrupodatDTO entity)
        {
            entity.Deleted2 = entity.Deleted2 == null ? entity.Deleted : entity.Deleted2;
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Formuladat, DbType.String, entity.Formuladat);
            dbProvider.AddInParameter(command, helper.Fechaact, DbType.DateTime, entity.Fechaact ?? DateTime.Now);
            dbProvider.AddInParameter(command, helper.Deleted2, DbType.Int32, entity.Deleted2);
            dbProvider.AddInParameter(command, helper.Gdatcomentario, DbType.String, entity.Gdatcomentario);
            dbProvider.AddInParameter(command, helper.Gdatsustento, DbType.String, entity.Gdatsustento);
            dbProvider.AddInParameter(command, helper.Gdatcheckcero, DbType.Int32, entity.Gdatcheckcero);

            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.Fechadat);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarParametro(int idGrupo, int concepcodi, string formula, string lastUser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarParametro);

            dbProvider.AddInParameter(command, helper.Formuladat, DbType.String, formula);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, lastUser);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, idGrupo);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupodatDTO GetById(DateTime fechadat, int concepcodi, int grupocodi, int deleted)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, fechadat);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, deleted);
            PrGrupodatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrGrupodatDTO> List()
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
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

        public List<PrGrupodatDTO> GetByCriteria(int concepcodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetByCriteria, concepcodi));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }


        public List<ConceptoDatoDTO> ListarDatosConceptoGrupoDat(int iGrupoCodi)
        {
            var resultado = new List<ConceptoDatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValoresModoOperacionGrupoDat);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, iGrupoCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new ConceptoDatoDTO();
                    oDatoConc.CONCEPCODI = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.CONCEPUNID = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.VALOR = dr.IsDBNull(dr.GetOrdinal("VALOR")) ? "" : dr.GetString(dr.GetOrdinal("VALOR"));
                    resultado.Add(oDatoConc);
                }
            }

            return resultado;
        }


        public List<PrGrupodatDTO> ListarHistoricoValores(string concepcodi, int grupocodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();

            string query = string.Format(helper.SqlHistoricoValores, concepcodi, grupocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConcepDesc = dr.GetOrdinal(helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepDesc)) entity.ConcepDesc = dr.GetString(iConcepDesc);

                    int iConceppadre = dr.GetOrdinal(helper.Conceppadre);
                    if (!dr.IsDBNull(iConceppadre)) entity.Conceppadre = Convert.ToInt32(dr.GetValue(iConceppadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region "COSTO OPORTUNIDAD"
        public List<PrGrupoConceptoDato> ObtenerDatosMO_URS(int iGrupoCodi, DateTime fechaRegistro)
        {
            var resultado = new List<PrGrupoConceptoDato>();
            var sComando = string.Format(helper.SqlParametrosURSporGrupo, fechaRegistro.ToString("yyyy-MM-dd"), iGrupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrGrupoConceptoDato();
                    oDatoConc.Concepcodi = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.Formuladat = dr.IsDBNull(dr.GetOrdinal("VALOR")) ? "" : dr.GetString(dr.GetOrdinal("VALOR"));
                    oDatoConc.Concepunid = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.Concepdesc = dr.IsDBNull(dr.GetOrdinal("CONCEPDESC")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPDESC"));
                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }

        public List<PrGrupoConceptoDato> ObtenerDatosMO_URS_Hidro(int iGrupoCodi, DateTime fechaRegistro)
        {
            var resultado = new List<PrGrupoConceptoDato>();
            var sComando = string.Format(helper.SqlParametrosURSporGrupoHidro, fechaRegistro.ToString("yyyy-MM-dd"), iGrupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrGrupoConceptoDato();
                    oDatoConc.Concepcodi = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.Formuladat = dr.IsDBNull(dr.GetOrdinal("VALOR")) ? "" : dr.GetString(dr.GetOrdinal("VALOR"));
                    oDatoConc.Concepunid = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.Concepdesc = dr.IsDBNull(dr.GetOrdinal("CONCEPDESC")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPDESC"));
                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }
        #endregion

        public List<PrGrupoConceptoDato> ObtenerParametrosGeneralesUrs()
        {
            var resultado = new List<PrGrupoConceptoDato>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlParametrosGeneralesURS);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrGrupoConceptoDato();
                    oDatoConc.Concepcodi = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.Formuladat = dr.IsDBNull(dr.GetOrdinal("VALOR")) ? "" : dr.GetString(dr.GetOrdinal("VALOR"));
                    oDatoConc.Concepunid = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.Concepdesc = dr.IsDBNull(dr.GetOrdinal("CONCEPDESC")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPDESC"));
                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }

        public List<PrGrupodatDTO> ParametrosPorFecha(DateTime fecha)
        {
            return ParametrosConfiguracionPorFecha(fecha, "-1", "-1").Where(x => x.Grupocodi > 0).ToList();
        }

        public List<PrGrupodatDTO> ParametrosGeneralesPorFecha(DateTime fecha)
        {
            return ParametrosConfiguracionPorFecha(fecha, "0", "-1");
        }

        public List<PrGrupodatDTO> ObtenerParametroPorCentral(string centrales)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroPorCentral, centrales);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametroPorConcepto(string concepCodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroPorConcepto, concepCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustible(string empresas, DateTime fecha)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroCurvaConsumo, empresas,
                fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    #region Ajustar Curva Consumo

                    int iCurvcodi = dr.GetOrdinal(helper.Curvcodi);
                    if (!dr.IsDBNull(iCurvcodi)) entity.Curvcodi = Convert.ToInt32(dr.GetValue(iCurvcodi));

                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametroGeneral(DateTime fechaProceso)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerParametroGeneral, fechaProceso.ToString(ConstantesBase.FormatoFechaHora)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepunid = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.ConcepUni = dr.GetString(iConcepunid);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametroModoOperacion(string idsGrupos, DateTime fechaProceso)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            var query = string.Format(helper.SqlObtenerParametroModoOperacion, idsGrupos, fechaProceso.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepunid = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.ConcepUni = dr.GetString(iConcepunid);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        // Inicio de Agregado - Sistema de Compensaciones
        public List<PrGrupodatDTO> ListaModosOperacion(int ptoMediCodi, int pecacodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaModosOperacion, ptoMediCodi, pecacodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrGrupodatDTO> ListaCentral(int emprcodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaCentral, emprcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        // DSH 19-06-2017 Actualizacion
        public List<PrGrupodatDTO> ListaGrupo(int emprcodi, int grupopadre)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaGrupo, emprcodi, grupopadre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        // DSH 19-06-2017 Actualizacion
        public List<PrGrupodatDTO> ListaModo(int emprcodi, int grupopadre)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaModo, emprcodi, grupopadre);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        // DSH 31/05/2017 - Cambio por Requerimiento - Sistema de Compensanciones
        public List<PrGrupodatDTO> ListaCabecera(int periodo)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaCabecera, periodo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }

            }
            return entitys;
        }


        public IDataReader GetDataReaderCuerpo(int periodo)
        {
            string sql = String.Format(helper.SqlListaCabeceraBody, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            string query = "SELECT CRD.CRDETHORA";
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    query = query + ",SUM(CASE WHEN CRD.GRUPOCODI = " + dr["GRUPOCODI"] + " THEN CRD.CRDETCVT END) AS \"" + dr["GRUPOCODI"] + "_CV\"";
                    query = query + ",SUM(CASE WHEN CRD.GRUPOCODI = " + dr["GRUPOCODI"] + " THEN CRD.CRDETCMG END) AS \"" + dr["GRUPOCODI"] + "_CMG\"";
                    query = query + ",SUM(CASE WHEN CRD.GRUPOCODI = " + dr["GRUPOCODI"] + " THEN CRD.CRDETVALOR END) AS \"" + dr["GRUPOCODI"] + "_ENE\"";
                    query = query + ",SUM(CASE WHEN CRD.GRUPOCODI = " + dr["GRUPOCODI"] + " THEN CRD.CRDETCOMPENSACION END) AS \"" + dr["GRUPOCODI"] + "_COM\"";
                    query = query + ",MAX(CASE WHEN CRD.GRUPOCODI = " + dr["GRUPOCODI"] + " THEN SCE.SUBCAUSADESC END) AS \"" + dr["GRUPOCODI"] + "_CALIF\"";
                }
            }
            query = query + " FROM VCE_COMP_REGULAR_DET CRD LEFT JOIN EVE_SUBCAUSAEVENTO SCE ON CRD.SUBCAUSACODI = SCE.SUBCAUSACODI";
            query = query + " WHERE CRD.PECACODI = " + periodo;
            query = query + " GROUP BY CRD.CRDETHORA";
            query = query + " ORDER BY 1";

            command = dbProvider.GetSqlStringCommand(query);
            IDataReader reader = dbProvider.ExecuteReader(command);
            return reader;
        }

        public int GetGrupoCodi(string desc)
        {
            string query = string.Format(helper.SqlGetGrupoCodi, desc);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            int grupoCodi = 0;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) grupoCodi = dr.GetInt32(iGrupocodi);
                }
            }
            return grupoCodi;
        }

        // DSH 06-06-2017 : Se actualizo por requerimiento
        public List<PrGrupodatDTO> GetModosOperacion(string tipocatecodi)
        {
            string condicion = "";

            if (!tipocatecodi.Equals("") && tipocatecodi != null)
            {
                if (tipocatecodi.Equals("2"))
                {
                    condicion = condicion + " and catecodi = 2 ";
                }

                if (tipocatecodi.Equals("2/9"))
                {
                    condicion = condicion + " and catecodi in (2,9) ";
                }

            }

            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();

            string sql = string.Format(helper.SqlListaModoOperacion, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametroPorModoOperacionPorFecha(string grupos, DateTime fechaDatos)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroPorModoOperacionPorFecha, grupos, fechaDatos.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        // Fin de Agregado - Sistema de Compensaciones
        
        public decimal GetValorModoOperacion(int grupoCodi, int concepCodi, DateTime fecha)
        {
            string query = string.Format(helper.SqlValorModoOperacion, grupoCodi, concepCodi,
                fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            string formuladat = "0";

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) formuladat = dr.GetString(iFormuladat);

                    return Convert.ToDecimal(formuladat);
                }
            }

            return 0;
        }

        #region NotificacionesCambiosEquipamiento
        public List<PrGrupodatDTO> ListadoConceptosActualizados(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlConceptosModificados, dtFechaInicio.ToString("dd-MM-yyyy HH:mm"), dtFechaFin.ToString("dd-MM-yyyy HH:mm"));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();


                    int iDesConcepto = dr.GetOrdinal("CONCEPDESC");
                    if (!dr.IsDBNull(iDesConcepto)) entity.ConcepDesc = dr.GetString(iDesConcepto);

                    int iDesGrupo = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iDesGrupo)) entity.GrupoNomb = dr.GetString(iDesGrupo);

                    int iFormulaDat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormulaDat)) entity.Formuladat = dr.GetString(iFormulaDat);

                    int iFechaActualizacion = dr.GetOrdinal(helper.Fechaact);
                    if (!dr.IsDBNull(iFechaActualizacion)) entity.Fechaact = dr.GetDateTime(iFechaActualizacion);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iLastUser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

                    int iDeleted = dr.GetOrdinal(helper.Deleted);
                    if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));

                    int iIdConcepto = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iIdConcepto)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iIdConcepto));

                    int iIdGrupo = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iIdGrupo)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iIdGrupo));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupoestado = dr.GetOrdinal(helper.Grupoestado);
                    if (!dr.IsDBNull(iGrupoestado)) entity.Grupoestado = dr.GetString(iGrupoestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Curva Consumo de combustible

        public List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustibleporGrupoCodi(string GrupoCodi, DateTime fecha)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroCurvaConsumoporGrupoCodi, GrupoCodi,
                fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iFechaact = dr.GetOrdinal(helper.Fechaact);
                    if (!dr.IsDBNull(iFechaact)) entity.Fechaact = dr.GetDateTime(iFechaact);

                    int lastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(lastuser)) entity.Lastuser = dr.GetString(lastuser);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerParametrosCurvasConsumoCombustibleporFecha(DateTime fecha)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlObtenerParametroCurvaConsumoporFecha,
                fecha.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iConcepuni = dr.GetOrdinal(helper.Concepunid);
                    if (!dr.IsDBNull(iConcepuni)) entity.ConcepUni = dr.GetString(iConcepuni);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iGrupocodiNCP = dr.GetOrdinal(helper.GrupocodiNCP);
                    if (!dr.IsDBNull(iGrupocodiNCP)) entity.GrupocodiNCP = Convert.ToInt32(dr.GetValue(iGrupocodiNCP));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrGrupodatDTO BuscaIDCurvaPrincipal(int curvcodi)
        {
            PrGrupodatDTO entity = null;

            string sql = String.Format(helper.SqlObtenerIDCurvaPrincipal, curvcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupodatDTO();

                    int iCurvgrupocodiprincipal = dr.GetOrdinal(helper.Curvgrupocodiprincipal);
                    if (!dr.IsDBNull(iCurvgrupocodiprincipal)) entity.Curvgrupocodiprincipal = Convert.ToInt32(dr.GetValue(iCurvgrupocodiprincipal));

                }
            }

            return entity;
        }

        public string ObtenerFechaEdicionCurva(int grupoCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerFechaEdicionCurva);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupoCodi);

            string resultado = string.Empty;

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                resultado = Convert.ToString(result);
            }

            return resultado;
        }

        #endregion

        #region MigracionSGOCOES-GrupoB
        
        public List<PrGrupodatDTO> ListarParametrosActualizadosPorFecha(DateTime fecha)
        {
            var resultado = new List<PrGrupodatDTO>();
            var sComando = string.Format(helper.SqlParametrosActualizadosPorFecha2, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrGrupodatDTO();
                    oDatoConc.GrupoNomb = dr.IsDBNull(dr.GetOrdinal("GRUPONOMB")) ? "" : dr.GetString(dr.GetOrdinal("GRUPONOMB"));
                    oDatoConc.ConcepDesc = dr.IsDBNull(dr.GetOrdinal("CONCEPDESC")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPDESC"));
                    oDatoConc.Concepabrev = dr.IsDBNull(dr.GetOrdinal("CONCEPABREV")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPABREV"));
                    oDatoConc.Formuladat = dr.IsDBNull(dr.GetOrdinal("FORMULADAT")) ? "" : dr.GetString(dr.GetOrdinal("FORMULADAT"));
                    oDatoConc.ConcepUni = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.Fechadat = dr.IsDBNull(dr.GetOrdinal("FECHADAT")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("FECHADAT"));
                    oDatoConc.Lastuser = dr.IsDBNull(dr.GetOrdinal("LASTUSER")) ? "" : dr.GetString(dr.GetOrdinal("LASTUSER"));
                    oDatoConc.Grupocodi = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("GRUPOCODI"));
                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }

        public List<PrGrupodatDTO> ReporteControlCambios(DateTime fecha)
        {
            var resultado = new List<PrGrupodatDTO>();
            var sComando = string.Format(helper.SqlReporteControlCambios, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = this.helper.Create(dr);

                    int iCambio = dr.GetOrdinal(helper.Cambio);
                    if (!dr.IsDBNull(iCambio)) entity.Cambio = dr.GetInt32(iCambio);

                    int iConcepabrev = dr.GetOrdinal(this.helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConcepDesc = dr.GetOrdinal(this.helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepDesc)) entity.ConcepDesc = dr.GetString(iConcepDesc);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    resultado.Add(entity);
                }
            }
            return resultado;
        }

        #endregion

        public List<PrGrupodatDTO> ListarAsignacionBarraModoOperacion(int grupocodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string sql = String.Format(helper.SqlListaBarraModoOperacion, grupocodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iBarrbarratransferencia = dr.GetOrdinal(helper.Barrbarratransferencia);
                    if (!dr.IsDBNull(iBarrbarratransferencia)) entity.Barrbarratransferencia = dr.GetString(iBarrbarratransferencia);

                    int iFechaact = dr.GetOrdinal(helper.Fechaact);
                    if (!dr.IsDBNull(iFechaact)) entity.Fechaact = dr.GetDateTime(iFechaact);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region SGOCOES Func A

        public List<PrGrupodatDTO> ObtenerListaConfigScoSinac(DateTime fecha, int nroPage, int pageSize)
        {
            var resultado = new List<PrGrupodatDTO>();
            var sComando = string.Format(helper.SqlObtenerListaConfigScoSinac, fecha.ToString(ConstantesBase.FormatoFecha) + " 23:59:59", nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new PrGrupodatDTO();
                    oDatoConc.Fechadat = dr.IsDBNull(dr.GetOrdinal("FECHADAT")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("FECHADAT"));
                    oDatoConc.Grupocodi = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("GRUPOCODI"));
                    oDatoConc.GrupoNomb = dr.IsDBNull(dr.GetOrdinal("GRUPONOMB")) ? "" : dr.GetString(dr.GetOrdinal("GRUPONOMB"));
                    oDatoConc.Concepcodi = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.Formuladat = dr.IsDBNull(dr.GetOrdinal("FORMULADAT")) ? "" : dr.GetString(dr.GetOrdinal("FORMULADAT"));
                    oDatoConc.ConcepDesc = dr.IsDBNull(dr.GetOrdinal("CONCEPDESC")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPDESC"));
                    oDatoConc.Deleted = dr.IsDBNull(dr.GetOrdinal("DELETED")) ? -1 : dr.GetInt32(dr.GetOrdinal("DELETED"));

                    resultado.Add(oDatoConc);
                }
            }
            return resultado;
        }

        public int ObtenerTotalListaConfigScoSinac(DateTime fecha)
        {
            String query = String.Format(helper.SqlNroRegListaConfigScoSinac,
                fecha.ToString(ConstantesBase.FormatoFecha) + " 23:59:59");

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        #endregion SGOCOES Func A

        #region FIT - VALORIZACION DIARIA

        public PrGrupodatDTO ObtenerParametroValorizacion(DateTime fecha, int grupocodi, int concepcodi)
        {
            var entity = new PrGrupodatDTO();
            var sComando = string.Format(helper.SqlObtenerParametroValorizacion, grupocodi, concepcodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);
                }
            }
            return entity;
        }

        #endregion

        #region SIOSEIN2
        public List<PrGrupodatDTO> GetByCriteriaConceptoFecha(string concepcodi, DateTime fechaInicio, DateTime fechaFin)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetByCriteriaConceptoFecha, concepcodi, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerTodoParametroGeneral(DateTime fecha)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlObtenerTodoParametroGeneral, fecha.ToString(ConstantesBase.FormatoFecha)));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupodatDTO> ObtenerTodoParametroModoOperacion(string grupocodi, DateTime fecha)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            var query = string.Format(helper.SqlObtenerTodoParametroModoOperacion, grupocodi, fecha.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Numerales Datos Base

        public List<PrGrupodatDTO> ListaFechas_5_5_x(int stConcepcodi, string fechaIni)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBaseFecha_5_5_x, stConcepcodi, fechaIni);

            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);



                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupodatDTO> ListaNumerales_DatosBase_5_5_n(int concepcCodi, string stiniVA, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_5_n, concepcCodi, stiniVA, fechaFin);

            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iDeleted = dr.GetOrdinal(helper.Deleted);
                    if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));



                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupodatDTO> ListaNumerales_DatosBase_5_5_2(string stiniTC, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_5_2, stiniTC, fechaFin);

            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iFormuladat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormuladat)) entity.Formuladat = dr.GetString(iFormuladat);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iDeleted = dr.GetOrdinal(helper.Deleted);
                    if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));



                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupodatDTO> ListaNumerales_DatosBase_5_6_4()
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_4);

            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int IFormula = dr.GetOrdinal(helper.Formula);
                    if (!dr.IsDBNull(IFormula)) entity.Formula = dr.GetString(IFormula);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region Subastas
        public List<PrGrupodatDTO> ParametrosConfiguracionPorFecha(DateTime fecha, string grupos, string conceptos)
        {
            var resultado = new List<PrGrupodatDTO>();
            var sComando = string.Format(helper.SqlParametrosConfiguracionPorFecha, fecha.ToString(ConstantesBase.FormatoFecha), grupos, conceptos);
            DbCommand command = dbProvider.GetSqlStringCommand(sComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupodatDTO();
                    entity.ConcepTipo = dr.IsDBNull(dr.GetOrdinal("CONCEPTIPO")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPTIPO"));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    entity.GrupoNomb = "";
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entity.Formuladat = "";
                    int iFormulaDat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormulaDat)) entity.Formuladat = dr.GetString(iFormulaDat);

                    int iFechaDat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechaDat)) entity.Fechadat = dr.GetDateTime(iFechaDat);

                    int iGdatcomentario = dr.GetOrdinal(helper.Gdatcomentario);
                    if (!dr.IsDBNull(iGdatcomentario)) entity.Gdatcomentario = dr.GetString(iGdatcomentario);

                    int iGdatsustento = dr.GetOrdinal(helper.Gdatsustento);
                    if (!dr.IsDBNull(iGdatsustento)) entity.Gdatsustento = dr.GetString(iGdatsustento);

                    int iGdatcheckcero = dr.GetOrdinal(helper.Gdatcheckcero);
                    if (!dr.IsDBNull(iGdatcheckcero)) entity.Gdatcheckcero = Convert.ToInt32(dr.GetValue(iGdatcheckcero));

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iFechaact = dr.GetOrdinal(this.helper.Fechaact);
                    if (!dr.IsDBNull(iFechaact)) entity.Fechaact = dr.GetDateTime(iFechaact);

                    int iConcepcodi = dr.GetOrdinal(this.helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = dr.GetInt32(iConcepcodi);

                    int iConcepunid = dr.GetOrdinal(this.helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.ConcepUni = dr.GetString(iConcepunid);

                    entity.Concepabrev = "";
                    int iConcepabrev = dr.GetOrdinal(this.helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    entity.ConcepDesc = "";
                    int iConcepDesc = dr.GetOrdinal(this.helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepDesc)) entity.ConcepDesc = dr.GetString(iConcepDesc);

                    int iConcepnombficha = dr.GetOrdinal(helper.Concepnombficha);
                    if (!dr.IsDBNull(iConcepnombficha)) entity.Concepnombficha = dr.GetString(iConcepnombficha);

                    int iConcepfichatec = dr.GetOrdinal(helper.Concepfichatec);
                    if (!dr.IsDBNull(iConcepfichatec)) entity.Concepfichatec = dr.GetString(iConcepfichatec);

                    int iConceppropeq = dr.GetOrdinal(this.helper.Conceppropeq);
                    if (!dr.IsDBNull(iConceppropeq)) entity.Conceppropeq = Convert.ToInt32(dr.GetValue(iConceppropeq));

                    int iConcepocultocomentario = dr.GetOrdinal(helper.Concepocultocomentario);
                    if (!dr.IsDBNull(iConcepocultocomentario)) entity.Concepocultocomentario = dr.GetString(iConcepocultocomentario);

                    resultado.Add(entity);
                }
            }
            return resultado;
        }

        public List<PrGrupodatDTO> BuscarEliminados(DateTime fechadat, int concepcodi, int grupocodi)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBuscarEliminados);

            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, fechadat);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #endregion

        #region FICHA TÉCNICA

        public void Save(PrGrupodatDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;

                dbCommand.CommandText = helper.SqlSave;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechadat, DbType.DateTime, entity.Fechadat));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepcodi, DbType.Int32, entity.Concepcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Formuladat, DbType.String, entity.Formuladat));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Deleted, DbType.Int32, entity.Deleted));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechaact, DbType.DateTime, entity.Fechaact ?? DateTime.Now));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatcomentario, DbType.String, entity.Gdatcomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatsustento, DbType.String, entity.Gdatsustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatcheckcero, DbType.Int32, entity.Gdatcheckcero));

                dbCommand.ExecuteNonQuery();
            }
        }

        public void Update(PrGrupodatDTO entity, IDbConnection connection, IDbTransaction transaction)
        {

            using (var dbCommand = (DbCommand)connection.CreateCommand())
            {
                dbCommand.Transaction = (DbTransaction)transaction;
                dbCommand.Connection = (DbConnection)connection;
                dbCommand.CommandText = helper.SqlUpdate;

                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Lastuser, DbType.String, entity.Lastuser));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Formuladat, DbType.String, entity.Formuladat));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechaact, DbType.DateTime, entity.Fechaact ?? DateTime.Now));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Deleted2, DbType.Int32, entity.Deleted2));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatcomentario, DbType.String, entity.Gdatcomentario));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatsustento, DbType.String, entity.Gdatsustento));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Gdatcheckcero, DbType.Int32, entity.Gdatcheckcero));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Fechadat, DbType.DateTime, entity.Fechadat));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Concepcodi, DbType.Int32, entity.Concepcodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Grupocodi, DbType.Int32, entity.Grupocodi));
                dbCommand.Parameters.Add(dbProvider.CreateParameter(dbCommand, helper.Deleted, DbType.Int32, entity.Deleted));

                dbCommand.ExecuteNonQuery();
            }
        }

        public List<PrGrupodatDTO> ListarGrupoConValorModificado(DateTime dtFechaInicio, DateTime dtFechaFin, string concepcodis, string catecodis)
        {
            var entitys = new List<PrGrupodatDTO>();
            string query = String.Format(helper.SqlListarGrupoConValorModificado, dtFechaInicio.ToString(ConstantesBase.FormatoFecha),
                                                            dtFechaFin.ToString(ConstantesBase.FormatoFecha), concepcodis, catecodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoestado = dr.GetOrdinal(helper.Grupoestado);
                    if (!dr.IsDBNull(iGrupoestado)) entity.Grupoestado = dr.GetString(iGrupoestado);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

                    int iDesEmpresa = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iDesEmpresa)) entity.Emprnomb = dr.GetString(iDesEmpresa);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFechadat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechadat)) entity.Fechadat = dr.GetDateTime(iFechadat);

                    int iIdEmpresa = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iIdEmpresa)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iIdEmpresa));

                    int iCatecodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion
    }
}
