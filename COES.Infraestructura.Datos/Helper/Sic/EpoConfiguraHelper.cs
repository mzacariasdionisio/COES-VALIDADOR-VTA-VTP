using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPO_CONFIGURA
    /// </summary>
    public class EpoConfiguraHelper : HelperBase
    {
        public EpoConfiguraHelper(): base(Consultas.EpoConfiguraSql)
        {
        }

        public EpoConfiguraDTO Create(IDataReader dr)
        {
            EpoConfiguraDTO entity = new EpoConfiguraDTO();

            int iConfplazorevcoesporv = dr.GetOrdinal(this.Confplazorevcoesporv);
            if (!dr.IsDBNull(iConfplazorevcoesporv)) entity.Confplazorevcoesporv = Convert.ToInt32(dr.GetValue(iConfplazorevcoesporv));

            int iConfplazorevcoesvenc = dr.GetOrdinal(this.Confplazorevcoesvenc);
            if (!dr.IsDBNull(iConfplazorevcoesvenc)) entity.Confplazorevcoesvenc = Convert.ToInt32(dr.GetValue(iConfplazorevcoesvenc));

            int iConfplazolevobsporv = dr.GetOrdinal(this.Confplazolevobsporv);
            if (!dr.IsDBNull(iConfplazolevobsporv)) entity.Confplazolevobsporv = Convert.ToInt32(dr.GetValue(iConfplazolevobsporv));

            int iConfplazolevobsvenc = dr.GetOrdinal(this.Confplazolevobsvenc);
            if (!dr.IsDBNull(iConfplazolevobsvenc)) entity.Confplazolevobsvenc = Convert.ToInt32(dr.GetValue(iConfplazolevobsvenc));

            int iConfplazoalcancesvenc = dr.GetOrdinal(this.Confplazoalcancesvenc);
            if (!dr.IsDBNull(iConfplazoalcancesvenc)) entity.Confplazoalcancesvenc = Convert.ToInt32(dr.GetValue(iConfplazoalcancesvenc));

            int iConfplazoverificacionvenc = dr.GetOrdinal(this.Confplazoverificacionvenc);
            if (!dr.IsDBNull(iConfplazoverificacionvenc)) entity.Confplazoverificacionvenc = Convert.ToInt32(dr.GetValue(iConfplazoverificacionvenc));

            int iConfplazorevterceroinvporv = dr.GetOrdinal(this.Confplazorevterceroinvporv);
            if (!dr.IsDBNull(iConfplazorevterceroinvporv)) entity.Confplazorevterceroinvporv = Convert.ToInt32(dr.GetValue(iConfplazorevterceroinvporv));

            int iConfplazorevterceroinvvenc = dr.GetOrdinal(this.Confplazorevterceroinvvenc);
            if (!dr.IsDBNull(iConfplazorevterceroinvvenc)) entity.Confplazorevterceroinvvenc = Convert.ToInt32(dr.GetValue(iConfplazorevterceroinvvenc));

            int iConfplazoenvestterceroinvporv = dr.GetOrdinal(this.Confplazoenvestterceroinvporv);
            if (!dr.IsDBNull(iConfplazoenvestterceroinvporv)) entity.Confplazoenvestterceroinvporv = Convert.ToInt32(dr.GetValue(iConfplazoenvestterceroinvporv));

            int iConfplazoenvestterceroinvvenc = dr.GetOrdinal(this.Confplazoenvestterceroinvvenc);
            if (!dr.IsDBNull(iConfplazoenvestterceroinvvenc)) entity.Confplazoenvestterceroinvvenc = Convert.ToInt32(dr.GetValue(iConfplazoenvestterceroinvvenc));

            int iConfindigestionsnpepo = dr.GetOrdinal(this.Confindigestionsnpepo);
            if (!dr.IsDBNull(iConfindigestionsnpepo)) entity.Confindigestionsnpepo = Convert.ToInt32(dr.GetValue(iConfindigestionsnpepo));

            int iConfindiporcatencionepo = dr.GetOrdinal(this.Confindiporcatencionepo);
            if (!dr.IsDBNull(iConfindiporcatencionepo)) entity.Confindiporcatencionepo = Convert.ToInt32(dr.GetValue(iConfindiporcatencionepo));

            int iConfindigestionsnpeo = dr.GetOrdinal(this.Confindigestionsnpeo);
            if (!dr.IsDBNull(iConfindigestionsnpeo)) entity.Confindigestionsnpeo = Convert.ToInt32(dr.GetValue(iConfindigestionsnpeo));

            int iConfindiporcatencioneo = dr.GetOrdinal(this.Confindiporcatencioneo);
            if (!dr.IsDBNull(iConfindiporcatencioneo)) entity.Confindiporcatencioneo = Convert.ToInt32(dr.GetValue(iConfindiporcatencioneo));

            int iConfcodi = dr.GetOrdinal(this.Confcodi);
            if (!dr.IsDBNull(iConfcodi)) entity.Confcodi = Convert.ToInt32(dr.GetValue(iConfcodi));

            int iConfdescripcion = dr.GetOrdinal(this.Confdescripcion);
            if (!dr.IsDBNull(iConfdescripcion)) entity.Confdescripcion = dr.GetString(iConfdescripcion);

            int iConfplazoverificacionvencabs = dr.GetOrdinal(this.Confplazoverificacionvencabs);
            if (!dr.IsDBNull(iConfplazoverificacionvencabs)) entity.Confplazoverificacionvencabs = Convert.ToInt32(dr.GetValue(iConfplazoverificacionvencabs));


            return entity;
        }


        #region Mapeo de Campos

        public string Confplazorevcoesporv = "CONFPLAZOREVCOESPORV";
        public string Confplazorevcoesvenc = "CONFPLAZOREVCOESVENC";
        public string Confplazolevobsporv = "CONFPLAZOLEVOBSPORV";
        public string Confplazolevobsvenc = "CONFPLAZOLEVOBSVENC";
        public string Confplazoalcancesvenc = "CONFPLAZOALCANCESVENC";
        public string Confplazoverificacionvenc = "CONFPLAZOVERIFICACIONVENC";
        public string Confplazorevterceroinvporv = "CONFPLAZOREVTERCEROINVPORV";
        public string Confplazorevterceroinvvenc = "CONFPLAZOREVTERCEROINVVENC";
        public string Confplazoenvestterceroinvporv = "CONFPLAZOENVESTTERCEROINVPORV";
        public string Confplazoenvestterceroinvvenc = "CONFPLAZOENVESTTERCEROINVVENC";
        public string Confindigestionsnpepo = "CONFINDIGESTIONSNPEPO";
        public string Confindiporcatencionepo = "CONFINDIPORCATENCIONEPO";
        public string Confindigestionsnpeo = "CONFINDIGESTIONSNPEO";
        public string Confindiporcatencioneo = "CONFINDIPORCATENCIONEO";
        public string Confcodi = "CONFCODI";
        public string Confdescripcion = "CONFDESCRIPCION";
        public string Confplazoverificacionvencabs = "CONFPLAZOVERIFICACIONVENCABS";
        


        #endregion
    }
}
