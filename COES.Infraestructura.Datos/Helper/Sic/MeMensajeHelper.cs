using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla ME_MENSAJE
    /// </summary>
    public class MeMensajeHelper : HelperBase
    {
        public MeMensajeHelper() : base(Consultas.MeMensajeSql)
        {
        }

        public MeMensajeDTO Create(IDataReader dr)
        {
            MeMensajeDTO entity = new MeMensajeDTO();

            int iMsjcodi = dr.GetOrdinal(this.Msjcodi);
            if (!dr.IsDBNull(iMsjcodi)) entity.Msjcodi = Convert.ToInt32(dr.GetValue(iMsjcodi));

            int iFormatcodi = dr.GetOrdinal(this.Formatcodi);
            if (!dr.IsDBNull(iFormatcodi)) entity.Formatcodi = Convert.ToInt32(dr.GetValue(iFormatcodi));

            int iFormatnombre = dr.GetOrdinal(this.Formatnombre);
            if (!dr.IsDBNull(iFormatnombre)) entity.Formatnombre = dr.GetString(iFormatnombre);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iMsjestado = dr.GetOrdinal(this.Msjestado);
            if (!dr.IsDBNull(iMsjestado)) entity.Msjestado = dr.GetString(iMsjestado);

            int iMsjdescripcion = dr.GetOrdinal(this.Msjdescripcion);
            if (!dr.IsDBNull(iMsjdescripcion)) entity.Msjdescripcion = dr.GetString(iMsjdescripcion);
            entity.Msjdescripcion = entity.Msjdescripcion ?? "";

            int iMsjusucreacion = dr.GetOrdinal(this.Msjusucreacion);
            if (!dr.IsDBNull(iMsjusucreacion)) entity.Msjusucreacion = dr.GetString(iMsjusucreacion);
            entity.Msjusucreacion = entity.Msjusucreacion ?? "";

            int iMsjfeccreacion = dr.GetOrdinal(this.Msjfeccreacion);
            if (!dr.IsDBNull(iMsjfeccreacion)) entity.Msjfeccreacion = dr.GetDateTime(iMsjfeccreacion);

            int iMsjusumodificacion = dr.GetOrdinal(this.Msjusumodificacion);
            if (!dr.IsDBNull(iMsjusumodificacion)) entity.Msjusumodificacion = dr.GetString(iMsjusumodificacion);

            int iMsjfecmodificacion = dr.GetOrdinal(this.Msjfecmodificacion);
            if (!dr.IsDBNull(iMsjfecmodificacion)) entity.Msjfecmodificacion = dr.GetDateTime(iMsjfecmodificacion);

            int iMsjfecperiodo = dr.GetOrdinal(this.Msjfecperiodo);
            if (!dr.IsDBNull(iMsjfecperiodo)) entity.Msjfecperiodo = dr.GetDateTime(iMsjfecperiodo);

            return entity;
        }


        #region Mapeo de Campos

        public string Msjcodi = "MSJCODI";
        public string Formatcodi = "FORMATCODI";
        public string Formatnombre = "FORMATNOMBRE";
        public string Emprcodi = "EMPRCODI";
        public string Msjestado = "MSJESTADO";
        public string Msjdescripcion = "MSJDESCRIPCION";
        public string Msjusucreacion = "MSJUSUCREACION";
        public string Msjfeccreacion = "MSJFECCREACION";
        public string Msjusumodificacion = "MSJUSUMODIFICACION";
        public string Msjfecmodificacion = "MSJFECMODIFICACION";
        public string Msjfecperiodo = "MSJFECPERIODO";

        public string Emprnomb = "EMPRNOMB";

        #endregion

        public string SqlGetListaMensajes
        {
            get { return base.GetSqlXml("GetListaMensajes"); }
        }
        //
        public string SqlGetListaTodosMensajes
        {
            get { return base.GetSqlXml("GetListaTodosMensajes"); }
        }
        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

    }
}
