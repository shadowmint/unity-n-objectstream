using System.Collections.Generic;
using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
  /// A list of paths
  public class PathList : IPathGroup
  {
    private readonly List<IAnimationPath> _pathGroup = new List<IAnimationPath>();

    /// Add to the list
    public void Add(IAnimationPath path)
    {
      _pathGroup.Add(path);
    }

    /// Yield the set of IAnimationPath to apply to an object.
    public IEnumerable<IAnimationPath> Path
    {
      get { return _pathGroup; }
    }
  }
}