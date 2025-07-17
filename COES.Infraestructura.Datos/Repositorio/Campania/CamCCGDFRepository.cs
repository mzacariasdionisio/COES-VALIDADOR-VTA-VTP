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
    public class CamCCGDFRepository : RepositoryBase, ICamCCGDFRepository
    {
        public CamCCGDFRepository(string strConn) : base(strConn) { }

        CamCCGDFHelper Helper = new CamCCGDFHelper();

        public List<CCGDFDTO> GetCamCCGDF(int proyCodi)
        {
            List<CCGDFDTO> ccgdfDTOs = new List<CCGDFDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDF);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CCGDFDTO ob = new CCGDFDTO();
                    ob.CcgdfCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDFCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDFCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ccgdfDTOs.Add(ob);
                }
            }
            return ccgdfDTOs;
        }

        public bool SaveCamCCGDF(CCGDFDTO ccgdfDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCamCCGDF);
            dbProvider.AddInParameter(dbCommand, "CCGDFCODI", DbType.Int32, ccgdfDTO.CcgdfCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdfDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ccgdfDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ccgdfDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ccgdfDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ccgdfDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ccgdfDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ccgdfDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, ccgdfDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCamCCGDFById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCamCCGDFById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastCamCCGDFCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCamCCGDFCodi);
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

        public List<CCGDFDTO> GetCamCCGDFById(int id)
        {
            List<CCGDFDTO> ccgdfDTOs = new List<CCGDFDTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetCamCCGDFById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read()) { 

                    CCGDFDTO ob = new CCGDFDTO();
                    ob.CcgdfCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDFCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDFCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ccgdfDTOs.Add(ob);
                }
            }
            return ccgdfDTOs;
        }

        public bool UpdateCamCCGDF(CCGDFDTO ccgdfDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCamCCGDF);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, ccgdfDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ccgdfDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ccgdfDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ccgdfDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ccgdfDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ccgdfDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, ccgdfDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, ccgdfDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "CCGDFCODI", DbType.Int32, ccgdfDTO.CcgdfCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public List<CCGDFDTO> GetCamCCGDFByFilter(string plancodi, string empresa, string estado)
        {
            List<CCGDFDTO> ccgdfDTOs = new List<CCGDFDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM  FROM CAM_CCGDF CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PLANCODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CCGDFCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {

                    CCGDFDTO ob = new CCGDFDTO();
                    ob.CcgdfCodi = !dr.IsDBNull(dr.GetOrdinal("CCGDFCODI")) ? dr.GetInt32(dr.GetOrdinal("CCGDFCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ccgdfDTOs.Add(ob);
                }
            }
            return ccgdfDTOs;
        }

    }
}
