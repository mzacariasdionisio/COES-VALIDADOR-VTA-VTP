using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_TIPOPUNTOMEDICION
    /// </summary>
    public class MeTipopuntomedicionHelper : HelperBase
    {
        public MeTipopuntomedicionHelper()
            : base(Consultas.MeTipopuntomedicionSql)
        {
        }

        public MeTipopuntomedicionDTO Create(IDataReader dr)
        {
            MeTipopuntomedicionDTO entity = new MeTipopuntomedicionDTO();

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iTipoptomedinomb = dr.GetOrdinal(this.Tipoptomedinomb);
            if (!dr.IsDBNull(iTipoptomedinomb)) entity.Tipoptomedinomb = dr.GetString(iTipoptomedinomb);

            int iTipoptomedicodi = dr.GetOrdinal(this.Tipoptomedicodi);
            if (!dr.IsDBNull(iTipoptomedicodi)) entity.Tipoptomedicodi = Convert.ToInt32(dr.GetValue(iTipoptomedicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Famcodi = "FAMCODI";
        public string Tipoptomedinomb = "TPTOMEDINOMB";
        public string Tipoptomedicodi = "TPTOMEDICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Fenergnomb = "FENERGNOMB";
        public string Fenergcolor = "FENERCOLOR";

        public string SqlListarMeTipoPuntoMedicion
        {
            get { return base.GetSqlXml("ListarMeTipoPuntoMedicion"); }
        }

        public string SqlListFromPtomedicion
        {
            get { return base.GetSqlXml("ListFromPtomedicion"); }
        }

        #endregion

        #region Modificación Tipo punto de medición

        public string SqlListarTipoPtoMedicionXFamiliaxTipoInfo
        {
            get { return base.GetSqlXml("ListarTipoPtoMedicionXFamiliaxTipoInfo"); }
        }
        #endregion

        #region Medidores de Generación PR15
        public string SQLListarTipoPtoMedicionxTipoInfo
        {
            get { return base.GetSqlXml("ListarTipoPtoMedicionxTipoInfo"); }
        }
        #endregion
    }
}
