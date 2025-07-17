using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla RNT_TIPO_INTERRUPCION
    /// </summary>
    public class RntTipoInterrupcionHelper : HelperBase
    {
        public RntTipoInterrupcionHelper(): base(Consultas.RntTipoInterrupcionSql)
        {
        }

        public RntTipoInterrupcionDTO Create(IDataReader dr)
        {
            RntTipoInterrupcionDTO entity = new RntTipoInterrupcionDTO();

            int iTipointcodi = dr.GetOrdinal(this.Tipointcodi);
            if (!dr.IsDBNull(iTipointcodi)) entity.TipoIntCodi = Convert.ToInt32(dr.GetValue(iTipointcodi));

            int iTipointnombre = dr.GetOrdinal(this.Tipointnombre);
            if (!dr.IsDBNull(iTipointnombre)) entity.TipoIntNombre = dr.GetString(iTipointnombre);

            int iTipointusuariocreacion = dr.GetOrdinal(this.Tipointusuariocreacion);
            if (!dr.IsDBNull(iTipointusuariocreacion)) entity.TipoIntUsuarioCreacion = dr.GetString(iTipointusuariocreacion);

            int iTipointfechacreacion = dr.GetOrdinal(this.Tipointfechacreacion);
            if (!dr.IsDBNull(iTipointfechacreacion)) entity.TipoIntFechaCreacion = dr.GetDateTime(iTipointfechacreacion);

            int iTipointusuarioupdate = dr.GetOrdinal(this.Tipointusuarioupdate);
            if (!dr.IsDBNull(iTipointusuarioupdate)) entity.TipoIntUsuarioUpdate = dr.GetString(iTipointusuarioupdate);

            int iTipointfechaupdate = dr.GetOrdinal(this.Tipointfechaupdate);
            if (!dr.IsDBNull(iTipointfechaupdate)) entity.TipoIntFechaUpdate = dr.GetDateTime(iTipointfechaupdate);

            return entity;
        }

         
        #region Mapeo de Campos

        public string Tipointcodi = "TIPINTCODI";
        public string Tipointnombre = "TIPINTNOMBRE";
        public string Tipointusuariocreacion = "TIPINTUSUARIOCREACION";
        public string Tipointfechacreacion = "TIPINTFECHACREACION";
        public string Tipointusuarioupdate = "TIPINTUSUARIOUPDATE";
        public string Tipointfechaupdate = "TIPINTFECHAUPDATE";
       
        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

    }
}
