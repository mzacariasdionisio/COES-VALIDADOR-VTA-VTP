using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamT1LineasFichaADet2Repository : RepositoryBase, ICamT1LineasFichaADet2Repository
    {
        public CamT1LineasFichaADet2Repository(string strConn) : base(strConn) { }

        CamT1LinFichaADet2Helper Helper = new CamT1LinFichaADet2Helper();

        public List<T1LinFichaADet2DTO> GetLineasFichaADet2Codi(int fichaACodi)
        {
            List<T1LinFichaADet2DTO> t1LinFichaADet2DTOs = new List<T1LinFichaADet2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaaDet2);
            dbProvider.AddInParameter(command, "LINFICHAADET2CODI", DbType.Int32, fichaACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T1LinFichaADet2DTO ob = new T1LinFichaADet2DTO
                    {
                        LinFichaADet2Codi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAADET2CODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAADET2CODI")) : default(int),
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAACODI")) : default(int),
                        Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)dr.GetInt32(dr.GetOrdinal("TRAMO")) : null,
                        R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null,
                        X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null,
                        B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null,
                        G = !dr.IsDBNull(dr.GetOrdinal("G")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G")) : null,
                        R0 = !dr.IsDBNull(dr.GetOrdinal("R0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R0")) : null,
                        X0 = !dr.IsDBNull(dr.GetOrdinal("X0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X0")) : null,
                        B0 = !dr.IsDBNull(dr.GetOrdinal("B0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B0")) : null,
                        G0 = !dr.IsDBNull(dr.GetOrdinal("G0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G0")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : default(DateTime),
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : default(DateTime),
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null

                    };
                    t1LinFichaADet2DTOs.Add(ob);
                }
            }
            return t1LinFichaADet2DTOs;
        }

        public bool SaveLineasFichaADet2(T1LinFichaADet2DTO t1LinFichaADet2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLinfichaaDet2);

            dbProvider.AddInParameter(dbCommand, "LINFICHAADET2CODI", DbType.Int32, t1LinFichaADet2DTO.LinFichaADet2Codi);
            dbProvider.AddInParameter(dbCommand, "LINFICHAACODI", DbType.Int32, t1LinFichaADet2DTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.Int32, t1LinFichaADet2DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "R", DbType.Decimal, t1LinFichaADet2DTO.R);
            dbProvider.AddInParameter(dbCommand, "X", DbType.Decimal, t1LinFichaADet2DTO.X);
            dbProvider.AddInParameter(dbCommand, "B", DbType.Decimal, t1LinFichaADet2DTO.B);
            dbProvider.AddInParameter(dbCommand, "G", DbType.Decimal, t1LinFichaADet2DTO.G);
            dbProvider.AddInParameter(dbCommand, "R0", DbType.Decimal, t1LinFichaADet2DTO.R0);
            dbProvider.AddInParameter(dbCommand, "X0", DbType.Decimal, t1LinFichaADet2DTO.X0);
            dbProvider.AddInParameter(dbCommand, "B0", DbType.Decimal, t1LinFichaADet2DTO.B0);
            dbProvider.AddInParameter(dbCommand, "G0", DbType.Decimal, t1LinFichaADet2DTO.G0);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, t1LinFichaADet2DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, t1LinFichaADet2DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteLineasFichaADet2ById(int fichaADet2Codi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLinfichaaDet2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, fichaADet2Codi);

            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastLineasFichaADet2Id()
        {
            int lastId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLinfichaaDet2Id);
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                lastId = Convert.ToInt32(result) + 1;
            }
            else
            {
                lastId = 1;
            }

            return lastId;
        }

        public T1LinFichaADet2DTO GetLineasFichaADet2ById(int fichaADet2Codi)
        {
            T1LinFichaADet2DTO ob = null;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaaDet2ById);
            dbProvider.AddInParameter(command, "LINFICHAADET2CODI", DbType.Int32, fichaADet2Codi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new T1LinFichaADet2DTO
                    {
                        LinFichaADet2Codi = dr.GetInt32(dr.GetOrdinal("LINFICHAADET2CODI")),
                        LinFichaACodi = dr.GetInt32(dr.GetOrdinal("LINFICHAADET2CODI")),
                        Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)dr.GetInt32(dr.GetOrdinal("TRAMO")) : null,
                        R = !dr.IsDBNull(dr.GetOrdinal("R")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R")) : null,
                        X = !dr.IsDBNull(dr.GetOrdinal("X")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X")) : null,
                        B = !dr.IsDBNull(dr.GetOrdinal("B")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B")) : null,
                        G = !dr.IsDBNull(dr.GetOrdinal("G")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G")) : null,
                        R0 = !dr.IsDBNull(dr.GetOrdinal("R0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("R0")) : null,
                        X0 = !dr.IsDBNull(dr.GetOrdinal("X0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("X0")) : null,
                        B0 = !dr.IsDBNull(dr.GetOrdinal("B0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("B0")) : null,
                        G0 = !dr.IsDBNull(dr.GetOrdinal("G0")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("G0")) : null,
                        UsuCreacion = dr.GetString(dr.GetOrdinal("USU_CREACION")),
                        FecCreacion = dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")),
                        UsuModificacion = dr.GetString(dr.GetOrdinal("USU_MODIFICACION")),
                        FecModificacion = dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")),
                        IndDel = dr.GetString(dr.GetOrdinal("IND_DEL"))
                    };
                }
            }
            return ob;
        }
    }
}
