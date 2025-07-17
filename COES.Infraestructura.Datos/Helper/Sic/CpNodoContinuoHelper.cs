using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CP_NODO_CONTINUO
    /// </summary>
    public class CpNodoContinuoHelper : HelperBase
    {
        public CpNodoContinuoHelper() : base(Consultas.CpNodoContinuoSql)
        {
        }

        public CpNodoContinuoDTO Create(IDataReader dr)
        {
            CpNodoContinuoDTO entity = new CpNodoContinuoDTO();

            int iCpnodocodi = dr.GetOrdinal(this.Cpnodocodi);
            if (!dr.IsDBNull(iCpnodocodi)) entity.Cpnodocodi = Convert.ToInt32(dr.GetValue(iCpnodocodi));

            int iCpnodoestado = dr.GetOrdinal(this.Cpnodoestado);
            if (!dr.IsDBNull(iCpnodoestado)) entity.Cpnodoestado = dr.GetString(iCpnodoestado);

            int iCparbcodi = dr.GetOrdinal(this.Cparbcodi);
            if (!dr.IsDBNull(iCparbcodi)) entity.Cparbcodi = Convert.ToInt32(dr.GetValue(iCparbcodi));

            int iCpnodoconverge = dr.GetOrdinal(this.Cpnodoconverge);
            if (!dr.IsDBNull(iCpnodoconverge)) entity.Cpnodoconverge = dr.GetString(iCpnodoconverge);

            int iCpnodoflagcondterm = dr.GetOrdinal(this.Cpnodoflagcondterm);
            if (!dr.IsDBNull(iCpnodoflagcondterm)) entity.Cpnodoflagcondterm = dr.GetString(iCpnodoflagcondterm);

            int iCpnodoflagconcompr = dr.GetOrdinal(this.Cpnodoflagconcompr);
            if (!dr.IsDBNull(iCpnodoflagconcompr)) entity.Cpnodoflagconcompr = dr.GetString(iCpnodoflagconcompr);

            int iCpnodoflagsincompr = dr.GetOrdinal(this.Cpnodoflagsincompr);
            if (!dr.IsDBNull(iCpnodoflagsincompr)) entity.Cpnodoflagsincompr = dr.GetString(iCpnodoflagsincompr);

            int iCpnodoflagrer = dr.GetOrdinal(this.Cpnodoflagrer);
            if (!dr.IsDBNull(iCpnodoflagrer)) entity.Cpnodoflagrer = dr.GetString(iCpnodoflagrer);

            int iCpnodocarpeta = dr.GetOrdinal(this.Cpnodocarpeta);
            if (!dr.IsDBNull(iCpnodocarpeta)) entity.Cpnodocarpeta = dr.GetString(iCpnodocarpeta);

            int iCpnodofeciniproceso = dr.GetOrdinal(this.Cpnodofeciniproceso);
            if (!dr.IsDBNull(iCpnodofeciniproceso)) entity.Cpnodofeciniproceso = dr.GetDateTime(iCpnodofeciniproceso);

            int iCpnodofecfinproceso = dr.GetOrdinal(this.Cpnodofecfinproceso);
            if (!dr.IsDBNull(iCpnodofecfinproceso)) entity.Cpnodofecfinproceso = dr.GetDateTime(iCpnodofecfinproceso);

            int iCpnodomsjproceso = dr.GetOrdinal(this.Cpnodomsjproceso);
            if (!dr.IsDBNull(iCpnodomsjproceso)) entity.Cpnodomsjproceso = dr.GetString(iCpnodomsjproceso);

            int iCpnodoidtopguardado= dr.GetOrdinal(this.Cpnodoidtopguardado);
            if (!dr.IsDBNull(iCpnodoidtopguardado)) entity.Cpnodoidtopguardado = Convert.ToInt32(dr.GetValue(iCpnodoidtopguardado));

            int iCpnodonumero = dr.GetOrdinal(this.Cpnodonumero);
            if (!dr.IsDBNull(iCpnodonumero)) entity.Cpnodonumero = Convert.ToInt32(dr.GetValue(iCpnodonumero));

            return entity;
        }

        #region Mapeo de Campos

        public string Cpnodocodi = "CPNODOCODI";
        public string Cpnodoestado = "CPNODOESTADO";
        public string Cparbcodi = "CPARBCODI";
        public string Cpnodoconverge = "CPNODOCONVERGE";
        public string Cpnodoflagcondterm = "CPNODOFLAGCONDTERM";
        public string Cpnodoflagconcompr = "CPNODOFLAGCONCOMPR";
        public string Cpnodoflagsincompr = "CPNODOFLAGSINCOMPR";
        public string Cpnodoflagrer = "CPNODOFLAGRER";
        public string Cpnodocarpeta = "CPNODOCARPETA";
        public string Cpnodofeciniproceso = "CPNODOFECINIPROCESO";
        public string Cpnodofecfinproceso = "CPNODOFECFINPROCESO";
        public string Cpnodomsjproceso = "CPNODOMSJPROCESO";
        public string Cpnodoidtopguardado = "CPNODOIDTOPGUARDADO";
        public string Cpnodonumero = "CPNODONUMERO";

        #endregion

        public string Topiniciohora = "TOPINICIOHORA";
        public string Topcodi = "TOPCODI";
        public string Topnombre = "TOPNOMBRE";

        public string SqlListaPorArbol
        {
            get { return base.GetSqlXml("ListaPorArbol"); }
        }

        public string SqlGetByNumero
        {
            get { return base.GetSqlXml("GetByNumero"); }
        }
    }
}
