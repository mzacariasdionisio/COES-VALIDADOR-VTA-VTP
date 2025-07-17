using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_OSIG_FACTURA_UL
    /// </summary>
    public class IioOsigFacturaUlRepository : RepositoryBase, IIioOsigFacturaUlRepository
    {
        private readonly IioOsigFacturaUlHelper helper = new IioOsigFacturaUlHelper();

        public IioOsigFacturaUlRepository(string strConn) : base(strConn)
        {

        }

        public void BulkInsert(List<IioOsigFacturaUlDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Psiclicodi, DbType.Int32);

            //dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            //dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            
            dbProvider.AddColumnMapping(helper.Ulfactcodempresa, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactcodsuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactmesfacturado, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactcodbrg, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactcodpuntosuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactcodareademanda, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulfactpagavad, DbType.String);//ok
            dbProvider.AddColumnMapping(helper.Ulfactprecenergbrghp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactprecenergbrgfp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactprecpotenbrg, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactconsenergactvhpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactconsenergactvfpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactmaxdemhpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactmaxdemfpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactpeajetransmprin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactpeajetransmsec, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfpmpoten, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfpmenerg, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfactgeneracion, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfacttransmprin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfacttransmsec, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfactdistrib, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfactexcesopoten, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactfacturaciontotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactconsenergreacps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactppmt, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactpemt, DbType.Decimal);//ok
            dbProvider.AddColumnMapping(helper.Ulfactfactptoref, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactvadhp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactvadfp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactcargoelectrificarural, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactotrosconceptosnoafecigv, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ulfactotrosconceptosafectoigv, DbType.Decimal);

            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ptomedicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulfactbarrcodibrg, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ulfactbarrcodiptosumin, DbType.Int32);

            dbProvider.AddColumnMapping(helper.Ulfactusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Ulfactfeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<IioOsigFacturaUlDTO>(entitys, helper.TableName);
        }

        public void Delete(int psiclicodi, string ulfactcodempresa)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.AddInParameter(command, helper.Ulfactcodempresa, DbType.String, ulfactcodempresa);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdatePtoMediCodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            //Actualizamos la columna PTOMEDICODI 
            //string condicion = "";

            //if (!string.IsNullOrEmpty(empresa))
            //{
            //    condicion = " AND tmp.ULFACTCODEMPRESA IN (" + empresa + ")";
            //}
            string queryString = string.Format(helper.SqlUpdateOsigFactura, empresa);            
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.Emprcodisuministrador, DbType.Int32, emprcodisuministrador);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            //dbProvider.AddInParameter(command, helper.Ulfactcodempresa, DbType.String, empresa);
            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEmprcodiOsigFactura(int emprcodisuministrador, int psiclicodi, string empresa)
        {
            //Actualizamos la columna EMPRCODI 
            string condicion = "";

            if (!string.IsNullOrEmpty(empresa))
            {
                condicion = " AND TRIM(tmp.ULFACTCODEMPRESA) IN (" + empresa + ")";
            }
            string queryString = string.Format(helper.SqlUpdateEmprcodiOsigFactura, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.Emprcodisuministrador, DbType.Int32, emprcodisuministrador);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            //dbProvider.AddInParameter(command, helper.Ulfactcodempresa, DbType.String, empresa);
            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo registro de la factura.
        /// </summary>
        public void Save(List<IioOsigFacturaUlDTO> entitys)
        {
            DbCommand comando = null;

            foreach (IioOsigFacturaUlDTO iioFacturaDTO in entitys)
            {
                comando = dbProvider.GetSqlStringCommand(helper.SqlSave);

                dbProvider.AddInParameter(comando, helper.Psiclicodi, DbType.Int32, iioFacturaDTO.Psiclicodi);
                dbProvider.AddInParameter(comando, helper.Ulfactcodempresa, DbType.String, iioFacturaDTO.Ulfactcodempresa);
                dbProvider.AddInParameter(comando, helper.Ulfactcodsuministro, DbType.String, iioFacturaDTO.Ulfactcodsuministro);
                dbProvider.AddInParameter(comando, helper.Ulfactmesfacturado, DbType.String, iioFacturaDTO.Ulfactmesfacturado);
                dbProvider.AddInParameter(comando, helper.Ulfactcodbrg, DbType.String, iioFacturaDTO.Ulfactcodbrg);
                dbProvider.AddInParameter(comando, helper.Ulfactcodpuntosuministro, DbType.String, iioFacturaDTO.Ulfactcodpuntosuministro);
                dbProvider.AddInParameter(comando, helper.Ulfactcodareademanda, DbType.Int32, iioFacturaDTO.Ulfactcodareademanda);
                dbProvider.AddInParameter(comando, helper.Ulfactpagavad, DbType.String, iioFacturaDTO.Ulfactpagavad);
                dbProvider.AddInParameter(comando, helper.Ulfactprecenergbrghp, DbType.Decimal, iioFacturaDTO.Ulfactprecenergbrghp);
                dbProvider.AddInParameter(comando, helper.Ulfactprecenergbrgfp, DbType.Decimal, iioFacturaDTO.Ulfactprecenergbrgfp);
                dbProvider.AddInParameter(comando, helper.Ulfactprecpotenbrg, DbType.Decimal, iioFacturaDTO.Ulfactprecpotenbrg);
                dbProvider.AddInParameter(comando, helper.Ulfactconsenergactvhpps, DbType.Decimal, iioFacturaDTO.Ulfactconsenergactvhpps);
                dbProvider.AddInParameter(comando, helper.Ulfactconsenergactvfpps, DbType.Decimal, iioFacturaDTO.Ulfactconsenergactvfpps);
                dbProvider.AddInParameter(comando, helper.Ulfactmaxdemhpps, DbType.Decimal, iioFacturaDTO.Ulfactmaxdemhpps);
                dbProvider.AddInParameter(comando, helper.Ulfactmaxdemfpps, DbType.Decimal, iioFacturaDTO.Ulfactmaxdemfpps);
                dbProvider.AddInParameter(comando, helper.Ulfactpeajetransmprin, DbType.Decimal, iioFacturaDTO.Ulfactpeajetransmprin);
                dbProvider.AddInParameter(comando, helper.Ulfactpeajetransmsec, DbType.Decimal, iioFacturaDTO.Ulfactpeajetransmsec);
                dbProvider.AddInParameter(comando, helper.Ulfactfpmpoten, DbType.Decimal, iioFacturaDTO.Ulfactfpmpoten);
                dbProvider.AddInParameter(comando, helper.Ulfactfpmenerg, DbType.Decimal, iioFacturaDTO.Ulfactfpmenerg);
                dbProvider.AddInParameter(comando, helper.Ulfactfactgeneracion, DbType.Decimal, iioFacturaDTO.Ulfactfactgeneracion);
                dbProvider.AddInParameter(comando, helper.Ulfactfacttransmprin, DbType.Decimal, iioFacturaDTO.Ulfactfacttransmprin);
                dbProvider.AddInParameter(comando, helper.Ulfactfacttransmsec, DbType.Decimal, iioFacturaDTO.Ulfactfacttransmsec);
                dbProvider.AddInParameter(comando, helper.Ulfactfactdistrib, DbType.Decimal, iioFacturaDTO.Ulfactfactdistrib);
                dbProvider.AddInParameter(comando, helper.Ulfactfactexcesopoten, DbType.Decimal, iioFacturaDTO.Ulfactfactexcesopoten);
                dbProvider.AddInParameter(comando, helper.Ulfactfacturaciontotal, DbType.Decimal, iioFacturaDTO.Ulfactfacturaciontotal);
                dbProvider.AddInParameter(comando, helper.Ulfactconsenergreacps, DbType.Decimal, iioFacturaDTO.Ulfactconsenergreacps);
                dbProvider.AddInParameter(comando, helper.Ulfactppmt, DbType.Decimal, iioFacturaDTO.Ulfactppmt);
                dbProvider.AddInParameter(comando, helper.Ulfactpemt, DbType.Decimal, iioFacturaDTO.Ulfactpemt);
                dbProvider.AddInParameter(comando, helper.Ulfactfactptoref, DbType.String, iioFacturaDTO.Ulfactfactptoref);
                dbProvider.AddInParameter(comando, helper.Ulfactvadhp, DbType.Decimal, iioFacturaDTO.Ulfactvadhp);
                dbProvider.AddInParameter(comando, helper.Ulfactvadfp, DbType.Decimal, iioFacturaDTO.Ulfactvadfp);
                dbProvider.AddInParameter(comando, helper.Ulfactcargoelectrificarural, DbType.Decimal, iioFacturaDTO.Ulfactcargoelectrificarural);
                dbProvider.AddInParameter(comando, helper.Ulfactotrosconceptosnoafecigv, DbType.Decimal, iioFacturaDTO.Ulfactotrosconceptosnoafecigv);
                dbProvider.AddInParameter(comando, helper.Ulfactotrosconceptosafectoigv, DbType.Decimal, iioFacturaDTO.Ulfactotrosconceptosafectoigv);
                dbProvider.AddInParameter(comando, helper.Emprcodi, DbType.Int32, iioFacturaDTO.Emprcodi);
                dbProvider.AddInParameter(comando, helper.Ptomedicodi, DbType.Int32, iioFacturaDTO.Ptomedicodi);

                dbProvider.AddInParameter(comando, helper.Ulfactusucreacion, DbType.String, iioFacturaDTO.Ulfactusucreacion);
                dbProvider.AddInParameter(comando, helper.Ulfactfeccreacion, DbType.DateTime, iioFacturaDTO.Ulfactfeccreacion);

                try
                {
                    dbProvider.ExecuteNonQuery(comando);
                }
                catch (Exception ex)
                {   
                    throw ex;
                }
            }
            
        }

        public string ValidarOsigFacturaPuntoMedicion(int psiclicodi, string empresa)
        {
            string rpta = "";


            string queryString = string.Empty;
            DbCommand command = null;

            //Verificamos que todos los suministros tienen un equipo asignado
            queryString = helper.SqlValidarPuntoMedicion;
            if (!string.IsNullOrEmpty(empresa))
            {
                queryString = string.Format(queryString, " AND ULFACTCODEMPRESA = '" + empresa + "'");
            }
            else
            {
                queryString = string.Format(queryString, "");
            }
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);

            dbProvider.ExecuteNonQuery(command);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IioOsigFacturaUlDTO entity = new IioOsigFacturaUlDTO();
                    int iUlfactcodsuministro = dr.GetOrdinal(helper.Ulfactcodsuministro);
                    if (!dr.IsDBNull(iUlfactcodsuministro)) entity.Ulfactcodsuministro = dr.GetString(iUlfactcodsuministro);

                    if (rpta == "")
                    {
                        rpta = entity.Ulfactcodsuministro;
                    }
                    else
                    {
                        rpta = rpta + ", " + entity.Ulfactcodsuministro;
                    }
                }
            }

            if (!rpta.Equals(""))
            {
                return rpta;
            }

            rpta = "OK";

            return rpta;
        }

        public int ValidarOsigFacturaTablaEmpresas(int psiclicodi, string empresa)
        {
            var queryString = helper.SqlValidarTablaEmpresas;
            if (!string.IsNullOrEmpty(empresa))
            {                
                queryString = queryString + " AND RCIMEMPRESA IN (" + empresa + ")";
            }
           
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            int correlativo = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iCorrelativo = dr.GetOrdinal(helper.Registros);
                    if (!dr.IsDBNull(iCorrelativo)) correlativo = dr.GetInt32(iCorrelativo);
                }
            }

            return correlativo;
        }


        public void SaveIioFactura(int psiclicodi, string empresa)
        {            
            string queryString = string.Empty;
            DbCommand command = null;

            var queryWhere = string.Empty;
            if (!string.IsNullOrEmpty(empresa))
            {
                queryWhere = " AND ULFACTCODEMPRESA IN (" + empresa + ")";
            }
           
            command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlDeleteIioFactura, psiclicodi));
            //dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);

                                 
            queryString = string.Format(helper.SqlSaveIioFactura, queryWhere);            
            command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);

            dbProvider.ExecuteNonQuery(command);

           
        }

        public int GetCorrelativoDisponibleLogImportacionSicli()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxIdIioLogImportacion);
            int correlativo = 1;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iCorrelativo = dr.GetOrdinal(helper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) correlativo = dr.GetInt32(iCorrelativo);
                }
            }

            return correlativo;
        }

        public void GenerarOsigFacturaLogImportacionEmpresa(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {

            var maxId = GetCorrelativoDisponibleLogImportacionSicli();

            var paramsQuery = new object[] { maxId, usuario, periodo, tabla, empresas };
            var query = string.Format(helper.SqlRegistrarLogimportacionPorEmpresa, paramsQuery);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void GenerarOsigFacturaLogImportacionPtoMedicion(int psiclicodi, string periodo, string usuario, string tabla, string empresas)
        {

            var maxId = GetCorrelativoDisponibleLogImportacionSicli();

            var paramsQuery = new object[] { maxId, usuario, periodo, tabla, empresas };

            var query = string.Format(helper.SqlRegistrarLogimportacionPtoMedicion, paramsQuery);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarControlImportacionNoOK(int psiclicodi, string empresas, string usuario, string tabla)
        {
            var query = string.Format(helper.SqlActualizarControlImportacionNoOK, empresas, tabla);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarControlImportacionOK(int psiclicodi, string empresas, string usuario, string tabla)
        {
            var query = string.Format(helper.SqlActualizarControlImportacionOK, empresas, tabla);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarRegistrosImportacion(int psiclicodi, string empresas, string usuario, string tabla)
        {
            var query = string.Format(helper.SqlActualizarCantidadRegistrosImportacionCoes, psiclicodi, tabla, empresas);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ActualizarPeridoFechaSincCoes(int psiclicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarPeriodoFechaSincCoes);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            //dbProvider.AddInParameter(command, helper.Ulfactcodempresa, DbType.String, ulfactcodempresa);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}