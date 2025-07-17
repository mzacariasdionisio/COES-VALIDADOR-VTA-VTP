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
    /// Clase de acceso a datos de la tabla RCA_CUADRO_PROG
    /// </summary>
    public class RcaCuadroProgRepository : RepositoryBase, IRcaCuadroProgRepository
    {
        public RcaCuadroProgRepository(string strConn)
            : base(strConn)
        {
        }

        RcaCuadroProgHelper helper = new RcaCuadroProgHelper();

        public int Save(RcaCuadroProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            var sentenciaInsert = string.Format(helper.SqlSave, entity.Rcestacodi);
            //command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            command = dbProvider.GetSqlStringCommand(sentenciaInsert);

            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, id);
            if (entity.Rcprogcodi > 0)
            {
                dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, entity.Rcprogcodi);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, DBNull.Value);
            }
            
            dbProvider.AddInParameter(command, helper.Rccuadenergiaracionar, DbType.Decimal, entity.Rccuadenergiaracionar);
            dbProvider.AddInParameter(command, helper.Rccuadumbral, DbType.Decimal, entity.Rccuadumbral);
            dbProvider.AddInParameter(command, helper.Rccuadmotivo, DbType.String, entity.Rccuadmotivo);
            dbProvider.AddInParameter(command, helper.Rccuadbloquehor, DbType.String, entity.Rccuadbloquehor);
            dbProvider.AddInParameter(command, helper.Rcconpcodi, DbType.Int32, entity.Rcconpcodi);
            dbProvider.AddInParameter(command, helper.Rccuadflageracmf, DbType.String, entity.Rccuadflageracmf);
            dbProvider.AddInParameter(command, helper.Rccuadflageracmt, DbType.String, entity.Rccuadflageracmt);
            dbProvider.AddInParameter(command, helper.Rccuadflagregulado, DbType.String, entity.Rccuadflagregulado);
            dbProvider.AddInParameter(command, helper.Rccuadfechorinicio, DbType.DateTime, entity.Rccuadfechorinicio);
            dbProvider.AddInParameter(command, helper.Rccuadfechorfin, DbType.DateTime, entity.Rccuadfechorfin);
            dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, entity.Rccuadestregistro);
            dbProvider.AddInParameter(command, helper.Rccuadubicacion, DbType.String, entity.Rccuadubicacion);
            dbProvider.AddInParameter(command, helper.Rccuadusucreacion, DbType.String, entity.Rccuadusucreacion);
            dbProvider.AddInParameter(command, helper.Rccuadfeccreacion, DbType.DateTime, entity.Rccuadfeccreacion);
            //dbProvider.AddInParameter(command, helper.Rcestacodi, DbType.Int32, entity.Rcestacodi);
            //dbProvider.AddInParameter(command, helper.Rccuadestado, DbType.String, entity.Rccuadestado);
            //dbProvider.AddInParameter(command, helper.Rccuadfecmodificacion, DbType.DateTime, entity.Rccuadfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(RcaCuadroProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            if (entity.Rcprogcodi > 0)
            {
                dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, entity.Rcprogcodi);
            }
            else
            {
                dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, DBNull.Value);
            }
            dbProvider.AddInParameter(command, helper.Rccuadenergiaracionar, DbType.Decimal, entity.Rccuadenergiaracionar);
            dbProvider.AddInParameter(command, helper.Rccuadumbral, DbType.Decimal, entity.Rccuadumbral);
            dbProvider.AddInParameter(command, helper.Rccuadmotivo, DbType.String, entity.Rccuadmotivo);
            dbProvider.AddInParameter(command, helper.Rccuadbloquehor, DbType.String, entity.Rccuadbloquehor);
            dbProvider.AddInParameter(command, helper.Rcconpcodi, DbType.Int32, entity.Rcconpcodi);
            dbProvider.AddInParameter(command, helper.Rccuadflageracmf, DbType.String, entity.Rccuadflageracmf);
            dbProvider.AddInParameter(command, helper.Rccuadflageracmt, DbType.String, entity.Rccuadflageracmt);
            dbProvider.AddInParameter(command, helper.Rccuadflagregulado, DbType.String, entity.Rccuadflagregulado);
            dbProvider.AddInParameter(command, helper.Rccuadfechorinicio, DbType.DateTime, entity.Rccuadfechorinicio);
            dbProvider.AddInParameter(command, helper.Rccuadfechorfin, DbType.DateTime, entity.Rccuadfechorfin);
            dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, entity.Rccuadestregistro);
            dbProvider.AddInParameter(command, helper.Rccuadubicacion, DbType.String, entity.Rccuadubicacion);
            //dbProvider.AddInParameter(command, helper.Rcestacodi, DbType.Int32, entity.Rcestacodi);
            //dbProvider.AddInParameter(command, helper.Rccuadusucreacion, DbType.String, entity.Rccuadusucreacion);
            //dbProvider.AddInParameter(command, helper.Rccuadfeccreacion, DbType.DateTime, entity.Rccuadfeccreacion);
            dbProvider.AddInParameter(command, helper.Rccuadusumodificacion, DbType.String, entity.Rccuadusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadfecmodificacion, DbType.DateTime, entity.Rccuadfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateCuadroEstado(RcaCuadroProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCuadroEstado);
                        
            dbProvider.AddInParameter(command, helper.Rcestacodi, DbType.Int32, entity.Rcestacodi);
            //dbProvider.AddInParameter(command, helper.Rccuadusucreacion, DbType.String, entity.Rccuadusucreacion);
            //dbProvider.AddInParameter(command, helper.Rccuadfeccreacion, DbType.DateTime, entity.Rccuadfeccreacion);
            dbProvider.AddInParameter(command, helper.Rccuadusumodificacion, DbType.String, entity.Rccuadusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadfecmodificacion, DbType.DateTime, entity.Rccuadfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);

            dbProvider.ExecuteNonQuery(command);
        }
        
        public void Delete(int rccuadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, rccuadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public RcaCuadroProgDTO GetById(int rccuadcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, rccuadcodi);
            RcaCuadroProgDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<RcaCuadroProgDTO> List()
        {
            List<RcaCuadroProgDTO> entitys = new List<RcaCuadroProgDTO>();
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

        public List<RcaCuadroProgDTO> GetByCriteria(string programa, string estadoCuadro)
        {
            List<RcaCuadroProgDTO> entitys = new List<RcaCuadroProgDTO>();
            string condicion = " WHERE RCCUADESTREGISTRO = '1' ";
            if (!string.IsNullOrEmpty(programa))
            {
                condicion += " AND RCPROGCODI = " + programa.ToString();
            }

            if (!string.IsNullOrEmpty(estadoCuadro))
            {
                condicion += " AND RCESTACODI = " + estadoCuadro;
            }
            //var condicionFinal = string.IsNullOrEmpty(condicion) ? string.Empty : " WHERE " + condicion;
            string queryString = string.Format(helper.SqlGetByCriteria, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<RcaCuadroProgDTO> ListRcaCuadroProgFiltro(string horizonte, string configuracion, string estado, string fecIni, string fecFin, string energiaRechazadaInicio, 
            string energiaRechazadaFin, int sinPrograma)
        {
            List<RcaCuadroProgDTO> entitys = new List<RcaCuadroProgDTO>();
            string condicion = "";

            if (!string.IsNullOrEmpty(horizonte))
            {
                condicion = condicion + " AND PROG.RCHORPCODI = " + horizonte.ToString();
            }

            if (!string.IsNullOrEmpty(configuracion))
            {
                condicion = condicion + " AND CPROG.RCCONPCODI = " + configuracion;
            }

            if (!string.IsNullOrEmpty(estado))
            {
                condicion = condicion + " AND CPROG.RCESTACODI  = " + estado; 
            }

            if (!string.IsNullOrEmpty(energiaRechazadaInicio))
            {
                condicion = condicion + " AND RCCUADENERGIARACIONAR >= " + energiaRechazadaInicio;
            }

            if (!string.IsNullOrEmpty(energiaRechazadaFin))
            {
                condicion = condicion + " AND RCCUADENERGIARACIONAR <= " + energiaRechazadaFin;
            }

            if (fecIni != null && !fecIni.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCCUADFECHORINICIO) >= TO_DATE('" + fecIni + "' , 'DD/MM/YYYY') ";
            }

            if (fecFin != null && !fecFin.Equals(""))
            {
                condicion = condicion + " AND TRUNC(RCCUADFECHORINICIO) <= TO_DATE('" + fecFin + "' , 'DD/MM/YYYY') ";
            }

            if(sinPrograma > 0)
            {
                condicion = condicion + " AND CPROG.RCPROGCODI IS NULL ";

            }

            string queryString = string.Format(helper.SqlListCuadroProgFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, "1");

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroProgDTO entity = new RcaCuadroProgDTO();

                    int iRccuadcodi = dr.GetOrdinal(helper.Rccuadcodi);
                    if (!dr.IsDBNull(iRccuadcodi)) entity.Rccuadcodi = Convert.ToInt32(dr.GetValue(iRccuadcodi));

                    int iRcprogcodi = dr.GetOrdinal(helper.Rcprogcodi);
                    if (!dr.IsDBNull(iRcprogcodi)) entity.Rcprogcodi = Convert.ToInt32(dr.GetValue(iRcprogcodi));

                    int iRccuadenergiaracionar = dr.GetOrdinal(helper.Rccuadenergiaracionar);
                    if (!dr.IsDBNull(iRccuadenergiaracionar)) entity.Rccuadenergiaracionar = dr.GetDecimal(iRccuadenergiaracionar);

                    int iRccuadmotivo = dr.GetOrdinal(helper.Rccuadmotivo);
                    if (!dr.IsDBNull(iRccuadmotivo)) entity.Rccuadmotivo = dr.GetString(iRccuadmotivo);

                    int iRccuadbloquehor = dr.GetOrdinal(helper.Rccuadbloquehor);
                    if (!dr.IsDBNull(iRccuadbloquehor)) entity.Rccuadbloquehor = dr.GetString(iRccuadbloquehor);

                    int iRcconpcodi = dr.GetOrdinal(helper.Rcconpcodi);
                    if (!dr.IsDBNull(iRcconpcodi)) entity.Rcconpcodi = dr.GetInt32(iRcconpcodi);

                    int iRccuadflageracm = dr.GetOrdinal(helper.Rccuadflageracmf);
                    if (!dr.IsDBNull(iRccuadflageracm)) entity.Rccuadflageracmf = dr.GetString(iRccuadflageracm);

                    int iRccuadflagregulado = dr.GetOrdinal(helper.Rccuadflagregulado);
                    if (!dr.IsDBNull(iRccuadflagregulado)) entity.Rccuadflagregulado = dr.GetString(iRccuadflagregulado);

                    int iRccuadfechorinicio = dr.GetOrdinal(helper.Rccuadfechorinicio);
                    if (!dr.IsDBNull(iRccuadfechorinicio)) entity.Rccuadfechorinicio = dr.GetDateTime(iRccuadfechorinicio);

                    int iRccuadfechorfin = dr.GetOrdinal(helper.Rccuadfechorfin);
                    if (!dr.IsDBNull(iRccuadfechorfin)) entity.Rccuadfechorfin = dr.GetDateTime(iRccuadfechorfin);

                    int iRchorpcodi = dr.GetOrdinal(helper.Rchorpcodi);
                    if (!dr.IsDBNull(iRchorpcodi)) entity.Rchorpcodi = dr.GetInt32(iRchorpcodi);

                    int iRcprognombre = dr.GetOrdinal(helper.Rcprognombre);
                    if (!dr.IsDBNull(iRcprognombre)) entity.Rcprognombre = dr.GetString(iRcprognombre);

                    int iRcconpnombre = dr.GetOrdinal(helper.Rcconpnombre);
                    if (!dr.IsDBNull(iRcconpnombre)) entity.Rcconpnombre = dr.GetString(iRcconpnombre);

                    int iRchorpnombre = dr.GetOrdinal(helper.Rchorpnombre);
                    if (!dr.IsDBNull(iRchorpnombre)) entity.Rchorpnombre = dr.GetString(iRchorpnombre);

                    int iRccuadubicacion = dr.GetOrdinal(helper.Rccuadubicacion);
                    if (!dr.IsDBNull(iRccuadubicacion)) entity.Rccuadubicacion = dr.GetString(iRccuadubicacion);

                    int iRcestacodi = dr.GetOrdinal(helper.Rcestacodi);
                    if (!dr.IsDBNull(iRcestacodi)) entity.Rcestacodi = dr.GetInt32(iRcestacodi);

                    int iRcestanombre = dr.GetOrdinal(helper.Rcestanombre);
                    if (!dr.IsDBNull(iRcestanombre)) entity.Rcestanombre = dr.GetString(iRcestanombre);

                    int iRcprogabrev = dr.GetOrdinal(helper.Rcprogabrev);
                    if (!dr.IsDBNull(iRcprogabrev)) entity.Rcprogabrev = dr.GetString(iRcprogabrev); 

                    int iRccuadcodeventoctaf = dr.GetOrdinal(helper.Rccuadcodeventoctaf);
                    if (!dr.IsDBNull(iRccuadcodeventoctaf)) entity.Rccuadcodeventoctaf = dr.GetString(iRccuadcodeventoctaf); 

                    int iRccuadusucreacion = dr.GetOrdinal(helper.Rccuadusucreacion);
                    if (!dr.IsDBNull(iRccuadusucreacion)) entity.Rccuadusucreacion = dr.GetString(iRccuadusucreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaHorizonteProgDTO> ListHorizonteProg()
        {
            List<RcaHorizonteProgDTO> entitys = new List<RcaHorizonteProgDTO>();
            //string queryString = string.Format(helper.SqlListCuadroProgFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListHorizontePrograma);
            //dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, "1");

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaHorizonteProgDTO entity = new RcaHorizonteProgDTO();

                    int iRchorpcodi = dr.GetOrdinal(helper.Rchorpcodi);
                    if (!dr.IsDBNull(iRchorpcodi)) entity.Rchorpcodi = Convert.ToInt32(dr.GetValue(iRchorpcodi));

                    int iRchorpnombre = dr.GetOrdinal(helper.Rchorpnombre);
                    if (!dr.IsDBNull(iRchorpnombre)) entity.Rchorpnombre = dr.GetString(iRchorpnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaConfiguracionProgDTO> ListConfiguracionProg()
        {
            List<RcaConfiguracionProgDTO> entitys = new List<RcaConfiguracionProgDTO>();
            //string queryString = string.Format(helper.SqlListCuadroProgFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListConfiguracionPrograma);
            //dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, "1");

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaConfiguracionProgDTO entity = new RcaConfiguracionProgDTO();

                    int iRcconpcodi = dr.GetOrdinal(helper.Rcconpcodi);
                    if (!dr.IsDBNull(iRcconpcodi)) entity.Rcconpcodi = Convert.ToInt32(dr.GetValue(iRcconpcodi));

                    int iRcconpnombre = dr.GetOrdinal(helper.Rcconpnombre);
                    if (!dr.IsDBNull(iRcconpnombre)) entity.Rcconpnombre = dr.GetString(iRcconpnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<RcaCuadroProgDTO> ListCuadroEnvioArchivoPorPrograma(int rcprogcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCuadroEnvioArchivoPorPrograma);

            dbProvider.AddInParameter(command, helper.Rcprogcodi, DbType.Int32, rcprogcodi);
            List<RcaCuadroProgDTO> entitys = new List<RcaCuadroProgDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new RcaCuadroProgDTO();
                    int iRccuadcodi = dr.GetOrdinal(helper.Rccuadcodi);
                    if (!dr.IsDBNull(iRccuadcodi)) entity.Rccuadcodi = Convert.ToInt32(dr.GetValue(iRccuadcodi));                    

                    int iRccuadmotivo = dr.GetOrdinal(helper.Rccuadmotivo);
                    if (!dr.IsDBNull(iRccuadmotivo)) entity.Rccuadmotivo = dr.GetString(iRccuadmotivo);

                    entitys.Add(entity);

                }
            }

            return entitys;
        }

        public void UpdateCuadroProgramaEjecucion(RcaCuadroProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCuadroProgramaEjecucion);
                        
            dbProvider.AddInParameter(command, helper.Rccuadfechorinicio, DbType.DateTime, entity.Rccuadfechorinicioejec);
            dbProvider.AddInParameter(command, helper.Rccuadfechorfin, DbType.DateTime, entity.Rccuadfechorfinejec);            
            //dbProvider.AddInParameter(command, helper.Rccuadestado, DbType.String, entity.Rccuadestado);
            dbProvider.AddInParameter(command, helper.Rcestacodi, DbType.Int32, entity.Rcestacodi);      
            dbProvider.AddInParameter(command, helper.Rccuadusumodificacion, DbType.String, entity.Rccuadusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadfecmodificacion, DbType.DateTime, entity.Rccuadfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<RcaCuadroEstadoDTO> ListEstadoCuadroProg()
        {
            List<RcaCuadroEstadoDTO> entitys = new List<RcaCuadroEstadoDTO>();
            //string queryString = string.Format(helper.SqlListCuadroProgFiltro, condicion);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEstadoCuadroPrograma);
            //dbProvider.AddInParameter(command, helper.Rccuadestregistro, DbType.String, "1");

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RcaCuadroEstadoDTO entity = new RcaCuadroEstadoDTO();

                    int iRcestacodi = dr.GetOrdinal(helper.Rcestacodi);
                    if (!dr.IsDBNull(iRcestacodi)) entity.Rcestacodi = Convert.ToInt32(dr.GetValue(iRcestacodi));

                    int iRcestanombre = dr.GetOrdinal(helper.Rcestanombre);
                    if (!dr.IsDBNull(iRcestanombre)) entity.Rcestanombre = dr.GetString(iRcestanombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateCuadroEvento(RcaCuadroProgDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateCuadroEvento);

            dbProvider.AddInParameter(command, helper.Rccuadcodeventoctaf, DbType.String, entity.Rccuadcodeventoctaf);
            //dbProvider.AddInParameter(command, helper.Rccuadusucreacion, DbType.String, entity.Rccuadusucreacion);
            //dbProvider.AddInParameter(command, helper.Rccuadfeccreacion, DbType.DateTime, entity.Rccuadfeccreacion);
            dbProvider.AddInParameter(command, helper.Rccuadusumodificacion, DbType.String, entity.Rccuadusumodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadfecmodificacion, DbType.DateTime, entity.Rccuadfecmodificacion);
            dbProvider.AddInParameter(command, helper.Rccuadcodi, DbType.Int32, entity.Rccuadcodi);

            dbProvider.ExecuteNonQuery(command);
        }
    }
}
