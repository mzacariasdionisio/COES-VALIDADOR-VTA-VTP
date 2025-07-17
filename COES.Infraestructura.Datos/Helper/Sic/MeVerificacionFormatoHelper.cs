using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_VERIFICACION_FORMATO
    /// </summary>
    public class MeVerificacionFormatoHelper : HelperBase
    {
        public MeVerificacionFormatoHelper()
            : base(Consultas.MeVerificacionFormatoSql)
        {
        }

        public MeVerificacionFormatoDTO Create(IDataReader dr)
        {
            MeVerificacionFormatoDTO entity = new MeVerificacionFormatoDTO();

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iVerifcodi = dr.GetOrdinal(this.Verifcodi);
            if (!dr.IsDBNull(iVerifcodi)) entity.Verifcodi = Convert.ToInt32(dr.GetValue(iVerifcodi));

            int iFmtverifestado = dr.GetOrdinal(this.Fmtverifestado);
            if (!dr.IsDBNull(iFmtverifestado)) entity.Fmtverifestado = dr.GetString(iFmtverifestado);

            int iFmtverifusucreacion = dr.GetOrdinal(this.Fmtverifusucreacion);
            if (!dr.IsDBNull(iFmtverifusucreacion)) entity.Fmtverifusucreacion = dr.GetString(iFmtverifusucreacion);

            int iFmtveriffeccreacion = dr.GetOrdinal(this.Fmtveriffeccreacion);
            if (!dr.IsDBNull(iFmtveriffeccreacion)) entity.Fmtveriffeccreacion = dr.GetDateTime(iFmtveriffeccreacion);

            int iFmtverifusumodificacion = dr.GetOrdinal(this.Fmtverifusumodificacion);
            if (!dr.IsDBNull(iFmtverifusumodificacion)) entity.Fmtverifusumodificacion = dr.GetString(iFmtverifusumodificacion);

            int iFmtveriffecmodificacion = dr.GetOrdinal(this.Fmtveriffecmodificacion);
            if (!dr.IsDBNull(iFmtveriffecmodificacion)) entity.Fmtveriffecmodificacion = dr.GetDateTime(iFmtveriffecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Formatcodi = "FORMATCODI";
        public string Verifcodi = "VERIFCODI";
        public string Fmtverifestado = "FMTVRFESTADO";
        public string Fmtverifusucreacion = "FMTVRFUSUCREACION";
        public string Fmtveriffeccreacion = "FMTVRFFECCREACION";
        public string Fmtverifusumodificacion = "FMTVRFUSUMODIFICACION";
        public string Fmtveriffecmodificacion = "FMTVRFFECMODIFICACION";
        public string Formatnomb = "FORMATNOMBRE";
        public string Verifnomb = "VERIFNOMB";

        #endregion

        public string SqlListByFormato
        {
            get { return GetSqlXml("ListByFormato"); }
        }
    }
}
