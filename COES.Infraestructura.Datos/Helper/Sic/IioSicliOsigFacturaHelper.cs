using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IIO_SICLI_OSIG_FACTURA
    /// </summary>
    public class IioSicliOsigFacturaHelper : HelperBase
    {
        public IioSicliOsigFacturaHelper()
            : base(Consultas.IioSicliOsigFacturaSql)
        {
        }

        public IioSicliOsigFacturaDTO Create(IDataReader dr)
        {
            IioSicliOsigFacturaDTO entity = new IioSicliOsigFacturaDTO();

            int iClofaccodi = dr.GetOrdinal(this.Clofaccodi);
            if (!dr.IsDBNull(iClofaccodi)) entity.Clofaccodi = Convert.ToInt32(dr.GetValue(iClofaccodi));

            int iClofacanhiomes = dr.GetOrdinal(this.Clofacanhiomes);
            if (!dr.IsDBNull(iClofacanhiomes)) entity.Clofacanhiomes = dr.GetString(iClofacanhiomes);

            int iClofaccodempresa = dr.GetOrdinal(this.Clofaccodempresa);
            if (!dr.IsDBNull(iClofaccodempresa)) entity.Clofaccodempresa = dr.GetString(iClofaccodempresa);

            int iClofacnomempresa = dr.GetOrdinal(this.Clofacnomempresa);
            if (!dr.IsDBNull(iClofacnomempresa)) entity.Clofacnomempresa = dr.GetString(iClofacnomempresa);



            int iClofacefpbe = dr.GetOrdinal(this.Clofacefpbe);
            if (!dr.IsDBNull(iClofacefpbe)) entity.Clofacefpbe = dr.GetDecimal(iClofacefpbe);

            return entity;
        }


        #region Mapeo de Campos

        public string Clofaccodi = "CLOFACCODI";
        public string Clofacanhiomes = "CLOFACANHIOMES";
        public string Clofaccodempresa = "CLOFACCODEMPRESA";
        public string Clofacnomempresa = "CLOFACNOMEMPRESA";
        public string Clofacruc = "CLOFACRUC";
        public string Clofaccodcliente = "CLOFACCODCLIENTE";
        public string Clofacnomcliente = "CLOFACNOMCLIENTE";
        public string Clofaccodbarrasumin = "CLOFACCODBARRASUMIN";
        public string Clofacnombarrasumin = "CLOFACNOMBARRASUMIN";
        public string Clofactensionentrega = "CLOFACTENSIONENTREGA";
        public string Clofaccodbrg = "CLOFACCODBRG";
        public string Clofacnombrg = "CLOFACNOMBRG";

        public string Clofactensionbrg = "CLOFACTENSIONBRG";
        public string Clofacphpbe = "CLOFACPHPBE";
        public string Clofacpfpbe = "CLOFACPFPBE";
        public string Clofacehpbe = "CLOFACEHPBE";
        public string Clofacefpbe = "CLOFACEFPBE";

        public string Clofacusucreacion = "CLOFACUSUCREACION";
        public string Clofacfeccreacion = "CLOFACFECCREACION";
        public string Clofacusumodificacion = "CLOFACUSUMODIFICACION";
        public string Clofacfecmodificacion = "CLOFACFECMODIFICACION";        

        #endregion

        public string SqlGetCountTotal
        {
            get { return base.GetSqlXml("GetCountTotal"); }
        }

        public string SqlGetCountTotalFactura
        {
            get { return base.GetSqlXml("GetCountTotalFactura"); }
        }        

        public string SqlListRepCompCliente
        {
            get { return base.GetSqlXml("ListRepCompCliente"); }
        }

        public string SqlGetCountTotalRuc
        {
            get { return base.GetSqlXml("GetCountTotalRuc"); }
        }

        public string SqlGetCountTotalFacturaRuc
        {
            get { return base.GetSqlXml("GetCountTotalFacturaRuc"); }
        }
        public string SqlListRepCompEmpresa
        {
            get { return base.GetSqlXml("ListRepCompEmpresa"); }
        }
        public string SqlListRepCompHistorico
        {
            get { return base.GetSqlXml("ListRepCompHistorico"); }
        }
       
    }
}
