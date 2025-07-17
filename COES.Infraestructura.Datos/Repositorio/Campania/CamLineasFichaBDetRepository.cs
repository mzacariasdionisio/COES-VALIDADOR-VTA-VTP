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
    public class CamLineasFichaBDetRepository : RepositoryBase, ICamLineasFichaBDetRepository
    {
        public CamLineasFichaBDetRepository(string strConn) : base(strConn) { }

        CamLineasFichaBDetHelper Helper = new CamLineasFichaBDetHelper();

        public List<LineasFichaBDetDTO> GetLineasFichaBDet(int fichaccodi)
        {
            List<LineasFichaBDetDTO> detRegHojaCDTOs = new List<LineasFichaBDetDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaBDet);
            dbProvider.AddInParameter(command, "FICHABCODI", DbType.Int32, fichaccodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaBDetDTO ob = new LineasFichaBDetDTO();
                    ob.FichaBDetCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABDETCODI")) : 0;
                    ob.FichaBCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    detRegHojaCDTOs.Add(ob);
                }
            }

            return detRegHojaCDTOs;
        }

        public bool SaveLineasFichaBDet(LineasFichaBDetDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLineasFichaBDet);
            dbProvider.AddInParameter(dbCommand, "FICHABDETCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.FichaBDetCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.FichaBCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.DataCatCodi, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.Anio, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, ObtenerValorOrDefault(detRegHojaCDTO.Trimestre, typeof(int)));
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.Valor, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, ObtenerValorOrDefault(detRegHojaCDTO.UsuCreacion, typeof(string)));
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteLineasFichaBDetById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLineasFichaBDetById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastLineasFichaBDetCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLineasFichaBDetId);
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

        public LineasFichaBDetDTO GetLineasFichaBDetById(int id)
        {
            LineasFichaBDetDTO ob = new LineasFichaBDetDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaBDetById);
            dbProvider.AddInParameter(commandHoja, "FICHABDETCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.FichaBDetCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABDETCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABDETCODI")) : 0;
                    ob.FichaBCodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : string.Empty;
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }

            }
            return ob;
        }

        public bool UpdateLineasFichaBDet(LineasFichaBDetDTO detRegHojaCDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateLineasFichaBDet);
            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.Int32, detRegHojaCDTO.FichaBCodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, detRegHojaCDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, detRegHojaCDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, detRegHojaCDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, detRegHojaCDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, detRegHojaCDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FICHABDETCODI", DbType.Int32, detRegHojaCDTO.FichaBDetCodi);
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

