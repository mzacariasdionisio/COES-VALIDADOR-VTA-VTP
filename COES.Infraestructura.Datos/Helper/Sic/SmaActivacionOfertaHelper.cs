using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_ACTIVACION_OFERTA
    /// </summary>
    public class SmaActivacionOfertaHelper : HelperBase
    {
        public SmaActivacionOfertaHelper(): base(Consultas.SmaActivacionOfertaSql)
        {
        }

        public SmaActivacionOfertaDTO Create(IDataReader dr)
        {
            SmaActivacionOfertaDTO entity = new SmaActivacionOfertaDTO();

            int iSmapaccodi = dr.GetOrdinal(this.Smapaccodi);
            if (!dr.IsDBNull(iSmapaccodi)) entity.Smapaccodi = Convert.ToInt32(dr.GetValue(iSmapaccodi));

            int iSmapacfecha = dr.GetOrdinal(this.Smapacfecha);
            if (!dr.IsDBNull(iSmapacfecha)) entity.Smapacfecha = dr.GetDateTime(iSmapacfecha);

            int iSmapacestado = dr.GetOrdinal(this.Smapacestado);
            if (!dr.IsDBNull(iSmapacestado)) entity.Smapacestado = dr.GetString(iSmapacestado);

            int iSmapacusucreacion = dr.GetOrdinal(this.Smapacusucreacion);
            if (!dr.IsDBNull(iSmapacusucreacion)) entity.Smapacusucreacion = dr.GetString(iSmapacusucreacion);

            int iSmapacfeccreacion = dr.GetOrdinal(this.Smapacfeccreacion);
            if (!dr.IsDBNull(iSmapacfeccreacion)) entity.Smapacfeccreacion = dr.GetDateTime(iSmapacfeccreacion);

            int iSmapacusumodificacion = dr.GetOrdinal(this.Smapacusumodificacion);
            if (!dr.IsDBNull(iSmapacusumodificacion)) entity.Smapacusumodificacion = dr.GetString(iSmapacusumodificacion);

            int iSmapacfecmodificacion = dr.GetOrdinal(this.Smapacfecmodificacion);
            if (!dr.IsDBNull(iSmapacfecmodificacion)) entity.Smapacfecmodificacion = dr.GetDateTime(iSmapacfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Smapaccodi = "SMAPACCODI";
        public string Smapacfecha = "SMAPACFECHA";
        public string Smapacestado = "SMAPACESTADO";
        public string Smapacusucreacion = "SMAPACUSUCREACION";
        public string Smapacfeccreacion = "SMAPACFECCREACION";
        public string Smapacusumodificacion = "SMAPACUSUMODIFICACION";
        public string Smapacfecmodificacion = "SMAPACFECMODIFICACION";

        #endregion

        public string SqlListarPorFechas
        {
            get { return base.GetSqlXml("ListarPorFechas"); }
        }
    }
}
