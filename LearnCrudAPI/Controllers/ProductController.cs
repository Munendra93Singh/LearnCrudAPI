using LearnCrudAPI.Helper;
using LearnCrudAPI.Repos;
using LearnCrudAPI.Repos.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LearnCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly LearndataContext context;
        public ProductController(IWebHostEnvironment environment, LearndataContext context)
        {
            this.environment = environment;
            this.context = context;
        }
        [HttpPut("UploadImages")]
        public async Task<IActionResult> UploadImages(IFormFile formFile, string productcode)
        {
            APIResponse response = new APIResponse();
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }
                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                    response.ResponseCode = 200;
                    response.Result = "Upload Successfully !";
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return Ok(response);
        }
      

        [HttpPut("MultiUploadImages")]
        public async Task<IActionResult> MulltiUploadImages(IFormFileCollection Filecollection, string productcode)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {
                string Filepath = GetFilePath(productcode);
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }
                foreach (var file in Filecollection)
                {
                    string imagepath = Filepath + "\\" + file.FileName;
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagepath))
                    {
                        await file.CopyToAsync(stream);
                        passcount++;
                    }
                }

            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + "File upload &&" + errorcount + "files failed";
            return Ok(response);
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string productcode)
        {
            string Imageurl = string.Empty;
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = hosturl + "/Upload/product/" + productcode + "/" + productcode + ".png";
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }
        [HttpGet("GetMultiImage")]
        public async Task<IActionResult> GetMultiImage(string productcode)
        {
            List<string> Imageurl = new List<string>();
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                string Filepath = GetFilePath(productcode);

                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string filename = fileInfo.Name;
                        string imagepath = Filepath + "\\" + filename;
                        if (System.IO.File.Exists(imagepath))
                        {
                            string _Imageurl = hosturl + "/Upload/product/" + productcode + "/" + filename;
                            Imageurl.Add(_Imageurl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    MemoryStream stream = new MemoryStream();
                    using (FileStream fileStream = new FileStream(imagepath, FileMode.Open))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                    stream.Position = 0;
                    return File(stream, "image/png", productcode + "png");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("remove")]
        public async Task<IActionResult> remove(string productcode)
        {
            try
            {
                string Filepath = GetFilePath(productcode);
                string imagepath = Filepath + "\\" + productcode + ".png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                    return Ok("Delete Successfully !");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpGet("multiremove")]
        public async Task<IActionResult> MultiRemove(string productcode)
        {           
            try
            {
                string Filepath = GetFilePath(productcode);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();                   
                    }
                    return Ok("All Remove Succussfully !");
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
                return NotFound();
            }
           
        }

        [HttpPut("DBMultiUploadImages")]
        public async Task<IActionResult> DBMulltiUploadImages(IFormFileCollection Filecollection, string productcode)
        {
            APIResponse response = new APIResponse();
            int passcount = 0; int errorcount = 0;
            try
            {               
                foreach (var file in Filecollection)
                {
                    using(MemoryStream stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        this.context.TblProductimages.Add(new Repos.Models.TblProductimage()
                        {
                           Productcode = productcode,
                           Productimage =stream.ToArray()
                    });
                        await this.context.SaveChangesAsync();
                        passcount++;
                    }                    
                }

            }
            catch (Exception ex)
            {
                errorcount++;
                response.Message = ex.Message;
            }
            response.ResponseCode = 200;
            response.Result = passcount + "File uploaded &&" + errorcount + "files failed";
            return Ok(response);
        }

        [HttpGet("GetDBMultiImage")]
        public async Task<IActionResult> GetDBMultiImage(string productcode)
        {
            List<string> Imageurl = new List<string>();
            try
            {
                var _productimage = this.context.TblProductimages.Where(item => item.Productcode == productcode).ToList();
                if(_productimage != null && _productimage.Count > 0)
                {
                    _productimage.ForEach(item =>
                    {
                        Imageurl.Add(Convert.ToBase64String(item.Productimage));
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch(Exception ex)
            {
               
            }
            return Ok(Imageurl);
        }

        [HttpGet("dbdownload")]
        public async Task<IActionResult> dbdownload(string productcode)
        {

            try
            {

                var _productimage = await this.context.TblProductimages.FirstOrDefaultAsync(item => item.Productcode == productcode);
                if (_productimage != null)
                {
                    return File(_productimage.Productimage, "image/png", productcode + ".png");
                }               
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        
        [NonAction]
        private string GetFilePath(string productcode)
        {
            return this.environment.WebRootPath + "\\Upload\\product\\" + productcode;
        }
    }
}