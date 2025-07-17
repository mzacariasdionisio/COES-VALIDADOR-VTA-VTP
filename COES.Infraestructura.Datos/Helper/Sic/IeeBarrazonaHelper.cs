using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IEE_BARRAZONA
    /// </summary>
    public class IeeBarrazonaHelper : HelperBase
    {
        public IeeBarrazonaHelper(): base(Consultas.IeeBarrazonaSql)
        {
        }

        public IeeBarrazonaDTO Create(IDataReader dr)
        {
            IeeBarrazonaDTO entity = new IeeBarrazonaDTO();

            int iBarrzcodi = dr.GetOrdinal(this.Barrzcodi);
            if (!dr.IsDBNull(iBarrzcodi)) entity.Barrzcodi = Convert.ToInt32(dr.GetValue(iBarrzcodi));

            int iBarrzarea = dr.GetOrdinal(this.Barrzarea);
            if (!dr.IsDBNull(iBarrzarea)) entity.Barrzarea = Convert.ToInt32(dr.GetValue(iBarrzarea));

            int iBarrcodi = dr.GetOrdinal(this.Barrcodi);
            if (!dr.IsDBNull(iBarrcodi)) entity.Barrcodi = Convert.ToInt32(dr.GetValue(iBarrcodi));

            int iMrepcodi = dr.GetOrdinal(this.Mrepcodi);
            if (!dr.IsDBNull(iMrepcodi)) entity.Mrepcodi = Convert.ToInt32(dr.GetValue(iMrepcodi));

            int iBarrzdesc = dr.GetOrdinal(this.Barrzdesc);
            if (!dr.IsDBNull(iBarrzdesc)) entity.Barrzdesc = dr.GetString(iBarrzdesc);

            return entity;
        }


        #region Mapeo de Campos

        public string Barrzcodi = "BARRZCODI";
        public string Barrzarea = "BARRZAREA";
        public string Barrcodi = "BARRCODI";
        public string Mrepcodi = "MREPCODI";
        public string Barrzdesc = "BARRZDESC";
        public string Barrnombre = "BARRNOMBRE";
        public string Barrzusumodificacion = "BARRZUSUMODIFICACION";
        public string Barrzfecmodificacion = "BARRZFECMODIFICACION";

        #endregion

        public string SqlGetBarrasPorAreas
        {
            get { return base.GetSqlXml("GetBarrasPorArea"); }
        }

        public string SqlObtenerAgrupacionPorZona
        {
            get { return base.GetSqlXml("ObtenerAgrupacionPorZona"); }
        }

        public string SqlObtenerBarrasPorAgrupacion
        {
            get { return base.GetSqlXml("ObtenerBarrasPorAgrupacion"); }
        }

        public string SqlEliminarAgrupacion
        {
            get { return base.GetSqlXml("EliminarAgrupacion"); }
        }

        public string SqlValidarExistencia
        {
            get { return base.GetSqlXml("ValidarExistencia"); }
        }

        public string SqlValidarExistenciaEdicion
        {
            get { return base.GetSqlXml("ValidarExistenciaEdicion"); }
        }
    }
}
