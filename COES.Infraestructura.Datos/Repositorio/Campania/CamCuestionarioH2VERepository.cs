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
    public class CamCuestionarioH2VERepository : RepositoryBase, ICamCuestionarioH2VERepository
    {
        public CamCuestionarioH2VERepository(string strConn) : base(strConn) { }

        CamCuestionarioH2VEHelper Helper = new CamCuestionarioH2VEHelper();

        public List<CuestionarioH2VEDTO> GetCuestionarioH2VECodi(int proyCodi)
        {
            List<CuestionarioH2VEDTO> cuestionarios = new List<CuestionarioH2VEDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VECodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VEDTO cuestionario = new CuestionarioH2VEDTO
                    {
                        H2veCodi = !dr.IsDBNull(dr.GetOrdinal("H2VECODI")) ? dr.GetInt32(dr.GetOrdinal("H2VECODI")) : 0,
                        ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0,
                        Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "",
                        ConsumoEnergetico = !dr.IsDBNull(dr.GetOrdinal("CONSUMOENERGETICO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("CONSUMOENERGETICO")) : null,
                        ProduccionCentral = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONCENTRAL")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PRODUCCIONCENTRAL")) : null,
                        UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "",
                        FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue,
                        UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "",
                        FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue,
                        IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "",

                    };

                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }

        public bool SaveCuestionarioH2VE(CuestionarioH2VEDTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VE);
            dbProvider.AddInParameter(dbCommand, "H2VECODI", DbType.Int32, cuestionario.H2veCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, cuestionario.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "HORA", DbType.String, cuestionario.Hora);
            dbProvider.AddInParameter(dbCommand, "CONSUMOENERGETICO", DbType.Int32, cuestionario.ConsumoEnergetico);
            dbProvider.AddInParameter(dbCommand, "PRODUCCIONCENTRAL", DbType.Int32, cuestionario.ProduccionCentral);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VEById(int h2veCodi, string usuario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VEById);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2veCodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public int GetLastCuestionarioH2VEId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VEId);
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

        public List<CuestionarioH2VEDTO> GetCuestionarioH2VEById(int h2veCodi)
        {
            List<CuestionarioH2VEDTO> cuestionarios = new List<CuestionarioH2VEDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VEById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, h2veCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.Int32, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VEDTO cuestionario = new CuestionarioH2VEDTO();
                    cuestionario.H2veCodi = !dr.IsDBNull(dr.GetOrdinal("H2VECODI")) ? dr.GetInt32(dr.GetOrdinal("H2VECODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    cuestionario.ConsumoEnergetico = !dr.IsDBNull(dr.GetOrdinal("CONSUMOENERGETICO")) ? (decimal?)dr.GetInt32(dr.GetOrdinal("CONSUMOENERGETICO")) : null;
                    cuestionario.ProduccionCentral = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONCENTRAL")) ? (decimal?)dr.GetInt32(dr.GetOrdinal("PRODUCCIONCENTRAL")) : null;
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

        public List<CuestionarioH2VEDTO> GetFormatoH2VEByFilter(string plancodi, string empresa, string estado)
        {
            List<CuestionarioH2VEDTO> oblist = new List<CuestionarioH2VEDTO>();

            string query = $@"
        SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  
        FROM cam_cuestionarioh2ve CGB
        INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
        INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
        INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
        LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
        WHERE TR.PERICODI IN ({plancodi})
          AND PL.CODEMPRESA IN ({empresa})
          AND CGB.IND_DEL = 0
          AND PL.PLANESTADO = '{estado}'
        ORDER BY TR.PERICODI, CGB.PROYCODI, PL.CODEMPRESA, CGB.H2VECODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VEDTO ob = new CuestionarioH2VEDTO();

                    ob.H2veCodi = !dr.IsDBNull(dr.GetOrdinal("H2VECODI")) ? dr.GetInt32(dr.GetOrdinal("H2VECODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Hora = !dr.IsDBNull(dr.GetOrdinal("HORA")) ? dr.GetString(dr.GetOrdinal("HORA")) : "";
                    ob.ConsumoEnergetico = !dr.IsDBNull(dr.GetOrdinal("CONSUMOENERGETICO")) ? dr.GetDecimal(dr.GetOrdinal("CONSUMOENERGETICO")) : (decimal?)null;
                    ob.ProduccionCentral = !dr.IsDBNull(dr.GetOrdinal("PRODUCCIONCENTRAL")) ? dr.GetDecimal(dr.GetOrdinal("PRODUCCIONCENTRAL")) : (decimal?)null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : "";
                    // Nuevos campos
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : "";
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : "";
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : "";
                    ob.Confidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : "";
                    oblist.Add(ob);
                }
            }

            return oblist;
        }


    }
}