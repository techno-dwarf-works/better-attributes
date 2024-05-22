namespace Better.Commons.EditorAddons.Drawers.Caching
{
    public class CacheValue<T>
    {
        public bool IsValid { get; protected set; }
        public T Value { get; protected set; }

        public void Set(bool isValid, T value)
        {
            IsValid = isValid;
            Value = value;
        }

        public CacheValue<T> Copy()
        {
            return new CacheValue<T>()
            {
                IsValid = IsValid,
                Value = Value
            };
        }
    }
}