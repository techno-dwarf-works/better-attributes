namespace Better.Attributes.EditorAddons.Drawers.Validation.Handlers
{
    public class ValidationValue<T>
    {
        public bool State { get; set; }

        public T Result { get; set; }

        public void Set(bool state, T result)
        {
            State = state;
            Result = result;
        }
    }
}