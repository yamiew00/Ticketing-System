using Microsoft.AspNetCore.Mvc;
using TicketManager.Controllers.ControllerBases;
using TicketManager.Services;
using TicketManager.Services.Delete;

namespace TicketManager.Controllers
{
    public class DeleteController: NoAuthControllerBase
    {
        private readonly DeleteService _deleteService;

        public DeleteController(DeleteService deleteService)
        {
            this._deleteService = deleteService;
        }

        /// <summary>
        /// 清空所有用戶、event、交易等等資料。回歸至初始狀態。
        /// </summary>
        /// <returns></returns>
        [HttpDelete("All")]
        public async Task<ResponseBase> All()
        {
            await _deleteService.DeleteAll();
            return new ResponseBase();
        }
    }
}
