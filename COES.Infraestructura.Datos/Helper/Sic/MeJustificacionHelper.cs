using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_JUSTIFICACION
    /// </summary>
    public class MeJustificacionHelper : HelperBase
    {
        public MeJustificacionHelper(): base(Consultas.MeJustificacionSql)
        {
        }

        public MeJustificacionDTO Create(IDataReader dr)
        {
            MeJustificacionDTO entity = new MeJustificacionDTO();

            int iJustcodi = dr.GetOrdinal(this.Justcodi);
            if (!dr.IsDBNull(iJustcodi)) entity.Justcodi = Convert.ToInt32(dr.GetValue(iJustcodi));

            int iEnviocodi = dr.GetOrdinal(this.Enviocodi);
            if (!dr.IsDBNull(iEnviocodi)) entity.Enviocodi = Convert.ToInt32(dr.GetValue(iEnviocodi));

            //ASSETEC 201909: Nuevo atributo para distinguir la fuente de dato
            int iLectcodi = dr.GetOrdinal(this.Lectcodi);
            if (!dr.IsDBNull(iLectcodi)) entity.Lectcodi = Convert.ToInt32(dr.GetValue(iLectcodi));
            else entity.Lectcodi = 0;

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iJustfeccreacion = dr.GetOrdinal(this.Justfeccreacion);
            if (!dr.IsDBNull(iJustfeccreacion)) entity.Justfeccreacion = dr.GetDateTime(iJustfeccreacion);

            int iJustusucreacion = dr.GetOrdinal(this.Justusucreacion);
            if (!dr.IsDBNull(iJustusucreacion)) entity.Justusucreacion = dr.GetString(iJustusucreacion);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iJustdescripcionotros = dr.GetOrdinal(this.Justdescripcionotros);
            if (!dr.IsDBNull(iJustdescripcionotros)) entity.Justdescripcionotros = dr.GetString(iJustdescripcionotros);

            int iJustfechainicio = dr.GetOrdinal(this.Justfechainicio);
            if (!dr.IsDBNull(iJustfechainicio)) entity.Justfechainicio = dr.GetDateTime(iJustfechainicio);

            int iJustfechafin = dr.GetOrdinal(this.Justfechafin);
            if (!dr.IsDBNull(iJustfechafin)) entity.Justfechafin = dr.GetDateTime(iJustfechafin);

            return entity;
        }


        #region Mapeo de Campos

        public string Justcodi = "JUSTCODI";
        public string Enviocodi = "ENVIOCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Justfeccreacion = "JUSTFECCREACION";
        public string Justusucreacion = "JUSTUSUCREACION";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Justdescripcionotros = "JUSTDESCRIPCIONOTROS";
        public string Justfechainicio = "JUSTFECHAINICIO";
        public string Justfechafin = "JUSTFECHAFIN";
        //ASSETEC 201909: Nuevo atributo para distinguir la fuente de dato
        public string Lectcodi = "LECTCODI";

        public string SqlListByIdEnvio
        {
            get { return base.GetSqlXml("ListByIdEnvio"); }
        }
        //ASSETEC 201909: Nueva consulta
        public string SqlListByIdEnvioPtoMedicodi
        {
            get { return base.GetSqlXml("ListByIdEnvioPtoMedicodi"); }
        }

        #endregion
    }
}
