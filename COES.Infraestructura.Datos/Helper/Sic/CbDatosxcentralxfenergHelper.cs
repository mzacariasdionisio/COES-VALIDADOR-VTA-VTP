using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CB_DATOSXCENTRALXFENERG
    /// </summary>
    public class CbDatosxcentralxfenergHelper : HelperBase
    {
        public CbDatosxcentralxfenergHelper(): base(Consultas.CbDatosxcentralxfenergSql)
        {
        }

        public CbDatosxcentralxfenergDTO Create(IDataReader dr)
        {
            CbDatosxcentralxfenergDTO entity = new CbDatosxcentralxfenergDTO();

            int iCbdatcodi = dr.GetOrdinal(this.Cbdatcodi);
            if (!dr.IsDBNull(iCbdatcodi)) entity.Cbdatcodi = Convert.ToInt32(dr.GetValue(iCbdatcodi));

            int iCbcxfecodi = dr.GetOrdinal(this.Cbcxfecodi);
            if (!dr.IsDBNull(iCbcxfecodi)) entity.Cbcxfecodi = Convert.ToInt32(dr.GetValue(iCbcxfecodi));

            int iCcombcodi = dr.GetOrdinal(this.Ccombcodi);
            if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

            int iCbdatvalor1 = dr.GetOrdinal(this.Cbdatvalor1);
            if (!dr.IsDBNull(iCbdatvalor1)) entity.Cbdatvalor1 = dr.GetDecimal(iCbdatvalor1);

            int iCbdatvalor2 = dr.GetOrdinal(this.Cbdatvalor2);
            if (!dr.IsDBNull(iCbdatvalor2)) entity.Cbdatvalor2 = dr.GetDecimal(iCbdatvalor2);

            int iCbdatfecregistro = dr.GetOrdinal(this.Cbdatfecregistro);
            if (!dr.IsDBNull(iCbdatfecregistro)) entity.Cbdatfecregistro = dr.GetDateTime(iCbdatfecregistro);

            int iCbdatusuregistro = dr.GetOrdinal(this.Cbdatusuregistro);
            if (!dr.IsDBNull(iCbdatusuregistro)) entity.Cbdatusuregistro = dr.GetString(iCbdatusuregistro);

            return entity;
        }


        #region Mapeo de Campos

        public string Cbdatcodi = "CBDATCODI";
        public string Cbcxfecodi = "CBCXFECODI";
        public string Ccombcodi = "CCOMBCODI";
        public string Cbdatvalor1 = "CBDATVALOR1";
        public string Cbdatvalor2 = "CBDATVALOR2";
        public string Cbdatfecregistro = "CBDATFECREGISTRO";
        public string Cbdatusuregistro = "CBDATUSUREGISTRO";

        #endregion
    }
}
