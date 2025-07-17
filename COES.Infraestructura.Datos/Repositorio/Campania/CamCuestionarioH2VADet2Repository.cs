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
    public class CamCuestionarioH2VADet2Repository : RepositoryBase, ICamCuestionarioH2VADet2Repository
    {
        public CamCuestionarioH2VADet2Repository(string strConn) : base(strConn) { }

        CamCuestionarioH2VADet2Helper Helper = new CamCuestionarioH2VADet2Helper();

        public List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2Codi(int h2vaCodi)
        {
            List<CuestionarioH2VADet2DTO> cuestionarios = new List<CuestionarioH2VADet2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VADet2Codi);
            dbProvider.AddInParameter(command, "H2VACODI", DbType.Int32, h2vaCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADet2DTO cuestionario = new CuestionarioH2VADet2DTO();
                    cuestionario.H2vaDet2Codi = !dr.IsDBNull(dr.GetOrdinal("H2VADET2CODI")) ? dr.GetInt32(dr.GetOrdinal("H2VADET2CODI")) : 0;
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    cuestionario.EnElaboracion = !dr.IsDBNull(dr.GetOrdinal("ENELABORACION")) ? dr.GetString(dr.GetOrdinal("ENELABORACION")) : "";
                    cuestionario.Presentado = !dr.IsDBNull(dr.GetOrdinal("PRESENTADO")) ? dr.GetString(dr.GetOrdinal("PRESENTADO")) : "";
                    cuestionario.EnTramite = !dr.IsDBNull(dr.GetOrdinal("ENTRAMITE")) ? dr.GetString(dr.GetOrdinal("ENTRAMITE")) : "";
                    cuestionario.Aprobado = !dr.IsDBNull(dr.GetOrdinal("APROBADO")) ? dr.GetString(dr.GetOrdinal("APROBADO")) : "";
                    cuestionario.Firmado = !dr.IsDBNull(dr.GetOrdinal("FIRMADO")) ? dr.GetString(dr.GetOrdinal("FIRMADO")) : "";
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

        public bool SaveCuestionarioH2VADet2(CuestionarioH2VADet2DTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VADet2);
            dbProvider.AddInParameter(dbCommand, "H2VADET2CODI", DbType.Int32, cuestionario.H2vaDet2Codi);
            dbProvider.AddInParameter(dbCommand, "H2VACODI", DbType.Int32, cuestionario.H2vaCodi);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, cuestionario.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ENELABORACION", DbType.String, cuestionario.EnElaboracion);
            dbProvider.AddInParameter(dbCommand, "PRESENTADO", DbType.String, cuestionario.Presentado);
            dbProvider.AddInParameter(dbCommand, "ENTRAMITE", DbType.String, cuestionario.EnTramite);
            dbProvider.AddInParameter(dbCommand, "APROBADO", DbType.String, cuestionario.Aprobado);
            dbProvider.AddInParameter(dbCommand, "FIRMADO", DbType.String, cuestionario.Firmado);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VADet2ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VADet2ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastCuestionarioH2VADet2Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VADet2Id);
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

        public List<CuestionarioH2VADet2DTO> GetCuestionarioH2VADet2ById(int id)
        {
            List<CuestionarioH2VADet2DTO> cuestionarios = new List<CuestionarioH2VADet2DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VADet2ById);
            dbProvider.AddInParameter(command, "H2VACODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VADet2DTO cuestionario = new CuestionarioH2VADet2DTO();
                    cuestionario.H2vaDet2Codi = !dr.IsDBNull(dr.GetOrdinal("H2VADET2CODI")) ? dr.GetInt32(dr.GetOrdinal("H2VADET2CODI")) : 0;
                    cuestionario.H2vaCodi = !dr.IsDBNull(dr.GetOrdinal("H2VACODI")) ? dr.GetInt32(dr.GetOrdinal("H2VACODI")) : 0;
                    cuestionario.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    cuestionario.EnElaboracion = !dr.IsDBNull(dr.GetOrdinal("ENELABORACION")) ? dr.GetString(dr.GetOrdinal("ENELABORACION")) : "";
                    cuestionario.Presentado = !dr.IsDBNull(dr.GetOrdinal("PRESENTADO")) ? dr.GetString(dr.GetOrdinal("PRESENTADO")) : "";
                    cuestionario.EnTramite = !dr.IsDBNull(dr.GetOrdinal("ENTRAMITE")) ? dr.GetString(dr.GetOrdinal("ENTRAMITE")) : "";
                    cuestionario.Aprobado = !dr.IsDBNull(dr.GetOrdinal("APROBADO")) ? dr.GetString(dr.GetOrdinal("APROBADO")) : "";
                    cuestionario.Firmado = !dr.IsDBNull(dr.GetOrdinal("FIRMADO")) ? dr.GetString(dr.GetOrdinal("FIRMADO")) : "";
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