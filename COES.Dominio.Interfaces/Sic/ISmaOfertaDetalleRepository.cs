using System;
using System.Collections.Generic;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

using System.Data; //STS
using System.Data.Common; //STS

namespace COES.Dominio.Interfaces.Sic
{
    /// <summary>
    /// Interface de acceso a datos de la tabla SMA_OFERTA_DETALLE
    /// </summary>
    public interface ISmaOfertaDetalleRepository
    {
        int GetMaxId();
        int Save(int ofercodi, SmaOfertaDetalleDTO entity, IDbConnection conn, DbTransaction tran, int corrOferdet);
        void Update(SmaOfertaDetalleDTO entity);

        void UpdatePrecio(int ofdecodi, string precio, DateTime fechaActualizacion, string usuarioActualizacion);
        void Delete(int ofdecodi);
        SmaOfertaDetalleDTO GetById(int ofdecodi);
        List<SmaOfertaDetalleDTO> List();
        int GetByCriteria(int urscodi);

        #region FIT - VALORIZACIONES DIARIAS
        List<SmaOfertaDetalleDTO> ListByDate(DateTime fechaOfertaIni, DateTime fechaOfertaFin, int tipoOferta, string estado);
        List<SmaOfertaDetalleDTO> ListByDateTipo(DateTime fechaOfertaIni, DateTime fechaOfertaFin, int tipoOferta, string estado);
        #endregion

        List<SmaOfertaDetalleDTO> ListarPorOfertas(string ofercodis);

    }
}
