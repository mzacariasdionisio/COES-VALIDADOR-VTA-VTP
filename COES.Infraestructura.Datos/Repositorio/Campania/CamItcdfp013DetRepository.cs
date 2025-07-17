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
    public class CamItcdfp013DetRepository : RepositoryBase, ICamItcdfp013DetRepository
    {
        public CamItcdfp013DetRepository(string strConn) : base(strConn) { }

        CamItcdfp013DetHelper Helper = new CamItcdfp013DetHelper();

        public List<Itcdfp013DetDTO> GetItcdfp013DetCodi(int itcdfp013Codi)
        {
            List<Itcdfp013DetDTO> itcdfp013DetDTOs = new List<Itcdfp013DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp013DetCodi);
            dbProvider.AddInParameter(command, "ITCDFP013CODI", DbType.Int32, itcdfp013Codi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdfp013DetDTO ob = new Itcdfp013DetDTO
                    {
                        Itcdfp013DetCodi = !dr.IsDBNull(dr.GetOrdinal("TCDFP013DETCODI")) ? dr.GetInt32(dr.GetOrdinal("TCDFP013DETCODI")) : 0,
                        Itcdfp013Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP013CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP013CODI")) : 0,
                        Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0,
                        Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null
                    };
                    itcdfp013DetDTOs.Add(ob);
                }
            }

            return itcdfp013DetDTOs;
        }

        public bool SaveItcdfp013Det(Itcdfp013DetDTO itcdfp013DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdfp013Det);
            dbProvider.AddInParameter(dbCommand, "TCDFP013DETCODI", DbType.Int32, itcdfp013DetDTO.Itcdfp013DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDFP013CODI", DbType.Int32, itcdfp013DetDTO.Itcdfp013Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdfp013DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdfp013DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdfp013DetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdfp013DetDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdfp013DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdfp013DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "TCDFP013DETCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdfp013DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdfp013DetId);
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

        public Itcdfp013DetDTO GetItcdfp013DetById(int id)
        {
            Itcdfp013DetDTO ob = new Itcdfp013DetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdfp013DetById);
            dbProvider.AddInParameter(commandHoja, "TCDFP013DETCODI", DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Itcdfp013DetCodi = !dr.IsDBNull(dr.GetOrdinal("TCDFP013DETCODI")) ? dr.GetInt32(dr.GetOrdinal("TCDFP013DETCODI")) : 0;
                    ob.Itcdfp013Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDFP013CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDFP013CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null;
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                }
            }

            return ob;
        }

        public bool UpdateItcdfp013Det(Itcdfp013DetDTO itcdfp013DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdfp013Det);
            dbProvider.AddInParameter(dbCommand, "TCDFP013DETCODI", DbType.Int32, itcdfp013DetDTO.Itcdfp013DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDFP013CODI", DbType.Int32, itcdfp013DetDTO.Itcdfp013Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdfp013DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdfp013DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdfp013DetDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdfp013DetDTO.FecModificacion);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }

}
