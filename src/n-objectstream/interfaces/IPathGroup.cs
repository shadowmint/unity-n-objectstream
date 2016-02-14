using System.Collections.Generic;
using UnityEngine;
using N;

namespace N.Package.ObjectStream
{
    /// An object stream returns a series of IAnimationPath objects to apply
    /// to a set of objects.
    public interface IPathGroup
    {
        /// Yield the set of IAnimationPath to apply to an object.
        IEnumerable<IAnimationPath> Path { get; }
    }
}
