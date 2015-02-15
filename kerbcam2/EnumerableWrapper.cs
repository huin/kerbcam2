using System.Collections.Generic;

namespace kerbcam2 {
    /// <summary>
    /// Class to encapsulate a container so that only GetEnumerator can be called.
    /// (insert rant about stupid enumerator [iterator!] design in C#)
    /// </summary>
    /// <typeparam name="T">The item type yielded by the enumerator</typeparam>
    struct EnumerableWrapper<T> {
        private IEnumerable<T> container;

        public EnumerableWrapper(IEnumerable<T> c) {
            this.container = c;
        }

        public IEnumerator<T> GetEnumerator() {
            return container.GetEnumerator();
        }
    }
}
