
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace ToDoList.Client
{
    public partial class ConDataService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ConDataService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ConData/");
        }


        public async System.Threading.Tasks.Task ExportPriorityLevelsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/prioritylevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/prioritylevels/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPriorityLevelsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/prioritylevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/prioritylevels/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPriorityLevels(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.PriorityLevel>> GetPriorityLevels(Query query)
        {
            return await GetPriorityLevels(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.PriorityLevel>> GetPriorityLevels(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"PriorityLevels");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPriorityLevels(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.PriorityLevel>>(response);
        }

        partial void OnCreatePriorityLevel(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> CreatePriorityLevel(ToDoList.Server.Models.ConData.PriorityLevel priorityLevel = default(ToDoList.Server.Models.ConData.PriorityLevel))
        {
            var uri = new Uri(baseUri, $"PriorityLevels");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(priorityLevel), Encoding.UTF8, "application/json");

            OnCreatePriorityLevel(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.PriorityLevel>(response);
        }

        partial void OnDeletePriorityLevel(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePriorityLevel(int priorityId = default(int))
        {
            var uri = new Uri(baseUri, $"PriorityLevels({priorityId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePriorityLevel(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPriorityLevelByPriorityId(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.PriorityLevel> GetPriorityLevelByPriorityId(string expand = default(string), int priorityId = default(int))
        {
            var uri = new Uri(baseUri, $"PriorityLevels({priorityId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPriorityLevelByPriorityId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.PriorityLevel>(response);
        }

        partial void OnUpdatePriorityLevel(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePriorityLevel(int priorityId = default(int), ToDoList.Server.Models.ConData.PriorityLevel priorityLevel = default(ToDoList.Server.Models.ConData.PriorityLevel))
        {
            var uri = new Uri(baseUri, $"PriorityLevels({priorityId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", priorityLevel.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(priorityLevel), Encoding.UTF8, "application/json");

            OnUpdatePriorityLevel(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetStatuses(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.Status>> GetStatuses(Query query)
        {
            return await GetStatuses(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.Status>> GetStatuses(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Statuses");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStatuses(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.Status>>(response);
        }

        partial void OnCreateStatus(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.Status> CreateStatus(ToDoList.Server.Models.ConData.Status status = default(ToDoList.Server.Models.ConData.Status))
        {
            var uri = new Uri(baseUri, $"Statuses");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(status), Encoding.UTF8, "application/json");

            OnCreateStatus(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.Status>(response);
        }

        partial void OnDeleteStatus(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteStatus(int statusId = default(int))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetStatusByStatusId(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.Status> GetStatusByStatusId(string expand = default(string), int statusId = default(int))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetStatusByStatusId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.Status>(response);
        }

        partial void OnUpdateStatus(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateStatus(int statusId = default(int), ToDoList.Server.Models.ConData.Status status = default(ToDoList.Server.Models.ConData.Status))
        {
            var uri = new Uri(baseUri, $"Statuses({statusId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", status.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(status), Encoding.UTF8, "application/json");

            OnUpdateStatus(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportToDoListsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/todolists/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/todolists/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportToDoListsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/todolists/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/todolists/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetToDoLists(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.ToDoList>> GetToDoLists(Query query)
        {
            return await GetToDoLists(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.ToDoList>> GetToDoLists(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"ToDoLists");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetToDoLists(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<ToDoList.Server.Models.ConData.ToDoList>>(response);
        }

        partial void OnCreateToDoList(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.ToDoList> CreateToDoList(ToDoList.Server.Models.ConData.ToDoList toDoList = default(ToDoList.Server.Models.ConData.ToDoList))
        {
            var uri = new Uri(baseUri, $"ToDoLists");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(toDoList), Encoding.UTF8, "application/json");

            OnCreateToDoList(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.ToDoList>(response);
        }

        partial void OnDeleteToDoList(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteToDoList(int taskId = default(int))
        {
            var uri = new Uri(baseUri, $"ToDoLists({taskId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteToDoList(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetToDoListByTaskId(HttpRequestMessage requestMessage);

        public async Task<ToDoList.Server.Models.ConData.ToDoList> GetToDoListByTaskId(string expand = default(string), int taskId = default(int))
        {
            var uri = new Uri(baseUri, $"ToDoLists({taskId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetToDoListByTaskId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<ToDoList.Server.Models.ConData.ToDoList>(response);
        }

        partial void OnUpdateToDoList(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateToDoList(int taskId = default(int), ToDoList.Server.Models.ConData.ToDoList toDoList = default(ToDoList.Server.Models.ConData.ToDoList))
        {
            var uri = new Uri(baseUri, $"ToDoLists({taskId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", toDoList.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(toDoList), Encoding.UTF8, "application/json");

            OnUpdateToDoList(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}