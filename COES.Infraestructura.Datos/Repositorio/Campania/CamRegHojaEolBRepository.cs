using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaEolBRepository : RepositoryBase, ICamRegHojaEolBRepository
    {
        public CamRegHojaEolBRepository(string strConn) : base(strConn) { }
        CamRegHojaEolBHelper Helper = new CamRegHojaEolBHelper();
        public List<RegHojaEolBDTO> GetRegHojaEolBCodi(int proyCodi)
        {
            List<RegHojaEolBDTO> regHojaEolBDTOs = new List<RegHojaEolBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolBCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaEolBDTO ob = new RegHojaEolBDTO();

                    ob.CentralBCodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALBCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALBCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;

                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;

                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;

                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;

                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;

                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;

                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    regHojaEolBDTOs.Add(ob);
                }
            }

            return regHojaEolBDTOs;
        }

        public bool SaveRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaEolB);

            dbProvider.AddInParameter(dbCommand, "CENTRALBCODI", DbType.Int32, regHojaEolBDTO.CentralBCodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaEolBDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, regHojaEolBDTO.Estudiofactibilidad ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, regHojaEolBDTO.Investigacionescampo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, regHojaEolBDTO.Gestionesfinancieras ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, regHojaEolBDTO.Disenospermisos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, regHojaEolBDTO.Obrasciviles ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, regHojaEolBDTO.Equipamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, regHojaEolBDTO.Lineatransmision ?? (object)DBNull.Value);

            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, regHojaEolBDTO.Administracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, regHojaEolBDTO.Aduanas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, regHojaEolBDTO.Supervision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, regHojaEolBDTO.Gastosgestion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, regHojaEolBDTO.Imprevistos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, regHojaEolBDTO.Igv ?? (object)DBNull.Value);

            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, regHojaEolBDTO.Otrosgastos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, regHojaEolBDTO.Inversiontotalsinigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, regHojaEolBDTO.Inversiontotalconigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Financiamientotipo) ? (object)DBNull.Value : regHojaEolBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Financiamientoestado) ? (object)DBNull.Value : regHojaEolBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, regHojaEolBDTO.Porcentajefinanciado ?? (object)DBNull.Value);

            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Concesiondefinitiva) ? (object)DBNull.Value : regHojaEolBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Ventaenergia) ? (object)DBNull.Value : regHojaEolBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Ejecucionobra) ? (object)DBNull.Value : regHojaEolBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Contratosfinancieros) ? (object)DBNull.Value : regHojaEolBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Observaciones) ? (object)DBNull.Value : regHojaEolBDTO.Observaciones);

            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, string.IsNullOrEmpty(regHojaEolBDTO.Usucreacion) ? (object)DBNull.Value : regHojaEolBDTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }

        public bool DeleteRegHojaEolBById(int id, string usuario)
            {
                DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaEolBById);
                dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
                dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
                dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
                dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
                dbProvider.ExecuteNonQuery(command);
                return true;
            }
        public int GetLastRegHojaEolBId()
            {
                int count = 0;
                DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaEolBId);
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
        public RegHojaEolBDTO GetRegHojaEolBById(int id)
        {
            RegHojaEolBDTO ob = new RegHojaEolBDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaEolBById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.CentralBCodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALBCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALBCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;

                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;

                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;

                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;

                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;

                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                }
            }

            return ob;
        }

        public bool UpdateRegHojaEolB(RegHojaEolBDTO regHojaEolBDTO)
            {
                DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaEolB);

                dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaEolBDTO.Proycodi);
                dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Int32, regHojaEolBDTO.Estudiofactibilidad);
                dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, regHojaEolBDTO.Investigacionescampo);
                dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, regHojaEolBDTO.Gestionesfinancieras);
                dbProvider.AddInParameter(dbCommand, "DISEÑOSPERMISOS", DbType.Decimal, regHojaEolBDTO.Disenospermisos);
                dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, regHojaEolBDTO.Obrasciviles);
                dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, regHojaEolBDTO.Equipamiento);
                dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, regHojaEolBDTO.Lineatransmision);

                dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, regHojaEolBDTO.Administracion);
                dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, regHojaEolBDTO.Aduanas);
                dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, regHojaEolBDTO.Supervision);
                dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, regHojaEolBDTO.Gastosgestion);
                dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, regHojaEolBDTO.Imprevistos);
                dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, regHojaEolBDTO.Igv);

                dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, regHojaEolBDTO.Otrosgastos);
                dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, regHojaEolBDTO.Inversiontotalsinigv);
                dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, regHojaEolBDTO.Inversiontotalconigv);
                dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, regHojaEolBDTO.Financiamientotipo);
                dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, regHojaEolBDTO.Financiamientoestado);
                dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, regHojaEolBDTO.Porcentajefinanciado);
                dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.DateTime, regHojaEolBDTO.Concesiondefinitiva);
                dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.DateTime, regHojaEolBDTO.Ventaenergia);
                dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.DateTime, regHojaEolBDTO.Ejecucionobra);
                dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.DateTime, regHojaEolBDTO.Contratosfinancieros);
                dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, regHojaEolBDTO.Observaciones);
                dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaEolBDTO.Usumodificacion);
                dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
                dbProvider.AddInParameter(dbCommand, "CENTRALBCODI", DbType.Int32, regHojaEolBDTO.CentralBCodi);
                
                dbProvider.ExecuteNonQuery(dbCommand);

                return true;
            }

        public List<RegHojaEolBDTO> GetRegHojaEolBByFilter(string plancodi, string empresa, string estado)
        {
            List<RegHojaEolBDTO> oblist = new List<RegHojaEolBDTO>();
            string query = $@"
                SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TP.TIPONOMBRE, TF.TIPOFINOMBRE  FROM CAM_CENEOLIHOJAB CGB
                INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({plancodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.CENTRALBCODI ASC";
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaEolBDTO ob = new RegHojaEolBDTO();
                    ob.CentralBCodi = !dr.IsDBNull(dr.GetOrdinal("CENTRALBCODI")) ? dr.GetInt32(dr.GetOrdinal("CENTRALBCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : null;

                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("IGV")) : null;

                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : null;

                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? (decimal?)dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : null;

                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;

                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.Usumodificacion = !dr.IsDBNull(dr.GetOrdinal("USU_MODIFICACION")) ? dr.GetString(dr.GetOrdinal("USU_MODIFICACION")) : string.Empty;
                    ob.Fechamodificacion = !dr.IsDBNull(dr.GetOrdinal("FEC_MODIFICACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_MODIFICACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;

                    ob.Empresa = dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? null : dr.GetString(dr.GetOrdinal("EMPRESANOM"));
                    ob.NombreProyecto = dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? null : dr.GetString(dr.GetOrdinal("PROYNOMBRE"));
                    ob.TipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPONOMBRE"));
                    ob.SubTipoProyecto = dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? null : dr.GetString(dr.GetOrdinal("TIPOFINOMBRE"));
                    oblist.Add(ob);
                }
            }

            return oblist;
        }

    }

}
