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
    public class CamFormatoD1BDetRepository : RepositoryBase, ICamFormatoD1BDETRepository
    {
        public CamFormatoD1BDetRepository(string strConn) : base(strConn) { }

        CamFormatoD1BDetHelper Helper = new CamFormatoD1BDetHelper();

        public List<FormatoD1BDetDTO> GetFormatoD1BDETCodi(int formatoD1BCodi)
        {
            List<FormatoD1BDetDTO> formatoD1BDetDTOs = new List<FormatoD1BDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1BDetCodi);
            dbProvider.AddInParameter(command, "FORMATOD1BCODI", DbType.Int32, formatoD1BCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1BDetDTO ob = new FormatoD1BDetDTO();
                    ob.FormatoD1BDetCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BDETCODI")) : 0;
                    ob.FormatoD1BCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.DemandaNetaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHP")) : null;
                    ob.DemandaNetaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHFP")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    formatoD1BDetDTOs.Add(ob);
                }
            }
            return formatoD1BDetDTOs;
        }

        public bool SaveFormatoD1BDET(FormatoD1BDetDTO formatoD1BDetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1BDet);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1BDETCODI", DbType.Int32, formatoD1BDetDTO.FormatoD1BDetCodi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1BCODI", DbType.Int32, formatoD1BDetDTO.FormatoD1BCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, formatoD1BDetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, formatoD1BDetDTO.Mes);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, formatoD1BDetDTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, formatoD1BDetDTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, formatoD1BDetDTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, formatoD1BDetDTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, formatoD1BDetDTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, formatoD1BDetDTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "DEMANDANETAHP", DbType.Decimal, formatoD1BDetDTO.DemandaNetaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDANETAHFP", DbType.Decimal, formatoD1BDetDTO.DemandaNetaHFP);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1BDetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, formatoD1BDetDTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1BDetDTO.IndDel); 

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1BDETById(int formatoD1BDetCodi, string usuario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1BDetById);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "PROYOCI", DbType.Int32, formatoD1BDetCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public int GetLastFormatoD1BDETId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1BDetId);
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
        public FormatoD1BDetDTO GetFormatoD1BDETById(int id)
        {
            FormatoD1BDetDTO ob = new FormatoD1BDetDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1BDetById);
            dbProvider.AddInParameter(command, "FORMATOD1BDETCODI", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1BDetCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BDETCODI")) : 0;
                    ob.FormatoD1BCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1BCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1BCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.DemandaNetaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHP")) : null;
                    ob.DemandaNetaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHFP")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }
            return ob;
        }

    }
}
