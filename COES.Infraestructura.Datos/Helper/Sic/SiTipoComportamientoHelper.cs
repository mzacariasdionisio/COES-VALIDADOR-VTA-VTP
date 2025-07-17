using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_TIPO_COMPORTAMIENTO
    /// </summary>
    public class SiTipoComportamientoHelper : HelperBase
    {
        public SiTipoComportamientoHelper(): base(Consultas.SiTipoComportamientoSql)
        {
        }

        public SiTipoComportamientoDTO Create(IDataReader dr)
        {
            SiTipoComportamientoDTO entity = new SiTipoComportamientoDTO();

            int iTipocodi = dr.GetOrdinal(this.Tipocodi);
            if (!dr.IsDBNull(iTipocodi)) entity.Tipocodi = Convert.ToInt32(dr.GetValue(iTipocodi));

            int iTipoprincipal = dr.GetOrdinal(this.Tipoprincipal);
            if (!dr.IsDBNull(iTipoprincipal)) entity.Tipoprincipal = dr.GetString(iTipoprincipal);

            int iTipotipagente = dr.GetOrdinal(this.Tipotipagente);
            if (!dr.IsDBNull(iTipotipagente)) entity.Tipotipagente = dr.GetString(iTipotipagente);

            int iTipodocsustentatorio = dr.GetOrdinal(this.Tipodocsustentatorio);
            if (!dr.IsDBNull(iTipodocsustentatorio)) entity.Tipodocsustentatorio = dr.GetString(iTipodocsustentatorio);

            int iTipoarcdigitalizado = dr.GetOrdinal(this.Tipoarcdigitalizado);
            if (!dr.IsDBNull(iTipoarcdigitalizado)) entity.Tipoarcdigitalizado = dr.GetString(iTipoarcdigitalizado);

            int iTipopotenciainstalada = dr.GetOrdinal(this.Tipopotenciainstalada);
            if (!dr.IsDBNull(iTipopotenciainstalada)) entity.Tipopotenciainstalada = dr.GetString(iTipopotenciainstalada);

            int iTiponrocentrales = dr.GetOrdinal(this.Tiponrocentrales);
            if (!dr.IsDBNull(iTiponrocentrales)) entity.Tiponrocentrales = dr.GetString(iTiponrocentrales);

            int iTipolineatrans500 = dr.GetOrdinal(this.Tipolineatrans500);
            if (!dr.IsDBNull(iTipolineatrans500)) entity.Tipolineatrans500 = dr.GetString(iTipolineatrans500);

            int iTipolineatrans220 = dr.GetOrdinal(this.Tipolineatrans220);
            if (!dr.IsDBNull(iTipolineatrans220)) entity.Tipolineatrans220 = dr.GetString(iTipolineatrans220);

            int iTipolineatrans138 = dr.GetOrdinal(this.Tipolineatrans138);
            if (!dr.IsDBNull(iTipolineatrans138)) entity.Tipolineatrans138 = dr.GetString(iTipolineatrans138);

            int iTipolineatrans500km = dr.GetOrdinal(this.Tipolineatrans500km);
            if (!dr.IsDBNull(iTipolineatrans500km)) entity.Tipolineatrans500km = dr.GetString(iTipolineatrans500km);

            int iTipolineatrans220km = dr.GetOrdinal(this.Tipolineatrans220km);
            if (!dr.IsDBNull(iTipolineatrans220km)) entity.Tipolineatrans220km = dr.GetString(iTipolineatrans220km);

            int iTipolineatrans138km = dr.GetOrdinal(this.Tipolineatrans138km);
            if (!dr.IsDBNull(iTipolineatrans138km)) entity.Tipolineatrans138km = dr.GetString(iTipolineatrans138km);

            int iTipototallineastransmision = dr.GetOrdinal(this.Tipototallineastransmision);
            if (!dr.IsDBNull(iTipototallineastransmision)) entity.Tipototallineastransmision = dr.GetString(iTipototallineastransmision);

            int iTipomaxdemandacoincidente = dr.GetOrdinal(this.Tipomaxdemandacoincidente);
            if (!dr.IsDBNull(iTipomaxdemandacoincidente)) entity.Tipomaxdemandacoincidente = dr.GetString(iTipomaxdemandacoincidente);

            int iTipomaxdemandacontratada = dr.GetOrdinal(this.Tipomaxdemandacontratada);
            if (!dr.IsDBNull(iTipomaxdemandacontratada)) entity.Tipomaxdemandacontratada = dr.GetString(iTipomaxdemandacontratada);

            int iTiponumsuministrador = dr.GetOrdinal(this.Tiponumsuministrador);
            if (!dr.IsDBNull(iTiponumsuministrador)) entity.Tiponumsuministrador = dr.GetString(iTiponumsuministrador);

            int iTipousucreacion = dr.GetOrdinal(this.Tipousucreacion);
            if (!dr.IsDBNull(iTipousucreacion)) entity.Tipousucreacion = dr.GetString(iTipousucreacion);

            int iTipofeccreacion = dr.GetOrdinal(this.Tipofeccreacion);
            if (!dr.IsDBNull(iTipofeccreacion)) entity.Tipofeccreacion = dr.GetDateTime(iTipofeccreacion);

            int iTipousumodificacion = dr.GetOrdinal(this.Tipousumodificacion);
            if (!dr.IsDBNull(iTipousumodificacion)) entity.Tipousumodificacion = dr.GetString(iTipousumodificacion);

            int iTipofecmodificacion = dr.GetOrdinal(this.Tipofecmodificacion);
            if (!dr.IsDBNull(iTipofecmodificacion)) entity.Tipofecmodificacion = dr.GetDateTime(iTipofecmodificacion);

            int iTipoemprcodi = dr.GetOrdinal(this.Tipoemprcodi);
            if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(dr.GetValue(iTipoemprcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            // doc

            int iTipodocname1 = dr.GetOrdinal(this.Tipodocname1);
            if (!dr.IsDBNull(iTipodocname1)) entity.Tipodocname1 = dr.GetString(iTipodocname1);

            int iTipodocadjfilename1 = dr.GetOrdinal(this.Tipodocadjfilename1);
            if (!dr.IsDBNull(iTipodocadjfilename1)) entity.Tipodocadjfilename1 = dr.GetString(iTipodocadjfilename1);

            int iTipodocname2 = dr.GetOrdinal(this.Tipodocname2);
            if (!dr.IsDBNull(iTipodocname2)) entity.Tipodocname2 = dr.GetString(iTipodocname2);

            int iTipodocadjfilename2 = dr.GetOrdinal(this.Tipodocadjfilename2);
            if (!dr.IsDBNull(iTipodocadjfilename2)) entity.Tipodocadjfilename2 = dr.GetString(iTipodocadjfilename2);

            int iTipodocname3 = dr.GetOrdinal(this.Tipodocname3);
            if (!dr.IsDBNull(iTipodocname3)) entity.Tipodocname3 = dr.GetString(iTipodocname3);

            int iTipodocadjfilename3 = dr.GetOrdinal(this.Tipodocadjfilename3);
            if (!dr.IsDBNull(iTipodocadjfilename3)) entity.Tipodocadjfilename3 = dr.GetString(iTipodocadjfilename3);

            int iTipodocname4 = dr.GetOrdinal(this.Tipodocname4);
            if (!dr.IsDBNull(iTipodocname4)) entity.Tipodocname4 = dr.GetString(iTipodocname4);

            int iTipodocadjfilename4 = dr.GetOrdinal(this.Tipodocadjfilename4);
            if (!dr.IsDBNull(iTipodocadjfilename4)) entity.Tipodocadjfilename4 = dr.GetString(iTipodocadjfilename4);

            int iTipodocname5 = dr.GetOrdinal(this.Tipodocname5);
            if (!dr.IsDBNull(iTipodocname5)) entity.Tipodocname5 = dr.GetString(iTipodocname5);

            int iTipodocadjfilename5 = dr.GetOrdinal(this.Tipodocadjfilename5);
            if (!dr.IsDBNull(iTipodocadjfilename5)) entity.Tipodocadjfilename5 = dr.GetString(iTipodocadjfilename5);

            int iTipobaja = dr.GetOrdinal(this.TipoBaja);
            if (!dr.IsDBNull(iTipobaja)) entity.Tipobaja = dr.GetString(iTipobaja);

            int iTipoinicial = dr.GetOrdinal(this.TipoInicial);
            if (!dr.IsDBNull(iTipoinicial)) entity.Tipoinicial = dr.GetString(iTipoinicial);

            int iTipoarcdigitalizadofilename = dr.GetOrdinal(this.Tipoarcdigitalizadofilename);
            if (!dr.IsDBNull(iTipoarcdigitalizadofilename)) entity.Tipoarcdigitalizadofilename = dr.GetString(iTipoarcdigitalizadofilename);

            int iTipocomentario = dr.GetOrdinal(this.Tipocomentario);
            if (!dr.IsDBNull(iTipocomentario)) entity.Tipocomentario = dr.GetString(iTipocomentario);


            return entity;
        }

        //
        #region Consultas SQL
        public string SqlListByEmprcodi
        {
            get { return base.GetSqlXml("ListByEmprcodi"); }
        }
        #endregion

        #region Mapeo de Campos

        public string Tipocodi = "TIPOCODI";
        public string Tipoprincipal = "TIPOPRINCIPAL";
        public string Tipotipagente = "TIPOTIPAGENTE";
        public string Tipodocsustentatorio = "TIPODOCSUSTENTATORIO";
        public string Tipoarcdigitalizado = "TIPOARCDIGITALIZADO";
        public string Tipoarcdigitalizadofilename = "TIPOARCDIGITALIZADOFILENAME";
        public string Tipopotenciainstalada = "TIPOPOTENCIAINSTALADA";
        public string Tiponrocentrales = "TIPONROCENTRALES";
        public string Tipolineatrans500 = "TIPOLINEATRANS_500";
        public string Tipolineatrans220 = "TIPOLINEATRANS_220";
        public string Tipolineatrans138 = "TIPOLINEATRANS_138";
        public string Tipolineatrans500km = "TIPOLINEATRANS_500KM";
        public string Tipolineatrans220km = "TIPOLINEATRANS_220KM";
        public string Tipolineatrans138km = "TIPOLINEATRANS_138KM";
        public string Tipototallineastransmision = "TIPOTOTALLINEASTRANSMISION";
        public string Tipomaxdemandacoincidente = "TIPOMAXDEMANDACOINCIDENTE";
        public string Tipomaxdemandacontratada = "TIPOMAXDEMANDACONTRATADA";
        public string Tiponumsuministrador = "TIPONUMSUMINISTRADOR";
        public string Tipousucreacion = "TIPOUSUCREACION";
        public string Tipofeccreacion = "TIPOFECCREACION";
        public string Tipousumodificacion = "TIPOUSUMODIFICACION";
        public string Tipofecmodificacion = "TIPOFECMODIFICACION";
        public string Tipoemprcodi = "TIPOEMPRCODI";
        public string Emprcodi = "EMPRCODI";
        public string TipoBaja = "TIPOBAJA";
        public string TipoInicial = "TIPOINICIAL";
        public string Tipodocname1 = "tipodocNAME1";
        public string Tipodocadjfilename1 = "tipodocADJFILENAME1";
        public string Tipodocname2 = "tipodocNAME2";
        public string Tipodocadjfilename2 = "tipodocADJFILENAME2";
        public string Tipodocname3 = "tipodocNAME3";
        public string Tipodocadjfilename3 = "tipodocADJFILENAME3";
        public string Tipodocname4 = "tipodocNAME4";
        public string Tipodocadjfilename4 = "tipodocADJFILENAME4";
        public string Tipodocname5 = "tipodocNAME5";
        public string Tipodocadjfilename5 = "tipodocADJFILENAME5";
        public string Tipocomentario = "TIPOCOMENTARIO";
        #endregion

        public string SqlUpdateTipo
        {
            get { return base.GetSqlXml("UpdateTipo"); }
        }
    }

    
}
