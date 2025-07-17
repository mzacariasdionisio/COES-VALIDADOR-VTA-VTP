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
    public class CamFormatoD1ADet1Repository : RepositoryBase, ICamFormatoD1ADET1Repository
    {
        public CamFormatoD1ADet1Repository(string strConn) : base(strConn) { }

        CamFormatoD1ADet1Helper Helper = new CamFormatoD1ADet1Helper();

        public List<FormatoD1ADet1DTO> GetFormatoD1ADET1Codi(int formatoD1ACodi)
        {
            List<FormatoD1ADet1DTO> formatoD1ADet1DTOs = new List<FormatoD1ADet1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet1Codi);
            dbProvider.AddInParameter(command, "FORMATOD1ACODI", DbType.Int32, formatoD1ACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1ADet1DTO ob = new FormatoD1ADet1DTO();
                    ob.FormatoD1ADet1Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET1CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.DemandaCarga = !dr.IsDBNull(dr.GetOrdinal("DEMANDACARGA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDACARGA")) : null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    ob.DemandaNetaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHP")) : null;
                    ob.DemandaNetaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDANETAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDANETAHFP")) : null;
                    formatoD1ADet1DTOs.Add(ob);
                }
            }
            return formatoD1ADet1DTOs;
        }

        public bool SaveFormatoD1ADET1(FormatoD1ADet1DTO formatoD1ADet1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1ADet1);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ADET1CODI", DbType.Int32, formatoD1ADet1DTO.FormatoD1ADet1Codi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ACODI", DbType.Int32, formatoD1ADet1DTO.FormatoD1ACodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, formatoD1ADet1DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, formatoD1ADet1DTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, formatoD1ADet1DTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, formatoD1ADet1DTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "DEMANDACARGA", DbType.Decimal, formatoD1ADet1DTO.DemandaCarga);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, formatoD1ADet1DTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, formatoD1ADet1DTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, formatoD1ADet1DTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "DEMANDANETAHP", DbType.Decimal, formatoD1ADet1DTO.DemandaNetaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDANETAHFP", DbType.Decimal, formatoD1ADet1DTO.DemandaNetaHFP);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1ADet1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1ADET1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1ADet1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1ADET1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1ADet1Id);
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

        public FormatoD1ADet1DTO GetFormatoD1ADET1ById(int id)
        {
            FormatoD1ADet1DTO ob = new FormatoD1ADet1DTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1ADet1ById);
            dbProvider.AddInParameter(command, "FORMATOD1ADET1CODI", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1ADet1Codi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ADET1CODI")) : 0;
                    ob.FormatoD1ACodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1ACODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1ACODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    ob.DemandaCarga = !dr.IsDBNull(dr.GetOrdinal("DEMANDACARGA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDACARGA")) : null;
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

        public bool UpdateFormatoD1ADET1(FormatoD1ADet1DTO formatoD1ADet1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateFormatoD1ADet1);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ADET1CODI", DbType.Int32, formatoD1ADet1DTO.FormatoD1ADet1Codi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1ACODI", DbType.Int32, formatoD1ADet1DTO.FormatoD1ACodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, formatoD1ADet1DTO.Anio);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, formatoD1ADet1DTO.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, formatoD1ADet1DTO.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, formatoD1ADet1DTO.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "DEMANDACARGA", DbType.Decimal, formatoD1ADet1DTO.DemandaCarga);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, formatoD1ADet1DTO.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, formatoD1ADet1DTO.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, formatoD1ADet1DTO.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, formatoD1ADet1DTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1ADet1DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
    }

}
