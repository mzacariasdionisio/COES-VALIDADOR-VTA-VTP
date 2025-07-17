using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_SUBESTACION
    /// </summary>
    public class EprSubestacionHelper : HelperBase
    {
        public EprSubestacionHelper(): base(Consultas.EprSubEstacionSql)
        {
        }

        public void ObtenerMetaDatos(IDataReader dr, ref Dictionary<int, MetadataDTO> metadatos)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                metadatos.Add(i, new MetadataDTO
                {
                    FieldName = dr.GetName(i),
                    TipoDato = dr.GetFieldType(i)
                });
            }
        }
        public EprSubestacionDTO Create(IDataReader dr)
        {
            EprSubestacionDTO entity = new EprSubestacionDTO();

            int iEpsubecodi = dr.GetOrdinal(this.Epsubecodi);
            if (!dr.IsDBNull(iEpsubecodi)) entity.Epsubecodi = Convert.ToInt32(dr.GetValue(iEpsubecodi));

            int iAreacodi = dr.GetOrdinal(this.Areacodi);
            if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

            int iAreanomb = dr.GetOrdinal(this.Areanomb);
            if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = Convert.ToString(dr.GetValue(iAreanomb));

            int iEpproycodi = dr.GetOrdinal(this.Epproycodi);
            if (!dr.IsDBNull(iEpproycodi)) entity.Epproycodi = Convert.ToInt32(dr.GetValue(iEpproycodi));

            int iEpproynomb = dr.GetOrdinal(this.Epproynomb);
            if (!dr.IsDBNull(iEpproynomb)) entity.Epproynomb = Convert.ToString(dr.GetValue(iEpproynomb));

            int iEpsubemotivo = dr.GetOrdinal(this.Epsubemotivo);
            if (!dr.IsDBNull(iEpsubemotivo)) entity.Epsubemotivo = dr.GetValue(iEpsubemotivo).ToString();

            int iEpsubefecha= dr.GetOrdinal(this.Epsubefecha);
            if (!dr.IsDBNull(iEpsubefecha)) entity.Epsubefecha = dr.GetValue(iEpsubefecha).ToString();

            if (validaColumna(dr, this.Epsubefechadate))
            {
                int iEpsubefechadate = dr.GetOrdinal(this.Epsubefechadate);
                if (!dr.IsDBNull(iEpsubefechadate)) entity.Epsubefechadate = Convert.ToDateTime(dr.GetValue(iEpsubefechadate).ToString());
            }

            int iEpsubememoriacalculo = dr.GetOrdinal(this.Epsubememoriacalculo);
            if (!dr.IsDBNull(iEpsubememoriacalculo)) entity.Epsubememoriacalculo = dr.GetValue(iEpsubememoriacalculo).ToString();

            int iEpsubeestregistro = dr.GetOrdinal(this.Epsubeestregistro);
            if (!dr.IsDBNull(iEpsubeestregistro)) entity.Epsubeestregistro = Convert.ToString(dr.GetValue(iEpsubeestregistro));

            int iEpsubeusucreacion = dr.GetOrdinal(this.Epsubeusucreacion);
            if (!dr.IsDBNull(iEpsubeusucreacion)) entity.Epsubeusucreacion = dr.GetValue(iEpsubeusucreacion).ToString();

            int iEpsubefeccreacion = dr.GetOrdinal(this.Epsubefeccreacion);
            if (!dr.IsDBNull(iEpsubefeccreacion)) entity.Epsubefeccreacion = Convert.ToString(dr.GetValue(iEpsubefeccreacion));

            int iEpsubeusumodificacion = dr.GetOrdinal(this.Epsubeusumodificacion);
            if (!dr.IsDBNull(iEpsubeusumodificacion)) entity.Epsubeusumodificacion = dr.GetValue(iEpsubeusumodificacion).ToString();

            int iEpsubefecmodificacion = dr.GetOrdinal(this.Epsubefecmodificacion);
            if (!dr.IsDBNull(iEpsubefecmodificacion)) entity.Epsubefecmodificacion = Convert.ToString(dr.GetValue(iEpsubefecmodificacion));

            int iUltMemoriaCalculo = dr.GetOrdinal(this.UltMemoriaCalculo);
            if (!dr.IsDBNull(iUltMemoriaCalculo)) entity.UltMemoriaCalculo = Convert.ToString(dr.GetValue(iUltMemoriaCalculo));


            return entity;
        }

        //GESPROTEC - 20241029
        #region GESPROTECT
        bool validaColumna(IDataReader dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Mapeo de Campos
        public string Epsubecodi = "EPSUBECODI";
        public string Areacodi = "AREACODI";
        public string Epproycodi = "EPPROYCODI";
        public string Epproynomb = "EPPROYNOMB";
        public string Epsubemotivo = "EPSUBEMOTIVO";
        public string Epsubefecha = "EPSUBEFECHA";
        public string Epsubefechadate = "EPSUBEFECHADATE";
        public string Epsubememoriacalculo = "EPSUBEMEMORIACALCULO";
        public string Epsubeestregistro = "EPSUBEESTREGISTRO";
        public string Epsubeusucreacion = "EPSUBEUSUCREACION";
        public string Epsubefeccreacion = "EPSUBEFECCREACION";
        public string Epsubeusumodificacion = "EPSUBEUSUMODIFICACION";
        public string Epsubefecmodificacion = "EPSUBEFECMODIFICACION";
        public string UltMemoriaCalculo = "ULT_MEMORIA_CALCULO";
        public string Areanomb = "AREANOMB";
        #endregion

        #region Parametros
        public string Zonacodi = "ZONACODI";
        #endregion

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

    }

}

