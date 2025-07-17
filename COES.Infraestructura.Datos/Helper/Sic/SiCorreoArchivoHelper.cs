using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_CORREO_ARCHIVO
    /// </summary>
    public class SiCorreoArchivoHelper : HelperBase
    {
        public SiCorreoArchivoHelper(): base(Consultas.SiCorreoArchivoSql)
        {
        }

        public SiCorreoArchivoDTO Create(IDataReader dr)
        {
            SiCorreoArchivoDTO entity = new SiCorreoArchivoDTO();

            int iEarchcodi = dr.GetOrdinal(this.Earchcodi);
            if (!dr.IsDBNull(iEarchcodi)) entity.Earchcodi = Convert.ToInt32(dr.GetValue(iEarchcodi));

            int iCorrcodi = dr.GetOrdinal(this.Corrcodi);
            if (!dr.IsDBNull(iCorrcodi)) entity.Corrcodi = Convert.ToInt32(dr.GetValue(iCorrcodi));

            int iEarchtipo = dr.GetOrdinal(this.Earchtipo);
            if (!dr.IsDBNull(iEarchtipo)) entity.Earchtipo = Convert.ToInt32(dr.GetValue(iEarchtipo));

            int iEarchnombreoriginal = dr.GetOrdinal(this.Earchnombreoriginal);
            if (!dr.IsDBNull(iEarchnombreoriginal)) entity.Earchnombreoriginal = dr.GetString(iEarchnombreoriginal);

            int iEarchnombrefisico = dr.GetOrdinal(this.Earchnombrefisico);
            if (!dr.IsDBNull(iEarchnombrefisico)) entity.Earchnombrefisico = dr.GetString(iEarchnombrefisico);

            int iEarchorden = dr.GetOrdinal(this.Earchorden);
            if (!dr.IsDBNull(iEarchorden)) entity.Earchorden = Convert.ToInt32(dr.GetValue(iEarchorden));

            int iEarchestado = dr.GetOrdinal(this.Earchestado);
            if (!dr.IsDBNull(iEarchestado)) entity.Earchestado = Convert.ToInt32(dr.GetValue(iEarchestado));

            return entity;
        }


        #region Mapeo de Campos

        public string Earchcodi = "EARCHCODI";
        public string Corrcodi = "CORRCODI";
        public string Earchtipo = "EARCHTIPO";
        public string Earchnombreoriginal = "EARCHNOMBREORIGINAL";
        public string Earchnombrefisico = "EARCHNOMBREFISICO";
        public string Earchorden = "EARCHORDEN";
        public string Earchestado = "EARCHESTADO";

        #endregion

        public string SqlGetByCorreos
        {
            get { return base.GetSqlXml("GetByCorreos"); }
        }
    }
}
