#nullable enable
using D2LOffice.Models;
using D2LOffice.Models.D2L;
using D2LOffice.Service;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;


namespace D2LOffice.Service
{
    public partial class D2LService
    {

        private HttpClient httpClient = new HttpClient();
        private BehaviorSubject<bool> hasAccess = new BehaviorSubject<bool>(false);
        public IObservable<bool> HasAccess => hasAccess.AsObservable();

        private AuthToken? _token;

        private string host = "https://online.pcc.edu";
        private string baseUri = $"https://online.pcc.edu/d2l/api/";
        private string _version = "1.0";



        public D2LService()
        {
            Observable.FromAsync(async x =>
            {
                try
                {
                    var localSettings = Preferences.Default;
                    var serializedToken = localSettings.Get<string>("token", "");
                    if (serializedToken == "")
                    {
                        throw new Exception();
                    }
                    _token = System.Text.Json.JsonSerializer.Deserialize<AuthToken>(serializedToken);
                    if (_token == null)
                    {
                        throw new Exception("Token deserialized into null.");
                    }
                    DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    var dateTime = epoch + TimeSpan.FromSeconds(_token.ExpiresAt);
                    if (dateTime < DateTime.UtcNow)
                    {
                        //if (hasAccess.Value)
                            hasAccess.OnNext(false);
                    }
                    else
                    {
                        //if (!hasAccess.Value)
                            hasAccess.OnNext(true);
                    }
                }
                catch
                {
                    //if (hasAccess.Value)
                        hasAccess.OnNext(false);
                }
            }).Subscribe();
        }

        public void Logout()
        {
            var localSettings = Preferences.Default;
            localSettings.Set("token", "");
            _token = null;
            hasAccess.OnNext(false);
        }

        public async Task LoadTokenAsync(AuthToken token)
        {
            _token = token;
            if (token != null && !string.IsNullOrWhiteSpace(token.AccessToken))
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dateTime = epoch + TimeSpan.FromSeconds(_token.ExpiresAt);
                if (dateTime > DateTime.UtcNow)
                {
                    try
                    {
                        var serializedToken = System.Text.Json.JsonSerializer.Serialize(_token);
                        var localSettings = Preferences.Default;
                        localSettings.Set("token",serializedToken);
                        //if (!hasAccess.Value)
                        hasAccess.OnNext(true);
                    }
                    catch
                    {
                        //if (hasAccess.Value)
                        hasAccess.OnNext(false);
                    }


                }
            }
        }



        private void Timer_Tick(object sender, object e)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllCourseFilesAsync(string orgUnitId)
        {
            var courseFiles = new List<string>();

            await GetAllCourseFilesAsyncRecursive(orgUnitId, null, courseFiles);

            return courseFiles;

        }

        private async Task GetAllCourseFilesAsyncRecursive(string orgUnitId, string path, List<string> courseFiles)
        {
            var nextPath = "";
            if (path != null)
            {
                nextPath = path + "/";
            }
            var rootFiles = await GetCourseFilesAsync(orgUnitId, path);
            foreach (var file in rootFiles)
            {
                if (file.FileSystemObjectType == FileSystemObjectType.File)
                {
                    courseFiles.Add(nextPath + file.Name);
                }
                else
                {                    
                    await GetAllCourseFilesAsyncRecursive(orgUnitId, nextPath + file.Name, courseFiles);
                }
            }
        }

        public async Task<List<FileSystemObject>> GetCourseFilesAsync(string orgUnitId, string? path = null)
        {
            var route = $"{orgUnitId}/managefiles/";
            if (path != null)
            {
                route += "?path="+HttpUtility.UrlEncode(path);
            }
            var items = await GetObjectListAsync<FileSystemObject>(route, "lp", "1.46");
            return items;
        }

        public async Task<Stream> GetCourseFileStreamAsync(string orgUnitId, string path)
        {
            var route = $"{orgUnitId}/managefiles/file?path={HttpUtility.UrlEncode(path)}";
            var stream = await GetStreamAsync(route, "lp", "1.43");
            return stream;
        }


