using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class SolicitudCodigoDetalleHelper : HelperBase
    {
        public SolicitudCodigoDetalleHelper() : base(Consultas.SolicitudCodigoDetalleSql)
        {

        }

        public SolicitudCodigoDetalleDTO Create(IDataReader dr)
        {
            SolicitudCodigoDetalleDTO entity = new SolicitudCodigoDetalleDTO();

            int iSolcodretdetcodi = dr.GetOrdinal(this.Solcodretdetcodi);
            if (!dr.IsDBNull(iSolcodretdetcodi)) entity.Coresdcodi = dr.GetInt32(iSolcodretdetcodi);

            int iBarrcoditra = dr.GetOrdinal(this.Barrcoditra);
            if (!dr.IsDBNull(iBarrcoditra)) entity.Barracoditra = dr.GetInt32(iBarrcoditra);

            int iBarrcodisum = dr.GetOrdinal(this.Barrcodisum);
            if (!dr.IsDBNull(iBarrcodisum)) entity.Barracodisum = dr.GetInt32(iBarrcodisum);

            int iBarrnombsum = dr.GetOrdinal(this.Barrnombsum);
            if (!dr.IsDBNull(iBarrnombsum)) entity.Barranomsum = dr.GetString(iBarrnombsum);

            return entity;
        }

        #region Mapeo de Campos
        public string Solcodretdetcodi = "CORESDCODI";
        public string Solcodretcodi = "CORESOCODI";
        public string Barrcoditra = "BARRCODITRA";
        public string Barrcodisum = "BARRCODISUM";
        public string Solcodretdetnroregistro = "CORESDREG";
        public string Solcodretdetusucreacion = "CORESDUSUCREACION";
        public string Solcodretdetfecregistro = "CORESDFECCREACION";
        public string Solcodretdetusumodificacion = "CORESDUSUMODIFICACION";
        public string Solcodretdetfecmodificacion = "CORESDFECMODIFICACION";

        //Para la vista
        public string Barrnombsum = "BARRNOMBSUM";

        //Sub Detalle
        public string Solcodgencodi = "COREGECODI";
        public string Solcodgenestado = "COREGEESTADO";
        public string Solcodgencodvtp = "COREGECODVTP";
        public string Solcodgenusucreacion = "CORESDUSUCREACION";
        public string Solcodgenfecregistro = "CORESDFECCREACION";
        public string Solcodgenusumodificacion = "COREGEUSUMODIFICACION";
        public string Solcodgenfecmodificacion = "COREGEFECMODIFICACION";

        //barra relacion
        public string Barrcodi = "BARRCODI";
        public string Barecodi = "BARECODI";
        public string Barrnombre = "BARRNOMBRE";
        public string Bareusucreacion = "BAREUSUCREACION";
        public string Barefeccreacion = "BAREFECCREACION";

        #endregion

        public string SqlGetMaxId
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListaRelacion
        {
            get { return base.GetSqlXml("ListaRelacion"); }
        }

        public string SqlSaveGenerado
        {
            get { return base.GetSqlXml("SaveGenerado"); }
        }

        public string SqlDeleteGenerado
        {
            get { return base.GetSqlXml("DeleteGenerado"); }
        }

        public string SqlGetMaxIdGenerado
        {
            get { return base.GetSqlXml("GetMaxIdGenerado"); }
        }

        public string SqlGetMaxIdBR
        {
            get { return base.GetSqlXml("GetMaxIdBR"); }
        }
        public string SqlSaveBR
        {
            get { return base.GetSqlXml("SaveBR"); }
        }
        public string SqlDeleteBR
        {
            get { return base.GetSqlXml("DeleteBR"); }
        }
        public string SqlListarBarraSuministro
        {
            get { return base.GetSqlXml("ListarBarraSuministro"); }
        }

        public string SqlListarDetalle
        {
            get { return base.GetSqlXml("ListarDetalle"); }
        }
        public string SqlTotalRecordsGenerado
        {
            get { return GetSqlXml("TotalRecordsGenerado"); }
        }

        public string SqlSolicitarBajarGenerado
        {
            get { return GetSqlXml("SolicitarBajarGenerado"); }
        }

        public string SqlGetByIdGenerado
        {
            get { return GetSqlXml("GetByIdGenerado"); }
        }
    }
}
