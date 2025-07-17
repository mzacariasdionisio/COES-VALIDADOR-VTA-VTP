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
    public class CamDet2RegHojaCCTTCRepository : RepositoryBase, ICamDet2RegHojaCCTTCRepository
    {

        public CamDet2RegHojaCCTTCRepository(string strConn) : base(strConn) { }

        CamDet2RegHojaCCTTCHelper Helper = new CamDet2RegHojaCCTTCHelper();

        public List<Det2RegHojaCCTTCDTO> GetDet2RegHojaCCTTCCentralCodi(int fichaccodi)
        {
            List<Det2RegHojaCCTTCDTO> Det2RegHojaCCTTCDTOs = new List<Det2RegHojaCCTTCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetDet2RegHojaCCTTCCentralCodi);
            dbProvider.AddInParameter(command, "CENTRALCODI", DbType.String, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    Det2RegHojaCCTTCDTO ob = new Det2RegHojaCCTTCDTO();
                    ob.Det2centermhccodi = !dr.IsDBNull(dr.GetOrdinal("DET2CENTERMHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DET2CENTERMHCCODI")) : 0;
                    ob.Centralcodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALCODI")) : 0;
                    ob.Datacatcodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : string.Empty;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    Det2RegHojaCCTTCDTOs.Add(ob);
                }
            }

            return Det2RegHojaCCTTCDTOs;
        }

        public bool SaveDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO Det2RegHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveDet2RegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "DET2CENTERMHCCODI", DbType.Int32, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Det2centermhccodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Centralcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Datacatcodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Tipo, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Trimestre, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(Det2RegHojaCCTTCDTO.Usucreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteDet2RegHojaCCTTCById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteDet2RegHojaCCTTCById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastDet2RegHojaCCTTCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastDet2RegHojaCCTTCId);
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

        public Det2RegHojaCCTTCDTO GetDet2RegHojaCCTTCById(int id)
        {
            Det2RegHojaCCTTCDTO ob = new Det2RegHojaCCTTCDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetDet2RegHojaCCTTCById);
            dbProvider.AddInParameter(commandHoja, "DET2CENTERMHCCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Det2centermhccodi = !dr.IsDBNull(dr.GetOrdinal("DET2CENTERMHCCODI")) ? dr.GetInt32(dr.GetOrdinal("DET2CENTERMHCCODI")) : 0;
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

        public bool UpdateDet2RegHojaCCTTC(Det2RegHojaCCTTCDTO Det2RegHojaCCTTCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateDet2RegHojaCCTTC);
            dbProvider.AddInParameter(dbCommand, "CENTRALCODI", DbType.Int32, Det2RegHojaCCTTCDTO.Centralcodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, Det2RegHojaCCTTCDTO.Datacatcodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, Det2RegHojaCCTTCDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, Det2RegHojaCCTTCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, Det2RegHojaCCTTCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, Det2RegHojaCCTTCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, Det2RegHojaCCTTCDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "DET2CENTERMHCCODI", DbType.Int32, Det2RegHojaCCTTCDTO.Det2centermhccodi);
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
