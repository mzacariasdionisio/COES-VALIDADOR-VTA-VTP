using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;
using COES.Framework.Base.Core;
using COES.Framework.Base.Tools;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EPR_PROYECTO_ACTEQP
    /// </summary>
    public class EprProyectoActEqpHelper : HelperBase
    {
        public EprProyectoActEqpHelper(): base(Consultas.EprProyectoActEqpSql)
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
        public EprProyectoActEqpDTO Create(IDataReader dr)
        {
            EprProyectoActEqpDTO entity = new EprProyectoActEqpDTO();

            int iEpprproycodi = dr.GetOrdinal(this.Epproycodi);
            if (!dr.IsDBNull(iEpprproycodi)) entity.Epproycodi = Convert.ToInt32(dr.GetValue(iEpprproycodi));

            int iEpproysgcodi = dr.GetOrdinal(this.Epproysgcodi);
            if (!dr.IsDBNull(iEpproysgcodi)) entity.Epproysgcodi = Convert.ToInt32(dr.GetValue(iEpproysgcodi));

            int iEppproyflgtieneeo = dr.GetOrdinal(this.Eppproyflgtieneeo);
            if (!dr.IsDBNull(iEppproyflgtieneeo)) entity.Eppproyflgtieneeo = dr.GetValue(iEppproyflgtieneeo).ToString();

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEpproynemotecnico = dr.GetOrdinal(this.Epproynemotecnico);
            if (!dr.IsDBNull(iEpproynemotecnico)) entity.Epproynemotecnico = dr.GetValue(iEpproynemotecnico).ToString();

            int iEpproynomb = dr.GetOrdinal(this.Epproynomb);
            if (!dr.IsDBNull(iEpproysgcodi)) entity.Epproynomb = dr.GetValue(iEpproynomb).ToString();

            if (validaColumna(dr, this.Area))
            {
                int iArea = dr.GetOrdinal(this.Area);
                if (!dr.IsDBNull(iArea)) entity.Area = dr.GetValue(iArea).ToString();
            }

            int iEpproyfecregistro = dr.GetOrdinal(this.Epproyfecregistro);
            if (!dr.IsDBNull(iEpproyfecregistro)) entity.Epproyfecregistro = dr.GetValue(iEpproyfecregistro).ToString();


            if (validaColumna(dr, this.EpproyfecregistroDate))
            {
                int iEpproyfecregistroDate = dr.GetOrdinal(this.EpproyfecregistroDate);
                if (dr.GetValue(iEpproyfecregistroDate) != null && dr.GetValue(iEpproyfecregistroDate).ToString() != "")
                {
                    entity.Epproyfecregistrodate = DateTime.Parse(dr.GetValue(iEpproyfecregistroDate).ToString());
                }
            }

            int iEpproydescripcion = dr.GetOrdinal(this.Epproydescripcion);
            if (!dr.IsDBNull(iEpproydescripcion)) entity.Epproydescripcion = dr.GetValue(iEpproydescripcion).ToString();

            int iEpproyestregistro = dr.GetOrdinal(this.Epproyestregistro);
            if (!dr.IsDBNull(iEpproyestregistro)) entity.Epproyestregistro = dr.GetValue(iEpproyestregistro).ToString();

            int iEpproyusucreacion = dr.GetOrdinal(this.Epproyusucreacion);
            if (!dr.IsDBNull(iEpproyusucreacion)) entity.Epproyusucreacion = dr.GetValue(iEpproyusucreacion).ToString();

            int iEpproyfeccreacion = dr.GetOrdinal(this.Epproyfeccreacion);
            if (!dr.IsDBNull(iEpproyfeccreacion)) entity.Epproyfeccreacion = Convert.ToString(dr.GetValue(iEpproyfeccreacion));

            int iEppproyusumodificacion = dr.GetOrdinal(this.Eppproyusumodificacion);
            if (!dr.IsDBNull(iEppproyusumodificacion)) entity.Eppproyusumodificacion = dr.GetValue(iEppproyusumodificacion).ToString();

            int iEpproyfecmodificacion = dr.GetOrdinal(this.Epproyfecmodificacion);
            if (!dr.IsDBNull(iEpproyfecmodificacion)) entity.Epproyfecmodificacion = Convert.ToString(dr.GetValue(iEpproyfecmodificacion));

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
        public string Epproycodi = "EPPROYCODI";
        public string Epproysgcodi = "EPPROYSGCODI";
        public string Eppproyflgtieneeo = "EPPROYFLGTIENEEO";
        public string Emprcodi = "EMPRCODI";
        public string Epproynemotecnico = "EPPROYNEMOTECNICO";
        public string Epproynomb = "EPPROYNOMB";
        public string Area = "AREA";
        
        public string Epproyfecregistro = "EPPROYFECREGISTRO";
        public string EpproyfecregistroDate = "EPPROYFECREGISTRODATE";
        public string Epproydescripcion = "EPPROYDESCRIPCION";
        public string Epproyestregistro = "EPPROYESTREGISTRO";
        public string Epproyusucreacion = "EPPROYUSUCREACION";

        public string Epproyfeccreacion = "EPPROYFECCREACION";
        public string Eppproyusumodificacion = "EPPROYUSUMODIFICACION";
        public string Epproyfecmodificacion = "EPPROYFECMODIFICACION";

        public string EpproyfecregistroIni = "EPPROYFECREGISTROINI";
        public string EpproyfecregistroFin = "EPPROYFECREGISTROFIN";

        public string Emprnomb = "EMPRNOMB";
        public string Equicodi = "EQUICODI";
        #endregion

        #region Campos Paginacion
        public static string MaxRowToFetch = "MAXROWTOFETCH";
        public static string MinRowToFetch = "MINROWTOFETCH";
        #endregion

        public string SqlListarProyecto
        {
            get { return base.GetSqlXml("SqlListarProyecto"); }
        }

        public string ListProyectoProyectoActualizacion
        {
            get { return base.GetSqlXml("ListProyectoProyectoActualizacion"); }
        }

        public string SqlListMaestroProyecto
        {
            get { return base.GetSqlXml("ListMaestroProyecto"); }
        }

        public string ValidarProyectoPorRele
        {
            get { return base.GetSqlXml("ValidarProyectoPorRele"); }
        }
    }
}
