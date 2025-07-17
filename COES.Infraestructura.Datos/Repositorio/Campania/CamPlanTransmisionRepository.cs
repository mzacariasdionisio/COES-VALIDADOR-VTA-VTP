using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamPlanTransmisionRepository : RepositoryBase, ICamPlanTransmisionRepository
    {
        public CamPlanTransmisionRepository(string strConn) : base(strConn) { }
        public CamPlanTransmisionHelper Helper = new CamPlanTransmisionHelper();
        public List<PlanTransmisionDTO> GetPlanTransmision()
        {
            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetPlanTransmision);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;
        }

        public bool SavePlanTransmision(PlanTransmisionDTO planTransmision)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SqlSavePlanTransmision);
            dbProvider.AddInParameter(dbcommad, "PLANCODI", DbType.Int32, planTransmision.Plancodi);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, planTransmision.Pericodi);
            dbProvider.AddInParameter(dbcommad, "CODEMPRESA", DbType.String, planTransmision.Codempresa);
            dbProvider.AddInParameter(dbcommad, "NOMEMPRESA", DbType.String, planTransmision.Nomempresa);
            dbProvider.AddInParameter(dbcommad, "NUMREG", DbType.Int32, planTransmision.Numreg);
            dbProvider.AddInParameter(dbcommad, "PLANVERSION", DbType.Int32, planTransmision.Planversion);
            dbProvider.AddInParameter(dbcommad, "PLANESTADO", DbType.String, planTransmision.Planestado);
            dbProvider.AddInParameter(dbcommad, "PLANCUMPLIMIENTO", DbType.String, planTransmision.Plancumplimiento);
            dbProvider.AddInParameter(dbcommad, "VIGENTE", DbType.String, planTransmision.Vigente);
            dbProvider.AddInParameter(dbcommad, "USU_CREACION", DbType.String, planTransmision.Usucreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, planTransmision.IndDel);
            dbProvider.AddInParameter(dbcommad, "CORREOUSU", DbType.String, planTransmision.CorreoUsu);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public bool DeletePlanTransmisionById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeletePlanTransmisionById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastPlanTransmisionId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastPlanTransmisionId);
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

        public PlanTransmisionDTO GetPlanTransmisionById(int id)
        {
            PlanTransmisionDTO ob = new PlanTransmisionDTO();
            DbCommand commandPeriodo = dbProvider.GetSqlStringCommand(Helper.SqlGetPlanTransmisionById);
            dbProvider.AddInParameter(commandPeriodo, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(commandPeriodo, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandPeriodo);
            using (IDataReader dr = dbProvider.ExecuteReader(commandPeriodo))
            {
                if (dr.Read())
                {
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    ob.PeriNombre = !dr.IsDBNull(dr.GetOrdinal("PERINOMBRE")) ? dr.GetString(dr.GetOrdinal("PERINOMBRE")) : string.Empty;
                }
                return ob;
            }
        }

        public bool UpdatePlanTransmision(PlanTransmisionDTO planTransmision)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SqlUpdatePlanTransmision);
            dbProvider.AddInParameter(dbcommad, "CODEMPRESA", DbType.String, planTransmision.Codempresa);
            dbProvider.AddInParameter(dbcommad, "NOMEMPRESA", DbType.String, planTransmision.Nomempresa);
            dbProvider.AddInParameter(dbcommad, "NUMREG", DbType.Int32, planTransmision.Numreg);
            dbProvider.AddInParameter(dbcommad, "PLANVERSION", DbType.Int32, planTransmision.Planversion);
            dbProvider.AddInParameter(dbcommad, "PLANESTADO", DbType.String, planTransmision.Planestado);
            dbProvider.AddInParameter(dbcommad, "PLANCUMPLIMIENTO", DbType.String, planTransmision.Plancumplimiento);
            dbProvider.AddInParameter(dbcommad, "VIGENTE", DbType.String, planTransmision.Vigente);
            dbProvider.AddInParameter(dbcommad, "USU_CREACION", DbType.String, planTransmision.Usucreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, planTransmision.IndDel);
            dbProvider.AddInParameter(dbcommad, "PLANCODI", DbType.Int32, planTransmision.Plancodi);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByFilters(int planTransmision)
        {
            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetPlanTransmisionByFilters);
            dbProvider.AddInParameter(command, "PLANCODI", DbType.Int32, planTransmision);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.PeriNombre = !dr.IsDBNull(dr.GetOrdinal("PERINOMBRE")) ? dr.GetString(dr.GetOrdinal("PERINOMBRE")) : string.Empty;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByEstado(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {
            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            string query = $@"
                SELECT *
                FROM CAM_PLANTRANSMISION
                WHERE IND_DEL = :IND_DEL
                AND (PLANESTADO != :ESTADOEXCL)
                AND (PERICODI = :PERIODO)
                AND (CODEMPRESA IN ({empresa}))
                AND (:ESTADO = 'T' OR PLANESTADO = :ESTADOT)
                AND (:VIGENTE = 'T' OR VIGENTE = :VIGENTET)
                AND (:FUERAPLAZO = 'T' OR PLANCUMPLIMIENTO = :FUERAPLAZO)
                ORDER BY CODEMPRESA, FECENVIO ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            // Añadiendo parámetros al comando
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "ESTADOEXCL", DbType.String, estadoExcl);
            dbProvider.AddInParameter(command, "PERIODO", DbType.Int32, periodo);
            dbProvider.AddInParameter(command, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(command, "ESTADOT", DbType.String, estado);
            dbProvider.AddInParameter(command, "VIGENTE", DbType.String, vigente);
            dbProvider.AddInParameter(command, "VIGENTET", DbType.String, vigente);
            dbProvider.AddInParameter(command, "FUERAPLAZO", DbType.String, fueraplazo);
            dbProvider.AddInParameter(command, "FUERAPLAZOT", DbType.String, fueraplazo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByEstadoEmpresa(string empresa, string estado, string periodo, string vigente, string fueraplazo, string estadoExcl)
        {

            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            // Construir la consulta dinámica
            string query = $@"
                SELECT *
                FROM CAM_PLANTRANSMISION
                WHERE IND_DEL = :IND_DEL
                AND (PLANESTADO != :ESTADOEXCL)
                AND (PERICODI = :PERIODO)
                AND (CODEMPRESA IN ({empresa}))
                AND (:ESTADO = 'T' OR PLANESTADO = :ESTADOT)
                AND (:VIGENTE = 'T' OR VIGENTE = :VIGENTET)
                AND (:FUERAPLAZO = 'T' OR PLANCUMPLIMIENTO = :FUERAPLAZO)
                ORDER BY CODEMPRESA, PLANCODI, PLANVERSION DESC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
           
            // Añadiendo parámetros al comando
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "ESTADOEXCL", DbType.String, estadoExcl);
            dbProvider.AddInParameter(command, "PERIODO", DbType.Int32, periodo);
            dbProvider.AddInParameter(command, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(command, "ESTADOT", DbType.String, estado);
            dbProvider.AddInParameter(command, "VIGENTE", DbType.String, vigente);
            dbProvider.AddInParameter(command, "VIGENTET", DbType.String, vigente);
            dbProvider.AddInParameter(command, "FUERAPLAZO", DbType.String, fueraplazo);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;




        }

        public bool DesactivatePlanById(int id, string vigencia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDesactivatePlanById);
            dbProvider.AddInParameter(command, "VIGENTE", DbType.String, vigencia);
            dbProvider.AddInParameter(command, "IDD", DbType.Int32, id);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public bool ActivatePlanById(int id, string vigencia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlActivatePlanById);
            dbProvider.AddInParameter(command, "VIGENTE", DbType.String, vigencia);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public bool UpdatePlanEstadoById(int id, string planestado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdatePlanEstadoById);
            dbProvider.AddInParameter(command, "PLANESTADO", DbType.String, planestado);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public bool UpdatePlanEstadoEnviarById(int id, string planestado, string correo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdatePlanEstadoEnviarById);
            dbProvider.AddInParameter(command, "PLANESTADO", DbType.String, planestado);
            dbProvider.AddInParameter(command, "FECENVIO", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "FECENVIOP", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "IDFI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IDFF", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IDE", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IDP", DbType.Int32, id);
            dbProvider.AddInParameter(command, "CORREOUSU", DbType.String, correo);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public List<PlanTransmisionDTO> GetPlanTransmisionByEstadoVigente(string empresa, string estado, string periodo, string tipoproyecto, string subtipoproyecto, string observados, string estadoExcl)
        {
            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            // //DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetPlanTransmisionByEstadoVigente);
            if (string.IsNullOrWhiteSpace(empresa)) empresa = "''";
            if (string.IsNullOrWhiteSpace(tipoproyecto)) tipoproyecto = "''";
            if (string.IsNullOrWhiteSpace(subtipoproyecto)) subtipoproyecto = "''";
             string query = $@"
             SELECT PL.*,PR.PROYCODI, PR.PROYNOMBRE, PR.TIPOCODI, PR.TIPOFICODI, PR.PROYESTADO, PR.FECHAENVIOOBS, TP.TIPONOMBRE, TF.TIPOFINOMBRE, 
            ( SELECT COUNT(*) FROM CAM_OBSERVACIONES OBS WHERE OBS.PROYCODI = PR.PROYCODI AND OBS.ESTADO = 'Pendiente' AND OBS.IND_DEL = {Constantes.IndDel}) AS OBSERVPENDIENTE 
            FROM CAM_PLANTRANSMISION PL INNER JOIN CAM_TRNSMPROYECTO PR ON PL.PLANCODI = PR.PLANCODI AND PR.IND_DEL=:IND_DELP INNER JOIN CAM_TIPOPROYECTO TP ON PR.TIPOCODI = TP.TIPOCODI LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON PR.TIPOFICODI = TF.TIPOFICODI WHERE PL.IND_DEL = :IND_DEL AND (PR.PROYESTADO != :ESTADOEXCL) AND (PL.PERICODI = :PERIODO)
			AND (CODEMPRESA IN ({empresa})) 
            AND (PR.TIPOCODI IN ({tipoproyecto}))
            AND ((PR.TIPOCODI = 1 AND PR.TIPOFICODI IN ({subtipoproyecto})) OR PR.TIPOCODI != 1)
			AND (:ESTADO = 'T' OR PR.PROYESTADO = :ESTADOT) ORDER BY PL.CODEMPRESA, PL.PLANCODI, PL.PLANVERSION, PL.FECENVIO DESC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, "IND_DELP", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "ESTADOEXCL", DbType.String, estadoExcl);
            dbProvider.AddInParameter(command, "PERIODO", DbType.Int32, periodo);
            dbProvider.AddInParameter(command, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(command, "ESTADOT", DbType.String, estado);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : string.Empty;
                    ob.Proynombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Proyestado = !dr.IsDBNull(dr.GetOrdinal("PROYESTADO")) ? dr.GetString(dr.GetOrdinal("PROYESTADO")) : string.Empty;
                    ob.FechaenvObs = !dr.IsDBNull(dr.GetOrdinal("FECHAENVIOOBS")) ? dr.GetDateTime(dr.GetOrdinal("FECHAENVIOOBS")) : (DateTime?)null;
                    ob.Tiponombre = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.Tipofinombre = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.ObservPendiente = !dr.IsDBNull(dr.GetOrdinal("OBSERVPENDIENTE")) ? dr.GetInt32(dr.GetOrdinal("OBSERVPENDIENTE")) : 0;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;
        }

         public List<PlanTransmisionDTO> GetPlanTransmisionByVigente(string empresa, string estado, string periodo, string estadoExcl)
        {
            List<PlanTransmisionDTO> PlanTransmisionDTOs = new List<PlanTransmisionDTO>();
            //DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetPlanTransmisionByVigente);

            int periodoInt;
            if (!int.TryParse(periodo, out periodoInt))
            {
            }
            if (string.IsNullOrWhiteSpace(empresa)) empresa = "''";
             string query = $@"
             SELECT * 
			FROM CAM_PLANTRANSMISION 
			WHERE IND_DEL = :IND_DEL 
			AND (PLANESTADO != :ESTADOEXCL)
			AND (PERICODI = :PERIODO)
			AND (VIGENTE = :VIGENTE)
			AND (CODEMPRESA IN ({empresa}))
			AND (:ESTADO = 'T' OR PLANESTADO = :ESTADOT)
			ORDER BY CODEMPRESA, PLANCODI, PLANVERSION, FECENVIO DESC";

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            // Añadiendo parámetros al comando
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "ESTADOEXCL", DbType.String, estadoExcl);
            dbProvider.AddInParameter(command, "PERIODO", DbType.Int32, periodoInt);
            dbProvider.AddInParameter(command, "VIGENTE", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "ESTADO", DbType.String, estado);
            dbProvider.AddInParameter(command, "ESTADOT", DbType.String, estado);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PlanTransmisionDTO ob = new PlanTransmisionDTO();
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Codempresa = !dr.IsDBNull(dr.GetOrdinal("CODEMPRESA")) ? dr.GetString(dr.GetOrdinal("CODEMPRESA")) : string.Empty;
                    ob.Nomempresa = !dr.IsDBNull(dr.GetOrdinal("NOMEMPRESA")) ? dr.GetString(dr.GetOrdinal("NOMEMPRESA")) : string.Empty;
                    ob.Fecenvio = !dr.IsDBNull(dr.GetOrdinal("FECENVIO")) ? dr.GetDateTime(dr.GetOrdinal("FECENVIO")) : (DateTime?)null;
                    ob.Numreg = !dr.IsDBNull(dr.GetOrdinal("NUMREG")) ? dr.GetInt32(dr.GetOrdinal("NUMREG")) : 0;
                    ob.Planversion = !dr.IsDBNull(dr.GetOrdinal("PLANVERSION")) ? dr.GetInt32(dr.GetOrdinal("PLANVERSION")) : 0;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.Plancumplimiento = !dr.IsDBNull(dr.GetOrdinal("PLANCUMPLIMIENTO")) ? dr.GetString(dr.GetOrdinal("PLANCUMPLIMIENTO")) : string.Empty;
                    ob.Vigente = !dr.IsDBNull(dr.GetOrdinal("VIGENTE")) ? dr.GetString(dr.GetOrdinal("VIGENTE")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                    PlanTransmisionDTOs.Add(ob);
                }
            }

            return PlanTransmisionDTOs;
        }

        public bool UpdateProyRegById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdateProyRegById);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "IDD", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }


    }
}