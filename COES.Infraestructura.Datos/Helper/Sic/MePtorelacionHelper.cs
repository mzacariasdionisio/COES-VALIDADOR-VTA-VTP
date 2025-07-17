using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PTORELACION
    /// </summary>
    public class MePtorelacionHelper : HelperBase
    {
        public MePtorelacionHelper(): base(Consultas.MePtorelacionSql)
        {
        }

        public MePtorelacionDTO Create(IDataReader dr)
        {
            MePtorelacionDTO entity = new MePtorelacionDTO();

            int iPtorelcodi = dr.GetOrdinal(this.Ptorelcodi);
            if (!dr.IsDBNull(iPtorelcodi)) entity.Ptorelcodi = Convert.ToInt32(dr.GetValue(iPtorelcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iPtorelpunto1 = dr.GetOrdinal(this.Ptorelpunto1);
            if (!dr.IsDBNull(iPtorelpunto1)) entity.Ptorelpunto1 = Convert.ToInt32(dr.GetValue(iPtorelpunto1));

            int iPtorelpunto2 = dr.GetOrdinal(this.Ptorelpunto2);
            if (!dr.IsDBNull(iPtorelpunto2)) entity.Ptorelpunto2 = Convert.ToInt32(dr.GetValue(iPtorelpunto2));

            int iPtoreltipo = dr.GetOrdinal(this.Ptoreltipo);
            if (!dr.IsDBNull(iPtoreltipo)) entity.Ptoreltipo = dr.GetString(iPtoreltipo);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        #region Mapeo de Campos

        public string Ptorelcodi = "PTORELCODI";
        public string Equicodi = "EQUICODI";
        public string Ptorelpunto1 = "PTORELPUNTO1";
        public string Ptorelpunto2 = "PTORELPUNTO2";
        public string Ptoreltipo = "PTORELTIPO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Origlectcodi = "ORIGLECTCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Centralcodi = "CENTRALCODI";
        public string Centralnomb = "CENTRALNOMB";
        public string Seleccion = "SELECCION";
        public string CampoH = "H";

        public string SqlObtenerEmpresas
        {
            get { return base.GetSqlXml("ObtenerEmpresas"); }
        }

        public string SqlObtenerCentrales
        {
            get { return base.GetSqlXml("ObtenerCentrales"); }
        }

        public string SqlObtenerPuntosMedicion
        {
            get { return base.GetSqlXml("ObtenerPuntosRelacion"); }
        }

        public string SqlObtenerDatosDespacho
        {
            get { return base.GetSqlXml("ObtenerDatosDespacho"); }
        }

        public string SqlObtenerPuntosRPF
        {
            get { return base.GetSqlXml("ObtenerPuntosMedicionRPF"); }
        }

        #endregion
    }
}
