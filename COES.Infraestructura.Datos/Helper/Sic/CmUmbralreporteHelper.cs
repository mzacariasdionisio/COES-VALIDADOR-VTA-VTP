using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_UMBRALREPORTE
    /// </summary>
    public class CmUmbralreporteHelper : HelperBase
    {
        public CmUmbralreporteHelper(): base(Consultas.CmUmbralreporteSql)
        {
        }

        public CmUmbralreporteDTO Create(IDataReader dr)
        {
            CmUmbralreporteDTO entity = new CmUmbralreporteDTO();

            int iCmurcodi = dr.GetOrdinal(this.Cmurcodi);
            if (!dr.IsDBNull(iCmurcodi)) entity.Cmurcodi = Convert.ToInt32(dr.GetValue(iCmurcodi));

            int iCmurminbarra = dr.GetOrdinal(this.Cmurminbarra);
            if (!dr.IsDBNull(iCmurminbarra)) entity.Cmurminbarra = dr.GetDecimal(iCmurminbarra);

            int iCmurmaxbarra = dr.GetOrdinal(this.Cmurmaxbarra);
            if (!dr.IsDBNull(iCmurmaxbarra)) entity.Cmurmaxbarra = dr.GetDecimal(iCmurmaxbarra);

            int iCmurminenergia = dr.GetOrdinal(this.Cmurminenergia);
            if (!dr.IsDBNull(iCmurminenergia)) entity.Cmurminenergia = dr.GetDecimal(iCmurminenergia);

            int iCmurmaxenergia = dr.GetOrdinal(this.Cmurmaxenergia);
            if (!dr.IsDBNull(iCmurmaxenergia)) entity.Cmurmaxenergia = dr.GetDecimal(iCmurmaxenergia);

            int iCmurminconges = dr.GetOrdinal(this.Cmurminconges);
            if (!dr.IsDBNull(iCmurminconges)) entity.Cmurminconges = dr.GetDecimal(iCmurminconges);

            int iCmurmaxconges = dr.GetOrdinal(this.Cmurmaxconges);
            if (!dr.IsDBNull(iCmurmaxconges)) entity.Cmurmaxconges = dr.GetDecimal(iCmurmaxconges);

            int iCmurdiferencia = dr.GetOrdinal(this.Cmurdiferencia);
            if (!dr.IsDBNull(iCmurdiferencia)) entity.Cmurdiferencia = dr.GetDecimal(iCmurdiferencia);

            int iCmurestado = dr.GetOrdinal(this.Cmurestado);
            if (!dr.IsDBNull(iCmurestado)) entity.Cmurestado = dr.GetString(iCmurestado);

            int iCmurvigencia = dr.GetOrdinal(this.Cmurvigencia);
            if (!dr.IsDBNull(iCmurvigencia)) entity.Cmurvigencia = dr.GetDateTime(iCmurvigencia);

            int iCmurexpira = dr.GetOrdinal(this.Cmurexpira);
            if (!dr.IsDBNull(iCmurexpira)) entity.Cmurexpira = dr.GetDateTime(iCmurexpira);

            int iCmurusucreacion = dr.GetOrdinal(this.Cmurusucreacion);
            if (!dr.IsDBNull(iCmurusucreacion)) entity.Cmurusucreacion = dr.GetString(iCmurusucreacion);

            int iCmurfeccreacion = dr.GetOrdinal(this.Cmurfeccreacion);
            if (!dr.IsDBNull(iCmurfeccreacion)) entity.Cmurfeccreacion = dr.GetDateTime(iCmurfeccreacion);

            int iCmurusumodificacion = dr.GetOrdinal(this.Cmurusumodificacion);
            if (!dr.IsDBNull(iCmurusumodificacion)) entity.Cmurusumodificacion = dr.GetString(iCmurusumodificacion);

            int iCmurfecmodificacion = dr.GetOrdinal(this.Cmurfecmodificacion);
            if (!dr.IsDBNull(iCmurfecmodificacion)) entity.Cmurfecmodificacion = dr.GetDateTime(iCmurfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Cmurcodi = "CMURCODI";
        public string Cmurminbarra = "CMURMINBARRA";
        public string Cmurmaxbarra = "CMURMAXBARRA";
        public string Cmurminenergia = "CMURMINENERGIA";
        public string Cmurmaxenergia = "CMURMAXENERGIA";
        public string Cmurminconges = "CMURMINCONGES";
        public string Cmurmaxconges = "CMURMAXCONGES";
        public string Cmurdiferencia = "CMURDIFERENCIA";
        public string Cmurestado = "CMURESTADO";
        public string Cmurvigencia = "CMURVIGENCIA";
        public string Cmurexpira = "CMUREXPIRA";
        public string Cmurusucreacion = "CMURUSUCREACION";
        public string Cmurfeccreacion = "CMURFECCREACION";
        public string Cmurusumodificacion = "CMURUSUMODIFICACION";
        public string Cmurfecmodificacion = "CMURFECMODIFICACION";

        #endregion

        public string SqlObtenerHistorico
        {
            get { return base.GetSqlXml("ObtenerHistorico"); }
        }
    }
}
