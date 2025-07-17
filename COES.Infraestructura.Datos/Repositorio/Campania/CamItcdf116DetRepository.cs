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
    public class CamItcdf116DetRepository : RepositoryBase, ICamItcdf116DetRepository
    {
        public CamItcdf116DetRepository(string strConn) : base(strConn) { }

        CamItcdf116DetHelper Helper = new CamItcdf116DetHelper();

        public List<Itcdf116DetDTO> GetItcdf116DetCodi(int itcdf116Codi)
        {
            List<Itcdf116DetDTO> itcdf116DetDTOs = new List<Itcdf116DetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf116DetCodi);
            dbProvider.AddInParameter(command, "ITCDF116CODI", DbType.Int32, itcdf116Codi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Itcdf116DetDTO ob = new Itcdf116DetDTO();
                    ob.Itcdf116DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF116DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF116DETCODI")) : 0;
                    ob.Itcdf116Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF116CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF116CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetDecimal(dr.GetOrdinal("VALOR")) : 0;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    itcdf116DetDTOs.Add(ob);
                }
            }

            return itcdf116DetDTOs;
        }

        public bool SaveItcdf116Det(Itcdf116DetDTO itcdf116DetDTO)
        {
            // Validar que el valor no sea null antes de guardar
            if (itcdf116DetDTO.Valor == null)
            {
                return false; // No guarda el detalle si el valor es null
            }

            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveItcdf116Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF116DETCODI", DbType.Int32, itcdf116DetDTO.Itcdf116DetCodi);
            dbProvider.AddInParameter(dbCommand, "ITCDF116CODI", DbType.Int32, itcdf116DetDTO.Itcdf116Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf116DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf116DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, itcdf116DetDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, itcdf116DetDTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public bool DeleteItcdf116DetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteItcdf116DetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastItcdf116DetId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastItcdf116DetId);
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

        public Itcdf116DetDTO GetItcdf116DetById(int id)
        {
            Itcdf116DetDTO ob = new Itcdf116DetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetItcdf116DetById);
            dbProvider.AddInParameter(commandHoja, "ITCDF116DETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Itcdf116DetCodi = !dr.IsDBNull(dr.GetOrdinal("ITCDF116DETCODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF116DETCODI")) : 0;
                    ob.Itcdf116Codi = !dr.IsDBNull(dr.GetOrdinal("ITCDF116CODI")) ? dr.GetInt32(dr.GetOrdinal("ITCDF116CODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetInt32(dr.GetOrdinal("ANIO")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetDecimal(dr.GetOrdinal("VALOR")) : 0;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                }
            }

            return ob;
        }

        public bool UpdateItcdf116Det(Itcdf116DetDTO itcdf116DetDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateItcdf116Det);
            dbProvider.AddInParameter(dbCommand, "ITCDF116CODI", DbType.Int32, itcdf116DetDTO.Itcdf116Codi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, itcdf116DetDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.Decimal, itcdf116DetDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, itcdf116DetDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, itcdf116DetDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "ITCDF116DETCODI", DbType.Int32, itcdf116DetDTO.Itcdf116DetCodi);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }
    }

}