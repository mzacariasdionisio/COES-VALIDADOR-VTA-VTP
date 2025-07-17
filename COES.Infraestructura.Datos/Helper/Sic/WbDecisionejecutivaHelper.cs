using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_DECISIONEJECUTIVA
    /// </summary>
    public class WbDecisionejecutivaHelper : HelperBase
    {
        public WbDecisionejecutivaHelper(): base(Consultas.WbDecisionejecutivaSql)
        {
        }

        public WbDecisionejecutivaDTO Create(IDataReader dr)
        {
            WbDecisionejecutivaDTO entity = new WbDecisionejecutivaDTO();

            int iDesejecodi = dr.GetOrdinal(this.Desejecodi);
            if (!dr.IsDBNull(iDesejecodi)) entity.Desejecodi = Convert.ToInt32(dr.GetValue(iDesejecodi));

            int iDesejedescripcion = dr.GetOrdinal(this.Desejedescripcion);
            if (!dr.IsDBNull(iDesejedescripcion)) entity.Desejedescripcion = dr.GetString(iDesejedescripcion);

            int iDesejefechapub = dr.GetOrdinal(this.Desejefechapub);
            if (!dr.IsDBNull(iDesejefechapub)) entity.Desejefechapub = dr.GetDateTime(iDesejefechapub);

            int iDesejetipo = dr.GetOrdinal(this.Desejetipo);
            if (!dr.IsDBNull(iDesejetipo)) entity.Desejetipo = dr.GetString(iDesejetipo);

            int iDesejeestado = dr.GetOrdinal(this.Desejeestado);
            if (!dr.IsDBNull(iDesejeestado)) entity.Desejeestado = dr.GetString(iDesejeestado);

            int iDesejefile = dr.GetOrdinal(this.Desejefile);
            if (!dr.IsDBNull(iDesejefile)) entity.Desejefile = dr.GetString(iDesejefile);

            int iDesejeextension = dr.GetOrdinal(this.Desejeextension);
            if (!dr.IsDBNull(iDesejeextension)) entity.Desejeextension = dr.GetString(iDesejeextension);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Desejecodi = "DESEJECODI";
        public string Desejedescripcion = "DESEJEDESCRIPCION";
        public string Desejefechapub = "DESEJEFECHAPUB";
        public string Desejetipo = "DESEJETIPO";
        public string Desejeestado = "DESEJEESTADO";
        public string Desejefile = "DESEJEFILE";
        public string Desejeextension = "DESEJEEXTENSION";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion
    }
}
