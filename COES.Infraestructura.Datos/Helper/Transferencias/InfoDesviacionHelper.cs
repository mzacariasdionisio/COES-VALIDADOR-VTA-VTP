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
    /// Clase que contiene el mapeo de la consulta para la desviacion
    /// </summary>
    public class InfoDesviacionHelper:HelperBase
    {
        public InfoDesviacionHelper() : base(Consultas.InfoDesviacionSql)
        {
        }

        public InfoDesviacionDTO Create(IDataReader dr)
        {
            InfoDesviacionDTO entity = new InfoDesviacionDTO();


            int iCodigo = dr.GetOrdinal(this.Codigo);
            if (!dr.IsDBNull(iCodigo)) entity.Codigo = dr.GetString(iCodigo);

            int iGenerador = dr.GetOrdinal(this.Generador);
            if (!dr.IsDBNull(iGenerador)) entity.Generador = dr.GetString(iGenerador);

            int iCliente = dr.GetOrdinal(this.Cliente);
            if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

            int iNroDia = dr.GetOrdinal(this.NroDia);
            if (!dr.IsDBNull(iNroDia)) entity.NroDia = dr.GetDecimal(iNroDia);

            int iEnergia = dr.GetOrdinal(this.Energia);
            if (!dr.IsDBNull(iEnergia)) entity.Energia = dr.GetDecimal(iEnergia);

            return entity;
        }

        #region Mapeo de Campos

        public string Codigo = "CODIGO";
        public string GenEmprCodi = "GENEMPRCODI";
        public string Generador = "GENERADOR";
        public string CliEmprCodi = "CLIEMPRCODI";
        public string Cliente = "CLIENTE";
        public string NroDia = "NRODIA";
        public string Energia = "ENERGIA";
        
        public string PeriCodi = "PERICODI";
        public string Version = "VERSION";
        public string BarrCodi = "BARRCODI";
        #endregion

        public string SqlGetByListaCodigo
        {
            get { return base.GetSqlXml("GetByListaCodigo"); }
        }

        public string SqlGetByEnergiaByBarraCodigo
        {
            get { return base.GetSqlXml("GetByEnergiaByBarraCodigo"); }
        }
    }
}
