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
    [Route("odata/ConData/ToDoLists")]
    public partial class ToDoListsController : ODataController
    {
        private ToDoList.Server.Data.ConDataContext context;

        public ToDoListsController(ToDoList.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ToDoList.Server.Models.ConData.ToDoList> GetToDoLists()
        {
            var items = this.context.ToDoLists.AsQueryable<ToDoList.Server.Models.ConData.ToDoList>();
            this.OnToDoListsRead(ref items);

            return items;
        }

        partial void OnToDoListsRead(ref IQueryable<ToDoList.Server.Models.ConData.ToDoList> items);

        partial void OnToDoListGet(ref SingleResult<ToDoList.Server.Models.ConData.ToDoList> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/ToDoLists(TaskID={TaskID})")]
        public SingleResult<ToDoList.Server.Models.ConData.ToDoList> GetToDoList(int key)
        {
            var items = this.context.ToDoLists.Where(i => i.TaskID == key);
            var result = SingleResult.Create(items);

            OnToDoListGet(ref result);

            return result;
        }
        partial void OnToDoListDeleted(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListDeleted(ToDoList.Server.Models.ConData.ToDoList item);

        [HttpDelete("/odata/ConData/ToDoLists(TaskID={TaskID})")]
        public IActionResult DeleteToDoList(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.ToDoLists
                    .Where(i => i.TaskID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.ToDoList>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnToDoListDeleted(item);
                this.context.ToDoLists.Remove(item);
                this.context.SaveChanges();
                this.OnAfterToDoListDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnToDoListUpdated(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListUpdated(ToDoList.Server.Models.ConData.ToDoList item);

        [HttpPut("/odata/ConData/ToDoLists(TaskID={TaskID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutToDoList(int key, [FromBody]ToDoList.Server.Models.ConData.ToDoList item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ToDoLists
                    .Where(i => i.TaskID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.ToDoList>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnToDoListUpdated(item);
                this.context.ToDoLists.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ToDoLists.Where(i => i.TaskID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "PriorityLevel,Status");
                this.OnAfterToDoListUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/ToDoLists(TaskID={TaskID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchToDoList(int key, [FromBody]Delta<ToDoList.Server.Models.ConData.ToDoList> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.ToDoLists
                    .Where(i => i.TaskID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.ToDoList>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnToDoListUpdated(item);
                this.context.ToDoLists.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ToDoLists.Where(i => i.TaskID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "PriorityLevel,Status");
                this.OnAfterToDoListUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnToDoListCreated(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListCreated(ToDoList.Server.Models.ConData.ToDoList item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ToDoList.Server.Models.ConData.ToDoList item)
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

                this.OnToDoListCreated(item);
                this.context.ToDoLists.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.ToDoLists.Where(i => i.TaskID == item.TaskID);

                Request.QueryString = Request.QueryString.Add("$expand", "PriorityLevel,Status");

                this.OnAfterToDoListCreated(item);

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
