using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using ToDoList.Server.Data;

namespace ToDoList.Server.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/prioritylevels/csv")]
        [HttpGet("/export/ConData/prioritylevels/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPriorityLevelsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPriorityLevels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/prioritylevels/excel")]
        [HttpGet("/export/ConData/prioritylevels/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPriorityLevelsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPriorityLevels(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/statuses/csv")]
        [HttpGet("/export/ConData/statuses/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/statuses/excel")]
        [HttpGet("/export/ConData/statuses/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetStatuses(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/todolists/csv")]
        [HttpGet("/export/ConData/todolists/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportToDoListsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetToDoLists(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/todolists/excel")]
        [HttpGet("/export/ConData/todolists/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportToDoListsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetToDoLists(), Request.Query, false), fileName);
        }
    }
}
