using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_AMPLIACIONFECHA
    /// </summary>
    public class PmpoObraHelper : HelperBase
    {
        public PmpoObraHelper(): base(Consultas.PmpoObraSql)
        {
        }

        public PmpoObraDTO Create(IDataReader dr)
        {
            PmpoObraDTO entity = new PmpoObraDTO();

            int iObracodi = dr.GetOrdinal(this.Obracodi);
            if (!dr.IsDBNull(iObracodi)) entity.Obracodi = Convert.ToInt32(dr.GetValue(iObracodi));

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iTObracodi = dr.GetOrdinal(this.TObracodi);
            if (!dr.IsDBNull(iTObracodi)) entity.TObracodi = Convert.ToInt32(dr.GetValue(iTObracodi));

            int iTObradescripcion = dr.GetOrdinal(this.TObradescripcion);
            if (!dr.IsDBNull(iTObradescripcion)) entity.TObradescripcion = dr.GetString(iTObradescripcion);

            int IEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(IEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(IEmprcodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iEquinomb = dr.GetOrdinal(this.Equinomb);
            if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
            
            int iObrafechaplanificada = dr.GetOrdinal(this.Obrafechaplanificada);
            if (!dr.IsDBNull(iObrafechaplanificada)) entity.Obrafechaplanificada = dr.GetDateTime(iObrafechaplanificada);

            int iObradescripcion = dr.GetOrdinal(this.Obradescripcion);
            if (!dr.IsDBNull(iObradescripcion)) entity.Obradescripcion = dr.GetString(iObradescripcion);

            int iObrausucreacion = dr.GetOrdinal(this.Obrausucreacion);
            if (!dr.IsDBNull(iObrausucreacion)) entity.Obrausucreacion = dr.GetString(iObrausucreacion);

            int iObrafeccreacion = dr.GetOrdinal(this.Obrafeccreacion);
            if (!dr.IsDBNull(iObrafeccreacion)) entity.Obrafeccreacion = dr.GetDateTime(iObrafeccreacion);

            int iObrausumodificacion = dr.GetOrdinal(this.Obrausumodificacion);
            if (!dr.IsDBNull(iObrausumodificacion)) entity.Obrausumodificacion = dr.GetString(iObrausumodificacion);

            int iObrafecmodificacion = dr.GetOrdinal(this.Obrafecmodificacion);
            if (!dr.IsDBNull(iObrafecmodificacion)) entity.Obrafecmodificacion = dr.GetDateTime(iObrafecmodificacion);

            int iObraflagformat = dr.GetOrdinal(this.ObraFlagFormat);
            if (!dr.IsDBNull(iObraflagformat)) entity.ObraFlagFormat = Convert.ToInt32(dr.GetValue(iObraflagformat));

            int iGruponomb = dr.GetOrdinal(this.GrupoNomb);
            if (!dr.IsDBNull(iGruponomb)) entity.GrupoNomb = dr.GetString(iGruponomb);

            int iBarranomb = dr.GetOrdinal(this.BarraNomb);
            if (!dr.IsDBNull(iBarranomb)) entity.BarraNomb = dr.GetString(iBarranomb);


            return entity;
        }

        #region Mapeo de Campos

        public string Emprnomb = "EMPRNOMB";
        public string Obracodi = "OBRACODI";
        public string ObraDtcodi = "OBRADTCODI";
        public string TObracodi = "TOBRACODI";
        public string TObradescripcion = "TOBRADESCRIPCION";
        public string Equicodi = "EQUICODI";
        public string Equinomb = "EQUINOMB";
        public string Emprcodi = "EMPRCODI";
        public string Obrafechaplanificada = "OBRAFECHAPLANIFICADA";
        public string Obradescripcion = "OBRADESCRIPCION";
        public string Obrausucreacion = "OBRAUSUCREACION";
        public string Obrafeccreacion = "OBRAFECCREACION";
        public string Obrausumodificacion = "OBRAUSUMODIFICACION";
        public string Obrafecmodificacion = "OBRAFECMODIFICACION";
        public string ObraFlagFormat = "OBRAFLAGFORMAT";
        public string GrupoNomb = "GRUPONOMB";
        public string BarraNomb = "BARRANOMB";
        public string EquiNomb = "EQUINOMB";


        #endregion


        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListObras
        {
            get { return base.GetSqlXml("ListObras"); }
        }

        public string SqlListGeneracion
        {
            get { return base.GetSqlXml("ListGeneracion"); }
        }

        public string SqlGetByIdGeneracion
        {
            get { return base.GetSqlXml("GetByIdGeneracion"); }
        }

        public string SqlListPlanificadas
        {
            get { return base.GetSqlXml("ListPlanificadas"); }
        }

    }
}
