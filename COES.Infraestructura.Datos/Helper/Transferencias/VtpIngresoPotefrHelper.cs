using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;
using System;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTP_INGRESO_POTEFR
    /// </summary>
    public class VtpIngresoPotefrHelper : HelperBase
    {
        public VtpIngresoPotefrHelper(): base(Consultas.VtpIngresoPotefrSql)
        {
        }

        public VtpIngresoPotefrDTO Create(IDataReader dr)
        {
            VtpIngresoPotefrDTO entity = new VtpIngresoPotefrDTO();

            int iIpefrcodi = dr.GetOrdinal(this.Ipefrcodi);
            if (!dr.IsDBNull(iIpefrcodi)) entity.Ipefrcodi = Convert.ToInt32(dr.GetValue(iIpefrcodi));

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.Pericodi = Convert.ToInt32(dr.GetValue(iPericodi));

            int iRecpotcodi = dr.GetOrdinal(this.Recpotcodi);
            if (!dr.IsDBNull(iRecpotcodi)) entity.Recpotcodi = Convert.ToInt32(dr.GetValue(iRecpotcodi));

            int iIpefrintervalo = dr.GetOrdinal(this.Ipefrintervalo);
            if (!dr.IsDBNull(iIpefrintervalo)) entity.Ipefrintervalo = Convert.ToInt32(dr.GetValue(iIpefrintervalo));

            int iIpefrdia = dr.GetOrdinal(this.Ipefrdia);
            if (!dr.IsDBNull(iIpefrdia)) entity.Ipefrdia = Convert.ToInt32(dr.GetValue(iIpefrdia));

            int iIpefrdescripcion = dr.GetOrdinal(this.Ipefrdescripcion);
            if (!dr.IsDBNull(iIpefrdescripcion)) entity.Ipefrdescripcion = dr.GetString(iIpefrdescripcion);

            int iIpefrusucreacion = dr.GetOrdinal(this.Ipefrusucreacion);
            if (!dr.IsDBNull(iIpefrusucreacion)) entity.Ipefrusucreacion = dr.GetString(iIpefrusucreacion);

            int iIpefrfeccreacion = dr.GetOrdinal(this.Ipefrfeccreacion);
            if (!dr.IsDBNull(iIpefrfeccreacion)) entity.Ipefrfeccreacion = dr.GetDateTime(iIpefrfeccreacion);

            int iIpefrusumodificacion = dr.GetOrdinal(this.Ipefrusumodificacion);
            if (!dr.IsDBNull(iIpefrusumodificacion)) entity.Ipefrusumodificacion = dr.GetString(iIpefrusumodificacion);

            int iIpefrfecmodificacion = dr.GetOrdinal(this.Ipefrfecmodificacion);
            if (!dr.IsDBNull(iIpefrfecmodificacion)) entity.Ipefrfecmodificacion = dr.GetDateTime(iIpefrfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Ipefrcodi = "IPEFRCODI";
        public string Pericodi = "PERICODI";
        public string Recpotcodi = "RECPOTCODI";
        public string Ipefrintervalo = "IPEFRINTERVALO";      
        public string Ipefrdia = "IPEFRDIA";
        public string Ipefrdescripcion = "IPEFRDESCRIPCION";
        public string Ipefrusucreacion = "IPEFRUSUCREACION";
        public string Ipefrfeccreacion = "IPEFRFECCREACION";
        public string Ipefrusumodificacion = "IPEFRUSUMODIFICACION";
        public string Ipefrfecmodificacion = "IPEFRFECMODIFICACION";

        #endregion

        public string SqlGetResultSave
        {
            get { return base.GetSqlXml("GetResultSave"); }
        }

        public string SqlGetResultUpdate
        {
            get { return base.GetSqlXml("GetResultUpdate"); }
        }


        public string SqlDeleteByCriteria
        {
            get { return base.GetSqlXml("DeleteByCriteria"); }
        }
    }
}
