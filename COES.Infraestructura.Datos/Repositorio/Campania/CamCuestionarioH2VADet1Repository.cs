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
    public class CamCuestionarioH2VADet1Repository : RepositoryBase, ICamCuestionarioH2VADet1Repository
    {
        public CamCuestionarioH2VADet1Repository(string strConn) : base(strConn) { }

        CamCuestionarioH2VADet1Helper Helper = new CamCuestionarioH2VADet1Helper();

        public List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1Codi(int h2vaCodi)
        {
            List<CuestionarioH2VADet1DTO> cuestionarios = new List<CuestionarioH2VADet1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VADet1Codi);
            dbProvider.AddInParameter(command, "H2VACODI", DbType.Int32, h2vaCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADet1DTO cuestionario = new CuestionarioH2VADet1DTO();
                    cuestionario.H2vaDet1Codi = !dr.IsDBNull(dr.GetOrdinal("H2VADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("H2VADET1CODI")) : 0;
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? (int?)dr.GetInt32(dr.GetOrdinal("ANIO")) : null;
                    cuestionario.MontoInversion = !dr.IsDBNull(dr.GetOrdinal("MONTOINVERSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MONTOINVERSION")) : null;
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    cuestionario.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";


                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }

        public bool SaveCuestionarioH2VADet1(CuestionarioH2VADet1DTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VADet1);
            dbProvider.AddInParameter(dbCommand, "H2VADET1CODI", DbType.Int32, cuestionario.H2vaDet1Codi);
            dbProvider.AddInParameter(dbCommand, "H2VACODI", DbType.Int32, cuestionario.H2vaCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.Int32, cuestionario.Anio);
            dbProvider.AddInParameter(dbCommand, "MONTOINVERSION", DbType.Decimal, cuestionario.MontoInversion);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VADet1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VADet1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCuestionarioH2VADet1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VADet1Id);
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

        public List<CuestionarioH2VADet1DTO> GetCuestionarioH2VADet1ById(int id)
        {
            List<CuestionarioH2VADet1DTO> cuestionarios = new List<CuestionarioH2VADet1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VADet1ById);
            dbProvider.AddInParameter(command, "H2VACODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADet1DTO cuestionario = new CuestionarioH2VADet1DTO();
                    cuestionario.H2vaDet1Codi = !dr.IsDBNull(dr.GetOrdinal("H2VADET1CODI")) ? dr.GetInt32(dr.GetOrdinal("H2VADET1CODI")) : 0;
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? (int?)dr.GetInt32(dr.GetOrdinal("ANIO")) : null;
                    cuestionario.MontoInversion = !dr.IsDBNull(dr.GetOrdinal("MONTOINVERSION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("MONTOINVERSION")) : null;
                    cuestionario.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    cuestionario.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    cuestionario.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    cuestionario.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    cuestionario.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";

                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }
    }
}
