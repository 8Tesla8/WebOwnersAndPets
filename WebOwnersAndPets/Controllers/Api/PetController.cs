using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Domain.DTO;
using Domain.ErrorsWriter;
using Domain.Repository;
using Domain.Status;

namespace WebOwnersAndPets.Controllers.Api
{
    public class PetController : ApiController
    {
        IPetRepository _repository;

        public PetController()
        {
            string dbPAth = HttpContext.Current.Server.MapPath("~/App_Data/database.db");
            _repository = new PetRepository(dbPAth);
        }

        // GET api/<controller>
        //public IHttpActionResult Get()
        //{
        //    try
        //    {
        //        var owners = _repository.GetList();
        //        if (_repository.Status == StatusRequest.Ok)
        //        {
        //            return Json(owners);
        //        }
        //        return BadRequest(ErrorsWriter.GetErrors(_repository.ErrorMessage));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // GET api/<controller>/5
        public IHttpActionResult Get(string owner)
        {
            try
            {
                int ownerId = int.Parse(owner);

                var owners = _repository.GetOwnerPetsList(ownerId);

                if (_repository.Status == StatusRequest.Ok)
                {
                    return Json(owners);
                }


                return BadRequest(ErrorsWriter.GetErrors(_repository.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]Pet dto)
        {
            try
            {
                _repository.Create(dto);

                if (_repository.Status == StatusRequest.Created)
                {
                    return Ok();
                }


                return BadRequest(ErrorsWriter.GetErrors(_repository.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<controller>
        public IHttpActionResult Put([FromBody]Pet dto)
        {
            try
            {
                _repository.Update(dto);

                if (_repository.Status == StatusRequest.Updated)
                {
                    return Ok();
                }


                return BadRequest(ErrorsWriter.GetErrors(_repository.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);

                if (_repository.Status == StatusRequest.Deleted)
                {
                    return Ok();
                }


                return BadRequest(ErrorsWriter.GetErrors(_repository.ErrorMessage));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}