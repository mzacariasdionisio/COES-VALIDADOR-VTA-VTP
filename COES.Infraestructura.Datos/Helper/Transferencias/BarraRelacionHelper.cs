using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using System.Data;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    public class BarraRelacionHelper : HelperBase
    {
        public BarraRelacionHelper() : base(Consultas.BarraRelacionSql)
        {
        }

        #region MonitoreoMME
        public string Barecodi= "BARECODI";
        public string Barrcoditra = "BARRCODITRA";
        public string Barrcodisum = "BARRCODISUM";
        public string Bareusucreacion = "BAREUSUCREACION";
        public string Barefeccreacion = "BAREFECCREACION";
        public string Bareusumodificacion = "BAREUSUMODIFICACION";
        public string Barefecmodificacion= "BAREFECMODIFICACION";
        public string Bareestado = "BAREESTADO";
        //PARA LA VISTA
        public string Barrnombsum = "BARRNOMBSUM";
        public string BarrTension = "BARRTENSION";

        #endregion

        public string SqlCodigoGenerado
        {
            get { return base.GetSqlXml("GetMaxId"); }
        }

        public string SqlListaRelacion
        {
            get { return base.GetSqlXml("ListaRelacion"); }
        }

        public string SqlExisteRelacionBarra
        {
            get { return base.GetSqlXml("ExisteRelacionBarra"); }
        }

    }
}
