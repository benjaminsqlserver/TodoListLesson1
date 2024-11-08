using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ToDoList.Server.Controllers.ConData
{
    [Route("odata/ConData/Statuses")]
    public partial class StatusesController : ODataController
    {
        private ToDoList.Server.Data.ConDataContext context;

        public StatusesController(ToDoList.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ToDoList.Server.Models.ConData.Status> GetStatuses()
        {
            var items = this.context.Statuses.AsQueryable<ToDoList.Server.Models.ConData.Status>();
            this.OnStatusesRead(ref items);

            return items;
        }

        partial void OnStatusesRead(ref IQueryable<ToDoList.Server.Models.ConData.Status> items);

        partial void OnStatusGet(ref SingleResult<ToDoList.Server.Models.ConData.Status> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/Statuses(StatusID={StatusID})")]
        public SingleResult<ToDoList.Server.Models.ConData.Status> GetStatus(int key)
        {
            var items = this.context.Statuses.Where(i => i.StatusID == key);
            var result = SingleResult.Create(items);

            OnStatusGet(ref result);

            return result;
        }
        partial void OnStatusDeleted(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusDeleted(ToDoList.Server.Models.ConData.Status item);

        [HttpDelete("/odata/ConData/Statuses(StatusID={StatusID})")]
        public IActionResult DeleteStatus(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .Include(i => i.ToDoLists)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.Status>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStatusDeleted(item);
                this.context.Statuses.Remove(item);
                this.context.SaveChanges();
                this.OnAfterStatusDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusUpdated(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusUpdated(ToDoList.Server.Models.ConData.Status item);

        [HttpPut("/odata/ConData/Statuses(StatusID={StatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutStatus(int key, [FromBody]ToDoList.Server.Models.ConData.Status item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.Status>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == key);
                
                this.OnAfterStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/Statuses(StatusID={StatusID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchStatus(int key, [FromBody]Delta<ToDoList.Server.Models.ConData.Status> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Statuses
                    .Where(i => i.StatusID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.Status>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnStatusUpdated(item);
                this.context.Statuses.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == key);
                
                this.OnAfterStatusUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnStatusCreated(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusCreated(ToDoList.Server.Models.ConData.Status item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ToDoList.Server.Models.ConData.Status item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnStatusCreated(item);
                this.context.Statuses.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Statuses.Where(i => i.StatusID == item.StatusID);

                

                this.OnAfterStatusCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
