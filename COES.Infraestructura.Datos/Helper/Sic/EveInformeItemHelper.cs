using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla EVE_INFORME_ITEM
    /// </summary>
    public class EveInformeItemHelper : HelperBase
    {
        public EveInformeItemHelper(): base(Consultas.EveInformeItemSql)
        {
        }

        public EveInformeItemDTO Create(IDataReader dr)
        {
            EveInformeItemDTO entity = new EveInformeItemDTO();

            int iInfitemcodi = dr.GetOrdinal(this.Infitemcodi);
            if (!dr.IsDBNull(iInfitemcodi)) entity.Infitemcodi = Convert.ToInt32(dr.GetValue(iInfitemcodi));

            int iEveninfcodi = dr.GetOrdinal(this.Eveninfcodi);
            if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

            int iItemnumber = dr.GetOrdinal(this.Itemnumber);
            if (!dr.IsDBNull(iItemnumber)) entity.Itemnumber = Convert.ToInt32(dr.GetValue(iItemnumber));

            int iSubitemnumber = dr.GetOrdinal(this.Subitemnumber);
            if (!dr.IsDBNull(iSubitemnumber)) entity.Subitemnumber = Convert.ToInt32(dr.GetValue(iSubitemnumber));

            int iNroitem = dr.GetOrdinal(this.Nroitem);
            if (!dr.IsDBNull(iNroitem)) entity.Nroitem = Convert.ToInt32(dr.GetValue(iNroitem));

            int iPotactiva = dr.GetOrdinal(this.Potactiva);
            if (!dr.IsDBNull(iPotactiva)) entity.Potactiva = dr.GetDecimal(iPotactiva);

            int iPotreactiva = dr.GetOrdinal(this.Potreactiva);
            if (!dr.IsDBNull(iPotreactiva)) entity.Potreactiva = dr.GetDecimal(iPotreactiva);

            int iEquicodi = dr.GetOrdinal(this.Equicodi);
            if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

            int iNiveltension = dr.GetOrdinal(this.Niveltension);
            if (!dr.IsDBNull(iNiveltension)) entity.Niveltension = dr.GetDecimal(iNiveltension);

            int iDesobservacion = dr.GetOrdinal(this.Desobservacion);
            if (!dr.IsDBNull(iDesobservacion)) entity.Desobservacion = dr.GetString(iDesobservacion);

            int iItemhora = dr.GetOrdinal(this.Itemhora);
            if (!dr.IsDBNull(iItemhora)) entity.Itemhora = dr.GetString(iItemhora);

            int iSenializacion = dr.GetOrdinal(this.Senializacion);
            if (!dr.IsDBNull(iSenializacion)) entity.Senializacion = dr.GetString(iSenializacion);

            int iInterrcodi = dr.GetOrdinal(this.Interrcodi);
            if (!dr.IsDBNull(iInterrcodi)) entity.Interrcodi = Convert.ToInt32(dr.GetValue(iInterrcodi));

            int iAc = dr.GetOrdinal(this.Ac);
            if (!dr.IsDBNull(iAc)) entity.Ac = dr.GetString(iAc);

            int iRa = dr.GetOrdinal(this.Ra);
            if (!dr.IsDBNull(iRa)) entity.Ra = Convert.ToInt32(dr.GetValue(iRa));

            int iSa = dr.GetOrdinal(this.Sa);
            if (!dr.IsDBNull(iSa)) entity.Sa = Convert.ToInt32(dr.GetValue(iSa));

            int iTa = dr.GetOrdinal(this.Ta);
            if (!dr.IsDBNull(iTa)) entity.Ta = Convert.ToInt32(dr.GetValue(iTa));

            int iRd = dr.GetOrdinal(this.Rd);
            if (!dr.IsDBNull(iRd)) entity.Rd = Convert.ToInt32(dr.GetValue(iRd));

            int iSd = dr.GetOrdinal(this.Sd);
            if (!dr.IsDBNull(iSd)) entity.Sd = Convert.ToInt32(dr.GetValue(iSd));

            int iTd = dr.GetOrdinal(this.Td);
            if (!dr.IsDBNull(iTd)) entity.Td = Convert.ToInt32(dr.GetValue(iTd));

            int iDescomentario = dr.GetOrdinal(this.Descomentario);
            if (!dr.IsDBNull(iDescomentario)) entity.Descomentario = dr.GetString(iDescomentario);

            int iSumininistro = dr.GetOrdinal(this.Sumininistro);
            if (!dr.IsDBNull(iSumininistro)) entity.Sumininistro = dr.GetString(iSumininistro);

            int iPotenciamw = dr.GetOrdinal(this.Potenciamw);
            if (!dr.IsDBNull(iPotenciamw)) entity.Potenciamw = dr.GetDecimal(iPotenciamw);

            int iProteccion = dr.GetOrdinal(this.Proteccion);
            if (!dr.IsDBNull(iProteccion)) entity.Proteccion = dr.GetString(iProteccion);

            int iIntinicio = dr.GetOrdinal(this.Intinicio);
            if (!dr.IsDBNull(iIntinicio)) entity.Intinicio = dr.GetDateTime(iIntinicio);

            int iIntfin = dr.GetOrdinal(this.Intfin);
            if (!dr.IsDBNull(iIntfin)) entity.Intfin = dr.GetDateTime(iIntfin);
            
            int iSubestacionde = dr.GetOrdinal(this.Subestacionde);
            if (!dr.IsDBNull(iSubestacionde)) entity.Subestacionde = dr.GetString(iSubestacionde);

            int iSubestacionhasta = dr.GetOrdinal(this.Subestacionhasta);
            if (!dr.IsDBNull(iSubestacionhasta)) entity.Subestacionhasta = dr.GetString(iSubestacionhasta);

            int iPtointerrcodi = dr.GetOrdinal(this.Ptointerrcodi);
            if (!dr.IsDBNull(iPtointerrcodi)) entity.Ptointerrcodi = Convert.ToInt32(dr.GetValue(iPtointerrcodi));

            return entity;
        }


        #region Mapeo de Campos

        public string Infitemcodi = "INFITEMCODI";
        public string Eveninfcodi = "EVENINFCODI";
        public string Itemnumber = "ITEMNUMBER";
        public string Subitemnumber = "SUBITEMNUMBER";
        public string Nroitem = "NROITEM";
        public string Potactiva = "POTACTIVA";
        public string Potreactiva = "POTREACTIVA";
        public string Equicodi = "EQUICODI";
        public string Niveltension = "NIVELTENSION";
        public string Desobservacion = "DESOBSERVACION";
        public string Itemhora = "ITEMHORA";
        public string Senializacion = "SENIALIZACION";
        public string Interrcodi = "INTERRCODI";
        public string Ac = "AC";
        public string Ra = "RA";
        public string Sa = "SA";
        public string Ta = "TA";
        public string Rd = "RD";
        public string Sd = "SD";
        public string Td = "TD";
        public string Descomentario = "DESCOMENTARIO";
        public string Sumininistro = "SUMININISTRO";
        public string Potenciamw = "POTENCIAMW";
        public string Proteccion = "PROTECCION";
        public string Intinicio = "INTINICIO";
        public string Intfin = "INTFIN";       
        public string Subestacionde = "SUBESTACIONDE";
        public string Subestacionhasta = "SUBESTACIONHASTA";
        public string Evencodi = "EVENCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equinomb = "EQUINOMB";
        public string Areanomb = "AREANOMB";        
        public string Internomb = "INTERNOMB";
        public string Ptointerrcodi = "PTOINTERRCODI";

        public string SqlObtenerItemInformeEvento
        {
            get { return base.GetSqlXml("ObtenerItemInformeEvento"); }
        }

        public string SqlObtenerItemTotalInformeEvento
        {
            get { return base.GetSqlXml("ObtenerItemTotalInformeEvento"); }
        }

        public string SqlObtenerItemInformePorId
        {
            get { return base.GetSqlXml("ObtenerItemInformePorId"); }
        }

        public string SqlVerificarExistencia
        {
            get { return base.GetSqlXml("VerificarExistencia"); }
        }

        public string SqlActualizarTextoInforme
        {
            get { return base.GetSqlXml("ActualizarTextoInforme"); }
        }

        public string SqlDeletePorInforme
        {
            get { return base.GetSqlXml("DeletePorInforme"); }
        }

        public string SqlDeleteConsolidado
        {
            get { return base.GetSqlXml("DeleteConsolidado"); }
        }

        public string SqlSaveConsolidado
        {
            get { return base.GetSqlXml("SaveConsolidado"); }
        }

        public string SqlObtenerInformeInterrupcion
        {
            get { return base.GetSqlXml("ObtenerInformeInterrupcion"); }
        }

        public string SqlActualizarInformeItem
        {
            get { return base.GetSqlXml("ActualizarInformeItem"); }
        }

        #endregion
    }
}
