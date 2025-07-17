using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class SiTipoMensajeRepository : RepositoryBase, ISiTipoMensajeRepository
    {
        public SiTipoMensajeRepository(string strConn): base(strConn)
        {           
        }
        SiTipoMensajeHelper helper = new SiTipoMensajeHelper() ;

        public List<SiTipoMensajeDTO> Listar()
        {
            List<SiTipoMensajeDTO> entitys = new List<SiTipoMensajeDTO>();

            string query = string.Format(helper.SqlListar);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SiTipoMensajeDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiTipoMensajeDTO();
                    entity = helper.Create(dr);

                    int iTMsgCodi = dr.GetOrdinal(helper.TMsgCodi);
                    if (!dr.IsDBNull(iTMsgCodi)) entity.Tmsgcodi = Convert.ToInt32(dr.GetValue(iTMsgCodi));

                    int iTMsgNombre = dr.GetOrdinal(helper.TMsgNombre);
                    if (!dr.IsDBNull(iTMsgNombre)) entity.Tmsgnombre = dr.GetString(iTMsgNombre);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Siosein

        public List<SiTipoMensajeDTO> ListarXMod(int modcodi)
        {
            List<SiTipoMensajeDTO> entitys = new List<SiTipoMensajeDTO>();

            string query = string.Format(helper.SqlListarXMod, modcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            SiTipoMensajeDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new SiTipoMensajeDTO();
                    entity = helper.Create(dr);

                    int iTMsgCodi = dr.GetOrdinal(helper.TMsgCodi);
                    if (!dr.IsDBNull(iTMsgCodi)) entity.Tmsgcodi = Convert.ToInt32(dr.GetValue(iTMsgCodi));

                    int iTMsgNombre = dr.GetOrdinal(helper.TMsgNombre);
                    if (!dr.IsDBNull(iTMsgNombre)) entity.Tmsgnombre = dr.GetString(iTMsgNombre);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

    }
}
