using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla MP_RECURSO
    /// </summary>
    public class MpRecursoHelper : HelperBase
    {
        public MpRecursoHelper(): base(Consultas.MpRecursoSql)
        {
        }

        public MpRecursoDTO Create(IDataReader dr)
        {
            MpRecursoDTO entity = new MpRecursoDTO();

            int iMtopcodi = dr.GetOrdinal(this.Mtopcodi);
            if (!dr.IsDBNull(iMtopcodi)) entity.Mtopcodi = Convert.ToInt32(dr.GetValue(iMtopcodi));

            int iMrecurcodi = dr.GetOrdinal(this.Mrecurcodi);
            if (!dr.IsDBNull(iMrecurcodi)) entity.Mrecurcodi = Convert.ToInt32(dr.GetValue(iMrecurcodi));

            int iMcatcodi = dr.GetOrdinal(this.Mcatcodi);
            if (!dr.IsDBNull(iMcatcodi)) entity.Mcatcodi = Convert.ToInt32(dr.GetValue(iMcatcodi));

            int iMrecurnomb = dr.GetOrdinal(this.Mrecurnomb);
            if (!dr.IsDBNull(iMrecurnomb)) entity.Mrecurnomb = dr.GetString(iMrecurnomb);

            int iMrecurtablasicoes = dr.GetOrdinal(this.Mrecurtablasicoes);
            if (!dr.IsDBNull(iMrecurtablasicoes)) entity.Mrecurtablasicoes = dr.GetString(iMrecurtablasicoes);

            int iMrecurcodisicoes = dr.GetOrdinal(this.Mrecurcodisicoes);
            if (!dr.IsDBNull(iMrecurcodisicoes)) entity.Mrecurcodisicoes = Convert.ToInt32(dr.GetValue(iMrecurcodisicoes));

            int iMrecurlogico = dr.GetOrdinal(this.Mrecurlogico);
            if (!dr.IsDBNull(iMrecurlogico)) entity.Mrecurlogico = Convert.ToInt32(dr.GetValue(iMrecurlogico));

            int iMrecurestado = dr.GetOrdinal(this.Mrecurestado);
            if (!dr.IsDBNull(iMrecurestado)) entity.Mrecurestado = Convert.ToInt32(dr.GetValue(iMrecurestado));

            int iMrecurpadre = dr.GetOrdinal(this.Mrecurpadre);
            if (!dr.IsDBNull(iMrecurpadre)) entity.Mrecurpadre = Convert.ToInt32(dr.GetValue(iMrecurpadre));

            int iMrecurorigen = dr.GetOrdinal(this.Mrecurorigen);
            if (!dr.IsDBNull(iMrecurorigen)) entity.Mrecurorigen = Convert.ToInt32(dr.GetValue(iMrecurorigen));

            int iMrecurorigen2 = dr.GetOrdinal(this.Mrecurorigen2);
            if (!dr.IsDBNull(iMrecurorigen2)) entity.Mrecurorigen2 = Convert.ToInt32(dr.GetValue(iMrecurorigen2));

            int iMrecurorigen3 = dr.GetOrdinal(this.Mrecurorigen3);
            if (!dr.IsDBNull(iMrecurorigen3)) entity.Mrecurorigen3 = Convert.ToInt32(dr.GetValue(iMrecurorigen3));

            int iMrecurorden = dr.GetOrdinal(this.Mrecurorden);
            if (!dr.IsDBNull(iMrecurorden)) entity.Mrecurorden = Convert.ToInt32(dr.GetValue(iMrecurorden));            

            int iMrecurusumodificacion = dr.GetOrdinal(this.Mrecurusumodificacion);
            if (!dr.IsDBNull(iMrecurusumodificacion)) entity.Mrecurusumodificacion = dr.GetString(iMrecurusumodificacion);

            int iMrecurfecmodificacion = dr.GetOrdinal(this.Mrecurfecmodificacion);
            if (!dr.IsDBNull(iMrecurfecmodificacion)) entity.Mrecurfecmodificacion = dr.GetDateTime(iMrecurfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos

        public string Mtopcodi = "MTOPCODI";
        public string Mrecurcodi = "MRECURCODI";
        public string Mcatcodi = "MCATCODI";
        public string Mrecurnomb = "MRECURNOMB";
        public string Mrecurtablasicoes = "MRECURTABLASICOES";
        public string Mrecurcodisicoes = "MRECURCODISICOES";
        public string Mrecurlogico = "MRECURLOGICO";
        public string Mrecurestado = "MRECURESTADO";
        public string Mrecurpadre = "MRECURPADRE";
        public string Mrecurorigen = "MRECURORIGEN";
        public string Mrecurorigen2 = "MRECURORIGEN2";
        public string Mrecurorigen3 = "MRECURORIGEN3";
        public string Mrecurorden = "MRECURORDEN";        
        public string Mrecurusumodificacion = "MRECURUSUMODIFICACION";
        public string Mrecurfecmodificacion = "MRECURFECMODIFICACION";

        
        public string SqlListarRecursosByTopologia
        {
            get { return base.GetSqlXml("ListarRecursosByTopologia"); }
        }
        public string SqlUpdateOrden
        {
            get { return GetSqlXml("UpdateOrden"); }
        }
        #endregion
    }
}
