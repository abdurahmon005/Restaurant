//using Microsoft.AspNetCore.Mvc;
//using WebApp.Aplication.Models.Tables;
//using WebApp.Aplication.Services.Interface;

//namespace RestaurantProject.API.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class TableController : ControllerBase
//    {
//        private readonly ITableService _tableService;

//        public TableController(ITableService tableService)
//        {
//            _tableService = tableService;
//        }
//        [HttpPost]
//        public IActionResult PostTable(TableCreateModel model)
//        {
//            var table = _tableService.CreateTableAsync(model);

//            return Ok("Table Created");
//        }
       
//    }
//}
