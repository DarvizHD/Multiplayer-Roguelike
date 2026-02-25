namespace Backend.ModelCollections
{
    public abstract class DescribedModelCollection<T> : SerializeModelCollection<T>
        where T : ISerializable
    {
        public virtual T Create(string descriptionKey)
        {
            DescriptionKey = descriptionKey;
            var model = CreateModel(descriptionKey);
            Add(GetCurrentKey(), model);
            return model;
        }

        protected abstract T CreateModel(string descriptionKey);
    }
}
