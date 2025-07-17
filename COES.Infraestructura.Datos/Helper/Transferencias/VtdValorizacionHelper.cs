using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla VTD_VALORIZACION
    /// </summary>
    public class VtdValorizacionHelper : HelperBase
    {
        public VtdValorizacionHelper(): base(Consultas.VtdValorizacionSql)
        {
        }

        public VtdValorizacionDTO Create(IDataReader dr)
        {
            VtdValorizacionDTO entity = new VtdValorizacionDTO();

            int iValocodi = dr.GetOrdinal(this.Valocodi);
            if (!dr.IsDBNull(iValocodi)) entity.Valocodi = Convert.ToInt32(dr.GetValue(iValocodi));

            int iValofecha = dr.GetOrdinal(this.Valofecha);
            if (!dr.IsDBNull(iValofecha)) entity.Valofecha = dr.GetDateTime(iValofecha);

            int iValomr = dr.GetOrdinal(this.Valomr);
            if (!dr.IsDBNull(iValomr)) entity.Valomr = Convert.ToDecimal(dr.GetValue(iValomr));

            int iValopreciopotencia = dr.GetOrdinal(this.Valopreciopotencia);
            if (!dr.IsDBNull(iValopreciopotencia)) entity.Valopreciopotencia = Convert.ToDecimal(dr.GetValue(iValopreciopotencia));

            int iValodemandacoes = dr.GetOrdinal(this.Valodemandacoes);
            if (!dr.IsDBNull(iValodemandacoes)) entity.Valodemandacoes = Convert.ToDecimal(dr.GetValue(iValodemandacoes));

            int iValofactorreparto = dr.GetOrdinal(this.Valofactorreparto);
            if (!dr.IsDBNull(iValofactorreparto)) entity.Valofactorreparto = Convert.ToDecimal(dr.GetValue(iValofactorreparto));

            int iValoporcentajeperdida = dr.GetOrdinal(this.Valoporcentajeperdida);
            if (!dr.IsDBNull(iValoporcentajeperdida)) entity.Valoporcentajeperdida = Convert.ToDecimal(dr.GetValue(iValoporcentajeperdida));

            int iValofrectotal = dr.GetOrdinal(this.Valofrectotal);
            if (!dr.IsDBNull(iValofrectotal)) entity.Valofrectotal = Convert.ToDecimal(dr.GetValue(iValofrectotal));

            int iValootrosequipos = dr.GetOrdinal(this.Valootrosequipos);
            if (!dr.IsDBNull(iValootrosequipos)) entity.Valootrosequipos = Convert.ToDecimal(dr.GetValue(iValootrosequipos));

            int iValocostofuerabanda = dr.GetOrdinal(this.Valocostofuerabanda);
            if (!dr.IsDBNull(iValocostofuerabanda)) entity.Valocostofuerabanda = Convert.ToDecimal(dr.GetValue(iValocostofuerabanda));

            int iValoco = dr.GetOrdinal(this.Valoco);
            if (!dr.IsDBNull(iValoco)) entity.Valoco = Convert.ToDecimal(dr.GetValue(iValoco));

            int iValora = dr.GetOrdinal(this.Valora);
            if (!dr.IsDBNull(iValora)) entity.Valora = Convert.ToDecimal(dr.GetValue(iValora));

            int iValoofmax = dr.GetOrdinal(this.Valoofmax);
            if (!dr.IsDBNull(iValoofmax)) entity.Valoofmax = Convert.ToDecimal(dr.GetValue(iValoofmax));

            int iValocompcostosoper = dr.GetOrdinal(this.Valocompcostosoper);
            if (!dr.IsDBNull(iValocompcostosoper)) entity.Valocompcostosoper = Convert.ToDecimal(dr.GetValue(iValocompcostosoper));

            int iValocomptermrt = dr.GetOrdinal(this.Valocomptermrt);
            if (!dr.IsDBNull(iValocomptermrt)) entity.Valocomptermrt = Convert.ToDecimal(dr.GetValue(iValocomptermrt));

            int iValoestado = dr.GetOrdinal(this.Valoestado);
            if (!dr.IsDBNull(iValoestado)) entity.Valoestado = dr.GetChar(iValoestado);

            int iValosucreacion = dr.GetOrdinal(this.Valousucreacion);
            if (!dr.IsDBNull(iValosucreacion)) entity.Valousucreacion = dr.GetString(iValosucreacion);

            int iValofeccreacion = dr.GetOrdinal(this.Valofeccreacion);
            if (!dr.IsDBNull(iValofeccreacion)) entity.Valofeccreacion = dr.GetDateTime(iValofeccreacion);

            int iValousumodificacion = dr.GetOrdinal(this.Valousumodificacion);
            if (!dr.IsDBNull(iValousumodificacion)) entity.Valousumodificacion = dr.GetString(iValousumodificacion);

            int iValofecmodificacion = dr.GetOrdinal(this.Valofecmodificacion);
            if (!dr.IsDBNull(iValofecmodificacion)) entity.Valofecmodificacion = dr.GetDateTime(iValofecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Valocodi = "VALOCODI";
        public string Valofecha = "VALOFECHA";
        public string Valomr = "VALOMR";
        public string Valopreciopotencia = "VALOPRECIOPOTENCIA";
        public string Valodemandacoes = "VALODEMANDACOES";
        public string Valofactorreparto = "VALOFACTORREPARTO";
        public string Valoporcentajeperdida = "VALOPORCENTAJEPERDIDA";
        public string Valofrectotal = "VALOFRECTOTAL";
        public string Valootrosequipos = "VALOOTROSEQUIPOS";
        public string Valocostofuerabanda = "VALOCOSTOFUERABANDA";

        public string Valoco = "VALOCO";
        public string Valora = "VALORA";
        public string ValoraSub = "VALORASUB";
        public string ValoraBaj = "VALORABAJ";
        public string Valoofmax = "VALOOFMAX";
        public string ValoofmaxBaj = "VALOOFMAXBAJ";
        public string Valocompcostosoper = "VALOCOMPCOSTOSOPER";
        public string Valocomptermrt = "VALOCOMPTERMRT";

        public string Valoestado = "VALOESTADO";
        public string Valousucreacion = "VALOUSUCREACION";        
        public string Valofeccreacion = "VALOFECCREACION";
        public string Valousumodificacion = "VALOUSUMODIFICACION";
        public string Valofecmodificacion = "VALOFECMODIFICACION";

        #endregion

        public string Emprcodi = "EMPRCODI";
        public string Emprnomb = "EMPRNOMB";
        public string SqlObtenerEmpresas
        {
            get { return base.GetSqlXml("ObtenerEmpresas"); }
        }

        public string SqlUpdateEstado
        {
            get { return base.GetSqlXml("UpdateFecha"); }
        }

        public string SqlUpdateEstadoPorEmpresa
        {
            get { return base.GetSqlXml("UpdateFechaPorEmpresa"); }
        }
    }
}
