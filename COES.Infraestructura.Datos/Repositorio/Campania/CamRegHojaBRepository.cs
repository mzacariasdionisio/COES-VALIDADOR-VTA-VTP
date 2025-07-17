using COES.Base.Core;
using COES.Infraestructura.Datos.Helper;
using COES.Dominio.DTO.Campania;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using COES.Infraestructura.Datos.Helper.Campania;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamRegHojaBRepository : RepositoryBase, ICamRegHojaBRepository
    {
        public CamRegHojaBRepository(string strConn) : base(strConn) { }

        CamRegHojaBHelper Helper = new CamRegHojaBHelper();

        public List<RegHojaBDTO> GetRegHojaBProyCodi(int proyCodi)
        {
            List<RegHojaBDTO> regHojaBDTOs = new List<RegHojaBDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaBProyCodi);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, proyCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    RegHojaBDTO ob = new RegHojaBDTO();

                    ob.Fichabcodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : (decimal?)null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : (decimal?)null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : (decimal?)null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : (decimal?)null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : (decimal?)null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : (decimal?)null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : (decimal?)null;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : (decimal?)null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : (decimal?)null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : (decimal?)null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : (decimal?)null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : (decimal?)null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : (decimal?)null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : (decimal?)null;
                    ob.Usoagua = !dr.IsDBNull(dr.GetOrdinal("USOAGUA")) ? dr.GetDecimal(dr.GetOrdinal("USOAGUA")) : (decimal?)null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : (decimal?)null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : (decimal?)null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : (decimal?)null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : (decimal?)null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    regHojaBDTOs.Add(ob);
                }
            }

            return regHojaBDTOs;
        }

        public bool SaveRegHojaB(RegHojaBDTO regHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlSaveRegHojaB);

            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.Int32, regHojaBDTO.Fichabcodi);
            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaBDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Decimal, regHojaBDTO.Estudiofactibilidad ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Decimal, regHojaBDTO.Investigacionescampo ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Decimal, regHojaBDTO.Gestionesfinancieras ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "DISENOSPERMISOS", DbType.Decimal, regHojaBDTO.Disenospermisos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Decimal, regHojaBDTO.Obrasciviles ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Decimal, regHojaBDTO.Equipamiento ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Decimal, regHojaBDTO.Lineatransmision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OBRASREGULACION", DbType.Decimal, regHojaBDTO.Obrasregulacion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Decimal, regHojaBDTO.Administracion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Decimal, regHojaBDTO.Aduanas ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Decimal, regHojaBDTO.Supervision ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Decimal, regHojaBDTO.Gastosgestion ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Decimal, regHojaBDTO.Imprevistos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Decimal, regHojaBDTO.Igv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "USOAGUA", DbType.Decimal, regHojaBDTO.Usoagua ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Decimal, regHojaBDTO.Otrosgastos ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Decimal, regHojaBDTO.Inversiontotalsinigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Decimal, regHojaBDTO.Inversiontotalconigv ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, regHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, regHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Decimal, regHojaBDTO.Porcentajefinanciado ?? (object)DBNull.Value);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.String, regHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.String, regHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.String, regHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.String, regHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, regHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_CREACION", DbType.String, regHojaBDTO.Usucreacion);
            dbProvider.AddInParameter(dbCommand, "FEC_CREACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "IND_DEL", DbType.String, Constantes.IndDel);

            dbProvider.ExecuteNonQuery(dbCommand);

            return true;
        }


        public bool DeleteRegHojaBById(int id, string usuario)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDeleteRegHojaBById);
            dbProvider.AddInParameter(command, "IND_DEL", DbType.String, Constantes.IndDelEliminado);
            dbProvider.AddInParameter(command, "USU_MODIFICACION", DbType.String, usuario);
            dbProvider.AddInParameter(command, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, "PROYCODI", DbType.Int32, id);
            dbProvider.ExecuteNonQuery(command);
            return true;
        }

        public int GetLastRegHojaBId()
        {
            int count = 0;
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetLastRegHojaBId);
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

        public RegHojaBDTO GetRegHojaBById(int id)
        {
            RegHojaBDTO ob = new RegHojaBDTO();
            DbCommand commandHoja = dbProvider.GetSqlStringCommand(Helper.SqlGetRegHojaBById);
            dbProvider.AddInParameter(commandHoja, "PROYCODI", DbType.Int32, id);
            dbProvider.AddInParameter(commandHoja, "IND_DEL", DbType.String, Constantes.IndDel);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                if (dr.Read())
                {
                    ob.Fichabcodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : (decimal?)null;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : (decimal?)null;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : (decimal?)null;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : (decimal?)null;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : (decimal?)null;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : (decimal?)null;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : (decimal?)null;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : (decimal?)null;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : (decimal?)null;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : (decimal?)null;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : (decimal?)null;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : (decimal?)null;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : (decimal?)null;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : (decimal?)null;
                    ob.Usoagua = !dr.IsDBNull(dr.GetOrdinal("USOAGUA")) ? dr.GetDecimal(dr.GetOrdinal("USOAGUA")) : (decimal?)null;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : (decimal?)null;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : (decimal?)null;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : (decimal?)null;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : (decimal?)null;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
         
                }
            }

            return ob;
        }


        public bool UpdateRegHojaB(RegHojaBDTO regHojaBDTO)
        {
            DbCommand dbCommand = dbProvider.GetSqlStringCommand(Helper.SqlUpdateRegHojaB);

            dbProvider.AddInParameter(dbCommand, "PROYCODI", DbType.Int32, regHojaBDTO.Proycodi);
            dbProvider.AddInParameter(dbCommand, "ESTUDIOFACTIBILIDAD", DbType.Int32, regHojaBDTO.Estudiofactibilidad);
            dbProvider.AddInParameter(dbCommand, "INVESTIGACIONESCAMPO", DbType.Int32, regHojaBDTO.Investigacionescampo);
            dbProvider.AddInParameter(dbCommand, "GESTIONESFINANCIERAS", DbType.Int32, regHojaBDTO.Gestionesfinancieras);
            dbProvider.AddInParameter(dbCommand, "DISEÑOSPERMISOS", DbType.Int32, regHojaBDTO.Disenospermisos);
            dbProvider.AddInParameter(dbCommand, "OBRASCIVILES", DbType.Int32, regHojaBDTO.Obrasciviles);
            dbProvider.AddInParameter(dbCommand, "EQUIPAMIENTO", DbType.Int32, regHojaBDTO.Equipamiento);
            dbProvider.AddInParameter(dbCommand, "LINEATRANSMISION", DbType.Int32, regHojaBDTO.Lineatransmision);
            dbProvider.AddInParameter(dbCommand, "OBRASREGULACION", DbType.Int32, regHojaBDTO.Obrasregulacion);
            dbProvider.AddInParameter(dbCommand, "ADMINISTRACION", DbType.Int32, regHojaBDTO.Administracion);
            dbProvider.AddInParameter(dbCommand, "ADUANAS", DbType.Int32, regHojaBDTO.Aduanas);
            dbProvider.AddInParameter(dbCommand, "SUPERVISION", DbType.Int32, regHojaBDTO.Supervision);
            dbProvider.AddInParameter(dbCommand, "GASTOSGESTION", DbType.Int32, regHojaBDTO.Gastosgestion);
            dbProvider.AddInParameter(dbCommand, "IMPREVISTOS", DbType.Int32, regHojaBDTO.Imprevistos);
            dbProvider.AddInParameter(dbCommand, "IGV", DbType.Int32, regHojaBDTO.Igv);
            dbProvider.AddInParameter(dbCommand, "USOAGUA", DbType.Int32, regHojaBDTO.Usoagua);
            dbProvider.AddInParameter(dbCommand, "OTROSGASTOS", DbType.Int32, regHojaBDTO.Otrosgastos);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALSINIGV", DbType.Int32, regHojaBDTO.Inversiontotalsinigv);
            dbProvider.AddInParameter(dbCommand, "INVERSIONTOTALCONIGV", DbType.Int32, regHojaBDTO.Inversiontotalconigv);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOTIPO", DbType.String, regHojaBDTO.Financiamientotipo);
            dbProvider.AddInParameter(dbCommand, "FINANCIAMIENTOESTADO", DbType.String, regHojaBDTO.Financiamientoestado);
            dbProvider.AddInParameter(dbCommand, "PORCENTAJEFINANCIADO", DbType.Int32, regHojaBDTO.Porcentajefinanciado);
            dbProvider.AddInParameter(dbCommand, "CONCESIONDEFINITIVA", DbType.DateTime, regHojaBDTO.Concesiondefinitiva);
            dbProvider.AddInParameter(dbCommand, "VENTAENERGIA", DbType.DateTime, regHojaBDTO.Ventaenergia);
            dbProvider.AddInParameter(dbCommand, "EJECUCIONOBRA", DbType.DateTime, regHojaBDTO.Ejecucionobra);
            dbProvider.AddInParameter(dbCommand, "CONTRATOSFINANCIEROS", DbType.DateTime, regHojaBDTO.Contratosfinancieros);
            dbProvider.AddInParameter(dbCommand, "OBSERVACIONES", DbType.String, regHojaBDTO.Observaciones);
            dbProvider.AddInParameter(dbCommand, "USU_MODIFICACION", DbType.String, regHojaBDTO.Usumodificacion);
            dbProvider.AddInParameter(dbCommand, "FEC_MODIFICACION", DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(dbCommand, "FICHABCODI", DbType.String, regHojaBDTO.Fichabcodi);
            dbProvider.ExecuteNonQuery(dbCommand);
            return true;
        }


        public List<RegHojaBDTO> GetRegHojaBByFilter(string pericodi, string empresa, string estado)
        {
            List<RegHojaBDTO> listob = new List<RegHojaBDTO>();
            string query = $@"
                 SELECT CGB.*, TR.EMPRESANOM, TR.PROYNOMBRE, TR.PROYDESCRIPCION, TR.PROYCONFIDENCIAL, TP.TIPONOMBRE, TF.TIPOFINOMBRE  FROM CAM_REGHOJAB CGB
                 INNER JOIN CAM_TRNSMPROYECTO TR ON TR.PROYCODI = CGB.PROYCODI
                 INNER JOIN CAM_PLANTRANSMISION PL ON PL.PLANCODI = TR.PLANCODI
                 INNER JOIN CAM_TIPOPROYECTO TP ON TP.TIPOCODI = TR.TIPOCODI
                 LEFT JOIN CAM_TIPOFICHAPROYECTO TF ON TF.TIPOFICODI = TR.TIPOFICODI
                WHERE TR.PERICODI  IN ({pericodi}) AND 
                PL.CODEMPRESA IN ({empresa})  AND 
                CGB.IND_DEL = 0 AND 
                PL.PLANESTADO ='{estado}'
                ORDER BY TR.PERICODI, CGB.PROYCODI,PL.CODEMPRESA, CGB.FICHABCODI ASC";

            DbCommand commandHoja = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(commandHoja))
            {
                while (dr.Read())
                {
                    RegHojaBDTO ob = new RegHojaBDTO();
                    ob.Fichabcodi = !dr.IsDBNull(dr.GetOrdinal("FICHABCODI")) ? dr.GetInt32(dr.GetOrdinal("FICHABCODI")) : 0;
                    ob.Proycodi = !dr.IsDBNull(dr.GetOrdinal("PROYCODI")) ? dr.GetInt32(dr.GetOrdinal("PROYCODI")) : 0;
                    ob.Estudiofactibilidad = !dr.IsDBNull(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) ? dr.GetDecimal(dr.GetOrdinal("ESTUDIOFACTIBILIDAD")) : 0;
                    ob.Investigacionescampo = !dr.IsDBNull(dr.GetOrdinal("INVESTIGACIONESCAMPO")) ? dr.GetDecimal(dr.GetOrdinal("INVESTIGACIONESCAMPO")) : 0;
                    ob.Gestionesfinancieras = !dr.IsDBNull(dr.GetOrdinal("GESTIONESFINANCIERAS")) ? dr.GetDecimal(dr.GetOrdinal("GESTIONESFINANCIERAS")) : 0;
                    ob.Disenospermisos = !dr.IsDBNull(dr.GetOrdinal("DISENOSPERMISOS")) ? dr.GetDecimal(dr.GetOrdinal("DISENOSPERMISOS")) : 0;
                    ob.Obrasciviles = !dr.IsDBNull(dr.GetOrdinal("OBRASCIVILES")) ? dr.GetDecimal(dr.GetOrdinal("OBRASCIVILES")) : 0;
                    ob.Equipamiento = !dr.IsDBNull(dr.GetOrdinal("EQUIPAMIENTO")) ? dr.GetDecimal(dr.GetOrdinal("EQUIPAMIENTO")) : 0;
                    ob.Lineatransmision = !dr.IsDBNull(dr.GetOrdinal("LINEATRANSMISION")) ? dr.GetDecimal(dr.GetOrdinal("LINEATRANSMISION")) : 0;
                    ob.Obrasregulacion = !dr.IsDBNull(dr.GetOrdinal("OBRASREGULACION")) ? dr.GetDecimal(dr.GetOrdinal("OBRASREGULACION")) : 0;
                    ob.Administracion = !dr.IsDBNull(dr.GetOrdinal("ADMINISTRACION")) ? dr.GetDecimal(dr.GetOrdinal("ADMINISTRACION")) : 0;
                    ob.Aduanas = !dr.IsDBNull(dr.GetOrdinal("ADUANAS")) ? dr.GetDecimal(dr.GetOrdinal("ADUANAS")) : 0;
                    ob.Supervision = !dr.IsDBNull(dr.GetOrdinal("SUPERVISION")) ? dr.GetDecimal(dr.GetOrdinal("SUPERVISION")) : 0;
                    ob.Gastosgestion = !dr.IsDBNull(dr.GetOrdinal("GASTOSGESTION")) ? dr.GetDecimal(dr.GetOrdinal("GASTOSGESTION")) : 0;
                    ob.Imprevistos = !dr.IsDBNull(dr.GetOrdinal("IMPREVISTOS")) ? dr.GetDecimal(dr.GetOrdinal("IMPREVISTOS")) : 0;
                    ob.Igv = !dr.IsDBNull(dr.GetOrdinal("IGV")) ? dr.GetDecimal(dr.GetOrdinal("IGV")) : 0;
                    ob.Usoagua = !dr.IsDBNull(dr.GetOrdinal("USOAGUA")) ? dr.GetDecimal(dr.GetOrdinal("USOAGUA")) : 0;
                    ob.Otrosgastos = !dr.IsDBNull(dr.GetOrdinal("OTROSGASTOS")) ? dr.GetDecimal(dr.GetOrdinal("OTROSGASTOS")) : 0;
                    ob.Inversiontotalsinigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALSINIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALSINIGV")) : 0;
                    ob.Inversiontotalconigv = !dr.IsDBNull(dr.GetOrdinal("INVERSIONTOTALCONIGV")) ? dr.GetDecimal(dr.GetOrdinal("INVERSIONTOTALCONIGV")) : 0;
                    ob.Financiamientotipo = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOTIPO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOTIPO")) : string.Empty;
                    ob.Financiamientoestado = !dr.IsDBNull(dr.GetOrdinal("FINANCIAMIENTOESTADO")) ? dr.GetString(dr.GetOrdinal("FINANCIAMIENTOESTADO")) : string.Empty;
                    ob.Porcentajefinanciado = !dr.IsDBNull(dr.GetOrdinal("PORCENTAJEFINANCIADO")) ? dr.GetDecimal(dr.GetOrdinal("PORCENTAJEFINANCIADO")) : 0;
                    ob.Concesiondefinitiva = !dr.IsDBNull(dr.GetOrdinal("CONCESIONDEFINITIVA")) ? dr.GetString(dr.GetOrdinal("CONCESIONDEFINITIVA")) : string.Empty;
                    ob.Ventaenergia = !dr.IsDBNull(dr.GetOrdinal("VENTAENERGIA")) ? dr.GetString(dr.GetOrdinal("VENTAENERGIA")) : string.Empty;
                    ob.Ejecucionobra = !dr.IsDBNull(dr.GetOrdinal("EJECUCIONOBRA")) ? dr.GetString(dr.GetOrdinal("EJECUCIONOBRA")) : string.Empty;
                    ob.Contratosfinancieros = !dr.IsDBNull(dr.GetOrdinal("CONTRATOSFINANCIEROS")) ? dr.GetString(dr.GetOrdinal("CONTRATOSFINANCIEROS")) : string.Empty;
                    ob.Observaciones = !dr.IsDBNull(dr.GetOrdinal("OBSERVACIONES")) ? dr.GetString(dr.GetOrdinal("OBSERVACIONES")) : string.Empty;
                    ob.Usucreacion = !dr.IsDBNull(dr.GetOrdinal("USU_CREACION")) ? dr.GetString(dr.GetOrdinal("USU_CREACION")) : string.Empty;
                    ob.Fechacreacion = !dr.IsDBNull(dr.GetOrdinal("FEC_CREACION")) ? dr.GetDateTime(dr.GetOrdinal("FEC_CREACION")) : DateTime.MinValue;
                    ob.IndDel = !dr.IsDBNull(dr.GetOrdinal("IND_DEL")) ? dr.GetString(dr.GetOrdinal("IND_DEL")) : string.Empty;
                    ob.Empresa = !dr.IsDBNull(dr.GetOrdinal("EMPRESANOM")) ? dr.GetString(dr.GetOrdinal("EMPRESANOM")) : string.Empty;
                    ob.NombreProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYNOMBRE")) ? dr.GetString(dr.GetOrdinal("PROYNOMBRE")) : string.Empty;
                    ob.DetalleProyecto = !dr.IsDBNull(dr.GetOrdinal("PROYDESCRIPCION")) ? dr.GetString(dr.GetOrdinal("PROYDESCRIPCION")) : string.Empty;
                    ob.TipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPONOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPONOMBRE")) : string.Empty;
                    ob.SubTipoProyecto = !dr.IsDBNull(dr.GetOrdinal("TIPOFINOMBRE")) ? dr.GetString(dr.GetOrdinal("TIPOFINOMBRE")) : string.Empty;
                    ob.Condifencial = !dr.IsDBNull(dr.GetOrdinal("PROYCONFIDENCIAL")) ? dr.GetString(dr.GetOrdinal("PROYCONFIDENCIAL")) : string.Empty;
                    listob.Add(ob);

                }
            }

            return listob;
        }
    }


    }
