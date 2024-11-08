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
    [Route("odata/ConData/PriorityLevels")]
    public partial class PriorityLevelsController : ODataController
    {
        private ToDoList.Server.Data.ConDataContext context;

        public PriorityLevelsController(ToDoList.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<ToDoList.Server.Models.ConData.PriorityLevel> GetPriorityLevels()
        {
            var items = this.context.PriorityLevels.AsQueryable<ToDoList.Server.Models.ConData.PriorityLevel>();
            this.OnPriorityLevelsRead(ref items);

            return items;
        }

        partial void OnPriorityLevelsRead(ref IQueryable<ToDoList.Server.Models.ConData.PriorityLevel> items);

        partial void OnPriorityLevelGet(ref SingleResult<ToDoList.Server.Models.ConData.PriorityLevel> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/PriorityLevels(PriorityID={PriorityID})")]
        public SingleResult<ToDoList.Server.Models.ConData.PriorityLevel> GetPriorityLevel(int key)
        {
            var items = this.context.PriorityLevels.Where(i => i.PriorityID == key);
            var result = SingleResult.Create(items);

            OnPriorityLevelGet(ref result);

            return result;
        }
        partial void OnPriorityLevelDeleted(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelDeleted(ToDoList.Server.Models.ConData.PriorityLevel item);

        [HttpDelete("/odata/ConData/PriorityLevels(PriorityID={PriorityID})")]
        public IActionResult DeletePriorityLevel(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.PriorityLevels
                    .Where(i => i.PriorityID == key)
                    .Include(i => i.ToDoLists)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.PriorityLevel>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPriorityLevelDeleted(item);
                this.context.PriorityLevels.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPriorityLevelDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPriorityLevelUpdated(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelUpdated(ToDoList.Server.Models.ConData.PriorityLevel item);

        [HttpPut("/odata/ConData/PriorityLevels(PriorityID={PriorityID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPriorityLevel(int key, [FromBody]ToDoList.Server.Models.ConData.PriorityLevel item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.PriorityLevels
                    .Where(i => i.PriorityID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.PriorityLevel>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPriorityLevelUpdated(item);
                this.context.PriorityLevels.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PriorityLevels.Where(i => i.PriorityID == key);
                
                this.OnAfterPriorityLevelUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/PriorityLevels(PriorityID={PriorityID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPriorityLevel(int key, [FromBody]Delta<ToDoList.Server.Models.ConData.PriorityLevel> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.PriorityLevels
                    .Where(i => i.PriorityID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<ToDoList.Server.Models.ConData.PriorityLevel>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnPriorityLevelUpdated(item);
                this.context.PriorityLevels.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PriorityLevels.Where(i => i.PriorityID == key);
                
                this.OnAfterPriorityLevelUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPriorityLevelCreated(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelCreated(ToDoList.Server.Models.ConData.PriorityLevel item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] ToDoList.Server.Models.ConData.PriorityLevel item)
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

                this.OnPriorityLevelCreated(item);
                this.context.PriorityLevels.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.PriorityLevels.Where(i => i.PriorityID == item.PriorityID);

                

                this.OnAfterPriorityLevelCreated(item);

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
