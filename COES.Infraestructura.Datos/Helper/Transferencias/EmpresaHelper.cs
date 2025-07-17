using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Transferencias;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Transferencias
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SI_EMPRESA
    /// </summary>
    public class EmpresaHelper : HelperBase
    {
        public EmpresaHelper(): base(Consultas.EmpresaSql) 
        {
        }
        private bool columnsExist(string columnName, IDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {

                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
        public EmpresaDTO Create(IDataReader dr)
        {
            EmpresaDTO entity = new EmpresaDTO();

            int iEmprCodi = dr.GetOrdinal(this.EmprCodi);
            if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);

            int iEmprNombre = dr.GetOrdinal(this.EmprNombre);
            if (!dr.IsDBNull(iEmprNombre)) entity.EmprNombre = dr.GetString(iEmprNombre);

            int iTipoEmprCodi = dr.GetOrdinal(this.TipoEmprCodi);
            if (!dr.IsDBNull(iTipoEmprCodi)) entity.TipoEmprCodi = dr.GetInt32(iTipoEmprCodi);

            return entity;
        }


        #region Mapeo de Campos
       
        public string EmprCodi = "EMPRCODI";
        public string EmprNombre = "EMPRNOMB";
        public string TipoEmprCodi = "TIPOEMPRCODI";
        public string EMPRABREV = "EMPRABREV";
        public string Pericodi = "PERICODI";
        public string EmprAbrevCodi = "EmprAbrevCodi";
        public string Mensaje = "MENSAJE";
        public string Emprrazsocial = "EMPRRAZSOCIAL";
        public string Verscodi = "VERSION";

        #endregion

        public string SqlObtenerPorTipo
        {
            get { return base.GetSqlXml("ObtenerPorTipo"); }
        }

        public string SqlGetByNombre
        {
            get { return base.GetSqlXml("GetByNombre"); }
        }
        public string SqlGetByNombreEstado
        {
            get { return base.GetSqlXml("GetByNombreEstado"); }

        }
        

        public string SqlListEmpresasSTR
        {
            get { return base.GetSqlXml("ListEmpresasSTR"); }

        }

        public string SqlListaInterCodEnt
        {
            get { return base.GetSqlXml("ListaInterCodEnt"); }

        }

        public string SqlListaInterCoReSoGen
        {
            get { return base.GetSqlXml("ListaInterCoReSoGen"); }

        }

        public string SqlListaInterCoReSoCli
        {
            get { return base.GetSqlXml("ListaInterCoReSoCli"); }

        }
        public string SqlListaInterCoReSoCliPorEmpresa
        {
            get { return base.GetSqlXml("ListaInterCoReSoCliPorEmpresa"); }

        }   
        public string SqlListaRetiroCliente
        {
            get { return base.GetSqlXml("ListaRetiroCliente"); }

        }

        public string SqlListaInterCoReSC
        {
            get { return base.GetSqlXml("ListaInterCoReSC"); }

        }

        public string SqlListaInterValorTrans
        {
            get { return base.GetSqlXml("ListaInterValorTrans"); }

        }

        public string SqlListaInterCodInfoBase 
        {
            get { return base.GetSqlXml("ListaInterCodInfoBase"); }
        }

        public string SqlListEmpresasCombo
        {
            get { return base.GetSqlXml("ListEmpresasCombos"); }
        }

        public string SqlListInterCodEntregaRetiro  
        {
            get { return base.GetSqlXml("ListInterCodEntregaRetiro"); }
        }

        public string SqlListInterCodEntregaRetiroxPeriodo
        {
            get { return base.GetSqlXml("ListInterCodEntregaRetiroxPeriodo"); }
        }

        public string SqlListEmpresasComboActivos
        {
            get { return base.GetSqlXml("ListEmpresasCombosActivos"); }
        }

        #region SQL
        public string SqlActualizarAbreviatura
        {
            get { return base.GetSqlXml("ActualizarAbreviatura"); }
        }
         
        #endregion SQL

        public string SqlListEmpresasConfPtoMME
        {
            get { return base.GetSqlXml("ListEmpresasConfPtoMME"); }
        }

        public string SqlGetByNombreSic
        {
            get { return base.GetSqlXml("GetByNombreSic"); }
        }
    }
}
