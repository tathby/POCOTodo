namespace POCOTodoCross
{
    public interface IAssignable
    {
        bool isAssigned { get; set; }
        void ToggleCompleted();
    }

}