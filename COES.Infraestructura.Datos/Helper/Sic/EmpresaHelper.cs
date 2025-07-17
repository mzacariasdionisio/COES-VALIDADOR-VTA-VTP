using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESA
    /// </summary>
    public class EmpresaHelper : HelperBase
    {
        public EmpresaHelper(): base(Consultas.EventoSql) 
        {
        }

        public EmpresaDTO Create(IDataReader dr)
        {
            EmpresaDTO entity = new EmpresaDTO();

            //int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            //if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

            //int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            //if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            //int iCompcode = dr.GetOrdinal(this.Compcode);
            //if (!dr.IsDBNull(iCompcode)) entity.Compcode = dr.GetInt32(iCompcode);

            //int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            //if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = dr.GetInt32(iTipoemprcodi);

            //int iEmprdire = dr.GetOrdinal(this.Emprdire);
            //if (!dr.IsDBNull(iEmprdire)) entity.Emprdire = dr.GetString(iEmprdire);

            //int iEmprtele = dr.GetOrdinal(this.Emprtele);
            //if (!dr.IsDBNull(iEmprtele)) entity.Emprtele = dr.GetString(iEmprtele);

            //int iEmprnumedocu = dr.GetOrdinal(this.Emprnumedocu);
            //if (!dr.IsDBNull(iEmprnumedocu)) entity.Emprnumedocu = dr.GetString(iEmprnumedocu);

            //int iTipodocucodi = dr.GetOrdinal(this.Tipodocucodi);
            //if (!dr.IsDBNull(iTipodocucodi)) entity.Tipodocucodi = dr.GetString(iTipodocucodi);

            //int iEmprruc = dr.GetOrdinal(this.Emprruc);
            //if (!dr.IsDBNull(iEmprruc)) entity.Emprruc = dr.GetString(iEmprruc);

            //int iEmprabrev = dr.GetOrdinal(this.Emprabrev);
            //if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

            //int iEmprorden = dr.GetOrdinal(this.Emprorden);
            //if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = dr.GetInt32(iEmprorden);

            //int iEmprdom = dr.GetOrdinal(this.Emprdom);
            //if (!dr.IsDBNull(iEmprdom)) entity.Emprdom = dr.GetString(iEmprdom);

            //int iEmprsein = dr.GetOrdinal(this.Emprsein);
            //if (!dr.IsDBNull(iEmprsein)) entity.Emprsein = dr.GetString(iEmprsein);

            //int iEmprrazsocial = dr.GetOrdinal(this.Emprrazsocial);
            //if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);

            //int iEmprcoes = dr.GetOrdinal(this.Emprcoes);
            //if (!dr.IsDBNull(iEmprcoes)) entity.Emprcoes = dr.GetString(iEmprcoes);

            //int iLastuser = dr.GetOrdinal(this.Lastuser);
            //if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            //int iLastdate = dr.GetOrdinal(this.Lastdate);
            //if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            //int iInddemanda = dr.GetOrdinal(this.Inddemanda);
            //if (!dr.IsDBNull(iInddemanda)) entity.Inddemanda = dr.GetString(iInddemanda);            

            

            return entity;
        }


        #region Mapeo de Campos


       
        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string Compcode = "COMPCODE";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Emprdire = "EMPRDIRE";
        public string Emprtele = "EMPRTELE";
        public string Emprnumedocu = "EMPRNUMEDOCU";
        public string Tipodocucodi = "TIPODOCUCODI";
        public string Emprruc = "EMPRRUC";
        public string Emprabrev = "EMPRABREV";
        public string Emprorden = "EMPRORDEN";
        public string Emprdom = "EMPRDOM";
        public string Emprsein = "EMPRSEIN";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Emprcoes = "EMPRCOES";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";
        public string Inddemanda = "INDDEMANDA";

        #endregion

        public string SqlObtenerPorTipo
        {
            get { return base.GetSqlXml("ObtenerPorTipo"); }
        }
    }
}

