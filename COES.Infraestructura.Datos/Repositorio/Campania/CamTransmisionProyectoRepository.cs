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
    public class CamTransmisionProyectoRepository : RepositoryBase, ICamTransmisionProyectoRepository
    {

        public CamTransmisionProyectoRepository(string strConn) : base(strConn) { }

        public CamTransmisionProyectoHelper Helper = new CamTransmisionProyectoHelper();
        public List<TransmisionProyectoDTO> GetTransmisionProyecto(int id)
        {
            List<TransmisionProyectoDTO> transmisionProyectoDTOs = new List<TransmisionProyectoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetTransmisionProyecto);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "PLANCODI", DbType.Int32, id);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransmisionProyectoDTO ob = new TransmisionProyectoDTO();
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Tipocodi = !dr.IsDBNull(dr.GetOrdinal("TIPOCODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOCODI")) : 0;
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.TipoNombre = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.EmpresaCodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESACODI")) ? dr.GetString(dr.GetOrdinal("EMPRESACODI")) : string.Empty;
                    ob.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.Proynombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Proydescripcion = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.Proyconfidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Tipoficodi = !dr.IsDBNull(dr.GetOrdinal("TIPOFICODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOFICODI")) : (int?)null;
                    ob.Areademanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetInt32(dr.GetOrdinal("AREADEMANDA")) : 0;
                    ob.Proyestado = !dr.IsDBNull(dr.GetOrdinal("PROYESTADO")) ? dr.GetString(dr.GetOrdinal("PROYESTADO")) : string.Empty;
                    ob.TipofiNom = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    transmisionProyectoDTOs.Add(ob);
                }
            }

            return transmisionProyectoDTOs;
        }

        public bool SaveTransmisionProyecto(TransmisionProyectoDTO transmisionProy)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SqlSaveTransmisionProyecto);
            dbProvider.AddInParameter(dbcommad, "PROYCODI", DbType.Int32, transmisionProy.Proycodi);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, transmisionProy.Pericodi);
            dbProvider.AddInParameter(dbcommad, "TIPOCODI", DbType.Int32, transmisionProy.Tipocodi);
            dbProvider.AddInParameter(dbcommad, "PLANCODI", DbType.Int32, transmisionProy.Plancodi);
            dbProvider.AddInParameter(dbcommad, "EMPRESACODI", DbType.String, transmisionProy.EmpresaCodi);
            dbProvider.AddInParameter(dbcommad, "EMPRESANOM", DbType.String, transmisionProy.EmpresaNom);
            dbProvider.AddInParameter(dbcommad, "PROYNOMBRE", DbType.String, transmisionProy.Proynombre);
            dbProvider.AddInParameter(dbcommad, "PROYDESCRIPCION", DbType.String, transmisionProy.Proydescripcion);
            dbProvider.AddInParameter(dbcommad, "PROYCONFIDENCIAL", DbType.String, transmisionProy.Proyconfidencial);
            dbProvider.AddInParameter(dbcommad, "USU_CREACION", DbType.String, transmisionProy.Usucreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, transmisionProy.IndDel);
            dbProvider.AddInParameter(dbcommad, "TIPOFICODI", DbType.Int32, transmisionProy.Tipoficodi);
            dbProvider.AddInParameter(dbcommad, "AREADEMANDA", DbType.Int32, transmisionProy.Areademanda);
            dbProvider.AddInParameter(dbcommad, "PROYESTADO", DbType.String, transmisionProy.Proyestado);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public bool DeleteTransmisionProyectoById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteTransmisionProyectoById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastTransmisionProyectoId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastTransmisionProyectoId);
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

        public TransmisionProyectoDTO GetTransmisionProyectoById(int id)
        {
            TransmisionProyectoDTO ob = new TransmisionProyectoDTO();
            DbCommand commandPeriodo = dbProvider.GetSqlStringCommand(Helper.SqlGetTransmisionProyectoById);
            dbProvider.AddInParameter(commandPeriodo, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(commandPeriodo, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(commandPeriodo);
            using (IDataReader dr = dbProvider.ExecuteReader(commandPeriodo))
            {
                if (dr.Read())
                {
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.NomPeri = !dr.IsDBNull(dr.GetOrdinal("PERINOMBRE")) ? dr.GetString(dr.GetOrdinal("PERINOMBRE")) : string.Empty;
                    ob.Tipocodi = !dr.IsDBNull(dr.GetOrdinal("TIPOCODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOCODI")) : 0;
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.EmpresaCodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESACODI")) ? dr.GetString(dr.GetOrdinal("EMPRESACODI")) : string.Empty;
                    ob.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.Proynombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Proydescripcion = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.Proyconfidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Tipoficodi = !dr.IsDBNull(dr.GetOrdinal("TIPOFICODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOFICODI")) : 0;
                    ob.Areademanda = !dr.IsDBNull(dr.GetOrdinal("AREADEMANDA")) ? dr.GetInt32(dr.GetOrdinal("AREADEMANDA")) : 0;
                    ob.Proyestado = !dr.IsDBNull(dr.GetOrdinal("PROYESTADO")) ? dr.GetString(dr.GetOrdinal("PROYESTADO")) : string.Empty;
                    ob.CorreoUsu = !dr.IsDBNull(dr.GetOrdinal("CORREOUSU")) ? dr.GetString(dr.GetOrdinal("CORREOUSU")) : string.Empty;
                }
            }
            return ob;
        }

        public bool UpdateTransmisionProyecto(TransmisionProyectoDTO transmisionProy)
        {
            DbCommand dbcommad = dbProvider.GetSqlStringCommand(Helper.SqlUpdateTransmisionProyecto);
            dbProvider.AddInParameter(dbcommad, "PERICODI", DbType.Int32, transmisionProy.Pericodi);
            dbProvider.AddInParameter(dbcommad, "TIPOCODI", DbType.Int32, transmisionProy.Tipocodi);
            dbProvider.AddInParameter(dbcommad, "PLANCODI", DbType.Int32, transmisionProy.Plancodi);
            dbProvider.AddInParameter(dbcommad, "PROYNOMBRE", DbType.String, transmisionProy.Proynombre);
            dbProvider.AddInParameter(dbcommad, "EMPRESACODI", DbType.String, transmisionProy.EmpresaCodi);
            dbProvider.AddInParameter(dbcommad, "EMPRESANOM", DbType.String, transmisionProy.EmpresaNom);
            dbProvider.AddInParameter(dbcommad, "PROYDESCRIPCION", DbType.String, transmisionProy.Proydescripcion);
            dbProvider.AddInParameter(dbcommad, "PROYCONFIDENCIAL", DbType.String, transmisionProy.Proyconfidencial);
            dbProvider.AddInParameter(dbcommad, "USU_MODIFICACION", DbType.String, transmisionProy.Usucreacion);
            dbProvider.AddInParameter(dbcommad, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbcommad, "IND_DEL", DbType.String, transmisionProy.IndDel);
            dbProvider.AddInParameter(dbcommad, "PROYCODI", DbType.Int32, transmisionProy.Proycodi);
            dbProvider.AddInParameter(dbcommad, "TIPOFICODI", DbType.Int32, transmisionProy.Tipoficodi);
            dbProvider.AddInParameter(dbcommad, "AREADEMANDA", DbType.Int32, transmisionProy.Areademanda);
            dbProvider.AddInParameter(dbcommad, "PROYESTADO", DbType.Int32, transmisionProy.Proyestado);
            dbProvider.ExecuteNonQuery(dbcommad);
            return true;
        }

        public List<TransmisionProyectoDTO> GetTransmisionProyectoByPeriodo(int id)
        {
            List<TransmisionProyectoDTO> transmisionProyectoDTOs = new List<TransmisionProyectoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetTransmisionProyectoByPeriodo);
            dbProvider.AddInParameter(command, "PERICODI", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransmisionProyectoDTO ob = new TransmisionProyectoDTO();
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Tipocodi = !dr.IsDBNull(dr.GetOrdinal("TIPOCODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOCODI")) : 0;
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.EmpresaCodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESACODI")) ? dr.GetString(dr.GetOrdinal("EMPRESACODI")) : string.Empty;
                    ob.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.Proynombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Proydescripcion = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.Proyconfidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    transmisionProyectoDTOs.Add(ob);
                }
            }

            return transmisionProyectoDTOs;
        }

        public bool UpdateProyEstadoById(int id, string proyestado)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdateProyEstadoById);
            dbProvider.AddInParameter(command, "PROYESTADO", DbType.String, proyestado);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public bool UpdateProyEstadoByIdProy(int id, string proyestado, string proyestadoini)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdateProyEstadoByIdProy);
            dbProvider.AddInParameter(command, "IDP", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DELP", DbType.String, Constantes.IndDel);
            dbProvider.AddInParameter(command, "PROYESTADO", DbType.String, proyestado);
            dbProvider.AddInParameter(command, "PROYESTADOINI", DbType.String, proyestadoini);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

         public bool UpdateProyFechaEnvioObsById(int id)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlUpdateProyFechaEnvioObsById);
            dbProvider.AddInParameter(command, "FECHAENVIOOBS", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public List<TransmisionProyectoDTO> GetTransmisionProyectoByPeriodoFilter(string pericodi, string empresa, string estado)
        {
            List<TransmisionProyectoDTO> transmisionProyectoDTOs = new List<TransmisionProyectoDTO>();
            string query = $@"
                SELECT TR.*, PL.PLANESTADO, TP.TIPONOMBRE, TF.TIPOFINOMBRE FROM CAM_TRNSMPROYECTO TR 
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({pericodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                TR.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, TR.PROYCODI,PL.CODEMPRESA ASC";
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TransmisionProyectoDTO ob = new TransmisionProyectoDTO();
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Pericodi = !dr.IsDBNull(dr.GetOrdinal("PERICODI")) ? dr.GetInt32(dr.GetOrdinal("PERICODI")) : 0;
                    ob.Tipocodi = !dr.IsDBNull(dr.GetOrdinal("TIPOCODI")) ? dr.GetInt32(dr.GetOrdinal("TIPOCODI")) : 0;
                    ob.Plancodi = !dr.IsDBNull(dr.GetOrdinal("PLANCODI")) ? dr.GetInt32(dr.GetOrdinal("PLANCODI")) : 0;
                    ob.EmpresaCodi = !dr.IsDBNull(dr.GetOrdinal("EMPRESACODI")) ? dr.GetString(dr.GetOrdinal("EMPRESACODI")) : string.Empty;
                    ob.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.Proynombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.Proydescripcion = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.Proyconfidencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : string.Empty;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Planestado = !dr.IsDBNull(dr.GetOrdinal("PLANESTADO")) ? dr.GetString(dr.GetOrdinal("PLANESTADO")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.TipoSubProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    transmisionProyectoDTOs.Add(ob);
                }
            }

            return transmisionProyectoDTOs;
        }

    }
}
