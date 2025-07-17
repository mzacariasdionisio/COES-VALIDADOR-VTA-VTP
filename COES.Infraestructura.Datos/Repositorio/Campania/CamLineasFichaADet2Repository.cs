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
    public class CamLineasFichaADet2Repository : RepositoryBase, ICamLineasFichaADet2Repository
    {
        public CamLineasFichaADet2Repository(string strConn) : base(strConn) { }

        CamLineasFichaADet2Helper Helper = new CamLineasFichaADet2Helper();

        public List<LineasFichaADet2DTO> GetLineasFichaADet2Codi(int fichaACodi)
        {
            List<LineasFichaADet2DTO> lineasFichaADet2DTOs = new List<LineasFichaADet2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaADet2);
            dbProvider.AddInParameter(command, "FICHAADET2CODI", DbType.Int32, fichaACodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaADet2DTO ob = new LineasFichaADet2DTO
                    {
                        LinFichaADet2Codi = !dr.IsDBNull(dr.GetOrdinal("FICHAADET2CODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAADET2CODI")) : default(int),
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int),
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
                    lineasFichaADet2DTOs.Add(ob);
                }
            }
            return lineasFichaADet2DTOs;
        }

        public bool SaveLineasFichaADet2(LineasFichaADet2DTO lineasFichaADet2DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLineasFichaADet2);

            dbProvider.AddInParameter(dbCommand, "FICHAADET2CODI", DbType.Int32, lineasFichaADet2DTO.LinFichaADet2Codi);
            dbProvider.AddInParameter(dbCommand, "FICHAACODI", DbType.Int32, lineasFichaADet2DTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.Int32, lineasFichaADet2DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "R", DbType.Decimal, lineasFichaADet2DTO.R);
            dbProvider.AddInParameter(dbCommand, "X", DbType.Decimal, lineasFichaADet2DTO.X);
            dbProvider.AddInParameter(dbCommand, "B", DbType.Decimal, lineasFichaADet2DTO.B);
            dbProvider.AddInParameter(dbCommand, "G", DbType.Decimal, lineasFichaADet2DTO.G);
            dbProvider.AddInParameter(dbCommand, "R0", DbType.Decimal, lineasFichaADet2DTO.R0);
            dbProvider.AddInParameter(dbCommand, "X0", DbType.Decimal, lineasFichaADet2DTO.X0);
            dbProvider.AddInParameter(dbCommand, "B0", DbType.Decimal, lineasFichaADet2DTO.B0);
            dbProvider.AddInParameter(dbCommand, "G0", DbType.Decimal, lineasFichaADet2DTO.G0);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, lineasFichaADet2DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, lineasFichaADet2DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteLineasFichaADet2ById(int fichaADet2Codi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLineasFichaADet2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, fichaADet2Codi);

            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastLineasFichaADet2Codi()
        {
            int lastId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLineasFichaADet2Id);
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

        public LineasFichaADet2DTO GetLineasFichaADet2ById(int fichaADet2Codi)
        {
            LineasFichaADet2DTO ob = null;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaADet2ById);
            dbProvider.AddInParameter(command, "FICHAADET2CODI", DbType.Int32, fichaADet2Codi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new LineasFichaADet2DTO
                    {
                        LinFichaADet2Codi = dr.GetInt32(dr.GetOrdinal("FICHAADET2CODI")),
                        LinFichaACodi = dr.GetInt32(dr.GetOrdinal("FICHAADET2CODI")),
                        Tramo = dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)dr.GetInt32(dr.GetOrdinal("TRAMO")):null,
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
