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
    public class CamCuestionarioH2VCRepository : RepositoryBase, ICamCuestionarioH2VCRepository
    {
        private readonly CamCuestionarioH2VCHelper Helper = new CamCuestionarioH2VCHelper();

        public CamCuestionarioH2VCRepository(string connectionString) : base(connectionString) { }

        public List<CuestionarioH2VCDTO> GetCuestionarioH2VCCodi(int proyCodi)
        {
            List<CuestionarioH2VCDTO> cuestionarios = new List<CuestionarioH2VCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VCCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VCDTO cuestionario = new CuestionarioH2VCDTO();
                    cuestionario.H2vcCodi = !dr.IsDBNull(dr.GetOrdinal("H2VCCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VCCODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    cuestionario.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    cuestionario.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    cuestionario.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    cuestionario.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    cuestionario.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    cuestionario.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    cuestionario.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
                    cuestionarios.Add(cuestionario);
                }
            }
            return cuestionarios;
        }

        public bool SaveCuestionarioH2VC(CuestionarioH2VCDTO cuestionario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveCuestionarioH2VC);
            dbProvider.AddInParameter(dbCommand, "H2VCCODI", DbType.Int32, cuestionario.H2vcCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, cuestionario.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "ANIO", DbType.String, cuestionario.Anio);
            dbProvider.AddInParameter(dbCommand, "MES", DbType.String, cuestionario.Mes);
            dbProvider.AddInParameter(dbCommand, "DEMANDAENERGIA", DbType.Decimal, cuestionario.DemandaEnergia);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHP", DbType.Decimal, cuestionario.DemandaHP);
            dbProvider.AddInParameter(dbCommand, "DEMANDAHFP", DbType.Decimal, cuestionario.DemandaHFP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONENERGIA", DbType.Decimal, cuestionario.GeneracionEnergia);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHP", DbType.Decimal, cuestionario.GeneracionHP);
            dbProvider.AddInParameter(dbCommand, "GENERACIONHFP", DbType.Decimal, cuestionario.GeneracionHFP);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, cuestionario.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, cuestionario.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, cuestionario.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public bool DeleteCuestionarioH2VCById(int h2vcCodi, string usuario)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlDeleteCuestionarioH2VCById);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, h2vcCodi);

            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }

        public int GetLastCuestionarioH2VCId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastCuestionarioH2VCId);
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

        public List<CuestionarioH2VCDTO> GetCuestionarioH2VCById(int h2vcCodi)
        {
            List<CuestionarioH2VCDTO> cuestionarios = new List<CuestionarioH2VCDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCuestionarioH2VCById);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, h2vcCodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VCDTO cuestionario = new CuestionarioH2VCDTO();
                    cuestionario.H2vcCodi = !dr.IsDBNull(dr.GetOrdinal("H2VCCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VCCODI")) : 0;
                    cuestionario.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    cuestionario.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    cuestionario.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    cuestionario.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : null;
                    cuestionario.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : null;
                    cuestionario.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : null;
                    cuestionario.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : null;
                    cuestionario.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : null;
                    cuestionario.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : null;
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

        public List<CuestionarioH2VCDTO> GetFormatoH2VCByFilter(string plancodi, string empresa, string estado)
        {
            List<CuestionarioH2VCDTO> oblist = new List<CuestionarioH2VCDTO>();

            string query = $@"
        SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TP.TIPONOMBRE, TF.TIPOFINOMBRE,TR.PROYCONFIDENCIAL  
        FROM cam_cuestionarioh2vc CGB
        INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
        INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
        INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
        LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
        WHERE TR.PERICODI IN ({plancodi})
          AND PL.CODEMPRESA IN ({empresa})
          AND CGB.IND_DEL = 0
          AND PL.PLANESTADO = '{estado}'
        ORDER BY TR.PERICODI, CGB.PROYCODI, PL.CODEMPRESA, CGB.H2VCCODI ASC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CuestionarioH2VCDTO ob = new CuestionarioH2VCDTO();

                    ob.H2vcCodi = !dr.IsDBNull(dr.GetOrdinal("H2VCCODI")) ? dr.GetInt32(dr.GetOrdinal("H2VCCODI")) : 0;
                    ob.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Anio = !dr.IsDBNull(dr.GetOrdinal("ANIO")) ? dr.GetString(dr.GetOrdinal("ANIO")) : "";
                    ob.Mes = !dr.IsDBNull(dr.GetOrdinal("MES")) ? dr.GetString(dr.GetOrdinal("MES")) : "";
                    ob.DemandaEnergia = !dr.IsDBNull(dr.GetOrdinal("DEMANDAENERGIA")) ? dr.GetDecimal(dr.GetOrdinal("DEMANDAENERGIA")) : (decimal?)null;
                    ob.DemandaHP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHP")) ? dr.GetDecimal(dr.GetOrdinal("DEMANDAHP")) : (decimal?)null;
                    ob.DemandaHFP = !dr.IsDBNull(dr.GetOrdinal("DEMANDAHFP")) ? dr.GetDecimal(dr.GetOrdinal("DEMANDAHFP")) : (decimal?)null;
                    ob.GeneracionEnergia = !dr.IsDBNull(dr.GetOrdinal("GENERACIONENERGIA")) ? dr.GetDecimal(dr.GetOrdinal("GENERACIONENERGIA")) : (decimal?)null;
                    ob.GeneracionHP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHP")) ? dr.GetDecimal(dr.GetOrdinal("GENERACIONHP")) : (decimal?)null;
                    ob.GeneracionHFP = !dr.IsDBNull(dr.GetOrdinal("GENERACIONHFP")) ? dr.GetDecimal(dr.GetOrdinal("GENERACIONHFP")) : (decimal?)null;
                    ob.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : "";
                    ob.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.UsuModificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : "";
                    ob.FecModificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : "";
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
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
