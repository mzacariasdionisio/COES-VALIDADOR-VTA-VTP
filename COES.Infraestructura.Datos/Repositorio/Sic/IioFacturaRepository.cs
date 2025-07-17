using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_FACTURA
    /// </summary>
    public class IioFacturaRepository : RepositoryBase, IIioFacturaRepository
    {
        private readonly IioFacturaHelper helper = new IioFacturaHelper();

        public IioFacturaRepository(string strConn) : base(strConn)
        {

        }

        public void BulkInsert(List<IioFacturaDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.Psiclicodi, DbType.Int32);

            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            
            dbProvider.AddColumnMapping(helper.Ufacmesfacturado, DbType.String);
            dbProvider.AddColumnMapping(helper.Ufaccodbrg, DbType.String);
            dbProvider.AddColumnMapping(helper.Ufaccodpuntosuministro, DbType.String);
            dbProvider.AddColumnMapping(helper.Ufacidareademanda, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Ufacpagavad, DbType.String);
            dbProvider.AddColumnMapping(helper.Ufacprecenergbrghp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacprecenergbrgfp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacprecpotenbrg, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacconsenergactvhpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacconsenergactvfpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacmaxdemhpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacmaxdemfpps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacpeajetransmprin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacpeajetransmsec, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfpmpoten, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfpmenerg, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfactgeneracion, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfacttransmprin, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfacttransmsec, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfactdistrib, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfactexcesopoten, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfacturaciontotal, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacconsenergreacps, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacppmt, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacpemt, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacfactptoref, DbType.String);
            dbProvider.AddColumnMapping(helper.Ufacvadhp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacvadfp, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufaccargoelectrificacionrural, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacotrosconceptosnoafectoigv, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Ufacotrosconceptosafectoigv, DbType.Decimal);


            dbProvider.BulkInsert<IioFacturaDTO>(entitys, helper.TableName);
        }

        public void Delete(int psiclicodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Psiclicodi, DbType.Int32, psiclicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.HDT - 09/07/2017: Cambio para atender el requerimiento. 
        /// <summary>
        /// Permite crear un nuevo registro de la factura.
        /// </summary>
        public void Save(List<IioFacturaDTO> entitys)
        {
            DbCommand comando = null;

            foreach (IioFacturaDTO iioFacturaDTO in entitys)
            {
                comando = dbProvider.GetSqlStringCommand(helper.SqlSave);

                dbProvider.AddInParameter(comando, helper.Psiclicodi, DbType.Int32, iioFacturaDTO.Psiclicodi);
                dbProvider.AddInParameter(comando, helper.Ufacmesfacturado, DbType.String, iioFacturaDTO.Ufacmesfacturado);
                dbProvider.AddInParameter(comando, helper.Ufaccodbrg, DbType.String, iioFacturaDTO.Ufaccodbrg);
                dbProvider.AddInParameter(comando, helper.Ufaccodpuntosuministro, DbType.String, iioFacturaDTO.Ufaccodpuntosuministro);
                dbProvider.AddInParameter(comando, helper.Ufacidareademanda, DbType.Int32, iioFacturaDTO.Ufacidareademanda);
                dbProvider.AddInParameter(comando, helper.Ufacpagavad, DbType.String, iioFacturaDTO.Ufacpagavad);
                dbProvider.AddInParameter(comando, helper.Ufacprecenergbrghp, DbType.Decimal, iioFacturaDTO.Ufacprecenergbrghp);
                dbProvider.AddInParameter(comando, helper.Ufacprecenergbrgfp, DbType.Decimal, iioFacturaDTO.Ufacprecenergbrgfp);
                dbProvider.AddInParameter(comando, helper.Ufacprecpotenbrg, DbType.Decimal, iioFacturaDTO.Ufacprecpotenbrg);
                dbProvider.AddInParameter(comando, helper.Ufacconsenergactvhpps, DbType.Decimal, iioFacturaDTO.Ufacconsenergactvhpps);
                dbProvider.AddInParameter(comando, helper.Ufacconsenergactvfpps, DbType.Decimal, iioFacturaDTO.Ufacconsenergactvfpps);
                dbProvider.AddInParameter(comando, helper.Ufacmaxdemhpps, DbType.Decimal, iioFacturaDTO.Ufacmaxdemhpps);
                dbProvider.AddInParameter(comando, helper.Ufacmaxdemfpps, DbType.Decimal, iioFacturaDTO.Ufacmaxdemfpps);
                dbProvider.AddInParameter(comando, helper.Ufacpeajetransmprin, DbType.Decimal, iioFacturaDTO.Ufacpeajetransmprin);
                dbProvider.AddInParameter(comando, helper.Ufacpeajetransmsec, DbType.Decimal, iioFacturaDTO.Ufacpeajetransmsec);
                dbProvider.AddInParameter(comando, helper.Ufacfpmpoten, DbType.Decimal, iioFacturaDTO.Ufacfpmpoten);
                dbProvider.AddInParameter(comando, helper.Ufacfpmenerg, DbType.Decimal, iioFacturaDTO.Ufacfpmenerg);
                dbProvider.AddInParameter(comando, helper.Ufacfactgeneracion, DbType.Decimal, iioFacturaDTO.Ufacfactgeneracion);
                dbProvider.AddInParameter(comando, helper.Ufacfacttransmprin, DbType.Decimal, iioFacturaDTO.Ufacfacttransmprin);
                dbProvider.AddInParameter(comando, helper.Ufacfacttransmsec, DbType.Decimal, iioFacturaDTO.Ufacfacttransmsec);
                dbProvider.AddInParameter(comando, helper.Ufacfactdistrib, DbType.Decimal, iioFacturaDTO.Ufacfactdistrib);
                dbProvider.AddInParameter(comando, helper.Ufacfactexcesopoten, DbType.Decimal, iioFacturaDTO.Ufacfactexcesopoten);
                dbProvider.AddInParameter(comando, helper.Ufacfacturaciontotal, DbType.Decimal, iioFacturaDTO.Ufacfacturaciontotal);
                dbProvider.AddInParameter(comando, helper.Ufacconsenergreacps, DbType.Decimal, iioFacturaDTO.Ufacconsenergreacps);
                dbProvider.AddInParameter(comando, helper.Ufacppmt, DbType.Decimal, iioFacturaDTO.Ufacppmt);
                dbProvider.AddInParameter(comando, helper.Ufacpemt, DbType.Decimal, iioFacturaDTO.Ufacpemt);
                dbProvider.AddInParameter(comando, helper.Ufacfactptoref, DbType.String, iioFacturaDTO.Ufacfactptoref);
                dbProvider.AddInParameter(comando, helper.Ufacvadhp, DbType.Decimal, iioFacturaDTO.Ufacvadhp);
                dbProvider.AddInParameter(comando, helper.Ufacvadfp, DbType.Decimal, iioFacturaDTO.Ufacvadfp);
                dbProvider.AddInParameter(comando, helper.Ufaccargoelectrificacionrural, DbType.Decimal, iioFacturaDTO.Ufaccargoelectrificacionrural);
                dbProvider.AddInParameter(comando, helper.Ufacotrosconceptosnoafectoigv, DbType.Decimal, iioFacturaDTO.Ufacotrosconceptosnoafectoigv);
                dbProvider.AddInParameter(comando, helper.Ufacotrosconceptosafectoigv, DbType.Decimal, iioFacturaDTO.Ufacotrosconceptosafectoigv);
                dbProvider.AddInParameter(comando, helper.Emprcodi, DbType.Int32, iioFacturaDTO.Emprcodi);
                dbProvider.AddInParameter(comando, helper.Equicodi, DbType.Int32, iioFacturaDTO.Equicodi);

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

    }
}