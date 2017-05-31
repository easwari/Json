namespace JSONUtil
{
    public interface IJSONContainer
    {
        string ToJsonText();
        void AddChild(object value);

        bool IsComplete();
        void SetAsComplete();
    }
}
