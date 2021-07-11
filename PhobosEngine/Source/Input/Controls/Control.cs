using System;

namespace PhobosEngine.Input
{
    public abstract class Control
    {
        public string Name {get; private set;}
        public abstract void Update();
    }
}