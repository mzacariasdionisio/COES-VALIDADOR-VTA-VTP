using COES.Base.Core;
using COES.Dominio.DTO.Campania;
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
    public class CamT2SubestFicha1Repository : RepositoryBase, ICamT2SubestFicha1Repository
    {
        public CamT2SubestFicha1Repository(string strConn) : base(strConn) { }

       CamT2SubestFicha1Helper Helper = new CamT2SubestFicha1Helper();

        public List<T2SubestFicha1DTO> GetT2SubestFicha1(int idcodi)
        {
            List<T2SubestFicha1DTO> T2SubestFicha1DTOs = new List<T2SubestFicha1DTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetT2SubestFicha1);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, idcodi);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    T2SubestFicha1DTO dto = new T2SubestFicha1DTO();
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : "";
                    dto.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : "";
                    dto.FechaPuestaServicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetString(dr.GetOrdinal("FECHAPUESTASERVICIO")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : "";
                    dto.SistemaBarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : null;
                    dto.OtroSistemaBarras = !dr.IsDBNull(dr.GetOrdinal("OTROSISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("OTROSISTEMABARRAS")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    dto.NumEquipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                   
                    T2SubestFicha1DTOs.Add(dto);
                }
            }

            return T2SubestFicha1DTOs;
        }

        public bool SaveT2SubestFicha1(T2SubestFicha1DTO T2SubestFicha1DTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveT2SubestFicha1);
            dbProvider.AddInParameter(dbCommand, "SUBESTFICHA1CODI", DbType.Int32, T2SubestFicha1DTO.SubestFicha1Codi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, T2SubestFicha1DTO.ProyCodi);
            dbProvider.AddInParameter(dbCommand, "NOMBRESUBESTACION", DbType.String, T2SubestFicha1DTO.NombreSubestacion);
            dbProvider.AddInParameter(dbCommand, "TIPOPROYECTO", DbType.String, T2SubestFicha1DTO.TipoProyecto);
            dbProvider.AddInParameter(dbCommand, "FECHAPUESTASERVICIO", DbType.String, T2SubestFicha1DTO.FechaPuestaServicio);
            dbProvider.AddInParameter(dbCommand, "EMPRESAPROPIETARIA", DbType.String, T2SubestFicha1DTO.EmpresaPropietaria);
            dbProvider.AddInParameter(dbCommand, "SISTEMABARRAS", DbType.String, T2SubestFicha1DTO.SistemaBarras);
            dbProvider.AddInParameter(dbCommand, "OTROSISTEMABARRAS", DbType.String, T2SubestFicha1DTO.OtroSistemaBarras);
            dbProvider.AddInParameter(dbCommand, "NUMTRAFOS", DbType.Int32, T2SubestFicha1DTO.NumTrafo);
            dbProvider.AddInParameter(dbCommand, "NUMEQUIPOS", DbType.Int32, T2SubestFicha1DTO.NumEquipos);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, T2SubestFicha1DTO.UsuCreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, T2SubestFicha1DTO.FecCreacion);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, T2SubestFicha1DTO.IndDel);
            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteT2SubestFicha1ById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteT2SubestFicha1ById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);

            return true;
        }

        public int GetLastT2SubestFicha1Id()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastT2SubestFicha1Id);
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

        public T2SubestFicha1DTO GetT2SubestFicha1ById(int id)
        {
            T2SubestFicha1DTO dto = new T2SubestFicha1DTO();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetT2SubestFicha1ById);
            dbProvider.AddInParameter(command, "ID", DbType.Int32, id);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : "";
                    dto.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : "";
                    dto.FechaPuestaServicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetString(dr.GetOrdinal("FECHAPUESTASERVICIO")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : "";
                    dto.SistemaBarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : null;
                    dto.OtroSistemaBarras = !dr.IsDBNull(dr.GetOrdinal("OTROSISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("OTROSISTEMABARRAS")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    dto.NumEquipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                }
            }

            return dto;
        }

        public List<T2SubestFicha1DTO> GetT2SubFicha1ByFilter(string plancodi, string empresa, string estado)
        {
            List<T2SubestFicha1DTO> oblist = new List<T2SubestFicha1DTO>();
            string query = $@"
                SELECT SUB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,TR.PROYCONFIDENCIAL  FROM CAM_T2SUBESTFICHA1 SUB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = SUB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                SUB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, SUB.PROYCODI,PL.CODEMPRESA, SUB.SUBESTFICHA1CODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    T2SubestFicha1DTO dto = new T2SubestFicha1DTO();
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : "";
                    dto.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOPROYECTO")) ? dr.GetString(dr.GetOrdinal("TIPOPROYECTO")) : "";
                    dto.FechaPuestaServicio = !dr.IsDBNull(dr.GetOrdinal("FECHAPUESTASERVICIO")) ? dr.GetString(dr.GetOrdinal("FECHAPUESTASERVICIO")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : "";
                    dto.SistemaBarras = !dr.IsDBNull(dr.GetOrdinal("SISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("SISTEMABARRAS")) : null;
                    dto.OtroSistemaBarras = !dr.IsDBNull(dr.GetOrdinal("OTROSISTEMABARRAS")) ? dr.GetString(dr.GetOrdinal("OTROSISTEMABARRAS")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFOS")) ? dr.GetInt32(dr.GetOrdinal("NUMTRAFOS")) : 0;
                    dto.NumEquipos = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPOS")) ? dr.GetInt32(dr.GetOrdinal("NUMEQUIPOS")) : 0;
                    dto.UsuCreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : null;
                    dto.FecCreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.Now;
                    dto.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : null;
                    dto.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    dto.Confidencial = dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? null : dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL"));
                   
                    oblist.Add(dto);
                }
            }

            return oblist;
        }

        public List<T2SubestFicha1TransDTO> GetT2SubFicha1TransByFilter(string plancodi, string empresa, string estado)
        {
            List<T2SubestFicha1TransDTO> oblist = new List<T2SubestFicha1TransDTO>();
            string query = $@"
            SELECT SUBESTFICHA1CODI, PROYCODI, NOMBRESUBESTACION, EMPRESAPROPIETARIA, NUMTRAFO, 
            RTRIM(XMLAGG(XMLELEMENT(e, SUBDET.DATACATCODI || ', ')).EXTRACT('//text()'), ', ') AS DATACATCODIGROUP,
            RTRIM(XMLAGG(XMLELEMENT(e, SUBDET.VALORTRAFO || ', ')).EXTRACT('//text()'), ', ') AS VALORGROUP,
            EMPRESANOM, PROYNOMBRE, PROYDESCRIPCION, PROYCONFIDENCIAL
            FROM (
            SELECT SUB.SUBESTFICHA1CODI, SUB.PROYCODI, SUB.NOMBRESUBESTACION, SUB.EMPRESAPROPIETARIA, TO_CHAR(SUBDET.NUMTRAFO) AS NUMTRAFO,  SUBDET.DATACATCODI, SUBDET.VALORTRAFO, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TR.PERICODI,PL.CODEMPRESA, TR.PROYCONFIDENCIAL
            FROM CAM_T2SUBESTFICHA1 SUB 
            INNER JOIN CAM_T2SUBESTFICHA1DET1 SUBDET ON SUB.SUBESTFICHA1CODI = SUBDET.SUBESTFICHA1CODI
            INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = SUB.PROYCODI
            INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
            WHERE TR.PERICODI IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa})  AND SUB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
            UNION ALL
            SELECT SUB.SUBESTFICHA1CODI, SUB.PROYCODI, SUB.NOMBRESUBESTACION, SUB.EMPRESAPROPIETARIA, TO_CHAR(SUBDET.NUMTRAFO) AS NUMTRAFO, SUBDET.DATACATCODI, SUBDET.VALORTRAFO, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,TR.PERICODI,PL.CODEMPRESA, TR.PROYCONFIDENCIAL
            FROM CAM_T2SUBESTFICHA1 SUB 
            INNER JOIN CAM_T2SUBESTFICHA1DET2 SUBDET ON SUB.SUBESTFICHA1CODI = SUBDET.SUBESTFICHA1CODI
            INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = SUB.PROYCODI
            INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
            WHERE TR.PERICODI IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa})  AND SUB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
            ) SUBDET
            GROUP BY SUBESTFICHA1CODI, PROYCODI, NOMBRESUBESTACION, EMPRESAPROPIETARIA, NUMTRAFO, EMPRESANOM, PROYNOMBRE, PROYDESCRIPCION, PROYCONFIDENCIAL,PERICODI,CODEMPRESA
            ORDER BY PERICODI, PROYCODI, CODEMPRESA, SUBESTFICHA1CODI, NUMTRAFO";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    T2SubestFicha1TransDTO dto = new T2SubestFicha1TransDTO();
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.DataCatCodiGroup = !dr.IsDBNull(dr.GetOrdinal("DATACATCODIGROUP")) ? dr.GetString(dr.GetOrdinal("DATACATCODIGROUP")) : null;
                    dto.NumTrafo = !dr.IsDBNull(dr.GetOrdinal("NUMTRAFO")) ? dr.GetString(dr.GetOrdinal("NUMTRAFO")) : null;
                    dto.ValorGroup = !dr.IsDBNull(dr.GetOrdinal("VALORGROUP")) ? dr.GetString(dr.GetOrdinal("VALORGROUP")) : null;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : null;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : null;
                    dto.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : null;
                    dto.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : null;
                    oblist.Add(dto);
                }
            }

            return oblist;
        }

        public List<T2SubestFicha1EquiDTO> GetT2SubFicha1EquiByFilter(string plancodi, string empresa, string estado)
        {
            List<T2SubestFicha1EquiDTO> oblist = new List<T2SubestFicha1EquiDTO>();
            string query = $@"
            SELECT SUB.SUBESTFICHA1CODI, SUB.PROYCODI, SUB.NOMBRESUBESTACION, SUB.EMPRESAPROPIETARIA,SUBDET.NUMEQUIPO, 
            RTRIM(XMLAGG(XMLELEMENT(e, SUBDET.DATACATCODI || ', ')).EXTRACT('//text()'), ', ') AS DATACATCODIGROUP,
            RTRIM(XMLAGG(XMLELEMENT(e, SUBDET.VALOREQUIPO || ', ')).EXTRACT('//text()'), ', ') AS VALORGROUP,
            TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,TR.PROYCONFIDENCIAL
            FROM CAM_T2SUBESTFICHA1 SUB 
            INNER JOIN CAM_T2SUBESTFICHA1DET3 SUBDET ON SUB.SUBESTFICHA1CODI = SUBDET.SUBESTFICHA1CODI
            INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = SUB.PROYCODI
            INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
            WHERE TR.PERICODI  IN ({plancodi}) AND PL.CODEMPRESA IN ({empresa})  AND SUB.IND_DEL = 0 AND PL.PLANESTADO ='{estado}'
            GROUP BY TR.PERICODI, PL.CODEMPRESA, SUB.SUBESTFICHA1CODI, SUB.PROYCODI, SUB.NOMBRESUBESTACION, SUB.EMPRESAPROPIETARIA, SUBDET.NUMEQUIPO, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION,TR.PROYCONFIDENCIAL
            ORDER BY TR.PERICODI, SUB.PROYCODI,PL.CODEMPRESA, SUB.SUBESTFICHA1CODI, SUBDET.NUMEQUIPO ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);
            dbProvider.ExecuteNonQuery(commandHoja);
            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    T2SubestFicha1EquiDTO dto = new T2SubestFicha1EquiDTO();
                    dto.SubestFicha1Codi = !dr.IsDBNull(dr.GetOrdinal("SUBESTFICHA1CODI")) ? dr.GetInt32(dr.GetOrdinal("SUBESTFICHA1CODI")) : 0;
                    dto.DataCatCodiGroup = !dr.IsDBNull(dr.GetOrdinal("DATACATCODIGROUP")) ? dr.GetString(dr.GetOrdinal("DATACATCODIGROUP")) : null;
                    dto.NumEquipo = !dr.IsDBNull(dr.GetOrdinal("NUMEQUIPO")) ? dr.GetString(dr.GetOrdinal("NUMEQUIPO")) : null;
                    dto.ValorGroup = !dr.IsDBNull(dr.GetOrdinal("VALORGROUP")) ? dr.GetString(dr.GetOrdinal("VALORGROUP")) : null;
                    dto.ProyCodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetString(dr.GetOrdinal("PROYCODI")) : null;
                    dto.NombreSubestacion = !dr.IsDBNull(dr.GetOrdinal("NOMBRESUBESTACION")) ? dr.GetString(dr.GetOrdinal("NOMBRESUBESTACION")) : null;
                    dto.EmpresaPropietaria = !dr.IsDBNull(dr.GetOrdinal("EMPRESAPROPIETARIA")) ? dr.GetString(dr.GetOrdinal("EMPRESAPROPIETARIA")) : null;
                    dto.EmpresaNom = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : null;
                    dto.ProyNombre = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : null;
                    oblist.Add(dto);
                }
            }

            return oblist;
        }
    }
}

