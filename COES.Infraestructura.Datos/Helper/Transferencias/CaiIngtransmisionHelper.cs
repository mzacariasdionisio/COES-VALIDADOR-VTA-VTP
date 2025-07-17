using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CAI_INGTRANSMISION
    /// </summary>
    public class CaiIngtransmisionHelper : HelperBase
    {
        public CaiIngtransmisionHelper(): base(Consultas.CaiIngtransmisionSql)
        {
        }

        public CaiIngtransmisionDTO Create(IDataReader dr)
        {
            CaiIngtransmisionDTO entity = new CaiIngtransmisionDTO();

            int iCaitrcodi = dr.GetOrdinal(this.Caitrcodi);
            if (!dr.IsDBNull(iCaitrcodi)) entity.Caitrcodi = Convert.ToInt32(dr.GetValue(iCaitrcodi));

            int iCaiajcodi = dr.GetOrdinal(this.Caiajcodi);
            if (!dr.IsDBNull(iCaiajcodi)) entity.Caiajcodi = Convert.ToInt32(dr.GetValue(iCaiajcodi));

            int iCaitrcalidadinfo = dr.GetOrdinal(this.Caitrcalidadinfo);
            if (!dr.IsDBNull(iCaitrcalidadinfo)) entity.Caitrcalidadinfo = dr.GetString(iCaitrcalidadinfo);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iPtomedicodi = dr.GetOrdinal(this.Ptomedicodi);
            if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

            int iCaitrmes = dr.GetOrdinal(this.Caitrmes);
            if (!dr.IsDBNull(iCaitrmes)) entity.Caitrmes = Convert.ToInt32(dr.GetValue(iCaitrmes));

            int iCaitringreso = dr.GetOrdinal(this.Caitringreso);
            if (!dr.IsDBNull(iCaitringreso)) entity.Caitringreso = dr.GetDecimal(iCaitringreso);

            int iCaitrtipoinfo = dr.GetOrdinal(this.Caitrtipoinfo);
            if (!dr.IsDBNull(iCaitrtipoinfo)) entity.Caitrtipoinfo = dr.GetString(iCaitrtipoinfo);

            int iCaitrusucreacion = dr.GetOrdinal(this.Caitrusucreacion);
            if (!dr.IsDBNull(iCaitrusucreacion)) entity.Caitrusucreacion = dr.GetString(iCaitrusucreacion);

            int iCaitrfeccreacion = dr.GetOrdinal(this.Caitrfeccreacion);
            if (!dr.IsDBNull(iCaitrfeccreacion)) entity.Caitrfeccreacion = dr.GetDateTime(iCaitrfeccreacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Caitrcodi = "CAITRCODI";
        public string Caiajcodi = "CAIAJCODI";
        public string Caitrcalidadinfo = "CAITRCALIDADINFO";
        public string Emprcodi = "EMPRCODI";
        public string Ptomedicodi = "PTOMEDICODI";
        public string Caitrmes = "CAITRMES";
        public string Caitringreso = "CAITRINGRESO";
        public string Caitrtipoinfo = "CAITRTIPOINFO";
        public string Caitrusucreacion = "CAITRUSUCREACION";
        public string Caitrfeccreacion = "CAITRFECCREACION";

        #endregion

        public string SqlSaveAsSelectMeMedicion1
        {
            get { return base.GetSqlXml("SaveAsSelectMeMedicion1"); }
        }
    }
}
