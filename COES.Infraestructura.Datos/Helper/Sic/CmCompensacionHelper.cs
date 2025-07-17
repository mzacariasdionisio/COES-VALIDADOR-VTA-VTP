using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using System;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla CM_COMPENSACION
    /// </summary>
    public class CmCompensacionHelper : HelperBase
    {
        public CmCompensacionHelper() : base(Consultas.CmCompensacionSql)
        {
        }

        public CmCompensacionDTO Create(IDataReader dr)
        {
            CmCompensacionDTO entity = new CmCompensacionDTO();

            int iCompcodi = dr.GetOrdinal(this.Compcodi);
            if (!dr.IsDBNull(iCompcodi)) entity.Compcodi = Convert.ToInt32(dr.GetValue(iCompcodi));         

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iSubcausaevencodi = dr.GetOrdinal(this.Subcausaevencodi);
            if (!dr.IsDBNull(iSubcausaevencodi)) entity.Subcausaevencodi = Convert.ToInt32(dr.GetValue(iSubcausaevencodi));

            int iCompfecha = dr.GetOrdinal(this.Compfecha);
            if (!dr.IsDBNull(iCompfecha)) entity.Compfecha = dr.GetDateTime(iCompfecha);

            int iCompintervalo = dr.GetOrdinal(this.Compintervalo);
            if (!dr.IsDBNull(iCompintervalo)) entity.Compintervalo = Convert.ToInt32(dr.GetValue(iCompintervalo));

            int iCompvalor = dr.GetOrdinal(this.Compvalor);
            if (!dr.IsDBNull(iCompvalor)) entity.Compvalor = dr.GetDecimal(iCompvalor);

            int iCompsucreacion = dr.GetOrdinal(this.Compsucreacion);
            if (!dr.IsDBNull(iCompsucreacion)) entity.Compsucreacion = dr.GetString(iCompsucreacion);

            int iCompfeccreacion = dr.GetOrdinal(this.Compfeccreacion);
            if (!dr.IsDBNull(iCompfeccreacion)) entity.Compfeccreacion = dr.GetDateTime(iCompfeccreacion);

            int iCompusumodificacion = dr.GetOrdinal(this.Compusumodificacion);
            if (!dr.IsDBNull(iCompusumodificacion)) entity.Compusumodificacion = dr.GetString(iCompusumodificacion);

            int iCompfecmodificacion = dr.GetOrdinal(this.Compfecmodificacion);
            if (!dr.IsDBNull(iCompfecmodificacion)) entity.Compfecmodificacion = dr.GetDateTime(iCompfecmodificacion);

            return entity;
        }


        #region Mapeo de Campos      

        public string Compcodi = "COMPCODI";
        public string Equicodi = "EQUICODI";
        public string Subcausaevencodi = "SUBCAUSAEVENCODI";
        public string Compfecha = "COMPFECHA";
        public string Compintervalo = "COMPINTERVALO";
        public string Compvalor = "COMPVALOR";
        public string Compsucreacion = "COMPSUCREACION";
        public string Compfeccreacion = "COMPFECCREACION";
        public string Compusumodificacion = "COMPUSUMODIFICACION";
        public string Compfecmodificacion = "COMPFECMODIFICACION";

        public string Emprcodi = "EMPRCODI";
        #endregion

        public string SqlDeleteByCriteria
        {
            get { return GetSqlXml("DeleteByCriteria"); }
        }


        public string SqlGetCompensacionporCalificacion
        {
            get { return GetSqlXml("GetCompensacionporCalificacion"); }
        }


        public string SqlGetCompensacionporCalificacionParticipante
        {
            get { return GetSqlXml("GetCompensacionporCalificacionParticipante"); }
        }
        

            

    }
}
