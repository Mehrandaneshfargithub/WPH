using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WPH.Models.Hospital;
using WPH.MvcMockingServices;

namespace WPH.Controllers.Cost
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDIUnit _IDUNIT;

        public ValuesController(IDIUnit dIUnit)
        {
            _IDUNIT = dIUnit;
        }

        [HttpPost]
        public ActionResult AddOrUpdate(HospitalViewModel id)
        {
            try
            {
                //HospitalViewModel Hospital = new HospitalViewModel()
                //{
                //    Name = "jadid"
                //};
                

                    //Disease.Guid = Guid.NewGuid();
                    Guid hospitalid = _IDUNIT.hospital.AddNewHospital(id);
                    return Ok(JsonSerializer.Serialize(44));
                
            }
            catch (Exception ex) { throw ex; }
            
        }
    }
}
