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
    /// Clase de acceso a datos de la tabla RCA_CUADRO_PROG_USUARIO
    /// </summary>
    public class RcaCuadroProgUsuarioRepository : RepositoryBase, IRcaCuadroProgUsuarioRepository
    {
        public RcaCuadroProgUsuarioRepository(string strConn) : base(strConn)
        {
        }

        RcaCuadroProgUsuarioHelper helper = new RcaCuadroProgUsuarioHelper();

        public int Save(RcaCuadroProgUsuarioDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            string queryString = string.Format(helper.SqlSave, entity.Rcprouemprcodisuministrador);
            command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, id);
            //dbProvider.AddInParameter(command, helper.Rcprouusumodificacion, DbType.String, entity.Rcprouusumodificacion);
            //dbProvider.AddInParameter(command, helper.Rcproufecmodificacion, DbType.DateTime, entity.Rcproufecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            //dbProvider.AddInParameter(command, helper.Rcprouemprcodisuministrador, DbType.Int32,
            //    entity.Rcprouemprcodisuministrador > 0 ? entity.Rcprouemprcodisuministrador : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Rcproudemanda, DbType.Decimal, entity.Rcproudemanda);
            dbProvider.AddInParameter(command, helper.Rcproudemandareal, DbType.Decimal, entity.Rcproudemandareal);
            dbProvider.AddInParameter(command, helper.Rcproufuente, DbType.String, entity.Rcproufuente);
            dbProvider.AddInParameter(command, helper.Rcproudemandaracionar, DbType.Decimal, entity.Rcproudemandaracionar);
            dbProvider.AddInParameter(command, helper.Rcprouestregistro, DbType.String, entity.Rcprouestregistro);
            dbProvider.AddInParameter(command, helper.Rcproucargadisponible, DbType.Decimal, entity.Rcproucargadisponible);
            dbProvider.AddInParameter(command, helper.Rcproufactork, DbType.Decimal, entity.Rcproufactork);
            dbProvider.AddInParameter(command, helper.Rcproudemandaatender, DbType.Decimal, entity.Rcproudemandaatender);
            dbProvider.AddInParameter(command, helper.Rcprouusucreacion, DbType.String, entity.Rcprouusucreacion);
            dbProvider.AddInParameter(command, helper.Rcproufeccreacion, DbType.DateTime, entity.Rcproufeccreacion);

            dbProvider.AddInParameter(command, helper.Rcproucargarechazarcoord, DbType.Decimal, entity.Rcproucargarechazarcoord);
            dbProvider.AddInParameter(command, helper.Rccuadhorinicoord, DbType.String, entity.Rccuadhorinicoord);
            dbProvider.AddInParameter(command, helper.Rccuadhorfincoord, DbType.String, entity.Rccuadhorfincoord);
            dbProvider.AddInParameter(command, helper.Rcproucargarechazarejec, DbType.Decimal, entity.Rcproucargarechazarejec);
            dbProvider.AddInParameter(command, helper.Rccuadhoriniejec, DbType.String, entity.Rccuadhoriniejec);
            dbProvider.AddInParameter(command, helper.Rccuadhorfinejec, DbType.String, entity.Rccuadhorfinejec);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCuadroProgUsuarioDTO entity)
        {
            string queryString = string.Format(helper.SqlUpdate, entity.Rcprouemprcodisuministrador);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Rcprouusumodificacion, DbType.String, entity.Rcprouusumodificacion);
            dbProvider.AddInParameter(command, helper.Rcproufecmodificacion, DbType.DateTime, entity.Rcproufecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi > 0 ? entity.Equicodi : (object)DBNull.Value);
            //dbProvider.AddInParameter(command, helper.Rcprouemprcodisuministrador, DbType.Int32, 
            //    entity.Rcprouemprcodisuministrador > 0 ? entity.Rcprouemprcodisuministrador : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Rcproudemanda, DbType.Decimal, entity.Rcproudemanda);
            dbProvider.AddInParameter(command, helper.Rcproudemandareal, DbType.Decimal, entity.Rcproudemandareal);
            dbProvider.AddInParameter(command, helper.Rcproufuente, DbType.String, entity.Rcproufuente);
            dbProvider.AddInParameter(command, helper.Rcproudemandaracionar, DbType.Decimal, entity.Rcproudemandaracionar);
            dbProvider.AddInParameter(command, helper.Rcprouestregistro, DbType.String, entity.Rcprouestregistro);
            dbProvider.AddInParameter(command, helper.Rcproucargadisponible, DbType.Decimal, entity.Rcproucargadisponible);
            dbProvider.AddInParameter(command, helper.Rcproufactork, DbType.Decimal, entity.Rcproufactork);
            dbProvider.AddInParameter(command, helper.Rcproudemandaatender, DbType.Decimal, entity.Rcproudemandaatender);

            dbProvider.AddInParameter(command, helper.Rcproucargarechazarcoord, DbType.Decimal, entity.Rcproucargarechazarcoord);
            dbProvider.AddInParameter(command, helper.Rccuadhorinicoord, DbType.String, entity.Rccuadhorinicoord);
            dbProvider.AddInParameter(command, helper.Rccuadhorfincoord, DbType.String, entity.Rccuadhorfincoord);
            dbProvider.AddInParameter(command, helper.Rcproucargarechazarejec, DbType.Decimal, entity.Rcproucargarechazarejec);
            dbProvider.AddInParameter(command, helper.Rccuadhoriniejec, DbType.String, entity.Rccuadhoriniejec);
            dbProvider.AddInParameter(command, helper.Rccuadhorfinejec, DbType.String, entity.Rccuadhorfinejec);
            //dbProvider.AddInParameter(command, helper.Rcprouusucreacion, DbType.String, entity.Rcprouusucreacion);
            //dbProvider.AddInParameter(command, helper.Rcproufeccreacion, DbType.DateTime, entity.Rcproufeccreacion);
            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, entity.Rcproucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int rcproucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, rcproucodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCuadroProgUsuarioDTO GetById(int rcproucodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rcproucodi, DbType.Int32, rcproucodi);
            RcaCuadroProgUsuarioDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCuadroProgUsuarioDTO> List()
        {
            List<RcaCuadroProgUsuarioDTO> entitys = new List<RcaCuadroProgUsuarioDTO>();
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

        public List<RcaCuadroProgUsuarioDTO> GetByCriteria()
        {
            List<RcaCuadroProgUsuarioDTO> entitys = new List<RcaCuadroProgUsuarioDTO>();
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

        public List<RcaCuadroProgUsuarioDTO> ListProgramaRechazoCarga(string empresasId, string codigoCuadroPrograma)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            var condicion = String.Empty;
            if (!String.IsNullOrEmpty(empresasId))
            {
                condicion = condicion + " AND EM.EMPRCODI IN (" + empresasId + ")";
            }

            if (!String.IsNullOrEmpty(codigoCuadroPrograma))
            {
                condicion = condicion + " AND PU.RCCUADCODI = " + codigoCuadroPrograma;
            }

            string queryString = string.Format(helper.SqlListProgramaRechazoCarga, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iRcproucodi = dr.GetOrdinal(helper.Rcproucodi);
                    if (!dr.IsDBNull(iRcproucodi)) entity.Rcproucodi = Convert.ToInt32(dr.GetValue(iRcproucodi));

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Puntomedicion);
                    if (!dr.IsDBNull(iEquinomb)) entity.Puntomedicion = dr.GetString(iEquinomb);

                    int iRcproufuente = dr.GetOrdinal(helper.Rcproufuente);
                    if (!dr.IsDBNull(iRcproufuente)) entity.Rcproufuente = dr.GetString(iRcproufuente);

                    int iRcproudemanda = dr.GetOrdinal(helper.Rcproudemanda);
                    if (!dr.IsDBNull(iRcproudemanda)) entity.Rcproudemanda = dr.GetDecimal(iRcproudemanda);

                    int iRcproucargadisponible = dr.GetOrdinal(helper.Rcproucargadisponible);
                    if (!dr.IsDBNull(iRcproucargadisponible)) entity.Rcproucargadisponible = dr.GetDecimal(iRcproucargadisponible);

                    int iRcproufactork = dr.GetOrdinal(helper.Rcproufactork);
                    if (!dr.IsDBNull(iRcproufactork)) entity.Rcproufactork = dr.GetDecimal(iRcproufactork);

                    int iRcproudemandaracionar = dr.GetOrdinal(helper.Rcproudemandaracionar);
                    if (!dr.IsDBNull(iRcproudemandaracionar)) entity.Rcproudemandaracionar = dr.GetDecimal(iRcproudemandaracionar);

                    int iCargaluegoaplicar = dr.GetOrdinal(helper.Rcproudemandaatender);
                    if (!dr.IsDBNull(iCargaluegoaplicar)) entity.Rcproudemandaatender = dr.GetDecimal(iCargaluegoaplicar);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcprouemprcodisuministrador = dr.GetOrdinal(helper.Rcprouemprcodisuministrador);
                    if (!dr.IsDBNull(iRcprouemprcodisuministrador)) entity.Rcprouemprcodisuministrador = Convert.ToInt32(dr.GetValue(iRcprouemprcodisuministrador));

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);


                    int iRcproucargarechazarcoord = dr.GetOrdinal(helper.Rcproucargarechazarcoord);
                    if (!dr.IsDBNull(iRcproucargarechazarcoord)) entity.Rcproucargarechazarcoord = dr.GetDecimal(iRcproucargarechazarcoord);

                    int iRccuadhorinicoord = dr.GetOrdinal(helper.Rccuadhorinicoord);
                    if (!dr.IsDBNull(iRccuadhorinicoord)) entity.Rccuadhorinicoord = dr.GetString(iRccuadhorinicoord);

                    int iRccuadhorfincoord = dr.GetOrdinal(helper.Rccuadhorfincoord);
                    if (!dr.IsDBNull(iRccuadhorfincoord)) entity.Rccuadhorfincoord = dr.GetString(iRccuadhorfincoord);

                    int iRcproucargarechazarejec = dr.GetOrdinal(helper.Rcproucargarechazarejec);
                    if (!dr.IsDBNull(iRcproucargarechazarejec)) entity.Rcproucargarechazarejec = dr.GetDecimal(iRcproucargarechazarejec);

                    int iRccuadhoriniejec = dr.GetOrdinal(helper.Rccuadhoriniejec);
                    if (!dr.IsDBNull(iRccuadhoriniejec)) entity.Rccuadhoriniejec = dr.GetString(iRccuadhoriniejec);

                    int iRccuadhorfinejec = dr.GetOrdinal(helper.Rccuadhorfinejec);
                    if (!dr.IsDBNull(iRccuadhorfinejec)) entity.Rccuadhorfinejec = dr.GetString(iRccuadhorfinejec);

                    int iRcproudemandareal = dr.GetOrdinal(helper.Rcproudemandareal);
                    if (!dr.IsDBNull(iRcproudemandareal)) entity.Rcproudemandareal = dr.GetDecimal(iRcproudemandareal);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ListEmpresasProgramaRechazoCarga(string bloqueHorario, string zona, string estacion, decimal medicion, string nombreEmpresa, string empresasId, string equiposId)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            var condicion = String.Empty;

            if (!String.IsNullOrEmpty(estacion))
            {
                //condicion = condicion + " AND EQ.AREACODI = " + estacion;
                condicion = condicion + " AND EQ.AREACODI IN ( " + estacion + " )";
            }

            if (!String.IsNullOrEmpty(nombreEmpresa))
            {
                condicion = condicion + " AND EMP.EMPRRAZSOCIAL LIKE '%" + nombreEmpresa.ToUpper() + "%'";
            }

            if (!String.IsNullOrEmpty(empresasId))
            {
                condicion = condicion + " AND EMP.EMPRCODI IN (" + empresasId + ")";
            }

            if (!String.IsNullOrEmpty(equiposId))
            {
                condicion = condicion + " AND EQ.EQUICODI IN (" + equiposId + ")";
            }

            if (medicion > 0)
            {
                var filtroMedicion = @" AND EQ.EMPRCODI IN (SELECT EQX.EMPRCODI

                  FROM RCA_DEMANDA_USUARIO_TMP DEX JOIN EQ_EQUIPO EQX ON DEX.EQUICODI = EQX.EQUICODI

                  JOIN SI_EMPRESA EMPX ON EQX.EMPRCODI = EMPX.EMPRCODI

                  WHERE EQX.FAMCODI = 45 AND EQX.EQUIESTADO='A' AND EQX.OSINERGCODI IS NOT NULL

                  {0}

                  GROUP BY EQX.EMPRCODI

                  HAVING SUM(DEX.{1}) >= {2}   )";

                var condicionMedicion = " ";

                if (!String.IsNullOrEmpty(estacion))
                {
                    //condicionMedicion = condicionMedicion + " AND EQX.AREACODI = " + estacion;
                    condicionMedicion = condicionMedicion + " AND EQX.AREACODI IN ( " + estacion + " )";
                }

                if (!String.IsNullOrEmpty(nombreEmpresa))
                {
                    condicionMedicion = condicionMedicion + " AND EMPX.EMPRRAZSOCIAL LIKE '%" + nombreEmpresa.ToUpper() + "%'";
                }
                filtroMedicion = String.Format(filtroMedicion, condicionMedicion, bloqueHorario.Equals("HP") ? "RCDEUTDEMANDAHP" : "RCDEUTDEMANDAHFP", medicion);

                condicion = condicion + filtroMedicion;
            }

            string queryString = string.Format(helper.SqListEmpresasProgramaRechazoCarga, bloqueHorario.Equals("HP") ? "RCDEUTDEMANDAHP" : "RCDEUTDEMANDAHFP", condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    //int iRcparecodi = dr.GetOrdinal(helper.Rcparecodi);
                    //if (!dr.IsDBNull(iRcparecodi)) entity.Rcparecodi = Convert.ToInt32(dr.GetValue(iRcparecodi));

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Puntomedicion);
                    if (!dr.IsDBNull(iEquinomb)) entity.Puntomedicion = dr.GetString(iEquinomb);

                    int iRcdeulfuente = dr.GetOrdinal(helper.Rcdeulfuente);
                    if (!dr.IsDBNull(iRcdeulfuente)) entity.Rcproufuente = dr.GetString(iRcdeulfuente);

                    int iDemanda = dr.GetOrdinal(helper.Demanda);
                    if (!dr.IsDBNull(iDemanda)) entity.Rcproudemanda = dr.GetDecimal(iDemanda);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcprouemprcodisuministrador = dr.GetOrdinal(helper.Rcprouemprcodisuministrador);
                    if (!dr.IsDBNull(iRcprouemprcodisuministrador)) entity.Rcprouemprcodisuministrador = Convert.ToInt32(dr.GetValue(iRcprouemprcodisuministrador));

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<AreaDTO> ListSubEstacion(int codigoZona)
        {
            List<AreaDTO> entities = new List<AreaDTO>();

            var condicion = String.Empty;

            if (codigoZona > 0)
            {
                condicion = string.Format(" AND A.AREACODI IN (SELECT AREACODI FROM EQ_AREAREL WHERE AREAPADRE = {0}) ", codigoZona);
            }

            string queryString = string.Format(helper.SqlListSubEstacion, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AreaDTO entity = new AreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.AREACODI = Convert.ToInt16(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ListEnvioArchivoMagnitud(int programa, int cuadro, int suministrador, string cumplio)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            var condicion = String.Empty;

            if (programa > 0)
            {
                //condicion = condicion + " EQ.AREACODI = " + estacion;
            }

            if (cuadro > 0)
            {
                var filtro = condicion.Length > 0 ? "AND" : String.Empty;
                condicion = condicion + string.Format(" {0} CP.RCCUADCODI = " + cuadro, filtro);
            }

            if (suministrador > 0)
            {
                var filtro = condicion.Length > 0 ? "AND" : String.Empty;
                condicion = condicion + string.Format(" {0} CP.RCPROUEMPRCODISUMINISTRADOR = " + suministrador, filtro);
            }

            if (!String.IsNullOrEmpty(cumplio))
            {
                var filtro = condicion.Length > 0 ? "AND" : String.Empty;
                condicion = condicion + string.Format(" {0} CASE WHEN EJEP.RCEJEUCARGARECHAZADA IS NOT NULL OR EJED.RCEJEUCARGARECHAZADA IS NOT NULL THEN 'S' ELSE 'N' END = '{1}'", filtro, cumplio);
            }

            if (String.IsNullOrEmpty(condicion))
            {
                condicion = " 1=1";
            }

            string queryString = string.Format(helper.SqlListEnvioArchivoMagnitud, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();


                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iEquinomb = dr.GetOrdinal(helper.Puntomedicion);
                    if (!dr.IsDBNull(iEquinomb)) entity.Puntomedicion = dr.GetString(iEquinomb);

                    int iRcproudemandaracionar = dr.GetOrdinal(helper.Rcproudemandaracionar);
                    if (!dr.IsDBNull(iRcproudemandaracionar)) entity.Rcproudemandaracionar = dr.GetDecimal(iRcproudemandaracionar);

                    int iRcproudemanda = dr.GetOrdinal(helper.Rcproudemanda);
                    if (!dr.IsDBNull(iRcproudemanda)) entity.Rcproudemanda = dr.GetDecimal(iRcproudemanda);

                    int iRcejeucargarechazadapreliminar = dr.GetOrdinal(helper.Rcejeucargarechazadapreliminar);
                    if (!dr.IsDBNull(iRcejeucargarechazadapreliminar)) entity.Rcejeucargarechazadapreliminar = dr.GetDecimal(iRcejeucargarechazadapreliminar);

                    int iRcejeufechoriniciopreliminar = dr.GetOrdinal(helper.Rcejeufechoriniciopreliminar);
                    if (!dr.IsDBNull(iRcejeufechoriniciopreliminar)) entity.Rcejeufechoriniciopreliminar = dr.GetDateTime(iRcejeufechoriniciopreliminar);

                    int iRcejeufechorfinpreliminar = dr.GetOrdinal(helper.Rcejeufechorfinpreliminar);
                    if (!dr.IsDBNull(iRcejeufechorfinpreliminar)) entity.Rcejeufechorfinpreliminar = dr.GetDateTime(iRcejeufechorfinpreliminar);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicio = dr.GetOrdinal(helper.Rcejeufechorinicio);
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcejeufechorinicio = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal(helper.Rcejeufechorfin);
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);

                    int iCumplio = dr.GetOrdinal(helper.Cumplio);
                    if (!dr.IsDBNull(iCumplio)) entity.Cumplio = dr.GetString(iCumplio);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<SiEmpresaDTO> ListSuministrador(int rccuadcodi)
        {
            List<SiEmpresaDTO> entities = new List<SiEmpresaDTO>();

            var condicion = String.Empty;

            string queryString = string.Format(helper.SqlListSuministrador, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, rccuadcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new SiEmpresaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Emprrazsocial = dr.GetString(iAreanomb);

                    entities.Add(entity);
                }
            }
            return entities;
        }

        public void UpdateEstado(int codigoCuadroPrograma, string estado)
        {
            //var listaIn = string.Join(",", codigos);
            string queryString = string.Format(helper.SqlUpdateEstado.Trim(), codigoCuadroPrograma, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<AreaDTO> ListZonas(int codigoNivel)
        {
            List<AreaDTO> entities = new List<AreaDTO>();

            //var condicion = String.Empty;

            //string queryString = string.Format(helper.SqlListSuministrador, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListZonas);
            dbProvider.AddInParameter(command, helper.Anivelcodi, DbType.Int32, codigoNivel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new AreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.AREACODI = dr.GetInt16(iAreacodi);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.AREANOMB = dr.GetString(iAreanomb);

                    entities.Add(entity);
                }
            }
            return entities;
        }

        public void EliminarDemandaUsuarioLibre()
        {
            //string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibre, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarDemandaUsuarioLibre);

            dbProvider.ExecuteNonQuery(command);
        }
        public void CargarDemandaUsuarioLibre()
        {
            //string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibre, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCargarDemandaUsuarioLibre);

            dbProvider.ExecuteNonQuery(command);
        }

        public void CargarDemandaUsuarioLibreSicli(int indiceHP, int indiceHFP, string periodo, string fechaInicio, int idLectura, int tipoInfoCodi)
        {
            //string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibre, fecha);
            string condicionSelect = string.Format("ME.H{0},ME.H{1}", indiceHP, indiceHFP);
            string condicionWhere = string.Format(" AND ME.MEDIFECHA = TO_DATE('{0}','DD/MM/YYYY')", fechaInicio);

            string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibreSicli, condicionSelect, periodo, condicionWhere);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "LECTCODI", DbType.Int32, idLectura);
            dbProvider.AddInParameter(command, "TIPOINFOCODI ", DbType.Int32, tipoInfoCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarDemandaUsuarioLibre(string fecha)
        {
            string queryString = string.Format(helper.SqlActualizarDemandaUsuarioLibre, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.ExecuteNonQuery(command);
        }

        public int ListAntiguedadDatos()
        {
            //string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibre, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarAntiguedadDatos);

            object result = dbProvider.ExecuteScalar(command);
            int meses = 0;
            if (result != null)
            {
                meses = Convert.ToInt32(result);
            }

            return meses;
        }

        public string ListUltimoPeriodo()
        {
            //string queryString = string.Format(helper.SqlCargarDemandaUsuarioLibre, fecha);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarUltimoPeriodo);

            object result = dbProvider.ExecuteScalar(command);
            string periodo = "";
            if (result != null)
            {
                periodo = Convert.ToString(result);
            }

            return periodo;
        }

        #region Reportes Rechazo Carga
        public List<RcaCuadroProgUsuarioDTO> ReporteTotalDatos(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteTotalDatos;

            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcproucargarechazarejec = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcproucargarechazarejec)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcproucargarechazarejec);

                    int iRccuadhoriniejec = dr.GetOrdinal(helper.Rccuadhoriniejec);
                    if (!dr.IsDBNull(iRccuadhoriniejec)) entity.Rccuadhoriniejec = dr.GetString(iRccuadhoriniejec);

                    int iRccuadhorfinejec = dr.GetOrdinal(helper.Rccuadhorfinejec);
                    if (!dr.IsDBNull(iRccuadhorfinejec)) entity.Rccuadhorfinejec = dr.GetString(iRccuadhorfinejec);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ReporteDemoraEjecucion(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteDemoraEjecucion;

            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    //int iRcproucargarechazarejec = dr.GetOrdinal(helper.Rcproucargarechazarejec);
                    //if (!dr.IsDBNull(iRcproucargarechazarejec)) entity.Rcproucargarechazarejec = dr.GetDecimal(iRcproucargarechazarejec);

                    int iRccuadhorinicoord = dr.GetOrdinal(helper.Rccuadhorinicoord);
                    if (!dr.IsDBNull(iRccuadhorinicoord)) entity.Rccuadhorinicoord = dr.GetString(iRccuadhorinicoord);

                    int iRccuadhoriniejec = dr.GetOrdinal(helper.Rccuadhoriniejec);
                    if (!dr.IsDBNull(iRccuadhoriniejec)) entity.Rccuadhoriniejec = dr.GetString(iRccuadhoriniejec);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ReporteDemoraReestablecimiento(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteDemoraReestablecimiento;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    //int iRcproucargarechazarejec = dr.GetOrdinal(helper.Rcproucargarechazarejec);
                    //if (!dr.IsDBNull(iRcproucargarechazarejec)) entity.Rcproucargarechazarejec = dr.GetDecimal(iRcproucargarechazarejec);

                    int iRccuadhorfincoord = dr.GetOrdinal(helper.Rccuadhorfincoord);
                    if (!dr.IsDBNull(iRccuadhorfincoord)) entity.Rccuadhorfincoord = dr.GetString(iRccuadhorfincoord);

                    int iRccuadhorfinejec = dr.GetOrdinal(helper.Rccuadhorfinejec);
                    if (!dr.IsDBNull(iRccuadhorfinejec)) entity.Rccuadhorfinejec = dr.GetString(iRccuadhorfinejec);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ReporteInterrupcionesMenores(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteInterrupcionesMenores;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicioRep = dr.GetOrdinal(helper.RcejeufechorinicioRep);
                    if (!dr.IsDBNull(iRcejeufechorinicioRep)) entity.RcejeufechorinicioRep = dr.GetString(iRcejeufechorinicioRep);

                    int iRcejeufechorfinRep = dr.GetOrdinal(helper.RcejeufechorfinRep);
                    if (!dr.IsDBNull(iRcejeufechorfinRep)) entity.RcejeufechorfinRep = dr.GetString(iRcejeufechorfinRep);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ReporteDemorasFinalizar(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteDemorasFinalizar;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    //int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    //if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicioRep = dr.GetOrdinal(helper.RcejeufechorinicioRep);
                    if (!dr.IsDBNull(iRcejeufechorinicioRep)) entity.RcejeufechorinicioRep = dr.GetString(iRcejeufechorinicioRep);

                    int iRcejeufechorfinRep = dr.GetOrdinal(helper.RcejeufechorfinRep);
                    if (!dr.IsDBNull(iRcejeufechorfinRep)) entity.RcejeufechorfinRep = dr.GetString(iRcejeufechorfinRep);

                    int iRccuadfechorfinRep = dr.GetOrdinal(helper.RccuadfechorfinRep);
                    if (!dr.IsDBNull(iRccuadfechorfinRep)) entity.RccuadfechorfinRep = dr.GetString(iRccuadfechorfinRep);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public List<RcaCuadroProgUsuarioDTO> ReporteDemorasResarcimiento(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteDemorasResarcimiento;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    //int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    //if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicioRep = dr.GetOrdinal(helper.RcejeufechorinicioRep);
                    if (!dr.IsDBNull(iRcejeufechorinicioRep)) entity.RcejeufechorinicioRep = dr.GetString(iRcejeufechorinicioRep);

                    int iRcejeufechorfinRep = dr.GetOrdinal(helper.RcejeufechorfinRep);
                    if (!dr.IsDBNull(iRcejeufechorfinRep)) entity.RcejeufechorfinRep = dr.GetString(iRcejeufechorfinRep);

                    int iRccuadfechorfinRep = dr.GetOrdinal(helper.RccuadfechorfinRep);
                    if (!dr.IsDBNull(iRccuadfechorfinRep)) entity.RccuadfechorfinRep = dr.GetString(iRccuadfechorfinRep);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        #endregion


        #region Reporte Evaluacion Cumplimiento

        public void DeleteTablasTemporalesReporteCumplimiento(string eventoCTAF)
        {

            string queryString = string.Format(helper.SqlDeleteRcaMedidorMinutoTmp.Trim(), eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlDeleteRcaEvaluacionTmp.Trim(), eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlDeleteRcaResulEvaluacionTmp.Trim(), eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }
        

        public List<RcaCuadroProgUsuarioDTO> ObtenerDatosEvaluacionCumplimiento(string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlObtenerDatosEvaluacionCumplimiento;
            //var sqlWhere = "";

            queryString = string.Format(queryString, eventoCTAF);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcejeufechorinicio = dr.GetOrdinal(helper.Rcejeufechorinicio);
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcejeufechorinicio = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal(helper.Rcejeufechorfin);
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void InsertarTablaTempEvaluacionCumplimiento(string eventoCTAF, int emprcodi, int equicodi, DateTime fechaInicioEjec, DateTime fechaFinEjec)
        {

            string queryString = string.Format(helper.SqlInsertarTablaTempEvaluacionCumplimiento, eventoCTAF, equicodi, emprcodi,
                fechaInicioEjec.ToString("dd/MM/yyyy HH:mm"), fechaFinEjec.ToString("dd/MM/yyyy HH:mm"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, "RCEJEDFECHORFIN", DbType.DateTime, fechaFinEjec);
            //dbProvider.AddInParameter(command, "RCEJEDFECHORINI", DbType.DateTime, fechaInicioEjec);

            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizacionCalculoPorMinutoEvaluacionCumplimiento(string eventoCTAF)
        {

            string queryString = string.Format(helper.SqlActualizacionCalculoPorMinutoEvaluacionCumplimiento, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void RegistrarIntervaloEvaluacionCumplimiento(string eventoCTAF)
        {

            string queryString = string.Format(helper.SqlRegistrarIntervaloEvaluacionCumplimiento, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<RcaEvaluacionTempoDTO> ObtenerValoresEvaluacionCliente(string eventoCTAF)
        {
            List<RcaEvaluacionTempoDTO> entities = new List<RcaEvaluacionTempoDTO>();

            string queryString = string.Format(helper.SqlObtenerValoresEvaluacionCliente, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEvaluacionTempoDTO entity = new RcaEvaluacionTempoDTO();

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcejeufechorinicio = dr.GetOrdinal("RCERMCFECHORINI");
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcermcfechorini = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal("RCERMCFECHORFIN");
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcermcfechorfin = dr.GetDateTime(iRcejeufechorfin);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void ActualizarEvaluacionCliente(string eventoCTAF, int grupo, int equicodi, DateTime fechorini, DateTime fechorfin)
        {
            string queryString = string.Format(helper.SqlActualizarEvaluacionCliente, eventoCTAF, grupo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, "RCERMCFECHORINI", DbType.DateTime, fechorini);
            dbProvider.AddInParameter(command, "RCERMCFECHORFIN", DbType.DateTime, fechorfin);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public List<RcaEvaluacionTempoDTO> ObtenerValoresGeneralesEvaluacionCumplimientoTmp(string eventoCTAF)
        {
            List<RcaEvaluacionTempoDTO> entities = new List<RcaEvaluacionTempoDTO>();

            string queryString = string.Format(helper.SqlObtenerValoresGeneralesEvaluacionCumplimiento, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEvaluacionTempoDTO entity = new RcaEvaluacionTempoDTO();

                    //int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcermcgrupo = dr.GetOrdinal("RCERMCGRUPO");
                    if (!dr.IsDBNull(iRcermcgrupo)) entity.Rcermcgrupo = Convert.ToInt32(dr.GetValue(iRcermcgrupo));

                    int iRcejeufechorinicio = dr.GetOrdinal("RCERMCFECHORINI");
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcermcfechorini = dr.GetDateTime(iRcejeufechorinicio);

                    //int iRcejeufechorfin = dr.GetOrdinal(helper.Rcejeufechorfin);
                    //if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcejeufechorfin = dr.GetDateTime(iRcejeufechorfin);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public decimal ObtenerValorPromedioEvaluacionCumplimiento(string eventoCTAF, int equicodi, DateTime fechaInicio)
        {
            var promedio = Decimal.Zero;
            string queryString = string.Format(helper.SqlObtenerValorPromedioEvaluacionCumplimiento, eventoCTAF, equicodi, fechaInicio.ToString("dd/MM/yyyy HH:mm"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, "RCERMCFECHORINI", DbType.DateTime, fechaInicio);
            //dbProvider.AddInParameter(command, "RCEJEDFECHORINI", DbType.DateTime, fechaInicioEjec);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                promedio = Convert.ToDecimal(result);
            }

            return promedio;
        }

        public void ActualizarValorPromedioEvaluacionCumplimiento(string eventoCTAF, int equicodi, decimal valorPromedio, int grupo)
        {

            string queryString = string.Format(helper.SqlActualizarValorPromedioEvaluacionCumplimiento, eventoCTAF, equicodi, valorPromedio, grupo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<RcaEvaluacionTempoDTO> ObtenerDatosCalculoFinalTempEvaluacionCumplimiento(string eventoCTAF)
        {
            List<RcaEvaluacionTempoDTO> entities = new List<RcaEvaluacionTempoDTO>();

            string queryString = string.Format(helper.SqlObtenerDatosCalculoFinalEvaluacionCumplimiento, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaEvaluacionTempoDTO entity = new RcaEvaluacionTempoDTO();

                    int iRcermccodeventoctaf = dr.GetOrdinal("RCERMCCODEVENTOCTAF");
                    if (!dr.IsDBNull(iRcermccodeventoctaf)) entity.Rcermccodeventoctaf = dr.GetString(iRcermccodeventoctaf);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iRcejeufechorinicio = dr.GetOrdinal("RCERMCFECHORINI");
                    if (!dr.IsDBNull(iRcejeufechorinicio)) entity.Rcermcfechorini = dr.GetDateTime(iRcejeufechorinicio);

                    int iRcejeufechorfin = dr.GetOrdinal("RCERMCFECHORFIN");
                    if (!dr.IsDBNull(iRcejeufechorfin)) entity.Rcermcfechorfin = dr.GetDateTime(iRcejeufechorfin);
                    // rcermcpotenciarechazar, rcermcpotencianorechazada, rcermcenergiadebiorechazar, rcermcpotenciapromprevia
                    int iRcermcpotenciarechazar = dr.GetOrdinal("RCERMCPOTENCIARECHAZAR");
                    if (!dr.IsDBNull(iRcermcpotenciarechazar)) entity.Rcermcpotenciarechazar = dr.GetDecimal(iRcermcpotenciarechazar);

                    int iRcermcpotencianorechazada = dr.GetOrdinal("RCERMCPOTENCIANORECHAZADA");
                    if (!dr.IsDBNull(iRcermcpotencianorechazada)) entity.Rcermcpotencianorechazada = dr.GetDecimal(iRcermcpotencianorechazada);

                    int iRcermcenergiadebiorechazar = dr.GetOrdinal("RCERMCENERGIADEBIORECHAZAR");
                    if (!dr.IsDBNull(iRcermcenergiadebiorechazar)) entity.Rcermcenergiadebiorechazar = dr.GetDecimal(iRcermcenergiadebiorechazar);

                    int iRcermcpotenciapromprevia = dr.GetOrdinal("RCERMCPOTENCIAPROMPREVIA");
                    if (!dr.IsDBNull(iRcermcpotenciapromprevia)) entity.Rcermcpotenciapromprevia = dr.GetDecimal(iRcermcpotenciapromprevia);


                    entities.Add(entity);
                }
            }

            return entities;
        }

        public void ActualizarDatosCalculoFinalEvaluacionCumplimiento(string eventoCTAF, decimal potenciaNoRechazada, decimal potenciaPrevia, 
            DateTime fechaInicioEjec, DateTime fechaFinEjec, int equicodi)
        {

            string queryString = string.Format(helper.SqlActualizarDatosCalculoFinalEvaluacionCumplimiento, eventoCTAF, potenciaPrevia, potenciaNoRechazada,
                fechaInicioEjec.ToString("dd/MM/yyyy HH:mm"), fechaFinEjec.ToString("dd/MM/yyyy HH:mm"));
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, "RCERMCFECHORINI", DbType.DateTime, fechaFinEjec);
            //dbProvider.AddInParameter(command, "RCERMCFECHORFIN", DbType.DateTime, fechaInicioEjec);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlActualizarDatosCalculoFinal2EvaluacionCumplimiento, eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void InsertarEvaluacionFinal(string eventoCTAF)
        {

            string queryString = string.Format(helper.SqlInsertarResultadoEvaluacionFinal, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarEvaluacionFinal(string eventoCTAF)
        {

            string queryString = string.Format(helper.SqlActualizarResultadoEvaluacionFinal, eventoCTAF);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlActualizarResultadoEvaluacionFinal2, eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlActualizarResultadoEvaluacionFinal3, eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlActualizarResultadoEvaluacionFinalPotenciaRechazada, eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            queryString = string.Format(helper.SqlActualizarResultadoEvaluacionFinalPotenciaRechazadaEvaluacion, eventoCTAF);
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);
           
        }

     
        public List<RcaCuadroProgUsuarioDTO> ReporteEvalucionCumplimientoRMC(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteEvaluacionCumplimiento;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    //int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    //if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcproucargarechazarcoord = dr.GetOrdinal(helper.Rcproucargarechazarcoord);
                    if (!dr.IsDBNull(iRcproucargarechazarcoord)) entity.Rcproucargarechazarcoord = dr.GetDecimal(iRcproucargarechazarcoord);

                    int iRccuadfechoriniRep = dr.GetOrdinal(helper.RccuadfechoriniRep);
                    if (!dr.IsDBNull(iRccuadfechoriniRep)) entity.RccuadfechoriniRep = dr.GetString(iRccuadfechoriniRep);

                    int iRccuadfechorfinRep = dr.GetOrdinal(helper.RccuadfechorfinRep);
                    if (!dr.IsDBNull(iRccuadfechorfinRep)) entity.RccuadfechorfinRep = dr.GetString(iRccuadfechorfinRep);

                    int iRcreevpotenciaprompreviaRep = dr.GetOrdinal(helper.RcreevpotenciaprompreviaRep);
                    if (!dr.IsDBNull(iRcreevpotenciaprompreviaRep)) entity.RcreevpotenciaprompreviaRep = dr.GetDecimal(iRcreevpotenciaprompreviaRep);

                    int iRccuadfechoriniPrevioRep = dr.GetOrdinal(helper.RccuadfechoriniPrevioRep);
                    if (!dr.IsDBNull(iRccuadfechoriniPrevioRep)) entity.RccuadfechoriniPrevioRep = dr.GetString(iRccuadfechoriniPrevioRep);

                    int iRccuadfechorfinPrevioRep = dr.GetOrdinal(helper.RccuadfechorfinPrevioRep);
                    if (!dr.IsDBNull(iRccuadfechorfinPrevioRep)) entity.RccuadfechorfinPrevioRep = dr.GetString(iRccuadfechorfinPrevioRep);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicioRep = dr.GetOrdinal(helper.RcejeufechorinicioRep);
                    if (!dr.IsDBNull(iRcejeufechorinicioRep)) entity.RcejeufechorinicioRep = dr.GetString(iRcejeufechorinicioRep);

                    int iRcejeufechorfinRep = dr.GetOrdinal(helper.RcejeufechorfinRep);
                    if (!dr.IsDBNull(iRcejeufechorfinRep)) entity.RcejeufechorfinRep = dr.GetString(iRcejeufechorfinRep);

                    int iEvaluacion = dr.GetOrdinal(helper.Evaluacion);
                    if (!dr.IsDBNull(iEvaluacion)) entity.Evaluacion = dr.GetString(iEvaluacion);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        #endregion

        
        public List<RcaCuadroProgUsuarioDTO> ReporteInterrrupInformeTecnico(int codigoCuadroPrograma, string eventoCTAF)
        {
            List<RcaCuadroProgUsuarioDTO> entities = new List<RcaCuadroProgUsuarioDTO>();

            string queryString = helper.SqlReporteInterrupInformeTecnico;
            var sqlWhere = "";
            if (codigoCuadroPrograma > 0)
            {
                sqlWhere = sqlWhere + " AND P.RCCUADCODI = " + codigoCuadroPrograma + " ";
            }
            queryString = string.Format(queryString, sqlWhere);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, "RCCUADCODEVENTOCTAF", DbType.String, eventoCTAF);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgUsuarioDTO entity = new RcaCuadroProgUsuarioDTO();

                    int iEmprrazsocial = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Empresa = dr.GetString(iEmprrazsocial);

                    int iAreanomb = dr.GetOrdinal(helper.Suministrador);
                    if (!dr.IsDBNull(iAreanomb)) entity.Suministrador = dr.GetString(iAreanomb);

                    int iSubestacion = dr.GetOrdinal(helper.Subestacion);
                    if (!dr.IsDBNull(iSubestacion)) entity.Subestacion = dr.GetString(iSubestacion);

                    int iRcproucargarechazarcoord = dr.GetOrdinal(helper.Rcproucargarechazarcoord);
                    if (!dr.IsDBNull(iRcproucargarechazarcoord)) entity.Rcproucargarechazarcoord = dr.GetDecimal(iRcproucargarechazarcoord);

                    int iRccuadfechoriniRep = dr.GetOrdinal(helper.RccuadfechoriniRep);
                    if (!dr.IsDBNull(iRccuadfechoriniRep)) entity.RccuadfechoriniRep = dr.GetString(iRccuadfechoriniRep);

                    int iRccuadfechorfinRep = dr.GetOrdinal(helper.RccuadfechorfinRep);
                    if (!dr.IsDBNull(iRccuadfechorfinRep)) entity.RccuadfechorfinRep = dr.GetString(iRccuadfechorfinRep);

                    //int iRcreevpotenciaprompreviaRep = dr.GetOrdinal(helper.RcreevpotenciaprompreviaRep);
                    //if (!dr.IsDBNull(iRcreevpotenciaprompreviaRep)) entity.RcreevpotenciaprompreviaRep = dr.GetDecimal(iRcreevpotenciaprompreviaRep);

                    //int iRccuadfechoriniPrevioRep = dr.GetOrdinal(helper.RccuadfechoriniPrevioRep);
                    //if (!dr.IsDBNull(iRccuadfechoriniPrevioRep)) entity.RccuadfechoriniPrevioRep = dr.GetString(iRccuadfechoriniPrevioRep);

                    //int iRccuadfechorfinPrevioRep = dr.GetOrdinal(helper.RccuadfechorfinPrevioRep);
                    //if (!dr.IsDBNull(iRccuadfechorfinPrevioRep)) entity.RccuadfechorfinPrevioRep = dr.GetString(iRccuadfechorfinPrevioRep);

                    int iRcejeucargarechazada = dr.GetOrdinal(helper.Rcejeucargarechazada);
                    if (!dr.IsDBNull(iRcejeucargarechazada)) entity.Rcejeucargarechazada = dr.GetDecimal(iRcejeucargarechazada);

                    int iRcejeufechorinicioRep = dr.GetOrdinal(helper.RcejeufechorinicioRep);
                    if (!dr.IsDBNull(iRcejeufechorinicioRep)) entity.RcejeufechorinicioRep = dr.GetString(iRcejeufechorinicioRep);

                    int iRcejeufechorfinRep = dr.GetOrdinal(helper.RcejeufechorfinRep);
                    if (!dr.IsDBNull(iRcejeufechorfinRep)) entity.RcejeufechorfinRep = dr.GetString(iRcejeufechorfinRep);

                    int iDuracion = dr.GetOrdinal(helper.Duracion);
                    if (!dr.IsDBNull(iDuracion)) entity.Duracion = dr.GetDecimal(iDuracion);



                    entities.Add(entity);
                }
            }

            return entities;
        }


    }
}
