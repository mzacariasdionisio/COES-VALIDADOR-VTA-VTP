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
    public class CamFormatoD1CDetRepository : RepositoryBase, ICamFormatoD1CDETRepository
    {
        public CamFormatoD1CDetRepository(string strConn) : base(strConn) { }

        CamFormatoD1CDetHelper Helper = new CamFormatoD1CDetHelper();

        public List<FormatoD1CDetDTO> GetFormatoD1CDETCodi(int formatoD1CCodi)
        {
            List<FormatoD1CDetDTO> formatoD1CDetDTOs = new List<FormatoD1CDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1CDetCodi);
            dbProvider.AddInParameter(command, "FORMATOD1CCODI", DbType.Int32, formatoD1CCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    FormatoD1CDetDTO ob = new FormatoD1CDetDTO();
                    ob.FormatoD1CDetCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CDETCODI")) : 0;
                    ob.FormatoD1CCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.Demanda = !dr.IsDBNull(dr.GetOrdinal("DEMANDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDA")) : null;
                    ob.Generacion = !dr.IsDBNull(dr.GetOrdinal("GENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACION")) : null;
                    formatoD1CDetDTOs.Add(ob);
                }
            }
            return formatoD1CDetDTOs;
        }

        public bool SaveFormatoD1CDET(FormatoD1CDetDTO formatoD1CDetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveFormatoD1CDet);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1CDETCODI", DbType.Int32, formatoD1CDetDTO.FormatoD1CDetCodi);
            dbProvider.AddInParameter(dbCommand, "FORMATOD1CCODI", DbType.Int32, formatoD1CDetDTO.FormatoD1CCodi);
            dbProvider.AddInParameter(dbCommand, "HORA", DbType.String, formatoD1CDetDTO.Hora);
            dbProvider.AddInParameter(dbCommand, "DEMANDA", DbType.Decimal, formatoD1CDetDTO.Demanda);
            dbProvider.AddInParameter(dbCommand, "GENERACION", DbType.Decimal, formatoD1CDetDTO.Generacion);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, formatoD1CDetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, formatoD1CDetDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteFormatoD1CDETById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteFormatoD1CDetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastFormatoD1CDETId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastFormatoD1CDetId);
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

        public FormatoD1CDetDTO GetFormatoD1CDETById(int id)
        {
            FormatoD1CDetDTO ob = new FormatoD1CDetDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetFormatoD1CDetById);
            dbProvider.AddInParameter(command, "FORMATOD1CDETCODI", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob.FormatoD1CDetCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CDETCODI")) : 0;
                    ob.FormatoD1CCodi = !dr.IsDBNull(dr.GetOrdinal("FORMATOD1CCODI")) ? dr.GetInt32(dr.GetOrdinal("FORMATOD1CCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.Demanda = !dr.IsDBNull(dr.GetOrdinal("DEMANDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDA")) : null;
                    ob.Generacion = !dr.IsDBNull(dr.GetOrdinal("GENERACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACION")) : null;
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
