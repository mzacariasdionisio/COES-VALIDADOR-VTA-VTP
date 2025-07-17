using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_HISTORICO_STOCKCOMBUST
    /// </summary>
    public class IndHistoricoStockCombustHelper : HelperBase
    {
        #region Mapeo de Campos
        public string Hststkcodi = "HSTSTKCODI";
        public string Stkcmtcodi = "STKCMTCODI";
        public string Ipericodi = "IPERICODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Tipoinfocodi = "TIPOINFOCODI";
        public string Hststkperiodo = "HSTSTKPERIODO";
        public string Hststkempresa = "HSTSTKEMPRESA";
        public string Hststkcentral = "HSTSTKCENTRAL";
        public string Hststkunidad = "HSTSTKUNIDAD";
        public string Hststktipoinfo = "HSTSTKTIPOINFO";
        public string Hststkfecha = "HSTSTKFECHA";
        public string Hststkoriginal = "HSTSTKORIGINAL";
        public string Hststkmodificado = "HSTSTKMODIFICADO";
        public string Hststktipaccion = "HSTSTKTIPACCION";
        public string Hststkusucreacion = "HSTSTKUSUCREACION";
        public string Hststkfeccreacion = "HSTSTKFECCREACION";
        public string Hststkusumodificacion = "HSTSTKUSUMODIFICACION";
        public string Hststkfecmodificacion = "HSTSTKFECMODIFICACION";
        #endregion

        public IndHistoricoStockCombustHelper() : base(Consultas.IndHistoricoStockCombustSql)
        {
        }

        public IndHistoricoStockCombustDTO Create(IDataReader dr)
        {
            IndHistoricoStockCombustDTO entity = new IndHistoricoStockCombustDTO();

            int iHststkcodi = dr.GetOrdinal(this.Hststkcodi);
            if (!dr.IsDBNull(iHststkcodi)) entity.Hststkcodi = Convert.ToInt32(dr.GetValue(iHststkcodi));

            int iStkcmtcodi = dr.GetOrdinal(this.Stkcmtcodi);
            if (!dr.IsDBNull(iStkcmtcodi)) entity.Stkcmtcodi = Convert.ToInt32(dr.GetValue(iStkcmtcodi));

            int iIpericodi = dr.GetOrdinal(this.Ipericodi);
            if (!dr.IsDBNull(iIpericodi)) entity.Ipericodi = Convert.ToInt32(dr.GetValue(iIpericodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iTipoinfocodi = dr.GetOrdinal(this.Tipoinfocodi);
            if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

            int iHststkperiodo = dr.GetOrdinal(this.Hststkperiodo);
            if (!dr.IsDBNull(iHststkperiodo)) entity.Hststkperiodo = dr.GetString(iHststkperiodo);

            int iHststkempresa = dr.GetOrdinal(this.Hststkempresa);
            if (!dr.IsDBNull(iHststkempresa)) entity.Hststkempresa = dr.GetString(iHststkempresa);

            int iHststkcentral = dr.GetOrdinal(this.Hststkcentral);
            if (!dr.IsDBNull(iHststkcentral)) entity.Hststkcentral = dr.GetString(iHststkcentral);

            int iHststkunidad = dr.GetOrdinal(this.Hststkunidad);
            if (!dr.IsDBNull(iHststkunidad)) entity.Hststkunidad = dr.GetString(iHststkunidad);

            int iHststktipoinfo = dr.GetOrdinal(this.Hststktipoinfo);
            if (!dr.IsDBNull(iHststktipoinfo)) entity.Hststktipoinfo = dr.GetString(iHststktipoinfo);

            int iHststkfecha = dr.GetOrdinal(this.Hststkfecha);
            if (!dr.IsDBNull(iHststkfecha)) entity.Hststkfecha = dr.GetDateTime(iHststkfecha);

            int iHststkoriginal = dr.GetOrdinal(this.Hststkoriginal);
            if (!dr.IsDBNull(iHststkoriginal)) entity.Hststkoriginal = dr.GetString(iHststkoriginal);

            int iHststkmodificado = dr.GetOrdinal(this.Hststkmodificado);
            if (!dr.IsDBNull(iHststkmodificado)) entity.Hststkmodificado = dr.GetString(iHststkmodificado);

            int iHststktipaccion = dr.GetOrdinal(this.Hststktipaccion);
            if (!dr.IsDBNull(iHststktipaccion)) entity.Hststktipaccion = dr.GetString(iHststktipaccion);

            int iHststkusucreacion = dr.GetOrdinal(this.Hststkusucreacion);
            if (!dr.IsDBNull(iHststkusucreacion)) entity.Hststkusucreacion = dr.GetString(iHststkusucreacion);

            int iHststkfeccreacion = dr.GetOrdinal(this.Hststkfeccreacion);
            if (!dr.IsDBNull(iHststkfeccreacion)) entity.Hststkfeccreacion = dr.GetDateTime(iHststkfeccreacion);

            int iHststkusumodificacion = dr.GetOrdinal(this.Hststkusumodificacion);
            if (!dr.IsDBNull(iHststkusumodificacion)) entity.Hststkusumodificacion = dr.GetString(iHststkusumodificacion);

            int iHststkfecmodificacion = dr.GetOrdinal(this.Hststkfecmodificacion);
            if (!dr.IsDBNull(iHststkfecmodificacion)) entity.Hststkfecmodificacion = dr.GetDateTime(iHststkfecmodificacion);

            return entity;
        }

    }
}
