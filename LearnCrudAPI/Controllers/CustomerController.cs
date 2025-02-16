using ClosedXML.Excel;
using LearnCrudAPI.Container;
using LearnCrudAPI.Model;
using LearnCrudAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Data;
using System.Data;

namespace LearnCrudAPI.Controllers
{
    [Authorize]
    //[DisableCors]
    [EnableRateLimiting("fixedwindow")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService service;
        private readonly IWebHostEnvironment environment;
        private readonly IEmailService emailService;

        public CustomerController(ICustomerService service, IWebHostEnvironment environment, IEmailService service1)
        {
            this.service = service;
            this.environment = environment;
            this.emailService = service1;
           
        }
        [AllowAnonymous]
        // [EnableCors("corsepolicy1")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await this.service.Getall();

            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        /// <summary>
        /// [DisableRateLimiting]
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("Getbycode")]
        public async Task<IActionResult> Getbycode(string code)
        {
            var data = await this.service.Getbycode(code);

            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(CustomerModal _data)
        {
            var data = await this.service.Create(_data);
            return Ok(data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CustomerModal _data, string code)
        {
            var data = await this.service.Update(_data, code);
            return Ok(data);
        }

        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(string code)
        {
            var data = await this.service.Remove(code);
            return Ok(data);
        }

        [AllowAnonymous]
        [HttpGet("ExportEXl")]
        public async Task<IActionResult> ExportEXl()
        {
            try
            {
                string Filepath = GetFilePath();
                string excelpath = Filepath + "\\ customerInfo.xlsx";
                DataTable dt = new DataTable();
                dt.Columns.Add("Code", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Email", typeof(string));
                dt.Columns.Add("Phone", typeof(string));
                dt.Columns.Add("CreditLimit", typeof(string));
                var data = await this.service.Getall();
                if (data != null && data.Count > 0)
                {
                    data.ForEach(item =>
                    {
                        dt.Rows.Add(item.Code, item.Name, item.Email, item.Phone, item.Creditlimit);
                    });
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.AddWorksheet(dt, "Customer Info");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        if (System.IO.File.Exists(excelpath))
                        {
                            System.IO.File.Delete(excelpath);
                        }
                        wb.SaveAs(excelpath);

                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customer.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [NonAction]
        private string GetFilePath()
        {
            return this.environment.WebRootPath + "\\Export";
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.Email = "Munendrasingh93mahi@gmail.com";
                mailrequest.Subject = "Welcome to munendra Singh";
                mailrequest.Emailbody = GetHtmlcontent();
                await emailService.SendEmail(mailrequest);
                return Ok();
            }
            catch(Exception ex)
            {
                throw;    
            }
         
        }
        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Nihira Techiees</h1>";
            Response += "<img src=\"https://yt3.googleusercontent.com/v5hyLB4am6E0GZ3y-JXVCxT9g8157eSeNggTZKkWRSfq_B12sCCiZmRhZ4JmRop-nMA18D2IPw=s176-c-k-c0x00ffffff-no-rj\" />";
            Response += "<h2>Thanks for subscribed us</h2>";
            Response += "<a href=\"https://www.youtube.com/channel/UCsbmVmB_or8sVLLEq4XhE_A/join\">Please join membership by click the link</a>";
            Response += "<div><h1> Contact us : nihiratechiees@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}

