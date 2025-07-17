using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ST_SISTEMATRANS
    /// </summary>
    public class StSistematransHelper : HelperBase
    {
        public StSistematransHelper()
            : base(Consultas.StSistematransSql)
        {
        }

        public StSistematransDTO Create(IDataReader dr)
        {
            StSistematransDTO entity = new StSistematransDTO();

            int iSistrncodi = dr.GetOrdinal(this.Sistrncodi);
            if (!dr.IsDBNull(iSistrncodi)) entity.Sistrncodi = Convert.ToInt32(dr.GetValue(iSistrncodi));

            int iStrecacodi = dr.GetOrdinal(this.Strecacodi);
            if (!dr.IsDBNull(iStrecacodi)) entity.Strecacodi = Convert.ToInt32(dr.GetValue(iStrecacodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iSistrnnombre = dr.GetOrdinal(this.Sistrnnombre);
            if (!dr.IsDBNull(iSistrnnombre)) entity.Sistrnnombre = dr.GetString(iSistrnnombre);

            int iSistrnsucreacion = dr.GetOrdinal(this.Sistrnsucreacion);
            if (!dr.IsDBNull(iSistrnsucreacion)) entity.Sistrnsucreacion = dr.GetString(iSistrnsucreacion);

            int iSistrnfeccreacion = dr.GetOrdinal(this.Sistrnfeccreacion);
            if (!dr.IsDBNull(iSistrnfeccreacion)) entity.Sistrnfeccreacion = dr.GetDateTime(iSistrnfeccreacion);

            int iSistrnsumodificacion = dr.GetOrdinal(this.Sistrnsumodificacion);
            if (!dr.IsDBNull(iSistrnsumodificacion)) entity.Sistrnsumodificacion = dr.GetString(iSistrnsumodificacion);

            int iSistrnfecmodificacion = dr.GetOrdinal(this.Sistrnfecmodificacion);
            if (!dr.IsDBNull(iSistrnfecmodificacion)) entity.Sistrnfecmodificacion = dr.GetDateTime(iSistrnfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Sistrncodi = "SISTRNCODI";
        public string Strecacodi = "STRECACODI";
        public string Emprcodi = "EMPRCODI";
        public string Sistrnnombre = "SISTRNNOMBRE";
        public string Sistrnsucreacion = "SISTRNSUCREACION";
        public string Sistrnfeccreacion = "SISTRNFECCREACION";
        public string Sistrnsumodificacion = "SISTRNSUMODIFICACION";
        public string Sistrnfecmodificacion = "SISTRNFECMODIFICACION";
        //variable de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Stcompcodelemento = "STCOMPCODELEMENTO";
        public string Stcompnomelemento = "STCOMPNOMELEMENTO";
        public string Stcompimpcompensacion = "STCOMPIMPCOMPENSACION";
        public string Barrnombre1 = "BARRNOMBRE1";
        public string Barrnombre2 = "BARRNOMBRE2";

        #endregion

        //para consultas
        public string SqlGetBySisTransNombre
        {
            get { return base.GetSqlXml("GetBySisTransNombre"); }
        }

        public string SqlListByStSistemaTransVersion
        {
            get { return base.GetSqlXml("ListByStSistemaTransVersion"); }
        }

        public string SqlDeleteVersion
        {
            get { return base.GetSqlXml("DeleteVersion"); }
        }

        public string SqlListByStSistemaTransReporte
        {
            get { return base.GetSqlXml("ListByStSistemaTransReporte"); }
        }
    }
}
