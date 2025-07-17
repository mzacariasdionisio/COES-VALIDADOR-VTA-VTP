using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Scada;
using COES.Dominio.Interfaces.Scada;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Scada;

namespace COES.Infraestructura.Datos.Respositorio.Scada
{
    /// <summary>
    /// Clase de acceso a datos de la tabla TR_CANALCAMBIO_SP7
    /// </summary>
    public class TrCargaarchxmlSp7Repository : RepositoryBase, ITrCargaarchxmlSp7Repository
    {
        public TrCargaarchxmlSp7Repository(string strConn): base(strConn)
        {
        }

        TrCargaarchxmlSp7Helper helper = new TrCargaarchxmlSp7Helper();

        public List<TrCargaarchxmlSp7DTO> GetByFecha(DateTime fechaInicial, DateTime fechaFinal)
        {
            List<TrCargaarchxmlSp7DTO> entitys = new List<TrCargaarchxmlSp7DTO>();
            String sql = String.Format(this.helper.GetByFecha, fechaInicial.ToString(ConstantesBase.FormatoFecha), 
            fechaFinal.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrCargaarchxmlSp7DTO entity = new TrCargaarchxmlSp7DTO();

                    //int iCarCodi = dr.GetOrdinal(this.helper.CarCodi);
                    //if (!dr.IsDBNull(iCarCodi)) entity.CarCodi = Convert.ToInt32(dr.GetValue(iCarCodi));

                    int iCarFecha = dr.GetOrdinal(this.helper.CarFecha);
                    if (!dr.IsDBNull(iCarFecha)) entity.CarFecha = dr.GetDateTime(iCarFecha);

                    int iCarCantidad = dr.GetOrdinal(this.helper.CarCantidad);
                    if (!dr.IsDBNull(iCarCantidad)) entity.CarCantidad = Convert.ToInt32(dr.GetValue(iCarCantidad));

                    int iCarUsuario = dr.GetOrdinal(this.helper.CarUsuario);
                    if (!dr.IsDBNull(iCarUsuario)) entity.CarUsuario = dr.GetString(iCarUsuario);

                    //int iCarNombreXML = dr.GetOrdinal(this.helper.CarNombreXML);
                    //if (!dr.IsDBNull(iCarNombreXML)) entity.CarNombreXML = dr.GetString(iCarNombreXML);

                    int iCarTipo = dr.GetOrdinal(this.helper.CarTipo);
                    if (!dr.IsDBNull(iCarTipo)) entity.CarTipo = Convert.ToInt32(dr.GetValue(iCarTipo));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}
