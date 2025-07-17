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
    public class CamCuestionarioH2VGRepository : RepositoryBase, ICamCuestionarioH2VGRepository
    {
        public CamCuestionarioH2VGRepository(string strConn) : base(strConn) { }

        CamCuestionarioH2VGHelper Helper = new CamCuestionarioH2VGHelper();

        public List<CuestionarioH2VGDTO> GetCamCuestionarioH2VG(int proyCodi)
        {
            List<CuestionarioH2VGDTO> h2vgDTOs = new List<CuestionarioH2VGDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VGByCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VGDTO ob = new CuestionarioH2VGDTO();
                    ob.H2vGCodi = !dr.IsDBNull(dr.GetOrdinal("CUESTIONARIOH2VGCODI")) ? dr.GetInt32(dr.GetOrdinal("CUESTIONARIOH2VGCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    h2vgDTOs.Add(ob);
                }
            }
            return h2vgDTOs;
        }

        public bool SaveCamCuestionarioH2VG(CuestionarioH2VGDTO h2vgDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VG);
            dbProvider.AddInParameter(dbCommand, "CUESTIONARIOH2VGCODI", DbType.Int32, h2vgDTO.H2vGCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2vgDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, h2vgDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, h2vgDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, h2vgDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, h2vgDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, h2vgDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, h2vgDTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, h2vgDTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCamCuestionarioH2VGById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VGById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastCamCuestionarioH2VGCodi()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VGId);
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

        public List<CuestionarioH2VGDTO> GetCamCuestionarioH2VGById(int id)
        {
            List<CuestionarioH2VGDTO> h2vgDTOs = new List<CuestionarioH2VGDTO>();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VGById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {

                    CuestionarioH2VGDTO ob = new CuestionarioH2VGDTO();
                    ob.H2vGCodi = !dr.IsDBNull(dr.GetOrdinal("CUESTIONARIOH2VGCODI")) ? dr.GetInt32(dr.GetOrdinal("CUESTIONARIOH2VGCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Tipo = !dr.IsDBNull(dr.GetOrdinal("TIPO")) ? dr.GetString(dr.GetOrdinal("TIPO")) : "";
                    ob.DataCatCodi = !dr.IsDBNull(dr.GetOrdinal("DATACATCODI")) ? dr.GetInt32(dr.GetOrdinal("DATACATCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Trimestre = !dr.IsDBNull(dr.GetOrdinal("TRIMESTRE")) ? dr.GetInt32(dr.GetOrdinal("TRIMESTRE")) : 0;
                    ob.Valor = !dr.IsDBNull(dr.GetOrdinal("VALOR")) ? dr.GetString(dr.GetOrdinal("VALOR")) : "";
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    h2vgDTOs.Add(ob);
                }
            }
            return h2vgDTOs;
        }

        public bool UpdateCamCuestionarioH2VG(CuestionarioH2VGDTO h2vgDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateCuestionarioH2VG);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2vgDTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "TIPO", DbType.String, h2vgDTO.Tipo);
            dbProvider.AddInParameter(dbCommand, "DATACATCODI", DbType.Int32, h2vgDTO.DataCatCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, h2vgDTO.Anio);
            dbProvider.AddInParameter(dbCommand, "TRIMESTRE", DbType.Int32, h2vgDTO.Trimestre);
            dbProvider.AddInParameter(dbCommand, "VALOR", DbType.String, h2vgDTO.Valor);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, h2vgDTO.UsuModificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, h2vgDTO.FecModificacion);
            dbProvider.AddInParameter(dbCommand, "CUESTIONARIOSH2VGCODI", DbType.Int32, h2vgDTO.H2vGCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }
    }
}

