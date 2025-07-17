using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PTOSUMINISTRADOR
    /// </summary>
    public class MePtosuministradorHelper : HelperBase
    {
        public MePtosuministradorHelper(): base(Consultas.MePtosuministradorSql)
        {
        }

        public MePtosuministradorDTO Create(IDataReader dr)
        {
            MePtosuministradorDTO entity = new MePtosuministradorDTO();

            int iPtosucodi = dr.GetOrdinal(this.Ptosucodi);
            if (!dr.IsDBNull(iPtosucodi)) entity.Ptosucodi = Convert.ToInt32(dr.GetValue(iPtosucodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtosufechainicio = dr.GetOrdinal(this.Ptosufechainicio);
            if (!dr.IsDBNull(iPtosufechainicio)) entity.Ptosufechainicio = dr.GetDateTime(iPtosufechainicio);

            int iPtosufechafin = dr.GetOrdinal(this.Ptosufechafin);
            if (!dr.IsDBNull(iPtosufechafin)) entity.Ptosufechafin = dr.GetDateTime(iPtosufechafin);

            int iPtosuusucreacion = dr.GetOrdinal(this.Ptosuusucreacion);
            if (!dr.IsDBNull(iPtosuusucreacion)) entity.Ptosuusucreacion = dr.GetString(iPtosuusucreacion);

            int iPtosufeccreacion = dr.GetOrdinal(this.Ptosufeccreacion);
            if (!dr.IsDBNull(iPtosufeccreacion)) entity.Ptosufeccreacion = dr.GetDateTime(iPtosufeccreacion);

            int iPtosuusumodificacion = dr.GetOrdinal(this.Ptosuusumodificacion);
            if (!dr.IsDBNull(iPtosuusumodificacion)) entity.Ptosuusumodificacion = dr.GetString(iPtosuusumodificacion);

            int iPtosufecmodificacion = dr.GetOrdinal(this.Ptosufecmodificacion);
            if (!dr.IsDBNull(iPtosufecmodificacion)) entity.Ptosufecmodificacion = dr.GetDateTime(iPtosufecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ptosucodi = "PTOSUCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Emprcodi = "EMPRCODI";
        public string Ptosufechainicio = "PTOSUFECHAINICIO";
        public string Ptosufechafin = "PTOSUFECHAFIN";
        public string Ptosuusucreacion = "PTOSUUSUCREACION";
        public string Ptosufeccreacion = "PTOSUFECCREACION";
        public string Ptosuusumodificacion = "PTOSUUSUMODIFICACION";
        public string Ptosufecmodificacion = "PTOSUFECMODIFICACION";

        //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
        public string Ptomedidesc = "PTOMEDIDESC";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string VigEmprcodi = "VIG_EMPRCODI";
        public string PerPtosucodi = "PER_PTOSUCODI";
        public string PerEmprcodi = "PER_EMPRCODI";
        public string SelEmprcodi = "SEL_EMPRCODI";
        //- JDEL Fin
       

        #endregion

        //- pr16.JDEL - Inicio 21/10/2016: Cambio para atender el requerimiento.
        public string SqlListaEditorPtoSuministro
        {
            get { return base.GetSqlXml("ListaEditorPtoSuministro"); }
        }

        public string SqlGetByPtoPeriodo
        {
            get { return base.GetSqlXml("GetByPtoPeriodo"); }
        }
        //- JDEL Fin

        public string SqlObtenerSuministradorVigente
        {
            get { return base.GetSqlXml("ObtenerSuministradorVigente"); }
        }

        #region Rechazo Carga
        public string SqlListaSuministradoresRechazoCarga
        {
            get { return base.GetSqlXml("ListaSuministradoresRechazoCarga"); }
        }
        #endregion
    }
}
