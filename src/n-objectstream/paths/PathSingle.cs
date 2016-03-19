using System.Collections.Generic;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
    /// A list of paths
    public class PathSingle : IPathGroup
    {
        private IAnimationPath path;

        public PathSingle(IAnimationPath path)
        { this.path = path; }

        /// Yield the set of IAnimationPath to apply to an object.
        public IEnumerable<IAnimationPath> Path
        { get { yield return path; } }
    }
}
