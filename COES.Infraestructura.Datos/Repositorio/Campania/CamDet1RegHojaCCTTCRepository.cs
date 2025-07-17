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
    public class CamDet1RegHojaCCTTCRepository: RepositoryBase, ICamDet1RegHojaCCTTCRepository
    {

        public CamDet1RegHojaCCTTCRepository(string strConn) : base(strConn){}

        CamDet1RegHojaCCTTCHelper Helper = new CamDet1RegHojaCCTTCHelper();

        public List<Det1RegHojaCCTTCDTO> GetDet1RegHojaCCTTCCentralCodi(int fichaccodi)
        {
            List<Det1RegHojaCCTTCDTO> Det1RegHojaCCTTCDTOs = new List<Det1RegHojaCCTTCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDet1RegHojaCCTTCCodi);
            dbProvider.AddInParameter(command, "CENTRALCODI", DbType.String, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Det1RegHojaCCTTCDTO ob = new Det1RegHojaCCTTCDTO();
                    ob.Det1centermhccodi = !dr.IsDBNull(dr.GetOrdinal("DET1CENTERMHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DET1CENTERMHCCODI")) : 0;
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    Det1RegHojaCCTTCDTOs.Add(ob);
                }
            }

            return Det1RegHojaCCTTCDTOs;
        }

        public bool SaveDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO Det1RegHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDet1RegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "DET1CENTERMHCCODI", DbType.Int32, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Det1centermhccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Datacatcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Tipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Trimestre, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(Det1RegHojaCCTTCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDet1RegHojaCCTTCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDet1RegHojaCCTTCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDet1RegHojaCCTTCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDet1RegHojaCCTTCId);
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

        public Det1RegHojaCCTTCDTO GetDet1RegHojaCCTTCById(int id)
        {
            Det1RegHojaCCTTCDTO ob = new Det1RegHojaCCTTCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDet1RegHojaCCTTCById);
            dbProvider.AddInParameter(commandHoja, "DET1CENTERMHCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Det1centermhccodi = !dr.IsDBNull(dr.GetOrdinal("DET1CENTERMHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DET1CENTERMHCCODI")) : 0;
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateDet1RegHojaCCTTC(Det1RegHojaCCTTCDTO Det1RegHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDet1RegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, Det1RegHojaCCTTCDTO.Centralcodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, Det1RegHojaCCTTCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, Det1RegHojaCCTTCDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, Det1RegHojaCCTTCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, Det1RegHojaCCTTCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, Det1RegHojaCCTTCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, Det1RegHojaCCTTCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DET1CENTERMHCCODI", DbType.Int32, Det1RegHojaCCTTCDTO.Det1centermhccodi);
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
