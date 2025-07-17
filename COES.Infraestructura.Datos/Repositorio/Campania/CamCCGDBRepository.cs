using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public  class CamCCGDBRepository : RepositoryBase, ICamCCGDBRepository
    {
        public CamCCGDBRepository(string strConn) : base(strConn) { }

        CamCCGDBHelper Helper = new CamCCGDBHelper();

        public List<CCGDBDTO> GetCamCCGDB(int proyCodi)
        {
            List<CCGDBDTO> ccgdbDTOs = new List<CCGDBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDB);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDBDTO ob = new CCGDBDTO
                    {
                        CcgdbCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDBCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDBCODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0,
                        Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "",
                        DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null,
                        DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null,
                        DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null,
                        GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null,
                        GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null,
                        GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "",
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "",
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "",

                    };
                    ccgdbDTOs.Add(ob);
                }
            }

            return ccgdbDTOs;
        }
        public bool SaveCamCCGDB(CCGDBDTO ccgdbDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamCCGDB);
            dbProvider.AddInParameter(dbCommand, "CCGDBCODI", DbType.Int32, ccgdbDTO.CcgdbCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdbDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, ccgdbDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, ccgdbDTO.Mes);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, ccgdbDTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, ccgdbDTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, ccgdbDTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, ccgdbDTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, ccgdbDTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, ccgdbDTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgdbDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdbDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
        public bool DeleteCamCCGDBById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamCCGDBById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }
        public int GetLastCamCCGDBCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamCCGDBCodi);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                count = Convert.ToInt32(result) + 1;
            }
            else
            {
                count = 1;
            }
            return count;
        }
        public List<CCGDBDTO> GetCamCCGDBById(int id)
        {
            List<CCGDBDTO> lista = new List<CCGDBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDBById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDBDTO ob = new CCGDBDTO();
                    ob.CcgdbCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDBCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDBCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    lista.Add(ob);
                }
            }
            return lista;
        }
        public bool UpdateCamCCGDB(CCGDBDTO ccgdbDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamCCGDB);
            dbProvider.AddInParameter(dbCommand, "CCGDBCODI", DbType.Int32, ccgdbDTO.CcgdbCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdbDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, ccgdbDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, ccgdbDTO.Mes);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, ccgdbDTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, ccgdbDTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, ccgdbDTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, ccgdbDTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, ccgdbDTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, ccgdbDTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgdbDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ccgdbDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdbDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CCGDBDTO> GetCamCCGDAByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDBDTO> listob = new List<CCGDBDTO>();

            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM  FROM CAM_CCGDB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDBCODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDBDTO ob = new CCGDBDTO();
                    ob.CcgdbCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDBCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDBCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    listob.Add(ob);
                }
            }
            return listob;
        }

        public List<CCGDBDTO> GetCamCCGDBByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDBDTO> lista = new List<CCGDBDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CCGDB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDBCODI DESC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDBDTO ob = new CCGDBDTO();
                    ob.CcgdbCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDBCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDBCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    lista.Add(ob);
                }
            }
            return lista;
        }

        public List<CCGDCDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDCDTO> lista = new List<CCGDCDTO>();
            string query = $@"
            SELECT  TR.PERICODI, PL.CODEMPRESA, CGB.PROYCODI,'Base' AS ESCENARIO, CGB.ANIO, SUM(CGB.DEMANDAENERGIA) AS DEMANDAENERGIA, MAX(CGB.DEMANDAHP) AS DEMANDAHP, MAX(CGB.DEMANDAHFP) AS DEMANDAHFP, 
                SUM(CGB.GENERACIONENERGIA) AS GENERACIONENERGIA, MAX(CGB.GENERACIONHP) AS GENERACIONHP, MAX(CGB.GENERACIONHFP) AS GENERACIONHFP, TR.EMPRESANOM, TR.PROYNOMBRE  FROM CAM_CCGDB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa}) AND CGB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
                GROUP BY TR.PERICODI, CGB.PROYCODI, CGB.ANIO, TR.EMPRESANOM, PL.CODEMPRESA, TR.PROYNOMBRE         
            UNION ALL
            SELECT  TR.PERICODI, PL.CODEMPRESA, CGB.PROYCODI, 'Optimista' AS ESCENARIO, CGB.ANIO, SUM(CGB.DEMANDAENERGIA) AS DEMANDAENERGIA, MAX(CGB.DEMANDAHP) AS DEMANDAHP, MAX(CGB.DEMANDAHFP) AS DEMANDAHFP, 
                SUM(CGB.GENERACIONENERGIA) AS GENERACIONENERGIA, MAX(CGB.GENERACIONHP) AS GENERACIONHP, MAX(CGB.GENERACIONHFP) AS GENERACIONHFP, TR.EMPRESANOM, TR.PROYNOMBRE  FROM CAM_CCGDCOPT CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa}) AND CGB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
                GROUP BY TR.PERICODI, CGB.PROYCODI, CGB.ANIO, TR.EMPRESANOM, PL.CODEMPRESA, TR.PROYNOMBRE
            UNION ALL
            SELECT  TR.PERICODI, PL.CODEMPRESA, CGB.PROYCODI, 'Pesimista' as ESCENARIO, CGB.ANIO, SUM(CGB.DEMANDAENERGIA) AS DEMANDAENERGIA, MAX(CGB.DEMANDAHP) AS DEMANDAHP, MAX(CGB.DEMANDAHFP) AS DEMANDAHFP, 
                SUM(CGB.GENERACIONENERGIA) AS GENERACIONENERGIA, MAX(CGB.GENERACIONHP) AS GENERACIONHP, MAX(CGB.GENERACIONHFP) AS GENERACIONHFP, TR.EMPRESANOM, TR.PROYNOMBRE  FROM CAM_CCGDCPES CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa}) AND CGB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
                GROUP BY TR.PERICODI, CGB.PROYCODI, CGB.ANIO, TR.EMPRESANOM, PL.CODEMPRESA, TR.PROYNOMBRE
            ORDER BY PERICODI,CODEMPRESA,PROYCODI, ESCENARIO, ANIO ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDCDTO ob = new CCGDCDTO();
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Escenario = !dr.IsDBNull(dr.GetOrdinal("ESCENARIO")) ? dr.GetString(dr.GetOrdinal("ESCENARIO")) : string.Empty;
                    lista.Add(ob);
                }
            }
            return lista;
        }
    }
}
