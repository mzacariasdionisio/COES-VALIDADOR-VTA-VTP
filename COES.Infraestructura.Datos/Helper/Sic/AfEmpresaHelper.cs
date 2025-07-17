using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla AF_EMPRESA
    /// </summary>
    public class AfEmpresaHelper : HelperBase
    {
        public AfEmpresaHelper() : base(Consultas.AfEmpresaSql)
        {
        }

        public AfEmpresaDTO Create(IDataReader dr)
        {
            AfEmpresaDTO entity = new AfEmpresaDTO();

            int iAfemprestado = dr.GetOrdinal(this.Afemprestado);
            if (!dr.IsDBNull(iAfemprestado)) entity.Afemprestado = Convert.ToInt32(dr.GetValue(iAfemprestado));

            int iAfemprosinergmin = dr.GetOrdinal(this.Afemprosinergmin);
            if (!dr.IsDBNull(iAfemprosinergmin)) entity.Afemprosinergmin = dr.GetString(iAfemprosinergmin);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iAfemprusumodificacion = dr.GetOrdinal(this.Afemprusumodificacion);
            if (!dr.IsDBNull(iAfemprusumodificacion)) entity.Afemprusumodificacion = dr.GetString(iAfemprusumodificacion);

            int iAfemprusucreacion = dr.GetOrdinal(this.Afemprusucreacion);
            if (!dr.IsDBNull(iAfemprusucreacion)) entity.Afemprusucreacion = dr.GetString(iAfemprusucreacion);

            int iAfemprfecmodificacion = dr.GetOrdinal(this.Afemprfecmodificacion);
            if (!dr.IsDBNull(iAfemprfecmodificacion)) entity.Afemprfecmodificacion = dr.GetDateTime(iAfemprfecmodificacion);

            int iAfemprfeccreacion = dr.GetOrdinal(this.Afemprfeccreacion);
            if (!dr.IsDBNull(iAfemprfeccreacion)) entity.Afemprfeccreacion = dr.GetDateTime(iAfemprfeccreacion);

            int iAfemprnomb = dr.GetOrdinal(this.Afemprnomb);
            if (!dr.IsDBNull(iAfemprnomb)) entity.Afemprnomb = dr.GetString(iAfemprnomb);

            int iAfemprcodi = dr.GetOrdinal(this.Afemprcodi);
            if (!dr.IsDBNull(iAfemprcodi)) entity.Afemprcodi = Convert.ToInt32(dr.GetValue(iAfemprcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Afemprestado = "AFEMPRESTADO";
        public string Afemprosinergmin = "AFEMPROSINERGMIN";
        public string Emprcodi = "EMPRCODI";
        public string Afemprusumodificacion = "AFEMPRUSUMODIFICACION";
        public string Afemprusucreacion = "AFEMPRUSUCREACION";
        public string Afemprfecmodificacion = "AFEMPRFECMODIFICACION";
        public string Afemprfeccreacion = "AFEMPRFECCREACION";
        public string Afemprnomb = "AFEMPRNOMB";
        public string Afemprcodi = "AFEMPRCODI";
        public string Afalerta = "AFALERTA";

        #endregion
    }
}
