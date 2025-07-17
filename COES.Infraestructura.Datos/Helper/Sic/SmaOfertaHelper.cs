using System;
using System.Collections.Generic;
using System.Data;
using COES.Dominio.DTO.Sic;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Helper.Sic
{
    /// <summary>
    /// Clase que contiene el mapeo de la tabla SMA_OFERTA
    /// </summary>
    public class SmaOfertaHelper : HelperBase
    {
        public SmaOfertaHelper(): base(Consultas.SmaOfertaSql)
        {
        }


        public SmaOfertaDTO Create(IDataReader dr)
        {
            SmaOfertaDTO entity = new SmaOfertaDTO();

            int iOfertipo = dr.GetOrdinal(this.Ofertipo);
            if (!dr.IsDBNull(iOfertipo)) entity.Ofertipo = Convert.ToInt32(dr.GetValue(iOfertipo));

            int iOferfechainicio = dr.GetOrdinal(this.Oferfechainicio);
            if (!dr.IsDBNull(iOferfechainicio)) entity.Oferfechainicio = dr.GetDateTime(iOferfechainicio);

            int iOferfechafin = dr.GetOrdinal(this.Oferfechafin);
            if (!dr.IsDBNull(iOferfechafin)) entity.Oferfechafin = dr.GetDateTime(iOferfechafin);

            int iOfercodenvio = dr.GetOrdinal(this.Ofercodenvio);
            if (!dr.IsDBNull(iOfercodenvio)) entity.Ofercodenvio = dr.GetString(iOfercodenvio);

            int iOferestado = dr.GetOrdinal(this.Oferestado);
            if (!dr.IsDBNull(iOferestado)) entity.Oferestado = dr.GetString(iOferestado);

            int iOferusucreacion = dr.GetOrdinal(this.Oferusucreacion);
            if (!dr.IsDBNull(iOferusucreacion)) entity.Oferusucreacion = dr.GetString(iOferusucreacion);

            int iOferfeccreacion = dr.GetOrdinal(this.Oferfeccreacion);
            if (!dr.IsDBNull(iOferfeccreacion)) entity.Oferfeccreacion = dr.GetDateTime(iOferfeccreacion);

            int iOferusumodificacion = dr.GetOrdinal(this.Oferusumodificacion);
            if (!dr.IsDBNull(iOferusumodificacion)) entity.Oferusumodificacion = dr.GetString(iOferusumodificacion);

            int iOferfecmodificacion = dr.GetOrdinal(this.Oferfecmodificacion);
            if (!dr.IsDBNull(iOferfecmodificacion)) entity.Oferfecmodificacion = dr.GetDateTime(iOferfecmodificacion);

            int iOferfechaenvio = dr.GetOrdinal(this.Oferfechaenvio);
            if (!dr.IsDBNull(iOferfechaenvio)) entity.Oferfechaenvio = dr.GetDateTime(iOferfechaenvio);

            int iOfercodi = dr.GetOrdinal(this.Ofercodi);
            if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iOferfuente = dr.GetOrdinal(this.Oferfuente);
            if (!dr.IsDBNull(iOferfuente)) entity.Oferfuente = dr.GetString(iOferfuente);

            int iSmapaccodi = dr.GetOrdinal(this.Smapaccodi);
            if (!dr.IsDBNull(iSmapaccodi)) entity.Smapaccodi = Convert.ToInt32(dr.GetValue(iSmapaccodi));

            return entity;
        }

        public SmaOfertaDTO CreateList(IDataReader dr)
        {
            SmaOfertaDTO entity = new SmaOfertaDTO();

            int iOfertipo = dr.GetOrdinal(this.Ofertipo);
            if (!dr.IsDBNull(iOfertipo)) entity.Ofertipo = Convert.ToInt32(dr.GetValue(iOfertipo));

            int iOferfechainicio = dr.GetOrdinal(this.Oferfechainicio);
            if (!dr.IsDBNull(iOferfechainicio)) entity.Oferfechainicio = dr.GetDateTime(iOferfechainicio);

            int iOferfechafin = dr.GetOrdinal(this.Oferfechafin);
            if (!dr.IsDBNull(iOferfechafin)) entity.Oferfechafin = dr.GetDateTime(iOferfechafin);

            int iOfercodenvio = dr.GetOrdinal(this.Ofercodenvio);
            if (!dr.IsDBNull(iOfercodenvio)) entity.Ofercodenvio = dr.GetString(iOfercodenvio);

            int iOferestado = dr.GetOrdinal(this.Oferestado);
            if (!dr.IsDBNull(iOferestado)) entity.Oferestado = dr.GetString(iOferestado);

            int iOferusucreacion = dr.GetOrdinal(this.Oferusucreacion);
            if (!dr.IsDBNull(iOferusucreacion)) entity.Oferusucreacion = dr.GetString(iOferusucreacion);

            int iOferfeccreacion = dr.GetOrdinal(this.Oferfeccreacion);
            if (!dr.IsDBNull(iOferfeccreacion)) entity.Oferfeccreacion = dr.GetDateTime(iOferfeccreacion);

            int iOferusumodificacion = dr.GetOrdinal(this.Oferusumodificacion);
            if (!dr.IsDBNull(iOferusumodificacion)) entity.Oferusumodificacion = dr.GetString(iOferusumodificacion);

            int iOferfecmodificacion = dr.GetOrdinal(this.Oferfecmodificacion);
            if (!dr.IsDBNull(iOferfecmodificacion)) entity.Oferfecmodificacion = dr.GetDateTime(iOferfecmodificacion);

            int iOferfechaenvio = dr.GetOrdinal(this.Oferfechaenvio);
            if (!dr.IsDBNull(iOferfechaenvio)) entity.Oferfechaenvio = dr.GetDateTime(iOferfechaenvio);

            int iOfercodi = dr.GetOrdinal(this.Ofercodi);
            if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            int iOfdecodi = dr.GetOrdinal(this.Ofdecodi);
            if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

            int iOfdehorainicio = dr.GetOrdinal(this.Ofdehorainicio);
            if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

            int iOfdehorafin = dr.GetOrdinal(this.Ofdehorafin);
            if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

            int iBandaCalificada = dr.GetOrdinal(this.BandaCalificada);
            if (!dr.IsDBNull(iBandaCalificada)) entity.BandaCalificada = dr.GetDecimal(iBandaCalificada);

            int iBandaDisponible = dr.GetOrdinal(this.BandaDisponible);
            if (!dr.IsDBNull(iBandaDisponible)) entity.BandaDisponible = dr.GetDecimal(iBandaDisponible);

            int iRepopotofer = dr.GetOrdinal(this.Repopotofer);
            if (!dr.IsDBNull(iRepopotofer)) entity.Repopotofer = dr.GetDecimal(iRepopotofer);

            int iRepoprecio = dr.GetOrdinal(this.Repoprecio);
            if (!dr.IsDBNull(iRepoprecio)) entity.Repoprecio = dr.GetString(iRepoprecio);

            int iRepomoneda = dr.GetOrdinal(this.Repomoneda);
            if (!dr.IsDBNull(iRepomoneda)) entity.Repomoneda = dr.GetString(iRepomoneda);

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iGrupocodi = dr.GetOrdinal(this.Grupocodi);
            if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

            int iGruponomb = dr.GetOrdinal(this.Gruponomb);
            if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

            int iOfdetipo = dr.GetOrdinal(this.Ofdetipo);
            if (!dr.IsDBNull(iOfdetipo)) entity.Ofdetipo = Convert.ToInt32(dr.GetValue(iOfdetipo));

            return entity;
        }

        public SmaOfertaDTO CreateInterna(IDataReader dr)
        {
            SmaOfertaDTO entity = new SmaOfertaDTO();

            int iOfertipo = dr.GetOrdinal(this.Ofertipo);
            if (!dr.IsDBNull(iOfertipo)) entity.Ofertipo = Convert.ToInt32(dr.GetValue(iOfertipo));

            int iOferfechainicio = dr.GetOrdinal(this.Oferfechainicio);
            if (!dr.IsDBNull(iOferfechainicio)) entity.Oferfechainicio = dr.GetDateTime(iOferfechainicio);

            int iOferfechafin = dr.GetOrdinal(this.Oferfechafin);
            if (!dr.IsDBNull(iOferfechafin)) entity.Oferfechafin = dr.GetDateTime(iOferfechafin);

            int iOfercodenvio = dr.GetOrdinal(this.Ofercodenvio);
            if (!dr.IsDBNull(iOfercodenvio)) entity.Ofercodenvio = dr.GetString(iOfercodenvio);

            int iOferestado = dr.GetOrdinal(this.Oferestado);
            if (!dr.IsDBNull(iOferestado)) entity.Oferestado = dr.GetString(iOferestado);

            int iOferusucreacion = dr.GetOrdinal(this.Oferusucreacion);
            if (!dr.IsDBNull(iOferusucreacion)) entity.Oferusucreacion = dr.GetString(iOferusucreacion);

            int iOferfeccreacion = dr.GetOrdinal(this.Oferfeccreacion);
            if (!dr.IsDBNull(iOferfeccreacion)) entity.Oferfeccreacion = dr.GetDateTime(iOferfeccreacion);

            int iOferusumodificacion = dr.GetOrdinal(this.Oferusumodificacion);
            if (!dr.IsDBNull(iOferusumodificacion)) entity.Oferusumodificacion = dr.GetString(iOferusumodificacion);

            int iOferfecmodificacion = dr.GetOrdinal(this.Oferfecmodificacion);
            if (!dr.IsDBNull(iOferfecmodificacion)) entity.Oferfecmodificacion = dr.GetDateTime(iOferfecmodificacion);

            int iOferfechaenvio = dr.GetOrdinal(this.Oferfechaenvio);
            if (!dr.IsDBNull(iOferfechaenvio)) entity.Oferfechaenvio = dr.GetDateTime(iOferfechaenvio);

            int iOfercodi = dr.GetOrdinal(this.Ofercodi);
            if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iOferfuente = dr.GetOrdinal(this.Oferfuente);
            if (!dr.IsDBNull(iOferfuente)) entity.Oferfuente = dr.GetString(iOferfuente);

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            int iOfdecodi = dr.GetOrdinal(this.Ofdecodi);
            if (!dr.IsDBNull(iOfdecodi)) entity.Ofdecodi = Convert.ToInt32(dr.GetValue(iOfdecodi));

            int iOfdehorainicio = dr.GetOrdinal(this.Ofdehorainicio);
            if (!dr.IsDBNull(iOfdehorainicio)) entity.Ofdehorainicio = dr.GetString(iOfdehorainicio);

            int iOfdehorafin = dr.GetOrdinal(this.Ofdehorafin);
            if (!dr.IsDBNull(iOfdehorafin)) entity.Ofdehorafin = dr.GetString(iOfdehorafin);

            int iOferlistMO = dr.GetOrdinal(this.OferlistMO);
            if (!dr.IsDBNull(iOferlistMO)) entity.OferlistMO = dr.GetString(iOferlistMO);

            int iOferlistMODes = dr.GetOrdinal(this.OferlistMODes);
            if (!dr.IsDBNull(iOferlistMODes)) entity.OferlistMODes = dr.GetString(iOferlistMODes);

            int iEmprnomb = dr.GetOrdinal(this.Emprnomb);
            if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

            int iEmprcodi = dr.GetOrdinal(this.Emprcodi);
            if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

            int iRepopotofer = dr.GetOrdinal(this.Repopotofer);
            if (!dr.IsDBNull(iRepopotofer)) entity.Repopotofer = dr.GetDecimal(iRepopotofer);

            int iRepoprecio = dr.GetOrdinal(this.Repoprecio);
            if (!dr.IsDBNull(iRepoprecio)) entity.Repoprecio = dr.GetString(iRepoprecio);

            int iRepomoneda = dr.GetOrdinal(this.Repomoneda);
            if (!dr.IsDBNull(iRepomoneda)) entity.Repomoneda = dr.GetString(iRepomoneda);

            int iUrscodi = dr.GetOrdinal(this.Urscodi);
            if (!dr.IsDBNull(iUrscodi)) entity.Urscodi = Convert.ToInt32(dr.GetValue(iUrscodi));

            int iUrsnomb = dr.GetOrdinal(this.Ursnomb);
            if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

            int iOfdetipo = dr.GetOrdinal(this.Ofdetipo);
            if (!dr.IsDBNull(iOfdetipo)) entity.Ofdetipo = Convert.ToInt32(dr.GetValue(iOfdetipo));

            int iBandaCalificada = dr.GetOrdinal(this.BandaCalificada);
            if (!dr.IsDBNull(iBandaCalificada)) entity.BandaCalificada = dr.GetDecimal(iBandaCalificada);

            int iBandaDisponible = dr.GetOrdinal(this.BandaDisponible);
            if (!dr.IsDBNull(iBandaDisponible)) entity.BandaDisponible = dr.GetDecimal(iBandaDisponible);

            return entity;
        }

        public SmaOfertaDTO CreateOfertaDia(IDataReader dr)
        {
            SmaOfertaDTO entity = new SmaOfertaDTO();

            int iOfertipo = dr.GetOrdinal(this.Ofertipo);
            if (!dr.IsDBNull(iOfertipo)) entity.Ofertipo = Convert.ToInt32(dr.GetValue(iOfertipo));

            int iOferfechainicio = dr.GetOrdinal(this.Oferfechainicio);
            if (!dr.IsDBNull(iOferfechainicio)) entity.Oferfechainicio = dr.GetDateTime(iOferfechainicio);

            int iOferfechafin = dr.GetOrdinal(this.Oferfechafin);
            if (!dr.IsDBNull(iOferfechafin)) entity.Oferfechafin = dr.GetDateTime(iOferfechafin);

            int iOfercodenvio = dr.GetOrdinal(this.Ofercodenvio);
            if (!dr.IsDBNull(iOfercodenvio)) entity.Ofercodenvio = dr.GetString(iOfercodenvio);

            int iOferestado = dr.GetOrdinal(this.Oferestado);
            if (!dr.IsDBNull(iOferestado)) entity.Oferestado = dr.GetString(iOferestado);

            int iOferfecmodificacion = dr.GetOrdinal(this.Oferfecmodificacion);
            if (!dr.IsDBNull(iOferfecmodificacion)) entity.Oferfecmodificacion = dr.GetDateTime(iOferfecmodificacion);

            int iOferfechaenvio = dr.GetOrdinal(this.Oferfechaenvio);
            if (!dr.IsDBNull(iOferfechaenvio)) entity.Oferfechaenvio = dr.GetDateTime(iOferfechaenvio);

            int iOfercodi = dr.GetOrdinal(this.Ofercodi);
            if (!dr.IsDBNull(iOfercodi)) entity.Ofercodi = Convert.ToInt32(dr.GetValue(iOfercodi));

            int iUsercode = dr.GetOrdinal(this.Usercode);
            if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

            int iUsername = dr.GetOrdinal(this.Username);
            if (!dr.IsDBNull(iUsername)) entity.Username = dr.GetString(iUsername);

            return entity;
        }

        #region Mapeo de Campos

        public string Ofertipo = "OFERTIPO";
        public string Oferfechainicio = "OFERFECHAINICIO";
        public string Oferfechafin = "OFERFECHAFIN";
        public string Ofercodenvio = "OFERCODENVIO";
        public string Oferestado = "OFERESTADO";
        public string Oferusucreacion = "OFERUSUCREACION";
        public string Oferfeccreacion = "OFERFECCREACION";
        public string Oferusumodificacion = "OFERUSUMODIFICACION";
        public string Oferfecmodificacion = "OFERFECMODIFICACION";
        public string Oferfechaenvio = "OFERFECHAENVIO";
        public string Ofercodi = "OFERCODI";
        public string Usercode = "USERCODE";
        public string Oferfuente = "OFERFUENTE";
        public string Username = "USERNAME";

        public string  Ofdecodi = "OFDECODI";
        public string  Ofdehorainicio = "OFDEHORAINICIO";
        public string  Ofdehorafin = "OFDEHORAFIN";
        public string  Grupocodi = "GRUPOCODI";
        public string OferlistMO = "OFERLISTMO";
        public string OferlistMODes = "OFERLISTMODES";

        /*REPORTE*/
        public string   Gruponomb = "GRUPONOMB";
        public string Grupoabrev = "GRUPOABREV";
        public string   Gruponombncp = "GRUPONOMBNCP";
        public string Grupocodincp = "GRUPOCODINCP";
        public string   Grupotipo = "GRUPOTIPO";
        public string   Repofecha = "REPOFECHA";
        public string   Repointvnum = "REPOINTVNUM";
        public string   Repointvhini = "REPOINTVHINI";
        public string   Repointvmini = "REPOINTVMINI";
        public string   Repointvhfin = "REPOINTVHFIN";
        public string   Repointvmfin = "REPOINTVMFIN";
        public string   Repohoraini = "REPOHORAINI";
        public string   Repohorafin = "REPOHORAFIN";
        public string   Urscodi = "URSCODI";
        public string   Urstipo = "URSTIPO";
        public string   Ursnomb = "URSNOMB";
        public string   Repopotmaxofer = "REPOPOTMAXOFER";
        public string   BandaCalificada = "BANDACALIFICADA";
        public string   BandaDisponible = "BANDADISPONIBLE";
        //Repopotofer
        public string   Repopotofer = "REPOPOTOFER";
        public string   Repoprecio = "REPOPRECIO";
        public string   Repomoneda = "REPOMONEDA";
        public string   Reponrounit = "REPONROUNIT";
        public string   Emprcodi = "EMPRCODI";
        public string   Emprnomb = "EMPRNOMB";

        public string Ofdetipo = "OFDETIPO";
        public string Smapaccodi = "SMAPACCODI";
        

        #endregion


        public string SqlUpdateOferDia
        {
            get { return base.GetSqlXml("UpdateOferDia"); }
        }

        public string SqlGetNumOferDia
        {
            get { return base.GetSqlXml("GetNumOferDia"); }
        }

        public string SqlListInterna
        {
            get { return base.GetSqlXml("ListInterna"); }
        }

        public string SqlListOfertas
        {
            get { return base.GetSqlXml("ListOfertas"); }
        }

        public string SqlListEntidadOfertaSimetricaHorario
        {
            get { return base.GetSqlXml("ListOfertaSimetricaHorario"); }
        }

        public string SqlEliminarParaCrearEntidadOfertaSimetricaHorario
        {
            get { return base.GetSqlXml("EliminarParaCrearEntidadOfertaSimetricaHorario"); }
        }

        public string SqlCrearEntidadOfertaSimetricaHorario
        {
            get { return base.GetSqlXml("CrearOfertaSimetricaHorario"); }
        }

        public string SqlRevertirEstadoEntidadOfertaSimetricaHorario
        {
            get { return base.GetSqlXml("RevertirEstadoEntidadOfertaSimetricaHorario"); }
        }

        public string SqlObtenerActivosEntidadOfertaSimetricaHorario
        {
            get { return base.GetSqlXml("EstaVigenteEntidadOfertaSimetricaHorario"); }
        }

        public string SqlResetearOfertaDefecto
        {
            get { return base.GetSqlXml("ResetearOfertaDefecto"); }
        }

        public SmaOfertaSimetricaHorarioDTO CrearEntidadOfertaSimetricaHorario(IDataReader dr)
        {
            SmaOfertaSimetricaHorarioDTO entity = new SmaOfertaSimetricaHorarioDTO();

            int id = dr.GetOrdinal("ID");
            if (!dr.IsDBNull(id)) entity.Id = Convert.ToInt32(dr.GetValue(id));

            int horarioInicio = dr.GetOrdinal("HORARIO_INICIO");
            if (!dr.IsDBNull(horarioInicio)) entity.HorarioInicio = dr.GetDateTime(horarioInicio);

            int horarioFin = dr.GetOrdinal("HORARIO_FIN");
            if (!dr.IsDBNull(horarioInicio)) entity.HorarioFin = dr.GetDateTime(horarioFin);

            int estado = dr.GetOrdinal("ESTADO");
            if (!dr.IsDBNull(estado)) entity.Estado = Convert.ToInt32(dr.GetValue(estado)) == 1 ? true : false;

            /*
                int iOferfechaenvio = dr.GetOrdinal(this.Oferfechaenvio);
                if (!dr.IsDBNull(iOferfechaenvio)) entity.Oferfechaenvio = dr.GetDateTime(iOferfechaenvio);
            */

            return entity;
        }

    }

}
