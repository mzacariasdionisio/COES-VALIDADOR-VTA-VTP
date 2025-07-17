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
    public class CamDetRegHojaEolCRepository : RepositoryBase, ICamDetRegHojaEolCRepository
    {
        public CamDetRegHojaEolCRepository(string strConn) : base(strConn) { }

        CamDetRegHojaEolCHelper Helper = new CamDetRegHojaEolCHelper();

        public List<DetRegHojaEolCDTO> GetDetRegHojaEolCCodi(int id)
        {
            List<DetRegHojaEolCDTO> DetRegHojaEolCDTOs = new List<DetRegHojaEolCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaEolCCodi);
            dbProvider.AddInParameter(command, "CENTRALCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetRegHojaEolCDTO ob = new DetRegHojaEolCDTO();
                    ob.DetEloCCodi = !dr.IsDBNull(dr.GetOrdinal("DETELOCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETELOCCODI")) : 0;
                    ob.Centralccodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    DetRegHojaEolCDTOs.Add(ob);
                }
            }

            return DetRegHojaEolCDTOs;
        }

        public bool SaveDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDetRegHojaEolC);
            dbProvider.AddInParameter(dbCommand, "DETELOCCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaEolCDTO.DetEloCCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALCCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaEolCDTO.Centralccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaEolCDTO.Datacatcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(detRegHojaEolCDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ObtenerValorOrDefault(detRegHojaEolCDTO.Trimestre, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(detRegHojaEolCDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(detRegHojaEolCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDetRegHojaEolCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDetRegHojaEolCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDetRegHojaEolCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDetRegHojaEolCId);
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

        public DetRegHojaEolCDTO GetDetRegHojaEolCById(int id)
        {
            DetRegHojaEolCDTO ob = new DetRegHojaEolCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDetRegHojaEolCById);
            dbProvider.AddInParameter(commandHoja, "DETELOCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.DetEloCCodi = !dr.IsDBNull(dr.GetOrdinal("DETELOCCODI")) ? dr.GetInt32(dr.GetOrdinal("DETELOCCODI")) : 0;
                    ob.Centralccodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDetRegHojaEolC(DetRegHojaEolCDTO detRegHojaEolCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDetRegHojaEolC);
            dbProvider.AddInParameter(dbCommand, "CENTRALCCODI", DbType.Int32, detRegHojaEolCDTO.Centralccodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaEolCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaEolCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, detRegHojaEolCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaEolCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaEolCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DETELOCCODI", DbType.Int32, detRegHojaEolCDTO.DetEloCCodi);
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
