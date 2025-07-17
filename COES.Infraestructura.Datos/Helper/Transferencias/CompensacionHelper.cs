using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TRN_CAB_COPENSACION
    /// </summary>
    public class CompensacionHelper : HelperBase
    {
        public CompensacionHelper()
            : base(Consultas.CompensacionSql)
        {
        }

        public CompensacionDTO Create(IDataReader dr)
        {
            CompensacionDTO entity = new CompensacionDTO();

            int iCabecompcodi = dr.GetOrdinal(this.Cabecompcodi);
            if (!dr.IsDBNull(iCabecompcodi)) entity.CabeCompCodi = dr.GetInt32(iCabecompcodi);

            int iCabecompnombre = dr.GetOrdinal(this.Cabecompnombre);
            if (!dr.IsDBNull(iCabecompnombre)) entity.CabeCompNombre = dr.GetString(iCabecompnombre);

            int iCabecompver = dr.GetOrdinal(this.Cabecompver);
            if (!dr.IsDBNull(iCabecompver)) entity.CabeCompVer = dr.GetString(iCabecompver);

            int iCabecompestado = dr.GetOrdinal(this.Cabecompestado);
            if (!dr.IsDBNull(iCabecompestado)) entity.CabeCompEstado = dr.GetString(iCabecompestado);

            int iCabecompusername = dr.GetOrdinal(this.Cabecompusername);
            if (!dr.IsDBNull(iCabecompusername)) entity.CabeCompUserName = dr.GetString(iCabecompusername);

            int iCabecompfecins = dr.GetOrdinal(this.Cabecompfecins);
            if (!dr.IsDBNull(iCabecompfecins)) entity.CabeCompFecIns = dr.GetDateTime(iCabecompfecins);

            int iCabecompfecact = dr.GetOrdinal(this.Cabecompfecact);
            if (!dr.IsDBNull(iCabecompfecact)) entity.CabeCompFecAct = dr.GetDateTime(iCabecompfecact);

            int iCabecomppericodi = dr.GetOrdinal(this.Cabecomppericodi);
            if (!dr.IsDBNull(iCabecomppericodi)) entity.CabeCompPeriCodi = dr.GetInt32(iCabecomppericodi);

            int iCabecomprentconge = dr.GetOrdinal(this.Cabecomprentconge);
            if (!dr.IsDBNull(iCabecomprentconge)) entity.CabeCompRentConge = dr.GetString(iCabecomprentconge);

            return entity;
        }

        #region Mapeo de Campos

        public string Cabecompcodi = "CABCOMCODI";
        public string Cabecompnombre = "CABCOMNOMBRE";
        public string Cabecompver = "CABCOMVISUALIZAR";
        public string Cabecompestado = "CABCOMESTADO";
        public string Cabecompusername = "CABCOMUSERNAME";
        public string Cabecompfecins = "CABCOMFECINS";
        public string Cabecompfecact = "CABCOMFECACT";
        public string Cabecomppericodi = "PERICODI";
        public string Cabecomprentconge = "CABCOMRENTCONGE";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlGetByCodigo
        {
            get { return base.GetSqlXml("GetByCodigo"); }
        }

        public string SqlListBase
        {
            get { return base.GetSqlXml("ListBase"); }
        }

        public string SqlListReporte
        {
            get { return base.GetSqlXml("ListReporte"); }
        }
    }
}

