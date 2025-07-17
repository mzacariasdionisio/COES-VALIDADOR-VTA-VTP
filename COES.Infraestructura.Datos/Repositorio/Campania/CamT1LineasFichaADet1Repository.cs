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
    public class CamT1LineasFichaADet1Repository : RepositoryBase, ICamT1LineasFichaADet1Repository
    {
        public CamT1LineasFichaADet1Repository(string strConn) : base(strConn) { }

        CamT1LinFichaADet1Helper Helper = new CamT1LinFichaADet1Helper();

        public List<T1LinFichaADet1DTO> GetLineasFichaADet1Codi(int proyCodi)
        {
            List<T1LinFichaADet1DTO> T1LinFichaADet1DTOs = new List<T1LinFichaADet1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaaDet1);
            dbProvider.AddInParameter(command, "LINFICHAACODI", DbType.Int32, proyCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T1LinFichaADet1DTO ob = new T1LinFichaADet1DTO
                    {
                        LinFichaADet1Codi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAADET1CODI")) : default(int),
                        LinFichaACodi = !dr.IsDBNull(dr.GetOrdinal("LINFICHAACODI")) ? dr.GetInt32(dr.GetOrdinal("LINFICHAACODI")) : default(int),
                        Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)dr.GetInt32(dr.GetOrdinal("TRAMO")) : null,
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
                    T1LinFichaADet1DTOs.Add(ob);
                }
            }
            return T1LinFichaADet1DTOs;
        }
        public bool SaveLineasFichaADet1(T1LinFichaADet1DTO T1LinFichaADet1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveLinfichaaDet1);

            dbProvider.AddInParameter(dbCommand, "LINFICHAADET1CODI", DbType.Int32, T1LinFichaADet1DTO.LinFichaADet1Codi);
            dbProvider.AddInParameter(dbCommand, "LINFICHAACODI", DbType.Int32, T1LinFichaADet1DTO.LinFichaACodi);
            dbProvider.AddInParameter(dbCommand, "TRAMO", DbType.String, T1LinFichaADet1DTO.Tramo);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, T1LinFichaADet1DTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "LONGITUD", DbType.Decimal, T1LinFichaADet1DTO.Longitud);
            dbProvider.AddInParameter(dbCommand, "MATCONDUCTOR", DbType.String, T1LinFichaADet1DTO.MatConductor);
            dbProvider.AddInParameter(dbCommand, "SECCONDUCTOR", DbType.Decimal, T1LinFichaADet1DTO.SecConductor);
            dbProvider.AddInParameter(dbCommand, "CONDUCTORFASE", DbType.Decimal, T1LinFichaADet1DTO.ConductorFase);
            dbProvider.AddInParameter(dbCommand, "CAPACIDADTOT", DbType.Decimal, T1LinFichaADet1DTO.CapacidadTot);
            dbProvider.AddInParameter(dbCommand, "CABGUARDA", DbType.Decimal, T1LinFichaADet1DTO.CabGuarda);
            dbProvider.AddInParameter(dbCommand, "RESISTCABGUARDA", DbType.Decimal, T1LinFichaADet1DTO.ResistCabGuarda);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, T1LinFichaADet1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, T1LinFichaADet1DTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, T1LinFichaADet1DTO.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
        public bool DeleteLineasFichaADet1ById(int LinFichaADet1Codi, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteLinfichaaDet1ById);
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
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastLinfichaaDet1Id);
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
        public T1LinFichaADet1DTO GetLineasFichaADet1ById(int LinFichaADet1Codi)
        {
            T1LinFichaADet1DTO ob = null;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLinfichaaDet1ById);
            dbProvider.AddInParameter(command, "LINLINFICHAADET1CODI", DbType.Int32, LinFichaADet1Codi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    ob = new T1LinFichaADet1DTO
                    {
                        LinFichaADet1Codi = dr.GetInt32(dr.GetOrdinal("LINFICHAADET1CODI")),
                        LinFichaACodi = dr.GetInt32(dr.GetOrdinal("LINFICHAACODI")),
                        Tramo = !dr.IsDBNull(dr.GetOrdinal("TRAMO")) ? (int?)dr.GetInt32(dr.GetOrdinal("TRAMO")):null,
                        Tipo = dr.GetString(dr.GetOrdinal("TIPO")),
                        Longitud = !dr.IsDBNull(dr.GetOrdinal("LONGITUD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LONGITUD")) : null,
                        MatConductor = !dr.IsDBNull(dr.GetOrdinal("MATCONDUCTOR")) ? dr.GetString(dr.GetOrdinal("MATCONDUCTOR")) : null,
                        SecConductor = !dr.IsDBNull(dr.GetOrdinal("SECCONDUCTOR")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SECCONDUCTOR")) : null,
                        ConductorFase = !dr.IsDBNull(dr.GetOrdinal("CONDUCTORFASE")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONDUCTORFASE")) : null,
                        CapacidadTot = !dr.IsDBNull(dr.GetOrdinal("CAPACIDADTOT")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CAPACIDADTOT")) : null,
                        CabGuarda = !dr.IsDBNull(dr.GetOrdinal("CABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CABGUARDA")) : null,
                        ResistCabGuarda = !dr.IsDBNull(dr.GetOrdinal("RESISTCABGUARDA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("RESISTCABGUARDA")) : null,

                        //Longitud = dr.GetDecimal(dr.GetOrdinal("LONGITUD")),
                        //MatConductor = dr.GetString(dr.GetOrdinal("MATCONDUCTOR")),
                        //SecConductor = dr.GetDecimal(dr.GetOrdinal("SECCONDUCTOR")),
                        //ConductorFase = dr.GetInt32(dr.GetOrdinal("CONDUCTORFASE")),
                        //CapacidadTot = dr.GetDecimal(dr.GetOrdinal("CAPACIDADTOT")),
                        //CabGuarda = dr.GetInt32(dr.GetOrdinal("CABGUARDA")),
                        //ResistCabGuarda = dr.GetDecimal(dr.GetOrdinal("RESISTCABGUARDA")),
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
