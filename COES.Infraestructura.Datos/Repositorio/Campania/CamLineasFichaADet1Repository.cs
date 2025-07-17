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
    public class CamLineasFichaADet1Repository : RepositoryBase, ICamLineasFichaADet1Repository
    {
        public CamLineasFichaADet1Repository(string strConn) : base(strConn) { }

        CamLineasFichaADet1Helper Helper = new CamLineasFichaADet1Helper();

        public List<LineasFichaADet1DTO> GetLineasFichaADet1Codi(int proyCodi)
        {
            List<LineasFichaADet1DTO> lineasFichaADet1DTOs = new List<LineasFichaADet1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaADet1);
            dbProvider.AddInParameter(command, "FICHAACODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    LineasFichaADet1DTO ob = new LineasFichaADet1DTO
                    {
                        LinFichaADet1Codi = !dr.IsDBNull(dr.GetOrdinal("FICHAADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAADET1CODI")) : default(int),
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("FICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("FICHAACODI")) : default(int),
                        Tramo = dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("TRAMO")),
                        Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : null,
                        Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null,
                        MatConductor = !dr.IsDBNull(dr.GetOrdinal("MATCONDUCTOR")) ? dr.GetString(dr.GetOrdinal("MATCONDUCTOR")) : null,
                        SecConductor = !dr.IsDBNull(dr.GetOrdinal("SECCONDUCTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCONDUCTOR")) : null,
                        ConductorFase = !dr.IsDBNull(dr.GetOrdinal("CONDUCTORFASE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONDUCTORFASE")) : null,
                        CapacidadTot = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTOT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDADTOT")) : null,
                        CabGuarda = !dr.IsDBNull(dr.GetOrdinal("CABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CABGUARDA")) : null,
                        ResistCabGuarda = !dr.IsDBNull(dr.GetOrdinal("RESISTCABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RESISTCABGUARDA")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null,
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : default(DateTime),
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : null,
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : default(DateTime),
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null

                    };
                    lineasFichaADet1DTOs.Add(ob);
                }
            }
            return lineasFichaADet1DTOs;
        }
        public bool SaveLineasFichaADet1(LineasFichaADet1DTO lineasFichaADet1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLineasFichaADet1);

            dbProvider.AddInParameter(dbCommand, "FICHAADET1CODI", DbType.Int32, lineasFichaADet1DTO.LinFichaADet1Codi);
            dbProvider.AddInParameter(dbCommand, "FICHAACODI", DbType.Int32, lineasFichaADet1DTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.String, lineasFichaADet1DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, lineasFichaADet1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, lineasFichaADet1DTO.Longitud);
            dbProvider.AddInParameter(dbCommand, "MATCONDUCTOR", DbType.String, lineasFichaADet1DTO.MatConductor);
            dbProvider.AddInParameter(dbCommand, "SECCONDUCTOR", DbType.Decimal, lineasFichaADet1DTO.SecConductor);
            dbProvider.AddInParameter(dbCommand, "CONDUCTORFASE", DbType.Decimal, lineasFichaADet1DTO.ConductorFase);
            dbProvider.AddInParameter(dbCommand, "CAPACIDADTOT", DbType.Decimal, lineasFichaADet1DTO.CapacidadTot);
            dbProvider.AddInParameter(dbCommand, "CABGUARDA", DbType.Decimal, lineasFichaADet1DTO.CabGuarda);
            dbProvider.AddInParameter(dbCommand, "RESISTCABGUARDA", DbType.Decimal, lineasFichaADet1DTO.ResistCabGuarda);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, lineasFichaADet1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, lineasFichaADet1DTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, lineasFichaADet1DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
        public bool DeleteLineasFichaADet1ById(int LinFichaADet1Codi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLineasFichaADet1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, LinFichaADet1Codi);

            dbProvider.ExecuteNonQuery(command);
            return true;
        }
        public int GetLastLineasFichaADet1Id()
        {
            int lastId = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLineasFichaADet1Id);
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
        public LineasFichaADet1DTO GetLineasFichaADet1ById(int LinFichaADet1Codi)
        {
            LineasFichaADet1DTO ob = null;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLineasFichaADet1ById);
            dbProvider.AddInParameter(command, "LINFICHAADET1CODI", DbType.Int32, LinFichaADet1Codi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new LineasFichaADet1DTO
                    {
                        LinFichaADet1Codi = dr.GetInt32(dr.GetOrdinal("FICHAADET1CODI")),
                        LinFichaACodi = dr.GetInt32(dr.GetOrdinal("FICHAACODI")),
                        Tramo = dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)null : dr.GetInt32(dr.GetOrdinal("TRAMO")),
                        Tipo = dr.GetString(dr.GetOrdinal("TIPO")),
                        Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null,
                        MatConductor = dr.GetString(dr.GetOrdinal("MATCONDUCTOR")),
                        SecConductor = !dr.IsDBNull(dr.GetOrdinal("SECCONDUCTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCONDUCTOR")) : null,
                        ConductorFase = dr.GetInt32(dr.GetOrdinal("CONDUCTORFASE")),
                        CapacidadTot = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTOT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDADTOT")) : null,
                        CabGuarda = dr.GetInt32(dr.GetOrdinal("CABGUARDA")),
                        ResistCabGuarda = !dr.IsDBNull(dr.GetOrdinal("RESISTCABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RESISTCABGUARDA")) : null,
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
