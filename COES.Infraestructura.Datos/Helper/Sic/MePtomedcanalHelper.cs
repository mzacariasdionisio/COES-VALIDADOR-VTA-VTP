using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_PTOMEDCANAL
    /// </summary>
    public class MePtomedcanalHelper : HelperBase
    {
        public MePtomedcanalHelper()
            : base(Consultas.MePtomedcanalSql)
        {
        }

        public MePtomedcanalDTO Create(IDataReader dr)
        {
            MePtomedcanalDTO entity = new MePtomedcanalDTO();

            int iPcancodi = dr.GetOrdinal(this.Pcancodi);
            if (!dr.IsDBNull(iPcancodi)) entity.Pcancodi = Convert.ToInt32(dr.GetValue(iPcancodi));

            int iCanalcodi = dr.GetOrdinal(this.Canalcodi);
            if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iPmedclfestado = dr.GetOrdinal(this.Pcanestado);
            if (!dr.IsDBNull(iPmedclfestado)) entity.Pcanestado = dr.GetString(iPmedclfestado);

            int iPmedclusucreacion = dr.GetOrdinal(this.Pcanusucreacion);
            if (!dr.IsDBNull(iPmedclusucreacion)) entity.Pcanusucreacion = dr.GetString(iPmedclusucreacion);

            int iPmedclfeccreacion = dr.GetOrdinal(this.Pcanfeccreacion);
            if (!dr.IsDBNull(iPmedclfeccreacion)) entity.Pcanfeccreacion = dr.GetDateTime(iPmedclfeccreacion);

            int iPmedclusumodificacion = dr.GetOrdinal(this.Pcanusumodificacion);
            if (!dr.IsDBNull(iPmedclusumodificacion)) entity.Pcanusumodificacion = dr.GetString(iPmedclusumodificacion);

            int iPmedclfecmodificacion = dr.GetOrdinal(this.Pcanfecmodificacion);
            if (!dr.IsDBNull(iPmedclfecmodificacion)) entity.Pcanfecmodificacion = dr.GetDateTime(iPmedclfecmodificacion);

            int iPcanfactor = dr.GetOrdinal(this.Pcanfactor);
            if (!dr.IsDBNull(iPcanfactor)) entity.Pcanfactor = dr.GetDecimal(iPcanfactor);

            return entity;
        }

        #region Mapeo de Campos

        public string Pcancodi = "PCANCODI";
        public string Canalcodi = "CANALCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Pcanestado = "PCANESTADO";
        public string Pcanusucreacion = "PCANUSUCREACION";
        public string Pcanfeccreacion = "PCANFECCREACION";
        public string Pcanusumodificacion = "PCANUSUMODIFICACION";
        public string Pcanfecmodificacion = "PCANFECMODIFICACION";
        public string Pcanfactor = "PCANFACTOR";

        public string Equipadre = "EQUIPADRE";
        public string Central = "CENTRAL";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Equiabrev = "EQUIABREV";
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Famcodi = "Famcodi";
        public string Famnomb = "Famnomb";
        public string Famabrev = "Famabrev";
        public string Tipoinfoabrev = "TIPOINFOABREV";

        public string Canalnomb = "CANALNOMB";
        public string Canaliccp = "CANALICCP";
        public string Canalunidad = "CANALUNIDAD";
        public string Canalabrev = "CANALABREV";
        public string CanalPointType = "CANALPOINTTYPE";
        public string Zonacodi = "ZONACODI";
        public string Zonanomb = "ZONANOMB";
        public string Zonaabrev = "ZONAABREV";
        public string TrEmprcodi = "TREMPRCODI";
        public string TrEmprnomb = "TREMPRNOMB";
        public string TrEmprabrev = "TREMPRABREV";

        #region MigracionSGOCOES-GrupoB
        public string Origlectnombre = "Origlectnombre";
        #endregion

        public string Ptomedielenomb = "Ptomedielenomb";

        #endregion

        public string SqlListarEquivalencia
        {
            get { return base.GetSqlXml("ListarEquivalencia"); }
        }

        public string SqlObtenerEquivalencia
        {
            get { return base.GetSqlXml("ObtenerEquivalencia"); }
        }

    }
}
