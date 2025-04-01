namespace D2LOffice.Models
{
    public class DialogList<T>
    {
        public List<T> Items { get; set; }
        public T SelectedItem { get; set; }
    }
}