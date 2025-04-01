#nullable enable
using D2LOffice.Models.D2L;



namespace D2LOffice.Service
{
    public partial class D2LService
    {
        public Task<List<ChecklistReadData>> GetAllChecklistsAsync(long orgUnitId)
        {
            var checklists = GetObjectListAsync<ChecklistReadData>($"{orgUnitId}/checklists/", "le", "1.67");
            return checklists;
        }

        public Task<List<ChecklistCategoryReadData>> GetAllCategoriesForChecklistAsync(long orgUnitId, long checklistId)
        {
            var categories = GetObjectListAsync<ChecklistCategoryReadData>($"{orgUnitId}/checklists/{checklistId}/categories/", "le", "1.67");
            return categories;
        }

        public Task<List<ChecklistItemReadData>> GetAllItemsFromChecklistAsync(long orgUnitId, long checklistId)
        {
            var items = GetObjectListAsync<ChecklistItemReadData>($"{orgUnitId}/checklists/{checklistId}/items/", "le", "1.67");
            return items;
        }

        public Task<ChecklistReadData?> AddChecklistAsync(long orgUnitId, ChecklistUpdateData checklist)
        {
            var checklistWithId = PostAsync<ChecklistUpdateData, ChecklistReadData>($"{orgUnitId}/checklists/", "le", "1.67", checklist);
            return checklistWithId;
        }

        public Task<ChecklistCategoryReadData?> AddCategoryToChecklistAsync(long orgUnitId, long checklistId, ChecklistCategoryUpdateData category)
        {
            var categoryData = PostAsync<ChecklistCategoryUpdateData, ChecklistCategoryReadData>($"{orgUnitId}/checklists/{checklistId}/categories/", "le", "1.67", category);
            return categoryData;
        }

        public Task<ChecklistItemReadData?> AddItemToChecklistAsync(long orgUnitId, long checklistId, ChecklistItemUpdateData item)
        {
            var itemData = PostAsync<ChecklistItemUpdateData, ChecklistItemReadData>($"{orgUnitId}/checklists/{checklistId}/items/", "le", "1.67", item);
            return itemData;
        }

        public Task<ChecklistReadData?> UpdateChecklistAsync(long orgUnitId, ChecklistUpdateData checklist)
        {
            var checklistWithId = PutAsync<ChecklistUpdateData, ChecklistReadData>($"{orgUnitId}/checklists/", "le", "1.67", checklist);
            return checklistWithId;
        }

        public Task<ChecklistCategoryReadData?> UpdateCategoryOnChecklistAsync(long orgUnitId, long checklistId, long categoryId, ChecklistCategoryUpdateData category)
        {
            var categoryData = PutAsync<ChecklistCategoryUpdateData, ChecklistCategoryReadData>($"{orgUnitId}/checklists/{checklistId}/categories/{categoryId}", "le", "1.67", category);
            return categoryData;
        }

        public Task<ChecklistItemReadData?> UpdateItemFromChecklistAsync(long orgUnitId, long checklistId, long itemId, ChecklistItemUpdateData item)
        {
            var itemData = PutAsync<ChecklistItemUpdateData, ChecklistItemReadData>($"{orgUnitId}/checklists/{checklistId}/items/{itemId}", "le", "1.67", item);
            return itemData;
        }

        public Task<bool> DeleteChecklistAsync(long orgUnitId, long checklistId)
        {
            var result = DeleteAsync($"{orgUnitId}/checklists/{checklistId}", "le", "1.67");
            return result;
        }

        public Task<bool> DeleteCategoryOnChecklistAsync(long orgUnitId, long checklistId, long categoryId)
        {
            var result = DeleteAsync($"{orgUnitId}/checklists/{checklistId}/categories/{categoryId}", "le", "1.67");
            return result;
        }

        public Task<bool> DeleteItemFromChecklistAsync(long orgUnitId, long checklistId, long itemId)
        {
            var result = DeleteAsync($"{orgUnitId}/checklists/{checklistId}/items/{itemId}", "le", "1.67");
            return result;
        }


    }
}