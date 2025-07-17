// =======================================================================================
//
// (c) COES Sinac. Todos los derechos reservados. 
//
// Autor: Henry Manuel Díaz Tuesta
// Acronimo: HDT
// Requerimiento: compensaciones
//
// Fecha creacion: 29/03/2017
// Descripcion: Archivo para la atencion del requerimiento.
//
// Historial de cambios:
// 
// Correlativo	Fecha		Requerimiento		Comentario
//
// =======================================================================================

using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{

    public class VceArrparCompEspRepository : RepositoryBase, IVceArrparCompEspRepository
    {

        VceArrparCompEspHelper helper = new VceArrparCompEspHelper();

        public VceArrparCompEspRepository(string strConn)
            : base(strConn)
        {
        }

        public void Save(VceArrparCompEspDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //- Claves:
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apespfecha, DbType.DateTime, entity.Apespfecha);
            
            //- Otros:
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, entity.Apstocodi);
            dbProvider.AddInParameter(command, helper.Apespcargafinal, DbType.Decimal, entity.Apespcargafinal);
            dbProvider.AddInParameter(command, helper.Apespenergprod, DbType.Decimal, entity.Apespenergprod);
            dbProvider.AddInParameter(command, helper.Apesprendvigente, DbType.Decimal, entity.Apesprendvigente);
            dbProvider.AddInParameter(command, helper.Apesppreciocomb, DbType.Decimal, entity.Apesppreciocomb);
            dbProvider.AddInParameter(command, helper.Apespcombbase, DbType.Decimal, entity.Apespcombbase);
            dbProvider.AddInParameter(command, helper.Apespcombrampa, DbType.Decimal, entity.Apespcombrampa);
            dbProvider.AddInParameter(command, helper.Apespcombreconocxtransf, DbType.Decimal, entity.Apespcombreconocxtransf);
            dbProvider.AddInParameter(command, helper.Apesppreciocombalt, DbType.Decimal, entity.Apesppreciocombalt);
            dbProvider.AddInParameter(command, helper.Apespcombbasealt, DbType.Decimal, entity.Apespcombbasealt);
            dbProvider.AddInParameter(command, helper.Apespcombrampaalt, DbType.Decimal, entity.Apespcombrampaalt);
            dbProvider.AddInParameter(command, helper.Apespcombreconocxtransfalt, DbType.Decimal, entity.Apespcombreconocxtransfalt);
            dbProvider.AddInParameter(command, helper.Apespcompensacion, DbType.Decimal, entity.Apespcompensacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(VceArrparCompEspDTO entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(VceArrparCompEspDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, entity.PecaCodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Apespfechadesc, DbType.String, entity.Apespfechadesc);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, entity.Apstocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<VceArrparCompEspDTO> List()
        {
            throw new NotImplementedException();
        }

        public VceArrparCompEspDTO GetById(int periodo, int grupo, string fechaDesc, string apstocodi)

        {
            VceArrparCompEspDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Pecacodi, DbType.Int32, periodo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupo);
            dbProvider.AddInParameter(command, helper.Apespfechadesc, DbType.String, fechaDesc);
            dbProvider.AddInParameter(command, helper.Apstocodi, DbType.String, apstocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VceArrparCompEspDTO> GetByPeriod(int periodo)
        {
            List<VceArrparCompEspDTO> entities = new List<VceArrparCompEspDTO>();

            string queryString = string.Format(helper.SqlListByPeriod, periodo);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.Create(dr));
                }
            }

            return entities;
        }

    }

}
