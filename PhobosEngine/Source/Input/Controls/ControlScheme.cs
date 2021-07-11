using System.Collections.Generic;

namespace PhobosEngine.Input
{
    // Implementations of this class should have actual Controls defined internally, and add them to the List
    // Only controls that are added to the List are updated/send events!
    public abstract class ControlScheme
    {
        protected List<Control> Controls {get; private set;}
        public void Update()
        {
            foreach(Control control in Controls)
            {
                control.Update();
            }
        }

        protected void Register(Control control)
        {
            Controls.Add(control);
        }
    }
}