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
    public class CamItcdf121DetRepository : RepositoryBase, ICamItcdf121DetRepository
    {
        public CamItcdf121DetRepository(string strConn) : base(strConn) { }

        CamItcdf121DetHelper Helper = new CamItcdf121DetHelper();

        public List<Itcdf121DetDTO> GetItcdf121DetCodi(int itcdf121Codi)
        {
            List<Itcdf121DetDTO> itcdf121DetDTOs = new List<Itcdf121DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf121DetCodi);
            dbProvider.AddInParameter(command, "ITCDF121CODI", DbType.Int32, itcdf121Codi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf121DetDTO ob = new Itcdf121DetDTO();
                    ob.Itcdf121DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF121DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF121DETCODI")) : 0;
                    ob.Itcdf121Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF121CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF121CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("VALOR")) : null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    itcdf121DetDTOs.Add(ob);
                }
            }

            return itcdf121DetDTOs;
        }

        public bool SaveItcdf121Det(Itcdf121DetDTO itcdf121DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf121Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF121DETCODI", DbType.Int32, itcdf121DetDTO.Itcdf121DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDF121CODI", DbType.Int32, itcdf121DetDTO.Itcdf121Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf121DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf121DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf121DetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf121DetDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteItcdf121DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf121DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf121DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf121DetId);
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

        public Itcdf121DetDTO GetItcdf121DetById(int id)
        {
            Itcdf121DetDTO ob = new Itcdf121DetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf121DetById);
            dbProvider.AddInParameter(commandHoja, "ITCDF121DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Itcdf121DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF121DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF121DETCODI")) : 0;
                    ob.Itcdf121Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF121CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF121CODI")) : 0;
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

        public bool UpdateItcdf121Det(Itcdf121DetDTO itcdf121DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf121Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF121CODI", DbType.Int32, itcdf121DetDTO.Itcdf121Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf121DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf121DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf121DetDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf121DetDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF121DETCODI", DbType.Int32, itcdf121DetDTO.Itcdf121DetCodi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }

}