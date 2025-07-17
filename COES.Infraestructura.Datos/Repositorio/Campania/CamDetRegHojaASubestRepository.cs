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
    public class CamDetRegHojaASubestRepository: RepositoryBase, ICamDetRegHojaASubestRepository
    {

        public CamDetRegHojaASubestRepository(string strConn) : base(strConn){}

        CamDetRegHojaASubestHelper Helper = new CamDetRegHojaASubestHelper();

        public List<DetRegHojaASubestDTO> GetDetRegHojaASubestFichaCCodi(int fichaccodi)
        {
            List<DetRegHojaASubestDTO> detRegHojaASubestDTOs = new List<DetRegHojaASubestDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaASubestFichaCCodi);
            dbProvider.AddInParameter(command, "DATACATCODI", DbType.String, fichaccodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetRegHojaASubestDTO ob = new DetRegHojaASubestDTO();
                    ob.Detsubesthacodi = !dr.IsDBNull(dr.GetOrdinal("DETSUBESTHACODI")) ? dr.GetInt32(dr.GetOrdinal("DETSUBESTHACODI")) : 0;
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.numData = !dr.IsDBNull(dr.GetOrdinal("NUMDATA")) ? dr.GetInt32(dr.GetOrdinal("NUMDATA")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    detRegHojaASubestDTOs.Add(ob);
                }
            }

            return detRegHojaASubestDTOs;
        }

        public bool SaveDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDetRegHojaASubest);
            dbProvider.AddInParameter(dbCommand, "DETSUBESTHACODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaASubestDTO.Detsubesthacodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaASubestDTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaASubestDTO.Datacatcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ObtenerValorOrDefault(detRegHojaASubestDTO.Tipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "NUMDATA", DbType.Int32, ObtenerValorOrDefault(detRegHojaASubestDTO.numData, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(detRegHojaASubestDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(detRegHojaASubestDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDetRegHojaASubestById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetRegHojaASubestById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDetRegHojaASubestId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDetRegHojaASubestId);
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

        public DetRegHojaASubestDTO GetDetRegHojaASubestById(int id)
        {
            DetRegHojaASubestDTO ob = new DetRegHojaASubestDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaASubestById);
            dbProvider.AddInParameter(commandHoja, "DETSUBESTHACODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Detsubesthacodi = !dr.IsDBNull(dr.GetOrdinal("DETSUBESTHACODI")) ? dr.GetInt32(dr.GetOrdinal("DETSUBESTHACODI")) : 0;
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.numData = !dr.IsDBNull(dr.GetOrdinal("NUMDATA")) ? dr.GetInt32(dr.GetOrdinal("NUMDATA")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDetRegHojaASubest(DetRegHojaASubestDTO detRegHojaASubestDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDetRegHojaASubest);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, detRegHojaASubestDTO.Centralcodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaASubestDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, detRegHojaASubestDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "NUMDATA", DbType.Int32, detRegHojaASubestDTO.numData);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaASubestDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaASubestDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DETSUBESTHACODI", DbType.Int32, detRegHojaASubestDTO.Detsubesthacodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        object ObtenerValorOrDefault(object valor, Type tipo)
        {
            DateTime fechaMinimaValida = DateTime.Now;
            if (valor == null || (valor is DateTime && (DateTime)valor == DateTime.MinValue))
            {
                if (tipo == typeof(int) || tipo == typeof(int?))
                {
                    return 0;
                }
                else if (tipo == typeof(string))
                {
                    return "";
                }
                else if (tipo == typeof(DateTime) || tipo == typeof(DateTime?))
                {
                    return fechaMinimaValida;
                }
            }
            return valor;
        }

    }
}
