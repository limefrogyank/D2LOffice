#nullable enable
using D2LOffice.Models.D2L;



namespace D2LOffice.Service
{
    public partial class D2LService
    {
        // Dropboxes

        public async Task<List<DropboxFolder>> GetDropboxFoldersAsync(string orgUnitId)
        {
            var items = await GetAsync<List<DropboxFolder>>($"{orgUnitId}/dropbox/folders/", "le", "1.67");
            return items;
        }

        public async Task<DropboxFolder?> CreateDropboxFolderAsync(string orgUnitId, DropboxFolderUpdateData dropboxFolderUpdateData)
        {
            var folder = await PostAsync<DropboxFolderUpdateData, DropboxFolder>($"{orgUnitId}/dropbox/folders/", "le", "1.67", dropboxFolderUpdateData);
            return folder;
        }

        public async Task<DropboxFolder?> CreateDropboxFolderWithAttachmentsAsync(string orgUnitId, DropboxFolderUpdateData dropboxFolderUpdateData, List<Stream> attachments)
        {
            var folder = await PostAsync<DropboxFolderUpdateData, DropboxFolder>($"{orgUnitId}/dropbox/folders/", "le", "1.67", dropboxFolderUpdateData);
            return folder;
        }

        public async Task<DropboxFolder?> UpdateDropboxFolderAsync(string orgUnitId, string folderId, DropboxFolderUpdateData dropboxFolderUpdateData)
        {
            var folder = await PutAsync<DropboxFolderUpdateData, DropboxFolder>($"{orgUnitId}/dropbox/folders/{folderId}", "le", "1.67", dropboxFolderUpdateData);
            return folder;
        }

        public async Task<DropboxFolder> GetDropboxFolderAsync(string orgUnitId, string folderId)
        {
            var folder = await GetAsync<DropboxFolder>($"{orgUnitId}/dropbox/folders/{folderId}", "le", "1.67");
            return folder;
        }

        public async Task<bool> DeleteDropboxFolderAsync(string orgUnitId, string folderId)
        {
            var success = await DeleteAsync($"{orgUnitId}/dropbox/folders/{folderId}", "le", "1.73");
            return success;
        }

        // Attachments
        // attachments can be added using basic file upload as multipart/mixed but NOT IMPLEMENTED here as we can just link files
        // that are from onedrive/googledrive or uploaded to the course content already.
        // https://docs.valence.desire2learn.com/basic/fileupload.html
        public async Task<Stream> GetDropboxFolderAttachmentAsync(string orgUnitId, string folderId, string fileId)
        {
            var stream = await GetStreamAsync($"{orgUnitId}/dropbox/folders/{folderId}/attachments/{fileId}", "le", "1.67");
            return stream;
        }

        // Categories

        public async Task<List<DropboxCategory>> GetAllDropboxCategoriesAsync(string orgUnitId)
        {
            var items = await GetAsync<List<DropboxCategory>>($"{orgUnitId}/dropbox/categories/", "le", "1.67");
            return items;
        }

        public async Task<DropboxCategoryWithFolders> GetDropboxCategoryAsync(string orgUnitId, string categoryId)
        {
            var item = await GetAsync<DropboxCategoryWithFolders>($"{orgUnitId}/dropbox/categories/{categoryId}", "le", "1.67");
            return item;
        }

        public async Task<DropboxCategory?> CreateDropboxCategoryAsync(string orgUnitId, DropboxCategory dropboxCategory)
        {
            var item = await PostAsync<DropboxCategory>($"{orgUnitId}/dropbox/categories/", "le", "1.67", dropboxCategory);
            return item;
        }

        public async Task<DropboxCategory?> UpdateDropboxCategoryAsync(string orgUnitId, string categoryId, DropboxCategory dropboxCategory)
        {
            var item = await PutAsync<DropboxCategory, DropboxCategory>($"{orgUnitId}/dropbox/categories/{categoryId}", "le", "1.67", dropboxCategory);
            return item;
        }

        public async Task<bool> DeleteDropboxCategoryAsync(string orgUnitId, string categoryId)
        {
            var success = await DeleteAsync($"{orgUnitId}/dropbox/categories/{categoryId}", "le", "1.67");
            return success;
        }

    }
}