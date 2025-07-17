using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Scada;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Scada
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public class TrCargaarchxmlSp7Helper : HelperBase
    {
        public TrCargaarchxmlSp7Helper(): base(Consultas.TrCargaarchxmlSp7Sql)
        {
        }

        public TrCargaarchxmlSp7DTO Create(IDataReader dr)
        {
            TrCargaarchxmlSp7DTO entity = new TrCargaarchxmlSp7DTO();

            int iCarCodi = dr.GetOrdinal(this.CarCodi);
            if (!dr.IsDBNull(iCarCodi)) entity.CarCodi = Convert.ToInt32(dr.GetValue(iCarCodi));

            int iCarFecha = dr.GetOrdinal(this.CarFecha);
            if (!dr.IsDBNull(iCarFecha)) entity.CarFecha = dr.GetDateTime(iCarFecha);

            int iCarCantidad = dr.GetOrdinal(this.CarCantidad);
            if (!dr.IsDBNull(iCarCantidad)) entity.CarCantidad = Convert.ToInt32(dr.GetValue(iCarCantidad));

            int iCarUsuario = dr.GetOrdinal(this.CarUsuario);
            if (!dr.IsDBNull(iCarUsuario)) entity.CarUsuario = dr.GetString(iCarUsuario);

            int iCarNombreXML = dr.GetOrdinal(this.CarNombreXML);
            if (!dr.IsDBNull(iCarNombreXML)) entity.CarNombreXML = dr.GetString(iCarNombreXML);

            int iCarTipo = dr.GetOrdinal(this.CarTipo);
            if (!dr.IsDBNull(iCarTipo)) entity.CarTipo = Convert.ToInt32(dr.GetValue(iCarTipo));

            return entity;
        }


        #region Mapeo de Campos

        public string CarCodi = "CARCODI";
        public string CarFecha = "CARFECHA";
        public string CarCantidad = "CARCANTIDAD";
        public string CarUsuario = "CARUSUARIO";
        public string CarNombreXML = "CARNOMBREXML";
        public string CarTipo = "CARTIPO";

        public string GetByFecha
        {
            get { return base.GetSqlXml("GetByFecha"); }
        }        

        #endregion
    }
}
