namespace Core3lb
{
    //For combining two inputs
    public class InputXRDoubleEvent : BaseInputXREvent
    {
        public BaseInputXREvent button1;
        public BaseInputXREvent button2;
        public bool OrNotAnd = false;
        // Start is called before the first frame update

        public override bool GetInput()
        {
            if(OrNotAnd)
            {
                return button1.GetInput() || button2.GetInput();
            }
            return button1.GetInput() && button2.GetInput();
        }
    }
}
