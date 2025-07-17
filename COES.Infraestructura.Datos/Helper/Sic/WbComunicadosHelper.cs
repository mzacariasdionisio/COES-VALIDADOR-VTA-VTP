using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla WB_COMUNICADOS
    /// </summary>
    public class WbComunicadosHelper : HelperBase
    {
        public WbComunicadosHelper(): base(Consultas.WbComunicadosSql)
        {
        }

        public WbComunicadosDTO Create(IDataReader dr)
        {
            WbComunicadosDTO entity = new WbComunicadosDTO();

            int iComcodi = dr.GetOrdinal(this.Comcodi);
            if (!dr.IsDBNull(iComcodi)) entity.Comcodi = Convert.ToInt32(dr.GetValue(iComcodi));

            int iComfecha = dr.GetOrdinal(this.Comfecha);
            if (!dr.IsDBNull(iComfecha)) entity.Comfecha = dr.GetDateTime(iComfecha);

            int iComtitulo = dr.GetOrdinal(this.Comtitulo);
            if (!dr.IsDBNull(iComtitulo)) entity.Comtitulo = dr.GetString(iComtitulo);

            int iComresumen = dr.GetOrdinal(this.Comresumen);
            if (!dr.IsDBNull(iComresumen)) entity.Comresumen = dr.GetString(iComresumen);

            int iComdesc = dr.GetOrdinal(this.Comdesc);
            if (!dr.IsDBNull(iComdesc)) entity.Comdesc = dr.GetString(iComdesc);

            int iComlink = dr.GetOrdinal(this.Comlink);
            if (!dr.IsDBNull(iComlink)) entity.Comlink = dr.GetString(iComlink);

            int iComestado = dr.GetOrdinal(this.Comestado);
            if (!dr.IsDBNull(iComestado)) entity.Comestado = dr.GetString(iComestado);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iComfechaini = dr.GetOrdinal(this.Comfechaini);
            if (!dr.IsDBNull(iComfechaini)) entity.Comfechaini = dr.GetDateTime(iComfechaini);

            int iComfechafin = dr.GetOrdinal(this.Comfechafin);
            if (!dr.IsDBNull(iComfechafin)) entity.Comfechafin = dr.GetDateTime(iComfechafin);

            int iModcodi = dr.GetOrdinal(this.Modcodi);
            if (!dr.IsDBNull(iModcodi)) entity.Modcodi = Convert.ToInt32(dr.GetValue(iModcodi));

            int iComorden = dr.GetOrdinal(this.Comorden);
            if (!dr.IsDBNull(iComorden)) entity.Comorden = Convert.ToInt32(dr.GetValue(iComorden));

            int iComposition = dr.GetOrdinal(this.Composition);
            if (!dr.IsDBNull(iComposition)) entity.Composition = Convert.ToInt32(dr.GetValue(iComposition));

            int iComtipo = dr.GetOrdinal(this.Comtipo);
            if (!dr.IsDBNull(iComtipo)) entity.Comtipo = dr.GetString(iComtipo);

            return entity;
        }


        #region Mapeo de Campos

        public string Comcodi = "COMCODI";
        public string Comfecha = "COMFECHA";
        public string Comtitulo = "COMTITULO";
        public string Comresumen = "COMRESUMEN";
        public string Comdesc = "COMDESC";
        public string Comlink = "COMLINK";
        public string Comestado = "COMESTADO";
        public string Lastdate = "LASTDATE";
        public string Lastuser = "LASTUSER";
        public string Comfechaini = "COMFECHAINI";
        public string Comfechafin = "COMFECHAFIN";
        public string Modcodi = "MODCODI";
        public string Comtipo = "COMTIPO";

        #region MigracionSGOCOES-GrupoB
        public string Comorden = "Comorden";
        public string Composition = "Composition";
        #endregion

        #endregion
    }
}
