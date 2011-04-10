using System.Collections.Generic;

namespace ChainableComparer
{
    public abstract class ChainableComparer<T> : IComparer<T>
    {
        private ChainableComparer<T> Successor { get; set; }

        private IComparer<T> DecisionMaker { get; set; }

        public int Compare(T obj1, T obj2)
        {
            if (CanHandle(obj1, obj2) && DecisionMaker != null)
                return DecisionMaker.Compare(obj1, obj2);

            if (Successor == null)
                return 0;

            return Successor.Compare(obj1, obj2);
        }

        protected virtual bool CanHandle(T obj1, T obj2)
        {
            return false;
        }

        public ChainableComparer<T> If<TNext>() where TNext : ChainableComparer<T>, new()
        {
            Successor = new TNext();

            return Successor;
        }

        public ChainableComparer<T> Then<TDecisionMaker>()
            where TDecisionMaker : IComparer<T>, new()
        {
            DecisionMaker = new TDecisionMaker();

            return this;
        }
    }
}