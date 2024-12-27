using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.WebApi.Controllers
{

    /// <summary>
    /// 控制器抽象
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Demo()
        {
            return "OK";
        }


        [HttpGet]
        [Authorize]
        public async Task<string> Demo1()
        {
            return "OK";
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Demo3()
        {
            return "OK";
        }
        

    }
}
