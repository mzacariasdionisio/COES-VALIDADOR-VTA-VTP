using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_AREA
    /// </summary>
    public class EprPropCatalogoDataHelper : HelperBase
    {
        public EprPropCatalogoDataHelper(): base(Consultas.EprPropCatalogoDataSql)
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
        public EprPropCatalogoDataDTO Create(IDataReader dr)
        {
            EprPropCatalogoDataDTO entity = new EprPropCatalogoDataDTO();

            int iEqcatdcodi = dr.GetOrdinal(this.Eqcatdcodi);
            if (!dr.IsDBNull(iEqcatdcodi)) entity.Eqcatdcodi = Convert.ToInt32(dr.GetValue(iEqcatdcodi));

            int iEqcatccodi = dr.GetOrdinal(this.Eqcatccodi);
            if (!dr.IsDBNull(iEqcatccodi)) entity.Eqcatccodi = Convert.ToInt32(dr.GetValue(iEqcatccodi));

            int iEqcatdabrev = dr.GetOrdinal(this.Eqcatdabrev);
            if (!dr.IsDBNull(iEqcatdabrev)) entity.Eqcatdabrev = Convert.ToString(dr.GetValue(iEqcatdabrev));

            int iEqcatddescripcion = dr.GetOrdinal(this.Eqcatddescripcion);
            if (!dr.IsDBNull(iEqcatddescripcion)) entity.Eqcatddescripcion = dr.GetValue(iEqcatddescripcion).ToString();

            int iEqcatdorden = dr.GetOrdinal(this.Eqcatdorden);
            if (!dr.IsDBNull(iEqcatdorden)) entity.Eqcatdorden = Convert.ToInt32(dr.GetValue(iEqcatdorden));

            int iEqcatdvalor = dr.GetOrdinal(this.Eqcatdvalor);
            if (!dr.IsDBNull(iEqcatdvalor)) entity.Eqcatdvalor = Convert.ToDouble(dr.GetValue(iEqcatdvalor));

            int iEqcatdnota = dr.GetOrdinal(this.Eqcatdnota);
            if (!dr.IsDBNull(iEqcatdnota)) entity.Eqcatdnota = Convert.ToString(dr.GetValue(iEqcatdnota));

            int iEqcatdestregistro = dr.GetOrdinal(this.Eqcatdestregistro);
            if (!dr.IsDBNull(iEqcatdestregistro)) entity.Eqcatdestregistro = dr.GetValue(iEqcatdestregistro).ToString();

            int iEqcatdusucreacion = dr.GetOrdinal(this.Eqcatdusucreacion);
            if (!dr.IsDBNull(iEqcatdusucreacion)) entity.Eqcatdusucreacion = dr.GetValue(iEqcatdusucreacion).ToString();

            int iEqcatdfeccreacion = dr.GetOrdinal(this.Eqcatdfeccreacion);
            if (!dr.IsDBNull(iEqcatdfeccreacion)) entity.Eqcatdfeccreacion = Convert.ToString(dr.GetValue(iEqcatdfeccreacion));

            int iEqcatdusumodificacion = dr.GetOrdinal(this.Eqcatdusumodificacion);
            if (!dr.IsDBNull(iEqcatdusumodificacion)) entity.Eqcatdusumodificacion = dr.GetValue(iEqcatdusumodificacion).ToString();

            int iEqcatdfecmodificacion = dr.GetOrdinal(this.Eqcatdfecmodificacion);
            if (!dr.IsDBNull(iEqcatdfecmodificacion)) entity.Eqcatdfecmodificacion = Convert.ToString(dr.GetValue(iEqcatdfecmodificacion));

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
        public string Eqcatdcodi = "EQCATDCODI";
        public string Eqcatccodi = "EQCATCCODI";
        public string Eqcatdabrev = "EQCATDABREV";
        public string Eqcatddescripcion = "EQCATDDESCRIPCION";
        public string Eqcatdorden = "EQCATDORDEN";
        public string Eqcatdvalor = "EQCATDVALOR";
        public string Eqcatdnota = "EQCATDNOTA";

        public string Eqcatdestregistro = "EQCATDESTREGISTRO";
        public string Eqcatdusucreacion = "EQCATDUSUCREACION";
        public string Eqcatdfeccreacion = "EQCATDFECCREACION";
        public string Eqcatdusumodificacion = "EQCATDUSUMODIFICACION";
        public string Eqcatdfecmodificacion = "EQCATDFECMODIFICACION";
        #endregion


        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        public string List
        {
            get { return GetSqlXml("List"); }
        }

        public string SqlListMaestroMarcaProteccion
        {
            get { return GetSqlXml("ListMaestroMarcaProteccion"); }
        }
    }
}
