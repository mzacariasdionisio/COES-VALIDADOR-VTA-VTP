using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla IND_RELACION_EMPRESA
    /// </summary>
    public class IndRelacionEmpresaHelper : HelperBase
    {

        public IndRelacionEmpresaHelper() : base(Consultas.IndRelacionEmpresaSql)
        {
        }
        public IndRelacionEmpresaDTO Create(IDataReader dr)
        {
            IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

            int iRelempcodi = dr.GetOrdinal(this.Relempcodi);
            if (!dr.IsDBNull(iRelempcodi)) entity.Relempcodi = Convert.ToInt32(dr.GetValue(iRelempcodi));

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iEquicodiunidad = dr.GetOrdinal(this.Equicodiunidad);
            if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

            int iEquicodicentral = dr.GetOrdinal(this.Equicodicentral);
            if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = Convert.ToInt32(dr.GetValue(iEquicodicentral));

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iFamcodi = dr.GetOrdinal(this.Famcodi);
            if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

            int iRelempunidadnomb = dr.GetOrdinal(this.Relempunidadnomb);
            if (!dr.IsDBNull(iRelempunidadnomb)) entity.Relempunidadnomb = dr.GetString(iRelempunidadnomb);

            int iGaseoductoequicodi = dr.GetOrdinal(this.Gaseoductoequicodi);
            if (!dr.IsDBNull(iGaseoductoequicodi)) entity.Gaseoductoequicodi = Convert.ToInt32(dr.GetValue(iGaseoductoequicodi));

            int iGrupocodicn2 = dr.GetOrdinal(this.Grupocodicn2);
            if (!dr.IsDBNull(iGrupocodicn2)) entity.Grupocodicn2 = Convert.ToInt32(dr.GetValue(iGrupocodicn2));

            int iRelempcuadro1 = dr.GetOrdinal(this.Relempcuadro1);
            if (!dr.IsDBNull(iRelempcuadro1)) entity.Relempcuadro1 = dr.GetString(iRelempcuadro1);

            int iRelempcuadro2 = dr.GetOrdinal(this.Relempcuadro2);
            if (!dr.IsDBNull(iRelempcuadro2)) entity.Relempcuadro2 = dr.GetString(iRelempcuadro2);

            int iRelempsucad = dr.GetOrdinal(this.Relempsucad);
            if (!dr.IsDBNull(iRelempsucad)) entity.Relempsucad = dr.GetString(iRelempsucad);

            int iRelempsugad = dr.GetOrdinal(this.Relempsugad);
            if (!dr.IsDBNull(iRelempsugad)) entity.Relempsugad = dr.GetString(iRelempsugad);

            int iRelempestado = dr.GetOrdinal(this.Relempestado);
            if (!dr.IsDBNull(iRelempestado)) entity.Relempestado = dr.GetString(iRelempestado);

            int iRelemptecnologia = dr.GetOrdinal(this.Relemptecnologia);
            if (!dr.IsDBNull(iRelemptecnologia)) entity.Relemptecnologia = Convert.ToInt32(dr.GetValue(iRelemptecnologia));

            int iRelempusucreacion = dr.GetOrdinal(this.Relempusucreacion);
            if (!dr.IsDBNull(iRelempusucreacion)) entity.Relempusucreacion = dr.GetString(iRelempusucreacion);

            int iRelempfeccreacion = dr.GetOrdinal(this.Relempfeccreacion);
            if (!dr.IsDBNull(iRelempfeccreacion)) entity.Relempfeccreacion = dr.GetDateTime(iRelempfeccreacion);

            int iRelempusumodificacion = dr.GetOrdinal(this.Relempusumodificacion);
            if (!dr.IsDBNull(iRelempusumodificacion)) entity.Relempusumodificacion = dr.GetString(iRelempusumodificacion);

            int iRelempfecmodificacion = dr.GetOrdinal(this.Relempfecmodificacion);
            if (!dr.IsDBNull(iRelempfecmodificacion)) entity.Relempfecmodificacion = dr.GetDateTime(iRelempfecmodificacion);


            return entity;
        }

        #region Mapeo de Campos

        public string Relempcodi = "RELEMPCODI";
        public string Emprcodi = "EMPRCODI";
        public string Equicodiunidad = "EQUICODIUNIDAD";
        public string Equicodicentral = "EQUICODICENTRAL";
        public string Grupocodi = "GRUPOCODI";
        public string Famcodi = "FAMCODI";
        public string Relempunidadnomb = "RELEMPUNIDADNOMB";
        public string Gaseoductoequicodi = "GASEODUCTOEQUICODI";
        public string Grupocodicn2 = "GRUPOCODICN2";
        public string Relempcuadro1 = "RELEMPCUADRO1";
        public string Relempcuadro2 = "RELEMPCUADRO2";
        public string Relempsucad = "RELEMPSUCAD";
        public string Relempsugad = "RELEMPSUGAD";
        public string Relempestado = "RELEMPESTADO";
        public string Relemptecnologia = "RELEMPTECNOLOGIA";
        public string Relempusucreacion = "RELEMPUSUCREACION";
        public string Relempfeccreacion = "RELEMPFECCREACION";
        public string Relempusumodificacion = "RELEMPUSUMODIFICACION";
        public string Relempfecmodificacion = "RELEMPFECMODIFICACION";
        
        //Atributos de consulta
        public string Emprnomb = "EMPRNOMB";
        public string Equinomb = "EQUINOMB";
        public string Equinombcentral = "EQUINOMBCENTRAL";
        public string Equinombunidad = "EQUINOMBUNIDAD";
        public string Gaseoductoequinomb = "GASEODUCTOEQUINOMB";
        public string Gruporeservafria = "GRUPORESERVAFRIA";
        public string Gruponomb = "GRUPONOMB";

        public string Concepdesc = "CONCEPDESC";
        public string Concepabrev = "CONCEPABREV";
        public string Conceporden = "CONCEPORDEN";
        public string Concepunid = "CONCEPUNID";
        public string Concepcodi = "CONCEPCODI";
        public string Formuladat = "FORMULADAT";
        public string Fechadat = "FECHADAT";
        public string Catecodi = "CATECODI";
        public string Lastuser = "LASTUSER";

        public string Equicodi = "EQUICODI";
        public string Greqvafechadat = "GREQVAFECHADAT";
        public string Greqvaformuladat = "GREQVAFORMULADAT";
        public string Greqvadeleted = "GREQVADELETED";
        public string Greqvausucreacion = "GREQVAUSUCREACION";
        public string Greqvafeccreacion = "GREQVAFECCREACION";
        public string Greqvausumodificacion = "GREQVAUSUMODIFICACION";
        public string Greqvafecmodificacion = "GREQVAFECMODIFICACION";
        public string Greqvacomentario = "GREQVACOMENTARIO";
        public string Greqvasustento = "GREQVASUSTENTO";
        public string Greqvacheckcero = "GREQVACHECKCERO";
        public string Conceppadre = "CONCEPPADRE";
        #endregion


        #region Consultas a la BD

        public string SqlListByIdEmpresa
        {
            get { return base.GetSqlXml("ListByIdEmpresa"); }
        }

        public string SqlGetByIdCentral
        {
            get { return base.GetSqlXml("GetByIdCentral"); }
        }

        public string SqlGetByIdUnidad
        {
            get { return base.GetSqlXml("GetByIdUnidad"); }
        }


        public string SqlListEmpresas
        {
            get { return base.GetSqlXml("ListEmpresas"); }
        }

        public string SqlListEmpresasConGaseoducto
        {
            get { return base.GetSqlXml("ListEmpresasConGaseoducto"); }
        }

        public string SqlListCentrales
        {
            get { return base.GetSqlXml("ListCentrales"); }
        }

        public string SqlListCentralesConGaseoducto
        {
            get { return base.GetSqlXml("ListCentralesConGaseoducto"); }
        }

        public string SqlListUnidades
        {
            get { return base.GetSqlXml("ListUnidades"); }
        }

        public string SqlListGrupos
        {
            get { return base.GetSqlXml("ListGrupos"); }
        }

        public string SqlListUnidadNombres
        {
            get { return base.GetSqlXml("ListUnidadNombres"); }
        }
        public string SqlListUnidadNombresConGaseoducto
        {
            get { return base.GetSqlXml("ListUnidadNombresConGaseoducto"); }
        }

        public string SqlListCentral
        {
            get { return base.GetSqlXml("ListCentral"); }
        }

        public string SqlListUnidad
        {
            get { return base.GetSqlXml("ListUnidad"); }
        }

        public string SqlListGaseoducto
        {
            get { return base.GetSqlXml("ListGaseoducto"); }
        }

        public string SqlGetByCriteria2
        {
            get { return base.GetSqlXml("GetByCriteria2"); }
        }

        public string SqlListPrGrupoForCN2
        {
            get { return base.GetSqlXml("ListPrGrupoForCN2"); }
        }

        public string SqlListPrGrupodatByCriteria
        {
            get { return base.GetSqlXml("ListPrGrupodatByCriteria"); }
        }

        public string SqlListPrGrupoEquipoValByCriteria
        {
            get { return base.GetSqlXml("ListPrGrupoEquipoValByCriteria"); }
        }
        #endregion

    }
}
