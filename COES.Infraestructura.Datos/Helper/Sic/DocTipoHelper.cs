using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla DOC_TIPO
    /// </summary>
    public class DocTipoHelper : HelperBase
    {
        public DocTipoHelper(): base(Consultas.DocTipoSql)
        {
        }

        public DocTipoDTO Create(IDataReader dr)
        {
            DocTipoDTO entity = new DocTipoDTO();

            int iTipcodi = dr.GetOrdinal(this.Tipcodi);
            if (!dr.IsDBNull(iTipcodi)) entity.Tipcodi = Convert.ToInt32(dr.GetValue(iTipcodi));


            int iTipplazo = dr.GetOrdinal(this.Tipplazo);
            if (!dr.IsDBNull(iTipplazo)) entity.Tipplazo = Convert.ToInt32(dr.GetValue(iTipplazo));

            int iTipnombre = dr.GetOrdinal(this.Tipnombre);
            if (!dr.IsDBNull(iTipnombre)) entity.Tipnombre = dr.GetString(iTipnombre);

            int iTipdesc = dr.GetOrdinal(this.Tipdesc);
            if (!dr.IsDBNull(iTipdesc)) entity.Tipdesc = dr.GetString(iTipdesc);

            int iTipselec = dr.GetOrdinal(this.Tipselec);
            if (!dr.IsDBNull(iTipselec)) entity.Tipselec = dr.GetString(iTipselec);

            int iTipdiacal = dr.GetOrdinal(this.Tipdiacal);
            if (!dr.IsDBNull(iTipdiacal)) entity.Tipdiacal = dr.GetString(iTipdiacal);
            return entity;
        }


        #region Mapeo de Campos

        public string Tipcodi = "TIPCODI";
        public string Tipnombre = "TIPNOMBRE";
        public string Tipdesc = "TIPDESC";
        public string Tipselec = "TIPSELEC";
        public string Tipplazo = "TIPPLAZO";
        public string Tipdiacal = "TIPDIACAL";


        #endregion
    }
}
