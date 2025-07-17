using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCCGDCPesRepository : RepositoryBase, ICamCCGDCPesRepository
    {
        public CamCCGDCPesRepository(string strConn) : base(strConn) { }
       
        CamCCGDCPesHelper Helper = new CamCCGDCPesHelper();

        public List<CCGDCPesDTO> GetCamCCGDC(int proyCodi)
        {
            List<CCGDCPesDTO> ccgdcDTOs = new List<CCGDCPesDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDC);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDCPesDTO ob = new CCGDCPesDTO();
                    ob.CcgdcPesCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDCPESCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDCPESCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) :null;
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
                    ccgdcDTOs.Add(ob);
                }
            }
            return ccgdcDTOs;
        }
        public bool SaveCamCCGDC(CCGDCPesDTO ccgdcDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamCCGDC);
            dbProvider.AddInParameter(dbCommand, "CCGDCPESCODI", DbType.Int32, ccgdcDTO.CcgdcPesCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdcDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, ccgdcDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, ccgdcDTO.Mes);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, ccgdcDTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, ccgdcDTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, ccgdcDTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, ccgdcDTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, ccgdcDTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, ccgdcDTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgdcDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdcDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCamCCGDCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamCCGDCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }
        public int GetLastCamCCGDCCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamCCGDCCodi);
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
        public List<CCGDCPesDTO> GetCamCCGDCById(int id)
        {
            List<CCGDCPesDTO> lista = new List<CCGDCPesDTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDCById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    CCGDCPesDTO ob = new CCGDCPesDTO();
                    ob.CcgdcPesCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDCPESCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDCPESCODI")) : 0;
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
        public bool UpdateCamCCGDC(CCGDCPesDTO ccgdcDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamCCGDC);
            dbProvider.AddInParameter(dbCommand, "CCGDCPESCODI", DbType.Int32, ccgdcDTO.CcgdcPesCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdcDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, ccgdcDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, ccgdcDTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, ccgdcDTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, ccgdcDTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, ccgdcDTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, ccgdcDTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, ccgdcDTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgdcDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ccgdcDTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CCGDCPesDTO> GetCamCCGDCByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDCPesDTO> lista = new List<CCGDCPesDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  FROM CAM_CCGDCPES CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDCPESCODI ASC";
            
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
        

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    CCGDCPesDTO ob = new CCGDCPesDTO();
                    ob.CcgdcPesCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDCPESCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDCPESCODI")) : 0;
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
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                    ob.NombreProyecto = dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("PROYNOMBRE"));
                    lista.Add(ob);
                }
            }
            return lista;
        }
    }
}
