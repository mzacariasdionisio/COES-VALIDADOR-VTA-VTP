using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_PUBLICACION
    /// </summary>
    public class WbPublicacionHelper : HelperBase
    {
        public WbPublicacionHelper()
            : base(Consultas.WbPublicacionSql)
        {
        }

        public WbPublicacionDTO Create(IDataReader dr)
        {
            WbPublicacionDTO entity = new WbPublicacionDTO();

            int iPubliccodi = dr.GetOrdinal(this.Publiccodi);
            if (!dr.IsDBNull(iPubliccodi)) entity.Publiccodi = Convert.ToInt32(dr.GetValue(iPubliccodi));

            int iPublicnombre = dr.GetOrdinal(this.Publicnombre);
            if (!dr.IsDBNull(iPublicnombre)) entity.Publicnombre = dr.GetString(iPublicnombre);

            int iPublicestado = dr.GetOrdinal(this.Publicestado);
            if (!dr.IsDBNull(iPublicestado)) entity.Publicestado = dr.GetString(iPublicestado);

            int iPublicplantilla = dr.GetOrdinal(this.Publicplantilla);
            if (!dr.IsDBNull(iPublicplantilla)) entity.Publicplantilla = dr.GetString(iPublicplantilla);

            int iPublicasunto = dr.GetOrdinal(this.Publicasunto);
            if (!dr.IsDBNull(iPublicasunto)) entity.Publicasunto = dr.GetString(iPublicasunto);

            int iPublicemail = dr.GetOrdinal(this.Publicemail);
            if (!dr.IsDBNull(iPublicemail)) entity.Publicemail = dr.GetString(iPublicemail);

            int iPublicemail1 = dr.GetOrdinal(this.Publicemail1);
            if (!dr.IsDBNull(iPublicemail1)) entity.Publicemail1 = dr.GetString(iPublicemail1);

            int iPublicemail2 = dr.GetOrdinal(this.Publicemail2);
            if (!dr.IsDBNull(iPublicemail2)) entity.Publicemail2 = dr.GetString(iPublicemail2);

            int iAreacode = dr.GetOrdinal(this.Areacode);
            if (!dr.IsDBNull(iAreacode)) entity.Areacode = Convert.ToInt32(dr.GetValue(iAreacode));

            return entity;
        }


        #region Mapeo de Campos

        public string Publiccodi = "PUBLICCODI";
        public string Publicnombre = "PUBLICNOMBRE";
        public string Publicestado = "PUBLICESTADO";
        public string Publicplantilla = "PUBLICPLANTILLA";
        public string Publicasunto = "PUBLICASUNTO";
        public string Publicemail = "PUBLICEMAIL";
        public string Publicemail1 = "PUBLICEMAIL1";
        public string Publicemail2 = "PUBLICEMAIL2";
        public string Areacode = "AREACODE";
        public string Areaname = "AREANAME";

        #endregion
    }
}
