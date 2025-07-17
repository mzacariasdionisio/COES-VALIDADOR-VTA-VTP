using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IN_ARCHIVO
    /// </summary>
    public class InArchivoHelper : HelperBase
    {
        public InArchivoHelper() : base(Consultas.InArchivoSql)
        {
        }

        public InArchivoDTO Create(IDataReader dr)
        {
            InArchivoDTO entity = new InArchivoDTO();

            int iInarchcodi = dr.GetOrdinal(this.Inarchcodi);
            if (!dr.IsDBNull(iInarchcodi)) entity.Inarchcodi = Convert.ToInt32(dr.GetValue(iInarchcodi));

            int iInarchnombreoriginal = dr.GetOrdinal(this.Inarchnombreoriginal);
            if (!dr.IsDBNull(iInarchnombreoriginal)) entity.Inarchnombreoriginal = dr.GetString(iInarchnombreoriginal);
            entity.Inarchnombreoriginal = entity.Inarchnombreoriginal  ?? "";

            int iInarchnombrefisico = dr.GetOrdinal(this.Inarchnombrefisico);
            if (!dr.IsDBNull(iInarchnombrefisico)) entity.Inarchnombrefisico = dr.GetString(iInarchnombrefisico);
            entity.Inarchnombrefisico = entity.Inarchnombrefisico ?? "";

            int iInarchorden = dr.GetOrdinal(this.Inarchorden);
            if (!dr.IsDBNull(iInarchorden)) entity.Inarchorden = Convert.ToInt32(dr.GetValue(iInarchorden));

            int iInarchestado = dr.GetOrdinal(this.Inarchestado);
            if (!dr.IsDBNull(iInarchestado)) entity.Inarchestado = Convert.ToInt32(dr.GetValue(iInarchestado));

            int iInarchtipo = dr.GetOrdinal(this.Inarchtipo);
            if (!dr.IsDBNull(iInarchtipo)) entity.Inarchtipo = Convert.ToInt32(dr.GetValue(iInarchtipo));

            if (string.IsNullOrEmpty(entity.Inarchnombrefisico)) entity.Inarchnombrefisico = entity.Inarchnombreoriginal; //cuando se subió con error al FileServer

            return entity;
        }

        #region Mapeo de Campos

        public string Inarchcodi = "INARCHCODI";
        public string Inarchnombreoriginal = "INARCHNOMBREORIGINAL";
        public string Inarchnombrefisico = "INARCHNOMBREFISICO";
        public string Inarchorden = "INARCHORDEN";
        public string Inarchestado = "INARCHESTADO";
        public string Inarchtipo = "INARCHTIPO";

        public string Infmmcodi = "INFMMCODI";
        public string Infvercodi = "INFVERCODI";
        public string Infmmhoja = "INFMMHOJA";

        public string Emprnomb = "EMPRNOMB";
        public string Areanomb = "AREANOMB";
        public string Equiabrev = "EQUIABREV";
        public string Equicodi = "EQUICODI";
        public string Emprcodi = "EMPRCODI";

        public string Intercodi = "INTERCODI";
        public string Progrcodi = "PROGRCODI";
        public string Intercarpetafiles = "INTERCARPETAFILES";
        public string Msgcodi = "MSGCODI";
        public string Instcodi = "INSTCODI";
        public string Instdcodi = "INSTDCODI";

        #endregion

        public string SqlListByIntervencion
        {
            get { return GetSqlXml("ListByIntervencion"); }
        }

        public string SqlListByMensaje
        {
            get { return GetSqlXml("ListByMensaje"); }
        }

        public string SqlListBySustento
        {
            get { return GetSqlXml("ListBySustento"); }
        }

        public string SqlListarArchivoSinFormato
        {
            get { return GetSqlXml("ListarArchivoSinFormato"); }
        }

    }
}
