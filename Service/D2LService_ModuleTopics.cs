#nullable enable
using D2LOffice.Models;
using D2LOffice.Models.D2L;



namespace D2LOffice.Service
{
    public partial class D2LService
    {
        public async Task<List<ContentObject>> GetAllModulesAndTopicsAsync(int orgUnitId)
        {
            var modulesAndTopics = await GetRootModulesAsync(orgUnitId.ToString());
            foreach (var item in modulesAndTopics)
            {
                await GetAllModulesAndTopicsAsyncRecursive(orgUnitId.ToString(), item);
            }
            return modulesAndTopics;

        }

        private async Task GetAllModulesAndTopicsAsyncRecursive(string orgUnitId, ContentObject contentObject)
        {
            if (contentObject.Type == ContentObjectType.Module)
            {
                var module = (Models.D2L.Module)contentObject;
                module.Structure = await GetStructureForModuleAsync(orgUnitId, module.Id.ToString());
                foreach (var item in module.Structure)
                {
                    await GetAllModulesAndTopicsAsyncRecursive(orgUnitId, item);
                }
            }
            else
            {
                var topic = (Topic)contentObject;
                var contents = await GetTopicContentsAsync(orgUnitId, topic.Id.ToString());
                topic = contents;
            }

        }

        public async Task<List<ContentObject>> GetStructureForModuleAsync(string orgUnitId, string moduleId)
        {

            var items = await GetAsync<List<ContentObject>>($"{orgUnitId}/content/modules/{moduleId}/structure/", "le", "1.67");
            return items;
        }

        public async Task<Topic> GetTopicContentsAsync(string orgUnitId, string topicId)
        {
            var item = await GetAsync<Topic>($"{orgUnitId}/content/topics/{topicId}", "le", "1.67");
            return item;
        }

        public async Task<List<ContentObject>> GetRootModulesAsync(string orgUnitId)
        {

            var items = await GetAsync<List<ContentObject>>($"{orgUnitId}/content/root/", "le", "1.67");
            return items;
        }

        public async Task<Module?> CreateModuleInRootAsync(string orgUnitId, ModuleData moduleData)
        {
            
            var item = await PostAsync<ModuleData,Module>($"{orgUnitId}/content/root/", "le", "1.67", moduleData);
            return item;
        }

        public async Task<ContentObject?> CreateTopicInModuleAsync(string orgUnitId, string moduleId, ContentObjectData contentObjectData)
        {                    
            var item = await PostAsync<ContentObjectData, ContentObject>($"{orgUnitId}/content/modules/{moduleId}/structure/?base64=false", "le", "1.67", contentObjectData);
            return item;
        }

        public async Task<Topic?> CreateTopicInModuleWithFileAsync(string orgUnitId, string moduleId, TopicData topicData, string fileName, string mimeType, Stream stream)
        {
            var item = await PostWithSimpleUploadAsync<TopicData, Topic>($"{orgUnitId}/content/modules/{moduleId}/structure/", "le", "1.67", topicData, fileName, mimeType, stream);
            return item;
        }



        public async Task<ContentObject?> UpdateModuleAsync(string orgUnitId, string moduleId, ContentObjectData contentObjectData)
        {
            var item = await PutAsync<ContentObjectData,ContentObject>($"{orgUnitId}/content/modules/{moduleId}", "le", "1.67", contentObjectData);
            return item;
        }

        public async Task<ContentObject?> UpdateTopicAsync(string orgUnitId, string topicId, ContentObjectData contentObjectData)
        {
            var item = await PutAsync<ContentObjectData, ContentObject>($"{orgUnitId}/content/topics/{topicId}", "le", "1.67", contentObjectData);
            return item;
        }

     

        //public async Task<ContentObject?> ReplaceTopicFileAsync(int orgUnitId, string topicId, ContentObjectData contentObjectData)
        //{
        //    var item = await PutAsync<ContentObjectData, ContentObject>($"{orgUnitId}/content/topics/{topicId}", "le", "1.67", contentObjectData);
        //    return item;
        //}


        public async Task<bool> DeleteModuleAsync(string orgUnitId, string moduleId)
        {            
            var success = await DeleteAsync($"{orgUnitId}/content/modules/{moduleId}", "le", "1.67");
            return success;
        }

        public async Task<bool> DeleteTopicAsync(string orgUnitId, string topicId)
        {
            var success = await DeleteAsync($"{orgUnitId}/content/topics/{topicId}", "le", "1.67");
            return success;
        }

    }
}
