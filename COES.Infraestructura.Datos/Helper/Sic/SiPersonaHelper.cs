using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_PERSONA
    /// </summary>
    public class SiPersonaHelper : HelperBase
    {
        public SiPersonaHelper(): base(Consultas.SiPersonaSql)
        {
        }

        public SiPersonaDTO Create(IDataReader dr)
        {
            SiPersonaDTO entity = new SiPersonaDTO();

            int iPercodi = dr.GetOrdinal(this.Percodi);
            if (!dr.IsDBNull(iPercodi)) entity.Percodi = Convert.ToInt32(dr.GetValue(iPercodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iTipopercodi = dr.GetOrdinal(this.Tipopercodi);
            if (!dr.IsDBNull(iTipopercodi)) entity.Tipopercodi = Convert.ToInt32(dr.GetValue(iTipopercodi));

            int iPernomb = dr.GetOrdinal(this.Pernomb);
            if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

            int iPerapellido = dr.GetOrdinal(this.Perapellido);
            if (!dr.IsDBNull(iPerapellido)) entity.Perapellido = dr.GetString(iPerapellido);

            int iPertelefono = dr.GetOrdinal(this.Pertelefono);
            if (!dr.IsDBNull(iPertelefono)) entity.Pertelefono = dr.GetString(iPertelefono);

            int iPerfax = dr.GetOrdinal(this.Perfax);
            if (!dr.IsDBNull(iPerfax)) entity.Perfax = dr.GetString(iPerfax);

            int iPercargo = dr.GetOrdinal(this.Percargo);
            if (!dr.IsDBNull(iPercargo)) entity.Percargo = dr.GetString(iPercargo);

            int iPertitulo = dr.GetOrdinal(this.Pertitulo);
            if (!dr.IsDBNull(iPertitulo)) entity.Pertitulo = dr.GetString(iPertitulo);

            int iPeremail = dr.GetOrdinal(this.Peremail);
            if (!dr.IsDBNull(iPeremail)) entity.Peremail = dr.GetString(iPeremail);

            int iPercelular = dr.GetOrdinal(this.Percelular);
            if (!dr.IsDBNull(iPercelular)) entity.Percelular = dr.GetString(iPercelular);

            int iPerg1 = dr.GetOrdinal(this.Perg1);
            if (!dr.IsDBNull(iPerg1)) entity.Perg1 = dr.GetString(iPerg1);

            int iPerasunto = dr.GetOrdinal(this.Perasunto);
            if (!dr.IsDBNull(iPerasunto)) entity.Perasunto = dr.GetString(iPerasunto);

            int iPerg2 = dr.GetOrdinal(this.Perg2);
            if (!dr.IsDBNull(iPerg2)) entity.Perg2 = dr.GetString(iPerg2);

            int iPerg3 = dr.GetOrdinal(this.Perg3);
            if (!dr.IsDBNull(iPerg3)) entity.Perg3 = dr.GetString(iPerg3);

            int iPerg4 = dr.GetOrdinal(this.Perg4);
            if (!dr.IsDBNull(iPerg4)) entity.Perg4 = dr.GetString(iPerg4);

            int iPerg5 = dr.GetOrdinal(this.Perg5);
            if (!dr.IsDBNull(iPerg5)) entity.Perg5 = dr.GetString(iPerg5);

            int iPerg6 = dr.GetOrdinal(this.Perg6);
            if (!dr.IsDBNull(iPerg6)) entity.Perg6 = dr.GetString(iPerg6);

            int iPerg7 = dr.GetOrdinal(this.Perg7);
            if (!dr.IsDBNull(iPerg7)) entity.Perg7 = dr.GetString(iPerg7);

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iPerclientelibre = dr.GetOrdinal(this.Perclientelibre);
            if (!dr.IsDBNull(iPerclientelibre)) entity.Perclientelibre = dr.GetString(iPerclientelibre);

            int iPercomision = dr.GetOrdinal(this.Percomision);
            if (!dr.IsDBNull(iPercomision)) entity.Percomision = dr.GetString(iPercomision);

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iPerestado = dr.GetOrdinal(this.Perestado);
            if (!dr.IsDBNull(iPerestado)) entity.Perestado = dr.GetString(iPerestado);

            int iPerorden = dr.GetOrdinal(this.Perorden);
            if (!dr.IsDBNull(iPerorden)) entity.Perorden = Convert.ToInt32(dr.GetValue(iPerorden));

            int iPeradminrolturno = dr.GetOrdinal(this.Peradminrolturno);
            if (!dr.IsDBNull(iPeradminrolturno)) entity.Peradminrolturno = dr.GetString(iPeradminrolturno);

            int iPerg8 = dr.GetOrdinal(this.Perg8);
            if (!dr.IsDBNull(iPerg8)) entity.Perg8 = dr.GetString(iPerg8);

            int iPerg9 = dr.GetOrdinal(this.Perg9);
            if (!dr.IsDBNull(iPerg9)) entity.Perg9 = dr.GetString(iPerg9);

            int iLastuser = dr.GetOrdinal(this.Lastuser);
            if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

            int iLastdate = dr.GetOrdinal(this.Lastdate);
            if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

            return entity;
        }


        public SiPersonaDTO CreateLista(IDataReader dr)
        {
            SiPersonaDTO entity = new SiPersonaDTO();

            int iPernomb = dr.GetOrdinal(this.Pernomb);
            if (!dr.IsDBNull(iPernomb)) entity.Pernomb = dr.GetString(iPernomb);

            return entity;
        }

        public string CreateListaCoordinadores(IDataReader dr)
        {
            string entity = "";

            int iPernomb = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iPernomb)) entity = (dr.GetString(iPernomb));

            return entity;
        }

        #region Mapeo de Campos

        public string Percodi = "PERCODI";
        public string Emprcodi = "EMPRCODI";
        public string Tipopercodi = "TIPOPERCODI";
        public string Pernomb = "PERNOMB";
        public string Perdni = "PERDNI";
        public string Perapellido = "PERAPELLIDO";
        public string Pertelefono = "PERTELEFONO";
        public string Perfax = "PERFAX";
        public string Percargo = "PERCARGO";
        public string Pertitulo = "PERTITULO";
        public string Peremail = "PEREMAIL";
        public string Percelular = "PERCELULAR";
        public string Perg1 = "PERG1";
        public string Perasunto = "PERASUNTO";
        public string Perg2 = "PERG2";
        public string Perg3 = "PERG3";
        public string Perg4 = "PERG4";
        public string Perg5 = "PERG5";
        public string Perg6 = "PERG6";
        public string Perg7 = "PERG7";
        public string Usercode = "USERCODE";
        public string Perclientelibre = "PERCLIENTELIBRE";
        public string Percomision = "PERCOMISION";
        public string Areacodi = "AREACODI";
        public string Perestado = "PERESTADO";
        public string Perorden = "PERORDEN";
        public string Peradminrolturno = "PERADMINROLTURNO";
        public string Perg8 = "PERG8";
        public string Perg9 = "PERG9";
        public string Username = "USERNAME";
        public string Lastuser = "LASTUSER";
        public string Lastdate = "LASTDATE";

        #endregion

        public string SqlGetByCriteriaArea
        {
            get { return base.GetSqlXml("GetByCriteriaArea"); }
        }
        

        public string SqlListaProgramador
        {
            get { return base.GetSqlXml("KeyListaProgramador"); }
        }
        public string SqlListaCoordinadores
        {
            get { return base.GetSqlXml("ListaCoordinadores"); }
        }

        public string SqlListaEspecialistasSME
        {
            get { return base.GetSqlXml("ListaEspecialistasSME"); }
        }

        public string Cargo
        {
            get { return base.GetSqlXml("KeyCargo"); }
        }

        public string Area
        {
            get { return base.GetSqlXml("KeyArea"); }
        }


        public string Telefono
        {
            get { return base.GetSqlXml("KeyTelefono"); }
        }


        public string Mail
        {
            get { return base.GetSqlXml("KeyMail"); }
        }

        #region MigracionSGOCOES-GrupoB
        public string SqlListaPersonalRol
        {
            get { return base.GetSqlXml("ListaPersonalRol"); }
        }
        #endregion
    }
}
