using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using ToDoList.Server.Data;

namespace ToDoList.Server
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportPriorityLevelsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/prioritylevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/prioritylevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPriorityLevelsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/prioritylevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/prioritylevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPriorityLevelsRead(ref IQueryable<ToDoList.Server.Models.ConData.PriorityLevel> items);

        public async Task<IQueryable<ToDoList.Server.Models.ConData.PriorityLevel>> GetPriorityLevels(Query query = null)
        {
            var items = Context.PriorityLevels.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnPriorityLevelsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPriorityLevelGet(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnGetPriorityLevelByPriorityId(ref IQueryable<ToDoList.Server.Models.ConData.PriorityLevel> items);


        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> GetPriorityLevelByPriorityId(int priorityid)
        {
            var items = Context.PriorityLevels
                              .AsNoTracking()
                              .Where(i => i.PriorityID == priorityid);

 
            OnGetPriorityLevelByPriorityId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnPriorityLevelGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPriorityLevelCreated(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelCreated(ToDoList.Server.Models.ConData.PriorityLevel item);

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> CreatePriorityLevel(ToDoList.Server.Models.ConData.PriorityLevel prioritylevel)
        {
            OnPriorityLevelCreated(prioritylevel);

            var existingItem = Context.PriorityLevels
                              .Where(i => i.PriorityID == prioritylevel.PriorityID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.PriorityLevels.Add(prioritylevel);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(prioritylevel).State = EntityState.Detached;
                throw;
            }

            OnAfterPriorityLevelCreated(prioritylevel);

            return prioritylevel;
        }

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> CancelPriorityLevelChanges(ToDoList.Server.Models.ConData.PriorityLevel item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPriorityLevelUpdated(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelUpdated(ToDoList.Server.Models.ConData.PriorityLevel item);

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> UpdatePriorityLevel(int priorityid, ToDoList.Server.Models.ConData.PriorityLevel prioritylevel)
        {
            OnPriorityLevelUpdated(prioritylevel);

            var itemToUpdate = Context.PriorityLevels
                              .Where(i => i.PriorityID == prioritylevel.PriorityID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(prioritylevel);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPriorityLevelUpdated(prioritylevel);

            return prioritylevel;
        }

        partial void OnPriorityLevelDeleted(ToDoList.Server.Models.ConData.PriorityLevel item);
        partial void OnAfterPriorityLevelDeleted(ToDoList.Server.Models.ConData.PriorityLevel item);

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> DeletePriorityLevel(int priorityid)
        {
            var itemToDelete = Context.PriorityLevels
                              .Where(i => i.PriorityID == priorityid)
                              .Include(i => i.ToDoLists)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPriorityLevelDeleted(itemToDelete);


            Context.PriorityLevels.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPriorityLevelDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStatusesRead(ref IQueryable<ToDoList.Server.Models.ConData.Status> items);

        public async Task<IQueryable<ToDoList.Server.Models.ConData.Status>> GetStatuses(Query query = null)
        {
            var items = Context.Statuses.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStatusGet(ToDoList.Server.Models.ConData.Status item);
        partial void OnGetStatusByStatusId(ref IQueryable<ToDoList.Server.Models.ConData.Status> items);


        public async Task<ToDoList.Server.Models.ConData.Status> GetStatusByStatusId(int statusid)
        {
            var items = Context.Statuses
                              .AsNoTracking()
                              .Where(i => i.StatusID == statusid);

 
            OnGetStatusByStatusId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnStatusCreated(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusCreated(ToDoList.Server.Models.ConData.Status item);

        public async Task<ToDoList.Server.Models.ConData.Status> CreateStatus(ToDoList.Server.Models.ConData.Status status)
        {
            OnStatusCreated(status);

            var existingItem = Context.Statuses
                              .Where(i => i.StatusID == status.StatusID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Statuses.Add(status);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(status).State = EntityState.Detached;
                throw;
            }

            OnAfterStatusCreated(status);

            return status;
        }

        public async Task<ToDoList.Server.Models.ConData.Status> CancelStatusChanges(ToDoList.Server.Models.ConData.Status item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnStatusUpdated(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusUpdated(ToDoList.Server.Models.ConData.Status item);

        public async Task<ToDoList.Server.Models.ConData.Status> UpdateStatus(int statusid, ToDoList.Server.Models.ConData.Status status)
        {
            OnStatusUpdated(status);

            var itemToUpdate = Context.Statuses
                              .Where(i => i.StatusID == status.StatusID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(status);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterStatusUpdated(status);

            return status;
        }

        partial void OnStatusDeleted(ToDoList.Server.Models.ConData.Status item);
        partial void OnAfterStatusDeleted(ToDoList.Server.Models.ConData.Status item);

        public async Task<ToDoList.Server.Models.ConData.Status> DeleteStatus(int statusid)
        {
            var itemToDelete = Context.Statuses
                              .Where(i => i.StatusID == statusid)
                              .Include(i => i.ToDoLists)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStatusDeleted(itemToDelete);


            Context.Statuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStatusDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportToDoListsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/todolists/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/todolists/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportToDoListsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/todolists/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/todolists/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnToDoListsRead(ref IQueryable<ToDoList.Server.Models.ConData.ToDoList> items);

        public async Task<IQueryable<ToDoList.Server.Models.ConData.ToDoList>> GetToDoLists(Query query = null)
        {
            var items = Context.ToDoLists.AsQueryable();

            items = items.Include(i => i.PriorityLevel);
            items = items.Include(i => i.Status);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnToDoListsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnToDoListGet(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnGetToDoListByTaskId(ref IQueryable<ToDoList.Server.Models.ConData.ToDoList> items);


        public async Task<ToDoList.Server.Models.ConData.ToDoList> GetToDoListByTaskId(int taskid)
        {
            var items = Context.ToDoLists
                              .AsNoTracking()
                              .Where(i => i.TaskID == taskid);

            items = items.Include(i => i.PriorityLevel);
            items = items.Include(i => i.Status);
 
            OnGetToDoListByTaskId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnToDoListGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnToDoListCreated(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListCreated(ToDoList.Server.Models.ConData.ToDoList item);

        public async Task<ToDoList.Server.Models.ConData.ToDoList> CreateToDoList(ToDoList.Server.Models.ConData.ToDoList todolist)
        {
            OnToDoListCreated(todolist);

            var existingItem = Context.ToDoLists
                              .Where(i => i.TaskID == todolist.TaskID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.ToDoLists.Add(todolist);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(todolist).State = EntityState.Detached;
                throw;
            }

            OnAfterToDoListCreated(todolist);

            return todolist;
        }

        public async Task<ToDoList.Server.Models.ConData.ToDoList> CancelToDoListChanges(ToDoList.Server.Models.ConData.ToDoList item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnToDoListUpdated(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListUpdated(ToDoList.Server.Models.ConData.ToDoList item);

        public async Task<ToDoList.Server.Models.ConData.ToDoList> UpdateToDoList(int taskid, ToDoList.Server.Models.ConData.ToDoList todolist)
        {
            OnToDoListUpdated(todolist);

            var itemToUpdate = Context.ToDoLists
                              .Where(i => i.TaskID == todolist.TaskID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(todolist);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterToDoListUpdated(todolist);

            return todolist;
        }

        partial void OnToDoListDeleted(ToDoList.Server.Models.ConData.ToDoList item);
        partial void OnAfterToDoListDeleted(ToDoList.Server.Models.ConData.ToDoList item);

        public async Task<ToDoList.Server.Models.ConData.ToDoList> DeleteToDoList(int taskid)
        {
            var itemToDelete = Context.ToDoLists
                              .Where(i => i.TaskID == taskid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnToDoListDeleted(itemToDelete);


            Context.ToDoLists.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterToDoListDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}