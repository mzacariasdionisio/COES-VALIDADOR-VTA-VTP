using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla trn_tramite
    /// </summary>
    public  class TramiteHelper: HelperBase
    {
      public TramiteHelper() : base(Consultas.TramiteSql)
      {
      }

        public TramiteDTO Create(IDataReader dr)
        {
            TramiteDTO entity = new TramiteDTO();

            int iTramcodi = dr.GetOrdinal(this.Tramcodi);
            if (!dr.IsDBNull(iTramcodi)) entity.TramCodi = dr.GetInt32(iTramcodi);

            int iUsuacoescodi = dr.GetOrdinal(this.Usuacoescodi);
            if (!dr.IsDBNull(iUsuacoescodi)) entity.UsuaCoesCodi = dr.GetString(iUsuacoescodi);

            int iUsuaseincodi = dr.GetOrdinal(this.Usuaseincodi);
            if (!dr.IsDBNull(iUsuaseincodi)) entity.UsuaSeinCodi = dr.GetString(iUsuaseincodi);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.EmprCodi = dr.GetInt32(iEmprcodi);

            int iPericodi = dr.GetOrdinal(this.Pericodi);
            if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = dr.GetInt32(iPericodi);

            int iTipotramcodi = dr.GetOrdinal(this.Tipotramcodi);
            if (!dr.IsDBNull(iTipotramcodi)) entity.TipoTramcodi = dr.GetInt32(iTipotramcodi);

            int iTramcorrinf = dr.GetOrdinal(this.Tramcorrinf);
            if (!dr.IsDBNull(iTramcorrinf)) entity.TramCorrInf = dr.GetString(iTramcorrinf);

            int iTramdescripcion = dr.GetOrdinal(this.Tramdescripcion);
            if (!dr.IsDBNull(iTramdescripcion)) entity.TramDescripcion = dr.GetString(iTramdescripcion);

            int iTramrespuesta = dr.GetOrdinal(this.Tramrespuesta);
            if (!dr.IsDBNull(iTramrespuesta)) entity.TramRespuesta = dr.GetString(iTramrespuesta);

            int iTramfecreg = dr.GetOrdinal(this.Tramfecreg);
            if (!dr.IsDBNull(iTramfecreg)) entity.TramFecReg = dr.GetDateTime(iTramfecreg);

            int iTramfecres = dr.GetOrdinal(this.Tramfecres);
            if (!dr.IsDBNull(iTramfecres)) entity.TramFecRes = dr.GetDateTime(iTramfecres);

            int iTramfecins = dr.GetOrdinal(this.Tramfecins);
            if (!dr.IsDBNull(iTramfecins)) entity.TramFecIns = dr.GetDateTime(iTramfecins);

            int iTramfecact = dr.GetOrdinal(this.Tramfecact);
            if (!dr.IsDBNull(iTramfecact)) entity.TramFecAct = dr.GetDateTime(iTramfecact);

            int iTramversion = dr.GetOrdinal(this.Tramversion);
            if (!dr.IsDBNull(iTramversion)) entity.TramVersion = dr.GetInt32(iTramversion);
          
            int iTipotramnombre = dr.GetOrdinal(this.Tipotramnombre);
            if (!dr.IsDBNull(iTipotramnombre)) entity.TipoTramNombre = dr.GetString(iTipotramnombre);


            int iEmprnombre = dr.GetOrdinal(this.Emprnombre);
            if (!dr.IsDBNull(iEmprnombre)) entity.EmprNombre = dr.GetString(iEmprnombre);


            return entity;
        }

        #region Mapeo de Campos

        public string Tramcodi = "TRMCODI";
        public string Usuacoescodi = "COESUSERNAME";
        public string Usuaseincodi = "SEINUSERNAME";
        public string Emprcodi = "EMPRCODI";
        public string Pericodi = "PERICODI";
        public string Tipotramcodi = "TIPTRMCODI";
        public string Tramcorrinf = "TRMCORRIGEINFORME";
        public string Tramdescripcion = "TRMDESCRIPCION";
        public string Tramrespuesta = "TRMRESPUESTA";
        public string Tramfecreg = "TRMFECREGISTRO";
        public string Tramfecres = "TRMFECRESPUESTA";
        public string Tramfecins = "TRMFECINS";
        public string Tramfecact = "TRMFECACT";
        public string Tramversion = "TRMVERSION";
        public string Tipotramnombre = "TIPTRMNOMBRE";
        public string Emprnombre = "NOMBEMPRESA"; 

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlDeleteListaTramite
        {
            get { return base.GetSqlXml("DeleteListaTramite"); }
        }
    }
}