        public Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            var items = GetPagedItemsAsync<Enrollment>("enrollments/myenrollments/", "lp", "1.43");
            return items;
        }

        public Task<Enrollment?> GetEnrollmentDetailsAsync(int orgUnitId)
        {
            var item = GetAsync<Enrollment?>($"enrollments/myenrollments/{orgUnitId}", "lp", "1.43");
            return item;
        }


        public async Task<UploadedFile?> UploadFileStreamAsync(string orgUnitId, string path, string fileName, string mimeType, Stream stream, CancellationToken ctoken, IProgress<ProgressInfo>? progress=null, bool overwrite = false)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AllowAutoRedirect = false;
                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Length", stream.Length.ToString());
                httpClient.DefaultRequestHeaders.Add("X-Upload-File-Name", fileName);
                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Type", mimeType);

                var uploadResult = await httpClient.PostAsync(new Uri(baseUri + $"lp/1.44/{orgUnitId}/managefiles/file/upload"), null);
                if ((int)uploadResult.StatusCode == 308)
                {
                    var location = uploadResult.Headers.Location.ToString();
                    var id = location.Split('/').Last();

                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Type");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Length");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-File-Name");

                    var success = await UploadAsync(httpClient, host + location, stream, progress, ctoken);
                    if (success)
                    {
                        var form = new FormUrlEncodedContent(new Dictionary<string, string>() { { "fileKey", id }, { "relativePath", path } });
                        var saveResult = await httpClient.PostAsync(new Uri(baseUri + $"lp/1.44/{orgUnitId}/managefiles/file/save{(overwrite ? "?overwriteFile=true" : "")}"), form);
                        if (saveResult.IsSuccessStatusCode)
                        {
                            var uploadedFileStr = await saveResult.Content.ReadAsStringAsync();
                            var uploadedFile = System.Text.Json.JsonSerializer.Deserialize<UploadedFile>(uploadedFileStr);
                            return uploadedFile;
                        }
                        else
                            return default;
                    }
                    else
                        return default;

                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }


        public async Task<UploadedFile?> CreateFileFromStreamAsync(string orgUnitId, string path, string fileName, string mimeType, Stream stream, CancellationToken ctoken, IProgress<ProgressInfo>? progress)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AllowAutoRedirect = false;
                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Length", stream.Length.ToString());
                httpClient.DefaultRequestHeaders.Add("X-Upload-File-Name", fileName);
                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Type", mimeType);

                var uploadResult = await httpClient.PostAsync(new Uri(baseUri + $"lp/1.44/{orgUnitId}/managefiles/file/upload"), null);
                if ((int)uploadResult.StatusCode == 308)
                {
                    var location = uploadResult.Headers.Location.ToString();
                    var id = location.Split('/').Last();

                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Type");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Length");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-File-Name");

                    var success = await UploadAsync(httpClient, host+location, stream, progress, ctoken);
                    if (success)
                    {
                        var form = new FormUrlEncodedContent(new Dictionary<string, string>() { { "fileKey", id },{"relativePath", path } });
                        var saveResult = await httpClient.PostAsync(new Uri(baseUri + $"lp/1.44/{orgUnitId}/managefiles/file/save"), form);
                        if (saveResult.IsSuccessStatusCode)
                        {
                            var uploadedFileStr = await saveResult.Content.ReadAsStringAsync();
                            var uploadedFile = System.Text.Json.JsonSerializer.Deserialize<UploadedFile>(uploadedFileStr);
                            return uploadedFile;
                        }
                        else
                            return default;
                    }
                    else
                        return default;

                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

       



        public Task<List<Quiz>> GetQuizzesAsync(string orgUnitId)
        {         
            var items = GetObjectListAsync<Quiz>($"{orgUnitId}/quizzes/", "le", "1.41");
            return items;
        }


        public Task<bool> UpdateQuizAsync(string orgUnitId, QuizUpdate quizUpdate)
        {
            return PutAsync($"{orgUnitId}/quizzes/{quizUpdate.QuizId}", "le", "1.41", quizUpdate);
        }

       


       


        public async Task<List<EntityDropbox>> GetSubmissionsForDropboxAsync(int orgUnitId, int folderId)
        {

            var items = await GetAsync<List<EntityDropbox>>($"{orgUnitId}/dropbox/folders/{folderId}/submissions/", "le", "1.41");
            return items;
        }

        public async Task<bool> MarkSubmittedFileAsReadAsync(int orgUnitId, int folderId, int submissionId, int fileId)
        {
            var success = await PostAsync($"{orgUnitId}/dropbox/folders/{folderId}/submissions/{submissionId}/files/{fileId}/markAsRead", "le", "1.41", new FormUrlEncodedContent(new Dictionary<string, string>()));
            return success;
        }

        public async Task<string> GetFileForSubmissionAsync(int orgUnitId, int folderId, int submissionId, int fileId, string extension, CancellationToken ctoken, IProgress<ProgressInfo> progress)
        {
            var file = await GetStreamAsStringAsync($"{orgUnitId}/dropbox/folders/{folderId}/submissions/{submissionId}/files/{fileId}", "le", "1.41", ctoken, extension, progress);
            return file;
        }


        public async Task<bool> PostFeedbackAsync(int orgUnitId, int folderId, string entityType, int entityId, DropboxFeedback feedback, CancellationToken ctoken, IProgress<ProgressInfo> progress)
        {
            var success = await PostWithUploadAsync($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}", "le", "1.41", feedback, ctoken, progress);
            return success;
        }

        public async Task<bool> UploadFeedbackFileAsync(int orgUnitId, int folderId, string entityType, int entityId, string fileName, string localFilePath, CancellationToken ctoken, IProgress<ProgressInfo> progress)
        {
            var id = await PostStreamAsync($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}/upload", "le", "1.41", fileName, localFilePath, ctoken, progress);

            var dic = new Dictionary<string, string>();
            dic.Add("fileKey", id);
            dic.Add("fileName", fileName);
            var urlForm = new FormUrlEncodedContent(dic);
            var success = await PostAsync($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}/attach", "le", "1.41", urlForm);
            return success;
        }

        public async Task<bool> DeleteFeedbackFileAsync(int orgUnitId, int folderId, string entityType, int entityId, int fileId)
        {
            var success = await DeleteAsync($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}/attachments/{fileId}", "le", "1.41");
            return success;
        }

        public async Task<string> GetFeedbackFileAsync(int orgUnitId, int folderId, string entityType, int entityId, int fileId, string extension, CancellationToken ctoken, IProgress<ProgressInfo> progress = null)
        {
            var filePath = await GetStreamAsStringAsync($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}/attachments/{fileId}", "le", "1.41", ctoken, extension, progress);
            return filePath;
        }

        public async Task<DropboxFeedbackOut> GetFeedbackEntryAsync(int orgUnitId, int folderId, string entityType, int entityId)
        {

            var feedback = await GetAsync<DropboxFeedbackOut>($"{orgUnitId}/dropbox/folders/{folderId}/feedback/{entityType}/{entityId}", "le", "1.41");

            return feedback;
        }

        private async Task<List<T>> GetPagedItemsAsync<T>(string path, string product, string version)
        {
            var list = new List<T>();
            bool hasMoreItems = true;
            string bookmark = null;
            while (hasMoreItems)
            {
                if (bookmark == null)
                {

                    var response = await GetAsync<PagedResultSet<T>>(path, product, version);
                    list.AddRange(response.Items);
                    hasMoreItems = response.PagingInfo.HasMoreItems;
                    bookmark = response.PagingInfo.Bookmark;
                }
                else
                {
                    var moddedPath = path.Contains("?") ? path + $"&bookmark={bookmark}" : path + $"?bookmark={bookmark}";

                    var response = await GetAsync<PagedResultSet<T>>(moddedPath, product, version);
                    list.AddRange(response.Items);
                    hasMoreItems = response.PagingInfo.HasMoreItems;
                    bookmark = response.PagingInfo.Bookmark;
                }
            }
            return list;
        }

        private async Task<List<T>> GetObjectListAsync<T>(string path, string product, string version)
        {
            var list = new List<T>();
            bool hasMoreItems = true;
            string next = null;
            while (hasMoreItems)
            {
                if (next == null)
                {
                    var response = await GetAsync<ObjectListPage<T>>(path, product, version);
                    list.AddRange(response.Objects);
                    hasMoreItems = response.Next != null;
                    next = response.Next;
                }
                else
                {
                    var response = await GetAsync<ObjectListPage<T>>(next);
                    list.AddRange(response.Objects);
                    hasMoreItems = response.Next != null;
                    next = response.Next;
                }
            }
            return list;
        }

        private async Task<T> GetAsync<T>(string path, string product=null, string version=null, Func<string, T> customDeserializer = null)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                var fullPath = baseUri;  
                if (product != null && version != null)
                {
                    fullPath += $"{product}/{version}/" + path; 
                }
                else
                {
                    //assume this is from a "next"
                    fullPath = host + path;
                }
                
                var result = await httpClient.GetAsync(new Uri(fullPath));
                if (result.IsSuccessStatusCode)
                {
                    var stringresult = await result.Content.ReadAsStringAsync();
                    if (customDeserializer != null)
                    {
                        var response = customDeserializer(stringresult);
                        return response;
                    }
                    else
                    {
                        var response = System.Text.Json.JsonSerializer.Deserialize<T>(stringresult);
                        return response;
                    }
                }
                else
                {
                    var stringresult = await result.Content.ReadAsStringAsync();
                    return default;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        private async Task<Stream?> GetStreamAsync(string path, string product, string version)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                var result = await httpClient.GetAsync(new Uri(baseUri + $"{product}/{version}/" + path));
                if (result.IsSuccessStatusCode)
                {
                    var stream = await result.Content.ReadAsStreamAsync();
                    return stream;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<bool> DeleteAsync(string path, string product, string version)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                var result = await httpClient.DeleteAsync(new Uri(baseUri + $"{product}/{version}/" + path));
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Task<string> DownloadAsync(HttpClient client, string url, IProgress<ProgressInfo> progress, CancellationToken token, string extension = ".tmp")
        {
            return Task.Run<string>(async () =>
            {
                string resultString = "";
                var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                var canReportProgress = total != -1 && progress != null;
                var filePath = Path.Combine(FileSystem.Current.CacheDirectory, Guid.NewGuid().ToString() + extension);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileWriteStream = System.IO.File.OpenWrite(filePath))
                {
                    var totalRead = 0L;
                    var buffer = new byte[4096];
                    var isMoreToRead = true;

                    do
                    {
                        token.ThrowIfCancellationRequested();

                        var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                        if (read == 0)
                        {
                            isMoreToRead = false;
                        }
                        else
                        {
                            var data = new byte[read];
                            buffer.ToList().CopyTo(0, data, 0, read);

                            await fileWriteStream.WriteAsync(data, 0, data.Length);
                            totalRead += read;

                            if (canReportProgress)
                            {
                                progress.Report(new ProgressInfo(totalRead, total, ProgressStatus.Running));
                            }
                        }
                    } while (isMoreToRead);

                    progress.Report(new ProgressInfo(totalRead, total, ProgressStatus.Completed));
                }

                return filePath;
            });
        }

        private async Task<string> GetStreamAsStringAsync(string path, string product, string version, CancellationToken ctoken, string extension = ".tmp", IProgress<ProgressInfo> progress = null)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                var result = await DownloadAsync(httpClient, baseUri + $"{product}/{version}/" + path, progress, ctoken, extension);
                return result;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var data = ex.Data;
                return null;
            }
        }

        private async Task<string> PostStreamAsync(string path, string product, string version, string filename, string localFilePath, CancellationToken ctoken, IProgress<ProgressInfo> progress)
        {
            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AllowAutoRedirect = false;
                var httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Type", "application/pdf");
                var fileInfo = new FileInfo(localFilePath);

                httpClient.DefaultRequestHeaders.Add("X-Upload-Content-Length", fileInfo.Length.ToString());
                httpClient.DefaultRequestHeaders.Add("X-Upload-File-Name", filename);

                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), null);
                if ((int)result.StatusCode == 308)
                {

                    var location = result.Headers.Location.ToString();
                    var id = location.Split('/').Last();

                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Type");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-Content-Length");
                    httpClient.DefaultRequestHeaders.Remove("X-Upload-File-Name");
                    using (var stream = fileInfo.OpenRead())
                    {
                        var success = await UploadAsync(httpClient, location, stream, progress, ctoken);
                        if (success)
                        {
                            return id;
                        }
                        else
                            return default;
                    }
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                return default;
            }
        }

        private async Task<bool> UploadAsync(HttpClient client, string url, Stream stream, IProgress<ProgressInfo>? progress, CancellationToken token)
        {

            bool success = false;
            try
            {
                var requestMsg = new HttpRequestMessage(HttpMethod.Post, url);

                requestMsg.Content = new StreamContent(stream);
                var response = await client.SendAsync(requestMsg, HttpCompletionOption.ResponseHeadersRead, token);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                var total = stream.Length;
                var canReportProgress = total != -1 && progress != null;


                var totalRead = 0L;

                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = stream.Position;     

                    if (read == stream.Length)
                    {
                        isMoreToRead = false;
                    }

                    totalRead = read;

                    if (canReportProgress)
                    {
                        progress?.Report(new ProgressInfo(totalRead, total, ProgressStatus.Running));
                    }
                } while (isMoreToRead);

                progress?.Report(new ProgressInfo(totalRead, total, ProgressStatus.Completed));

                return true;
            }
            catch (Exception ex)
            {
                progress?.Report(new ProgressInfo(0, 0, ProgressStatus.Error));
                return false;
            }
        }

        private async Task<bool> PostAsync(string path, string product, string version, FormUrlEncodedContent formUrlEncodedContent)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), formUrlEncodedContent);
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<T?> PostAsync<T>(string path, string product, string version, T send)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                var str = System.Text.Json.JsonSerializer.Serialize(send);
                var stringContent = new StringContent(str);

                
                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), stringContent);

                if (result.IsSuccessStatusCode)
                {
                    var text = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(text);
                }
                else
                {
                    return default(T);
                }
                
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        private async Task<T2?> PostAsync<T1,T2>(string path, string product, string version, T1 send)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
                JsonSerializerOptions options = new()
                {
                    //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                //var a = JsonSerializerDefaults.Web;

                var str = System.Text.Json.JsonSerializer.Serialize(send, options);
                var d = JsonContent.Create(send, options:options);
                var test = await d.ReadAsStringAsync();
                var stringContent = new StringContent(str, Encoding.UTF8, "application/json");

                
                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), d);

                if (result.IsSuccessStatusCode)
                {
                    var text = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T2>(text);
                }
                else
                {
                    return default(T2);
                }
                
            }
            catch (Exception ex)
            {
                return default(T2);
            }
        }

        private async Task<T2?> PostWithSimpleUploadAsync<T1,T2>(string path, string product, string version, T1 json, string fileName, string mimeType, Stream upload)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
                //JsonSerializerOptions options = new()
                //{
                //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                //};
           
                var jsonContent = JsonContent.Create(json);
                //var test = await jsonContent.ReadAsStringAsync();

                var binaryContent = new System.Net.Http.StreamContent(upload);
                binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                binaryContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "",
                    FileName = fileName
                };

                var multipart = new MultipartContent("mixed");
                multipart.Add(jsonContent);
                multipart.Add(binaryContent);


                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), multipart);

                if (result.IsSuccessStatusCode)
                {
                    var text = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T2>(text);
                }
                else
                {
                    return default(T2);
                }
            }
            catch (Exception ex)
            {
                return default(T2);
            }
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private async Task<bool> PostWithUploadAsync<T>(string path, string product, string version, T jsonContent, CancellationToken ctoken, IProgress<ProgressInfo> progress = null)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

                var str = System.Text.Json.JsonSerializer.Serialize(jsonContent);
                var stringContent = new StringContent(str);



                using (var stream = GenerateStreamFromString(str))
                {
                    var result = await UploadAsync(httpClient, baseUri + $"{product}/{version}/" + path, stream, progress, ctoken);

                    if (result)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<T> GetStringAsync<T>(string path)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
            var result = await httpClient.GetStringAsync(new Uri(path));
            var response = System.Text.Json.JsonSerializer.Deserialize<T>(result);
            return response;
        }

        private async Task<bool> PutAsync<T>(string path, string product, string version, T content)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    IgnoreReadOnlyProperties = true
                };
                var stringContent = System.Text.Json.JsonSerializer.Serialize(content, options);
                var result = await httpClient.PutAsync(new Uri(baseUri + $"{product}/{version}/" + path), new StringContent(stringContent, Encoding.UTF8, "application/json"));
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<T2?> PutAsync<T1,T2>(string path, string product, string version, T1 content)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    IgnoreReadOnlyProperties = true
                };
                var stringContent = System.Text.Json.JsonSerializer.Serialize(content, options);
                var result = await httpClient.PutAsync(new Uri(baseUri + $"{product}/{version}/" + path), new StringContent(stringContent, Encoding.UTF8, "application/json"));
                if (result.IsSuccessStatusCode)
                {
                    var item = await result.Content.ReadAsStringAsync();
                    var model = System.Text.Json.JsonSerializer.Deserialize<T2>(item);
                    return model;
                }
                else
                {
                    return default;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }

        private async Task<T2?> PutWithSimpleUploadAsync<T1, T2>(string path, string product, string version, T1 json, string fileName, string mimeType, Stream upload)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("utf-8"));
                //JsonSerializerOptions options = new()
                //{
                //    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                //};

                var jsonContent = JsonContent.Create(json);
                //var test = await jsonContent.ReadAsStringAsync();

                var binaryContent = new System.Net.Http.StreamContent(upload);
                binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                binaryContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "",
                    FileName = fileName
                };

                var multipart = new MultipartContent("mixed");
                multipart.Add(jsonContent);
                multipart.Add(binaryContent);


                var result = await httpClient.PostAsync(new Uri(baseUri + $"{product}/{version}/" + path), multipart);

                if (result.IsSuccessStatusCode)
                {
                    var text = await result.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T2>(text);
                }
                else
                {
                    return default(T2);
                }
            }
            catch (Exception ex)
            {
                return default(T2);
            }
        }

    }
}
