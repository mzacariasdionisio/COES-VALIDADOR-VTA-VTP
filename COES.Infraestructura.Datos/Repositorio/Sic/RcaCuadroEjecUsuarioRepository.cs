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
    /// Clase de acceso a datos de la tabla RCA_CUADRO_EJEC_USUARIO
    /// </summary>
    public class RcaCuadroEjecUsuarioRepository: RepositoryBase, IRcaCuadroEjecUsuarioRepository
    {
        public RcaCuadroEjecUsuarioRepository(string strConn): base(strConn)
        {
        }

        RcaCuadroEjecUsuarioHelper helper = new RcaCuadroEjecUsuarioHelper();

        public int Save(RcaCuadroEjecUsuarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, entity.Rcproucodi);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcejeucargarechazada, DbType.Decimal, entity.Rcejeucargarechazada);
            dbProvider.AddInParameter(command, helper.Rcejeutiporeporte, DbType.String, entity.Rcejeutiporeporte);
            dbProvider.AddInParameter(command, helper.Rcejeufechorinicio, DbType.DateTime, entity.Rcejeufechorinicio);
            dbProvider.AddInParameter(command, helper.Rcejeufechorfin, DbType.DateTime, entity.Rcejeufechorfin);
            dbProvider.AddInParameter(command, helper.Rcejeuestregistro, DbType.String, entity.Rcejeuestregistro);
            dbProvider.AddInParameter(command, helper.Rcejeuusucreacion, DbType.String, entity.Rcejeuusucreacion);
            dbProvider.AddInParameter(command, helper.Rcejeufeccreacion, DbType.DateTime, entity.Rcejeufeccreacion);
            //dbProvider.AddInParameter(command, helper.Rcejeuusumodificacion, DbType.String, entity.Rcejeuusumodificacion);
            //dbProvider.AddInParameter(command, helper.Rcejeufecmodificacion, DbType.DateTime, entity.Rcejeufecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCuadroEjecUsuarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, entity.Rcproucodi);
            //dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            //dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Rcejeucargarechazada, DbType.Decimal, entity.Rcejeucargarechazada);
            dbProvider.AddInParameter(command, helper.Rcejeutiporeporte, DbType.String, entity.Rcejeutiporeporte);
            dbProvider.AddInParameter(command, helper.Rcejeufechorinicio, DbType.DateTime, entity.Rcejeufechorinicio);
            dbProvider.AddInParameter(command, helper.Rcejeufechorfin, DbType.DateTime, entity.Rcejeufechorfin);
            dbProvider.AddInParameter(command, helper.Rcejeuestregistro, DbType.String, entity.Rcejeuestregistro);
            //dbProvider.AddInParameter(command, helper.Rcejeuusucreacion, DbType.String, entity.Rcejeuusucreacion);
            //dbProvider.AddInParameter(command, helper.Rcejeufeccreacion, DbType.DateTime, entity.Rcejeufeccreacion);
            dbProvider.AddInParameter(command, helper.Rcejeuusumodificacion, DbType.String, entity.Rcejeuusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcejeufecmodificacion, DbType.DateTime, entity.Rcejeufecmodificacion);
            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, entity.Rcejeucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcejeucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, rcejeucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCuadroEjecUsuarioDTO GetById(int rcejeucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcejeucodi, DbType.Int32, rcejeucodi);
            RcaCuadroEjecUsuarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCuadroEjecUsuarioDTO> List()
        {
            List<RcaCuadroEjecUsuarioDTO> entitys = new List<RcaCuadroEjecUsuarioDTO>();
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

        public List<RcaCuadroEjecUsuarioDTO> GetByCriteria(int rcproucodi)
        {
            List<RcaCuadroEjecUsuarioDTO> entitys = new List<RcaCuadroEjecUsuarioDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, rcproucodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new RcaCuadroEjecUsuarioDTO();

                    int iRcejeucodi = dr.GetOrdinal(helper.Rcejeucodi);
                    if (!dr.IsDBNull(iRcejeucodi)) entity.Rcejeucodi = Convert.ToInt32(dr.GetValue(iRcejeucodi));

                    int iRcproucodi = dr.GetOrdinal(helper.Rcproucodi);
                    if (!dr.IsDBNull(iRcproucodi)) entity.Rcproucodi = Convert.ToInt32(dr.GetValue(iRcproucodi));
                  
                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeutiporeporte = dr.GetOrdinal(helper.Rcejeutiporeporte);
                    if (!dr.IsDBNull(iRcejeutiporeporte)) entity.Rcejeutiporeporte = dr.GetString(iRcejeutiporeporte);

                    int iRcejeufechorinicio = dr.GetOrdinal(helper.Rcejeufechorinicio);
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcejeufechorinicio = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal(helper.Rcejeufechorfin);
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaCuadroEjecUsuarioDTO> ListFiltro(string programa, string cuadroPrograma, string codigoSuministrador, string tipoReporte)
        {
            List<RcaCuadroEjecUsuarioDTO> entitys = new List<RcaCuadroEjecUsuarioDTO>();
            var condicion = string.Empty;

            if (!string.IsNullOrEmpty(cuadroPrograma))
            {
                condicion = condicion + " AND CP.RCCUADCODI = " + cuadroPrograma;
            }            

            if (!string.IsNullOrEmpty(codigoSuministrador))
            {
                condicion = condicion + " AND CP.RCPROUEMPRCODISUMINISTRADOR = " + codigoSuministrador;
            }

            string queryString = string.Format(helper.SqlListFiltro, tipoReporte, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroEjecUsuarioDTO entity = new RcaCuadroEjecUsuarioDTO();

                    int iRcejeucodi = dr.GetOrdinal(helper.Rcejeucodi);
                    if (!dr.IsDBNull(iRcejeucodi)) entity.Rcejeucodi = Convert.ToInt32(dr.GetValue(iRcejeucodi));

                    int iRcproucodi = dr.GetOrdinal(helper.Rcproucodi);
                    if (!dr.IsDBNull(iRcproucodi)) entity.Rcproucodi = Convert.ToInt32(dr.GetValue(iRcproucodi));

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeutiporeporte = dr.GetOrdinal(helper.Rcejeutiporeporte);
                    if (!dr.IsDBNull(iRcejeutiporeporte)) entity.Rcejeutiporeporte = dr.GetString(iRcejeutiporeporte);

                    int iRcejeufechorinicio = dr.GetOrdinal(helper.Rcejeufechorinicio);
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcejeufechorinicio = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal(helper.Rcejeufechorfin);
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);                                       

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);                   

                    int iEquinomb = dr.GetOrdinal(helper.Puntomedicion);
                    if (!dr.IsDBNull(iEquinomb)) entity.Puntomedicion = dr.GetString(iEquinomb);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcproufuente = dr.GetOrdinal(helper.Rcproufuente);
                    if (!dr.IsDBNull(iRcproufuente)) entity.Rcproufuente = dr.GetString(iRcproufuente);

                    int iRcproudemanda = dr.GetOrdinal(helper.Rcproudemanda);
                    if (!dr.IsDBNull(iRcproudemanda)) entity.Rcproudemanda = dr.GetDecimal(iRcproudemanda);

                    int iRcproudemandaracionar = dr.GetOrdinal(helper.Rcproudemandaracionar);
                    if (!dr.IsDBNull(iRcproudemandaracionar)) entity.Rcproudemandaracionar = dr.GetDecimal(iRcproudemandaracionar);

                    int iRcprouemprcodisuministrador = dr.GetOrdinal(helper.Rcprouemprcodisuministrador);
                    if (!dr.IsDBNull(iRcprouemprcodisuministrador)) entity.Rcprouemprcodisuministrador = Convert.ToInt32(dr.GetValue(iRcprouemprcodisuministrador));

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iRcproudemandareal = dr.GetOrdinal(helper.Rcproudemandareal);
                    if (!dr.IsDBNull(iRcproudemandareal)) entity.Rcproudemandareal = dr.GetDecimal(iRcproudemandareal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}
