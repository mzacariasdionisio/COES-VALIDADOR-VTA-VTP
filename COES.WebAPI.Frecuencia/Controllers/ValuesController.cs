﻿using COES.Dominio.DTO.Sic;
using COES.Servicios.Aplicacion.Mediciones;
using COES.WebAPI.Frecuencia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace COES.WebAPI.Frecuencia.Controllers
{
    public class FrecuenciaController : ApiController
    {
        LecturaAppServicio service = new LecturaAppServicio();
        // GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public IEnumerable<FLecturaDTO> Get()//(DateTime fecha) //List<FrecuenciaGpsDTO>
        {
            DateTime fecha = DateTime.Today;
            List<FLecturaDTO> dataLectura = service.GetByCriteriaFLecturas(1, fecha, fecha);
            return dataLectura;
            //return new Flectura() { };
        }

        public IEnumerable<FLecturaDTO> Get(DateTime fecha) //List<FrecuenciaGpsDTO>
        {            
            List<FLecturaDTO> dataLectura = service.GetByCriteriaFLecturas(1, fecha, fecha);
            return dataLectura;         
        }
    }
}
