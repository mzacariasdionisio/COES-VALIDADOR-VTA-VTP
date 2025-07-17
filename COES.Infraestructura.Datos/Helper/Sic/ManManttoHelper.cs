using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MAN_MANTTO
    /// </summary>
    public class ManManttoHelper : HelperBase
    {
        public ManManttoHelper()
            : base(Consultas.ManManttoSql)
        {
        }

        public ManManttoDTO Create(IDataReader dr)
        {
            ManManttoDTO entity = new ManManttoDTO();

            int iMancodi = dr.GetOrdinal(this.Mancodi);
            if (!dr.IsDBNull(iMancodi)) entity.Mancodi = Convert.ToInt32(dr.GetValue(iMancodi));

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iTipoevencodi = dr.GetOrdinal(this.Tipoevencodi);
            if (!dr.IsDBNull(iTipoevencodi)) entity.Tipoevencodi = Convert.ToInt32(dr.GetValue(iTipoevencodi));

            int iEmprcodireporta = dr.GetOrdinal(this.Emprcodireporta);
            if (!dr.IsDBNull(iEmprcodireporta)) entity.Emprcodireporta = Convert.ToInt32(dr.GetValue(iEmprcodireporta));

            int iEvenini = dr.GetOrdinal(this.Evenini);
            if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

            int iEvenpreini = dr.GetOrdinal(this.Evenpreini);
            if (!dr.IsDBNull(iEvenpreini)) entity.Evenpreini = dr.GetDateTime(iEvenpreini);

            int iEvenfin = dr.GetOrdinal(this.Evenfin);
            if (!dr.IsDBNull(iEvenfin)) entity.Evenfin = dr.GetDateTime(iEvenfin);

            int iEvenprefin = dr.GetOrdinal(this.Evenprefin);
            if (!dr.IsDBNull(iEvenprefin)) entity.Evenprefin = dr.GetDateTime(iEvenprefin);

            int iSubcausacodi = dr.GetOrdinal(this.Subcausacodi);
            if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

            int iEvenmwindisp = dr.GetOrdinal(this.Evenmwindisp);
            if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = dr.GetDecimal(iEvenmwindisp);

            int iEvenpadre = dr.GetOrdinal(this.Evenpadre);
            if (!dr.IsDBNull(iEvenpadre)) entity.Evenpadre = Convert.ToInt32(dr.GetValue(iEvenpadre));

            int iEvenindispo = dr.GetOrdinal(this.Evenindispo);
            if (!dr.IsDBNull(iEvenindispo)) entity.Evenindispo = dr.GetString(iEvenindispo);

            int iEveninterrup = dr.GetOrdinal(this.Eveninterrup);
            if (!dr.IsDBNull(iEveninterrup)) entity.Eveninterrup = dr.GetString(iEveninterrup);

            int iEventipoprog = dr.GetOrdinal(this.Eventipoprog);
            if (!dr.IsDBNull(iEventipoprog)) entity.Eventipoprog = dr.GetString(iEventipoprog);

            int iEvendescrip = dr.GetOrdinal(this.Evendescrip);
            if (!dr.IsDBNull(iEvendescrip)) entity.Evendescrip = dr.GetString(iEvendescrip);

            int iEvenobsrv = dr.GetOrdinal(this.Evenobsrv);
            if (!dr.IsDBNull(iEvenobsrv)) entity.Evenobsrv = dr.GetString(iEvenobsrv);

            int iEvenestado = dr.GetOrdinal(this.Evenestado);
            if (!dr.IsDBNull(iEvenestado)) entity.Evenestado = dr.GetString(iEvenestado);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            int iEvenprocesado = dr.GetOrdinal(this.Evenprocesado);
            if (!dr.IsDBNull(iEvenprocesado)) entity.Evenprocesado = Convert.ToInt32(dr.GetValue(iEvenprocesado));

            int iDeleted = dr.GetOrdinal(this.Deleted);
            if (!dr.IsDBNull(iDeleted)) entity.Deleted = Convert.ToInt32(dr.GetValue(iDeleted));

            int iRegcodi = dr.GetOrdinal(this.Regcodi);
            if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));

            int iManttocodi = dr.GetOrdinal(this.Manttocodi);
            if (!dr.IsDBNull(iManttocodi)) entity.Manttocodi = Convert.ToInt32(dr.GetValue(iManttocodi));

            int iIsfiles = dr.GetOrdinal(this.Isfiles);
            if (!dr.IsDBNull(iIsfiles)) entity.Isfiles = dr.GetString(iIsfiles);

            int iCreated = dr.GetOrdinal(this.Created);
            if (!dr.IsDBNull(iCreated)) entity.Created = dr.GetDateTime(iCreated);

            return entity;
        }


        #region Mapeo de Campos

        public string Mancodi = "MANCODI";
        public string Equicodi = "EQUICODI";
        public string Tipoevencodi = "TIPOEVENCODI";
        public string Emprcodireporta = "EMPRCODIREPORTA";
        public string Evenini = "EVENINI";
        public string Evenpreini = "EVENPREINI";
        public string Evenfin = "EVENFIN";
        public string Evenprefin = "EVENPREFIN";
        public string Subcausacodi = "SUBCAUSACODI";
        public string Evenmwindisp = "EVENMWINDISP";
        public string Evenpadre = "EVENPADRE";
        public string Evenindispo = "EVENINDISPO";
        public string Eveninterrup = "EVENINTERRUP";
        public string Eventipoprog = "EVENTIPOPROG";
        public string Evendescrip = "EVENDESCRIP";
        public string Evenobsrv = "EVENOBSRV";
        public string Evenestado = "EVENESTADO";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Evenprocesado = "EVENPROCESADO";
        public string Deleted = "DELETED";
        public string Regcodi = "REGCODI";
        public string Manttocodi = "MANTTOCODI";
        public string Isfiles = "ISFILES";
        public string Created = "CREATED";

        #endregion

        public string SqlMantenimietosPorEquipoFecha
        {
            get { return base.GetSqlXml("MantenimietosPorEquipoFecha"); }
        }
        
    }
}
