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
    public class CamItcdf110DetRepository : RepositoryBase, ICamItcdf110DetRepository
    {
        public CamItcdf110DetRepository(string strConn) : base(strConn) { }

        CamItcdf110DetHelper Helper = new CamItcdf110DetHelper();

        public List<Itcdf110DetDTO> GetItcdf110DetCodi(int itcdf110Codi)
        {
            List<Itcdf110DetDTO> itcdf110DetDTOs = new List<Itcdf110DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf110DetCodi);
            dbProvider.AddInParameter(command, "ITCDF110CODI", DbType.Int32, itcdf110Codi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf110DetDTO ob = new Itcdf110DetDTO();
                    ob.Itcdf110DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110DETCODI")) : 0;
                    ob.Itcdf110Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    itcdf110DetDTOs.Add(ob);
                }
            }

            return itcdf110DetDTOs;
        }

        public bool SaveItcdf110Det(Itcdf110DetDTO itcdf110DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf110Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF110DETCODI", DbType.Int32, itcdf110DetDTO.Itcdf110DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDF110CODI", DbType.Int32, itcdf110DetDTO.Itcdf110Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf110DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf110DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf110DetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf110DetDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf110DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf110DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf110DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf110DetId);
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

        public Itcdf110DetDTO GetItcdf110DetById(int id)
        {
            Itcdf110DetDTO ob = new Itcdf110DetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf110DetById);
            dbProvider.AddInParameter(commandHoja, "ITCDF110DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Itcdf110DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110DETCODI")) : 0;
                    ob.Itcdf110Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF110CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF110CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }

            return ob;
        }

        public bool UpdateItcdf110Det(Itcdf110DetDTO itcdf110DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf110Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF110CODI", DbType.Int32, itcdf110DetDTO.Itcdf110Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf110DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf110DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf110DetDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf110DetDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF110DETCODI", DbType.Int32, itcdf110DetDTO.Itcdf110DetCodi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }

}