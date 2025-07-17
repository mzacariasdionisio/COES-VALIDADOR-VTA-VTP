using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla NR_SOBRECOSTO
    /// </summary>
    public class NrSobrecostoHelper : HelperBase
    {
        public NrSobrecostoHelper(): base(Consultas.NrSobrecostoSql)
        {
        }

        public NrSobrecostoDTO Create(IDataReader dr)
        {
            NrSobrecostoDTO entity = new NrSobrecostoDTO();

            int iNrsccodi = dr.GetOrdinal(this.Nrsccodi);
            if (!dr.IsDBNull(iNrsccodi)) entity.Nrsccodi = Convert.ToInt32(dr.GetValue(iNrsccodi));

            //int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            //if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iNrscfecha = dr.GetOrdinal(this.Nrscfecha);
            if (!dr.IsDBNull(iNrscfecha)) entity.Nrscfecha = dr.GetDateTime(iNrscfecha);

            int iNrsccodespacho0 = dr.GetOrdinal(this.Nrsccodespacho0);
            if (!dr.IsDBNull(iNrsccodespacho0)) entity.Nrsccodespacho0 = dr.GetDecimal(iNrsccodespacho0);

            int iNrsccodespacho1 = dr.GetOrdinal(this.Nrsccodespacho1);
            if (!dr.IsDBNull(iNrsccodespacho1)) entity.Nrsccodespacho1 = dr.GetDecimal(iNrsccodespacho1);

            int iNrscsobrecosto = dr.GetOrdinal(this.Nrscsobrecosto);
            if (!dr.IsDBNull(iNrscsobrecosto)) entity.Nrscsobrecosto = dr.GetDecimal(iNrscsobrecosto);

            int iNrscnota = dr.GetOrdinal(this.Nrscnota);
            if (!dr.IsDBNull(iNrscnota)) entity.Nrscnota = dr.GetString(iNrscnota);
            
            int iNrsceliminado = dr.GetOrdinal(this.Nrsceliminado);
            if (!dr.IsDBNull(iNrsceliminado)) entity.Nrsceliminado = dr.GetString(iNrsceliminado);

            int iNrscpadre = dr.GetOrdinal(this.Nrscpadre);
            if (!dr.IsDBNull(iNrscpadre)) entity.Nrscpadre = Convert.ToInt32(dr.GetValue(iNrscpadre));

            int iNrscusucreacion = dr.GetOrdinal(this.Nrscusucreacion);
            if (!dr.IsDBNull(iNrscusucreacion)) entity.Nrscusucreacion = dr.GetString(iNrscusucreacion);

            int iNrscfeccreacion = dr.GetOrdinal(this.Nrscfeccreacion);
            if (!dr.IsDBNull(iNrscfeccreacion)) entity.Nrscfeccreacion = dr.GetDateTime(iNrscfeccreacion);

            int iNrscusumodificacion = dr.GetOrdinal(this.Nrscusumodificacion);
            if (!dr.IsDBNull(iNrscusumodificacion)) entity.Nrscusumodificacion = dr.GetString(iNrscusumodificacion);

            int iNrscfecmodificacion = dr.GetOrdinal(this.Nrscfecmodificacion);
            if (!dr.IsDBNull(iNrscfecmodificacion)) entity.Nrscfecmodificacion = dr.GetDateTime(iNrscfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Nrsccodi = "NRSCCODI";
        //public string Grupocodi = "GRUPOCODI";
        public string Nrscfecha = "NRSCFECHA";
        public string Nrsccodespacho0 = "NRSCCODESPACHO0";
        public string Nrsccodespacho1 = "NRSCCODESPACHO1";
        public string Nrscsobrecosto = "NRSCSOBRECOSTO";
        public string Nrscnota = "NRSCNOTA";        
        public string Nrsceliminado = "NRSCELIMINADO";
        public string Nrscpadre = "NRSCPADRE";
        public string Nrscusucreacion = "NRSCUSUCREACION";
        public string Nrscfeccreacion = "NRSCFECCREACION";
        public string Nrscusumodificacion = "NRSCUSUMODIFICACION";
        public string Nrscfecmodificacion = "NRSCFECMODIFICACION";
        public string Gruponomb = "GRUPONOMB";

        public string ObtenerListado
        {
            get { return base.GetSqlXml("ObtenerListado"); }
        }

        public string TotalRegistros
        {
            get { return base.GetSqlXml("TotalRegistros"); }
        }

        #endregion
    }
}
