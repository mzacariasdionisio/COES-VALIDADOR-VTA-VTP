using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MMM_JUSTIFICACION
    /// </summary>
    public class MmmJustificacionHelper : HelperBase
    {
        public MmmJustificacionHelper()
            : base(Consultas.MmmJustificacionSql)
        {
        }

        public MmmJustificacionDTO Create(IDataReader dr)
        {
            MmmJustificacionDTO entity = new MmmJustificacionDTO();

            int iMjustcodi = dr.GetOrdinal(this.Mjustcodi);
            if (!dr.IsDBNull(iMjustcodi)) entity.Mjustcodi = Convert.ToInt32(dr.GetValue(iMjustcodi));

            int iImmecodi = dr.GetOrdinal(this.Immecodi);
            if (!dr.IsDBNull(iImmecodi)) entity.Immecodi = Convert.ToInt32(dr.GetValue(iImmecodi));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMjustfecha = dr.GetOrdinal(this.Mjustfecha);
            if (!dr.IsDBNull(iMjustfecha)) entity.Mjustfecha = dr.GetDateTime(iMjustfecha);

            int iMjustdescripcion = dr.GetOrdinal(this.Mjustdescripcion);
            if (!dr.IsDBNull(iMjustdescripcion)) entity.Mjustdescripcion = dr.GetString(iMjustdescripcion);

            int iMjustusucreacion = dr.GetOrdinal(this.Mjustusucreacion);
            if (!dr.IsDBNull(iMjustusucreacion)) entity.Mjustusucreacion = dr.GetString(iMjustusucreacion);

            int iMjustfeccreacion = dr.GetOrdinal(this.Mjustfeccreacion);
            if (!dr.IsDBNull(iMjustfeccreacion)) entity.Mjustfeccreacion = dr.GetDateTime(iMjustfeccreacion);

            int iMjustusumodificacion = dr.GetOrdinal(this.Mjustusumodificacion);
            if (!dr.IsDBNull(iMjustusumodificacion)) entity.Mjustusumodificacion = dr.GetString(iMjustusumodificacion);

            int iMjustfecmodificacion = dr.GetOrdinal(this.Mjustfecmodificacion);
            if (!dr.IsDBNull(iMjustfecmodificacion)) entity.Mjustfecmodificacion = dr.GetDateTime(iMjustfecmodificacion);

            return entity;
        }
        
        #region Mapeo de Campos

        public string Mjustcodi = "MJUSTCODI";
        public string Immecodi = "IMMECODI";
        public string Barrcodi = "BARRCODI";
        public string Grupocodi = "GRUPOCODI";
        public string Emprcodi = "EMPRCODI";
        public string Mjustfecha = "MJUSTFECHA";
        public string Mjustdescripcion = "MJUSTDESCRIPCION";
        public string Mjustusucreacion = "MJUSTUSUCREACION";
        public string Mjustfeccreacion = "MJUSTFECCREACION";
        public string Mjustusumodificacion = "MJUSTUSUMODIFICACION";
        public string Mjustfecmodificacion = "MJUSTFECMODIFICACION";

        #endregion

        public string SqlListByFechaAndIndicador
        {
            get { return base.GetSqlXml("ListByFechaAndIndicador"); }
        }
    }
}
