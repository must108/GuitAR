namespace Core3lb
{
    public class OutwardActivator : BaseActivator
    {
        [CoreHeader("Also Run These")]
        [CoreEmphasize]
        public InwardActivator inwardActivator;
        public InwardActivator[] extraActivators;

        public void DealWithInWards(ActivatorEvents whichEvent)
        {
            if(inwardActivator != null)
            {
                UnityEngine.Debug.LogError(whichEvent.ToString(), inwardActivator);
                inwardActivator.CallEvent(whichEvent);
            }
            foreach(var activator in extraActivators)
            {
                
                activator.CallEvent(whichEvent);
            }
        }

        public override void CallEvent(ActivatorEvents whichEvent, bool bypassOverride = false)
        {
            base.CallEvent(whichEvent, bypassOverride);
            DealWithInWards(whichEvent);
        }

    }
}
